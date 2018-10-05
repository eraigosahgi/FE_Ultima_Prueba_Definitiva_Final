using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Procesos;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.ControllerSql;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetUBL;
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

namespace HGInetMiFacturaElectonicaController.Registros
{
	public partial class Ctl_Documento : BaseObject<TblDocumentos>
	{
		#region Constructores 

		public Ctl_Documento() : base(new ModeloAutenticacion()) { }
		public Ctl_Documento(ModeloAutenticacion autenticacion) : base(autenticacion) { }

		public Ctl_Documento(string servidor, string basedatos, string usuario, string clave) : base(servidor, basedatos, usuario, clave) { }
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
			documento = this.Edit(documento);

			return documento;

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
			var estado_dian = "";
			estado_dian = Coleccion.ConvertirString(Ctl_MaestrosEnum.ListaEnum(0, "publico"));

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

			var respuesta = (from datos in context.TblDocumentos
							 where (datos.StrEmpresaAdquiriente.Equals(identificacion_adquiente) || identificacion_adquiente.Equals("*"))
										&& (datos.IntNumero == num_doc || numero_documento.Equals("*"))
										   && (datos.IntAdquirienteRecibo == cod_estado_recibo || estado_recibo.Equals("*"))
										   && ((datos.DatFechaDocumento >= fecha_inicio && datos.DatFechaDocumento <= fecha_fin) || tipo_filtro_fecha == 2)
										   && ((datos.DatFechaIngreso >= fecha_inicio && datos.DatFechaIngreso <= fecha_fin) || tipo_filtro_fecha == 1)
										   && (estado_dian.Contains(datos.IntIdEstado.ToString()))
										   && (datos.IntIdEstado != ErrorDian)
							 orderby datos.IntNumero descending
							 select datos).ToList();

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

			if (estado_dian == null || estado_dian == "")
				estado_dian = Coleccion.ConvertirString(Ctl_MaestrosEnum.ListaEnum(0, "publico"));

			fecha_inicio = fecha_inicio.Date;

			fecha_fin = new DateTime(fecha_fin.Year, fecha_fin.Month, fecha_fin.Day, 23, 59, 59, 999);

			long num_doc = -1;
			long.TryParse(numero_documento, out num_doc);

			//short cod_estado_dian = -1;
			//short.TryParse(estado_dian, out cod_estado_dian);

			short cod_estado_recibo = -1;
			short.TryParse(estado_recibo, out cod_estado_recibo);

			if (string.IsNullOrWhiteSpace(numero_documento))
				numero_documento = "*";
			if (string.IsNullOrWhiteSpace(codigo_adquiriente))
				codigo_adquiriente = "*";
			//if (string.IsNullOrWhiteSpace(estado_dian))
			//    estado_dian = "*";
			if (string.IsNullOrWhiteSpace(estado_recibo))
				estado_recibo = "*";

			//if (estado_dian.Equals("3"))
			//    estado_dian = "2,3";

			List<string> LstResolucion = Coleccion.ConvertirLista(Resolucion);

			//array
			/*
             
             foreach(array)
             */

			List<TblDocumentos> documentos = (from datos in context.TblDocumentos
											  where (datos.StrEmpresaFacturador.Equals(codigo_facturador) || codigo_facturador.Equals("*"))
											  && (datos.IntNumero == num_doc || numero_documento.Equals("*"))
											  && (datos.StrEmpresaFacturador.Equals(codigo_adquiriente) || codigo_adquiriente.Equals("*"))
											  && (estado_dian.Contains(datos.IntIdEstado.ToString()))
											  && (datos.IntAdquirienteRecibo == cod_estado_recibo || estado_recibo.Equals("*"))
											  && (LstResolucion.Contains(datos.StrNumResolucion.ToString()) || Resolucion.Equals("*"))
												&& ((datos.DatFechaDocumento >= fecha_inicio && datos.DatFechaDocumento <= fecha_fin) || tipo_filtro_fecha == 2)
												&& ((datos.DatFechaIngreso >= fecha_inicio && datos.DatFechaIngreso <= fecha_fin) || tipo_filtro_fecha == 1)
											  orderby datos.IntNumero descending
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

			if (estado_dian == null || estado_dian == "")
				estado_dian = Coleccion.ConvertirString(Ctl_MaestrosEnum.ListaEnum(0, "publico"));

			fecha_inicio = fecha_inicio.Date;

			fecha_fin = new DateTime(fecha_fin.Year, fecha_fin.Month, fecha_fin.Day, 23, 59, 59, 999);

			long num_doc = -1;
			long.TryParse(numero_documento, out num_doc);

			//short cod_estado_dian = -1;
			//short.TryParse(estado_dian, out cod_estado_dian);

			short cod_estado_recibo = -1;
			short.TryParse(estado_recibo, out cod_estado_recibo);

			if (string.IsNullOrWhiteSpace(numero_documento))
				numero_documento = "*";
			if (string.IsNullOrWhiteSpace(codigo_adquiriente))
				codigo_adquiriente = "*";
			//if (string.IsNullOrWhiteSpace(estado_dian))
			//    estado_dian = "*";
			if (string.IsNullOrWhiteSpace(estado_recibo))
				estado_recibo = "*";


			List<string> LstResolucion = Coleccion.ConvertirLista(Resolucion);


			List<TblDocumentos> documentos = (from datos in context.TblDocumentos
											  join obligado in context.TblEmpresas on datos.StrEmpresaFacturador equals obligado.StrIdentificacion
											  join adquiriente in context.TblEmpresas on datos.StrEmpresaAdquiriente equals adquiriente.StrIdentificacion

											  where (obligado.StrIdentificacion.Equals(codigo_facturador) || codigo_facturador.Equals("*"))
											  && (datos.IntNumero == num_doc || numero_documento.Equals("*"))
											  && (adquiriente.StrIdentificacion.Equals(codigo_adquiriente) || codigo_adquiriente.Equals("*"))

											  && (estado_dian.Contains(datos.IntIdEstado.ToString()))

											  && (datos.IntAdquirienteRecibo == cod_estado_recibo || estado_recibo.Equals("*"))

											  && (LstResolucion.Contains(datos.StrNumResolucion.ToString()) || Resolucion.Equals("*"))


											  && ((datos.DatFechaDocumento >= fecha_inicio && datos.DatFechaDocumento <= fecha_fin) || tipo_fecha == 2)


											  && ((datos.DatAdquirienteFechaRecibo >= fecha_inicio && datos.DatAdquirienteFechaRecibo <= fecha_fin) || tipo_fecha == 1)




											  orderby datos.IntNumero descending
											  select datos).ToList();

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

			DateTime DiasTranscurridos = DateTime.Now.Subtract(new TimeSpan(0, CantDiasTacito(codigo_facturador), 0, 0));


			List<TblDocumentos> documentos = (from datos in context.TblDocumentos
											  join obligado in context.TblEmpresas on datos.StrEmpresaFacturador equals obligado.StrIdentificacion
											  join adquiriente in context.TblEmpresas on datos.StrEmpresaAdquiriente equals adquiriente.StrIdentificacion

											  //let d = SqlFunctions.DateAdd("hh", -datos.TblEmpresasFacturador.IntAcuseTacito.Value, DateTime.Now)

											  where (obligado.StrIdentificacion.Equals(codigo_facturador) || codigo_facturador.Equals("*"))
											  && (datos.IntNumero == num_doc || numero_documento.Equals("*"))
											  && (adquiriente.StrIdentificacion.Equals(codigo_adquiriente) || codigo_adquiriente.Equals("*"))

											  && (datos.IntAdquirienteRecibo.Equals(0))

											  && (datos.DatFechaIngreso <= SqlFunctions.DateAdd("hh", -datos.TblEmpresasFacturador.IntAcuseTacito.Value, DateTime.Now) && datos.TblEmpresasFacturador.IntAcuseTacito.Value > 0)

											  orderby datos.IntNumero descending
											  select datos).ToList();


			return documentos;
		}




		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Retorna la cantidad de dias que posee un facturador para dias Tacito del acuse
		/// </summary>
		/// <param name="CodFacturador"></param>
		/// <returns></returns>
		public short CantDiasTacito(string CodFacturador)
		{
			var numero = (from d in context.TblEmpresas
						  where d.StrIdentificacion.Equals(CodFacturador)
						  select d.IntAcuseTacito).FirstOrDefault();
			if (numero != null)
			{
				return numero.Value;
			}

			return 72;
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
		public List<TblDocumentos> ObtenerAdmin(string codigo_facturador, string numero_documento, string codigo_adquiriente, string estado_dian, string estado_recibo, DateTime fecha_inicio, DateTime fecha_fin, int TipoDocumento)
		{

			if (estado_dian == null || estado_dian == "")
				estado_dian = Coleccion.ConvertirString(Ctl_MaestrosEnum.ListaEnum(0, "publico")) + "," + Coleccion.ConvertirString(Ctl_MaestrosEnum.ListaEnum(0, "privado"));

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



			List<TblDocumentos> documentos = (from datos in context.TblDocumentos
											  join obligado in context.TblEmpresas on datos.StrEmpresaFacturador equals obligado.StrIdentificacion
											  join adquiriente in context.TblEmpresas on datos.StrEmpresaAdquiriente equals adquiriente.StrIdentificacion
											  where (obligado.StrIdentificacion.Equals(codigo_facturador) || codigo_facturador.Equals("*"))
											&& (datos.IntNumero == num_doc || numero_documento.Equals("*"))
											&& (adquiriente.StrIdentificacion.Equals(codigo_adquiriente) || codigo_adquiriente.Equals("*"))
											&& (estado_dian.Contains(datos.IntIdEstado.ToString()))
											&& (datos.IntAdquirienteRecibo == cod_estado_recibo || estado_recibo.Equals("*"))
											&& (datos.DatFechaDocumento >= fecha_inicio && datos.DatFechaDocumento <= fecha_fin)
											&& (datos.IntDocTipo.Equals(TipoDocumento) || TipoDocumento == 0)
											  orderby datos.IntNumero descending
											  select datos).ToList();

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
		/// Obtiene un documento por id se seguridad.
		/// </summary>
		/// <param name="id_seguridad"></param>
		/// <returns></returns>
		public TblDocumentos DocumentoPorIdSeguridad(System.Guid id_seguridad)
		{

			var respuesta = (from datos in context.TblDocumentos
							 where datos.StrIdSeguridad.Equals(id_seguridad)
							 select datos
							 ).FirstOrDefault();

			return respuesta;

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
		public List<TblDocumentos> ActualizarRespuestaAcuse(System.Guid id_seguridad, short estado, string motivo_rechazo)
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
				doc.IntIdEstado = 9;

				// obtiene los datos del facturador electrónico
				TblEmpresas facturador = ctl_empresa.Obtener(doc.StrEmpresaFacturador);

				//obtiene los datos del proveedor del facturador
				TblEmpresas proveedor_emisor = ctl_empresa.Obtener(Constantes.NitResolucionsinPrefijo);

				// obtiene los datos del adquiriente
				TblEmpresas adquiriente = ctl_empresa.Obtener(doc.StrEmpresaAdquiriente);

				//obtiene los datos del proveedor del adquirirente
				//TblEmpresas proveedor_receptor = ctl_empresa.Obtener("811021438");

				FacturaE_Documento resultado = Ctl_Documento.ConvertirAcuse(doc, facturador, adquiriente, proveedor_emisor, proveedor_emisor, estado, motivo_rechazo);

				try
				{   // envía el correo del acuse de recibo al facturador electrónico
					Ctl_EnvioCorreos email = new Ctl_EnvioCorreos();
					email.RespuestaAcuse(doc, facturador, adquiriente, resultado.RutaArchivosProceso);
					doc.IntIdEstado = 99;
				}
				catch (Exception) { }

				doc.DatFechaActualizaEstado = Fecha.GetFecha();
				doc.StrUrlAcuseUbl = resultado.RutaArchivosProceso;
				doc = Actualizar(doc);

				doc.TblEmpresasFacturador = facturador;
				doc.TblEmpresasAdquiriente = adquiriente;
				retorno.Add(doc);

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
			doc_acuse.IdAcuse = Guid.NewGuid().ToString();
			doc_acuse.IdSeguridad = doc.StrIdSeguridad.ToString();
			doc_acuse.Documento = doc.IntNumero;
			doc_acuse.MvoRespuesta = motivo_rechazo;
			doc_acuse.Fecha = Convert.ToDateTime(doc.DatAdquirienteFechaRecibo);
			doc_acuse.CodigoRespuesta = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.Enumerables.ResponseCode>(estado));
			doc_acuse.TipoDocumento = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.Enumerables.DocumentType>(doc.IntDocTipo));
			doc_acuse.DatosAdquiriente = Ctl_Empresa.Convertir(adquiriente);
			doc_acuse.DatosObligado = Ctl_Empresa.Convertir(facturador);

			//Convierte el objeto en archivo XML-UBL
			FacturaE_Documento resultado = AcuseReciboXML.CrearDocumento(doc_acuse, proveedor_emisor, proveedor_emisor, Enumeracion.GetEnumObjectByValue<TipoDocumento>(doc.IntDocTipo));
			resultado.IdSeguridadTercero = facturador.StrIdSeguridad;
			resultado.IdSeguridadDocumento = doc.StrIdSeguridad;
			resultado.IdSeguridadPeticion = new Guid(doc_acuse.IdAcuse);
			resultado.DocumentoTipo = TipoDocumento.AcuseRecibo;

            // ruta física del xml
            string carpeta_xml = string.Format("{0}\\{1}\\{2}",plataforma_datos.RutaDmsFisica,Constantes.CarpetaFacturaElectronica, resultado.IdSeguridadTercero.ToString());
			carpeta_xml = string.Format(@"{0}\{1}", carpeta_xml, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaXmlAcuse);

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
				tbl_documento.DatFechaDocumento = documento_obj.Fecha;
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
		/// <param name="tipo_doc"></param>
		/// <returns></returns>
		public static object ConvertirServicio(TblDocumentos objetoBd)
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

				serializacion = new XmlSerializer(typeof(InvoiceType));

				InvoiceType conversion = (InvoiceType)serializacion.Deserialize(xml_reader);

				documento_obj.DatosFactura = FacturaXML.Convertir(conversion);
				documento_obj.DatosFactura.CodigoRegistro = objetoBd.StrIdSeguridad.ToString();

			}
			else if (objetoBd.IntDocTipo == TipoDocumento.NotaCredito.GetHashCode())
			{

				NotaCreditoConsulta nota_credito = new NotaCreditoConsulta();

				documento_obj = nota_credito;

				serializacion = new XmlSerializer(typeof(CreditNoteType));

				CreditNoteType conversion = (CreditNoteType)serializacion.Deserialize(xml_reader);

				documento_obj.DatosNotaCredito = NotaCreditoXML.Convertir(conversion);
				documento_obj.DatosNotaCredito.CodigoRegistro = objetoBd.StrIdSeguridad.ToString();
			}
			else if (objetoBd.IntDocTipo == TipoDocumento.NotaDebito.GetHashCode())
			{

				NotaDebitoConsulta nota_debito = new NotaDebitoConsulta();

				documento_obj = nota_debito;

				serializacion = new XmlSerializer(typeof(DebitNoteType));

				DebitNoteType conversion = (DebitNoteType)serializacion.Deserialize(xml_reader);

				documento_obj.DatosNotaDebito = NotaDebitoXML.Convertir(conversion);
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

			//Obtiene la carpeta donde quedo la consulta de la DIAN
			TipoDocumento doc_tipo = Enumeracion.GetEnumObjectByValue<TipoDocumento>(objetoBd.IntDocTipo);

			// Nombre del archivo Xml 
			string archivo_xml = string.Format(@"{0}.xml", NombramientoArchivo.ObtenerXml(objetoBd.IntNumero.ToString(), objetoBd.StrEmpresaFacturador, doc_tipo, objetoBd.StrPrefijo));

			PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

			//Url publica de la respuesta de la DIAN en xml
			string url_ppal_respuesta = LibreriaGlobalHGInet.Dms.ObtenerUrlPrincipal(plataforma_datos.RutaPublica, objetoBd.TblEmpresasFacturador.StrIdSeguridad.ToString());

			string ruta_xml = string.Format(@"{0}{1}/{2}", url_ppal_respuesta, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEConsultaDian, archivo_xml);

			//Obtiene el archivo en la ruta Http
			ArchivoUrl archivo_consulta = Archivo.Obtener(ruta_xml);

			//Valida que el archivo si existe de lo contrario vuelve a consultar el documento en la DIAN.
			if (archivo_consulta == null)
			{

				DocumentoRespuesta consulta_dian = new DocumentoRespuesta();

				consulta_dian = Ctl_Documentos.Consultar(objetoBd, objetoBd.TblEmpresasFacturador, ref consulta_dian);

				ruta_xml = consulta_dian.EstadoDian.UrlXmlRespuesta;

			}

			//asigna la ruta que tiene el archivo donde se guardo la consulta que se hizo a la DIAN
			documento_obj.EstadoDian = new RespuestaDian();
			documento_obj.EstadoDian.UrlXmlRespuesta = ruta_xml;

			//Construye la url publica para el acuse de recibo del documento
			PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;
			documento_obj.UrlAcuse = string.Format("{0}{1}", plataforma.RutaPublica, Constantes.PaginaAcuseRecibo.Replace("{id_seguridad}", objetoBd.StrIdSeguridad.ToString()));

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

				DocumentoRespuesta obj_documento = new DocumentoRespuesta();

				obj_documento.Aceptacion = respuesta.IntAdquirienteRecibo;
				obj_documento.CodigoRegistro = respuesta.StrObligadoIdRegistro;
				obj_documento.Cufe = respuesta.StrCufe;
				obj_documento.DocumentoTipo = respuesta.IntDocTipo;
				obj_documento.Documento = respuesta.IntNumero;
				obj_documento.FechaRecepcion = respuesta.DatFechaIngreso;
				obj_documento.FechaUltimoProceso = respuesta.DatFechaActualizaEstado;
				obj_documento.IdDocumento = respuesta.StrIdSeguridad.ToString();
				obj_documento.Identificacion = respuesta.TblEmpresasAdquiriente.StrIdentificacion;
				obj_documento.IdProceso = respuesta.IntIdEstado;
				obj_documento.MotivoRechazo = respuesta.StrAdquirienteMvoRechazo;
				obj_documento.NumeroResolucion = respuesta.TblEmpresasResoluciones.StrNumResolucion;
				obj_documento.Prefijo = respuesta.TblEmpresasResoluciones.StrPrefijo;
				obj_documento.ProcesoFinalizado = 0;
				obj_documento.UrlPdf = respuesta.StrUrlArchivoPdf;
				obj_documento.UrlXmlUbl = respuesta.StrUrlArchivoUbl;


				// obj_documento.DescripcionProceso

				switch (respuesta.IntIdEstado)
				{
					case 1:
						obj_documento.DescripcionProceso = "Recepción - Información del documento.";
						break;

					case 2:
						obj_documento.DescripcionProceso = "Valida la información del documento.";
						break;

					case 3:
						obj_documento.DescripcionProceso = "Genera información en estandar UBL.";
						break;

					case 4:
						obj_documento.DescripcionProceso = "Almacena el archivo XML con la información en estandar UBL.";
						break;

					case 5:
						obj_documento.DescripcionProceso = "Firma el archivo XML con la información en estandar UBL.";
						break;

					case 6:
						obj_documento.DescripcionProceso = "Compresión del archivo XML firmado con la información en estandar UBL.";
						break;

					case 7:
						obj_documento.DescripcionProceso = "Envío del archivo ZIP con el XML firmado a la DIAN.";
						break;

					case 8:
						obj_documento.DescripcionProceso = "Envío correo adquiriente";
						break;

					case 9:
						obj_documento.DescripcionProceso = "Recepción acuse de recibo del Adquiriente";
						break;

					case 10:
						obj_documento.DescripcionProceso = "Envío correo acuse de recibo al facturador";
						break;


					case 90:
						obj_documento.DescripcionProceso = "Finalizado con inconsistencias.";
						obj_documento.ProcesoFinalizado = 1;
						break;

					case 99:
						obj_documento.DescripcionProceso = "Termina proceso";
						obj_documento.ProcesoFinalizado = 1;
						break;

					default:
						obj_documento.DescripcionProceso = "Proceso desconocido";
						break;
				}

				if (respuesta.IntIdEstado == 1)
					obj_documento.DescripcionProceso = "Recepción - Información del documento.";

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

			var respuesta = (from datos in context.TblDocumentos
							 join empresa in context.TblEmpresas on datos.StrEmpresaAdquiriente equals empresa.StrIdentificacion
							 where (estado_recibo.Contains(datos.IntIdEstado.ToString()))
							 && datos.DatFechaIngreso < SqlFunctions.DateAdd("ss", 15, DateTime.Now)

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
		public bool ReenviarRespuestaAcuse(System.Guid id_seguridad, string mail)
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
				TblEmpresas proveedor_emisor = ctl_empresa.Obtener(Constantes.NitResolucionsinPrefijo);

				// obtiene los datos del adquiriente
				TblEmpresas adquiriente = ctl_empresa.Obtener(doc.StrEmpresaAdquiriente);

				//obtiene los datos del proveedor del facturador
				//TblEmpresas proveedor_receptor = ctl_empresa.Obtener("811021438");

				string ruta_acuse = string.Empty;

				//Valida si ya tiene 
				if (string.IsNullOrEmpty(doc.StrUrlAcuseUbl))
				{
					FacturaE_Documento resultado = Ctl_Documento.ConvertirAcuse(doc, facturador, adquiriente, proveedor_emisor, proveedor_emisor, doc.IntAdquirienteRecibo, doc.StrAdquirienteMvoRechazo);
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
					email.RespuestaAcuse(doc, facturador, adquiriente, ruta_acuse, mail);

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


			List<TblDocumentos> respuesta = (from datos in context.TblDocumentos
											 join empresa in context.TblEmpresas on datos.StrEmpresaAdquiriente equals empresa.StrIdentificacion
											 where datos.StrEmpresaFacturador.Equals(codigo_facturador)
											 && (datos.StrIdSeguridad.ToString().Contains(IdSeguridad) || IdSeguridad.Equals("*"))
											 && (numero_resolucion.Contains(datos.StrNumResolucion.ToString()) || numero_resolucion.Equals("*"))
											 && (datos.IntNumero == numero_documento || numero_documento == null)
											 orderby datos.DatFechaIngreso descending
											 select datos).ToList();
			return respuesta;
		}
		#endregion

		#region Generar Acuse Tacito
		public bool GenerarAcuseTacito(List<TblDocumentos> lista)
		{
			//Convierte los registros de base de datos a objeto de servicio y los añade a la lista de retorno
			foreach (TblDocumentos item in lista)
			{
				item.IntAdquirienteRecibo = 3;
				item.StrAdquirienteMvoRechazo = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.AdquirienteRecibo>(3));
				item.DatAdquirienteFechaRecibo = Fecha.GetFecha();
				item.IntIdEstado = 99;
				item.DatFechaActualizaEstado = Fecha.GetFecha();
				this.Edit(item);
			}

			return true;
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
									   where (doc.IntIdEstado == DocPendiente || doc.IntIdEstado == AcusePendiente)
									   && doc.StrProveedorReceptor.Equals(IdentificacionProveedor) || IdentificacionProveedor.Equals("*")
									   select doc).ToList();


			return Doc;
		}

		public List<TblDocumentos> ObtenerAcusePendienteRecepcion(string IdentificacionProveedor)
		{
			try
			{
				int EnvioEmailAcuse = ProcesoEstado.EnvioExitosoProveedor.GetHashCode();

				List<TblDocumentos> documentos = (from documento in context.TblDocumentos
												  where documento.IntIdEstado == EnvioEmailAcuse
												  && documento.StrIdInteroperabilidad != null
												  && !documento.StrProveedorReceptor.Equals(Constantes.NitResolucionsinPrefijo)
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

	}

}


