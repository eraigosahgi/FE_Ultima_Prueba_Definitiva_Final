using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Procesos;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.ControllerSql;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Error;
using LibreriaGlobalHGInet.Formato;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Mail;
using LibreriaGlobalHGInet.Objetos;
using LibreriaGlobalHGInet.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Data.Entity.SqlServer;
using HGInetMiFacturaElectonicaController.Auditorias;
using static HGInetMiFacturaElectonicaController.Configuracion.Ctl_PlanesTransacciones;
using HGInetMiFacturaElectonicaData.ModeloAuditoria.Objetos;
using LibreriaGlobalHGInet.ObjetosComunes.Mensajeria;
using LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail;
using LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.Respuesta;
using Newtonsoft.Json;
using static LibreriaGlobalHGInet.Funciones.Fecha;
using Guid = System.Guid;
using LibreriaGlobalHGInet.RegistroLog;
using LibreriaGlobalHGInet.HgiNet.Controladores;
using HGInetMiFacturaElectonicaData.Enumerables;

namespace HGInetMiFacturaElectonicaController.Registros
{
	public partial class Ctl_Documento : BaseObject<TblDocumentos>
	{
		#region Constructores 

		public Ctl_Documento() : base(new ModeloAutenticacion()) { }

		#endregion

		/// <summary>
		/// Crea un documento en la Base de Datos
		/// </summary>
		/// <param name="documento"></param>
		/// <returns></returns>
		public TblDocumentos Crear(TblDocumentos documento)
		{
			documento = this.Add(documento);

			return documento;
		}




		public TblDocumentos Actualizar(TblDocumentos documento)
		{

			Ctl_Documento categoria_doc = new Ctl_Documento();
			documento.IdCategoriaEstado = Ctl_Documento.ObtenerCategoria(documento.IntIdEstado);
			documento = this.Edit(documento);

			return documento;

		}


		/// <summary>
		/// Obtiene la Categoria segun el estado en que este el documento
		/// </summary>
		/// <param name="estado">codigo del estado del documento</param>
		/// <returns></returns>
		public static int ObtenerCategoria(int estado)
		{

			int categoria = 0;

			switch (estado)
			{
				//case 4:
				case 5:
				case 6:
					categoria = CategoriaEstado.Recibido.GetHashCode();
					break;
				case 7:
					categoria = CategoriaEstado.EnvioDian.GetHashCode();
					break;
				case 8:
				case 9:
				case 10:
				case 11:
				case 12:
				case 13:
				case 14:
				case 15:
				case 16:
				case 99:
					categoria = CategoriaEstado.ValidadoDian.GetHashCode();
					break;
				case 90:
					categoria = CategoriaEstado.FallidoDian.GetHashCode();
					break;
				default:
					categoria = CategoriaEstado.NoRecibido.GetHashCode();
					break;
			}

			return categoria;
		}




		#region Obtener

		public TblDocumentos Obtener(string identificacion_obligado, long numero_documeto, string prefijo)
		{
			try
			{
				TblDocumentos documento = new TblDocumentos();


				documento = (from documentos in context.TblDocumentos
							 where (documentos.IntNumero == numero_documeto)
							 && (documentos.TblEmpresasFacturador.StrIdentificacion.Equals(identificacion_obligado)
							 && documentos.TblEmpresasResoluciones.StrPrefijo.Equals(prefijo))
							 select documentos).FirstOrDefault();



				return documento;


			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		/// <summary>
		/// Obtiene los documentos por número
		/// </summary>
		/// <param name="DataKey">Clave compuesta (serial + identificación obligado ) en formato Sha1</param>
		/// <param name="Identificacion">identificación obligado</param>
		/// <param name="TipoDocumento">tipo documento 1: factura - 2: nota débito - 3: nota crédito</param>
		/// <param name="Numeros">número de los documentos (recibe varios números separados por coma)</param>
		/// <returns></returns>
		public List<DocumentoRespuesta> ConsultaPorNumeros(string identificacion_obligado, int tipo_documento, string Numeros)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(identificacion_obligado))
					throw new ApplicationException("Número de identificación del obligado inválido.");
				if (tipo_documento < 1 || tipo_documento > 3)
					throw new ApplicationException("Tipo de documento inválido.");
				if (string.IsNullOrWhiteSpace(Numeros))
					throw new ApplicationException("Filtro por números inválido.");

				List<DocumentoRespuesta> lista_respuesta = new List<DocumentoRespuesta>();

				//Convierte Numeros en una lista.
				List<string> lista_documentos = Coleccion.ConvertirLista(Numeros);

				var respuesta = from documento in context.TblDocumentos
								join empresa in context.TblEmpresas on documento.StrEmpresaFacturador equals empresa.StrIdentificacion
								where empresa.StrIdentificacion.Equals(identificacion_obligado)
								 && documento.IntDocTipo == tipo_documento
								 && lista_documentos.Contains(documento.IntNumero.ToString())
								select documento;

				//Convierte los registros de base de datos a objeto de servicio y los añade a la lista de retorno
				foreach (TblDocumentos item in respuesta)
				{


					List<System.Guid> List_id_seguridad = new List<Guid>();

					List_id_seguridad.Add(item.StrIdSeguridad);

					Ctl_Documento documento = new Ctl_Documento();

					var lista = documento.ProcesarDocumentos(List_id_seguridad);

					lista_respuesta.Add(Convertir(item));
				}

				return lista_respuesta;
			}
			catch (Exception exec)
			{
				Error error = new Error(CodigoError.VALIDACION, exec);
				throw new FaultException<Error>(error, new FaultReason(string.Format("{0}", error.Mensaje)));
			}
		}

		/// <summary>
		/// Obtiene los documentos por Código de Registros
		/// </summary>
		/// <param name="DataKey">Clave compuesta (serial + identificación obligado ) en formato Sha1</param>
		/// <param name="Identificacion">identificación obligado</param>
		/// <param name="TipoDocumento">tipo documento 1: factura - 2: nota débito - 3: nota crédito</param>
		/// <param name="CodigosRegistros">código de registro de los documentos (recibe varios códigos separados por coma)</param>
		/// <returns></returns>
		public List<DocumentoRespuesta> ConsultaPorCodigoRegistro(string identificacion_obligado, int tipo_documento, string CodigosRegistros)
		{
			try
			{
				//Valida que los parametros sean correctos.
				if (string.IsNullOrWhiteSpace(identificacion_obligado))
					throw new ApplicationException("Número de identificación del obligado inválido.");
				if (tipo_documento < 1 || tipo_documento > 3)
					throw new ApplicationException("Tipo de documento inválido.");
				if (string.IsNullOrWhiteSpace(CodigosRegistros))
					throw new ApplicationException("Filtro por números inválido.");

				List<DocumentoRespuesta> lista_respuesta = new List<DocumentoRespuesta>();

				//Convierte CodigoRegistros en una lista.
				List<string> lista_documentos = Coleccion.ConvertirLista(CodigosRegistros);

				var respuesta = from documento in context.TblDocumentos
								join empresa in context.TblEmpresas on documento.StrEmpresaFacturador equals empresa.StrIdentificacion
								where empresa.StrIdentificacion.Equals(identificacion_obligado)
								&& documento.IntDocTipo == tipo_documento
								&& (lista_documentos.Contains(documento.StrObligadoIdRegistro) || lista_documentos.Contains(documento.StrIdSeguridad.ToString()))
								select documento;

				//Convierte los registros de base de datos a objeto de servicio y los añade a la lista de retorno
				foreach (TblDocumentos item in respuesta)
				{
					lista_respuesta.Add(Convertir(item));
				}

				return lista_respuesta;
			}
			catch (Exception exec)
			{
				Error error = new Error(CodigoError.VALIDACION, exec);
				throw new FaultException<Error>(error, new FaultReason(string.Format("{0}", error.Mensaje)));
			}
		}

		/// <summary>
		/// Obtiene los documentos por rangos de fecha de elaboración.
		/// </summary>
		/// <param name="identificacion_obligado">identificación obligado</param>
		/// <param name="tipo_documento">tipo documento 1: factura - 2: nota débito - 3: nota crédito</param>
		/// <param name="FechaInicial">fecha inicial del rango de búsqueda - aplica sobre la fecha del registro</param>
		/// <param name="FechaFinal">fecha final del rango de búsqueda - aplica sobre la fecha del registro</param>
		/// <returns></returns>
		public List<DocumentoRespuesta> ConsultaPorFechaElaboracion(string identificacion_obligado, int tipo_documento, DateTime FechaInicial, DateTime FechaFinal)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(identificacion_obligado))
					throw new ApplicationException("Número de identificación del obligado inválido.");
				if (tipo_documento < 1 || tipo_documento > 3)
					throw new ApplicationException("Tipo de documento inválido.");
				if (FechaInicial == null)
					throw new ApplicationException("Fecha inicial inválida.");
				if (FechaFinal == null)
					throw new ApplicationException("Fecha final inválida.");

				FechaInicial = FechaInicial.Date;
				FechaFinal = new DateTime(FechaFinal.Year, FechaFinal.Month, FechaFinal.Day, 23, 59, 59, 999);

				List<DocumentoRespuesta> lista_respuesta = new List<DocumentoRespuesta>();

				var respuesta = from documento in context.TblDocumentos
								join empresa in context.TblEmpresas on documento.StrEmpresaFacturador equals empresa.StrIdentificacion
								where empresa.StrIdentificacion.Equals(identificacion_obligado)
								 && documento.IntDocTipo == tipo_documento
								 && (documento.DatFechaIngreso >= FechaInicial && documento.DatFechaIngreso <= FechaFinal)
								select documento;

				//Convierte los registros de base de datos a objeto de servicio y los añade a la lista de retorno
				foreach (TblDocumentos item in respuesta)
				{
					lista_respuesta.Add(Convertir(item));
				}

				return lista_respuesta;
			}
			catch (Exception exec)
			{
				Error error = new Error(CodigoError.VALIDACION, exec);
				throw new FaultException<Error>(error, new FaultReason(string.Format("{0}", error.Mensaje)));
			}
		}

		/// <summary>
		///  Obtiene la lista de documentos de la vista de adquiriente, con estados Dian publicos, excepto el error dian
		/// </summary>
		/// <param name="identificacion_adquiente">Identificación adquiriente</param>
		/// <param name="numero_documento">Numero de Documento int</param>
		/// <param name="estado_recibo">Estados de recibo, Pendiente,Aprobado, etc</param>
		/// <param name="fecha_inicio">Fecha inicio del documento en la plataforma</param>
		/// <param name="fecha_fin">Fecha inicio del documento en la plataforma</param>
		/// <param name="tipo_filtro_fecha">indica sobre que fechas se aplica la consulta (1: Recepción - 2:Documento)</param>
		/// <returns></returns>
		public List<TblDocumentos> ObtenerPorFechasAdquiriente(string identificacion_adquiente, string numero_documento, string estado_recibo, DateTime fecha_inicio, DateTime fecha_fin, int tipo_filtro_fecha)
		{


			// Valida los estados de visibilidad pública para el adquiriente
			//var estado_dian = "";
			//estado_dian = Coleccion.ConvertirString(Ctl_MaestrosEnum.ListaEnum(0, "publico"));

			fecha_inicio = fecha_inicio.Date;
			//            fecha_fin = fecha_fin.Date.AddDays(1);
			fecha_fin = new DateTime(fecha_fin.Year, fecha_fin.Month, fecha_fin.Day, 23, 59, 59, 999);

			long num_doc = -1;
			long.TryParse(numero_documento, out num_doc);

			short cod_estado_recibo = -1;
			short.TryParse(estado_recibo, out cod_estado_recibo);

			if (string.IsNullOrWhiteSpace(numero_documento))
				numero_documento = "*";
			if (string.IsNullOrWhiteSpace(estado_recibo))
				estado_recibo = "*";

			int ErrorDian = ProcesoEstado.FinalizacionErrorDian.GetHashCode();

			int Categoria = CategoriaEstado.ValidadoDian.GetHashCode();


			List<TblDocumentos> respuesta = new List<TblDocumentos>();


			if (numero_documento.Equals("*"))
			{
				respuesta = (from datos in context.TblDocumentos
							 join empresa in context.TblEmpresas on datos.StrEmpresaAdquiriente equals empresa.StrIdentificacion
							 where (empresa.StrIdentificacion.Equals(identificacion_adquiente) || identificacion_adquiente.Equals("*"))
										&& (datos.IntAdquirienteRecibo == cod_estado_recibo || estado_recibo.Equals("*"))
										&& ((datos.DatFechaIngreso >= fecha_inicio && datos.DatFechaIngreso <= fecha_fin) || tipo_filtro_fecha == 2)
										&& ((datos.DatFechaDocumento >= fecha_inicio && datos.DatFechaDocumento <= fecha_fin) || tipo_filtro_fecha == 1)
										&& (datos.TblEmpresasFacturador.IntEnvioMailRecepcion == true || (datos.TblEmpresasFacturador.IntEnvioMailRecepcion == false && datos.IdCategoriaEstado == Categoria))
										&& (datos.IdCategoriaEstado == Categoria)
							 orderby datos.IntNumero descending
							 select datos).ToList();

			
			}
			else
			{
				var listaDocumetos = Coleccion.ConvertirStringlong(numero_documento);

				respuesta = (from datos in context.TblDocumentos
							 join empresa in context.TblEmpresas on datos.StrEmpresaAdquiriente equals empresa.StrIdentificacion
							 where (empresa.StrIdentificacion.Equals(identificacion_adquiente))
							 && (listaDocumetos.Contains(datos.IntNumero))
							 && (datos.TblEmpresasFacturador.IntEnvioMailRecepcion == true || (datos.TblEmpresasFacturador.IntEnvioMailRecepcion == false && datos.IdCategoriaEstado == Categoria))
							 orderby datos.IntNumero descending
							 select datos).ToList();

			}

			return respuesta;
		}

		/// <summary>
		/// Obtiene los documentos del Facturador
		/// </summary>
		/// <param name="codigo_facturador"></param>
		/// <param name="numero_documento"></param>
		/// <param name="codigo_tercero"></param>
		/// <param name="estado_dian"></param>
		/// <param name="estado_recibo"></param>
		/// <param name="fecha_inicio"></param>
		/// <param name="fecha_fin"></param>
		/// <returns></returns>
		public List<TblDocumentos> ObtenerPorFechasObligado(string codigo_facturador, string numero_documento, string codigo_adquiriente, string estado_dian, string estado_recibo, DateTime fecha_inicio, DateTime fecha_fin, string Resolucion, int tipo_filtro_fecha)
		{
			List<string> LstEstado = null;
			if (estado_dian == null || estado_dian == "")
			{
				estado_dian = "*";
				LstEstado = Coleccion.ConvertirLista(estado_dian);
			}
			else
			{
				LstEstado = Coleccion.ConvertirLista(estado_dian);
			}

			fecha_inicio = fecha_inicio.Date;

			fecha_fin = new DateTime(fecha_fin.Year, fecha_fin.Month, fecha_fin.Day, 23, 59, 59, 999);

			long num_doc = -1;
			long.TryParse(numero_documento, out num_doc);

			short cod_estado_recibo = -1;
			short.TryParse(estado_recibo, out cod_estado_recibo);

			if (string.IsNullOrWhiteSpace(numero_documento))
				numero_documento = "*";
			if (string.IsNullOrWhiteSpace(codigo_adquiriente))
				codigo_adquiriente = "*";

			if (string.IsNullOrWhiteSpace(estado_recibo))
				estado_recibo = "*";

			List<string> LstResolucion = Coleccion.ConvertirLista(Resolucion);

			List<TblDocumentos> documentos = new List<TblDocumentos>();

			if (numero_documento.Equals("*"))
			{
				documentos = (from datos in context.TblDocumentos
							  where (datos.StrEmpresaFacturador.Equals(codigo_facturador) || codigo_facturador.Equals("*"))
							  //&& (datos.IntNumero == num_doc || numero_documento.Equals("*"))
							  && (datos.StrEmpresaAdquiriente.Equals(codigo_adquiriente) || codigo_adquiriente.Equals("*"))
							  && (LstEstado.Contains(datos.IdCategoriaEstado.ToString()) || estado_dian.Equals("*"))
							  && (datos.IntAdquirienteRecibo == cod_estado_recibo || estado_recibo.Equals("*"))
							  && (LstResolucion.Contains(datos.StrNumResolucion.ToString()) || Resolucion.Equals("*"))
							  && ((datos.DatFechaIngreso >= fecha_inicio && datos.DatFechaIngreso <= fecha_fin) || tipo_filtro_fecha == 2)
							  && ((datos.DatFechaDocumento >= fecha_inicio && datos.DatFechaDocumento <= fecha_fin) || tipo_filtro_fecha == 1)
							  orderby datos.IntNumero descending
							  select datos).ToList();
			}
			else
			{
				var listaDocumetos = Coleccion.ConvertirStringlong(numero_documento);

				documentos = (from datos in context.TblDocumentos
							  where (datos.StrEmpresaFacturador.Equals(codigo_facturador))
							  && (listaDocumetos.Contains(datos.IntNumero))
							  select datos).ToList();
			}

			return documentos;
		}



		/// <summary>
		/// Obtine una lista documentos por el id del plan
		/// </summary>
		/// <param name="IdPlan">Guid de seguridad del plan</param>
		/// <returns></returns>
		public List<TblDocumentos> ObtenerPorPlan(Guid IdPlan)
		{
			List<TblDocumentos> documentos = (from datos in context.TblDocumentos
											  where (datos.StrIdPlanTransaccion == IdPlan)
											  orderby datos.DatFechaIngreso descending
											  select datos).ToList();

			return documentos;
		}

		/// <summary>
		/// Obtiene los documentos del Acuse Recibo
		/// </summary>
		/// <param name="codigo_facturador"></param>
		/// <param name="numero_documento"></param>
		/// <param name="codigo_tercero"></param>
		/// <param name="estado_dian"></param>
		/// <param name="estado_recibo"></param>
		/// <param name="fecha_inicio"></param>
		/// <param name="fecha_fin"></param>
		/// <returns></returns>
		public List<TblDocumentos> ObtenerAcuseRecibo(string codigo_facturador, string numero_documento, string codigo_adquiriente, string estado_dian, string estado_recibo, DateTime fecha_inicio, DateTime fecha_fin, string Resolucion, int tipo_fecha)
		{


			fecha_inicio = fecha_inicio.Date;

			fecha_fin = new DateTime(fecha_fin.Year, fecha_fin.Month, fecha_fin.Day, 23, 59, 59, 999);

			long num_doc = -1;
			long.TryParse(numero_documento, out num_doc);

			short cod_estado_recibo = -1;
			short.TryParse(estado_recibo, out cod_estado_recibo);

			if (string.IsNullOrWhiteSpace(numero_documento))
				numero_documento = "*";

			if (string.IsNullOrWhiteSpace(codigo_adquiriente))
				codigo_adquiriente = "*";

			List<long> lista_estado_visible = new List<long>();
			if (string.IsNullOrWhiteSpace(estado_recibo))
			{
				lista_estado_visible = Coleccion.ConvertirStringlong(string.Format("{0},{1},{2}", AdquirienteRecibo.Aprobado.GetHashCode(), AdquirienteRecibo.Rechazado.GetHashCode(), AdquirienteRecibo.AprobadoTacito.GetHashCode()));
			}

			List<string> LstResolucion = Coleccion.ConvertirLista(Resolucion);

			//int Categoria = CategoriaEstado.ValidadoDian.GetHashCode();


			List<TblDocumentos> documentos = new List<TblDocumentos>();


			if (numero_documento.Equals("*"))
			{

				documentos = (from datos in context.TblDocumentos
							  join obligado in context.TblEmpresas on datos.StrEmpresaFacturador equals obligado.StrIdentificacion
							  join adquiriente in context.TblEmpresas on datos.StrEmpresaAdquiriente equals adquiriente.StrIdentificacion
							  where (obligado.StrIdentificacion.Equals(codigo_facturador) || codigo_facturador.Equals("*"))
							  //&& (datos.IntNumero == num_doc || numero_documento.Equals("*"))
							  && (adquiriente.StrIdentificacion.Equals(codigo_adquiriente) || codigo_adquiriente.Equals("*"))
							  //&& (datos.IdCategoriaEstado == Categoria)
							  //&& (LstEstado.Contains(datos.IdCategoriaEstado.ToString()) || estado_dian.Equals("*"))
							  //&& (estado_dian.Contains(datos.IntIdEstado.ToString()))
							  && (datos.IntAdquirienteRecibo == cod_estado_recibo || lista_estado_visible.Contains(datos.IntAdquirienteRecibo))
							  && (LstResolucion.Contains(datos.StrNumResolucion.ToString()) || Resolucion.Equals("*"))
							  && ((datos.DatFechaDocumento >= fecha_inicio && datos.DatFechaDocumento <= fecha_fin) || tipo_fecha == 2)
							  && ((datos.DatAdquirienteFechaRecibo >= fecha_inicio && datos.DatAdquirienteFechaRecibo <= fecha_fin) || tipo_fecha == 1)
							  orderby datos.IntNumero descending
							  select datos).ToList();
			}
			else
			{

				var listaDocumetos = Coleccion.ConvertirStringlong(numero_documento);

				documentos = (from datos in context.TblDocumentos
							  join obligado in context.TblEmpresas on datos.StrEmpresaFacturador equals obligado.StrIdentificacion
							  join adquiriente in context.TblEmpresas on datos.StrEmpresaAdquiriente equals adquiriente.StrIdentificacion
							  where obligado.StrIdentificacion.Equals(codigo_facturador)
							  && (listaDocumetos.Contains(datos.IntNumero))
							  //&& (datos.IdCategoriaEstado == Categoria)
							  select datos).ToList();
			}

			return documentos;
		}

		#region Acuse tacito
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Obtiene los documentos que no se le han dado acuse y estan vencidos
		/// </summary>
		/// <param name="codigo_facturador"></param>
		/// <param name="numero_documento"></param>
		/// <param name="codigo_tercero"></param>        
		/// <param name="fecha_inicio"></param>
		/// <param name="fecha_fin"></param>
		/// <returns></returns>
		public List<TblDocumentos> ObtenerAcuseTacito(string codigo_facturador, string numero_documento, string codigo_adquiriente)
		{
			long num_doc = -1;
			long.TryParse(numero_documento, out num_doc);

			if (string.IsNullOrWhiteSpace(codigo_facturador))
				codigo_facturador = "*";
			if (string.IsNullOrWhiteSpace(numero_documento))
				numero_documento = "*";
			if (string.IsNullOrWhiteSpace(codigo_adquiriente))
				codigo_adquiriente = "*";

			int HATP = Convert.ToInt32(Constantes.HorasAcuseTacitoInteroperabilidad.ToString());

			int estado_error = ProcesoEstado.FinalizacionErrorDian.GetHashCode();

			int Enviomail = ProcesoEstado.EnvioZip.GetHashCode();


			DateTime FechaActual = Fecha.GetFecha();

			List<TblDocumentos> documentos = (from datos in context.TblDocumentos
											  join obligado in context.TblEmpresas on datos.StrEmpresaFacturador equals obligado.StrIdentificacion
											  join adquiriente in context.TblEmpresas on datos.StrEmpresaAdquiriente equals adquiriente.StrIdentificacion
											  where datos.IntAdquirienteRecibo.Equals(0) && datos.IntIdEstado > Enviomail && datos.IntIdEstado < estado_error
											  && (((datos.StrProveedorEmisor == Constantes.NitResolucionsinPrefijo || string.IsNullOrEmpty(datos.StrProveedorEmisor))
											  && (datos.DatFechaIngreso <= SqlFunctions.DateAdd("hh", -datos.TblEmpresasFacturador.IntAcuseTacito.Value, FechaActual)
											  && datos.TblEmpresasFacturador.IntAcuseTacito.Value > 0)))
											  //********************************************************
											  || ((datos.StrProveedorEmisor != Constantes.NitResolucionsinPrefijo && (!string.IsNullOrEmpty(datos.StrProveedorEmisor)))
											  && (datos.DatFechaIngreso <= SqlFunctions.DateAdd("hh", -HATP, FechaActual))
											  && datos.IntAdquirienteRecibo.Equals(0) && datos.IntIdEstado > Enviomail && datos.IntIdEstado < estado_error)

											  orderby datos.IntNumero descending
											  select datos).ToList();
			return documentos;
		}

		#endregion

		#region Documentos Administrador
		/// <summary>
		/// Obtiene todos los documentos para el administrador de Sistema (Solo Gerencia)
		/// </summary>
		/// <param name="codigo_facturador"></param>
		/// <param name="numero_documento"></param>
		/// <param name="codigo_tercero"></param>
		/// <param name="estado_dian"></param>
		/// <param name="estado_recibo"></param>
		/// <param name="fecha_inicio"></param>
		/// <param name="fecha_fin"></param>
		/// <returns></returns>
		public List<TblDocumentos> ObtenerAdmin(string codigo_facturador, string numero_documento, string codigo_adquiriente, string estado_dian, string estado_recibo, DateTime fecha_inicio, DateTime fecha_fin, int TipoDocumento, int tipo_fecha, int Desde, int Hasta)
		{

			List<string> LstEstado = null;
			if (estado_dian == null || estado_dian == "")
			{
				estado_dian = "*";
				LstEstado = Coleccion.ConvertirLista(estado_dian);
			}
			else
			{
				LstEstado = Coleccion.ConvertirLista(estado_dian);
			}

			if (string.IsNullOrEmpty(codigo_facturador))
				codigo_facturador = "*";
			//        if (estado_dian == null || estado_dian == "")
			//estado_dian = Coleccion.ConvertirString(Ctl_MaestrosEnum.ListaEnum(0, "publico")) + "," + Coleccion.ConvertirString(Ctl_MaestrosEnum.ListaEnum(0, "privado"));

			fecha_inicio = fecha_inicio.Date;

			fecha_fin = new DateTime(fecha_fin.Year, fecha_fin.Month, fecha_fin.Day, 23, 59, 59, 999);

			long num_doc = -1;
			long.TryParse(numero_documento, out num_doc);

			short cod_estado_recibo = -1;
			short.TryParse(estado_recibo, out cod_estado_recibo);

			if (string.IsNullOrWhiteSpace(numero_documento))
				numero_documento = "*";
			if (string.IsNullOrWhiteSpace(codigo_adquiriente))
				codigo_adquiriente = "*";

			if (string.IsNullOrWhiteSpace(estado_recibo))
				estado_recibo = "*";

			List<TblDocumentos> documentos = new List<TblDocumentos>();


			if (numero_documento.Equals("*"))
			{

				documentos = (from datos in context.TblDocumentos
							  join obligado in context.TblEmpresas on datos.StrEmpresaFacturador equals obligado.StrIdentificacion
							  join adquiriente in context.TblEmpresas on datos.StrEmpresaAdquiriente equals adquiriente.StrIdentificacion
							  where (obligado.StrIdentificacion.Equals(codigo_facturador) || codigo_facturador.Equals("*"))
							&& (datos.IntNumero == num_doc || numero_documento.Equals("*"))
							&& (adquiriente.StrIdentificacion.Equals(codigo_adquiriente) || codigo_adquiriente.Equals("*"))
							//&& (estado_dian.Contains(datos.IntIdEstado.ToString()))
							&& (LstEstado.Contains(datos.IdCategoriaEstado.ToString()) || estado_dian.Equals("*"))
							&& (datos.IntAdquirienteRecibo == cod_estado_recibo || estado_recibo.Equals("*"))

							 //-------------
							 //&& (datos.DatFechaDocumento >= fecha_inicio && datos.DatFechaDocumento <= fecha_fin)
							 && ((datos.DatFechaDocumento >= fecha_inicio && datos.DatFechaDocumento <= fecha_fin) || tipo_fecha == 1)
							 && ((datos.DatFechaIngreso >= fecha_inicio && datos.DatFechaIngreso <= fecha_fin) || tipo_fecha == 2)
							//-----------
							&& (datos.IntDocTipo.Equals(TipoDocumento) || TipoDocumento == 0)
							  orderby datos.IntNumero descending
							  select datos).OrderByDescending(x => x.DatFechaDocumento).Skip(Desde).Take(Hasta).ToList();
			}
			else
			{
				var listaDocumetos = Coleccion.ConvertirStringlong(numero_documento);
				documentos = (from datos in context.TblDocumentos
							  join obligado in context.TblEmpresas on datos.StrEmpresaFacturador equals obligado.StrIdentificacion
							  join adquiriente in context.TblEmpresas on datos.StrEmpresaAdquiriente equals adquiriente.StrIdentificacion
							  where listaDocumetos.Contains(datos.IntNumero)
							  select datos).OrderByDescending(x => x.DatFechaDocumento).Skip(Desde).Take(Hasta).ToList();
			}

			return documentos;
		}
		#endregion


		/// <summary>
		/// Obtiene los documentos por id se seguridad.
		/// </summary>
		/// <param name="id_seguridad"></param>
		/// <returns></returns>
		public List<TblDocumentos> ObtenerPorIdSeguridad(System.Guid id_seguridad)
		{

			try
			{
				var respuesta = (from datos in context.TblDocumentos
								 where datos.StrIdSeguridad.Equals(id_seguridad)
								 select datos
								 );

				return respuesta.ToList();
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Obtiene un documento por id de interoperabilidad.
		/// </summary>
		/// <param name="id_seguridad"></param>
		/// <returns></returns>
		public TblDocumentos ObtenerPorIdInteroperabilidad(System.Guid id_seguridad)
		{

			try
			{
				var respuesta = (from datos in context.TblDocumentos
								 where datos.StrIdInteroperabilidad == id_seguridad
								 select datos
									 ).FirstOrDefault();


				return respuesta;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);

			}

		}


		public List<TblDocumentos> Obtener(string identificacion_empresa)
		{
			try
			{
				var respuesta = (from datos in context.TblDocumentos
								 where datos.StrEmpresaFacturador.Equals(identificacion_empresa)
								 select datos
								 );

				return respuesta.ToList();
			}
			catch (Exception)
			{

				throw;
			}
		}

		/// <summary>
		/// Calcula el CUFE o CUDE y QR de los documentos electrónicos para la representación gráfica
		/// </summary>
		/// <param name="DataKey">Clave compuesta (serial + identificación obligado ) en formato Sha1</param>
		/// <param name="Identificacion">identificación obligado</param>
		/// <param name="TipoDocumento">tipo documento 1: factura - 2: nota débito - 3: nota crédito</param>
		/// <param name="Documentos">información básica de documentos electrónicos para cálculo</param>
		/// <returns>Información calculada de CUFE/CUDE y QR</returns>
		public List<DocumentoCufe> ObtenerCufe(List<DocumentoCufe> documentos)
		{
			try
			{
				//Valida que los parametros sean correctos.
				if (documentos == null || documentos.Count == 0)
					throw new ApplicationException("No se encontraron documentos para calcular.");

				Ctl_Empresa Peticion = new Ctl_Empresa();

				//Válida que la key sea correcta.
				TblEmpresas facturador_electronico = Peticion.Validar(documentos.FirstOrDefault().DataKey, documentos.FirstOrDefault().IdentificacionObligado);

				if (!facturador_electronico.IntObligado)
					throw new ApplicationException(string.Format("Licencia inválida para la Identificacion {0}.", facturador_electronico.StrIdentificacion));

				PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

				//---Ambiente de la DIAN al que se va enviar el documento: 1 - Produccion, 2 - Pruebas
				string ambiente_dian = string.Empty;

				if (plataforma_datos.RutaPublica.Contains("app"))
					ambiente_dian = "1";
				else
					ambiente_dian = "2";

				//Convierte los registros de base de datos a objeto de servicio y los añade a la lista de retorno
				foreach (DocumentoCufe item in documentos)
				{
					try
					{
						// valida la versión del documento electrónico
						if (item.IdVersionDian != 2)
							throw new ApplicationException(string.Format("No se encuentra disponible el cálculo para la versión {0} indicada.", item.IdVersionDian));

						string FecFac = string.Format("{0}{1}", item.Fecha.ToString(Fecha.formato_fecha_hginet), item.Fecha.AddHours(5).ToString(Fecha.formato_hora_zona));

						switch (item.DocumentoTipo)
						{
							// Factura
							case 1:

								if (item.IdVersionDian == 2)
									item.Cufe = Ctl_CalculoCufe.CufeFacturaV2(item.ClaveTecnica, item.Prefijo, item.Documento.ToString(), FecFac, item.IdentificacionObligado, ambiente_dian, item.IdentificacionAdquiriente, Convert.ToDecimal(item.Total), Convert.ToDecimal(item.ValorSubtotal), Convert.ToDecimal(item.ValorIva), Convert.ToDecimal(item.ValorImpuestoConsumo), Convert.ToDecimal(item.ValorIca), true);

								break;

							// Nota Crédito
							case 3:
								if (item.IdVersionDian == 2)
								{
									//Para las notas no se usa clave tecnica si no el pin del software
									DianProveedorV2 data_dian = HgiConfiguracion.GetConfiguration().DianProveedorV2;
									item.ClaveTecnica = data_dian.Pin;
									if (ambiente_dian.Equals("2") && facturador_electronico.StrIdentificacion.Equals("811021438"))
									{
										DianProveedor data_dian_hab = HgiConfiguracion.GetConfiguration().DianProveedor;
										item.ClaveTecnica = data_dian_hab.Pin;
									}
									item.Cufe = Ctl_CalculoCufe.CufeNotaCreditoV2(item.ClaveTecnica, item.Prefijo, item.Documento.ToString(), FecFac, item.IdentificacionObligado, ambiente_dian, item.IdentificacionAdquiriente, Convert.ToDecimal(item.Total), Convert.ToDecimal(item.ValorSubtotal), Convert.ToDecimal(item.ValorIva), Convert.ToDecimal(item.ValorImpuestoConsumo), Convert.ToDecimal(item.ValorIca), true);
								}
								break;

							// Nota Débito
							case 2:
								if (item.IdVersionDian == 2)
								{
									//Para las notas no se usa clave tecnica si no el pin del software
									DianProveedorV2 data_dian = HgiConfiguracion.GetConfiguration().DianProveedorV2;
									item.ClaveTecnica = data_dian.Pin;
									item.Cufe = Ctl_CalculoCufe.CufeNotaDebitoV2(item.ClaveTecnica, item.Prefijo, item.Documento.ToString(), FecFac, item.IdentificacionObligado, ambiente_dian, item.IdentificacionAdquiriente, Convert.ToDecimal(item.Total), Convert.ToDecimal(item.ValorSubtotal), Convert.ToDecimal(item.ValorIva), Convert.ToDecimal(item.ValorImpuestoConsumo), Convert.ToDecimal(item.ValorIca), true);
								}
								break;

							default:
								throw new ApplicationException("No se encuentra disponible el documento electrónica");
								break;
						}

						// obtiene el código QR
						string ruta_validar_doc = String.Empty;

						if (ambiente_dian.Equals("2"))
						{
							ruta_validar_doc = string.Format("{0}{1}", Constantes.RutaPaginaQRHabilitacionDIAN,item.Cufe);
						}
						else
						{
							ruta_validar_doc = string.Format("{0}{1}", Constantes.RutaPaginaQRProduccionDIAN, item.Cufe);
						}

						item.QR = Ctl_CalculoCufe.ObtenerQR(item.DocumentoTipo, item.Prefijo, item.Documento, item.Fecha, item.IdentificacionObligado, item.IdentificacionAdquiriente, item.ValorSubtotal, item.ValorIva, item.ValorImpuestoConsumo, item.Total, item.Cufe, ruta_validar_doc);


					}
					catch (Exception excepcion)
					{
						item.Cufe = "";
						item.QR = "";

						item.Error = new Error()
						{
							Codigo = CodigoError.VALIDACION,
							Fecha = Fecha.GetFecha(),
							Mensaje = string.Format("Error en el cálculo. Detalle: {0}", excepcion.Message)
						};

					}
				}

				return documentos;
			}
			catch (Exception exec)
			{
				Error error = new Error(CodigoError.VALIDACION, exec);
				throw new FaultException<Error>(error, new FaultReason(string.Format("{0}", error.Mensaje)));
			}
		}


		#endregion


		#region Actualizar

		/// <summary>
		/// Actualiza la respuesta del acuse de recibo.
		/// </summary>
		/// <param name="id_seguridad"></param>
		/// <param name="estado"></param>
		/// <param name="motivo_rechazo"></param>
		/// <returns></returns>
		public List<TblDocumentos> ActualizarRespuestaAcuse(System.Guid id_seguridad, short estado, string motivo_rechazo, string usuario = "")
		{
			try
			{
				List<TblDocumentos> retorno = new List<TblDocumentos>();
				TblDocumentos doc = ObtenerPorIdSeguridad(id_seguridad).FirstOrDefault();

				Ctl_Empresa ctl_empresa = new Ctl_Empresa();

				if (doc == null)
					throw new ArgumentException(string.Format(LibreriaGlobalHGInet.Properties.RecursoMensajes.ObjectNotExistError, "el documento", doc.IntNumero));

				doc.IntAdquirienteRecibo = estado;
				doc.StrAdquirienteMvoRechazo = motivo_rechazo;
				doc.DatAdquirienteFechaRecibo = Fecha.GetFecha();
				if (doc.IntIdEstado > (short)ProcesoEstado.EnvioZip.GetHashCode())
					doc.IntIdEstado = (short)ProcesoEstado.RecepcionAcuse.GetHashCode();

				// obtiene los datos del facturador electrónico
				TblEmpresas facturador = ctl_empresa.Obtener(doc.StrEmpresaFacturador);

				//obtiene los datos del proveedor del facturador
				ctl_empresa = new Ctl_Empresa();
				TblEmpresas proveedor_emisor = ctl_empresa.Obtener(doc.StrProveedorEmisor);

				// obtiene los datos del adquiriente
				ctl_empresa = new Ctl_Empresa();
				TblEmpresas adquiriente = ctl_empresa.Obtener(doc.StrEmpresaAdquiriente);

				FacturaE_Documento resultado = new FacturaE_Documento();

				if (string.IsNullOrEmpty(doc.StrProveedorReceptor))
				{
					//Crea el XML del Acuse
					resultado = Ctl_Documento.ConvertirAcuse(doc, facturador, adquiriente, proveedor_emisor, proveedor_emisor, estado, motivo_rechazo);

					try
					{
						// envía el correo del acuse de recibo al facturador electrónico
						if (estado != AdquirienteRecibo.AprobadoTacito.GetHashCode())
						{
							Ctl_EnvioCorreos email = new Ctl_EnvioCorreos();
							email.RespuestaAcuse(doc, facturador, adquiriente, resultado.RutaArchivosProceso, "", Procedencia.Usuario, usuario);
						}
						if (doc.IntIdEstado > (short)ProcesoEstado.EnvioZip.GetHashCode())
							doc.IntIdEstado = (short)ProcesoEstado.Finalizacion.GetHashCode();
					}
					catch (Exception) { }

				}
				else
				{
					//obtiene los datos del proveedor del adquirirente
					ctl_empresa = new Ctl_Empresa();
					TblEmpresas proveedor_receptor = ctl_empresa.Obtener(doc.StrProveedorReceptor);
					//Crea el XML del Acuse
					resultado = Ctl_Documento.ConvertirAcuse(doc, facturador, adquiriente, proveedor_emisor, proveedor_receptor, estado, motivo_rechazo);
					doc.IntIdEstado = (short)ProcesoEstado.PendienteEnvioProveedorAcuse.GetHashCode();
				}

				doc.DatFechaActualizaEstado = Fecha.GetFecha();
				doc.StrUrlAcuseUbl = resultado.RutaArchivosProceso;
				Ctl_Documento actualizar_doc = new Ctl_Documento();
				doc = actualizar_doc.Actualizar(doc);

				doc.TblEmpresasFacturador = facturador;
				doc.TblEmpresasAdquiriente = adquiriente;
				retorno.Add(doc);


				try
				{
					Ctl_DocumentosAudit clase_auditoria = new Ctl_DocumentosAudit();
					int estado_doc = Ctl_Documento.ObtenerCategoria(doc.IntIdEstado);
					clase_auditoria.Crear(doc.StrIdSeguridad, new Guid(), facturador.StrIdentificacion, ProcesoEstado.RecepcionAcuse, TipoRegistro.Proceso, Procedencia.Usuario, usuario, Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.ProcesoEstado>(ProcesoEstado.RecepcionAcuse.GetHashCode())), Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.AdquirienteRecibo>(doc.IntAdquirienteRecibo)), doc.StrPrefijo, Convert.ToString(doc.IntNumero), estado_doc);
				}
				catch (Exception) { throw; }


				return retorno;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		#endregion

		/// <summary>
		/// Convierte respuesta a un objeto de Acuse y Ubl
		/// </summary>
		/// <param name="doc">Documento al que dieron acuse</param>
		/// <param name="facturador">Informacion del emisor del documento electronico </param>
		/// <param name="adquiriente">Informacion del receptor del documento electronico</param>
		/// <param name="estado">estado del Acuse</param>
		/// <param name="motivo_rechazo">descripcion del acuse</param>
		/// <returns></returns>
		public static FacturaE_Documento ConvertirAcuse(TblDocumentos doc, TblEmpresas facturador, TblEmpresas adquiriente, TblEmpresas proveedor_emisor, TblEmpresas proveedor_receptor, short estado, string motivo_rechazo)
		{
			PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

			//Crea el Acuse
			Acuse doc_acuse = new Acuse();
			doc_acuse.IdAcuse = "1";
			doc_acuse.IdSeguridad = doc.StrIdSeguridad.ToString();
			doc_acuse.Documento = doc.IntNumero;
			doc_acuse.Prefijo = doc.StrPrefijo;
			doc_acuse.MvoRespuesta = motivo_rechazo;
			doc_acuse.Fecha = Convert.ToDateTime(doc.DatAdquirienteFechaRecibo);
			doc_acuse.CodigoRespuesta = Enumeracion.GetAmbiente(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.CodigoResponseV2>(estado));
			doc_acuse.TipoDocumento = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.DocumentTypeV2>(doc.IntDocTipo));
			doc_acuse.DatosAdquiriente = Ctl_Empresa.Convertir(adquiriente);
			doc_acuse.DatosObligado = Ctl_Empresa.Convertir(facturador);

			//---Ambiente de la DIAN al que se va enviar el documento: 1 - Produccion, 2 - Pruebas
			string ambiente_dian = string.Empty;

			if (plataforma_datos.RutaPublica.Contains("app"))
				ambiente_dian = "1";
			else
				ambiente_dian = "2";

			// obtiene los datos del proveedor tecnológico de la DIAN
			DianProveedorV2 data_dian = HgiConfiguracion.GetConfiguration().DianProveedorV2;

			//Para el ambiente de habilitacion a nombre de HGI se cambia informacion del pin e id del SW
			DianProveedor data_dian_habilitacion = HgiConfiguracion.GetConfiguration().DianProveedor;

			string PinSoftware = data_dian.Pin;

			// sobre escribe los datos de la resolución si se encuentra en estado de habilitación
			if (ambiente_dian.Equals("2") && facturador.StrIdentificacion.Equals(data_dian_habilitacion.NitProveedor))
			{
				PinSoftware = data_dian_habilitacion.Pin;
			}

			//Convierte el objeto en archivo XML-UBL
			FacturaE_Documento resultado = HGInetUBLv2_1.AcuseReciboXMLv2_1.CrearDocumento(doc_acuse, proveedor_receptor, proveedor_emisor, ambiente_dian, PinSoftware, doc.StrCufe);
			resultado.IdSeguridadTercero = facturador.StrIdSeguridad;
			resultado.IdSeguridadDocumento = doc.StrIdSeguridad;
			resultado.IdSeguridadPeticion = Guid.NewGuid();
			resultado.DocumentoTipo = TipoDocumento.AcuseRecibo;
			resultado.VersionDian = facturador.IntVersionDian;

			// valida el nodo de ExtensionContent
			resultado.DocumentoXml = HGInetUBL.ExtensionDian.ValidarNodo(resultado.DocumentoXml);

			// ruta física del xml
			string carpeta_xml = string.Format("{0}\\{1}\\{2}", plataforma_datos.RutaDmsFisica, Constantes.CarpetaFacturaElectronica, resultado.IdSeguridadTercero.ToString());
			carpeta_xml = string.Format(@"{0}\{1}", carpeta_xml, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaXmlFacturaE);

			// nombre del xml
			string archivo_xml = string.Format(@"{0}.xml", resultado.NombreXml);

			// ruta del xml
			string ruta_xml = string.Format(@"{0}\{1}", carpeta_xml, archivo_xml);

			// elimina el archivo xml si existe
			if (Archivo.ValidarExistencia(ruta_xml))
				Archivo.Borrar(ruta_xml);

			// almacena el archivo xml
			string ruta_save = Xml.Guardar(resultado.DocumentoXml, carpeta_xml, archivo_xml);
			

			// asigna la ruta del directorio para los archivos
			resultado.RutaArchivosProceso = carpeta_xml;
			resultado.RutaArchivosEnvio = carpeta_xml.Replace("XmlFacturaE", "XmlAcuse");

			//Proceso para firmar Acuse
			DocumentoRespuesta respuesta = new DocumentoRespuesta();

			TblEmpresas empresa_firma = new TblEmpresas();

			//Valida si el adquiriente del documento tiene certificado con nosotros para firmar el acuse con ese certificado
			if (adquiriente.IntCertFirma > 0)
			{
				empresa_firma = adquiriente;
				respuesta = Ctl_Documentos.UblFirmar(empresa_firma, doc, ref respuesta, ref resultado);
			}
			else
			{
				//Si existe lo elimine para que mueva el que se esta generando actualmente.
				if (Archivo.ValidarExistencia(string.Format(@"{0}\{1}", resultado.RutaArchivosEnvio, archivo_xml)))
					Archivo.Borrar(string.Format(@"{0}\{1}", resultado.RutaArchivosEnvio, archivo_xml));
				//Se mueve el Acuse sin Firmar a la Carpeta que se muestra en plataforma
				Archivo.Mover(ruta_xml, resultado.RutaArchivosEnvio, archivo_xml);

			}



			//Se elimina el Acuse generado sin Firmar de la carpeta donde se creo
			if (Archivo.ValidarExistencia(ruta_xml))
				Archivo.Borrar(ruta_xml);



			//resultado = Ctl_Ubl.Almacenar(resultado);

			// url pública del xml
			string url_ppal = string.Format("{0}/{1}/{2}", plataforma_datos.RutaDmsPublica, Constantes.CarpetaFacturaElectronica, resultado.IdSeguridadTercero.ToString());
			resultado.RutaArchivosProceso = string.Format(@"{0}/{1}/{2}.xml", url_ppal, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaXmlAcuse, resultado.NombreXml);

			return resultado;
		}

		/// <summary>
		/// Convierte un Objeto de Servicio a un Objeto de Bd
		/// </summary>
		/// <param name="respuesta">Objeto de respuesta</param>
		/// <param name="factura">Objeto enviado por el usuario</param>
		/// <param name="empresa"></param>
		/// <param name="adquiriente"></param>
		/// <param name="tipo_doc"></param>
		/// <returns></returns>
		public static TblDocumentos Convertir(DocumentoRespuesta respuesta, object documento, TblEmpresasResoluciones resolucion, TblEmpresas empresa, TipoDocumento tipo_doc)
		{
			try
			{
				//Asigna a un objeto dinamico el objeto enviado por el usuario
				var documento_obj = (dynamic)null;
				documento_obj = documento;

				TblDocumentos tbl_documento = new TblDocumentos();

				tbl_documento.DatFechaIngreso = respuesta.FechaRecepcion;
				tbl_documento.IntDocTipo = tipo_doc.GetHashCode();
				tbl_documento.IntNumero = documento_obj.Documento;
				tbl_documento.StrPrefijo = (!string.IsNullOrEmpty(documento_obj.Prefijo)) ? documento_obj.Prefijo : "";
				if (tipo_doc == TipoDocumento.NotaCredito || tipo_doc == TipoDocumento.NotaDebito)
				{
					tbl_documento.DatFechaVencDocumento = documento_obj.Fecha;
				}
				else
				{
					tbl_documento.DatFechaVencDocumento = documento_obj.FechaVence;
				}
				tbl_documento.DatFechaDocumento = Convert.ToDateTime(documento_obj.Fecha.ToString(Fecha.formato_fecha_hginet));
				tbl_documento.StrObligadoIdRegistro = documento_obj.CodigoRegistro;
				tbl_documento.StrNumResolucion = resolucion.StrNumResolucion;
				tbl_documento.StrEmpresaFacturador = empresa.StrIdentificacion;
				tbl_documento.StrEmpresaAdquiriente = documento_obj.DatosAdquiriente.Identificacion;
				tbl_documento.StrCufe = respuesta.Cufe;
				tbl_documento.IntVlrTotal = documento_obj.Total;
				tbl_documento.StrIdSeguridad = Guid.Parse(respuesta.IdDocumento);
				tbl_documento.IntAdquirienteRecibo = 0;
				tbl_documento.IntIdEstado = Convert.ToInt16(respuesta.IdProceso);
				tbl_documento.DatFechaActualizaEstado = Fecha.GetFecha();
				tbl_documento.StrVersion = documento_obj.VersionAplicativo;
				tbl_documento.StrProveedorEmisor = Constantes.NitResolucionsinPrefijo;
				tbl_documento.StrProveedorReceptor = documento_obj.IdentificacionProveedor;
				tbl_documento.IntValorSubtotal = documento_obj.ValorSubtotal;
				tbl_documento.IntValorNeto = documento_obj.Neto;
				tbl_documento.IntVersionDian = empresa.IntVersionDian;
				//validacion si es un formato de integrador para guardar los campos predeterminados
				if (documento_obj.DocumentoFormato.Codigo > 0 && empresa.IntVersionDian == 2)
				{
					if (documento_obj.DocumentoFormato != null)
					{
						tbl_documento.StrFormato = JsonConvert.SerializeObject(documento_obj.DocumentoFormato);
					}
				}

				return tbl_documento;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Convierte un objeto de tipo de base de datos a objeto de servicio. 
		/// </summary>
		/// <param name="objetoBd"></param>
		/// <param name="reenvio">Si es para reenviar correo valida existencia de la Respuesta de la DIAN</param>
		/// <returns></returns>
		public static object ConvertirServicio(TblDocumentos objetoBd, bool reenvio = false)
		{
			//Asigna a un objeto dinamico el objeto enviado por el usuario
			var documento_obj = (dynamic)null;

			// lee el archivo XML en UBL desde la ruta pública
			string contenido_xml = Archivo.ObtenerContenido(objetoBd.StrUrlArchivoUbl);

			// valida el contenido del archivo
			if (string.IsNullOrWhiteSpace(contenido_xml))
				throw new ArgumentException("El archivo XML UBL se encuentra vacío.");

			// convierte el contenido de texto a xml
			XmlReader xml_reader = XmlReader.Create(new StringReader(contenido_xml));

			// convierte el objeto de acuerdo con el tipo de documento
			XmlSerializer serializacion = null;

			//Segun el tipo del documento lo asigna al objeto dinamico y convierte el Ubl en objeto de servicio
			if (objetoBd.IntDocTipo == TipoDocumento.Factura.GetHashCode())
			{
				FacturaConsulta factura = new FacturaConsulta();
				documento_obj = factura;

				if (objetoBd.IntVersionDian == 1)
				{
					serializacion = new XmlSerializer(typeof(HGInetUBL.InvoiceType));

					HGInetUBL.InvoiceType conversion = (HGInetUBL.InvoiceType)serializacion.Deserialize(xml_reader);

					documento_obj.DatosFactura = HGInetUBL.FacturaXML.Convertir(conversion, objetoBd, true);
				}
				else
				{
					serializacion = new XmlSerializer(typeof(InvoiceType));

					InvoiceType conversion = (InvoiceType)serializacion.Deserialize(xml_reader);

					documento_obj.DatosFactura = HGInetUBLv2_1.FacturaXMLv2_1.Convertir(conversion, objetoBd, reenvio);
				}

				documento_obj.DatosFactura.CodigoRegistro = objetoBd.StrIdSeguridad.ToString();

			}
			else if (objetoBd.IntDocTipo == TipoDocumento.NotaCredito.GetHashCode())
			{

				NotaCreditoConsulta nota_credito = new NotaCreditoConsulta();

				documento_obj = nota_credito;

				if (objetoBd.IntVersionDian == 1)
				{

					serializacion = new XmlSerializer(typeof(HGInetUBL.CreditNoteType));

					HGInetUBL.CreditNoteType conversion = (HGInetUBL.CreditNoteType)serializacion.Deserialize(xml_reader);

					documento_obj.DatosNotaCredito = HGInetUBL.NotaCreditoXML.Convertir(conversion, objetoBd, true);
				}
				else
				{
					serializacion = new XmlSerializer(typeof(CreditNoteType));

					CreditNoteType conversion = (CreditNoteType)serializacion.Deserialize(xml_reader);

					documento_obj.DatosNotaCredito = HGInetUBLv2_1.NotaCreditoXMLv2_1.Convertir(conversion, objetoBd, reenvio);
				}

				documento_obj.DatosNotaCredito.CodigoRegistro = objetoBd.StrIdSeguridad.ToString();
			}
			else if (objetoBd.IntDocTipo == TipoDocumento.NotaDebito.GetHashCode())
			{

				NotaDebitoConsulta nota_debito = new NotaDebitoConsulta();

				documento_obj = nota_debito;

				if (objetoBd.IntVersionDian == 1)
				{

					serializacion = new XmlSerializer(typeof(HGInetUBL.DebitNoteType));

					HGInetUBL.DebitNoteType conversion = (HGInetUBL.DebitNoteType)serializacion.Deserialize(xml_reader);

					documento_obj.DatosNotaDebito = HGInetUBL.NotaDebitoXML.Convertir(conversion, objetoBd);
				}
				else
				{
					serializacion = new XmlSerializer(typeof(DebitNoteType));

					DebitNoteType conversion = (DebitNoteType)serializacion.Deserialize(xml_reader);

					documento_obj.DatosNotaDebito = HGInetUBLv2_1.NotaDebitoXMLv2_1.Convertir(conversion, objetoBd, reenvio);
				}


				documento_obj.DatosNotaDebito.CodigoRegistro = objetoBd.StrIdSeguridad.ToString();

			}
			// cerrar la lectura del archivo xml
			xml_reader.Close();

			//convierte las demas propiedades del objeto de BD al objeto de servicio
			documento_obj.IdDocumento = objetoBd.StrObligadoIdRegistro.ToString();
			documento_obj.CodigoRegistro = objetoBd.StrIdSeguridad.ToString();
			documento_obj.Documento = objetoBd.IntNumero;
			documento_obj.IdProceso = objetoBd.IntIdEstado;

			//obtengo el estado del proceso
			ProcesoEstado proceso_estado = Enumeracion.ParseToEnum<ProcesoEstado>((int)objetoBd.IntIdEstado);
			documento_obj.DescripcionProceso = Enumeracion.GetDescription(proceso_estado);

			documento_obj.IdentificacionFacturador = objetoBd.StrEmpresaFacturador;
			documento_obj.Aceptacion = objetoBd.IntAdquirienteRecibo;
			documento_obj.MotivoRechazo = objetoBd.StrAdquirienteMvoRechazo;
			documento_obj.UrlPdf = objetoBd.StrUrlArchivoPdf;
			documento_obj.UrlXmlUbl = objetoBd.StrUrlArchivoUbl;
			documento_obj.FechaUltimoProceso = objetoBd.DatFechaActualizaEstado;
			//documento_obj.Neto = objetoBd.IntValorNeto;

			if (!reenvio)
			{
				//Obtiene la carpeta donde quedo la consulta de la DIAN
				TipoDocumento doc_tipo = Enumeracion.GetEnumObjectByValue<TipoDocumento>(objetoBd.IntDocTipo);

				// Nombre del archivo Xml 
				string archivo_xml = string.Format(@"{0}.xml", HGInetUBL.NombramientoArchivo.ObtenerXml(objetoBd.IntNumero.ToString(), objetoBd.StrEmpresaFacturador, doc_tipo, objetoBd.StrPrefijo));

				PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

				//Url publica de la respuesta de la DIAN en xml
				string url_ppal_respuesta = string.Format("{0}/{1}/{2}", plataforma_datos.RutaDmsPublica, Constantes.CarpetaFacturaElectronica, objetoBd.TblEmpresasFacturador.StrIdSeguridad.ToString());

				string ruta_xml = string.Format(@"{0}/{1}/{2}", url_ppal_respuesta, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEConsultaDian, archivo_xml);

				//Obtiene el archivo en la ruta Http
				ArchivoUrl archivo_consulta = Archivo.Obtener(ruta_xml);

				//Valida que el archivo si existe de lo contrario vuelve a consultar el documento en la DIAN.

				if (archivo_consulta == null)
				{

					DocumentoRespuesta consulta_dian = new DocumentoRespuesta();

					string id_validacion_previa = String.Empty;

					if (objetoBd.StrIdRadicadoDian != null)
						id_validacion_previa = objetoBd.StrIdRadicadoDian.ToString();

					consulta_dian = Ctl_Documentos.Consultar(objetoBd, objetoBd.TblEmpresasFacturador, ref consulta_dian, id_validacion_previa);

					ruta_xml = consulta_dian.EstadoDian.UrlXmlRespuesta;

				}

				//asigna la ruta que tiene el archivo donde se guardo la consulta que se hizo a la DIAN
				documento_obj.EstadoDian = new RespuestaDian();
				documento_obj.EstadoDian.UrlXmlRespuesta = ruta_xml;

				//Construye la url publica para el acuse de recibo del documento
				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;
				documento_obj.UrlAcuse = string.Format("{0}{1}", plataforma.RutaPublica, Constantes.PaginaAcuseRecibo.Replace("{id_seguridad}", objetoBd.StrIdSeguridad.ToString()));
			}

			return documento_obj;
		}

		/// <summary>
		/// Convierte un objeto de tipo de base de datos a objeto de servicio.
		/// </summary>
		/// <param name="respuesta">Objeto de tipo TblDocumentos</param>
		/// <returns>Objeto Tipo DocumentoRespuesta</returns>
		public static DocumentoRespuesta Convertir(TblDocumentos respuesta)
		{
			try
			{
				//Valida el tipo de documento enviado por el usuario

				if (respuesta == null)
					throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "respuesta", "TblDocumentos"));

				//Construye la url publica para la Auditoria de recibo del documento
				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;

				DocumentoRespuesta obj_documento = new DocumentoRespuesta();

				obj_documento.Aceptacion = (respuesta.IntAdquirienteRecibo > AdquirienteRecibo.AprobadoTacito.GetHashCode()) ? AdquirienteRecibo.Pendiente.GetHashCode() : respuesta.IntAdquirienteRecibo;
				obj_documento.DescripcionAceptacion = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<AdquirienteRecibo>(respuesta.IntAdquirienteRecibo));
				obj_documento.CodigoRegistro = respuesta.StrObligadoIdRegistro;
				obj_documento.Cufe = respuesta.StrCufe;
				obj_documento.DocumentoTipo = respuesta.IntDocTipo;
				obj_documento.Documento = respuesta.IntNumero;
				obj_documento.FechaRecepcion = respuesta.DatFechaIngreso;
				obj_documento.FechaUltimoProceso = respuesta.DatFechaActualizaEstado;
				obj_documento.IdDocumento = respuesta.StrIdSeguridad.ToString();
				obj_documento.Identificacion = respuesta.TblEmpresasAdquiriente.StrIdentificacion;
				obj_documento.IdentificacionObligado = respuesta.TblEmpresasFacturador.StrIdentificacion;
				obj_documento.IdProceso = respuesta.IntIdEstado;
				obj_documento.DescripcionProceso = Enumeracion.GetDescription(Enumeracion.ParseToEnum<ProcesoEstado>(Convert.ToInt32(respuesta.IntIdEstado)));
				obj_documento.MotivoRechazo = respuesta.StrAdquirienteMvoRechazo;
				obj_documento.NumeroResolucion = respuesta.TblEmpresasResoluciones.StrNumResolucion;
				obj_documento.Prefijo = respuesta.TblEmpresasResoluciones.StrPrefijo;
				obj_documento.IdPlan = (respuesta.StrIdPlanTransaccion.HasValue) ? (respuesta.StrIdPlanTransaccion.Value) : new Guid();

				if (respuesta.IntIdEstado == ProcesoEstado.Finalizacion.GetHashCode() || respuesta.IntIdEstado == ProcesoEstado.FinalizacionErrorDian.GetHashCode())
				{
					obj_documento.ProcesoFinalizado = 1;
				}
				else
				{
					obj_documento.ProcesoFinalizado = 0;
				}
				obj_documento.UrlPdf = respuesta.StrUrlArchivoPdf;
				obj_documento.UrlXmlUbl = respuesta.StrUrlArchivoUbl;
				obj_documento.UrlAnexo = respuesta.StrUrlAnexo;

				//Ctl_DocumentosAudit clase_audit = new Ctl_DocumentosAudit();
				//List<TblAuditDocumentos> datos_auditoria = clase_audit.Obtener(respuesta.StrIdSeguridad.ToString(), respuesta.TblEmpresasFacturador.StrIdentificacion);

				//if (datos_auditoria.Count > 0)
				obj_documento.UrlAuditoria = string.Format("{0}{1}", plataforma.RutaPublica, Constantes.PaginaConsultaAuditoriaDoc.Replace("{id_seguridad_doc}", respuesta.StrIdSeguridad.ToString()));

				obj_documento.IdEstado = respuesta.IdCategoriaEstado;
				obj_documento.DescripcionEstado = Enumeracion.GetDescription(Enumeracion.ParseToEnum<CategoriaEstado>(respuesta.IdCategoriaEstado));

				if (respuesta.IntIdEstado == 1)
					obj_documento.DescripcionProceso = "Recepción - Información del documento.";

				obj_documento.IdEstadoEnvioMail = respuesta.IntEstadoEnvio;
				obj_documento.DescripcionEstadoEnvioMail = string.Format("{0} - {1}", Enumeracion.GetDescription(Enumeracion.ParseToEnum<EstadoEnvio>(Convert.ToInt32(respuesta.IntEstadoEnvio))), Enumeracion.GetDescription(Enumeracion.ParseToEnum<MensajeEstado>(Convert.ToInt32(respuesta.IntMensajeEnvio))));
				obj_documento.IdVersionDian = respuesta.IntVersionDian;

				return obj_documento;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}

		}



		#region ProcesarDocumentos

		/// <summary>
		/// Obtiene los documentos que esten pendientes por procesar
		/// </summary>
		/// <param name="IdSeguridad">Id del documento</param>
		/// <param name="estado_recibo">Estado del proceso del documento</param>
		/// <param name="fecha_inicio">Fecha inicio</param>
		/// <param name="fecha_fin">Fecha Fin</param>
		/// <returns>Objeto de tipo respuesta</returns>
		public List<TblDocumentos> ObtenerDocumentosaProcesar()
		{
			string estado_recibo = string.Empty;

			estado_recibo = Coleccion.ConvertirString(Ctl_MaestrosEnum.ListaEnum(0, "privado"));

			List<string> estados = Coleccion.ConvertirLista(estado_recibo);

			DateTime FechaActual = Fecha.GetFecha();

			var respuesta = (from datos in context.TblDocumentos
							 join empresa in context.TblEmpresas on datos.StrEmpresaAdquiriente equals empresa.StrIdentificacion
							 where (estado_recibo.Contains(datos.IntIdEstado.ToString()))
							 && datos.DatFechaIngreso < SqlFunctions.DateAdd("ss", -15, FechaActual)

							 orderby datos.DatFechaIngreso descending
							 select datos).ToList();

			return respuesta;
		}
		/// <summary>
		/// Documentos a Procesar por la sonda
		/// </summary>
		/// <param name="ListaEstados"></param>
		/// <returns></returns>
		public List<TblDocumentos> ObtenerDocumentosaProcesar(string ListaEstados, int dias)
		{

			List<string> estados = Coleccion.ConvertirLista(ListaEstados);

			DateTime FechaActual = Fecha.GetFecha();

			var respuesta = (from datos in context.TblDocumentos
							 join empresa in context.TblEmpresas on datos.StrEmpresaAdquiriente equals empresa.StrIdentificacion
							 where (estados.Contains(datos.IntIdEstado.ToString()))
							 && datos.DatFechaIngreso < SqlFunctions.DateAdd("ss", -50, FechaActual)
							 && (datos.DatFechaIngreso < SqlFunctions.DateAdd("ss", -50, FechaActual) && datos.DatFechaIngreso > SqlFunctions.DateAdd("dd", -dias, FechaActual) || dias == 0)

							 orderby datos.DatFechaIngreso descending
							 select datos).ToList();

			return respuesta;
		}
		#endregion

		#region Reenviar Acuse
		/// <summary>
		/// Actualiza la respuesta del acuse de recibo.
		/// </summary>
		/// <param name="id_seguridad"></param>
		/// <param name="estado"></param>
		/// <param name="motivo_rechazo"></param>
		/// <returns></returns>
		public bool ReenviarRespuestaAcuse(System.Guid id_seguridad, string mail, string Usuario)
		{
			try
			{
				List<TblDocumentos> retorno = new List<TblDocumentos>();
				TblDocumentos doc = ObtenerPorIdSeguridad(id_seguridad).FirstOrDefault();

				Ctl_Empresa ctl_empresa = new Ctl_Empresa();

				if (doc == null)
					throw new ArgumentException(string.Format(LibreriaGlobalHGInet.Properties.RecursoMensajes.ObjectNotExistError, "el documento", doc.IntNumero));


				// obtiene los datos del facturador electrónico
				TblEmpresas facturador = ctl_empresa.Obtener(doc.StrEmpresaFacturador);

				//obtiene los datos del proveedor del facturador
				TblEmpresas proveedor_emisor = ctl_empresa.Obtener(doc.StrProveedorEmisor);

				// obtiene los datos del adquiriente
				TblEmpresas adquiriente = ctl_empresa.Obtener(doc.StrEmpresaAdquiriente);

				string ruta_acuse = string.Empty;

				//Valida si ya tiene 
				if (string.IsNullOrEmpty(doc.StrUrlAcuseUbl))
				{
					FacturaE_Documento resultado = new FacturaE_Documento();

					if (string.IsNullOrEmpty(doc.StrProveedorReceptor) && (doc.StrProveedorReceptor.Equals(Constantes.NitResolucionsinPrefijo)))
					{
						resultado = Ctl_Documento.ConvertirAcuse(doc, facturador, adquiriente, proveedor_emisor, proveedor_emisor, doc.IntAdquirienteRecibo, doc.StrAdquirienteMvoRechazo);
					}
					else
					{
						//obtiene los datos del proveedor del facturador
						TblEmpresas proveedor_receptor = ctl_empresa.Obtener(doc.StrProveedorReceptor);
						resultado = Ctl_Documento.ConvertirAcuse(doc, facturador, adquiriente, proveedor_emisor, proveedor_receptor, doc.IntAdquirienteRecibo, doc.StrAdquirienteMvoRechazo);
					}
					ruta_acuse = resultado.RutaArchivosProceso;
					doc.StrUrlAcuseUbl = resultado.RutaArchivosProceso;
					doc.DatFechaActualizaEstado = Fecha.GetFecha();
					doc = Actualizar(doc);
				}
				else
				{
					ruta_acuse = doc.StrUrlAcuseUbl;
				}

				try
				{   // envía el correo del acuse de recibo al facturador electrónico
					Ctl_EnvioCorreos email = new Ctl_EnvioCorreos();
					email.RespuestaAcuse(doc, facturador, adquiriente, ruta_acuse, mail, Procedencia.Usuario, Usuario);

				}
				catch (Exception) { }


				return true;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		#endregion

		#region Recibir lista de documentos para procesar
		/// <summary>
		/// procesa Lista de Documentos
		/// </summary>
		/// <param name="List_id_seguridad"></param>        
		/// <returns></returns>
		public List<TblDocumentos> ProcesarDocumentos(List<System.Guid> List_id_seguridad)
		{
			try
			{
				List<TblDocumentos> retorno = (from doc in context.TblDocumentos
											   where List_id_seguridad.Contains(doc.StrIdSeguridad)
											   select doc).ToList();


				return retorno;

			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Retorna lista de documentos para dar acuse tacito
		/// </summary>
		/// <param name="List_Documentos"></param>        
		/// <returns></returns>
		public List<TblDocumentos> DocumentosAcuseTacito(List<long> List_Documentos)
		{
			try
			{
				List<TblDocumentos> retorno = (from doc in context.TblDocumentos
											   where List_Documentos.Contains(doc.IntNumero)
											   select doc).ToList();


				return retorno;

			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}
		#endregion

		#region Consulta de documentos por cliente (Soporte)
		/// <summary>
		/// Obtiene un documento por empresa y los primeros 8 digitos del idseguridad o por la empresa-- Resolución y Numero del documento
		/// </summary>
		/// <param name="codigo_adquiente"></param>
		/// <param name="numero_documento"></param>
		/// <param name="estado_recibo"></param>
		/// <param name="fecha_inicio"></param>
		/// <param name="fecha_fin"></param>
		/// <returns></returns>
		public List<TblDocumentos> ObtenerDocumentoCliente(string codigo_facturador, int? numero_documento, string IdSeguridad = "*", string numero_resolucion = "*")
		{


			if (codigo_facturador == "")
				throw new ApplicationException("Debe Indicar el Facturador");

			if (IdSeguridad == "*" && (numero_resolucion == "*" || numero_documento == 0 || numero_documento == null))
				throw new ApplicationException("No se han especificado los criterios de búsqueda");

			if ((IdSeguridad != "*" && IdSeguridad != null) && (numero_resolucion != "*" || numero_documento != null))
				throw new ApplicationException("No se han especificado los criterios de búsqueda");

			if (IdSeguridad != "*" && IdSeguridad != null)
				if ((IdSeguridad.Length != 8))
					throw new ApplicationException("El codigo de plataforma debe ser de 8 digitos");


			List<string> list_resolucion = Coleccion.ConvertirLista(numero_resolucion, '-');
			int tipo_doc = Enumeracion.GetValueFromDescription<TipoDocumento>(list_resolucion[0]).GetHashCode();
			string prefijo = list_resolucion[1];
			if (numero_resolucion.Contains("S/PREFIJO"))
				prefijo = string.Empty;
			string resolucion = "*";
			if (tipo_doc == TipoDocumento.Factura.GetHashCode())
				resolucion = list_resolucion[2];

			List<TblDocumentos> respuesta = (from datos in context.TblDocumentos
											 join empresa in context.TblEmpresas on datos.StrEmpresaAdquiriente equals empresa.StrIdentificacion
											 where datos.StrEmpresaFacturador.Equals(codigo_facturador)
												   && (datos.StrIdSeguridad.ToString().Contains(IdSeguridad) || IdSeguridad.Equals("*"))
												   && (datos.StrNumResolucion.Equals(resolucion) || resolucion.Equals("*"))
												   && (datos.StrPrefijo.Equals(prefijo))
												   && (datos.IntNumero == numero_documento || numero_documento == null)
												   && (datos.IntDocTipo == tipo_doc)
											 orderby datos.DatFechaIngreso descending
											 select datos).ToList();

			return respuesta;
		}
		#endregion


		#region Interoperabilidad
		/// <summary>
		/// Obtiene una lista de documentos pendientes para enviar
		/// Esto debe ir en el controlador de documentos.interoperabilidad
		/// </summary>
		/// <returns></returns>
		public List<TblDocumentos> ObtenerDocumentosProveedores(string IdentificacionProveedor)
		{
			int DocPendiente = ProcesoEstado.PendienteEnvioProveedorDoc.GetHashCode();
			int AcusePendiente = ProcesoEstado.PendienteEnvioProveedorAcuse.GetHashCode();

			List<TblDocumentos> Doc = (from doc in context.TblDocumentos
									   where (doc.IntIdEstado == DocPendiente && doc.StrProveedorReceptor.Equals(IdentificacionProveedor)) || (doc.IntIdEstado == AcusePendiente && doc.StrProveedorEmisor.Equals(IdentificacionProveedor))
										|| IdentificacionProveedor.Equals("*")
									   select doc).ToList();


			return Doc;
		}

		/// <summary>
		/// Consulta la historia de un documento de interoperabilidad
		/// Aceptado, rechazado, pagado, ect
		/// </summary>
		/// <param name="uuid">guid de seguridad de inteoperabilidad de un documento</param>
		/// <param name="IdentificacionProveedor">Proveedor emisor del documento</param>
		/// <returns></returns>
		public TblDocumentos ObtenerHistorialDococumento(Guid uuid, string IdentificacionProveedor)
		{
			TblDocumentos Datos = (from doc in context.TblDocumentos
								   where doc.StrIdInteroperabilidad == uuid
								   && doc.StrProveedorEmisor.Equals(IdentificacionProveedor)
								   select doc).FirstOrDefault();

			return Datos;
		}

		/// <summary>
		/// Consulta la historia de un documento de interoperabilidad
		/// Aceptado, rechazado, pagado, ect
		/// </summary>
		/// <param name="uuid">guid de seguridad de inteoperabilidad de un documento</param>		
		/// <returns></returns>
		public TblDocumentos ObtenerHistorialDococumento(Guid uuid)
		{
			TblDocumentos Datos = (from doc in context.TblDocumentos
								   where doc.StrIdInteroperabilidad == uuid
								   select doc).FirstOrDefault();

			return Datos;
		}

		/// <summary>
		/// Obtiene la cantidad de documentos pendidientes de Acuse en un proveedor especifico
		/// </summary>
		/// <param name="IdentificacionProveedor">Identificacion del proveedor</param>
		/// <returns></returns>
		public List<TblDocumentos> ObtenerAcusePendienteRecepcion(string IdentificacionProveedor)
		{
			try
			{
				int EnvioEmailAcuse = ProcesoEstado.EnvioExitosoProveedor.GetHashCode();

				List<TblDocumentos> documentos = (from documento in context.TblDocumentos
												  where documento.IntIdEstado == EnvioEmailAcuse
												  && documento.StrIdInteroperabilidad != null
												  //&& !documento.StrProveedorReceptor.Equals(Constantes.NitResolucionsinPrefijo)
												  && documento.StrProveedorReceptor.Equals(IdentificacionProveedor)
												  select documento).ToList();

				return documentos;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Obtiene un documento por el nombre del archivo xml
		/// con el fin de actualizar el estado
		/// </summary>
		/// <param name="NombreArchivo"></param>
		/// <returns></returns>
		public TblDocumentos Obtenerporxml(string NombreArchivo)
		{
			TblDocumentos Doc = (from doc in context.TblDocumentos
								 where (doc.StrUrlArchivoUbl.Contains(NombreArchivo) || doc.StrUrlAcuseUbl.Contains(NombreArchivo))
								 select doc).FirstOrDefault();
			return Doc;
		}

		#endregion

		#region Sonda Procesar Documentos

		/// <summary>
		/// Sonda para procesar documentos
		/// </summary>
		/// <returns></returns>
		public async Task SondaProcesarDocumentos(string ListaEstados, int dias, bool Consultar = true)
		{
			try
			{
				var Tarea = TareaProcesarDocumentos(ListaEstados, dias, Consultar);
				await Task.WhenAny(Tarea);
			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.actualizacion);
			}
		}

		/// <summary>
		/// Tarea para procesar Documentos
		/// </summary>
		/// <returns></returns>
		public async Task TareaProcesarDocumentos(string ListaEstados, int dias, bool Consultar = true)
		{
			await Task.Factory.StartNew(() =>
			{
				Ctl_Documento ctl_documento = new Ctl_Documento();
				//Se obtienen todos los documentos para procesar, pero se excluyen los que estan pendientes por enviar a la DIAN(Se hacen manuales por el momento)
				//List<TblDocumentos> datos = ctl_documento.ObtenerDocumentosaProcesar().Where(d => d.IntIdEstado != ProcesoEstado.CompresionXml.GetHashCode()).ToList();
				List<TblDocumentos> datos = ctl_documento.ObtenerDocumentosaProcesar(ListaEstados, dias);
				List<DocumentoRespuesta> datosProcesar = Procesos.Ctl_Documentos.Procesar(datos, Consultar);

			});
		}
		#endregion

		/// <summary>
		/// Sonda para Generar PDF de documentos
		/// </summary>
		/// <param name="ListaDoc">Lista de Idseguridad de los Documentos</param>
		/// <returns></returns>
		public async Task SondaGenerarPDF(string ListaDoc)
		{
			try
			{
				var Tarea = TareaGenerarPDF(ListaDoc);
				await Task.WhenAny(Tarea);
			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.creacion);

			}
		}

		/// <summary>
		/// Tarea para generar el PDF de los documentos
		/// </summary>
		/// <param name="ListaDoc">Lista de Idseguridad de los Documentos a generar</param>
		/// <returns></returns>
		public async Task TareaGenerarPDF(string ListaDoc)
		{
			try
			{
				await Task.Factory.StartNew(() =>
					{

						List<string> lista = Coleccion.ConvertirLista(ListaDoc, ',');
						foreach (string Idseg in Coleccion.ConvertirLista(ListaDoc, ','))
						{

							PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

							TblDocumentos documento = ObtenerPorIdSeguridad(Guid.Parse(Idseg)).FirstOrDefault();

							var documento_obj = (dynamic)null;

							FacturaE_Documento documento_result = new FacturaE_Documento();

							documento_result.IdSeguridadDocumento = Guid.Parse(documento.StrIdSeguridad.ToString());
							documento_result.IdSeguridadTercero = documento.TblEmpresasFacturador.StrIdSeguridad;
							documento_result.DocumentoTipo = Enumeracion.ParseToEnum<TipoDocumento>(documento.IntDocTipo);
							documento_result.NombreXml = HGInetUBL.NombramientoArchivo.ObtenerXml(documento.IntNumero.ToString(), documento.StrEmpresaFacturador, Enumeracion.ParseToEnum<TipoDocumento>(documento.IntDocTipo), documento.StrPrefijo);
							documento_result.NombrePdf = documento_result.NombreXml;

							XmlSerializer serializacion = null;

							// ruta física del xml
							string carpeta_xml = string.Format("{0}\\{1}\\{2}", plataforma_datos.RutaDmsFisica, Constantes.CarpetaFacturaElectronica, documento.TblEmpresasFacturador.StrIdSeguridad);
							carpeta_xml = string.Format(@"{0}\{1}", carpeta_xml, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian);

							documento_result.RutaArchivosEnvio = carpeta_xml;

							// ruta del xml
							string ruta_xml = string.Format(@"{0}\{1}.xml", carpeta_xml, documento_result.NombreXml);

							if (documento.IntVersionDian == 1)
							{
								//Se lee un xml de una ruta
								FileStream xml_reader = new FileStream(ruta_xml, FileMode.Open);

								// convierte el objeto de acuerdo con el tipo de documento
								if (documento.IntDocTipo == TipoDocumento.Factura.GetHashCode())
								{
									HGInetUBL.InvoiceType conversion = new HGInetUBL.InvoiceType();

									serializacion = new XmlSerializer(typeof(HGInetUBL.InvoiceType));

									conversion = (HGInetUBL.InvoiceType)serializacion.Deserialize(xml_reader);

									documento_obj = HGInetUBL.FacturaXML.Convertir(conversion, documento);

								}
								else if (documento.IntDocTipo == TipoDocumento.NotaCredito.GetHashCode())
								{

									HGInetUBL.CreditNoteType conversion = new HGInetUBL.CreditNoteType();

									serializacion = new XmlSerializer(typeof(HGInetUBL.CreditNoteType));

									conversion = (HGInetUBL.CreditNoteType)serializacion.Deserialize(xml_reader);

									documento_obj = HGInetUBL.NotaCreditoXML.Convertir(conversion, documento);

								}
								else if (documento.IntDocTipo == TipoDocumento.NotaDebito.GetHashCode())
								{
									HGInetUBL.DebitNoteType conversion = new HGInetUBL.DebitNoteType();

									serializacion = new XmlSerializer(typeof(HGInetUBL.DebitNoteType));

									conversion = (HGInetUBL.DebitNoteType)serializacion.Deserialize(xml_reader);

									documento_obj = HGInetUBL.NotaDebitoXML.Convertir(conversion, documento);

								}

								// cerrar la lectura del archivo xml
								xml_reader.Close();

							}
							else
							{
								//Se lee un xml de una ruta
								FileStream xml_reader = new FileStream(ruta_xml, FileMode.Open);

								// convierte el objeto de acuerdo con el tipo de documento
								if (documento.IntDocTipo == TipoDocumento.Factura.GetHashCode())
								{
									serializacion = new XmlSerializer(typeof(InvoiceType));

									InvoiceType conversion = (InvoiceType)serializacion.Deserialize(xml_reader);

									documento_obj = HGInetUBLv2_1.FacturaXMLv2_1.Convertir(conversion, documento);


								}
								else if (documento.IntDocTipo == TipoDocumento.NotaCredito.GetHashCode())
								{
									serializacion = new XmlSerializer(typeof(CreditNoteType));

									CreditNoteType conversion = (CreditNoteType)serializacion.Deserialize(xml_reader);

									documento_obj = HGInetUBLv2_1.NotaCreditoXMLv2_1.Convertir(conversion, documento);

								}
								else if (documento.IntDocTipo == TipoDocumento.NotaDebito.GetHashCode())
								{
									serializacion = new XmlSerializer(typeof(DebitNoteType));

									DebitNoteType conversion = (DebitNoteType)serializacion.Deserialize(xml_reader);

									documento_obj = HGInetUBLv2_1.NotaDebitoXMLv2_1.Convertir(conversion, documento);

								}
								if (!string.IsNullOrEmpty(documento.StrFormato))
								{
									documento_obj.DocumentoFormato = JsonConvert.DeserializeObject<Formato>(documento.StrFormato);
								}

								// cerrar la lectura del archivo xml
								xml_reader.Close();

							}


							// valida la conversión del objeto
							if (documento_obj == null)
								throw new ArgumentException("No se recibieron datos para realizar el proceso");

							documento_result.Documento = documento_obj;

							DocumentoRespuesta respuesta = new DocumentoRespuesta();
							//Genera Formato PDF
							Ctl_Documentos.GuardarFormato(documento_obj, documento, ref respuesta, ref documento_result, documento.TblEmpresasFacturador);

						}

					});
			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.creacion);
			}
		}

		/// <summary>
		/// Sonda para obtener el subtotal de los documentos y actualizarlo en la BD
		/// </summary>
		/// <returns></returns>
		public async Task SondaCampoSubtotal()
		{
			try
			{
				var Tarea = TareaCampoSubtotal();
				await Task.WhenAny(Tarea);
			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.actualizacion);
			}

		}

		public async Task TareaCampoSubtotal()
		{
			await Task.Factory.StartNew(() =>
			{

				DateTime fecha_inicio = new DateTime(2017, 12, 31);
				DateTime fecha_fin = Fecha.GetFecha();

				List<TblDocumentos> datos = ObtenerAdmin("*", "*", "*", "*", "*", fecha_inicio, fecha_fin, 0, 2, 1, 1);


				foreach (var item in datos)
				{
					try
					{
						var objeto = (dynamic)null;
						objeto = Ctl_Documento.ConvertirServicio(item, true);
						if (item.IntDocTipo == TipoDocumento.Factura.GetHashCode())
						{
							item.IntValorSubtotal = objeto.DatosFactura.ValorSubtotal;
						}

						if (item.IntDocTipo == TipoDocumento.NotaCredito.GetHashCode())
						{
							item.IntValorSubtotal = objeto.DatosNotaCredito.ValorSubtotal;
						}

						if (item.IntDocTipo == TipoDocumento.NotaDebito.GetHashCode())
						{
							item.IntValorSubtotal = objeto.DatosNotaDebito.ValorSubtotal;
						}
						Ctl_Documento ctl_documento = new Ctl_Documento();
						ctl_documento.Actualizar(item);
					}
					catch (Exception excepcion)
					{
						Ctl_Log.Guardar(excepcion, MensajeCategoria.BaseDatos, MensajeTipo.Error, MensajeAccion.actualizacion);
					}


				}

			});
		}


		#region Procesar Acuses
		/// <summary>
		/// Sonda para acuse tacito, no recibe parametros
		/// </summary>
		/// <returns></returns>
		public async Task sonda()
		{
			try
			{
				var Tarea1 = SondaAcuseTacito();
				await Task.WhenAny(Tarea1);
			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.actualizacion);
			}
		}

		/// <summary>
		/// Sonda de acuse tacito
		/// </summary>
		/// <returns></returns>
		public async Task SondaAcuseTacito()
		{
			await Task.Factory.StartNew(() =>
			{
				List<TblDocumentos> datos = ObtenerAcuseTacito("*", "*", "*");

				foreach (var item in datos)
				{
					try
					{
						ActualizarRespuestaAcuse(item.StrIdSeguridad, (short)AdquirienteRecibo.AprobadoTacito.GetHashCode(), string.Empty);
					}
					catch (Exception excepcion)
					{
						Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.actualizacion);
					}
				}

				return true;
			});
		}

		#endregion

		#region Planes Null
		public List<TblDocumentos> ObtenerDocumentosaPlanesNull(string Facturadores)
		{

			List<string> ListFacturadores = Coleccion.ConvertirLista(Facturadores);

			DateTime FechaActual = Fecha.GetFecha();

			var respuesta = (from datos in context.TblDocumentos
							 where (datos.StrIdPlanTransaccion == null)
							 && (ListFacturadores.Contains(datos.StrEmpresaFacturador) || Facturadores.Equals("*"))
							 orderby datos.StrEmpresaFacturador, datos.DatFechaIngreso
							 select datos).ToList();

			return respuesta;
		}
		#endregion



		#region ConfigurarPlanes

		/// <summary>
		/// Configura los planes en los documentos null
		/// </summary>
		/// <returns></returns>
		public async Task ConfigurarPlanesDocumentos(string Facturadores)
		{
			try
			{
				var Tarea = TareaConfigurarPlanesDocumentos(Facturadores);
				await Task.WhenAny(Tarea);
			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.actualizacion);
			}
		}

		/// <summary>
		/// Tarea para Asignar el codigo del plan a los documentos que esten con plan null
		/// Se debe tener en cuenta que solo le coloca el condigo de algun plan que el facturador tenga activo y con saldo para descontar
		/// </summary>
		/// <returns></returns>
		public async Task TareaConfigurarPlanesDocumentos(string Facturadores)
		{
			await Task.Factory.StartNew(() =>
			{
				try
				{
					Ctl_Documento ctl_documento = new Ctl_Documento();
					//Obtengo la lista de documentos con codigo de plan null
					List<TblDocumentos> datos = ctl_documento.ObtenerDocumentosaPlanesNull(Facturadores);
					List<ObjPlanEnProceso> obj_plan = new List<ObjPlanEnProceso>();
					Ctl_PlanesTransacciones controladorplanes = new Ctl_PlanesTransacciones();

					foreach (var item in datos)
					{
						try
						{
							controladorplanes = new Ctl_PlanesTransacciones();
							obj_plan = new List<ObjPlanEnProceso>();
							//Obtengo el plan con el que voy a descontar el saldo
							obj_plan = controladorplanes.ObtenerPlanesActivos(item.StrEmpresaFacturador, 1);
							if (obj_plan != null)
							{
								item.StrIdPlanTransaccion = obj_plan[0].plan;
								obj_plan[0].procesado = 1;
								this.Edit(item);
								controladorplanes.ConciliarPlanProceso(obj_plan[0]);
							}
						}
						catch (Exception excepcion)
						{
							Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.actualizacion);
						}
					}
				}
				catch (Exception excepcion)
				{
					Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.actualizacion);
				}

			});
		}
		#endregion

		/// <summary>
		/// Obtiene documentos que deben ser validados la entrega del correo
		/// </summary>
		/// <param name="dias">Dias hacia atras que valida los documentos</param>
		/// <returns></returns>
		public List<TblDocumentos> ObtenerDocumentosValidarEmail(int dias)
		{

			try
			{
				int estado_enviado = Convert.ToInt32(EstadoEnvio.Enviado.GetHashCode());
				int estado_adquiriente = Convert.ToInt32(AdquirienteRecibo.Leido.GetHashCode());
				DateTime FechaActual = Fecha.GetFecha();


				if (dias > 0)
				{
					var respuesta = (from datos in context.TblDocumentos
									 where datos.DatFechaIngreso > SqlFunctions.DateAdd("dd", -dias, FechaActual) &&
										   (datos.IntEstadoEnvio == (estado_enviado) ||
											datos.IntAdquirienteRecibo > estado_adquiriente)
									 select datos
							);
					return respuesta.ToList();
				}
				else
				{
					var respuesta = (from datos in context.TblDocumentos
									 where datos.IntEstadoEnvio == (estado_enviado)
									 select datos
						);
					return respuesta.ToList();
				}



			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}

		}

		public async Task SondaDocumentosValidarEmail(int Tiempo, int dias)
		{
			try
			{
				var Tarea = TareaDocumentosValidarEmail(Tiempo, dias);
				await Task.WhenAny(Tarea);
			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.envio);
			}

		}

		/// <summary>
		/// Tarea para consultar el estado del acuse del documento
		/// </summary>
		/// <param name="Tiempo">si estado de Acuse > al parametro en minutos se notifica al Facturador</param>
		/// <returns></returns>
		public async Task TareaDocumentosValidarEmail(int Tiempo, int dias)
		{
			await Task.Factory.StartNew(() =>
			{
				Ctl_DocumentosAudit clase_audit_doc = new Ctl_DocumentosAudit();
				List<TblDocumentos> datos = ObtenerDocumentosValidarEmail(dias);

				foreach (TblDocumentos item in datos)
				{

					try
					{

						MensajeValidarEmail respuesta_consulta = new MensajeValidarEmail();
						Ctl_DocumentosAudit documento_auditoria = new Ctl_DocumentosAudit();

						respuesta_consulta = documento_auditoria.ObtenerResultadoEmail(item.StrIdSeguridad);

						if (respuesta_consulta.EmailEnviado != null)
						{
							if (MensajeIdResultado.Entregado.GetHashCode().Equals(respuesta_consulta.IdResultado))
							{
								item.IntEstadoEnvio = (short)EstadoEnvio.Entregado.GetHashCode();
								item.IntMensajeEnvio = (short)Enumeracion.GetValueFromDescription<MensajeEstado>(respuesta_consulta.Estado);
								item.DatFechaActualizaEstado = Fecha.GetFecha();

							}
							else if (MensajeIdResultado.NoEntregado.GetHashCode().Equals(respuesta_consulta.IdResultado))
							{
								item.IntEstadoEnvio = (short)EstadoEnvio.NoEntregado.GetHashCode();
								item.IntMensajeEnvio = (short)Enumeracion.GetValueFromDescription<MensajeEstado>(respuesta_consulta.Estado);
								item.DatFechaActualizaEstado = (string.IsNullOrEmpty(respuesta_consulta.Estado)) ? Fecha.GetFecha() : respuesta_consulta.Recibido;
								Ctl_EnvioCorreos email = new Ctl_EnvioCorreos();
								List<MensajeEnvio> notificacion = email.NotificacionCorreofacturador(item, item.TblEmpresasAdquiriente.StrTelefono, respuesta_consulta.EmailEnviado, respuesta_consulta.Estado, item.StrIdSeguridad.ToString());

							}
							else
							{
								item.IntEstadoEnvio = (short)EstadoEnvio.Enviado.GetHashCode();
								item.IntMensajeEnvio = (short)Enumeracion.GetValueFromDescription<MensajeEstado>(respuesta_consulta.Estado);
								item.DatFechaActualizaEstado = (string.IsNullOrEmpty(respuesta_consulta.Estado)) ? Fecha.GetFecha() : respuesta_consulta.Recibido;

							}
						}
						else
						{
							respuesta_consulta.Estado = string.Empty;
							var objeto = (dynamic)null;
							objeto = Ctl_Documento.ConvertirServicio(item, true);
							string email_objeto = string.Empty;
							string telefono_objeto = string.Empty;
							if (item.IntDocTipo == TipoDocumento.Factura.GetHashCode())
							{
								email_objeto = objeto.DatosFactura.DatosAdquiriente.Email;
								telefono_objeto = objeto.DatosFactura.DatosAdquiriente.Telefono;
							}

							if (item.IntDocTipo == TipoDocumento.NotaCredito.GetHashCode())
							{
								email_objeto = objeto.DatosNotaCredito.DatosAdquiriente.Email;
								telefono_objeto = objeto.DatosNotaCredito.DatosAdquiriente.Telefono;
							}

							if (item.IntDocTipo == TipoDocumento.NotaDebito.GetHashCode())
							{
								email_objeto = objeto.DatosNotaDebito.DatosAdquiriente.Email;
								telefono_objeto = objeto.DatosNotaDebito.DatosAdquiriente.Telefono;
							}
							//Se hace validacion para hacer el reenvio del documento
							if ((item.IntEstadoEnvio == EstadoEnvio.Pendiente.GetHashCode() || item.IntEstadoEnvio == EstadoEnvio.Enviado.GetHashCode()))
							{

								Ctl_EnvioCorreos email = new Ctl_EnvioCorreos();
								List<MensajeEnvio> notificacion = new List<MensajeEnvio>();
								try
								{
									notificacion = email.NotificacionDocumento(item, telefono_objeto, email_objeto, item.StrIdSeguridad.ToString());
								}
								catch (Exception)
								{
									//Si se presenta un error en el envio se notifica al facturador para que valide
									item.IntEstadoEnvio = (short)EstadoEnvio.Desconocido.GetHashCode();
									item.DatFechaActualizaEstado = (string.IsNullOrEmpty(respuesta_consulta.Estado)) ? Fecha.GetFecha() : respuesta_consulta.Recibido;
									item.IntEnvioMail = false;
									email.NotificacionCorreofacturador(item, telefono_objeto, email_objeto, respuesta_consulta.Estado, item.StrIdSeguridad.ToString());

								}
								if ((notificacion != null) && (notificacion.Any()))
								{
									item.IntEstadoEnvio = (short)EstadoEnvio.Enviado.GetHashCode();
									item.DatFechaActualizaEstado = Fecha.GetFecha();
									item.IntEnvioMail = true;
								}
							}
							//Valida si lleva mucho tiempo en el mismo estado y notifico al Facturador
							if (Fecha.Diferencia(item.DatFechaIngreso, Fecha.GetFecha(), DateInterval.Minute) > Tiempo)
							{
								Ctl_EnvioCorreos email = new Ctl_EnvioCorreos();
								List<MensajeEnvio> notificacion = email.NotificacionCorreofacturador(item, telefono_objeto, email_objeto, respuesta_consulta.Estado, item.StrIdSeguridad.ToString());
								item.IntEstadoEnvio = (short)EstadoEnvio.Desconocido.GetHashCode();
								item.DatFechaActualizaEstado = (string.IsNullOrEmpty(respuesta_consulta.Estado)) ? Fecha.GetFecha() : respuesta_consulta.Recibido;
							}


						}
						//Cambio el estado del acuse siempre y cuando sea mayor a 3 el estado de acuse
						if (item.IntAdquirienteRecibo > AdquirienteRecibo.AprobadoTacito.GetHashCode())
						{
							item.IntAdquirienteRecibo = (short)AdquirienteRecibo.Pendiente.GetHashCode();
							item.DatAdquirienteFechaRecibo = null;
						}


						Ctl_Documento ctl_documento = new Ctl_Documento();
						ctl_documento.Actualizar(item);
					}
					catch (Exception excepcion)
					{
						Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.actualizacion);

					}

				}

			});
		}


		/// <summary>
		/// Proceso para generar el documento UBL2.1 Attach para enviar por correo al adquiriente con la respuesta de la DIAN
		/// </summary>
		/// <param name="documento">Objeto tipo servicio del documento Electronico enviado</param>
		/// <param name="doc">Objeto BD del documento</param>
		/// <param name="facturador">Informacion del Facturador(Tbl)</param>
		/// <returns></returns>
		public static bool ConvertirAttachedDoc(object documento, TblDocumentos doc, TblEmpresas facturador)
		{

			bool doc_creado = false;
			try
			{
				var documento_obj = (dynamic)null;

				if (documento == null)
				{
					var documento_ser = (dynamic)null;
					documento_ser = ConvertirServicio(doc, false);
					if (doc.IntDocTipo == TipoDocumento.Factura.GetHashCode())
					{
						documento_obj = documento_ser.DatosFactura;
					}
					else if (doc.IntDocTipo == TipoDocumento.NotaCredito.GetHashCode())
					{
						documento_obj = documento_ser.DatosNotaCredito;
					}
					else if (doc.IntDocTipo == TipoDocumento.NotaDebito.GetHashCode())
					{
						documento_obj = documento_ser.DatosNotaDebito;
					}
				}
				else
				{
					documento_obj = documento;
				}


				PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

				//---Ambiente de la DIAN al que se va enviar el documento: 1 - Produccion, 2 - Pruebas
				string ambiente_dian = string.Empty;

				if (plataforma_datos.RutaPublica.Contains("app"))
					ambiente_dian = "1";
				else
					ambiente_dian = "2";

				TipoDocumento tipo_doc = Enumeracion.GetEnumObjectByValue<TipoDocumento>(doc.IntDocTipo);

				string nombre_archivo = HGInetUBL.NombramientoArchivo.ObtenerXml(documento_obj.Documento.ToString(), facturador.StrIdentificacion, tipo_doc, documento_obj.Prefijo);

				// ruta física del xml
				string carpeta_xml = string.Format("{0}\\{1}\\{2}", plataforma_datos.RutaDmsFisica, Constantes.CarpetaFacturaElectronica, facturador.StrIdSeguridad.ToString());
				carpeta_xml = string.Format(@"{0}\{1}", carpeta_xml, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian);


				//Convierte el objeto en archivo XML-UBL
				FacturaE_Documento resultado = HGInetUBLv2_1.AttachedDocument.CrearDocumento(doc.StrIdSeguridad, documento_obj.DatosObligado, documento_obj.DatosAdquiriente, ambiente_dian, doc);
				resultado.IdSeguridadTercero = facturador.StrIdSeguridad;
				resultado.IdSeguridadDocumento = doc.StrIdSeguridad;
				resultado.IdSeguridadPeticion = new Guid();
				resultado.DocumentoTipo = TipoDocumento.Attached;
				resultado.NombreXml = nombre_archivo.Replace("face", "attach");

				// nombre del xml
				string archivo_xml = string.Format(@"{0}.xml", resultado.NombreXml);

				// ruta del xml
				string ruta_xml = string.Format(@"{0}\{1}", carpeta_xml, archivo_xml);

				// elimina el archivo xml si existe
				if (Archivo.ValidarExistencia(ruta_xml))
					Archivo.Borrar(ruta_xml);

				// almacena el archivo xml
				string ruta_save = Xml.Guardar(resultado.DocumentoXml, carpeta_xml, archivo_xml);

				// asigna la ruta del directorio para los archivos
				resultado.RutaArchivosProceso = carpeta_xml;

				//resultado = Ctl_Ubl.Almacenar(resultado);

				// url pública del xml
				//string url_ppal = string.Format("{0}/{1}/{2}", plataforma_datos.RutaDmsPublica, Constantes.CarpetaFacturaElectronica, resultado.IdSeguridadTercero.ToString());
				//resultado.RutaArchivosProceso = string.Format(@"{0}/{1}/{2}.xml", url_ppal, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaXmlAcuse, resultado.NombreXml);
				doc_creado = true;
			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Archivos, MensajeTipo.Error, MensajeAccion.creacion);

				throw excepcion;
			}

			return doc_creado;

		}

	}

}


