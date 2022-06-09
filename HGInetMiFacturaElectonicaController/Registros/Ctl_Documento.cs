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
using DevExpress.XtraReports.Web.ReportDesigner.Native.Services;
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
using HGInetMiFacturaElectonicaData.Objetos;
using HGInetMiFacturaElectonicaData.ModeloServicio.Respuestas;
using HGInetMiFacturaElectonicaController.PagosElectronicos;
using System.IO.Compression;
using HGInetUBL;
using HGInetDIANServicios;
using HGInetUtilidadAzure.Almacenamiento;

namespace HGInetMiFacturaElectonicaController.Registros
{
	public partial class Ctl_Documento : BaseObject<TblDocumentos>
	{
		#region Constructores 

		public Ctl_Documento() : base(new ModeloAutenticacion()) { }

		#endregion


		#region Prueba de Acuse Tacito
		public TblDocumentos COnsultaPorDocumento(string identificacion, int documento)
		{

			try
			{
				TblEmpresas empresa = new TblEmpresas();

				Ctl_Empresa _empresa = new Ctl_Empresa();

				empresa = _empresa.Obtener(identificacion, false);

				DateTime FechaActual = Fecha.GetFecha();

				int estado_error = ProcesoEstado.FinalizacionErrorDian.GetHashCode();

				int Enviomail = ProcesoEstado.EnvioZip.GetHashCode();

				TblDocumentos docs = (from datos in context.TblDocumentos.AsNoTracking()
									  where datos.IntAdquirienteRecibo.Equals(0) && datos.IntIdEstado > Enviomail && datos.IntIdEstado < estado_error
											&& datos.StrEmpresaFacturador == identificacion
											&& (((datos.DatFechaIngreso <= SqlFunctions.DateAdd("hh", -empresa.IntAcuseTacito.Value, FechaActual)
											&& (datos.IntNumero == documento)
											&& empresa.IntAcuseTacito.Value > 0)))
									  orderby datos.IntNumero descending
									  select datos).FirstOrDefault();
				return docs;
			}
			catch (Exception)
			{

				throw;
			}
		}
		#endregion


		#region HGIpay



		/// <summary>
		/// Obtiene los documentos de un facturador que estan pendiente por pago webpart
		/// </summary>
		/// <param name="identificacion_adquiente">Nit Facturador</param>
		/// <param name="identificacion_facturador">Identificación del adquiriente</param>
		/// <param name="numero_documento">Documento</param>
		/// <returns>List<ObjDocumentos></returns>
		public List<ObjDocumentos> ConsultarPagosFueraPlataforma(string identificacion_adquiente, string identificacion_facturador, string numero_documento)
		{


			if (string.IsNullOrEmpty(numero_documento))
				numero_documento = "*";

			if (numero_documento == "null")
				numero_documento = "*";

			long num_doc = -1;
			long.TryParse(numero_documento, out num_doc);


			int ErrorDian = ProcesoEstado.FinalizacionErrorDian.GetHashCode();

			int Categoria = CategoriaEstado.ValidadoDian.GetHashCode();

			List<ObjDocumentos> respuesta = new List<ObjDocumentos>();


			//context.Configuration.LazyLoadingEnabled = false;

			respuesta = (from datos in context.TblDocumentos//.Include("TblEmpresasFacturador").AsNoTracking()
						 where (datos.StrEmpresaAdquiriente.Equals(identificacion_adquiente))
							&& (datos.StrEmpresaFacturador.Equals(identificacion_facturador))
							&& (datos.IntNumero == num_doc || numero_documento.Equals("*"))
							&& (datos.TblEmpresasFacturador.IntEnvioMailRecepcion == true || (datos.TblEmpresasFacturador.IntEnvioMailRecepcion == false && datos.IdCategoriaEstado == Categoria))
						 orderby datos.IntNumero descending
						 select new ObjDocumentos
						 {
							 IdFacturador = datos.TblEmpresasFacturador.StrIdentificacion,
							 Facturador = datos.TblEmpresasFacturador.StrRazonSocial,
							 Prefijo = datos.StrPrefijo,
							 NumeroDocumento = datos.StrPrefijo + datos.IntNumero.ToString(),
							 DatFechaIngreso = datos.DatFechaIngreso,
							 DatFechaDocumento = datos.DatFechaDocumento,
							 DatFechaVencDocumento = datos.DatFechaVencDocumento,
							 IntVlrTotal = (datos.IntDocTipo == 3) ? -datos.IntVlrTotal : datos.IntVlrTotal,
							 IntSubTotal = (datos.IntDocTipo == 3) ? -datos.IntValorSubtotal : datos.IntValorSubtotal,
							 IntNeto = (datos.IntDocTipo == 3) ? -datos.IntValorNeto : datos.IntValorNeto,
							 EstadoFactura = datos.IntIdEstado,
							 EstadoCategoria = (Int16)datos.IdCategoriaEstado,
							 EstadoAcuse = datos.IntAdquirienteRecibo,
							 MotivoRechazo = datos.StrAdquirienteMvoRechazo,
							 StrAdquirienteMvoRechazo = datos.StrAdquirienteMvoRechazo,
							 IdentificacionAdquiriente = datos.TblEmpresasAdquiriente.StrIdentificacion,
							 NombreAdquiriente = datos.TblEmpresasAdquiriente.StrRazonSocial,
							 MailAdquiriente = datos.TblEmpresasAdquiriente.StrMailAdmin,
							 Xml = datos.StrUrlArchivoUbl,
							 Pdf = datos.StrUrlArchivoPdf,
							 StrIdSeguridad = datos.StrIdSeguridad,
							 tipodoc = datos.IntDocTipo,
							 zip = datos.StrUrlAnexo,
							 RutaServDian = datos.StrUrlArchivoUbl,
							 XmlAcuse = datos.StrUrlAcuseUbl,
							 permiteenvio = (Int16)datos.IdCategoriaEstado,
							 IntAdquirienteRecibo = datos.IntAdquirienteRecibo,
							 Estado = datos.IdCategoriaEstado,
							 EstadoEnvioMail = datos.IntEstadoEnvio.ToString(),
							 MensajeEnvio = datos.IntMensajeEnvio.ToString(),
							 EnvioMail = datos.IntEstadoEnvio,
							 poseeIdComercio = (datos.TblEmpresasFacturador.IntManejaPagoE) ? (datos.IntIdEstado != 90) ? 1 : 0 : 0,
							 FacturaCancelada = (datos.IntIdEstadoPago.Value != null) ? datos.IntIdEstadoPago.Value : (short)1,
							 PagosParciales = (string.IsNullOrEmpty(datos.TblEmpresasResoluciones.ComercioConfigId.ToString())) ? (datos.TblEmpresasFacturador.IntManejaPagoE) ? 1 : 0 : datos.TblEmpresasResoluciones.PermiteParciales,
							 IdComercio = (string.IsNullOrEmpty(datos.TblEmpresasResoluciones.ComercioConfigId.ToString())) ? datos.TblEmpresasFacturador.ComercioConfigId.ToString() : datos.TblEmpresasResoluciones.ComercioConfigId.ToString(),
							 IntValorPagar = datos.IntValorPagar
						 }).ToList();



			return respuesta;

		}

		/// <summary>
		/// Obtener Documentos  de adquiriente para ACH
		/// </summary>
		/// <param name="identificacion_adquiente">identificación de adquiriente</param>
		/// <param name="identificacion_facturador">identificación de facturador</param>
		/// <param name="numero_documento">numero de documento</param>
		/// <param name="estado_recibo">Estado</param>
		/// <param name="fecha_inicio">Fecha Inicial</param>
		/// <param name="fecha_fin">Fecha Final</param>
		/// <param name="tipo_filtro_fecha">Tipo de Fecha</param>
		/// <returns></returns>
		public List<ObjDocumentos> HGIpayObtenerPorFechasAdquiriente(string identificacion_adquiente, string identificacion_facturador, string numero_documento, string estado_recibo, DateTime fecha_inicio, DateTime fecha_fin, int tipo_filtro_fecha)
		{

			fecha_inicio = fecha_inicio.Date;
			fecha_fin = new DateTime(fecha_fin.Year, fecha_fin.Month, fecha_fin.Day, 23, 59, 59, 999);

			if (string.IsNullOrEmpty(numero_documento))
				numero_documento = "*";

			if (numero_documento == "null")
				numero_documento = "*";

			long num_doc = -1;
			long.TryParse(numero_documento, out num_doc);

			short cod_estado_recibo = -1;
			short.TryParse(estado_recibo, out cod_estado_recibo);


			if (string.IsNullOrWhiteSpace(estado_recibo))
				estado_recibo = "*";

			int ErrorDian = ProcesoEstado.FinalizacionErrorDian.GetHashCode();

			int Categoria = CategoriaEstado.ValidadoDian.GetHashCode();

			List<ObjDocumentos> respuesta = new List<ObjDocumentos>();


			if (numero_documento.Equals("*"))
			{
				//context.Configuration.LazyLoadingEnabled = false;

				//respuesta = (from datos in context.TblDocumentos.Include("TblEmpresasFacturador").AsNoTracking()
				respuesta = (from datos in context.TblDocumentos.Include("TblEmpresasFacturador").Include("TblEmpresasResoluciones").AsNoTracking()
							 where (datos.StrEmpresaAdquiriente.Equals(identificacion_adquiente))
				&& (datos.StrEmpresaFacturador.Equals(identificacion_facturador))
						   && (datos.IntAdquirienteRecibo == cod_estado_recibo || estado_recibo.Equals("*"))
						   && ((datos.DatFechaIngreso >= fecha_inicio && datos.DatFechaIngreso <= fecha_fin) || tipo_filtro_fecha == 2)
						   && ((datos.DatFechaDocumento >= fecha_inicio && datos.DatFechaDocumento <= fecha_fin) || tipo_filtro_fecha == 1)
						   && (datos.TblEmpresasFacturador.IntEnvioMailRecepcion == true || (datos.TblEmpresasFacturador.IntEnvioMailRecepcion == false && datos.IdCategoriaEstado == Categoria))
						   && (datos.IdCategoriaEstado == Categoria)
							 orderby datos.IntNumero descending
							 select new ObjDocumentos
							 {
								 IdFacturador = datos.TblEmpresasFacturador.StrIdentificacion,
								 Facturador = datos.TblEmpresasFacturador.StrRazonSocial,
								 Prefijo = datos.StrPrefijo,
								 NumeroDocumento = datos.StrPrefijo + datos.IntNumero.ToString(),
								 DatFechaIngreso = datos.DatFechaIngreso,
								 DatFechaDocumento = datos.DatFechaDocumento,
								 DatFechaVencDocumento = datos.DatFechaVencDocumento,
								 IntVlrTotal = (datos.IntDocTipo == 3) ? -datos.IntVlrTotal : datos.IntVlrTotal,
								 IntSubTotal = (datos.IntDocTipo == 3) ? -datos.IntValorSubtotal : datos.IntValorSubtotal,
								 IntNeto = (datos.IntDocTipo == 3) ? -datos.IntValorNeto : datos.IntValorNeto,
								 EstadoFactura = datos.IntIdEstado,
								 EstadoCategoria = (Int16)datos.IdCategoriaEstado,
								 EstadoAcuse = datos.IntAdquirienteRecibo,
								 MotivoRechazo = datos.StrAdquirienteMvoRechazo,
								 StrAdquirienteMvoRechazo = datos.StrAdquirienteMvoRechazo,
								 IdentificacionAdquiriente = datos.TblEmpresasAdquiriente.StrIdentificacion,
								 NombreAdquiriente = datos.TblEmpresasAdquiriente.StrRazonSocial,
								 MailAdquiriente = datos.TblEmpresasAdquiriente.StrMailAdmin,
								 Xml = datos.StrUrlArchivoUbl,
								 Pdf = datos.StrUrlArchivoPdf,
								 StrIdSeguridad = datos.StrIdSeguridad,
								 tipodoc = datos.IntDocTipo,
								 zip = datos.StrUrlAnexo,
								 RutaServDian = datos.StrUrlArchivoUbl,
								 XmlAcuse = datos.StrUrlAcuseUbl,
								 permiteenvio = (Int16)datos.IdCategoriaEstado,
								 IntAdquirienteRecibo = datos.IntAdquirienteRecibo,
								 Estado = datos.IdCategoriaEstado,
								 EstadoEnvioMail = datos.IntEstadoEnvio.ToString(),
								 MensajeEnvio = datos.IntMensajeEnvio.ToString(),
								 EnvioMail = datos.IntEstadoEnvio,
								 poseeIdComercio = (datos.TblEmpresasFacturador.IntManejaPagoE) ? (datos.IntIdEstado != 90) ? 1 : 0 : 0,
								 //FacturaCancelada = datos.IntIdEstadoPago.Value,
								 //PagosParciales = (datos.TblEmpresasFacturador.IntManejaPagoE) ? 1 : 0,
								 PagosParciales = (string.IsNullOrEmpty(datos.TblEmpresasResoluciones.ComercioConfigId.ToString())) ? (datos.TblEmpresasFacturador.IntManejaPagoE) ? 1 : 0 : datos.TblEmpresasResoluciones.PermiteParciales,
								 IdComercio = (string.IsNullOrEmpty(datos.TblEmpresasResoluciones.ComercioConfigId.ToString())) ? datos.TblEmpresasFacturador.ComercioConfigId.ToString() : datos.TblEmpresasResoluciones.ComercioConfigId.ToString(),
								 DescripComercio = (string.IsNullOrEmpty(datos.TblEmpresasResoluciones.ComercioConfigDescrip.ToString())) ? datos.TblEmpresasFacturador.ComercioConfigDescrip.ToString() : datos.TblEmpresasResoluciones.ComercioConfigDescrip.ToString(),
								 IntValorPagar = datos.IntValorPagar
							 }).ToList();
			}
			else
			{
				var listaDocumetos = Coleccion.ConvertirStringlong(numero_documento);

				context.Configuration.LazyLoadingEnabled = false;

				respuesta = (from datos in context.TblDocumentos.Include("TblEmpresasFacturador").AsNoTracking()
							 where (datos.StrEmpresaAdquiriente.Equals(identificacion_adquiente))
								&& (datos.StrEmpresaFacturador.Equals(identificacion_facturador))
								&& (listaDocumetos.Contains(datos.IntNumero))
								&& (datos.TblEmpresasFacturador.IntEnvioMailRecepcion == true || (datos.TblEmpresasFacturador.IntEnvioMailRecepcion == false && datos.IdCategoriaEstado == Categoria))
							 orderby datos.IntNumero descending
							 select new ObjDocumentos
							 {
								 IdFacturador = datos.TblEmpresasFacturador.StrIdentificacion,
								 Facturador = datos.TblEmpresasFacturador.StrRazonSocial,
								 Prefijo = datos.StrPrefijo,
								 NumeroDocumento = datos.StrPrefijo + datos.IntNumero.ToString(),
								 DatFechaIngreso = datos.DatFechaIngreso,
								 DatFechaDocumento = datos.DatFechaDocumento,
								 DatFechaVencDocumento = datos.DatFechaVencDocumento,
								 IntVlrTotal = (datos.IntDocTipo == 3) ? -datos.IntVlrTotal : datos.IntVlrTotal,
								 IntSubTotal = (datos.IntDocTipo == 3) ? -datos.IntValorSubtotal : datos.IntValorSubtotal,
								 IntNeto = (datos.IntDocTipo == 3) ? -datos.IntValorNeto : datos.IntValorNeto,
								 EstadoFactura = datos.IntIdEstado,
								 EstadoCategoria = (Int16)datos.IdCategoriaEstado,
								 EstadoAcuse = datos.IntAdquirienteRecibo,
								 MotivoRechazo = datos.StrAdquirienteMvoRechazo,
								 StrAdquirienteMvoRechazo = datos.StrAdquirienteMvoRechazo,
								 IdentificacionAdquiriente = datos.TblEmpresasAdquiriente.StrIdentificacion,
								 NombreAdquiriente = datos.TblEmpresasAdquiriente.StrRazonSocial,
								 MailAdquiriente = datos.TblEmpresasAdquiriente.StrMailAdmin,
								 Xml = datos.StrUrlArchivoUbl,
								 Pdf = datos.StrUrlArchivoPdf,
								 StrIdSeguridad = datos.StrIdSeguridad,
								 tipodoc = datos.IntDocTipo,
								 zip = datos.StrUrlAnexo,
								 RutaServDian = datos.StrUrlArchivoUbl,
								 XmlAcuse = datos.StrUrlAcuseUbl,
								 permiteenvio = (Int16)datos.IdCategoriaEstado,
								 IntAdquirienteRecibo = datos.IntAdquirienteRecibo,
								 Estado = datos.IdCategoriaEstado,
								 EstadoEnvioMail = datos.IntEstadoEnvio.ToString(),
								 MensajeEnvio = datos.IntMensajeEnvio.ToString(),
								 EnvioMail = datos.IntEstadoEnvio,
								 poseeIdComercio = (datos.TblEmpresasFacturador.IntManejaPagoE) ? (datos.IntIdEstado != 90) ? 1 : 0 : 0,
								 FacturaCancelada = datos.IntIdEstado,
								 PagosParciales = (datos.TblEmpresasFacturador.IntManejaPagoE) ? 1 : 0,
								 IntValorPagar = datos.IntValorPagar
							 }).ToList();

			}

			return respuesta;
		}
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

		public TblDocumentos Obtener(string identificacion_obligado, long numero_documeto, string prefijo, int tipo_doc = 0)
		{
			try
			{

				context.Configuration.LazyLoadingEnabled = false;

				TblDocumentos documento = new TblDocumentos();

				if (tipo_doc.Equals(0))
				{
					documento = (from documentos in context.TblDocumentos.Include("TblEmpresasAdquiriente").Include("TblEmpresasFacturador").Include("TblEmpresasResoluciones")
								 where (documentos.IntNumero == numero_documeto)
									   && (documentos.TblEmpresasFacturador.StrIdentificacion.Equals(identificacion_obligado)
										   && documentos.TblEmpresasResoluciones.StrPrefijo.Equals(prefijo))
								 select documentos).FirstOrDefault();
				}
				else
				{

					documento = (from documentos in context.TblDocumentos.Include("TblEmpresasAdquiriente").Include("TblEmpresasFacturador").Include("TblEmpresasResoluciones")
								 where (documentos.IntNumero == numero_documeto)
									   && (documentos.TblEmpresasFacturador.StrIdentificacion.Equals(identificacion_obligado)
										   && documentos.TblEmpresasResoluciones.StrPrefijo.Equals(prefijo))
									   && (documentos.IntDocTipo == tipo_doc)
								 select documentos).FirstOrDefault();
				}





				return documento;


			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		public TblDocumentos ObtenerParaPrueba(string identificacion_obligado, string numero_documento, string prefijo)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(prefijo))
					prefijo = "*";

				if (string.IsNullOrWhiteSpace(numero_documento))
					numero_documento = "*";

				long num_doc = -1;
				long.TryParse(numero_documento, out num_doc);

				context.Configuration.LazyLoadingEnabled = false;
				TblDocumentos documento = (from documentos in context.TblDocumentos.Include("TblEmpresasAdquiriente").Include("TblEmpresasFacturador").Include("TblEmpresasResoluciones")
										   where (documentos.IntNumero == num_doc) || numero_documento.Equals("*")
										   && (documentos.TblEmpresasFacturador.StrIdentificacion.Equals(identificacion_obligado)
										   && documentos.TblEmpresasResoluciones.StrPrefijo.Equals(prefijo) || prefijo.Equals("*"))
										   select documentos).OrderByDescending(x => x.DatFechaDocumento).FirstOrDefault();

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
				if (tipo_documento < TipoDocumento.Factura.GetHashCode() || tipo_documento > TipoDocumento.NotaCredito.GetHashCode() && tipo_documento < TipoDocumento.Nomina.GetHashCode() || tipo_documento > TipoDocumento.NominaAjuste.GetHashCode())
					throw new ApplicationException("Tipo de documento inválido.");
				if (string.IsNullOrWhiteSpace(Numeros))
					throw new ApplicationException("Filtro por números inválido.");

				List<DocumentoRespuesta> lista_respuesta = new List<DocumentoRespuesta>();

				//Convierte Numeros en una lista.
				List<string> lista_documentos = Coleccion.ConvertirLista(Numeros);

				//Se restringe la consulta a un número de documentos específicos  NOVIEMBRE 16
				if (lista_documentos.Count > 100)
					throw new ApplicationException("Supera el número máximo de 100 registros por consulta.");

				context.Configuration.LazyLoadingEnabled = false;

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

					var lista = documento.ProcesarDocumentosSinLazy(List_id_seguridad);

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

				//Se restringe la consulta a un número de documentos específicos  NOVIEMBRE 16
				if (lista_documentos.Count > 100)
					throw new ApplicationException("Supera el número máximo de 100 registros por consulta.");

				context.Configuration.LazyLoadingEnabled = false;

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
		/// Obtiene los documentos por Código de Registros
		/// </summary>
		/// <param name="DataKey">Clave compuesta (serial + identificación obligado ) en formato Sha1</param>
		/// <param name="Identificacion">identificación obligado</param>

		/// <param name="CodigosRegistros">código de registro de los documentos (recibe varios códigos separados por coma)</param>
		/// <returns></returns>
		public List<PagoElectronicoRespuesta> ConsultaPorCodigoRegistro(string identificacion_obligado, string CodigosRegistros)
		{
			try
			{
				//Valida que los parametros sean correctos.
				if (string.IsNullOrWhiteSpace(identificacion_obligado))
					throw new ApplicationException("Número de identificación del obligado inválido.");
				if (string.IsNullOrWhiteSpace(CodigosRegistros))
					throw new ApplicationException("Filtro por números inválido.");

				List<PagoElectronicoRespuesta> lista_respuesta = new List<PagoElectronicoRespuesta>();

				//Convierte CodigoRegistros en una lista.
				List<string> lista_documentos = Coleccion.ConvertirLista(CodigosRegistros);

				//Se restringe la consulta a un número de documentos específicos  NOVIEMBRE 16
				if (lista_documentos.Count > 100)
					throw new ApplicationException("Supera el número máximo de 100 registros por consulta.");


				context.Configuration.LazyLoadingEnabled = false;

				var respuesta = from documento in context.TblDocumentos//.Include("TblPagosElectronicos")
																	   //join empresa in context.TblEmpresas on documento.StrEmpresaFacturador equals empresa.StrIdentificacion
																	   //where empresa.StrIdentificacion.Equals(identificacion_obligado)
								where documento.StrEmpresaFacturador.Equals(identificacion_obligado)
								&& (lista_documentos.Contains(documento.StrObligadoIdRegistro) || lista_documentos.Contains(documento.StrIdSeguridad.ToString()))
								select documento;

				//Convierte los registros de base de datos a objeto de servicio y los añade a la lista de retorno
				foreach (TblDocumentos item in respuesta)
				{
					lista_respuesta.Add(ConvertirPago(item));
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

				//if (FechaFinal < FechaInicial)
				//	throw new ApplicationException("Fecha final debe ser mayor o igual que la fecha inicial.");

				//long diferencia = Fecha.Diferencia(FechaInicial, FechaFinal, DateInterval.Day);

				//if (diferencia > 30)
				//	throw new ApplicationException("El rango de fechas no pueder ser mayor a 30 dias");

				FechaInicial = FechaInicial.Date;
				FechaFinal = new DateTime(FechaFinal.Year, FechaFinal.Month, FechaFinal.Day, 23, 59, 59, 999);

				List<DocumentoRespuesta> lista_respuesta = new List<DocumentoRespuesta>();

				context.Configuration.LazyLoadingEnabled = false;

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
		public List<ObjDocumentos> ObtenerPorFechasAdquiriente(string identificacion_facturador, string identificacion_adquiente, string numero_documento, string estado_recibo, DateTime fecha_inicio, DateTime fecha_fin, int tipo_filtro_fecha)
		{

			fecha_inicio = fecha_inicio.Date;
			fecha_fin = new DateTime(fecha_fin.Year, fecha_fin.Month, fecha_fin.Day, 23, 59, 59, 999);

			if (string.IsNullOrEmpty(numero_documento))
				numero_documento = "*";

			if (numero_documento == "null")
				numero_documento = "*";

			if (string.IsNullOrEmpty(identificacion_facturador))
			{
				identificacion_facturador = "*";
			}

			long num_doc = -1;
			long.TryParse(numero_documento, out num_doc);

			short cod_estado_recibo = -1;
			short.TryParse(estado_recibo, out cod_estado_recibo);


			if (string.IsNullOrWhiteSpace(estado_recibo))
				estado_recibo = "*";

			int ErrorDian = ProcesoEstado.FinalizacionErrorDian.GetHashCode();

			int Categoria = CategoriaEstado.ValidadoDian.GetHashCode();

			List<ObjDocumentos> respuesta = new List<ObjDocumentos>();


			if (numero_documento.Equals("*"))
			{
				context.Configuration.LazyLoadingEnabled = false;

				respuesta = (from datos in context.TblDocumentos.Include("TblEmpresasFacturador").AsNoTracking()//.Include("TblEmpresasResoluciones").AsNoTracking()
							 join empresa in context.TblEmpresas on datos.StrEmpresaAdquiriente equals empresa.StrIdentificacion
							 where (empresa.StrIdentificacion.Equals(identificacion_adquiente) || identificacion_adquiente.Equals("*"))
										&& (datos.StrEmpresaFacturador.Equals(identificacion_facturador) || identificacion_facturador.Equals("*"))
										&& (datos.IntAdquirienteRecibo == cod_estado_recibo || estado_recibo.Equals("*"))
										&& ((datos.DatFechaIngreso >= fecha_inicio && datos.DatFechaIngreso <= fecha_fin) || tipo_filtro_fecha == 2)
										&& ((datos.DatFechaDocumento >= fecha_inicio && datos.DatFechaDocumento <= fecha_fin) || tipo_filtro_fecha == 1)
										&& (datos.TblEmpresasFacturador.IntEnvioMailRecepcion == true || (datos.TblEmpresasFacturador.IntEnvioMailRecepcion == false && datos.IdCategoriaEstado == Categoria))
										&& (datos.IdCategoriaEstado == Categoria)
										&& (datos.IntDocTipo < 10)
							 orderby datos.IntNumero descending
							 select new ObjDocumentos
							 {
								 IdFacturador = datos.TblEmpresasFacturador.StrIdentificacion,
								 Facturador = datos.TblEmpresasFacturador.StrRazonSocial,
								 Prefijo = datos.StrPrefijo,
								 NumeroDocumento = datos.StrPrefijo + datos.IntNumero.ToString(),
								 DatFechaIngreso = datos.DatFechaIngreso,
								 DatFechaDocumento = datos.DatFechaDocumento,
								 DatFechaVencDocumento = datos.DatFechaVencDocumento,
								 IntVlrTotal = (datos.IntDocTipo == 3) ? -datos.IntVlrTotal : datos.IntVlrTotal,
								 IntSubTotal = (datos.IntDocTipo == 3) ? -datos.IntValorSubtotal : datos.IntValorSubtotal,
								 IntNeto = (datos.IntDocTipo == 3) ? -datos.IntValorNeto : datos.IntValorNeto,
								 EstadoFactura = datos.IntIdEstado,
								 EstadoCategoria = (Int16)datos.IdCategoriaEstado,
								 EstadoAcuse = datos.IntAdquirienteRecibo,
								 MotivoRechazo = datos.StrAdquirienteMvoRechazo,
								 StrAdquirienteMvoRechazo = datos.StrAdquirienteMvoRechazo,
								 IdentificacionAdquiriente = datos.TblEmpresasAdquiriente.StrIdentificacion,
								 NombreAdquiriente = datos.TblEmpresasAdquiriente.StrRazonSocial,
								 MailAdquiriente = datos.TblEmpresasAdquiriente.StrMailAdmin,
								 Xml = datos.StrUrlArchivoUbl,
								 Pdf = datos.StrUrlArchivoPdf,
								 StrIdSeguridad = datos.StrIdSeguridad,
								 tipodoc = datos.IntDocTipo,
								 zip = datos.StrUrlAnexo,
								 RutaServDian = datos.StrUrlArchivoUbl,
								 XmlAcuse = datos.StrUrlAcuseUbl,
								 permiteenvio = (Int16)datos.IdCategoriaEstado,
								 IntAdquirienteRecibo = datos.IntAdquirienteRecibo,
								 Estado = datos.IdCategoriaEstado,
								 EstadoEnvioMail = datos.IntEstadoEnvio.ToString(),
								 MensajeEnvio = datos.IntMensajeEnvio.ToString(),
								 EnvioMail = datos.IntEstadoEnvio,
								 poseeIdComercio = (datos.TblEmpresasFacturador.IntManejaPagoE) ? (datos.IntIdEstado != 90) ? 1 : 0 : 0,
								 FacturaCancelada = datos.IntIdEstado,
								 //PagosParciales = (datos.TblEmpresasFacturador.IntManejaPagoE) ? 1 : 0,
								 PagosParciales = (datos.TblEmpresasFacturador.IntPagoEParcial) ? 1 : 0,// (string.IsNullOrEmpty(datos.TblEmpresasResoluciones.ComercioConfigId.ToString())) ? (datos.TblEmpresasFacturador.IntManejaPagoE) ? 1 : 0 : datos.TblEmpresasResoluciones.PermiteParciales,
								 NumResolucion = datos.StrNumResolucion,
								 IntValorPagar = datos.IntValorPagar
							 }).ToList();


			}
			else
			{
				var listaDocumetos = Coleccion.ConvertirStringlong(numero_documento);

				context.Configuration.LazyLoadingEnabled = false;

				respuesta = (from datos in context.TblDocumentos.Include("TblEmpresasFacturador").AsNoTracking()//.Include("TblEmpresasResoluciones").AsNoTracking()
							 join empresa in context.TblEmpresas on datos.StrEmpresaAdquiriente equals empresa.StrIdentificacion
							 where (empresa.StrIdentificacion.Equals(identificacion_adquiente))
							 && (datos.StrEmpresaFacturador.Equals(identificacion_facturador) || identificacion_facturador.Equals("*"))
							 && (listaDocumetos.Contains(datos.IntNumero))
							 && (datos.TblEmpresasFacturador.IntEnvioMailRecepcion == true || (datos.TblEmpresasFacturador.IntEnvioMailRecepcion == false && datos.IdCategoriaEstado == Categoria))
							 && (datos.IntDocTipo < 10)
							 orderby datos.IntNumero descending
							 select new ObjDocumentos
							 {
								 IdFacturador = datos.TblEmpresasFacturador.StrIdentificacion,
								 Facturador = datos.TblEmpresasFacturador.StrRazonSocial,
								 Prefijo = datos.StrPrefijo,
								 NumeroDocumento = datos.StrPrefijo + datos.IntNumero.ToString(),
								 DatFechaIngreso = datos.DatFechaIngreso,
								 DatFechaDocumento = datos.DatFechaDocumento,
								 DatFechaVencDocumento = datos.DatFechaVencDocumento,
								 IntVlrTotal = (datos.IntDocTipo == 3) ? -datos.IntVlrTotal : datos.IntVlrTotal,
								 IntSubTotal = (datos.IntDocTipo == 3) ? -datos.IntValorSubtotal : datos.IntValorSubtotal,
								 IntNeto = (datos.IntDocTipo == 3) ? -datos.IntValorNeto : datos.IntValorNeto,
								 EstadoFactura = datos.IntIdEstado,
								 EstadoCategoria = (Int16)datos.IdCategoriaEstado,
								 EstadoAcuse = datos.IntAdquirienteRecibo,
								 MotivoRechazo = datos.StrAdquirienteMvoRechazo,
								 StrAdquirienteMvoRechazo = datos.StrAdquirienteMvoRechazo,
								 IdentificacionAdquiriente = datos.TblEmpresasAdquiriente.StrIdentificacion,
								 NombreAdquiriente = datos.TblEmpresasAdquiriente.StrRazonSocial,
								 MailAdquiriente = datos.TblEmpresasAdquiriente.StrMailAdmin,
								 Xml = datos.StrUrlArchivoUbl,
								 Pdf = datos.StrUrlArchivoPdf,
								 StrIdSeguridad = datos.StrIdSeguridad,
								 tipodoc = datos.IntDocTipo,
								 zip = datos.StrUrlAnexo,
								 RutaServDian = datos.StrUrlArchivoUbl,
								 XmlAcuse = datos.StrUrlAcuseUbl,
								 permiteenvio = (Int16)datos.IdCategoriaEstado,
								 IntAdquirienteRecibo = datos.IntAdquirienteRecibo,
								 Estado = datos.IdCategoriaEstado,
								 EstadoEnvioMail = datos.IntEstadoEnvio.ToString(),
								 MensajeEnvio = datos.IntMensajeEnvio.ToString(),
								 EnvioMail = datos.IntEstadoEnvio,
								 poseeIdComercio = (datos.TblEmpresasFacturador.IntManejaPagoE) ? (datos.IntIdEstado != 90) ? 1 : 0 : 0,
								 FacturaCancelada = datos.IntIdEstado,
								 //PagosParciales = (datos.TblEmpresasFacturador.IntManejaPagoE) ? 1 : 0,
								 PagosParciales = (datos.TblEmpresasFacturador.IntPagoEParcial) ? 1 : 0,// (string.IsNullOrEmpty(datos.TblEmpresasResoluciones.ComercioConfigId.ToString())) ? (datos.TblEmpresasFacturador.IntManejaPagoE) ? 1 : 0 : datos.TblEmpresasResoluciones.PermiteParciales,
								 NumResolucion = datos.StrNumResolucion,
								 IntValorPagar = datos.IntValorPagar
							 }).ToList();

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
		public List<ObjDocumentos> ObtenerPorFechasObligado(string codigo_facturador, string numero_documento, string codigo_adquiriente, string estado_dian, string estado_recibo, DateTime fecha_inicio, DateTime fecha_fin, string Resolucion, int tipo_filtro_fecha)
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

			List<ObjDocumentos> documentos = new List<ObjDocumentos>();

			if (numero_documento.Equals("*"))
			{
				context.Configuration.LazyLoadingEnabled = false;

				documentos = (from datos in context.TblDocumentos.AsNoTracking()
							  where (datos.StrEmpresaFacturador.Equals(codigo_facturador) || codigo_facturador.Equals("*"))
							  && (datos.StrEmpresaAdquiriente.Equals(codigo_adquiriente) || codigo_adquiriente.Equals("*"))
							  && (LstEstado.Contains(datos.IdCategoriaEstado.ToString()) || estado_dian.Equals("*"))
							  && (datos.IntAdquirienteRecibo == cod_estado_recibo || estado_recibo.Equals("*"))
							  && (LstResolucion.Contains(datos.StrNumResolucion.ToString()) || Resolucion.Equals("*"))
							  && ((datos.DatFechaIngreso >= fecha_inicio && datos.DatFechaIngreso <= fecha_fin) || tipo_filtro_fecha == 2)
							  && ((datos.DatFechaDocumento >= fecha_inicio && datos.DatFechaDocumento <= fecha_fin) || tipo_filtro_fecha == 1)
							  && (datos.IntDocTipo < 10)
							  orderby datos.IntNumero descending
							  select new ObjDocumentos
							  {
								  IdFacturador = datos.TblEmpresasFacturador.StrIdentificacion,
								  Facturador = datos.TblEmpresasFacturador.StrRazonSocial,
								  Prefijo = datos.StrPrefijo,
								  NumeroDocumento = datos.StrPrefijo + datos.IntNumero.ToString(),
								  DatFechaIngreso = datos.DatFechaIngreso,
								  DatFechaDocumento = datos.DatFechaDocumento,
								  DatFechaVencDocumento = datos.DatFechaVencDocumento,
								  IntVlrTotal = (datos.IntDocTipo == 3) ? -datos.IntVlrTotal : datos.IntVlrTotal,
								  IntSubTotal = (datos.IntDocTipo == 3) ? -datos.IntValorSubtotal : datos.IntValorSubtotal,
								  IntNeto = (datos.IntDocTipo == 3) ? -datos.IntValorNeto : datos.IntValorNeto,
								  EstadoFactura = datos.IntIdEstado,
								  EstadoCategoria = (Int16)datos.IdCategoriaEstado,
								  EstadoAcuse = datos.IntAdquirienteRecibo,
								  MotivoRechazo = datos.StrAdquirienteMvoRechazo,
								  StrAdquirienteMvoRechazo = datos.StrAdquirienteMvoRechazo,
								  IdentificacionAdquiriente = datos.TblEmpresasAdquiriente.StrIdentificacion,
								  NombreAdquiriente = datos.TblEmpresasAdquiriente.StrRazonSocial,
								  MailAdquiriente = datos.TblEmpresasAdquiriente.StrMailAdmin,
								  Xml = datos.StrUrlArchivoUbl,
								  Pdf = datos.StrUrlArchivoPdf,
								  StrIdSeguridad = datos.StrIdSeguridad,
								  tipodoc = datos.IntDocTipo,
								  zip = datos.StrUrlAnexo,
								  RutaServDian = datos.StrUrlArchivoUbl,
								  XmlAcuse = datos.StrUrlAcuseUbl,
								  permiteenvio = (Int16)datos.IdCategoriaEstado,
								  IntAdquirienteRecibo = datos.IntAdquirienteRecibo,
								  Estado = datos.IdCategoriaEstado,
								  EstadoEnvioMail = datos.IntEstadoEnvio.ToString(),
								  MensajeEnvio = datos.IntMensajeEnvio.ToString(),
								  EnvioMail = datos.IntEstadoEnvio,
								  Radian = datos.TblEmpresasFacturador.IntRadian,
								  FormaPago = datos.IntFormaPago
							  }).ToList();
			}
			else
			{
				var listaDocumetos = Coleccion.ConvertirStringlong(numero_documento);

				context.Configuration.LazyLoadingEnabled = false;

				documentos = (from datos in context.TblDocumentos.AsNoTracking()
							  where (datos.StrEmpresaFacturador.Equals(codigo_facturador))
							  && (listaDocumetos.Contains(datos.IntNumero))
							  && (datos.IntDocTipo < 10)
							  select new ObjDocumentos
							  {
								  IdFacturador = datos.TblEmpresasFacturador.StrIdentificacion,
								  Facturador = datos.TblEmpresasFacturador.StrRazonSocial,
								  Prefijo = datos.StrPrefijo,
								  NumeroDocumento = datos.StrPrefijo + datos.IntNumero.ToString(),
								  DatFechaIngreso = datos.DatFechaIngreso,
								  DatFechaDocumento = datos.DatFechaDocumento,
								  DatFechaVencDocumento = datos.DatFechaVencDocumento,
								  IntVlrTotal = (datos.IntDocTipo == 3) ? -datos.IntVlrTotal : datos.IntVlrTotal,
								  IntSubTotal = (datos.IntDocTipo == 3) ? -datos.IntValorSubtotal : datos.IntValorSubtotal,
								  IntNeto = (datos.IntDocTipo == 3) ? -datos.IntValorNeto : datos.IntValorNeto,
								  EstadoFactura = datos.IntIdEstado,
								  EstadoCategoria = (Int16)datos.IdCategoriaEstado,
								  EstadoAcuse = datos.IntAdquirienteRecibo,
								  MotivoRechazo = datos.StrAdquirienteMvoRechazo,
								  StrAdquirienteMvoRechazo = datos.StrAdquirienteMvoRechazo,
								  IdentificacionAdquiriente = datos.TblEmpresasAdquiriente.StrIdentificacion,
								  NombreAdquiriente = datos.TblEmpresasAdquiriente.StrRazonSocial,
								  MailAdquiriente = datos.TblEmpresasAdquiriente.StrMailAdmin,
								  Xml = datos.StrUrlArchivoUbl,
								  Pdf = datos.StrUrlArchivoPdf,
								  StrIdSeguridad = datos.StrIdSeguridad,
								  tipodoc = datos.IntDocTipo,
								  zip = datos.StrUrlAnexo,
								  RutaServDian = datos.StrUrlArchivoUbl,
								  XmlAcuse = datos.StrUrlAcuseUbl,
								  permiteenvio = (Int16)datos.IdCategoriaEstado,
								  IntAdquirienteRecibo = datos.IntAdquirienteRecibo,
								  Estado = datos.IdCategoriaEstado,
								  EstadoEnvioMail = datos.IntEstadoEnvio.ToString(),
								  MensajeEnvio = datos.IntMensajeEnvio.ToString(),
								  EnvioMail = datos.IntEstadoEnvio,
								  Radian = datos.TblEmpresasFacturador.IntRadian,
								  FormaPago = datos.IntFormaPago
							  }).ToList();
			}

			return documentos;
		}


		/// <summary>
		/// Obtiene los documentos del Emisor
		/// </summary>
		/// <param name="codigo_facturador"></param>
		/// <param name="numero_documento"></param>
		/// <param name="codigo_tercero"></param>
		/// <param name="estado_dian"></param>
		/// <param name="estado_recibo"></param>
		/// <param name="fecha_inicio"></param>
		/// <param name="fecha_fin"></param>
		/// <returns></returns>
		public List<ObjDocumentos> ObtenerPorFechasEmisor(string codigo_emisor, string numero_documento, string codigo_empleado, string estado_dian, string estado_recibo, DateTime fecha_inicio, DateTime fecha_fin, int tipo_documento, int tipo_filtro_fecha)
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
			if (string.IsNullOrWhiteSpace(codigo_empleado))
				codigo_empleado = "*";

			if (string.IsNullOrWhiteSpace(estado_recibo))
				estado_recibo = "*";


			List<ObjDocumentos> documentos = new List<ObjDocumentos>();

			if (numero_documento.Equals("*"))
			{
				context.Configuration.LazyLoadingEnabled = false;

				documentos = (from datos in context.TblDocumentos.AsNoTracking()
							  where (datos.StrEmpresaFacturador.Equals(codigo_emisor) || codigo_emisor.Equals("*"))
							  && (datos.StrEmpresaAdquiriente.Equals(codigo_empleado) || codigo_empleado.Equals("*"))
							  && (LstEstado.Contains(datos.IdCategoriaEstado.ToString()) || estado_dian.Equals("*"))
							  && ((datos.IntDocTipo >= 10) && (datos.IntDocTipo == tipo_documento || tipo_documento == 0))
							  //&& (datos.IntAdquirienteRecibo == cod_estado_recibo || estado_recibo.Equals("*"))
							  //&& (LstResolucion.Contains(datos.StrNumResolucion.ToString()) || Resolucion.Equals("*")) // sustituir por tipo de documento
							  && ((datos.DatFechaIngreso >= fecha_inicio && datos.DatFechaIngreso <= fecha_fin) || tipo_filtro_fecha == 2)
							  && ((datos.DatFechaDocumento >= fecha_inicio && datos.DatFechaDocumento <= fecha_fin) || tipo_filtro_fecha == 1)
							  orderby datos.IntNumero descending
							  select new ObjDocumentos
							  {
								  IdFacturador = datos.TblEmpresasFacturador.StrIdentificacion,
								  Facturador = datos.TblEmpresasFacturador.StrRazonSocial,
								  Prefijo = datos.StrPrefijo,
								  NumeroDocumento = datos.StrPrefijo + datos.IntNumero.ToString(),
								  DatFechaIngreso = datos.DatFechaIngreso,
								  DatFechaDocumento = datos.DatFechaDocumento,
								  DatFechaVencDocumento = datos.DatFechaVencDocumento,
								  IntVlrTotal = (datos.IntDocTipo == 3) ? -datos.IntVlrTotal : datos.IntVlrTotal,
								  IntSubTotal = (datos.IntDocTipo == 3) ? -datos.IntValorSubtotal : datos.IntValorSubtotal,
								  IntNeto = (datos.IntDocTipo == 3) ? -datos.IntValorNeto : datos.IntValorNeto,
								  EstadoFactura = datos.IntIdEstado,
								  EstadoCategoria = (Int16)datos.IdCategoriaEstado,
								  EstadoAcuse = datos.IntAdquirienteRecibo,
								  MotivoRechazo = datos.StrAdquirienteMvoRechazo,
								  StrAdquirienteMvoRechazo = datos.StrAdquirienteMvoRechazo,
								  IdentificacionAdquiriente = datos.TblEmpresasAdquiriente.StrIdentificacion,
								  NombreAdquiriente = datos.TblEmpresasAdquiriente.StrRazonSocial,
								  MailAdquiriente = datos.TblEmpresasAdquiriente.StrMailAdmin,
								  Xml = datos.StrUrlArchivoUbl,
								  Pdf = datos.StrUrlArchivoPdf,
								  StrIdSeguridad = datos.StrIdSeguridad,
								  tipodoc = datos.IntDocTipo,
								  zip = datos.StrUrlAnexo,
								  RutaServDian = datos.StrUrlArchivoUbl,
								  XmlAcuse = datos.StrUrlAcuseUbl,
								  permiteenvio = (Int16)datos.IdCategoriaEstado,
								  IntAdquirienteRecibo = datos.IntAdquirienteRecibo,
								  Estado = datos.IdCategoriaEstado,
								  EstadoEnvioMail = datos.IntEstadoEnvio.ToString(),
								  MensajeEnvio = datos.IntMensajeEnvio.ToString(),
								  EnvioMail = datos.IntEstadoEnvio
							  }).ToList();
			}
			else
			{
				var listaDocumetos = Coleccion.ConvertirStringlong(numero_documento);

				context.Configuration.LazyLoadingEnabled = false;

				documentos = (from datos in context.TblDocumentos.AsNoTracking()
							  where (datos.StrEmpresaFacturador.Equals(codigo_emisor))
							  && (listaDocumetos.Contains(datos.IntNumero))
							  && (datos.IntDocTipo >= 10)
							  select new ObjDocumentos
							  {
								  IdFacturador = datos.TblEmpresasFacturador.StrIdentificacion,
								  Facturador = datos.TblEmpresasFacturador.StrRazonSocial,
								  Prefijo = datos.StrPrefijo,
								  NumeroDocumento = datos.StrPrefijo + datos.IntNumero.ToString(),
								  DatFechaIngreso = datos.DatFechaIngreso,
								  DatFechaDocumento = datos.DatFechaDocumento,
								  DatFechaVencDocumento = datos.DatFechaVencDocumento,
								  IntVlrTotal = (datos.IntDocTipo == 3) ? -datos.IntVlrTotal : datos.IntVlrTotal,
								  IntSubTotal = (datos.IntDocTipo == 3) ? -datos.IntValorSubtotal : datos.IntValorSubtotal,
								  IntNeto = (datos.IntDocTipo == 3) ? -datos.IntValorNeto : datos.IntValorNeto,
								  EstadoFactura = datos.IntIdEstado,
								  EstadoCategoria = (Int16)datos.IdCategoriaEstado,
								  EstadoAcuse = datos.IntAdquirienteRecibo,
								  MotivoRechazo = datos.StrAdquirienteMvoRechazo,
								  StrAdquirienteMvoRechazo = datos.StrAdquirienteMvoRechazo,
								  IdentificacionAdquiriente = datos.TblEmpresasAdquiriente.StrIdentificacion,
								  NombreAdquiriente = datos.TblEmpresasAdquiriente.StrRazonSocial,
								  MailAdquiriente = datos.TblEmpresasAdquiriente.StrMailAdmin,
								  Xml = datos.StrUrlArchivoUbl,
								  Pdf = datos.StrUrlArchivoPdf,
								  StrIdSeguridad = datos.StrIdSeguridad,
								  tipodoc = datos.IntDocTipo,
								  zip = datos.StrUrlAnexo,
								  RutaServDian = datos.StrUrlArchivoUbl,
								  XmlAcuse = datos.StrUrlAcuseUbl,
								  permiteenvio = (Int16)datos.IdCategoriaEstado,
								  IntAdquirienteRecibo = datos.IntAdquirienteRecibo,
								  Estado = datos.IdCategoriaEstado,
								  EstadoEnvioMail = datos.IntEstadoEnvio.ToString(),
								  MensajeEnvio = datos.IntMensajeEnvio.ToString(),
								  EnvioMail = datos.IntEstadoEnvio
							  }).ToList();
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
			context.Configuration.LazyLoadingEnabled = false;

			List<TblDocumentos> documentos = (from datos in context.TblDocumentos.Include("TblEmpresasFacturador")
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
				lista_estado_visible = Coleccion.ConvertirStringlong(string.Format("{0},{1},{2},{3},{4},{5}", CodigoResponseV2.Recibido.GetHashCode(), CodigoResponseV2.Rechazado.GetHashCode(), CodigoResponseV2.AprobadoTacito.GetHashCode(), CodigoResponseV2.Aceptado.GetHashCode(), CodigoResponseV2.Expresa.GetHashCode(), CodigoResponseV2.Inscripcion.GetHashCode()));
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
		public List<Guid> ObtenerAcuseTacito(string codigo_facturador, string numero_documento, string codigo_adquiriente, bool sonda = false)
		{
			try
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

				//context.Configuration.LazyLoadingEnabled = false;
				List<Guid> documentos = new List<Guid>();

				if (sonda)
				{
					Ctl_Empresa _empresa = new Ctl_Empresa();
					List<TblEmpresas> facturadores = _empresa.ObtenerEmpresaAcuse();

					context.Configuration.LazyLoadingEnabled = false;

					foreach (TblEmpresas item in facturadores)
					{
						try
						{
							List<Guid> docs = (from datos in context.TblDocumentos.AsNoTracking()
											   where (datos.IntAdquirienteRecibo.Equals(4)) && datos.IntDocTipo == 1 && datos.IntFormaPago == 2 && datos.IntIdEstado > Enviomail && datos.IntIdEstado < estado_error
													 && datos.StrEmpresaFacturador == item.StrIdentificacion
													 && (((datos.DatFechaIngreso <= SqlFunctions.DateAdd("hh", -item.IntAcuseTacito.Value, FechaActual)
															  && item.IntAcuseTacito.Value > 0)))
											   orderby datos.IntNumero descending
											   select datos.StrIdSeguridad).ToList();

							//List<Guid> docs = (from datos in context.TblDocumentos.AsNoTracking()
							//				   where (datos.IntAdquirienteRecibo.Equals(0) || datos.IntAdquirienteRecibo.Equals(4)) && datos.IntIdEstado > Enviomail && datos.IntIdEstado < estado_error
							//						 && datos.StrEmpresaFacturador == item.StrIdentificacion
							//						 && (((datos.DatFechaIngreso <= SqlFunctions.DateAdd("hh", -item.IntAcuseTacito.Value, FechaActual)
							//								  && item.IntAcuseTacito.Value > 0)))
							//				   orderby datos.IntNumero descending
							//				   select datos.StrIdSeguridad).ToList();

							if (docs.Count > 0)
							{
								foreach (var doc in docs)
								{
									try
									{
										Ctl_EventosRadian ctl_evento = new Ctl_EventosRadian();
										List<TblEventosRadian> lista_evento_doc = ctl_evento.Obtener(doc);
										//bool generar_acuse_tacito = true;

										if (lista_evento_doc.Count > 0)
										{
											TblEventosRadian evento_reclamo = lista_evento_doc.Where(x => x.IntEstadoEvento == 2).FirstOrDefault();
											TblEventosRadian evento_reciboM = lista_evento_doc.Where(x => x.IntEstadoEvento == 4).FirstOrDefault();
											TblEventosRadian evento_expresa = lista_evento_doc.Where(x => x.IntEstadoEvento == 5).FirstOrDefault();

											//Valido que no tenga eventos que evitene generar el Acuse Tacito
											if (evento_reciboM != null && evento_reclamo == null && evento_expresa == null)
											{
												//Primero consulto que este evento cuente con las 72 horas del acuse de recibo con respecto a la fecha actual
												TimeSpan horas_acuse_evento = Fecha.GetFecha().Subtract(evento_reciboM.DatFechaEvento);

												if (horas_acuse_evento.TotalHours >= 72)
												{
													//Se valida que el plazo para generar de Acuse tacito registrado por el facturador sea igual a superior a la fecha del recibo de mercancia
													bool cumple_acuse_tacito = horas_acuse_evento.TotalHours >= item.IntAcuseTacito ? true : false;

													if (cumple_acuse_tacito == true)
													{
														ActualizarRespuestaAcuse(doc, (short)CodigoResponseV2.AprobadoTacito.GetHashCode(), string.Empty);

														//Consulto que en la actualidad 
														//ActualizarRespuestaAcuse(doc, (short)CodigoResponseV2.AprobadoTacito.GetHashCode(), string.Empty, string.Empty, true);
														//lista_evento_doc = ctl_evento.Obtener(doc);
														//evento_reclamo = lista_evento_doc.Where(x => x.IntEstadoEvento == 2).FirstOrDefault();
														//evento_reciboM = lista_evento_doc.Where(x => x.IntEstadoEvento == 4).FirstOrDefault();
														//evento_expresa = lista_evento_doc.Where(x => x.IntEstadoEvento == 5).FirstOrDefault();

														//if (evento_reclamo != null || evento_expresa != null)
														//	generar_acuse_tacito = false;
													}
												}

												//if (generar_acuse_tacito == true)
												//	ActualizarRespuestaAcuse(doc, (short)CodigoResponseV2.AprobadoTacito.GetHashCode(), string.Empty);
											}
										}
										else
										{
											//ActualizarRespuestaAcuse(doc, (short)CodigoResponseV2.AprobadoTacito.GetHashCode(), string.Empty, string.Empty, true);
											var Tarea1 = SondaConsultareventos(false, doc.ToString());											
										}

									}
									catch (Exception excepcion)
									{
										RegistroLog.EscribirLog(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.actualizacion, item.StrIdentificacion);
										//Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.actualizacion);
									}
								}
							}

							documentos.AddRange(docs);
						}
						catch (Exception exception)
						{
							RegistroLog.EscribirLog(exception, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.consulta);
						}
					}
				}
				else
				{
					documentos = (from datos in context.TblDocumentos
								  join obligado in context.TblEmpresas on datos.StrEmpresaFacturador equals obligado.StrIdentificacion
								  join adquiriente in context.TblEmpresas on datos.StrEmpresaAdquiriente equals adquiriente.StrIdentificacion
								  where datos.IntAdquirienteRecibo.Equals(4) && datos.IntIdEstado > Enviomail && datos.IntIdEstado < estado_error && datos.TblEmpresasFacturador.IntRadian == true
										&& (((datos.StrProveedorEmisor == Constantes.NitResolucionsinPrefijo || string.IsNullOrEmpty(datos.StrProveedorEmisor))
											 && (datos.DatFechaIngreso <= SqlFunctions.DateAdd("hh", -datos.TblEmpresasFacturador.IntAcuseTacito.Value, FechaActual)
												 && datos.TblEmpresasFacturador.IntAcuseTacito.Value > 0)))
										//********************************************************
										//|| ((datos.StrProveedorEmisor != Constantes.NitResolucionsinPrefijo && (!string.IsNullOrEmpty(datos.StrProveedorEmisor)))
										//	&& (datos.DatFechaIngreso <= SqlFunctions.DateAdd("hh", -HATP, FechaActual))
										//	&& datos.IntAdquirienteRecibo.Equals(1) && datos.IntIdEstado > Enviomail && datos.IntIdEstado < estado_error && datos.TblEmpresasFacturador.IntRadian == true)

								  orderby datos.IntNumero descending
								  select datos.StrIdSeguridad).ToList();
				}

				return documentos;
			}
			catch (Exception exception)
			{
				RegistroLog.EscribirLog(exception, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.consulta);
				throw;
			}
		}

		public async Task SondaConsultareventos(bool sonda , string List_IdSeguridad)
		{
			try
			{
				var Tarea = TareaConsultarEventosRadian(sonda ,List_IdSeguridad);
				await Task.WhenAny(Tarea);
			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.actualizacion);
			}

		}

		public async Task TareaConsultarEventosRadian(bool sonda, string List_IdSeguridad)
		{
			await Task.Factory.StartNew(() =>
			{
				ConsultarEventosRadian(sonda, List_IdSeguridad);
			});
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
		public List<ObjDocumentos> ObtenerAdministrador(string codigo_facturador, string numero_documento, string codigo_adquiriente, string estado_dian, string estado_recibo, DateTime fecha_inicio, DateTime fecha_fin, int TipoDocumento, int tipo_fecha, int Desde, int Hasta)
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

			List<ObjDocumentos> documentos = new List<ObjDocumentos>();


			if (numero_documento.Equals("*"))
			{
				context.Configuration.LazyLoadingEnabled = false;
				documentos = (from datos in context.TblDocumentos.AsNoTracking()
							  join obligado in context.TblEmpresas on datos.StrEmpresaFacturador equals obligado.StrIdentificacion
							  join adquiriente in context.TblEmpresas on datos.StrEmpresaAdquiriente equals adquiriente.StrIdentificacion
							  where (obligado.StrIdentificacion.Equals(codigo_facturador) || codigo_facturador.Equals("*"))
									&& (datos.IntNumero == num_doc || numero_documento.Equals("*"))
									&& (adquiriente.StrIdentificacion.Equals(codigo_adquiriente) || codigo_adquiriente.Equals("*"))
									&& (LstEstado.Contains(datos.IdCategoriaEstado.ToString()) || estado_dian.Equals("*"))
									&& (datos.IntAdquirienteRecibo == cod_estado_recibo || estado_recibo.Equals("*"))
									&& ((datos.DatFechaDocumento >= fecha_inicio && datos.DatFechaDocumento <= fecha_fin) || tipo_fecha == 1)
									&& ((datos.DatFechaIngreso >= fecha_inicio && datos.DatFechaIngreso <= fecha_fin) || tipo_fecha == 2)
									&& (datos.IntDocTipo.Equals(TipoDocumento) || TipoDocumento == 0)
							  orderby datos.IntNumero descending
							  select new ObjDocumentos
							  {
								  IdFacturador = datos.TblEmpresasFacturador.StrIdentificacion,
								  Facturador = datos.TblEmpresasFacturador.StrRazonSocial,
								  Prefijo = datos.StrPrefijo,
								  NumeroDocumento = datos.StrPrefijo + datos.IntNumero.ToString(),
								  DatFechaIngreso = datos.DatFechaIngreso,
								  DatFechaDocumento = datos.DatFechaDocumento,
								  DatFechaVencDocumento = datos.DatFechaVencDocumento,
								  IntVlrTotal = (datos.IntDocTipo == 3) ? -datos.IntVlrTotal : datos.IntVlrTotal,
								  IntSubTotal = (datos.IntDocTipo == 3) ? -datos.IntValorSubtotal : datos.IntValorSubtotal,
								  IntNeto = (datos.IntDocTipo == 3) ? -datos.IntValorNeto : datos.IntValorNeto,
								  EstadoFactura = datos.IntIdEstado,
								  EstadoCategoria = (Int16)datos.IdCategoriaEstado,
								  EstadoAcuse = datos.IntAdquirienteRecibo,
								  MotivoRechazo = datos.StrAdquirienteMvoRechazo,
								  StrAdquirienteMvoRechazo = datos.StrAdquirienteMvoRechazo,
								  IdentificacionAdquiriente = datos.TblEmpresasAdquiriente.StrIdentificacion,
								  NombreAdquiriente = datos.TblEmpresasAdquiriente.StrRazonSocial,
								  MailAdquiriente = datos.TblEmpresasAdquiriente.StrMailAdmin,
								  Xml = datos.StrUrlArchivoUbl,
								  Pdf = datos.StrUrlArchivoPdf,
								  StrIdSeguridad = datos.StrIdSeguridad,
								  tipodoc = datos.IntDocTipo,
								  zip = datos.StrUrlAnexo,
								  RutaServDian = datos.StrUrlArchivoUbl,
								  XmlAcuse = datos.StrUrlAcuseUbl,
								  permiteenvio = (Int16)datos.IdCategoriaEstado,
								  IntAdquirienteRecibo = datos.IntAdquirienteRecibo,
								  Estado = datos.IdCategoriaEstado,
								  EstadoEnvioMail = datos.IntEstadoEnvio.ToString(),
								  MensajeEnvio = datos.IntMensajeEnvio.ToString(),
								  EnvioMail = datos.IntEstadoEnvio
							  }).OrderByDescending(x => x.DatFechaIngreso).Skip(Desde).Take(Hasta).ToList();
			}
			else
			{
				var listaDocumetos = Coleccion.ConvertirStringlong(numero_documento);

				context.Configuration.LazyLoadingEnabled = false;

				documentos = (from datos in context.TblDocumentos.AsNoTracking()
							  join obligado in context.TblEmpresas on datos.StrEmpresaFacturador equals obligado.StrIdentificacion
							  join adquiriente in context.TblEmpresas on datos.StrEmpresaAdquiriente equals adquiriente.StrIdentificacion
							  where listaDocumetos.Contains(datos.IntNumero)
							  select new ObjDocumentos
							  {
								  IdFacturador = datos.TblEmpresasFacturador.StrIdentificacion,
								  Facturador = datos.TblEmpresasFacturador.StrRazonSocial,
								  Prefijo = datos.StrPrefijo,
								  NumeroDocumento = datos.StrPrefijo + datos.IntNumero.ToString(),
								  DatFechaIngreso = datos.DatFechaIngreso,
								  DatFechaDocumento = datos.DatFechaDocumento,
								  DatFechaVencDocumento = datos.DatFechaVencDocumento,
								  IntVlrTotal = (datos.IntDocTipo == 3) ? -datos.IntVlrTotal : datos.IntVlrTotal,
								  IntSubTotal = (datos.IntDocTipo == 3) ? -datos.IntValorSubtotal : datos.IntValorSubtotal,
								  IntNeto = (datos.IntDocTipo == 3) ? -datos.IntValorNeto : datos.IntValorNeto,
								  EstadoFactura = datos.IntIdEstado,
								  EstadoCategoria = (Int16)datos.IdCategoriaEstado,
								  EstadoAcuse = datos.IntAdquirienteRecibo,
								  MotivoRechazo = datos.StrAdquirienteMvoRechazo,
								  StrAdquirienteMvoRechazo = datos.StrAdquirienteMvoRechazo,
								  IdentificacionAdquiriente = datos.TblEmpresasAdquiriente.StrIdentificacion,
								  NombreAdquiriente = datos.TblEmpresasAdquiriente.StrRazonSocial,
								  MailAdquiriente = datos.TblEmpresasAdquiriente.StrMailAdmin,
								  Xml = datos.StrUrlArchivoUbl,
								  Pdf = datos.StrUrlArchivoPdf,
								  StrIdSeguridad = datos.StrIdSeguridad,
								  tipodoc = datos.IntDocTipo,
								  zip = datos.StrUrlAnexo,
								  RutaServDian = datos.StrUrlArchivoUbl,
								  XmlAcuse = datos.StrUrlAcuseUbl,
								  permiteenvio = (Int16)datos.IdCategoriaEstado,
								  IntAdquirienteRecibo = datos.IntAdquirienteRecibo,
								  Estado = datos.IdCategoriaEstado,
								  EstadoEnvioMail = datos.IntEstadoEnvio.ToString(),
								  MensajeEnvio = datos.IntMensajeEnvio.ToString(),
								  EnvioMail = datos.IntEstadoEnvio

							  }).OrderByDescending(x => x.DatFechaIngreso).Skip(Desde).Take(Hasta).ToList();
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
				context.Configuration.LazyLoadingEnabled = false;

				var respuesta = (from datos in context.TblDocumentos.Include("TblEmpresasAdquiriente").Include("TblEmpresasFacturador").Include("TblEmpresasResoluciones")
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
		/// Obtiene los documentos por id se seguridad.
		/// </summary>
		/// <param name="id_seguridad"></param>
		/// <returns></returns>
		public TblDocumentos ObtenerDocumento(System.Guid id_seguridad)
		{

			try
			{
				context.Configuration.LazyLoadingEnabled = false;

				var respuesta = (from datos in context.TblDocumentos
								 where datos.StrIdSeguridad.Equals(id_seguridad)
								 select datos
								 );

				return respuesta.FirstOrDefault();
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		/// <summary>
		/// Obtiene los documentos por id se seguridad.
		/// </summary>
		/// <param name="id_seguridad"></param>
		/// <returns></returns>
		public List<TblDocumentos> ObtenerPorIdSeguridad(System.Guid id_seguridad, bool LazyLoading = true)
		{

			try
			{
				context.Configuration.LazyLoadingEnabled = LazyLoading;

				var respuesta = (from datos in context.TblDocumentos.Include("TblEmpresasAdquiriente").Include("TblEmpresasFacturador").Include("TblPagosDetalles")
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


		public List<TblDocumentos> ObtenerPorMes(string identificacion_empresa, int mes, string identificacion_adquiriente)
		{
			try
			{
				context.Configuration.LazyLoadingEnabled = false;

				var respuesta = (from datos in context.TblDocumentos
								 where datos.StrEmpresaFacturador.Equals(identificacion_empresa)
								 && datos.DatFechaIngreso.Month.Equals(mes)
								 && datos.StrEmpresaAdquiriente.Equals(identificacion_adquiriente)
								 select datos
								 ).OrderBy(y => y.IntNumero);

				return respuesta.ToList();
			}
			catch (Exception)
			{

				throw;
			}
		}

		/// <summary>
		/// Valida si existe un documento de ese adquiriente para ese facturador
		/// </summary>
		/// <param name="identificacion_empresa"></param>
		/// <param name="identificacion_adquiriente"></param>
		/// <returns>TblDocumentos</returns>
		public TblDocumentos ObtenerDocumentodeFacturadorAdquiriente(string identificacion_empresa, string identificacion_adquiriente)
		{
			try
			{
				context.Configuration.LazyLoadingEnabled = false;

				var respuesta = (from datos in context.TblDocumentos.AsNoTracking()
								 where datos.StrEmpresaFacturador.Equals(identificacion_empresa)
								 && datos.StrEmpresaAdquiriente.Equals(identificacion_adquiriente)
								 select datos
								 ).FirstOrDefault();

				return respuesta;
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
					throw new ApplicationException("No se encontraron documentos para calcular el Cufe.");

				Ctl_Empresa Peticion = new Ctl_Empresa();

				//Válida que la key sea correcta.
				TblEmpresas facturador_electronico = Peticion.Validar(documentos.FirstOrDefault().DataKey, documentos.FirstOrDefault().IdentificacionObligado);

				if (!facturador_electronico.IntObligado)
					throw new ApplicationException(string.Format("Licencia inválida para la Identificacion {0}.", facturador_electronico.StrIdentificacion));

				PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

				//---Ambiente de la DIAN al que se va enviar el documento: 1 - Produccion, 2 - Pruebas
				string ambiente_dian = string.Empty;

				if (plataforma_datos.RutaPublica.Contains("habilitacion") || plataforma_datos.RutaPublica.Contains("localhost"))
					ambiente_dian = "2";
				else
					ambiente_dian = "1";

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
							ruta_validar_doc = string.Format("{0}{1}", Constantes.RutaPaginaQRHabilitacionDIAN, item.Cufe);
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
		public TblDocumentos ActualizarRespuestaAcuse(System.Guid id_seguridad, short estado, string motivo_rechazo, string usuario = "")
		{
			try
			{
				//List<TblDocumentos> retorno = new List<TblDocumentos>();
				TblDocumentos doc = ObtenerPorIdSeguridad(id_seguridad).FirstOrDefault();

				Ctl_Empresa ctl_empresa = new Ctl_Empresa();

				if (doc == null)
					throw new ArgumentException(string.Format(LibreriaGlobalHGInet.Properties.RecursoMensajes.ObjectNotExistError, "el documento con Id Seguridad", id_seguridad.ToString()));

				bool evento_procesado_DIAN = false;

				Ctl_EventosRadian evento = new Ctl_EventosRadian();
				FacturaE_Documento resultado = null;

				// obtiene los datos del facturador electrónico
				TblEmpresas facturador = null;
				if (doc.TblEmpresasFacturador == null)
					facturador = ctl_empresa.Obtener(doc.StrEmpresaFacturador, false);
				else
					facturador = doc.TblEmpresasFacturador;

				facturador.IntVersionDian = 2;

				// obtiene los datos del adquiriente
				TblEmpresas adquiriente = null;
				ctl_empresa = new Ctl_Empresa();
				if (doc.TblEmpresasAdquiriente == null)
					adquiriente = ctl_empresa.Obtener(doc.StrEmpresaAdquiriente, false);
				else
					adquiriente = doc.TblEmpresasAdquiriente;

				adquiriente.IntVersionDian = 2;

				//obtiene los datos del proveedor del facturador
				ctl_empresa = new Ctl_Empresa();
				TblEmpresas proveedor_emisor = ctl_empresa.Obtener(doc.StrProveedorEmisor, false);

				//Si el adquiriente es de HGI es por que es Facturador, esta habilitado y se puede generar eventos basicos en la DIAN
				bool adquiriente_hgi = (adquiriente.IntHabilitacion > Habilitacion.Valida_Objeto.GetHashCode() && adquiriente.IntObligado == true && adquiriente.IntAdquiriente == true && adquiriente.IntIdEstado == EstadoEmpresa.ACTIVA.GetHashCode()) ? true : false;

				bool continuar_proceso = true;

				List<TblEventosRadian> list_evento = null;

				TblEventosRadian acuse = null;

				TblEventosRadian recibo = null;

				if (adquiriente_hgi == true)
				{
					list_evento = evento.Obtener(doc.StrIdSeguridad);
				   
					if (list_evento != null && list_evento.Count > 0)
					{

						acuse = list_evento.Where(x => x.IntEstadoEvento == 1).FirstOrDefault();

						if (acuse != null && estado == CodigoResponseV2.Recibido.GetHashCode())
							continuar_proceso = false;
					}
				}
				else if (estado != CodigoResponseV2.AprobadoTacito.GetHashCode() && estado != CodigoResponseV2.Inscripcion.GetHashCode())
				{
					continuar_proceso = false;
				}


				if (continuar_proceso == true)
				{
					if (!estado.Equals(CodigoResponseV2.AprobadoTacito.GetHashCode()))
					{
						if (estado == CodigoResponseV2.Rechazado.GetHashCode() && !string.IsNullOrEmpty(motivo_rechazo))
							doc.StrAdquirienteMvoRechazo = motivo_rechazo;
						doc.DatAdquirienteFechaRecibo = Fecha.GetFecha();
						if (doc.IntIdEstado > (short)ProcesoEstado.EnvioZip.GetHashCode() && estado != CodigoResponseV2.Recibido.GetHashCode())
							doc.IntIdEstado = (short)ProcesoEstado.RecepcionAcuse.GetHashCode();
					}

					//Valida si son pruebas para habilitarse en RADIAN para que sea Automatico
					bool habilitacion_radian = (facturador.StrIdentificacion.Equals(adquiriente.StrIdentificacion) && adquiriente.IntHabilitacion != Habilitacion.Produccion.GetHashCode() && !facturador.StrIdentificacion.Equals(Constantes.NitResolucionconPrefijo)) ? true : false;

					//Crea el XML del Acuse
					if (acuse == null && estado != CodigoResponseV2.AprobadoTacito.GetHashCode() && habilitacion_radian == false)
						resultado = Ctl_Documento.ConvertirAcuse(doc, facturador, adquiriente, (short)CodigoResponseV2.Recibido.GetHashCode(), motivo_rechazo);

					//Se valida si el adquiriente se esta habilitando en RADIAN o si va a registrar los eventos de Acuse en la DIAN
					if (estado != CodigoResponseV2.AprobadoTacito.GetHashCode() && habilitacion_radian == false)
					{

						if ((list_evento == null || list_evento.Count == 0) && estado == CodigoResponseV2.Recibido.GetHashCode())
						{
							acuse = EnviarAcuse(resultado, adquiriente, facturador, doc, (short)estado);
							if (acuse != null)
								evento_procesado_DIAN = true;
						}
						else if ((list_evento == null || list_evento.Count == 0) && estado != CodigoResponseV2.Recibido.GetHashCode())
						{
							try
							{
								acuse = EnviarAcuse(resultado, adquiriente, facturador, doc, (short)CodigoResponseV2.Recibido.GetHashCode());
							}
							catch (Exception excepcion)
							{
								RegistroLog.EscribirLog(excepcion, MensajeCategoria.ServicioDian, MensajeTipo.Error, MensajeAccion.envio, string.Format("Error procesando o enviando Evento {0}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.CodigoResponseV2>(CodigoResponseV2.Recibido.GetHashCode()))));

								throw new ApplicationException(excepcion.Message, excepcion.InnerException);
							}

							try
							{
								//Crea el XML del Acuse
								if (acuse != null)
								{
									resultado = Ctl_Documento.ConvertirAcuse(doc, facturador, adquiriente, (short)CodigoResponseV2.Aceptado.GetHashCode(), motivo_rechazo);
									recibo = EnviarAcuse(resultado, adquiriente, facturador, doc, (short)CodigoResponseV2.Aceptado.GetHashCode());
									if (recibo != null)
										evento_procesado_DIAN = true;
								}

							}
							catch (Exception)
							{

							}

							if (recibo != null && estado != CodigoResponseV2.Aceptado.GetHashCode())
							{
								//Crea el XML del Acuse
								resultado = Ctl_Documento.ConvertirAcuse(doc, facturador, adquiriente, estado, motivo_rechazo);
								TblEventosRadian evento_x = EnviarAcuse(resultado, adquiriente, facturador, doc, estado);
								if (evento_x != null)
									evento_procesado_DIAN = true;
							}
						}
						else if (list_evento != null && list_evento.Count > 0 && estado != CodigoResponseV2.Recibido.GetHashCode())
						{

							acuse = list_evento.Where(x => x.IntEstadoEvento == 1).FirstOrDefault();

							recibo = list_evento.Where(x => x.IntEstadoEvento == 4).FirstOrDefault();

							TblEventosRadian expresa = list_evento.Where(x => x.IntEstadoEvento == 5).FirstOrDefault();

							TblEventosRadian rechazo = list_evento.Where(x => x.IntEstadoEvento == 2).FirstOrDefault();

							TblEventosRadian tacito = list_evento.Where(x => x.IntEstadoEvento == 3).FirstOrDefault();

							if (acuse == null && estado != CodigoResponseV2.Recibido.GetHashCode())
							{
								acuse = EnviarAcuse(resultado, adquiriente, facturador, doc, (short)CodigoResponseV2.Recibido.GetHashCode());
							}


							if (recibo == null && estado != CodigoResponseV2.Recibido.GetHashCode())
							{
								//Crea el XML del Acuse
								resultado = Ctl_Documento.ConvertirAcuse(doc, facturador, adquiriente, (short)CodigoResponseV2.Aceptado.GetHashCode(), motivo_rechazo);
								recibo = EnviarAcuse(resultado, adquiriente, facturador, doc, (short)CodigoResponseV2.Aceptado.GetHashCode());
								evento_procesado_DIAN = true;
							}
							else if (recibo != null && !doc.IntAdquirienteRecibo.Equals(estado))
							{
								doc.IntAdquirienteRecibo = (short)CodigoResponseV2.Recibido.GetHashCode();
							}

							if (expresa == null && rechazo == null && tacito == null && estado != CodigoResponseV2.Aceptado.GetHashCode())
							{
								//Crea el XML del Acuse
								resultado = Ctl_Documento.ConvertirAcuse(doc, facturador, adquiriente, estado, motivo_rechazo);

								if (estado == CodigoResponseV2.Expresa.GetHashCode())
								{
									expresa = EnviarAcuse(resultado, adquiriente, facturador, doc, estado);
									if (expresa != null)
										evento_procesado_DIAN = true;
								}


								if (estado == CodigoResponseV2.Rechazado.GetHashCode())
								{
									rechazo = EnviarAcuse(resultado, adquiriente, facturador, doc, estado);
									if (rechazo != null)
										evento_procesado_DIAN = true;
								}


							}

							//Si el documento ya tiene eventos y por algun proceso no se actualizo en el documento, aqui se hace.
							if (evento_procesado_DIAN == false && (expresa != null || rechazo != null || tacito != null))
							{
								
								if (expresa != null && !doc.IntAdquirienteRecibo.Equals(estado))
								{
									doc.IntAdquirienteRecibo = (short)CodigoResponseV2.Expresa.GetHashCode();
								}

								if (rechazo != null && !doc.IntAdquirienteRecibo.Equals(estado))
								{
									doc.IntAdquirienteRecibo = (short)CodigoResponseV2.Rechazado.GetHashCode();
								}

								if (tacito != null && !doc.IntAdquirienteRecibo.Equals(estado))
								{
									doc.IntAdquirienteRecibo = (short)CodigoResponseV2.AprobadoTacito.GetHashCode();
								}

								TblEventosRadian inscripcion_titulo = list_evento.Where(x => x.IntEstadoEvento == 6).FirstOrDefault();

								if (inscripcion_titulo != null && !doc.IntAdquirienteRecibo.Equals((short)CodigoResponseV2.Inscripcion.GetHashCode()))
								{
									doc.IntAdquirienteRecibo = (short)CodigoResponseV2.Inscripcion.GetHashCode();
								}

								doc.DatFechaActualizaEstado = Fecha.GetFecha();

								Ctl_Documento actualizar_doc = new Ctl_Documento();
								doc = actualizar_doc.Actualizar(doc);

							}

							if (estado == CodigoResponseV2.Inscripcion.GetHashCode() && rechazo == null && (expresa != null || tacito != null))
							{
								//Crea el XML del Acuse
								resultado = Ctl_Documento.ConvertirAcuse(doc, facturador, adquiriente, (short)CodigoResponseV2.Inscripcion.GetHashCode(), motivo_rechazo);
								TblEventosRadian inscripcion_titulo = EnviarAcuse(resultado, adquiriente, facturador, doc, (short)CodigoResponseV2.Inscripcion.GetHashCode());
								if (inscripcion_titulo != null)
									evento_procesado_DIAN = true;
							}

						}
					}
					else if (estado == CodigoResponseV2.AprobadoTacito.GetHashCode() && habilitacion_radian == false)
					{
						TblEventosRadian tacito = null;
						//Crea el XML del Acuse
						resultado = Ctl_Documento.ConvertirAcuse(doc, facturador, adquiriente, (short)CodigoResponseV2.AprobadoTacito.GetHashCode(), motivo_rechazo);
						if (facturador.IntRadian == true)
						{
							tacito = EnviarAcuse(resultado, adquiriente, facturador, doc, (short)CodigoResponseV2.AprobadoTacito.GetHashCode());
							if (tacito == null)
							{
								throw new ArgumentException("No fue posible registrar el evento Aprobado Tacito");
							}
							else
							{
								evento_procesado_DIAN = true;
								doc.IntAdquirienteRecibo = (short)CodigoResponseV2.AprobadoTacito.GetHashCode();
							}
						}

					}
					else if (habilitacion_radian == true)
					{

						HabilitarRadian(ref resultado, adquiriente, facturador, proveedor_emisor, ref doc);
						estado = (short)CodigoResponseV2.Inscripcion.GetHashCode();
						evento_procesado_DIAN = true;
					}

					try
					{
						// envía el correo del acuse de recibo al facturador electrónico
						if ((estado == CodigoResponseV2.Expresa.GetHashCode() || estado == CodigoResponseV2.Rechazado.GetHashCode()) && adquiriente_hgi == true && evento_procesado_DIAN == true)
						{
							//********el adjunto debe ser un attach document con la aceptacion expresa o con el rechazo y la respuesta de la DIAN
							Ctl_EnvioCorreos email = new Ctl_EnvioCorreos();
							doc.StrUrlAcuseUbl = resultado.RutaArchivosProceso;
							email.RespuestaAcuse(doc, facturador, adquiriente, resultado.RutaArchivosProceso, "", Procedencia.Usuario, usuario, estado, resultado.CUFE);
						}
						//if (doc.IntIdEstado > (short)ProcesoEstado.EnvioZip.GetHashCode() && estado != CodigoResponseV2.Recibido.GetHashCode())
						//	doc.IntIdEstado = (short)ProcesoEstado.Finalizacion.GetHashCode();
					}
					catch (Exception) { }

					if (resultado != null && evento_procesado_DIAN == true)//&& (estado == CodigoResponseV2.Recibido.GetHashCode() || estado == CodigoResponseV2.AprobadoTacito.GetHashCode() || estado == CodigoResponseV2.Expresa.GetHashCode() || estado == CodigoResponseV2.Inscripcion.GetHashCode() || estado == CodigoResponseV2.Rechazado.GetHashCode()))
					{
						doc.DatFechaActualizaEstado = Fecha.GetFecha();
						doc.StrUrlAcuseUbl = resultado.RutaArchivosProceso;

						if (estado != CodigoResponseV2.Rechazado.GetHashCode() && !string.IsNullOrEmpty(doc.StrAdquirienteMvoRechazo))
							doc.StrAdquirienteMvoRechazo = string.Empty;

						Ctl_Documento actualizar_doc = new Ctl_Documento();
						doc = actualizar_doc.Actualizar(doc);

						doc.TblEmpresasFacturador = facturador;
						doc.TblEmpresasAdquiriente = adquiriente;

						try
						{
							Ctl_DocumentosAudit clase_auditoria = new Ctl_DocumentosAudit();
							int estado_doc = Ctl_Documento.ObtenerCategoria(doc.IntIdEstado);
							clase_auditoria.Crear(doc.StrIdSeguridad, new Guid(), facturador.StrIdentificacion, ProcesoEstado.RecepcionAcuse, TipoRegistro.Proceso, Procedencia.Usuario, usuario, Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.ProcesoEstado>(ProcesoEstado.RecepcionAcuse.GetHashCode())), Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.CodigoResponseV2>(doc.IntAdquirienteRecibo)), doc.StrPrefijo, Convert.ToString(doc.IntNumero), estado_doc);
						}
						catch (Exception) { throw; }
					}

				}

				//retorno.Add(doc);

				return doc;
			}
			catch (Exception excepcion)
			{
				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.consulta);

				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		#endregion

		/// <summary>
		///Si se esta habilitando el RADIAN se ejecutan todos los demas Eventos automaticamente para cumplir con el set de pruebas que es uno por cada evento
		///Codigo Evento - Nombre evento
		///1 - Acuse Recibo, 4 - Recibo Mercancia, 5 - Aceptacion Expresa, 6 - Inscripción de la factura electrónica de venta como título valor, 7 - Endoso en propiedad
		/// </summary>
		/// <param name="resultado"></param>
		/// <param name="adquiriente"></param>
		/// <param name="facturador"></param>
		/// <param name="proveedor"></param>
		/// <param name="doc"></param>
		public void HabilitarRadian (ref FacturaE_Documento resultado, TblEmpresas adquiriente, TblEmpresas facturador, TblEmpresas proveedor, ref TblDocumentos doc)
		{

			Ctl_EventosRadian evento = new Ctl_EventosRadian();

			List<TblEventosRadian> list_evento = evento.Obtener(doc.StrIdSeguridad);

			TblEventosRadian acuse = null;

			TblEventosRadian recibo = null;

			TblEventosRadian expresa = null;

			TblEventosRadian inscripcion = null;

			if (list_evento != null && list_evento.Count > 0)
			{

				acuse = list_evento.Where(x => x.IntEstadoEvento == 1).FirstOrDefault();

				recibo = list_evento.Where(x => x.IntEstadoEvento == 4).FirstOrDefault();

				expresa = list_evento.Where(x => x.IntEstadoEvento == 5).FirstOrDefault();

				inscripcion = list_evento.Where(x => x.IntEstadoEvento == 6).FirstOrDefault();

			}

			if (acuse == null)
			{
				try
				{
					//Crea el XML del Acuse
					resultado = Ctl_Documento.ConvertirAcuse(doc, facturador, adquiriente, (short)CodigoResponseV2.Recibido.GetHashCode(), string.Empty);
					EnviarAcuse(resultado, adquiriente, facturador, doc, (short)CodigoResponseV2.Recibido.GetHashCode());
				}
				catch (Exception excepcion)
				{
					RegistroLog.EscribirLog(excepcion, MensajeCategoria.ServicioDian, MensajeTipo.Error, MensajeAccion.envio, string.Format("Error procesando o enviando Evento {0}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.CodigoResponseV2>(CodigoResponseV2.Recibido.GetHashCode()))));

					throw new ApplicationException(excepcion.Message, excepcion.InnerException);
				}

				doc.IntAdquirienteRecibo = (short)CodigoResponseV2.Recibido.GetHashCode();
			}
			
			if (recibo == null)
			{
				try
				{
					//Crea el XML del Acuse
					resultado = Ctl_Documento.ConvertirAcuse(doc, facturador, adquiriente, (short)CodigoResponseV2.Aceptado.GetHashCode(), string.Empty);
					EnviarAcuse(resultado, adquiriente, facturador, doc, (short)CodigoResponseV2.Aceptado.GetHashCode());
				}
				catch (Exception excepcion)
				{
					RegistroLog.EscribirLog(excepcion, MensajeCategoria.ServicioDian, MensajeTipo.Error, MensajeAccion.envio, string.Format("Error procesando o enviando Evento {0}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.CodigoResponseV2>(CodigoResponseV2.Aceptado.GetHashCode()))));

					throw new ApplicationException(excepcion.Message, excepcion.InnerException);
				}

				doc.IntAdquirienteRecibo = (short)CodigoResponseV2.Aceptado.GetHashCode();
			}
			
			if (expresa == null)
			{
				try
				{
					//Crea el XML del Acuse
					resultado = Ctl_Documento.ConvertirAcuse(doc, facturador, adquiriente, (short)CodigoResponseV2.Expresa.GetHashCode(), string.Empty);
					EnviarAcuse(resultado, adquiriente, facturador, doc, (short)CodigoResponseV2.Expresa.GetHashCode());
				}
				catch (Exception excepcion)
				{
					RegistroLog.EscribirLog(excepcion, MensajeCategoria.ServicioDian, MensajeTipo.Error, MensajeAccion.envio, string.Format("Error procesando o enviando Evento {0}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.CodigoResponseV2>(CodigoResponseV2.Expresa.GetHashCode()))));

					throw new ApplicationException(excepcion.Message, excepcion.InnerException);
				}

				doc.IntAdquirienteRecibo = (short)CodigoResponseV2.Expresa.GetHashCode();
			}

			if (inscripcion == null)
			{
				try
				{
					//Crea el XML del Acuse
					resultado = Ctl_Documento.ConvertirAcuse(doc, facturador, adquiriente, (short)CodigoResponseV2.Inscripcion.GetHashCode(), string.Empty);
					EnviarAcuse(resultado, adquiriente, facturador, doc, (short)CodigoResponseV2.Inscripcion.GetHashCode());
				}
				catch (Exception excepcion)
				{
					RegistroLog.EscribirLog(excepcion, MensajeCategoria.ServicioDian, MensajeTipo.Error, MensajeAccion.envio, string.Format("Error procesando o enviando Evento {0}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.CodigoResponseV2>(CodigoResponseV2.Inscripcion.GetHashCode()))));

					throw new ApplicationException(excepcion.Message, excepcion.InnerException);
				}

				doc.IntAdquirienteRecibo = (short)CodigoResponseV2.Inscripcion.GetHashCode();
			}


			//try
			//{
			//	//Crea el XML del Acuse
			//	resultado = Ctl_Documento.ConvertirAcuse(doc, facturador, adquiriente, (short)CodigoResponseV2.EndosoPp.GetHashCode(), string.Empty);
			//	EnviarAcuse(resultado, adquiriente, facturador, doc, (short)CodigoResponseV2.EndosoPp.GetHashCode());
			//}
			//catch (Exception excepcion)
			//{
			//	RegistroLog.EscribirLog(excepcion, MensajeCategoria.ServicioDian, MensajeTipo.Error, MensajeAccion.envio, string.Format("Error procesando o enviando Evento {0}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.CodigoResponseV2>(CodigoResponseV2.EndosoPp.GetHashCode()))));

			//	throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			//}

		}

		public TblEventosRadian EnviarAcuse(FacturaE_Documento resultado, TblEmpresas adquiriente, TblEmpresas facturador, TblDocumentos doc, short estado)
		{

			TblEventosRadian eventobd = null;

			try
			{

				// ruta del zip
				string ruta_zip = string.Format(@"{0}\{1}.zip", resultado.RutaArchivosEnvio, resultado.NombreZip);

				// elimina el archivo xml si existe
				if (Archivo.ValidarExistencia(ruta_zip))
					Archivo.Borrar(ruta_zip);

				// genera la compresión del archivo en zip
				using (ZipArchive archive = ZipFile.Open(ruta_zip, ZipArchiveMode.Update))
				{
					archive.CreateEntryFromFile(string.Format(@"{0}\{1}.xml", resultado.RutaArchivosEnvio, resultado.NombreXml), Path.GetFileName(string.Format("{0}.xml", resultado.NombreXml)));
					archive.Dispose();
				}

				DocumentoRespuesta resp = new DocumentoRespuesta();

				HGInetDIANServicios.DianFactura.AcuseRecibo acuse = new HGInetDIANServicios.DianFactura.AcuseRecibo();

				//Se obtiene la resolucion del documento para usar el set id de pruebas por si son pruebas
				if (doc.TblEmpresasResoluciones == null)
				{
					TblEmpresasResoluciones resolucion = null;
					Ctl_EmpresaResolucion ctrl = new Ctl_EmpresaResolucion();
					resolucion = ctrl.Obtener(adquiriente.StrIdentificacion, doc.StrNumResolucion, doc.StrPrefijo);
					if (resolucion != null)
						doc.TblEmpresasResoluciones = resolucion;

				}

				//Se hace envio del evento a la DIAN
				acuse = ServiciosDian.Ctl_DocumentoDian.Enviar(resultado, doc, facturador, ref resp, doc.TblEmpresasResoluciones.StrIdSetDian, false, estado);

				//Se valida si esta haciendo pruebas para el Radian para consultar el resultado del envio
				bool habilitar_set = true;

				PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

				if (plataforma_datos.RutaPublica.Contains("habilitacion") || plataforma_datos.RutaPublica.Contains("localhost"))
				{
					if (facturador.IntRadian == true && !adquiriente.StrIdentificacion.Equals(facturador.StrIdentificacion) && !adquiriente.StrIdentificacion.Equals(Constantes.NitResolucionconPrefijo))
					{
						habilitar_set = false;
					}
				}
				else
				{
					habilitar_set = false;
				}

				if (habilitar_set == true && !string.IsNullOrEmpty(acuse.KeyV2))
				{
					//acuse = ServiciosDian.Ctl_DocumentoDian.Enviar(resultado, doc, facturador, ref resp, doc.TblEmpresasResoluciones.StrIdSetDian, false, estado);
					resp.DocumentoTipo = TipoDocumento.AcuseRecibo.GetHashCode();

					//Se da una pausa en proceso para que el servicio de la DIAN termine la validacion del documento
					System.Threading.Thread.Sleep(5000);

					Ctl_Documentos.Consultar(doc, facturador, ref resp, acuse.KeyV2);
				}

				if (resp.EstadoDian != null &&  resp.EstadoDian.EstadoDocumento == EstadoDocumentoDian.Aceptado.GetHashCode())
				{
					var acuse_obj = (dynamic)null;
					acuse_obj = resultado.Documento;
					long IdAcuse = Convert.ToInt64(acuse_obj.IdAcuse);
					DateTime fecha_acuse = acuse.ReceivedDateTime;
					Ctl_EventosRadian evento = new Ctl_EventosRadian();
					eventobd = evento.Convertir(doc.StrIdSeguridad, estado, IdAcuse, fecha_acuse);
					string ruta_respuestaDian = resultado.RutaArchivosProceso.Replace(resultado.NombreXml, string.Format("{0}-{1}", resultado.NombreXml, estado));
					eventobd.StrUrlEvento = ruta_respuestaDian;
					evento.Crear(eventobd);
					//RegistroLog.EscribirLog(new ApplicationException("Creacion del Evento"), MensajeCategoria.ServicioDian, MensajeTipo.Error, MensajeAccion.envio, string.Format("Codigo Estado Doc {0} - Codigo respuesta DIAN {1} - descripcion {2}", resp.EstadoDian.EstadoDocumento, resp.EstadoDian.CodigoRespuesta, resp.EstadoDian.Descripcion));
				}

				resp.Documento = estado;
				resp.DocumentoTipo = TipoDocumento.AcuseRecibo.GetHashCode();

			}
			catch (Exception excepcion)
			{
				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.consulta);
			}

			return eventobd;
		}

		/// <summary>
		/// Convierte respuesta a un objeto de Acuse y Ubl
		/// </summary>
		/// <param name="doc">Documento al que dieron acuse</param>
		/// <param name="facturador">Informacion del emisor del documento electronico </param>
		/// <param name="adquiriente">Informacion del receptor del documento electronico</param>
		/// <param name="estado">estado del Acuse</param>
		/// <param name="motivo_rechazo">descripcion del acuse</param>
		/// <returns></returns>
		public static FacturaE_Documento ConvertirAcuse(TblDocumentos doc, TblEmpresas facturador, TblEmpresas adquiriente, short estado, string motivo_rechazo)
		{
			PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

			//Crea el Acuse
			Acuse doc_acuse = new Acuse();
			CodigoResponseV2 cod_acuse = Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.CodigoResponseV2>(estado);
			switch (cod_acuse)
			{
				case CodigoResponseV2.Recibido:
					doc_acuse.IdAcuse = string.Format("{0}1", doc.IntNumero);
					doc_acuse.CodigoRespuesta = Enumeracion.GetAmbiente(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.CodigoResponseV2>(estado));
					break;
				case CodigoResponseV2.Rechazado:
					doc_acuse.IdAcuse = string.Format("{0}2", doc.IntNumero);
					doc_acuse.CodigoRespuesta = Enumeracion.GetAmbiente(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.CodigoResponseV2>(estado));
					break;
				case CodigoResponseV2.AprobadoTacito:
					doc_acuse.IdAcuse = string.Format("{0}3", doc.IntNumero);
					doc_acuse.CodigoRespuesta = Enumeracion.GetAmbiente(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.CodigoResponseV2>(estado));
					break;
				case CodigoResponseV2.Aceptado:
					doc_acuse.IdAcuse = string.Format("{0}4", doc.IntNumero);
					doc_acuse.CodigoRespuesta = Enumeracion.GetAmbiente(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.CodigoResponseV2>(estado));
					break;
				case CodigoResponseV2.Expresa:
					doc_acuse.IdAcuse = string.Format("{0}5", doc.IntNumero);
					doc_acuse.CodigoRespuesta = Enumeracion.GetAmbiente(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.CodigoResponseV2>(estado));
					break;
				case CodigoResponseV2.Inscripcion:
					var randomNumber = new Random().Next(0, 10);
					doc_acuse.IdAcuse = string.Format("{0}6{1}", doc.IntNumero, randomNumber);
					doc_acuse.CodigoRespuesta = Enumeracion.GetAmbiente(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.CodigoResponseV2>(estado));
					break;
				case CodigoResponseV2.EndosoPp:
					doc_acuse.IdAcuse = string.Format("{0}7", doc.IntNumero);
					doc_acuse.CodigoRespuesta = Enumeracion.GetAmbiente(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.CodigoResponseV2>(estado));
					break;
				case CodigoResponseV2.EndosoG:
					doc_acuse.IdAcuse = string.Format("{0}8", doc.IntNumero);
					doc_acuse.CodigoRespuesta = Enumeracion.GetAmbiente(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.CodigoResponseV2>(estado));
					break;
				case CodigoResponseV2.CancelacionEG:
					doc_acuse.IdAcuse = string.Format("{0}9", doc.IntNumero);
					doc_acuse.CodigoRespuesta = Enumeracion.GetAmbiente(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.CodigoResponseV2>(estado));
					break;
				case CodigoResponseV2.Aval:
					doc_acuse.IdAcuse = string.Format("{0}12", doc.IntNumero);
					doc_acuse.CodigoRespuesta = Enumeracion.GetAmbiente(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.CodigoResponseV2>(estado));
					break;
				case CodigoResponseV2.EndosoPc:
					doc_acuse.IdAcuse = string.Format("{0}15", doc.IntNumero);
					doc_acuse.CodigoRespuesta = Enumeracion.GetAmbiente(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.CodigoResponseV2>(estado));
					break;
				case CodigoResponseV2.MandatoG:
					doc_acuse.IdAcuse = string.Format("{0}20", doc.IntNumero);
					doc_acuse.CodigoRespuesta = Enumeracion.GetAmbiente(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.CodigoResponseV2>(estado));
					break;
				case CodigoResponseV2.TerminacionMandatoG:
					doc_acuse.IdAcuse = string.Format("{0}21", doc.IntNumero);
					doc_acuse.CodigoRespuesta = Enumeracion.GetAmbiente(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.CodigoResponseV2>(estado));
					break;
				case CodigoResponseV2.PagoFvTV:
					doc_acuse.IdAcuse = string.Format("{0}22", doc.IntNumero);
					doc_acuse.CodigoRespuesta = Enumeracion.GetAmbiente(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.CodigoResponseV2>(estado));
					break;
				case CodigoResponseV2.InformePago:
					doc_acuse.IdAcuse = string.Format("{0}23", doc.IntNumero);
					doc_acuse.CodigoRespuesta = Enumeracion.GetAmbiente(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.CodigoResponseV2>(estado));
					break;
				default:
					break;
			}

			doc_acuse.IdSeguridad = doc.StrIdSeguridad.ToString();
			doc_acuse.Documento = doc.IntNumero;
			doc_acuse.Prefijo = doc.StrPrefijo;
			doc_acuse.MvoRespuesta = motivo_rechazo;
			doc_acuse.Fecha = Convert.ToDateTime(doc.DatAdquirienteFechaRecibo);

			doc_acuse.TipoDocumento = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.DocumentTypeV2>(doc.IntDocTipo));
			doc_acuse.DatosAdquiriente = Ctl_Empresa.Convertir(adquiriente);
			if (doc_acuse.DatosAdquiriente.TipoIdentificacion != 31)
			{
				doc_acuse.DatosAdquiriente.TipoIdentificacion = 31;
				doc_acuse.DatosAdquiriente.IdentificacionDv = LibreriaGlobalHGInet.HgiNet.FuncionesIdentificacion.Dv(adquiriente.StrIdentificacion);
			}
			
			doc_acuse.DatosObligado = Ctl_Empresa.Convertir(facturador);
			if (doc_acuse.DatosObligado.TipoIdentificacion != 31)
			{
				doc_acuse.DatosObligado.TipoIdentificacion = 31;
				doc_acuse.DatosObligado.IdentificacionDv = LibreriaGlobalHGInet.HgiNet.FuncionesIdentificacion.Dv(adquiriente.StrIdentificacion);
			}

			//---Ambiente de la DIAN al que se va enviar el documento: 1 - Produccion, 2 - Pruebas
			string ambiente_dian = string.Empty;

			if (!plataforma_datos.RutaPublica.Contains("habilitacion") && !plataforma_datos.RutaPublica.Contains("localhost"))
				ambiente_dian = "1";
			else
				ambiente_dian = "2";

			// para el ambiente de habilitacion se toma la informacion de aqui
			//DianProveedorV2 data_dian__habilitacion = HgiConfiguracion.GetConfiguration().DianProveedorV2;

			//Para el RADIAN se toma la informacion de aqui
			DianProveedor data_dian = HgiConfiguracion.GetConfiguration().DianProveedor;

			string IdSoftware = data_dian.IdSoftware;
			string PinSoftware = data_dian.Pin;
			string NitProveedor = data_dian.NitProveedor;

			//sobre escribe los datos de la resolución si se encuentra en estado de habilitación
			//if (!plataforma_datos.RutaPublica.Contains("habilitacion"))
			//{
			//	IdSoftware = data_dian_habilitacion.IdSoftware;
			//	PinSoftware = data_dian_habilitacion.Pin;
			//	NitProveedor = data_dian_habilitacion.NitProveedor;
			//}

			// resolución del documento
			HGInetMiFacturaElectonicaData.ModeloServicio.ExtensionDian extension_documento = new HGInetMiFacturaElectonicaData.ModeloServicio.ExtensionDian();

			// convierte la información de la resolución a la extensión DIAN
			extension_documento = new HGInetMiFacturaElectonicaData.ModeloServicio.ExtensionDian()
			{
				IdSoftware = IdSoftware,
				NitProveedor = NitProveedor,
				PinSoftware = PinSoftware
			};

			TblEventosRadian evento_anterior = null;
			if (estado == CodigoResponseV2.CancelacionEG.GetHashCode())
			{
				Ctl_EventosRadian ctl = new Ctl_EventosRadian();
				List<TblEventosRadian> list_event = ctl.Obtener(doc.StrIdSeguridad);
				TblEventosRadian evento_endosoG = list_event.Where(x => x.IntEstadoEvento == CodigoResponseV2.EndosoG.GetHashCode()).FirstOrDefault();
				TblEventosRadian evento_endosoPc = list_event.Where(x => x.IntEstadoEvento == CodigoResponseV2.EndosoPc.GetHashCode()).FirstOrDefault();
				TblEventosRadian evento_endosoPP = list_event.Where(x => x.IntEstadoEvento == CodigoResponseV2.EndosoPp.GetHashCode()).FirstOrDefault();

				if (evento_endosoG != null && evento_endosoPc == null && evento_endosoPP == null)
				{
					evento_anterior = evento_endosoG;
				}
				else if (evento_endosoG == null && evento_endosoPc != null && evento_endosoPP == null)
				{
					evento_anterior = evento_endosoPc;
				}
				else if (evento_endosoG == null && evento_endosoPc == null && evento_endosoPP != null)
				{
					evento_anterior = evento_endosoPP;
				}
			}

			if (estado == CodigoResponseV2.TerminacionMandatoG.GetHashCode())
			{
				Ctl_EventosRadian ctl = new Ctl_EventosRadian();
				List<TblEventosRadian> list_event = ctl.Obtener(doc.StrIdSeguridad);
				TblEventosRadian evento_mandato = list_event.Where(x => x.IntEstadoEvento == CodigoResponseV2.MandatoG.GetHashCode()).FirstOrDefault();

				if (evento_mandato != null)
				{
					evento_anterior = evento_mandato;
				}
			}

			//Convierte el objeto en archivo XML-UBL
			FacturaE_Documento resultado = HGInetUBLv2_1.AcuseReciboXMLv2_1.CrearDocumento(doc_acuse, ambiente_dian, PinSoftware, doc.StrCufe, extension_documento, doc, cod_acuse, evento_anterior);
			resultado.IdSeguridadTercero = facturador.StrIdSeguridad;
			resultado.IdSeguridadDocumento = doc.StrIdSeguridad;
			resultado.IdSeguridadPeticion = Guid.NewGuid();
			resultado.DocumentoTipo = TipoDocumento.AcuseRecibo;
			resultado.VersionDian = 2;
			resultado.NombreZip = resultado.NombreXml;

			// valida el nodo de ExtensionContent
			if (cod_acuse.GetHashCode() < CodigoResponseV2.Inscripcion.GetHashCode() || cod_acuse.Equals(CodigoResponseV2.CancelacionEG) || cod_acuse.Equals(CodigoResponseV2.MandatoG) || cod_acuse.Equals(CodigoResponseV2.TerminacionMandatoG))
			{
				resultado.DocumentoXml = HGInetUBL.ExtensionDian.ValidarNodo(resultado.DocumentoXml);
			}
			else
			{
				resultado.DocumentoXml = HGInetUBL.ExtensionDian.ValidarNodo(resultado.DocumentoXml, 1);
			}


			// ruta física del xml
			string carpeta_xml = string.Format("{0}\\{1}\\{2}", plataforma_datos.RutaDmsFisica, Constantes.CarpetaFacturaElectronica, resultado.IdSeguridadTercero.ToString());
			carpeta_xml = string.Format(@"{0}\{1}", carpeta_xml, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaXmlFacturaE);

			Directorio.CrearDirectorio(carpeta_xml.Replace("XmlFacturaE", "XmlAcuse"));

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

			//Si el adquiriente es de HGI es por que es Facturador, esta habilitado y tiene la forma de firmar sea certificado propio o nuestro
			bool adquiriente_hgi = (adquiriente.IntHabilitacion > Habilitacion.Valida_Objeto.GetHashCode() && adquiriente.IntObligado == true && adquiriente.IntAdquiriente == true && adquiriente.IntIdEstado == EstadoEmpresa.ACTIVA.GetHashCode()) ? true : false;


			//Valida si el adquiriente del documento tiene certificado con nosotros para firmar el acuse con ese certificado
			if ((adquiriente.IntCertFirma > 0 || adquiriente_hgi == true))
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
		/// Proceso para consultar eventos en la DIAN de los documentos seleccionados o que cumplan las condiciones
		/// </summary>
		/// <param name="list_idsegudiridad"></param>
		public void ConsultarEventosRadian(bool sonda, string list_idsegudiridad)
		{
			Ctl_Empresa _empresa = new Ctl_Empresa();

			List<Guid> guid_documentos = new List<Guid>();

			if (sonda == true)
			{
				int estado_error = ProcesoEstado.FinalizacionErrorDian.GetHashCode();

				int Enviomail = ProcesoEstado.EnvioZip.GetHashCode();

				DateTime FechaActual = Fecha.GetFecha();				

				List<TblEmpresas> facturadores = _empresa.ObtenerEmpresaAcuse();

				context.Configuration.LazyLoadingEnabled = false;

				foreach (TblEmpresas item in facturadores)
				{
					try
					{
						guid_documentos = (from datos in context.TblDocumentos.AsNoTracking()
										   where (datos.IntAdquirienteRecibo.Equals(0) || datos.IntAdquirienteRecibo.Equals(1) || datos.IntAdquirienteRecibo.Equals(4)) && datos.IntDocTipo == 1 && datos.IntFormaPago == 2 && datos.IntIdEstado > Enviomail && datos.IntIdEstado < estado_error
												 && datos.StrEmpresaFacturador == item.StrIdentificacion
												 && datos.DatFechaIngreso <= SqlFunctions.DateAdd("hh", -1, FechaActual)
										   orderby datos.IntNumero descending
										   select datos.StrIdSeguridad).ToList();

					}
					catch (Exception exception)
					{
						RegistroLog.EscribirLog(exception, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.consulta);
					}
				}

			}
			else
			{
				List<string> lista_doc = Coleccion.ConvertirLista(list_idsegudiridad, ',');

				foreach (var item in lista_doc)
				{
					guid_documentos.Add(Guid.Parse(item));
				}
			}
			

			foreach (Guid item in guid_documentos)
			{

				//ActualizarRespuestaAcuse(item, Convert.ToInt16(ResponseCode.AprobadoTacito.GetHashCode()), string.Empty, string.Empty, true);

				TblDocumentos doc = ObtenerPorIdSeguridad(item).FirstOrDefault();

				if (doc == null)
					throw new ArgumentException(string.Format(LibreriaGlobalHGInet.Properties.RecursoMensajes.ObjectNotExistError, "el documento con Id Seguridad", item.ToString()));

				Ctl_EventosRadian evento = new Ctl_EventosRadian();

				List<TblEventosRadian> list_evento = null;

				TblEventosRadian acuse = null;

				TblEventosRadian recibo = null;

				TblEventosRadian expresa = null;

				TblEventosRadian rechazo = null;

				TblEventosRadian tacito = null;

				TblEventosRadian inscripcion = null;

				bool consulta_dian = true;

				list_evento = evento.Obtener(doc.StrIdSeguridad);

				if (list_evento != null && list_evento.Count > 0)
				{

					acuse = list_evento.Where(x => x.IntEstadoEvento == 1).FirstOrDefault();

					recibo = list_evento.Where(x => x.IntEstadoEvento == 4).FirstOrDefault();

					expresa = list_evento.Where(x => x.IntEstadoEvento == 5).FirstOrDefault();

					rechazo = list_evento.Where(x => x.IntEstadoEvento == 2).FirstOrDefault();

					tacito = list_evento.Where(x => x.IntEstadoEvento == 3).FirstOrDefault();

					inscripcion = list_evento.Where(x => x.IntEstadoEvento == 6).FirstOrDefault();

					if (rechazo != null || inscripcion != null)
					{
						consulta_dian = false;
					}

					if ((expresa != null || tacito != null) && sonda == true)
					{
						consulta_dian = false;
					}
				}

				//Primero consulto que tiempo de recepcion respecto a la fecha actual
				TimeSpan tiempo_transcurrido = Fecha.GetFecha().Subtract(doc.DatFechaIngreso);

				DateTime fecha_recibo = Fecha.GetFecha();
				if (doc.DatAdquirienteFechaRecibo != null)
					fecha_recibo = Convert.ToDateTime(doc.DatAdquirienteFechaRecibo);

				//Si lleva ese tiempo y aun esta en el mismo estado, lo consulto de nuevo a ver si ya tiene eventos
				if (tiempo_transcurrido.TotalHours >= 12 && tiempo_transcurrido.TotalHours < 16 && doc.IntAdquirienteRecibo <= 1 && sonda == true)
				{
					fecha_recibo = Fecha.GetFecha();
				}

				if (tiempo_transcurrido.TotalHours >= 24 && tiempo_transcurrido.TotalHours < 28 && doc.IntAdquirienteRecibo <= 1 && sonda == true)
				{
					fecha_recibo = Fecha.GetFecha();
				}

				if (tiempo_transcurrido.TotalHours >= 36 && tiempo_transcurrido.TotalHours < 40 && doc.IntAdquirienteRecibo <= 1 && sonda == true)
				{
					fecha_recibo = Fecha.GetFecha();
				}

				if (tiempo_transcurrido.TotalHours >= 48 && tiempo_transcurrido.TotalHours < 52 && doc.IntAdquirienteRecibo <= 1 && sonda == true)
				{
					fecha_recibo = Fecha.GetFecha();
				}

				if (tiempo_transcurrido.TotalHours >= 60 && tiempo_transcurrido.TotalHours < 64 && doc.IntAdquirienteRecibo <= 1 && sonda == true)
				{
					fecha_recibo = Fecha.GetFecha();
				}

				TimeSpan ultima_consulta = Fecha.GetFecha().Subtract(fecha_recibo);

				if (doc.IntAdquirienteRecibo == 0 && ultima_consulta.TotalHours >= 3 && sonda == true)
					consulta_dian = false;

				if (doc.IntAdquirienteRecibo == 1 && ultima_consulta.TotalHours >= 3 && sonda == true)
					consulta_dian = false;

				if (doc.IntAdquirienteRecibo == 4 && ultima_consulta.TotalHours >= 3 && sonda == true)
					consulta_dian = false;

				if (sonda == false && ultima_consulta.TotalMinutes <= 1 && consulta_dian == true && tiempo_transcurrido.TotalMinutes >= 10)
					consulta_dian = false;

				if (consulta_dian == true)
				{
					// obtiene los datos del facturador electrónico
					TblEmpresas facturador = null;
					if (doc.TblEmpresasFacturador == null)
						facturador = _empresa.Obtener(doc.StrEmpresaFacturador, false);
					else
						facturador = doc.TblEmpresasFacturador;

					DocumentoRespuesta resp = new DocumentoRespuesta();
					Ctl_Documentos.ConsultarEventos(doc, facturador, ref resp);

					List<Acuse> objeto_acuse = null;
					try
					{
						// valida si la Respuesta de la DIAN es correcta, siginifica que tiene eventos
						if (resp.EstadoDian != null && resp.EstadoDian.EstadoDocumento == EstadoDocumentoDian.Aceptado.GetHashCode())
						{
							FacturaE_Documento resultado = null;

							string url_acuse = resp.EstadoDian.UrlXmlRespuesta.Replace("FacturaEConsultaDian", "XmlAcuse");

							string contenido_xml = Archivo.ObtenerContenido(url_acuse);

							// valida el contenido del archivo
							if (string.IsNullOrWhiteSpace(contenido_xml))
								throw new ArgumentException("El archivo XML UBL se encuentra vacío.");

							// convierte el contenido de texto a xml
							XmlReader xml_reader = XmlReader.Create(new StringReader(contenido_xml));

							// convierte el objeto de acuerdo con el tipo de documento
							XmlSerializer serializacion1 = new XmlSerializer(typeof(HGInetUBLv2_1.ApplicationResponseType));

							HGInetUBLv2_1.ApplicationResponseType conversion = (HGInetUBLv2_1.ApplicationResponseType)serializacion1.Deserialize(xml_reader);

							objeto_acuse = HGInetUBLv2_1.AcuseReciboXMLv2_1.Convertir(conversion);

							//Si tiene eventos se procesan, se guardan en BD y se valida si es posible hacer el acuse tacito
							if (objeto_acuse != null)
							{
								foreach (Acuse item_acuse in objeto_acuse)
								{
									CodigoResponseV2 cod_acuse = Enumeracion.GetValueFromAmbiente<HGInetMiFacturaElectonicaData.CodigoResponseV2>(item_acuse.CodigoRespuesta);
									TblEventosRadian eventobd = evento.Convertir(doc.StrIdSeguridad, Convert.ToInt16(cod_acuse.GetHashCode()), Convert.ToInt64(item_acuse.IdAcuse), item_acuse.Fecha);
									eventobd.StrUrlEvento = url_acuse;
									bool crear_evento = true;
									if (list_evento != null && list_evento.Count > 0)
									{
										if (acuse != null && cod_acuse == CodigoResponseV2.Recibido)
										{
											crear_evento = false;
										}

										if (recibo != null && cod_acuse == CodigoResponseV2.Aceptado)
										{
											crear_evento = false;
										}

										if (expresa != null && cod_acuse == CodigoResponseV2.Expresa)
										{
											crear_evento = false;
										}

										if (rechazo != null && cod_acuse == CodigoResponseV2.Rechazado)
										{
											crear_evento = false;
										}

										if (inscripcion != null && cod_acuse == CodigoResponseV2.Inscripcion)
										{
											crear_evento = false;
										}
									}

									if (crear_evento == true)
									{
										evento.Crear(eventobd);
										//RegistroLog.EscribirLog(new ApplicationException("Creacion del Evento"), MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.consulta, string.Format("Codigo Estado Doc {0} - Codigo respuesta DIAN {1} - descripcion {2}", resp.EstadoDian.EstadoDocumento, resp.EstadoDian.CodigoRespuesta, resp.EstadoDian.Descripcion));
										doc.IntAdquirienteRecibo = Convert.ToInt16(cod_acuse.GetHashCode());
										doc.DatAdquirienteFechaRecibo = item_acuse.Fecha;
									}
									else if (doc.IntAdquirienteRecibo != cod_acuse.GetHashCode())
									{
										doc.IntAdquirienteRecibo = Convert.ToInt16(cod_acuse.GetHashCode());
										doc.DatAdquirienteFechaRecibo = Fecha.GetFecha();
									}
								}
							}
						}
						else
						{
							//if (doc.IntAdquirienteRecibo > 0)
							//{
							//	doc.IntAdquirienteRecibo = 0;
							//}
							doc.DatAdquirienteFechaRecibo = Fecha.GetFecha();
						}

						Actualizar(doc);

					}
					catch (Exception ex)
					{
						RegistroLog.EscribirLog(ex, MensajeCategoria.Archivos, MensajeTipo.Error, MensajeAccion.lectura, string.Format("Error procesando Eventos consultados {0}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.CodigoResponseV2>(CodigoResponseV2.Recibido.GetHashCode()))));

					}
				}

			}
				
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

				if (tipo_doc < TipoDocumento.Nomina)
				{
					if (tipo_doc == TipoDocumento.NotaCredito || tipo_doc == TipoDocumento.NotaDebito)
					{
						tbl_documento.DatFechaVencDocumento = documento_obj.Fecha;
					}
					else
					{
						tbl_documento.DatFechaVencDocumento = documento_obj.FechaVence;
						//Validacion: Si < 0 el valor es 0, si es = 0 es el valor del documento, si es > 0 es el valor a pagar que envian
						tbl_documento.IntValorPagar = (documento_obj.ValorPagar < 0) ? 0 : (documento_obj.ValorPagar == 0) ? documento_obj.Total : (documento_obj.ValorPagar > 0) ? documento_obj.ValorPagar : 0;
						tbl_documento.IntFormaPago = (documento_obj.FormaPago == 0) ? 1 : Convert.ToInt16(documento_obj.FormaPago);
					}

					tbl_documento.DatFechaDocumento = Convert.ToDateTime(documento_obj.Fecha.ToString(Fecha.formato_fecha_hginet));
					tbl_documento.StrEmpresaAdquiriente = documento_obj.DatosAdquiriente.Identificacion;
					tbl_documento.StrProveedorReceptor = documento_obj.IdentificacionProveedor;
					tbl_documento.IntVlrTotal = documento_obj.Total;
					tbl_documento.IntValorSubtotal = documento_obj.ValorSubtotal;
					tbl_documento.IntValorNeto = documento_obj.Neto;
					tbl_documento.IntTipoOperacion = documento_obj.TipoOperacion;
					tbl_documento.StrLineaNegocio = documento_obj.LineaNegocio;
				}
				else
				{
					tbl_documento.DatFechaVencDocumento = Convert.ToDateTime(documento_obj.FechaGen.ToString(Fecha.formato_fecha_hginet));
					tbl_documento.DatFechaDocumento = documento_obj.FechaGen;
					tbl_documento.StrEmpresaAdquiriente = documento_obj.DatosTrabajador.Identificacion;
					tbl_documento.StrProveedorReceptor = Constantes.NitResolucionsinPrefijo;
					tbl_documento.IntVlrTotal = documento_obj.ComprobanteTotal;
					tbl_documento.IntValorSubtotal = documento_obj.ComprobanteTotal;
					tbl_documento.IntValorNeto = documento_obj.ComprobanteTotal;
				}

				tbl_documento.DatFechaIngreso = respuesta.FechaRecepcion;
				tbl_documento.IntDocTipo = tipo_doc.GetHashCode();
				tbl_documento.IntNumero = documento_obj.Documento;
				tbl_documento.StrPrefijo = (!string.IsNullOrEmpty(documento_obj.Prefijo)) ? documento_obj.Prefijo : "";
				tbl_documento.StrObligadoIdRegistro = documento_obj.CodigoRegistro;
				tbl_documento.StrNumResolucion = resolucion.StrNumResolucion;
				tbl_documento.StrEmpresaFacturador = empresa.StrIdentificacion;
				tbl_documento.StrCufe = respuesta.Cufe;
				tbl_documento.StrIdSeguridad = Guid.Parse(respuesta.IdDocumento);
				tbl_documento.IntAdquirienteRecibo = 0;
				tbl_documento.IntIdEstado = Convert.ToInt16(respuesta.IdProceso);
				tbl_documento.DatFechaActualizaEstado = Fecha.GetFecha();
				tbl_documento.StrVersion = documento_obj.VersionAplicativo;
				tbl_documento.StrProveedorEmisor = Constantes.NitResolucionsinPrefijo;
				tbl_documento.IntVersionDian = empresa.IntVersionDian;

				//validacion si es un formato de integrador para guardar los campos predeterminados
				//if (documento_obj.DocumentoFormato != null && documento_obj.DocumentoFormato.Codigo > 0 && empresa.IntVersionDian == 2)
				//{
				//	if (documento_obj.DocumentoFormato != null)
				//	{
				//		tbl_documento.StrFormato = JsonConvert.SerializeObject(documento_obj.DocumentoFormato);
				//	}
				//}

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
		public static object ConvertirServicio(TblDocumentos objetoBd, bool reenvio = false, bool proceso_adquiriente = false, bool evento_radian = false)
		{
			//Asigna a un objeto dinamico el objeto enviado por el usuario
			var documento_obj = (dynamic)null;

			// lee el archivo XML en UBL desde la ruta pública
			string contenido_xml = Archivo.ObtenerContenido(objetoBd.StrUrlArchivoUbl);

			if (evento_radian == true)
			{
				contenido_xml = Archivo.ObtenerContenido(objetoBd.StrUrlAcuseUbl);
			}

			// valida el contenido del archivo
			if (string.IsNullOrWhiteSpace(contenido_xml))
				throw new ArgumentException("El archivo XML UBL se encuentra vacío.");

			// convierte el contenido de texto a xml
			XmlReader xml_reader = XmlReader.Create(new StringReader(contenido_xml));

			// convierte el objeto de acuerdo con el tipo de documento
			XmlSerializer serializacion = null;

			//Segun el tipo del documento lo asigna al objeto dinamico y convierte el Ubl en objeto de servicio
			if (objetoBd.IntDocTipo == TipoDocumento.Factura.GetHashCode() && evento_radian == false)
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
			else if (objetoBd.IntDocTipo == TipoDocumento.NotaCredito.GetHashCode() && evento_radian == false)
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
			else if (objetoBd.IntDocTipo == TipoDocumento.NotaDebito.GetHashCode() && evento_radian == false)
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
			else if (evento_radian == true)
			{

				AcuseConsulta evento = new AcuseConsulta();

				documento_obj = evento;

				serializacion = new XmlSerializer(typeof(HGInetUBLv2_1.ApplicationResponseType));

				HGInetUBLv2_1.ApplicationResponseType conversion = (HGInetUBLv2_1.ApplicationResponseType)serializacion.Deserialize(xml_reader);

				Acuse objeto_acuse = HGInetUBLv2_1.AcuseReciboXMLv2_1.Convertir(conversion).FirstOrDefault();

				documento_obj.DatosAcuse = objeto_acuse;

				documento_obj.DatosAcuse.IdSeguridad = objetoBd.StrIdSeguridad.ToString();

			}
			//else if (objetoBd.IntDocTipo == TipoDocumento.Nomina.GetHashCode())
			//{

			//	Nomina nomina = new Nomina();

			//	documento_obj = nomina;

			//	serializacion = new XmlSerializer(typeof(NominaIndividualType));

			//	NominaIndividualType conversion = (NominaIndividualType)serializacion.Deserialize(xml_reader);

			//	documento_obj = HGInetUBLv2_1.NominaXML.Convertir(conversion, objetoBd);

			//	if (reenvio == true)
			//		return documento_obj;

			//}
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

			if (!reenvio && proceso_adquiriente == false)
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
			}

			//Construye la url publica para el acuse de recibo del documento
			PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;
			documento_obj.UrlAcuse = string.Format("{0}{1}", plataforma.RutaPublica, Constantes.PaginaAcuseRecibo.Replace("{id_seguridad}", objetoBd.StrIdSeguridad.ToString()));

			if (evento_radian == true)
				documento_obj.UrlAcuse = objetoBd.StrUrlAcuseUbl;

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

				//****Se agrega siempre a uno para dejar procesos como estaba sin Radian.
				//if (respuesta.IntAdquirienteRecibo == AdquirienteRecibo.Entregado.GetHashCode())
				//	respuesta.IntAdquirienteRecibo = (short)AdquirienteRecibo.Aprobado.GetHashCode();

				DocumentoRespuesta obj_documento = new DocumentoRespuesta();

				obj_documento.Aceptacion = (respuesta.IntAdquirienteRecibo > CodigoResponseV2.Inscripcion.GetHashCode()) ? CodigoResponseV2.Inscripcion.GetHashCode() : respuesta.IntAdquirienteRecibo;
				obj_documento.DescripcionAceptacion = (respuesta.IntAdquirienteRecibo > CodigoResponseV2.Inscripcion.GetHashCode()) ? "Titulo Valor" : Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<CodigoResponseV2>(respuesta.IntAdquirienteRecibo)); 
				obj_documento.CodigoRegistro = respuesta.StrObligadoIdRegistro;
				obj_documento.Cufe = respuesta.StrCufe;
				obj_documento.DocumentoTipo = respuesta.IntDocTipo;
				obj_documento.Documento = respuesta.IntNumero;
				obj_documento.FechaRecepcion = respuesta.DatFechaIngreso;
				obj_documento.FechaUltimoProceso = respuesta.DatFechaActualizaEstado;
				obj_documento.IdDocumento = respuesta.StrIdSeguridad.ToString();
				obj_documento.Identificacion = respuesta.StrEmpresaAdquiriente;
				obj_documento.IdentificacionObligado = respuesta.StrEmpresaFacturador;
				obj_documento.IdProceso = respuesta.IntIdEstado;
				obj_documento.DescripcionProceso = Enumeracion.GetDescription(Enumeracion.ParseToEnum<ProcesoEstado>(Convert.ToInt32(respuesta.IntIdEstado)));
				obj_documento.MotivoRechazo = respuesta.StrAdquirienteMvoRechazo;
				obj_documento.NumeroResolucion = respuesta.StrNumResolucion;
				obj_documento.Prefijo = respuesta.StrPrefijo;
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


		/// <summary>
		/// Convierte un objeto de tipo de base de datos a objeto de servicio.
		/// </summary>
		/// <param name="respuesta">Objeto de tipo TblDocumentos</param>
		/// <returns>Objeto Tipo DocumentoRespuesta</returns>
		public static PagoElectronicoRespuesta ConvertirPago(TblDocumentos respuesta)
		{
			try
			{
				if (respuesta == null)
					throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "respuesta", "TblDocumentos"));


				PagoElectronicoRespuesta obj_documento = new PagoElectronicoRespuesta();

				obj_documento.Cufe = respuesta.StrCufe;
				obj_documento.DocumentoTipo = respuesta.IntDocTipo;
				obj_documento.Documento = respuesta.IntNumero;
				obj_documento.FechaDocumento = respuesta.DatFechaIngreso;
				obj_documento.IdDocumento = respuesta.StrIdSeguridad.ToString();
				obj_documento.Identificacion = respuesta.StrEmpresaAdquiriente;
				obj_documento.NumeroResolucion = respuesta.StrNumResolucion;
				obj_documento.Prefijo = respuesta.StrPrefijo;
				obj_documento.DocumentoTipo = respuesta.IntDocTipo;

				List<PagoElectronicoRespuestaDetalle> DetallesPagos = new List<PagoElectronicoRespuestaDetalle>();


				//Ctl_PagosElectronicos _Pagos = new Ctl_PagosElectronicos();
				//List<TblPagosElectronicos> lista_pagos = _Pagos.ObtenerPagos(respuesta.StrIdSeguridad);				
				//foreach (TblPagosElectronicos pago in lista_pagos)
				//{
				//	try
				//	{
				//		var respuesta_pago = new PagoElectronicoRespuestaDetalle();
				//		respuesta_pago.IdRegistro = pago.StrIdRegistro.ToString();
				//		respuesta_pago.Fecha = pago.DatFechaRegistro;
				//		respuesta_pago.IdPago = pago.StrIdSeguridadPago;
				//		respuesta_pago.ReferenciaCUS = pago.StrTransaccionCUS;
				//		respuesta_pago.TicketID = pago.StrTicketID;
				//		respuesta_pago.PagoEstadoDescripcion = pago.StrMensaje;
				//		respuesta_pago.PagoEstado = pago.IntEstadoPago;
				//		respuesta_pago.Valor = pago.IntValorPago;
				//		respuesta_pago.FormaPago = pago.IntFormaPago.ToString();
				//		respuesta_pago.Franquicia = pago.StrCodigoFranquicia;

				//		DetallesPagos.Add(respuesta_pago);
				//	}
				//	catch (Exception)
				//	{
				//	}
				//}

				Ctl_PagosElectronicos _Pago = new Ctl_PagosElectronicos();
				Ctl_PagosDetalles _Pago_Detalle = new Ctl_PagosDetalles();
				List<TblPagosDetalles> lista_pagos = _Pago_Detalle.Obtener(respuesta.StrIdSeguridad);
				foreach (TblPagosDetalles pago in lista_pagos)
				{
					try
					{

						TblPagosElectronicos pago_principal = _Pago.ObtenerPagoPorRegistroPrincipal(pago.StrIdPagoPrincipal);

						var respuesta_pago = new PagoElectronicoRespuestaDetalle();
						respuesta_pago.IdRegistro = pago_principal.StrIdRegistro.ToString();
						respuesta_pago.Fecha = pago_principal.DatFechaRegistro;
						respuesta_pago.IdPago = pago_principal.StrIdSeguridadPago;
						respuesta_pago.ReferenciaCUS = pago_principal.StrTransaccionCUS;
						respuesta_pago.TicketID = pago_principal.StrTicketID;
						respuesta_pago.PagoEstadoDescripcion = pago_principal.StrMensaje;
						respuesta_pago.PagoEstado = pago_principal.IntEstadoPago;
						respuesta_pago.Valor = pago.IntValorPago;
						respuesta_pago.FormaPago = pago_principal.IntFormaPago.ToString();
						respuesta_pago.Franquicia = pago_principal.StrCodigoFranquicia;

						DetallesPagos.Add(respuesta_pago);
					}
					catch (Exception)
					{
					}
				}


				obj_documento.DetallesPagos = DetallesPagos;

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

			//List<string> estados = Coleccion.ConvertirLista(estado_recibo);
			string estados_permitidos = string.Format("{0},{1},{2},{3}", ProcesoEstado.AlmacenaXML.GetHashCode(), ProcesoEstado.FirmaXml.GetHashCode(), ProcesoEstado.CompresionXml.GetHashCode(), ProcesoEstado.EnvioZip.GetHashCode());
			int estado_pausado = ProcesoEstado.ProcesoPausadoPlataformaDian.GetHashCode();

			DateTime FechaActual = Fecha.GetFecha();

			var respuesta = (from datos in context.TblDocumentos
							 join empresa in context.TblEmpresas on datos.StrEmpresaAdquiriente equals empresa.StrIdentificacion
							 where (estados_permitidos.Contains(datos.IntIdEstado.ToString()) || estado_pausado == datos.IntIdEstado)
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
						resultado = Ctl_Documento.ConvertirAcuse(doc, facturador, adquiriente, doc.IntAdquirienteRecibo, doc.StrAdquirienteMvoRechazo);
					}
					else
					{
						//obtiene los datos del proveedor del facturador
						TblEmpresas proveedor_receptor = ctl_empresa.Obtener(doc.StrProveedorReceptor);
						resultado = Ctl_Documento.ConvertirAcuse(doc, facturador, adquiriente, doc.IntAdquirienteRecibo, doc.StrAdquirienteMvoRechazo);
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
		/// procesa Lista de Documentos sin lazyloading
		/// </summary>
		/// <param name="List_id_seguridad"></param>        
		/// <returns></returns>
		public List<TblDocumentos> ProcesarDocumentosSinLazy(List<System.Guid> List_id_seguridad)
		{
			try
			{
				context.Configuration.LazyLoadingEnabled = false;

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
		public async Task SondaGenerarPDF(string ListaDoc, bool validacion_dian)
		{
			try
			{
				var Tarea = TareaGenerarPDF(ListaDoc, validacion_dian);
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
		public async Task TareaGenerarPDF(string ListaDoc, bool validacion_dian)
		{
			try
			{
				await Task.Factory.StartNew(() =>
				{

					List<string> lista = Coleccion.ConvertirLista(ListaDoc, ',');
					foreach (string Idseg in Coleccion.ConvertirLista(ListaDoc, ','))
					{

						TblDocumentos documento = ObtenerPorIdSeguridad(Guid.Parse(Idseg)).FirstOrDefault();

						if (validacion_dian == false)
						{
							PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

							

							var documento_obj = (dynamic)null;

							FacturaE_Documento documento_result = new FacturaE_Documento();

							documento_result.IdSeguridadDocumento = Guid.Parse(documento.StrIdSeguridad.ToString());
							documento_result.IdSeguridadTercero = documento.TblEmpresasFacturador.StrIdSeguridad;
							documento_result.DocumentoTipo = Enumeracion.ParseToEnum<TipoDocumento>(documento.IntDocTipo);
							documento_result.NombreXml = HGInetUBLv2_1.NombramientoArchivo.ObtenerXml(documento.IntNumero.ToString(), documento.StrEmpresaFacturador, Enumeracion.ParseToEnum<TipoDocumento>(documento.IntDocTipo), documento.StrPrefijo);
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
								else if (documento.IntDocTipo == TipoDocumento.Nomina.GetHashCode())
								{
									serializacion = new XmlSerializer(typeof(NominaIndividualType));

									NominaIndividualType conversion = (NominaIndividualType)serializacion.Deserialize(xml_reader);

									documento_obj = HGInetUBLv2_1.NominaXML.Convertir(conversion, documento);

									if (!documento.StrCufe.Equals(documento_obj.Cune))
									{

										string texto_xml = Archivo.ObtenerContenido(documento.StrUrlArchivoUbl);

										texto_xml = texto_xml.Replace(documento_obj.Cune, documento.StrCufe);
										documento_result.DocumentoXml = new StringBuilder(texto_xml);

										documento_obj.Cune = documento.StrCufe;

										xml_reader.Close();

										// almacena el archivo xml
										string ruta_save = Xml.Guardar(documento_result.DocumentoXml, carpeta_xml, string.Format("{0}.xml", documento_result.NombreXml));

									}

								}
								else if (documento.IntDocTipo == TipoDocumento.NominaAjuste.GetHashCode())
								{
									serializacion = new XmlSerializer(typeof(NominaIndividualDeAjusteType));

									NominaIndividualDeAjusteType conversion = (NominaIndividualDeAjusteType)serializacion.Deserialize(xml_reader);

									documento_obj = HGInetUBLv2_1.NominaAjusteXML.Convertir(conversion, documento);

									if (!documento.StrCufe.Equals(documento_obj.Cune))
									{

										string texto_xml = Archivo.ObtenerContenido(documento.StrUrlArchivoUbl);

										texto_xml = texto_xml.Replace(documento_obj.Cune, documento.StrCufe);
										documento_result.DocumentoXml = new StringBuilder(texto_xml);

										documento_obj.Cune = documento.StrCufe;

										xml_reader.Close();

										// almacena el archivo xml
										string ruta_save = Xml.Guardar(documento_result.DocumentoXml, carpeta_xml, string.Format("{0}.xml", documento_result.NombreXml));

									}

								}

								//if (!string.IsNullOrEmpty(documento.StrFormato))
								//{
								//	documento_obj.DocumentoFormato = JsonConvert.DeserializeObject<Formato>(documento.StrFormato);
								//}

								string contenido_formato = Archivo.ObtenerContenido(documento.StrUrlArchivoUbl.Replace("FacturaEDian", "XmlFacturaE").Replace("xml", "json"));

								if (documento_obj.DocumentoFormato == null && !string.IsNullOrEmpty(contenido_formato))
								{
									documento_obj.DocumentoFormato = JsonConvert.DeserializeObject<Formato>(contenido_formato);
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
						else
						{
							try
							{
								if (documento.TblEmpresasFacturador == null)
								{
									Ctl_Empresa _empresa = new Ctl_Empresa();
									documento.TblEmpresasFacturador = _empresa.Obtener(documento.StrEmpresaFacturador);
								}

								TblEmpresas empresa_obligado = documento.TblEmpresasFacturador;

								GeneracionFechaDian(empresa_obligado, documento);
							}
							catch (Exception excepcion)
							{
								Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.actualizacion, "Error Generando fecha de validacion en el PDF desde la Sonda");
							}

						}

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
				List<Guid> datos = ObtenerAcuseTacito("*", "*", "*", true);

				//RegistroLog.EscribirLog(new ApplicationException("Guids: " + datos.Count), MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.consulta);

				/*
				foreach (var item in datos)
				{
					try
					{
						ActualizarRespuestaAcuse(item, (short)AdquirienteRecibo.AprobadoTacito.GetHashCode(), string.Empty);
					}
					catch (Exception excepcion)
					{
						RegistroLog.EscribirLog(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.actualizacion);
						Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.actualizacion);
					}
				}*/

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
							int tipo_doc_plan = 0;
							if (item.IntDocTipo == TipoDocumento.Nomina.GetHashCode() || item.IntDocTipo == TipoDocumento.NominaAjuste.GetHashCode())
							{
								tipo_doc_plan = TipoDocPlanes.Nomina.GetHashCode();
							}
							else
							{
								tipo_doc_plan = TipoDocPlanes.Documento.GetHashCode();
							}
							//Obtengo el plan con el que voy a descontar el saldo
							obj_plan = controladorplanes.ObtenerPlanesActivos(item.StrEmpresaFacturador, 1, tipo_doc_plan);
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
		public List<TblDocumentos> ObtenerDocumentosValidarEmail(int dias, bool solodia)
		{

			try
			{
				int estado_enviado = Convert.ToInt32(EstadoEnvio.Enviado.GetHashCode());
				int estado_adquiriente = Convert.ToInt32(CodigoResponseV2.Recibido.GetHashCode());
				int estado_doc = CategoriaEstado.ValidadoDian.GetHashCode();
				DateTime FechaActual = Fecha.GetFecha();

				context.Configuration.LazyLoadingEnabled = false;

				if (dias > 0)
				{
					if (solodia == true)
					{
						DateTime FechaInicial = FechaActual.AddDays(-dias).Date;
						DateTime FechaFinal = new DateTime(FechaInicial.Year, FechaInicial.Month, FechaInicial.Day, 23, 59, 59, 999);

						var respuesta = (from datos in context.TblDocumentos.Include("TblEmpresasAdquiriente")
										 where datos.DatFechaIngreso >= FechaInicial && datos.DatFechaIngreso <= FechaFinal &&
												datos.IdCategoriaEstado == estado_doc &&
												(datos.IntEstadoEnvio == (estado_enviado) ||
												 datos.IntAdquirienteRecibo > estado_adquiriente)
										 select datos
							);
						return respuesta.ToList();
					}
					else
					{
						var respuesta = (from datos in context.TblDocumentos.Include("TblEmpresasAdquiriente")
										 where datos.DatFechaIngreso >= SqlFunctions.DateAdd("dd", -dias, FechaActual) &&
											   datos.IdCategoriaEstado == estado_doc &&
											   (datos.IntEstadoEnvio == (estado_enviado) ||
												datos.IntAdquirienteRecibo > estado_adquiriente)
										 select datos
							);
						return respuesta.ToList();
					}


				}
				else
				{
					DateTime FechaInicial = Fecha.GetFecha().Date;
					DateTime FechaFinal = new DateTime(Fecha.GetFecha().Year, Fecha.GetFecha().Month, Fecha.GetFecha().Day, 23, 59, 59, 999);

					var respuesta = (from datos in context.TblDocumentos.Include("TblEmpresasAdquiriente")
									 where datos.IntEstadoEnvio == (estado_enviado) &&
										   datos.IdCategoriaEstado == estado_doc &&
											datos.DatFechaIngreso >= FechaInicial && datos.DatFechaIngreso <= FechaFinal
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

		/// <summary>
		/// Sonda para validar el estado del Email
		/// </summary>
		/// <param name="dias">0 para el dia actual, > 0 resta el valor a la fecha actual</param>
		/// <param name="Solodia">si dias > 0 procesa solo lo del dia que resulta</param>
		/// <param name="SoloCorreo">Procesa solo los correos que tenga por enviar , en el dia inidicado y tenga estado false en la TblDocumentos</param>
		/// <param name="CorreoAudit">Valida el correo sacando la informacion desde la Auditoria</param>
		/// <returns></returns>
		public async Task SondaDocumentosValidarEmail(int dias, bool Solodia, bool SoloCorreo, bool CorreoAudit)
		{
			try
			{
				var Tarea = TareaDocumentosValidarEmail(dias, Solodia, SoloCorreo, CorreoAudit);
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
		/// <param name="dias">si es mayor a Cero resta ese valor a la fecha actual para validar, si es 0 valida los documentos actuales</param>
		/// <param name="solodia">Si es true y dias > 0 valida los documentos solo de esa fecha, si no procesa </param>
		/// <returns></returns>
		public async Task TareaDocumentosValidarEmail(int dias, bool solodia, bool SoloCorreo, bool CorreoAudit)
		{
			await Task.Factory.StartNew(() =>
			{

				if (SoloCorreo == false)
				{
					if (CorreoAudit == false)
					{
						MensajeValidarEmail MailPlataforma = new MensajeValidarEmail();
						MensajeResumen datos_retorno = null;
						Ctl_ProcesosCorreos procesos_correo = new Ctl_ProcesosCorreos();
						List<TblProcesoCorreo> list_correos = procesos_correo.ObtenerPorValidar(dias, solodia);
						if (list_correos != null && list_correos.Count > 0)
						{
							Ctl_EnvioCorreos email = new Ctl_EnvioCorreos();

							foreach (TblProcesoCorreo item in list_correos)
							{
								datos_retorno = email.ConsultarCorreo(Convert.ToInt64(item.StrIdMensaje));

								if (datos_retorno != null && !string.IsNullOrEmpty(datos_retorno.Estado))
								{
									TblDocumentos doc_bd = ObtenerPorIdSeguridad(item.StrIdSeguridadDoc, true).FirstOrDefault();

									if (MensajeIdResultado.Entregado.GetHashCode().Equals(datos_retorno.IdResultado))
									{
										doc_bd.IntEstadoEnvio = (short)EstadoEnvio.Entregado.GetHashCode();
										doc_bd.IntMensajeEnvio = (short)Enumeracion.GetValueFromDescription<MensajeEstado>(datos_retorno.Estado);
										doc_bd.DatFechaActualizaEstado = Fecha.GetFecha();
										item.IntValidadoMail = true;

									}
									else if (MensajeIdResultado.NoEntregado.GetHashCode().Equals(datos_retorno.IdResultado))
									{
										doc_bd.IntEstadoEnvio = (short)EstadoEnvio.NoEntregado.GetHashCode();
										doc_bd.IntMensajeEnvio = (short)Enumeracion.GetValueFromDescription<MensajeEstado>(datos_retorno.Estado);
										doc_bd.DatFechaActualizaEstado = (string.IsNullOrEmpty(datos_retorno.Estado)) ? Fecha.GetFecha() : datos_retorno.Recibido;
										List<MensajeEnvio> notificacion = email.NotificacionCorreofacturador(doc_bd, doc_bd.TblEmpresasAdquiriente.StrTelefono, item.StrMailEnviado, datos_retorno.Estado, item.StrIdSeguridadDoc.ToString());
										item.IntValidadoMail = true;

									}
									else
									{
										doc_bd.IntEstadoEnvio = (short)EstadoEnvio.Enviado.GetHashCode();
										doc_bd.IntMensajeEnvio = (short)Enumeracion.GetValueFromDescription<MensajeEstado>(datos_retorno.Estado);
										doc_bd.DatFechaActualizaEstado = (string.IsNullOrEmpty(datos_retorno.Estado)) ? Fecha.GetFecha() : datos_retorno.Recibido;
										if (item.DatFechaValidado == null)
										{
											item.DatFechaValidado = item.DatFecha;
										}
										else if (item.DatFechaValidado == item.DatFecha)
										{
											if (Fecha.Diferencia(doc_bd.DatFechaIngreso, Fecha.GetFecha(), DateInterval.Minute) > 120)
											{
												//Si pasa algo envio notificacion a tic para validar por que no se proceso
												List<string> mensajes = new List<string>();
												mensajes.Add("Plataforma de Correo no entrega un estado de la recepcion del correo y tiene una diferencia de 120 minutos respecto al ingreso, sin una respuesta");
												mensajes.Add(string.Format("Los Datos son: Documento: {0} , Correo: {1} , MessageId: {2}", doc_bd.IntNumero.ToString(), item.StrMailEnviado, item.StrIdMensaje));
												List<MensajeEnvio> notificacion = email.EnviaNotificacionAlertaDIAN(doc_bd.StrEmpresaFacturador, doc_bd.IntNumero.ToString(), mensajes, 2, false, Constantes.EmailCopiaOculta, 3);
												item.DatFechaValidado = Fecha.GetFecha();
												item.IntValidadoMail = true;
											}
										}

									}

									//Cambio el estado del acuse siempre y cuando sea mayor a 3 el estado de acuse
									if (doc_bd.IntAdquirienteRecibo > AdquirienteRecibo.AprobadoTacito.GetHashCode())
									{
										doc_bd.IntAdquirienteRecibo = (short)AdquirienteRecibo.Pendiente.GetHashCode();
										doc_bd.DatAdquirienteFechaRecibo = null;
									}

									Actualizar(doc_bd);

									//Actualizo tabla de correos
									if (item.DatFechaValidado == null)
									{
										item.DatFechaValidado = doc_bd.DatFechaActualizaEstado;
									}

									if (item.IntEnvioMail == false)
										item.IntEnvioMail = true;

									procesos_correo.Actualizar(item);

								}
								else
								{

								}

							}
						}


					}
					else
					{
						Ctl_DocumentosAudit clase_audit_doc = new Ctl_DocumentosAudit();
						List<TblDocumentos> datos = ObtenerDocumentosValidarEmail(dias, solodia);

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

									try
									{
										ReenviarCorreoSonda(item);
										item.IntEstadoEnvio = (short)EstadoEnvio.Enviado.GetHashCode();
										item.DatFechaActualizaEstado = Fecha.GetFecha();
										item.IntEnvioMail = true;
									}
									catch (Exception)
									{
										item.IntEstadoEnvio = (short)EstadoEnvio.Desconocido.GetHashCode();
										item.DatFechaActualizaEstado = Fecha.GetFecha();
										item.IntEnvioMail = false;
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
					}


				}
				else
				{
					List<TblDocumentos> documentos_sinmail = null;

					//Se valida que documentos no se han enviado al correo por algun motivo
					try
					{
						documentos_sinmail = ObtenerDocumentosValidarSinEmail(dias, solodia);

						if (documentos_sinmail != null && documentos_sinmail.Count > 0)
						{
							foreach (TblDocumentos item in documentos_sinmail)
							{
								try
								{
									Ctl_ProcesosCorreos procesos_correo = new Ctl_ProcesosCorreos();
									TblProcesoCorreo correo = procesos_correo.Obtener(item.StrIdSeguridad);
									if (correo != null && correo.IntEnvioMail == false)
									{
										ReenviarCorreoSonda(item);
									}
									item.IntEstadoEnvio = (short)EstadoEnvio.Enviado.GetHashCode();
									item.DatFechaActualizaEstado = Fecha.GetFecha();
									item.IntEnvioMail = true;
								}
								catch (Exception)
								{
									item.IntEstadoEnvio = (short)EstadoEnvio.Desconocido.GetHashCode();
									item.DatFechaActualizaEstado = Fecha.GetFecha();
									item.IntEnvioMail = false;
								}

								Ctl_Documento ctl_documento = new Ctl_Documento();
								ctl_documento.Actualizar(item);

							}
						}


					}
					catch (Exception excepcion)
					{
						RegistroLog.EscribirLog(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.consulta);

						Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.consulta);
					}
				}

			});
		}

		public static void ReenviarCorreoSonda(TblDocumentos doc)
		{
			string email_objeto = string.Empty;
			string telefono_objeto = string.Empty;

			try
			{

				if (doc.IntDocTipo < TipoDocumento.AcuseRecibo.GetHashCode())
				{
					var objeto = (dynamic)null;
					objeto = Ctl_Documento.ConvertirServicio(doc, true);

					if (doc.IntDocTipo == TipoDocumento.Factura.GetHashCode())
					{
						email_objeto = objeto.DatosFactura.DatosAdquiriente.Email;
						telefono_objeto = objeto.DatosFactura.DatosAdquiriente.Telefono;
					}

					if (doc.IntDocTipo == TipoDocumento.NotaCredito.GetHashCode())
					{
						email_objeto = objeto.DatosNotaCredito.DatosAdquiriente.Email;
						telefono_objeto = objeto.DatosNotaCredito.DatosAdquiriente.Telefono;
					}

					if (doc.IntDocTipo == TipoDocumento.NotaDebito.GetHashCode())
					{
						email_objeto = objeto.DatosNotaDebito.DatosAdquiriente.Email;
						telefono_objeto = objeto.DatosNotaDebito.DatosAdquiriente.Telefono;
					}
				}
				else
				{
					TblEmpresas facturador = doc.TblEmpresasFacturador;
					if (facturador == null)
					{
						Ctl_Empresa emprersa = new Ctl_Empresa();
						facturador = emprersa.Obtener(doc.StrEmpresaFacturador, false);
					}
					email_objeto = facturador.StrMailAdmin;
					telefono_objeto = facturador.StrTelefono;
				}

				Ctl_EnvioCorreos email = new Ctl_EnvioCorreos();
				List<MensajeEnvio> notificacion = email.NotificacionDocumento(doc, telefono_objeto, email_objeto, doc.StrIdSeguridad.ToString());

			}
			catch (Exception excepcion)
			{
				try
				{
					//Se notifica al facturador por que no se a podido enviar el documento por el proceso principal y tampoco por este
					Ctl_EnvioCorreos email = new Ctl_EnvioCorreos();
					List<MensajeEnvio> notificacion = email.NotificacionCorreofacturador(doc, telefono_objeto, email_objeto, "Error enviando Correo", doc.StrIdSeguridad.ToString());

				}
				catch (Exception) { }

				Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.creacion, "Error enviando correo desde la Sonda");

				throw excepcion;
			}

		}

		/// <summary>
		/// Proceso para generar el documento UBL2.1 Attach para enviar por correo al adquiriente con la respuesta de la DIAN
		/// </summary>
		/// <param name="documento">Objeto tipo servicio del documento Electronico enviado</param>
		/// <param name="doc">Objeto BD del documento</param>
		/// <param name="facturador">Informacion del Facturador(Tbl)</param>
		/// <returns></returns>
		public static bool ConvertirAttachedDoc(object documento, TblDocumentos doc, TblEmpresas facturador, int evento_radian = 0, string cude_evento = "")
		{

			bool doc_creado = false;
			try
			{
				var documento_obj = (dynamic)null;

				if (documento == null)
				{
					var documento_ser = (dynamic)null;

					if (evento_radian == 0)
					{
						documento_ser = ConvertirServicio(doc, false);
					}
					else
					{
						documento_ser = ConvertirServicio(doc, true, true, true);
					}

					if (doc.IntDocTipo == TipoDocumento.Factura.GetHashCode() && evento_radian == 0)
					{
						documento_obj = documento_ser.DatosFactura;
					}
					else if (doc.IntDocTipo == TipoDocumento.NotaCredito.GetHashCode() && evento_radian == 0)
					{
						documento_obj = documento_ser.DatosNotaCredito;
					}
					else if (doc.IntDocTipo == TipoDocumento.NotaDebito.GetHashCode() && evento_radian == 0)
					{
						documento_obj = documento_ser.DatosNotaDebito;
					}
					else if (doc.IntDocTipo == TipoDocumento.Nomina.GetHashCode() && evento_radian == 0)
					{
						documento_obj = documento_ser.DatosNomina;
					}
					else if (evento_radian > 0)
					{
						documento_obj = documento_ser.DatosAcuse;
					}
				}
				else if (evento_radian == 0)
				{
					documento_obj = documento;
				}


				PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

				//---Ambiente de la DIAN al que se va enviar el documento: 1 - Produccion, 2 - Pruebas
				string ambiente_dian = "1";

				//if (facturador.IntHabilitacion.Equals(Habilitacion.Produccion.GetHashCode()))
				//	ambiente_dian = "1";
				//else
				//	ambiente_dian = "2";

				TipoDocumento tipo_doc = Enumeracion.GetEnumObjectByValue<TipoDocumento>(doc.IntDocTipo);

				// ruta física del xml
				string carpeta_xml = string.Format("{0}\\{1}\\{2}", plataforma_datos.RutaDmsFisica, Constantes.CarpetaFacturaElectronica, facturador.StrIdSeguridad.ToString());
				carpeta_xml = string.Format(@"{0}\{1}", carpeta_xml, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaXmlFacturaE);

				string nombre_archivo = string.Empty;
				Tercero obligado_empleador = new Tercero();
				Tercero adquiriente_tratrajador = new Tercero();

				//Convierte el objeto en archivo XML-UBL
				FacturaE_Documento resultado = null;

				if (doc.IntDocTipo < TipoDocumento.AcuseRecibo.GetHashCode() && evento_radian == 0)
				{
					nombre_archivo = HGInetUBLv2_1.NombramientoArchivo.ObtenerXml(documento_obj.Documento.ToString(), facturador.StrIdentificacion, TipoDocumento.Attached, documento_obj.Prefijo);

					resultado = HGInetUBLv2_1.AttachedDocument.CrearDocumento(doc.StrIdSeguridad.ToString(), documento_obj.DatosObligado, documento_obj.DatosAdquiriente, ambiente_dian, doc);
				}
				else if (evento_radian > 0)
				{
					nombre_archivo = Path.GetFileNameWithoutExtension(doc.StrUrlAcuseUbl);
					Ctl_Empresa emp = new Ctl_Empresa();
					obligado_empleador = Ctl_Empresa.Convertir(facturador);
					adquiriente_tratrajador = Ctl_Empresa.Convertir(doc.TblEmpresasAdquiriente);

					resultado = HGInetUBLv2_1.AttachedDocument.CrearDocumento(string.Format("{0}{1}", doc.IntNumero, evento_radian), obligado_empleador, adquiriente_tratrajador, ambiente_dian, doc, evento_radian, doc.StrUrlAcuseUbl, cude_evento);
				}
				//else if (doc.IntDocTipo == TipoDocumento.Nomina.GetHashCode())
				//{
				//	nombre_archivo = HGInetUBL.NombramientoArchivo.ObtenerXml(documento_obj.Documento.ToString(), facturador.StrIdentificacion, tipo_doc, documento_obj.Prefijo);
				//	Ctl_Empresa emp = new Ctl_Empresa();
				//	obligado_empleador = Ctl_Empresa.Convertir(facturador);
				//	adquiriente_tratrajador = emp.ConvertirTrabajador(documento_obj.DatosTrabajador);

				//	resultado = HGInetUBLv2_1.AttachedDocument.CrearDocumento(doc.StrIdSeguridad.ToString(), obligado_empleador, adquiriente_tratrajador, ambiente_dian, doc);

				//}
				resultado.IdSeguridadTercero = facturador.StrIdSeguridad;
				resultado.IdSeguridadDocumento = doc.StrIdSeguridad;
				resultado.IdSeguridadPeticion = new Guid();
				resultado.DocumentoTipo = TipoDocumento.Attached;
				resultado.NombreXml = nombre_archivo;

				// valida el nodo de ExtensionContent
				resultado.DocumentoXml = HGInetUBL.ExtensionDian.ValidarNodo(resultado.DocumentoXml);

				// nombre del xml
				string archivo_xml = string.Format(@"{0}.xml", resultado.NombreXml);

				// ruta del xml
				string ruta_xml = string.Format(@"{0}\{1}", carpeta_xml, archivo_xml);

				// elimina el archivo xml si existe
				if (Archivo.ValidarExistencia(ruta_xml))
					Archivo.Borrar(ruta_xml);

				// almacena el archivo xml
				string ruta_save = Xml.Guardar(resultado.DocumentoXml, carpeta_xml, archivo_xml);

				// asigna la ruta del directorio para los archivos sin firmar
				resultado.RutaArchivosProceso = carpeta_xml;

				//Proceso para firmar Attached
				DocumentoRespuesta respuesta = new DocumentoRespuesta();

				// asigna la ruta del directorio para los archivos firmados
				resultado.RutaArchivosEnvio = carpeta_xml.Replace(LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaXmlFacturaE, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian);
				resultado.VersionDian = doc.IntVersionDian;

				string ruta_arch_fimado = string.Format(@"{0}\{1}", resultado.RutaArchivosEnvio, archivo_xml);

				// elimina el archivo xml si existe
				if (Archivo.ValidarExistencia(ruta_arch_fimado))
					Archivo.Borrar(ruta_arch_fimado);

				try
				{
					if (evento_radian == 0)
						respuesta = Ctl_Documentos.UblFirmar(facturador, doc, ref respuesta, ref resultado);
					else
						respuesta = Ctl_Documentos.UblFirmar(doc.TblEmpresasAdquiriente, doc, ref respuesta, ref resultado);

					// elimina el archivo xml si existe
					if (!Archivo.ValidarExistencia(ruta_arch_fimado))
						throw new ApplicationException("");
				}
				catch (Exception)
				{
					// almacena el archivo xml
					string ruta_save_sinfirma = Xml.Guardar(resultado.DocumentoXml, resultado.RutaArchivosEnvio, archivo_xml);
				}

				// elimina el archivo xml si existe
				if (Archivo.ValidarExistencia(ruta_xml))
					Archivo.Borrar(ruta_xml);

				//resultado = Ctl_Ubl.Almacenar(resultado);

				// url pública del xml
				//string url_ppal = string.Format("{0}/{1}/{2}", plataforma_datos.RutaDmsPublica, Constantes.CarpetaFacturaElectronica, resultado.IdSeguridadTercero.ToString());
				//resultado.RutaArchivosProceso = string.Format(@"{0}/{1}/{2}.xml", url_ppal, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaXmlAcuse, resultado.NombreXml);
				doc_creado = true;
			}
			catch (Exception excepcion)
			{
				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Convertir, MensajeTipo.Error, MensajeAccion.ninguna);

				Ctl_Log.Guardar(excepcion, MensajeCategoria.Archivos, MensajeTipo.Error, MensajeAccion.creacion);

				throw excepcion;
			}

			return doc_creado;

		}

		/// <summary>
		/// Obtiene documentos que no se envio el correo y no se ha notificado al facturador
		/// </summary>
		/// <param name="dias">Dias hacia atras que valida los documentos</param>
		/// <param name="solodia">Solo busca los documentos del dia segun la diferencia enviada en dias</param>
		/// <returns></returns>
		public List<TblDocumentos> ObtenerDocumentosValidarSinEmail(int dias, bool solodia)
		{

			try
			{
				int estado_enviado = Convert.ToInt32(EstadoEnvio.Entregado.GetHashCode());
				int estado_doc = CategoriaEstado.ValidadoDian.GetHashCode();
				DateTime FechaActual = Fecha.GetFecha();

				context.Configuration.LazyLoadingEnabled = false;

				if (dias > 0)
				{
					if (solodia == true)
					{
						DateTime FechaInicial = FechaActual.AddDays(-dias).Date;
						DateTime FechaFinal = new DateTime(FechaInicial.Year, FechaInicial.Month, FechaInicial.Day, 23, 59, 59, 999);

						var respuesta = (from datos in context.TblDocumentos.Include("TblEmpresasAdquiriente")
										 where datos.DatFechaIngreso >= FechaInicial && datos.DatFechaIngreso <= FechaFinal &&
											   datos.IdCategoriaEstado == estado_doc &&
											   datos.IntEnvioMail == false &&
											   datos.IntEstadoEnvio < estado_enviado
										 select datos
							);
						return respuesta.ToList();
					}
					else
					{
						var respuesta = (from datos in context.TblDocumentos.Include("TblEmpresasAdquiriente")
										 where datos.DatFechaIngreso > SqlFunctions.DateAdd("dd", -dias, FechaActual) &&
												 datos.IdCategoriaEstado == estado_doc &&
												 datos.IntEnvioMail == false &&
												 datos.IntEstadoEnvio < estado_enviado
										 select datos
							);
						return respuesta.ToList();
					}


				}
				else
				{
					DateTime FechaInicial = FechaActual.Date;
					DateTime FechaFinal = new DateTime(FechaInicial.Year, FechaInicial.Month, FechaInicial.Day, 23, 59, 59, 999);

					var respuesta = (from datos in context.TblDocumentos.Include("TblEmpresasAdquiriente")
									 where datos.DatFechaIngreso >= FechaInicial && datos.DatFechaIngreso <= FechaFinal &&
										   datos.IdCategoriaEstado == estado_doc &&
										   datos.IntEnvioMail == false &&
										   datos.IntEstadoEnvio < estado_enviado
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

		public async Task SondaDocumentosEnviarEmail(int dias, bool Solodia)
		{
			try
			{
				var Tarea = TareaDocumentosEnviarEmail(dias, Solodia);
				await Task.WhenAny(Tarea);
			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.envio);
			}

		}

		public async Task TareaDocumentosEnviarEmail(int dias, bool Solodia)
		{
			await Task.Factory.StartNew(() =>
			{
				try
				{
					Ctl_ProcesosCorreos correo_procesos = new Ctl_ProcesosCorreos();
					List<TblProcesoCorreo> list_correos = correo_procesos.ObtenerCorreos(dias, Solodia);

					if (list_correos != null && list_correos.Count > 0)
					{
						foreach (TblProcesoCorreo item in list_correos)
						{
							TblDocumentos doc_bd = ObtenerPorIdSeguridad(item.StrIdSeguridadDoc, true).FirstOrDefault();
							try
							{
								if (item.IntEnvioMail == false && string.IsNullOrEmpty(item.StrIdMensaje))
								{
									ReenviarCorreoSonda(doc_bd);
								}
								else
								{
									item.IntEnvioMail = true;
									correo_procesos.Actualizar(item);
								}
								doc_bd.IntEstadoEnvio = (short)EstadoEnvio.Enviado.GetHashCode();
								doc_bd.DatFechaActualizaEstado = Fecha.GetFecha();
								doc_bd.IntEnvioMail = true;


							}
							catch (Exception excepcion)
							{
								Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.envio, "Error Enviando el Correo");
								//Se actualiza el documento en bd
								doc_bd.IntEstadoEnvio = (short)EstadoEnvio.Desconocido.GetHashCode();
								doc_bd.DatFechaActualizaEstado = Fecha.GetFecha();
								doc_bd.IntEnvioMail = false;

								//Se actualiza registro ProcesosCorreo puesto que tampoco se pudo enviar por la sonda y en este proceso se notifico al facturador
								item.IntValidadoMail = true;
								item.DatFechaValidado = Fecha.GetFecha();
								correo_procesos.Actualizar(item);
							}

							Actualizar(doc_bd);
						}
					}

				}
				catch (Exception excepcion)
				{
					Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.envio, "Error Enviando documento Por la sonda");
					throw new ApplicationException(excepcion.Message, excepcion.InnerException);
				}


			});
		}


		/// <summary>
		/// Consulta documentos 
		/// </summary>
		/// <param name="codigo_facturador"></param>
		/// <param name="numero_documento"></param>
		/// <param name="codigo_adquiriente"></param>
		/// <param name="prefijo"></param>
		/// <param name="empresa_maneja_lista_documentos"></param>
		/// <returns></returns>
		public List<QryDocumentosSaldo> ConsultarPagosFueraPlataforma(string codigo_facturador, string numero_documento, string codigo_adquiriente, bool empresa_maneja_lista_documentos)
		{

			long num_doc = -1;
			long.TryParse(numero_documento, out num_doc);

			if (string.IsNullOrWhiteSpace(numero_documento))
				numero_documento = "*";


			var datos = (from doc in context.QryDocumentosSaldo
						 where doc.StrEmpresaFacturador.Equals(codigo_facturador)
						 && doc.StrEmpresaAdquiriente.Equals(codigo_adquiriente)
						  && (doc.IntNumero == num_doc || numero_documento.Equals("*"))
						 select doc).OrderBy(x => x.IntNumero).ToList();


			return datos;
		}

		public string ValidarTextoXYPDF(Guid guid_facturador, string identificacion_facturador, int tipo_documento, string numero_documento, string numero_resolucion, decimal posicion_x, decimal posicion_y)
		{
			try
			{
				string ruta_retorno = string.Empty;

				//TblDocumentos documento = ObtenerDocumentoCliente(identificacion_facturador, Convert.ToInt32(numero_documento), "*", numero_resolucion).FirstOrDefault();


				long num_doc = Convert.ToInt64(numero_documento);
				TblDocumentos documento = (from datos in context.TblDocumentos
										   join empresa in context.TblEmpresas on datos.StrEmpresaAdquiriente equals empresa.StrIdentificacion
										   where datos.StrEmpresaFacturador.Equals(identificacion_facturador)
												 && (datos.StrNumResolucion.Equals(numero_resolucion))
												 && (datos.IntNumero == num_doc)
												 && (datos.IntDocTipo == tipo_documento)
										   select datos).FirstOrDefault();


				if (documento == null)
					throw new ApplicationException("El documento solicitado no ha sido encontrado.");

				if (string.IsNullOrWhiteSpace(documento.StrUrlArchivoPdf))
					throw new ApplicationException("El PDF para el documento solicitado no ha sido encontrado.");

				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;
				string carpeta_archivos = string.Format("{0}\\{1}\\{2}", plataforma.RutaDmsFisica, Constantes.CarpetaFacturaElectronicaTmp, guid_facturador.ToString());
				carpeta_archivos = string.Format(@"{0}\{1}", carpeta_archivos, RecursoDms.CarpetaFacturaEConsultaDian);

				string nombre_archivo = NombramientoArchivo.ObtenerXml(documento.IntNumero.ToString(), documento.StrEmpresaFacturador, Enumeracion.ParseToEnum<TipoDocumento>(documento.IntDocTipo), documento.StrPrefijo);

				string ruta_pdf_copia = carpeta_archivos.Replace(RecursoDms.CarpetaFacturaEConsultaDian, RecursoDms.CarpetaFacturaEDian);
				ruta_pdf_copia = Directorio.CrearDirectorio(ruta_pdf_copia);
				ruta_pdf_copia = string.Format(@"{0}\{1}.pdf", ruta_pdf_copia, nombre_archivo);

				byte[] byte_pdf = Archivo.ObtenerWeb(documento.StrUrlArchivoPdf);
				File.WriteAllBytes(ruta_pdf_copia, byte_pdf);

				string ruta_pdf_resultado = ruta_pdf_copia.Replace(nombre_archivo, string.Format("{0}_resultado", nombre_archivo));

				ruta_pdf_resultado = Pdf.AgregarTexto(ruta_pdf_copia, ruta_pdf_resultado, string.Empty, (float)posicion_x, (float)posicion_y, false);

				string url_ppal_pdf = string.Format("{0}/{1}/{2}", plataforma.RutaDmsPublica, Constantes.CarpetaFacturaElectronicaTmp, guid_facturador.ToString());
				ruta_retorno = string.Format(@"{0}/{1}/{2}.pdf", url_ppal_pdf, RecursoDms.CarpetaFacturaEDian, string.Format("{0}_resultado", nombre_archivo));

				return ruta_retorno;
			}
			catch (Exception exception)
			{
				throw new ApplicationException(exception.Message, exception.InnerException);
			}
		}


		public async Task SondaValidarDocumentosRechazados(bool resumen_mes)
		{
			try
			{
				var Tarea = TareaValidarDocumentosRechazados(resumen_mes);
				await Task.WhenAny(Tarea);
			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.envio);
			}

		}

		public class EstadisticaDocumentos
		{
			public string Id_Facturador { get; set; }
			public int CantRechazo { get; set; }
			public int CantValidado { get; set; }
			public int CantEnviado { get; set; }
			public int Cantidad { get; set; }
			public int Estado { get; set; }
			public int SaldoPlan { get; set; }

		}

		public async Task TareaValidarDocumentosRechazados(bool resumen_mes)
		{
			await Task.Factory.StartNew(() =>
			{
				try
				{
					DateTime FechaActual = Fecha.GetFecha();//new DateTime(2021, 09, 01);//
					DateTime FechaInicial = FechaActual;
					DateTime FechaFinal = FechaActual;

					if (resumen_mes == false)
					{
						FechaInicial = FechaActual.AddDays(-7);
						FechaFinal = FechaActual.AddDays(-1);

						try
						{
							List<EstadisticaDocumentos> respuesta = ObtenerPorEstado(FechaInicial, FechaFinal, resumen_mes);

							if (respuesta != null)
							{
								foreach (EstadisticaDocumentos item in respuesta)
								{
									Ctl_Empresa emp = new Ctl_Empresa();
									TblEmpresas facturador = emp.Obtener(item.Id_Facturador);
									Ctl_EnvioCorreos email = new Ctl_EnvioCorreos();
									List<string> ListaNotificacion = new List<string>();
									ListaNotificacion.Add(string.Format("validamos en nuestra plataforma y encontramos que en la semana del {0} hasta el {1}", FechaInicial.ToString("D"), FechaFinal.ToString("D")));
									ListaNotificacion.Add("nos registra documentos en estado No Recibido(Rechazados), a continuacion detallamos la informacion");
									ListaNotificacion.Add(string.Format("Cantidad Rechazados: {0}", item.CantRechazo));
									ListaNotificacion.Add("Por favor validar esta información en nuestra plataforma o puede comunicarse con nuestra area de soporte");
									email.EnviaNotificacionAlertaDIAN(item.Id_Facturador, item.CantRechazo.ToString(), ListaNotificacion, 4, false, facturador.StrMailAdmin, 1);
								}

							}
						}
						catch (Exception excepcion)
						{
							Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.envio, "Error Enviando resumen de documentos por semana");
							throw new ApplicationException(excepcion.Message, excepcion.InnerException);
						}


					}
					else
					{
						try
						{

							//FechaActual = new DateTime(2021, 11, 05);

							int dias_mes = DateTime.DaysInMonth(FechaActual.Year, FechaActual.AddMonths(-1).Month);
							FechaInicial = FechaActual.AddMonths(-1).AddDays(1 - FechaActual.Day);
							FechaFinal = FechaActual.AddMonths(-1).AddDays(dias_mes - FechaActual.Day);

							List<EstadisticaDocumentos> respuesta = ObtenerPorEstado(FechaInicial, FechaFinal, resumen_mes);

							if (respuesta != null)
							{
								foreach (EstadisticaDocumentos item in respuesta)
								{
									Ctl_Empresa emp = new Ctl_Empresa();
									TblEmpresas facturador = emp.Obtener(item.Id_Facturador);
									Ctl_EnvioCorreos email = new Ctl_EnvioCorreos();
									List<string> ListaNotificacion = new List<string>();
									try
									{
										ListaNotificacion.Add(string.Format("aqui encuentras un resumen de tu actividad en el envío de documentos electrónicos a nuestra plataforma correspondiente al mes de {0} que comprende del {1} hasta el {2}.", FechaInicial.ToString("MMMM"), FechaInicial.ToString("D"), FechaFinal.ToString("D")));
									}
									catch (Exception excepcion)
									{
										Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.envio, string.Format("Error encabezado lista de documentos - {0} - {1} - {2}", FechaInicial.ToString("MMMM"), FechaInicial.ToString("D"), FechaFinal.ToString("D")));
										throw new ApplicationException(excepcion.Message, excepcion.InnerException);
									}
									ListaNotificacion.Add("A continuación detallamos la información:");
									ListaNotificacion.Add(string.Format("Saldo disponible de documentos: {0}", item.SaldoPlan));
									ListaNotificacion.Add(string.Format("Total: {0}", item.Cantidad));
									ListaNotificacion.Add(string.Format("Validos: {0}", item.CantValidado));
									ListaNotificacion.Add(string.Format("Rechazados: {0}", item.CantRechazo));
									ListaNotificacion.Add(string.Format("Pendiente Respuesta: {0}", item.CantEnviado));
									ListaNotificacion.Add("Esta información la puede consultar en nuestra plataforma o si tienes alguna duda puede comunicarse con nuestra area de soporte");
									try
									{
										email.EnviaNotificacionAlertaDIAN(item.Id_Facturador, item.Cantidad.ToString(), ListaNotificacion, 4, true, facturador.StrMailAdmin, 4);
									}
									catch (Exception excepcion)
									{
										Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.envio, "Error Enviando Mail con resumen de documentos por mes");
										throw new ApplicationException(excepcion.Message, excepcion.InnerException);
									}
								}

							}
						}
						catch (Exception excepcion)
						{
							Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.envio, string.Format("Error Enviando resumen de documentos por mes - {0}", excepcion.InnerException.Message));
							throw new ApplicationException(excepcion.Message, excepcion.InnerException);
						}
					}

				}
				catch (Exception excepcion)
				{
					Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.envio, string.Format("Error Enviando documento Por la sonda - {0}", excepcion.InnerException.Message));
					throw new ApplicationException(excepcion.Message, excepcion.InnerException);
				}


			});
		}


		public List<EstadisticaDocumentos> ObtenerPorEstado(DateTime FechaInicial, DateTime FechaFinal, bool resumen_mes)
		{
			try
			{

				int estado_rechazado = CategoriaEstado.NoRecibido.GetHashCode();
				int estado_validado = CategoriaEstado.ValidadoDian.GetHashCode();
				int estado_enviado = CategoriaEstado.EnvioDian.GetHashCode();
				int estado_recibido = CategoriaEstado.Recibido.GetHashCode();

				context.Configuration.LazyLoadingEnabled = false;

				if (resumen_mes == false)
				{
					try
					{
						List<EstadisticaDocumentos> respuesta = (from datos in context.TblDocumentos
																 where datos.DatFechaIngreso >= FechaInicial && datos.DatFechaIngreso <= FechaFinal
																 && datos.IdCategoriaEstado == estado_rechazado
																 && datos.StrProveedorEmisor == Constantes.NitResolucionconPrefijo
																 orderby datos.StrEmpresaFacturador
																 group datos by datos.StrEmpresaFacturador into grp
																 select new EstadisticaDocumentos { Id_Facturador = grp.FirstOrDefault().StrEmpresaFacturador, CantRechazo = grp.Count() }).ToList();
						return respuesta;
					}
					catch (Exception excepcion)
					{
						Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.consulta, string.Format("Error Agrupando por Facturador y estado Rechazado - {0}", excepcion.InnerException.Message));
						throw new ApplicationException(excepcion.Message, excepcion.InnerException);
					}
				}
				else
				{

					List<EstadisticaDocumentos> agrupacion = new List<EstadisticaDocumentos>();

					try
					{
						agrupacion = (from datos in context.TblDocumentos
									  where datos.DatFechaIngreso >= FechaInicial && datos.DatFechaIngreso <= FechaFinal
									  && datos.StrProveedorEmisor == Constantes.NitResolucionconPrefijo
									  orderby datos.IdCategoriaEstado
									  group datos by new { datos.StrEmpresaFacturador, datos.IdCategoriaEstado } into grp
									  select new EstadisticaDocumentos
									  {
										  Id_Facturador = grp.FirstOrDefault().StrEmpresaFacturador,
										  Cantidad = grp.Count(),
										  Estado = grp.FirstOrDefault().IdCategoriaEstado,
									  }).ToList().ToList();

					}
					catch (Exception excepcion)
					{
						Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.consulta, string.Format("Error Agrupando por estado de documentos y Facturador - {0}", FechaInicial, FechaFinal));
						throw new ApplicationException(excepcion.Message, excepcion.InnerException);
					}

					List<EstadisticaDocumentos> respuesta = new List<EstadisticaDocumentos>();

					try
					{
						List<string> facturadores = agrupacion.Select(fac => fac.Id_Facturador).Distinct().ToList();
						foreach (string item in facturadores)
						{
							EstadisticaDocumentos estado = new EstadisticaDocumentos();
							List<EstadisticaDocumentos> fact_estados = null;
							try
							{
								fact_estados = agrupacion.Where(x => x.Id_Facturador == item).ToList();

								estado.Id_Facturador = item;
								try
								{
									estado.CantRechazo = fact_estados.Where(x => x.Estado.Equals(estado_rechazado)).FirstOrDefault().Cantidad;
								}
								catch (Exception)
								{
									estado.CantRechazo = 0;
								}

								try
								{
									estado.CantValidado = fact_estados.Where(x => x.Estado.Equals(estado_validado)).FirstOrDefault().Cantidad;
								}
								catch (Exception)
								{

									estado.CantValidado = 0;
								}

								int cant_enviado = 0;
								int cant_recibido = 0;

								try
								{
									cant_enviado = fact_estados.Where(x => x.Estado.Equals(estado_enviado)).FirstOrDefault().Cantidad;
								}
								catch (Exception)
								{
								}

								try
								{
									cant_recibido = fact_estados.Where(x => x.Estado.Equals(estado_recibido)).FirstOrDefault().Cantidad;
								}
								catch (Exception)
								{
								}

								estado.CantEnviado = cant_enviado + cant_recibido;
								estado.Cantidad = estado.CantEnviado + estado.CantRechazo + estado.CantValidado;
							}
							catch (Exception excepcion)
							{
								Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.consulta, string.Format("Error Totalizando por Estado - {0} - {1} - {2} - {3} - {4}", estado.Id_Facturador, fact_estados.Where(x => x.Estado.Equals(estado_rechazado)).FirstOrDefault().Cantidad, fact_estados.Where(x => x.Estado.Equals(estado_validado)).FirstOrDefault().Cantidad, fact_estados.Where(x => x.Estado.Equals(estado_enviado)).FirstOrDefault().Cantidad + fact_estados.Where(x => x.Estado.Equals(estado_recibido)).FirstOrDefault().Cantidad, estado.Cantidad));
								throw new ApplicationException(excepcion.Message, excepcion.InnerException);
							}

							//Busco documentos disponibles
							try
							{
								Ctl_PlanesTransacciones CtrPlanes = new Ctl_PlanesTransacciones();
								var Planes = CtrPlanes.obtenerSaldoDisponibles(item);

								if (Planes != null)
								{
									estado.SaldoPlan = Planes.TDisponible;
								}
							}
							catch (Exception excepcion)
							{
								Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.consulta, "Error Obtieniendo Plan del Facturador");
								throw new ApplicationException(excepcion.Message, excepcion.InnerException);
							}

							respuesta.Add(estado);
						}
					}
					catch (Exception excepcion)
					{
						Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.consulta, "Error Agrupando por Facturador y totalizando por estado");
						throw new ApplicationException(excepcion.Message, excepcion.InnerException);
					}

					return respuesta;
				}




			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.consulta, "Error Consultando documentos Por la sonda - {0}");
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Metodo que se encarga de poner en el PDF la fecha de Validacion del documento en la DIAN
		/// </summary>
		/// <param name="empresa_obligado"></param>
		/// <param name="documento"></param>
		public void GeneracionFechaDian(TblEmpresas empresa_obligado, TblDocumentos documento)
		{

			try
			{
				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;

				// ruta física del xml
				string carpeta_archivos = string.Format("{0}\\{1}\\{2}", plataforma.RutaDmsFisica, Constantes.CarpetaFacturaElectronica, empresa_obligado.StrIdSeguridad.ToString());
				carpeta_archivos = string.Format(@"{0}\{1}", carpeta_archivos, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEConsultaDian);

				// Nombre del archivo Xml 
				string nombre_archivo = HGInetUBLv2_1.NombramientoArchivo.ObtenerXml(documento.IntNumero.ToString(), documento.StrEmpresaFacturador, Enumeracion.ParseToEnum<TipoDocumento>(documento.IntDocTipo), documento.StrPrefijo);

				// ruta del xml
				string ruta_xml = string.Format(@"{0}\{1}.xml", carpeta_archivos, nombre_archivo);

				if (!Archivo.ValidarExistencia(ruta_xml))
					throw new ApplicationException("No se encontró ruta de archivo de respuesta de la DIAN");

				//Proceso para obtener la fecha y hora de la respuesta de la DIAN
				//string ruta_archivo = string.Format(@"{0}\{1}.xml", documento_result.RutaArchivosProceso.Replace("XmlFacturaE", LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEConsultaDian), documento_result.NombreXml);
				FileStream xml_reader_serializacion = new FileStream(ruta_xml, FileMode.Open);
				HGInetUBLv2_1.ApplicationResponseType obj_acuse_serializado = new HGInetUBLv2_1.ApplicationResponseType();
				XmlSerializer serializacion1 = new XmlSerializer(typeof(HGInetUBLv2_1.ApplicationResponseType));
				obj_acuse_serializado = (HGInetUBLv2_1.ApplicationResponseType)serializacion1.Deserialize(xml_reader_serializacion);
				string fecha_doc_resp = obj_acuse_serializado.IssueDate.Value.ToString("yyyy-MM-dd");
				string hora_doc_resp = obj_acuse_serializado.IssueTime.Value.ToString();
				xml_reader_serializacion.Close();

				// ruta física del archivo PDF 
				string ruta_pdf = string.Format(@"{0}\{1}.pdf", carpeta_archivos.Replace(LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEConsultaDian, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian), nombre_archivo);

				// texto para generar en el PDF, revisar si ponemos sólo hasta minutos la fecha
				string texto = string.Format("Fecha Validación DIAN: {0} {1}  DOCUMENTO ELECTRÓNICO GENERADO POR HGI S.A.S NIT 811021438-4", fecha_doc_resp, hora_doc_resp);

				// ejecución para poner el texto en el PDF
				string ruta_pdf_resultado = ruta_pdf.Replace(nombre_archivo, string.Format("{0}_resultado.pdf", nombre_archivo));
				ruta_pdf_resultado = LibreriaGlobalHGInet.Funciones.Pdf.AgregarTexto(ruta_pdf, ruta_pdf_resultado, texto, (float)empresa_obligado.IntPdfCampoDianPosX, (float)empresa_obligado.IntPdfCampoDianPosY, true);

			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Archivos, MensajeTipo.Error, MensajeAccion.actualizacion, "Error generando la fecha de validacion del documento en la representacion grafica");
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}

		}

		/// <summary>
		/// Sonda para importar los archivos de los documentos a Azure y actualizarlo en la BD
		/// </summary>
		/// <returns></returns>
		public async Task SondaProcesoStorage(int anyo)
		{
			try
			{
				var Tarea = TareaProcesoStorage(anyo);
				await Task.WhenAny(Tarea);
			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.actualizacion);
			}

		}


		public async Task TareaProcesoStorage(int anyo)
		{
			await Task.Factory.StartNew(() =>
			{

				try
				{
					int mes = 1;

					Ctl_AlmacenamientoDocs almacenamiento = new Ctl_AlmacenamientoDocs();
					TblAlmacenamientoDocs ultimo_registro = almacenamiento.Obtener().OrderByDescending(x => x.DatFechaSincronizacion).Take(1).FirstOrDefault();

					if (ultimo_registro != null)
					{
						mes = ultimo_registro.DatFechaRegistroDoc.Month;
					}

					DateTime fecha_inicio = new DateTime(anyo, 1, 1);

					for (int j = 0; j <= 12; j++)
					{
						if  (j == 0)
						{
							j = mes;
						}

						fecha_inicio = new DateTime(anyo, j, 1);

						DateTime fecha_fin = new DateTime(anyo, j, 1).AddMonths(1).AddDays(-1);

						DateTime fecha_actual = Fecha.GetFecha();
						//DateTime fecha_3mes_atras = fecha_actual.AddMonths(-3);

						TimeSpan Diff_dates = fecha_actual.Subtract(fecha_fin);

						if (Diff_dates.TotalDays < 91)
							throw new ApplicationException("No se puede sincronizar los archivos a azure");

						int diasmes = DateTime.DaysInMonth(anyo, j);

						//Recorre el mes para importar por dia
						for (int i = 0; i <= diasmes; i++)
						{
							DateTime fecha_proceso = fecha_inicio;
							if (i > 0 && i < diasmes)
								fecha_proceso = fecha_inicio.AddDays(i);

							if (i == diasmes)
								fecha_proceso = fecha_fin;

							List<TblDocumentos> datos = ObtenerAdmin("*", "*", "*", "*", "*", fecha_proceso, fecha_proceso, 0, 2, 1, 30000).OrderBy(x => x.DatFechaIngreso).ToList();

							//try
							//{
							//	List<TblAlmacenamientoDocs> list_almacenados = almacenamiento.Obtener();

							//	List<string> doc_alma = list_almacenados.Select(x => x.StrIdSeguridadDoc.ToString()).Distinct().ToList<string>();

							//	List<string> doc_cons = datos.Select(x => x.StrIdSeguridad.ToString()).Distinct().ToList<string>();

							//	List<string> doc_resultante = doc_cons.Except(doc_alma, StringComparer.OrdinalIgnoreCase).ToList();

							//	if ((doc_resultante == null || doc_resultante.Count == 0) && fecha_proceso.Equals(fecha_fin))
							//	{
							//		//Se envia notificacion a tic para indicar que termino de importar el rango especificado
							//		List<string> mensajes = new List<string>();
							//		mensajes.Add(string.Format("Termino de subir Archivos a Azure: Año {0} - Mes {1}", anyo, mes));
							//		try
							//		{
							//			Ctl_EnvioCorreos email = new Ctl_EnvioCorreos();
							//			email.EnviaNotificacionAlertaDIAN(Constantes.NitResolucionconPrefijo, "0", mensajes, 3, false, Constantes.EmailCopiaOculta, 2);
							//		}
							//		catch (Exception)
							//		{ }

							//		//List<Guid> doc_faltante = doc_resultante.ConvertAll(Guid.Parse);

							//		//foreach (Guid item in doc_faltante)
							//		//{
							//		//	TblDocumentos doc = ObtenerDocumento(item);

							//		//	datos.Add(doc);
							//		//}
							//	}
							//}
							//catch (Exception excepcion)
							//{
							//	RegistroLog.EscribirLog(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.consulta, "Obteniendo los documentos faltantes");
							//}


							if (datos != null && datos.Count > 0)
							{
								AzureStorage conexion = HgiConfiguracion.GetConfiguration().AzureStorage;

								foreach (TblDocumentos item in datos)
								{
									
									List<TblAlmacenamientoDocs> list_alm = almacenamiento.Obtener(item.StrIdSeguridad);

									string nombre_contenedor = string.Format("files-hgidocs-{0}", anyo);

									BlobController contenedor = new BlobController(conexion.connectionString, nombre_contenedor);

									contenedor.CrearContenedor(nombre_contenedor, HGInetUtilidadAzure.TipoAccesoEnum.Blob, null);

									Dictionary<string, string> metadata = new Dictionary<string, string>();

									// Add metadata to the dictionary by calling the Add method
									metadata.Add("Facturador", item.StrEmpresaFacturador);
									metadata.Add("IdSeguridadDoc", item.StrIdSeguridad.ToString());
									metadata.Add("TipoDoc", item.IntDocTipo.ToString());

									string ruta_blob_ubl = string.Empty;
									string ruta_blob_pdf = string.Empty;
									string ruta_blob_zip = string.Empty;
									string ruta_blob_acuse = string.Empty;
									string ruta_blob_resp_dian = string.Empty;
									string archivo_memoria = string.Empty;
									bool actualizarbd_doc = false;
									string url_zip_original = item.StrUrlArchivoZip;
									string url_resp_dian_original = string.Empty;

									//Se valida que el archivo XML del documento no este sincronizado y que no este guardado en BD
									try
									{
										if (list_alm.Count >= 0 && list_alm.Where(x => x.IntConsecutivo.Equals(TipoArchivoStorage.XML.GetHashCode())).FirstOrDefault() == null)
										{
											archivo_memoria = Archivo.ObtenerContenido(item.StrUrlArchivoUbl);

											if (!string.IsNullOrEmpty(archivo_memoria))
											{
												ruta_blob_ubl = contenedor.Enviar(archivo_memoria, Path.GetExtension(item.StrUrlArchivoUbl), metadata, Path.GetFileNameWithoutExtension(item.StrUrlArchivoUbl));
												var Tarea1 = RegistroArchivoStorage(item.StrIdSeguridad, item.DatFechaIngreso, TipoArchivoStorage.XML.GetHashCode(), item.StrUrlArchivoUbl, ruta_blob_ubl);
												url_resp_dian_original = item.StrUrlArchivoUbl;
												item.StrUrlArchivoUbl = ruta_blob_ubl;
												actualizarbd_doc = true;
											}
										}
									}
									catch (Exception excepcion)
									{
										RegistroLog.EscribirLog(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.consulta, "Importando XML del documento");
									}

									Dictionary<string, string> metadata_acuse = new Dictionary<string, string>();
									metadata_acuse.Add("Facturador", item.StrEmpresaFacturador);
									metadata_acuse.Add("IdSeguridadDoc", item.StrIdSeguridad.ToString());
									metadata_acuse.Add("TipoDoc", TipoDocumento.AcuseRecibo.GetHashCode().ToString());

									//Se valida que el archivo XML-ACUSE del documento no este sincronizado y que no este guardado en BD
									try
									{
										if (list_alm.Count >= 0 && list_alm.Where(x => x.IntConsecutivo.Equals(TipoArchivoStorage.XMLACUSE.GetHashCode())).FirstOrDefault() == null)
										{
											archivo_memoria = Archivo.ObtenerContenido(item.StrUrlAcuseUbl);

											if (!string.IsNullOrEmpty(archivo_memoria))
											{
												ruta_blob_acuse = contenedor.Enviar(archivo_memoria, Path.GetExtension(item.StrUrlAcuseUbl), metadata_acuse, Path.GetFileNameWithoutExtension(item.StrUrlAcuseUbl));
												var Tarea1 = RegistroArchivoStorage(item.StrIdSeguridad, item.DatFechaIngreso, TipoArchivoStorage.XMLACUSE.GetHashCode(), item.StrUrlAcuseUbl, ruta_blob_acuse);
												item.StrUrlAcuseUbl = ruta_blob_acuse;
												actualizarbd_doc = true;
											}
										}
									}
									catch (Exception excepcion)
									{
										RegistroLog.EscribirLog(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.consulta, "Importando XML Acuse del documento");
									}

									//Se valida que el archivo XML- Respuesta Dian del documento no este sincronizado y que no este guardado en BD
									try
									{
										if (list_alm.Count >= 0 && list_alm.Where(x => x.IntConsecutivo.Equals(TipoArchivoStorage.XMLRESPDIAN.GetHashCode())).FirstOrDefault() == null)
										{
											string ruta_resp_dian = url_resp_dian_original.Replace("FacturaEDian", LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEConsultaDian);
											archivo_memoria = Archivo.ObtenerContenido(ruta_resp_dian);
											if (!string.IsNullOrEmpty(archivo_memoria))
											{
												ruta_blob_resp_dian = contenedor.Enviar(archivo_memoria, Path.GetExtension(ruta_resp_dian), metadata_acuse, Path.GetFileNameWithoutExtension(ruta_resp_dian));
												var Tarea1 = RegistroArchivoStorage(item.StrIdSeguridad, item.DatFechaIngreso, TipoArchivoStorage.XMLRESPDIAN.GetHashCode(), ruta_resp_dian, ruta_blob_resp_dian);
											}
										}
									}
									catch (Exception excepcion)
									{
										RegistroLog.EscribirLog(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.consulta, "Importando XML Respuesta Dian del documento");
									}

									byte[] archivo = null;

									//Se valida que el archivo PDF del documento no este sincronizado y que no este guardado en BD
									try
									{
										if (list_alm.Count >= 0 && list_alm.Where(x => x.IntConsecutivo.Equals(TipoArchivoStorage.PDF.GetHashCode())).FirstOrDefault() == null)
										{
											archivo = Archivo.ObtenerWeb(item.StrUrlArchivoPdf);

											if (archivo != null)
											{
												ruta_blob_pdf = contenedor.Enviar(archivo, Path.GetExtension(item.StrUrlArchivoPdf), metadata, Path.GetFileNameWithoutExtension(item.StrUrlArchivoPdf));
												var Tarea1 = RegistroArchivoStorage(item.StrIdSeguridad, item.DatFechaIngreso, TipoArchivoStorage.PDF.GetHashCode(), item.StrUrlArchivoPdf, ruta_blob_pdf);
												item.StrUrlArchivoPdf = ruta_blob_pdf;
												actualizarbd_doc = true;
											}
										}
									}
									catch (Exception excepcion)
									{
										RegistroLog.EscribirLog(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.consulta, "Importando PDF del documento");
									}


									//Se valida que el archivo ZIP del documento no este sincronizado y que no este guardado en BD
									try
									{
										if (list_alm.Count >= 0 && list_alm.Where(x => x.IntConsecutivo.Equals(TipoArchivoStorage.ZIP.GetHashCode())).FirstOrDefault() == null)
										{
											archivo = Archivo.ObtenerWeb(item.StrUrlArchivoZip);

											if (archivo != null)
											{
												ruta_blob_zip = contenedor.Enviar(archivo, Path.GetExtension(item.StrUrlArchivoZip), metadata, Path.GetFileNameWithoutExtension(item.StrUrlArchivoZip));
												var Tarea1 = RegistroArchivoStorage(item.StrIdSeguridad, item.DatFechaIngreso, TipoArchivoStorage.ZIP.GetHashCode(), item.StrUrlArchivoZip, ruta_blob_zip);
												item.StrUrlArchivoZip = ruta_blob_zip;
												actualizarbd_doc = true;
											}
										}
									}
									catch (Exception excepcion)
									{
										RegistroLog.EscribirLog(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.consulta, "Importando ZIP del documento");
									}


									//Se valida que el archivo ZIP del attach y pdf del documento no este sincronizado y que no este guardado en BD
									try
									{
										if (list_alm.Count >= 0 && list_alm.Where(x => x.IntConsecutivo.Equals(TipoArchivoStorage.ZIPAttached.GetHashCode())).FirstOrDefault() == null)
										{
											string nombre_archivo = HGInetUBLv2_1.NombramientoArchivo.ObtenerXml(item.IntNumero.ToString(), item.StrEmpresaFacturador, TipoDocumento.Attached, item.StrPrefijo);

											string nombre_zip = Path.GetFileNameWithoutExtension(url_zip_original);

											archivo = Archivo.ObtenerWeb(url_zip_original.Replace(nombre_zip, nombre_archivo));

											if (archivo != null)
											{
												ruta_blob_zip = contenedor.Enviar(archivo, Path.GetExtension(url_zip_original.Replace(nombre_zip, nombre_archivo)), metadata, Path.GetFileNameWithoutExtension(nombre_archivo));
												var Tarea1 = RegistroArchivoStorage(item.StrIdSeguridad, item.DatFechaIngreso, TipoArchivoStorage.ZIPAttached.GetHashCode(), url_zip_original.Replace(nombre_zip, nombre_archivo), ruta_blob_zip);
											}
										}
									}
									catch (Exception excepcion)
									{
										RegistroLog.EscribirLog(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.consulta, "Importando ZIP Attach-PDF del documento");
									}

									//Se valida que el archivo ZIP del documento no este sincronizado y que no este guardado en BD
									try
									{
										if (list_alm.Count >= 0 && list_alm.Where(x => x.IntConsecutivo.Equals(TipoArchivoStorage.ZIPAnexo.GetHashCode())).FirstOrDefault() == null)
										{
											archivo = Archivo.ObtenerWeb(item.StrUrlAnexo);

											if (archivo != null)
											{
												ruta_blob_zip = contenedor.Enviar(archivo, Path.GetExtension(item.StrUrlAnexo), metadata, Path.GetFileNameWithoutExtension(item.StrUrlAnexo));
												var Tarea1 = RegistroArchivoStorage(item.StrIdSeguridad, item.DatFechaIngreso, TipoArchivoStorage.ZIP.GetHashCode(), item.StrUrlAnexo, ruta_blob_zip);
												item.StrUrlAnexo = ruta_blob_zip;
												actualizarbd_doc = true;
											}
										}
									}
									catch (Exception excepcion)
									{
										RegistroLog.EscribirLog(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.consulta, "Importando ZIPAnexos del documento");
									}

									//Se actualiza el documento en bd con los cambios en url
									if (actualizarbd_doc == true)
										Actualizar(item);


									Ctl_EventosRadian ctl_evento = new Ctl_EventosRadian();
									List<TblEventosRadian> list_event = ctl_evento.Obtener(item.StrIdSeguridad);

									try
									{
										if (list_event != null && list_event.Count > 0)
										{

											bool actualizarbd_eve = false;

											foreach (var item_eve in list_event)
											{
												int cont_consecutivo = 10;

												//Evento de Respuesta de la DIAN
												string url_evento_ini = item_eve.StrUrlEvento;
												archivo_memoria = Archivo.ObtenerContenido(item_eve.StrUrlEvento);

												if (!string.IsNullOrEmpty(archivo_memoria))
												{
													metadata_acuse.Add(string.Format("Evento{0}", item_eve.IntEstadoEvento), TipoDocumento.AcuseRecibo.GetHashCode().ToString());
													ruta_blob_acuse = contenedor.Enviar(archivo_memoria, Path.GetExtension(item_eve.StrUrlEvento), metadata_acuse, Path.GetFileNameWithoutExtension(item_eve.StrUrlEvento));
													cont_consecutivo += item_eve.IntEstadoEvento;
													var Tarea1 = RegistroArchivoStorage(item.StrIdSeguridad, item.DatFechaIngreso, cont_consecutivo, item_eve.StrUrlEvento, ruta_blob_acuse);
													item_eve.StrUrlEvento = ruta_blob_acuse;
													actualizarbd_eve = true;
												}

												//Evento enviado la DIAN

												string url_evento = url_evento_ini.Replace(string.Format("-{0}", item_eve.IntEstadoEvento), "");

												archivo_memoria = Archivo.ObtenerContenido(url_evento);

												if (!string.IsNullOrEmpty(archivo_memoria))
												{
													ruta_blob_acuse = contenedor.Enviar(archivo_memoria, Path.GetExtension(url_evento), metadata_acuse, Path.GetFileNameWithoutExtension(url_evento));
													cont_consecutivo += 10;
													var Tarea1 = RegistroArchivoStorage(item.StrIdSeguridad, item.DatFechaIngreso, cont_consecutivo, url_evento, ruta_blob_acuse);
												}

												if (actualizarbd_eve == true)
													ctl_evento.Actualizar(item_eve);

											}

										}
									}
									catch (Exception excepcion)
									{
										RegistroLog.EscribirLog(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.consulta, "Importando Eventos Radian del documento");
									}


								}
							}
						}

					}

					//Se envia notificacion a tic para indicar que termino de importar el rango especificado
					List<string> mensajes = new List<string>();
					mensajes.Add(string.Format("Termino de subir Archivos a Azure: Año: {0} - Fecha terminacion: {1}", anyo, Fecha.GetFecha()));
					try
					{
						Ctl_EnvioCorreos email = new Ctl_EnvioCorreos();
						email.EnviaNotificacionAlertaDIAN(Constantes.NitResolucionconPrefijo, "0", mensajes, 3, false, Constantes.EmailCopiaOculta, 2);
					}
					catch (Exception)
					{ }



				}
				catch (Exception excepcion)
				{
					RegistroLog.EscribirLog(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.consulta, "Sonda para importando Archivos del documento");
				}

			});
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="doc_StrIdSeguridad"></param>
		/// <param name="fecha_ingreso_doc"></param>
		/// <param name="tipo_archivo"></param>
		/// <param name="ruta_anterior"></param>
		/// <param name="ruta_actual"></param>
		/// <returns></returns>
		public async Task RegistroArchivoStorage(Guid doc_StrIdSeguridad, DateTime fecha_ingreso_doc, int tipo_archivo, string ruta_anterior, string ruta_actual)
		{
			try
			{
				var Tarea = TareaRegistroArchivoStorage(doc_StrIdSeguridad, fecha_ingreso_doc, tipo_archivo, ruta_anterior, ruta_actual);
				await Task.WhenAny(Tarea);
			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.actualizacion);
			}

		}


		public async Task TareaRegistroArchivoStorage(Guid doc_StrIdSeguridad, DateTime fecha_ingreso_doc, int tipo_archivo, string ruta_anterior, string ruta_actual)
		{
			await Task.Factory.StartNew(() =>
			{

				Ctl_AlmacenamientoDocs ctl_almacenamiento = new Ctl_AlmacenamientoDocs();
				TblAlmacenamientoDocs almacenamiento = ctl_almacenamiento.Convertir(doc_StrIdSeguridad, fecha_ingreso_doc, tipo_archivo, ruta_anterior, ruta_actual);

				ctl_almacenamiento.Crear(almacenamiento);

			});
		}

	}

}


