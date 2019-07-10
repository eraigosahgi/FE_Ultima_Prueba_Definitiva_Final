using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Properties;
using LibreriaGlobalHGInet.HgiNet.Controladores;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;

namespace HGInetFacturaEServicios
{
	/// <summary>
	/// Controlador para el envío de objetos de Factura
	/// </summary>
	public class Ctl_Factura
	{
		// ruta donde se encuentra el servicio web
		private static string UrlWcf = "wcf/factura.svc";

		/// <summary>
		/// Permite enviar los documentos de tipo Factura por el Facturador Electrónico
		/// Manual Técnico: 5.1.1 Metodo Web: Crear Factura
		/// </summary>
		/// <param name="UrlWs">ruta principal de ejecución del servicio web HGInet Facturación Electrónica (http)</param>
		/// <param name="Serial">serial de licenciamiento para HGInet Facturación Electrónica</param>
		/// <param name="Identificacion">número de identificación del Facturador Electrónico</param>
		/// <param name="documentos_envio">documentos de tipo Factura</param>
		/// <returns>respuesta del proceso de los documentos</returns>
		public static List<ServicioFactura.DocumentoRespuesta> Enviar(string UrlWs, string Serial, string Identificacion, List<ServicioFactura.Factura> documentos_envio)
		{
			// valida la URL del servicio web
			UrlWs = string.Format("{0}{1}", Ctl_Utilidades.ValidarUrl(UrlWs), UrlWcf);

			// valida el parámetro Serial
			if (string.IsNullOrEmpty(Serial))
				throw new ApplicationException("Parámetro Serial de tipo string inválido.");

			// valida el parámetro Identificacion
			if (string.IsNullOrEmpty(Identificacion))
				throw new ApplicationException("Parámetro Identificacion de tipo string inválido.");

			List<ServicioFactura.Factura> datos = new List<ServicioFactura.Factura>();

			// conexión cliente para el servicio web
			ServicioFactura.ServicioFacturaClient cliente_ws = new ServicioFactura.ServicioFacturaClient();
			cliente_ws.Endpoint.Address = new System.ServiceModel.EndpointAddress(UrlWs);

			try
			{
				// configura la cadena de autenticación para la ejecución del servicio web en SHA1
				string dataKey = Ctl_Utilidades.Encriptar_SHA1(string.Format("{0}{1}", Serial, Identificacion));

				foreach (ServicioFactura.Factura item in documentos_envio)
				{
					if (item == null)
						throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, documentos_envio, "ServicioFactura.Factura"));

					if (item.DocumentoDetalles == null || !item.DocumentoDetalles.Any())
						throw new Exception("El detalle del documento es inválido.");

					if (item.DocumentoFormato != null)
					{
						if (!string.IsNullOrEmpty(item.DocumentoFormato.ArchivoPdf))
						{
							byte[] pdf = Convert.FromBase64String(item.DocumentoFormato.ArchivoPdf);
							//valida el peso del formato
							if (pdf.Length < 5120)
								throw new Exception("El Formato de impresion es inválido.");
						}
					}
					item.DataKey = dataKey;
				}

				// datos para la petición
				ServicioFactura.RecepcionRequest peticion = new ServicioFactura.RecepcionRequest()
				{
					documentos = documentos_envio
				};

				// ejecución del servicio web
				ServicioFactura.RecepcionResponse respuesta = cliente_ws.Recepcion(peticion);

				// resultado del servicio web
				List<ServicioFactura.DocumentoRespuesta> result = respuesta.RecepcionResult;

				if (respuesta != null)
					return result.ToList();
				else
					throw new Exception("Error al obtener los datos con los parámetros indicados.");

			}
			catch (FaultException excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion);
			}
			catch (CommunicationException excepcion)
			{
				throw new Exception(string.Format("Error de comunicación: {0}", excepcion.Message), excepcion);
			}
			catch (Exception excepcion)
			{
				throw excepcion;
			}
			finally
			{
				if (cliente_ws != null)
					cliente_ws.Abort();
			}
		}

		/// <summary>
		/// Permite obtener los documentos enviados a la plataforma a nombre del Adquiriente
		/// </summary>
		/// <param name="UrlWs">ruta principal de ejecución del servicio web HGInet Facturación Electrónica (http)</param>
		/// <param name="Serial">serial de licenciamiento para HGInet Facturación Electrónica</param>
		/// <param name="Identificacion">número de identificación del Adquiriente</param>
		/// <param name="FechaInicio">fecha inicial de consulta</param>
		/// <param name="FechaFin">fecha final de consulta</param>
		/// <returns>Una lista de las Facturas generadas a nombre del adquiriente</returns>
		public static List<ServicioFactura.FacturaConsulta> ObtenerFacturaPorAdquiriente(string UrlWs, string Serial, string Identificacion, DateTime FechaInicio, DateTime FechaFin)
		{
			// valida la URL del servicio web
			UrlWs = string.Format("{0}{1}", Ctl_Utilidades.ValidarUrl(UrlWs), UrlWcf);

			// valida el parámetro Serial
			if (string.IsNullOrEmpty(Serial))
				throw new ApplicationException("Parámetro Serial de tipo string inválido.");

			// valida el parámetro Identificacion
			if (string.IsNullOrEmpty(Identificacion))
				throw new ApplicationException("Parámetro Identificacion de tipo string inválido.");

			if (FechaInicio == null)
				throw new ApplicationException("Fecha inicial inválida.");
			if (FechaFin == null)
				throw new ApplicationException("Fecha final inválida.");

			if (FechaFin < FechaInicio)
				throw new ApplicationException("Fecha final debe ser mayor o igual que la fecha inicial.");

			List<ServicioFactura.FacturaConsulta> datos = new List<ServicioFactura.FacturaConsulta>();

			// conexión cliente para el servicio web
			ServicioFactura.ServicioFacturaClient cliente_ws = new ServicioFactura.ServicioFacturaClient();
			cliente_ws.Endpoint.Address = new System.ServiceModel.EndpointAddress(UrlWs);

			try
			{
				// configura la cadena de autenticación para la ejecución del servicio web en SHA1
				string dataKey = Ctl_Utilidades.Encriptar_SHA1(string.Format("{0}{1}", Serial, Identificacion));

				// datos para la petición
				ServicioFactura.ObtenerPorFechasAdquirienteRequest peticion = new ServicioFactura.ObtenerPorFechasAdquirienteRequest()
				{
					DataKey = dataKey,
					Identificacion = Identificacion,
					FechaInicio = FechaInicio,
					FechaFinal = FechaFin,
				};

				// ejecución del servicio web
				ServicioFactura.ObtenerPorFechasAdquirienteResponse respuesta = cliente_ws.ObtenerPorFechasAdquiriente(peticion);

				// resultado del servicio web
				List<ServicioFactura.FacturaConsulta> result = respuesta.ObtenerPorFechasAdquirienteResult;

				if (respuesta != null)
					return result;
				else
					throw new Exception("Error al obtener los datos con los parámetros indicados.");

			}
			catch (FaultException excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion);
			}
			catch (CommunicationException excepcion)
			{
				throw new Exception(string.Format("Error de comunicación: {0}", excepcion.Message), excepcion);
			}
			catch (Exception excepcion)
			{
				throw excepcion;
			}
			finally
			{
				if (cliente_ws != null)
					cliente_ws.Abort();
			}
		}


		/// <summary>
		/// Permite obtener los documentos enviados a la plataforma a nombre del Adquiriente
		/// </summary>
		/// <param name="UrlWs">ruta principal de ejecución del servicio web HGInet Facturación Electrónica (http)</param>
		/// <param name="Serial">serial de licenciamiento para HGInet Facturación Electrónica</param>
		/// <param name="Identificacion">número de identificación del Adquiriente</param>
		/// <param name="CodigosDocumentos">còdigos de documentos del Facturador Electrónico para consulta separados por el caracter coma (,)</param>
		/// <returns>Una lista de las Facturas generadas a nombre del adquiriente</returns>
		public static List<ServicioFactura.FacturaConsulta> ObtenerPorIdSeguridadAdquiriente(string UrlWs, string Serial, string Identificacion, string CodigosDocumentos)
		{
			// valida la URL del servicio web
			UrlWs = string.Format("{0}{1}", Ctl_Utilidades.ValidarUrl(UrlWs), UrlWcf);

			// valida el parámetro Serial
			if (string.IsNullOrEmpty(Serial))
				throw new ApplicationException("Parámetro Serial de tipo string inválido.");

			// valida el parámetro Identificacion
			if (string.IsNullOrEmpty(Identificacion))
				throw new ApplicationException("Parámetro Identificacion de tipo string inválido.");

			// valida el parámetro Identificacion
			if (string.IsNullOrEmpty(CodigosDocumentos))
				throw new ApplicationException("Parámetro CodigosDocumentos de tipo string inválido.");

			List<ServicioFactura.FacturaConsulta> datos = new List<ServicioFactura.FacturaConsulta>();

			// conexión cliente para el servicio web
			ServicioFactura.ServicioFacturaClient cliente_ws = new ServicioFactura.ServicioFacturaClient();
			cliente_ws.Endpoint.Address = new System.ServiceModel.EndpointAddress(UrlWs);

			try
			{
				// configura la cadena de autenticación para la ejecución del servicio web en SHA1
				string dataKey = Ctl_Utilidades.Encriptar_SHA1(string.Format("{0}{1}", Serial, Identificacion));

				// datos para la petición
				ServicioFactura.ObtenerPorIdSeguridadAdquirienteRequest peticion = new ServicioFactura.ObtenerPorIdSeguridadAdquirienteRequest()
				{
					DataKey = dataKey,
					Identificacion = Identificacion,
					CodigosRegistros = CodigosDocumentos,
				};

				// ejecución del servicio web
				ServicioFactura.ObtenerPorIdSeguridadAdquirienteResponse respuesta = cliente_ws.ObtenerPorIdSeguridadAdquiriente(peticion);

				// resultado del servicio web
				List<ServicioFactura.FacturaConsulta> result = respuesta.ObtenerPorIdSeguridadAdquirienteResult;

				if (respuesta != null)
					return result;
				else
					throw new Exception("Error al obtener los datos con los parámetros indicados.");

			}
			catch (FaultException excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion);
			}
			catch (CommunicationException excepcion)
			{
				throw new Exception(string.Format("Error de comunicación: {0}", excepcion.Message), excepcion);
			}
			catch (Exception excepcion)
			{
				throw excepcion;
			}
			finally
			{
				if (cliente_ws != null)
					cliente_ws.Abort();
			}
		}


		/// <summary>
		/// Prueba del servicio web de la plataforma de Facturación Electrónica
		/// </summary>
		/// <param name="UrlWs">ruta principal de ejecución del servicio web HGInet Facturación Electrónica (http)</param>
		/// <returns></returns>
		public static string Test(string UrlWs)
		{   // valida la URL del servicio web
			UrlWs = string.Format("{0}{1}", Ctl_Utilidades.ValidarUrl(UrlWs), UrlWcf);

			// conexión cliente para el servicio web
			ServicioFactura.ServicioFacturaClient cliente_ws = new ServicioFactura.ServicioFacturaClient();
			cliente_ws.Endpoint.Address = new System.ServiceModel.EndpointAddress(UrlWs);

			try
			{
				// datos para la petición
				ServicioFactura.TestRequest peticion = new ServicioFactura.TestRequest();

				// ejecución del servicio web
				ServicioFactura.TestResponse respuesta = cliente_ws.Test(peticion);

				// resultado del servicio web
				string result = respuesta.TestResult;

				if (respuesta != null)
					return result;
				else
					throw new Exception("Error al obtener los datos con los parámetros indicados.");
			}
			catch (FaultException excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion);
			}
			catch (CommunicationException excepcion)
			{
				throw new Exception(string.Format("Error de comunicación: {0}", excepcion.Message), excepcion);
			}
			catch (Exception excepcion)
			{
				throw excepcion;
			}
			finally
			{
				if (cliente_ws != null)
					cliente_ws.Abort();
			}
		}


		/// <summary>
		/// Calcula el código CUFE de la factura
		/// </summary>     
		/// <param name="clave_tecnica">Clave técnica de la resolución</param>
		/// <param name="numero_factura">Número de la factura</param>
		/// <param name="fecha_factura">Fecha de elaboración de la factura</param>
		/// <param name="nit_facturador">Documento de identificación del facturador electrónico</param>
		/// <param name="tipo_identificacion_adquiriente">Código del tipo de identificación del adquiriente</param>
		/// <param name="nit_adquiriente">Número de identificación del adquiriente</param>
		/// <param name="total">Total de la factura</param>
		/// <param name="subtotal">Subtotal de la factura</param>
		/// <param name="iva">Iva de la factura</param>
		/// <param name="impto_consumo">Impuesto al consumo de la factura</param>
		/// <param name="rte_ica">Retención del ICA de la factura</param>
		/// <returns>Texto con la encriptación del CUFE</returns>
		public static string CalcularCUFE(string clave_tecnica, string prefijo, string numero_factura, DateTime fecha_factura, string nit_facturador, string tipo_identificacion_adquiriente, string nit_adquiriente, decimal total, decimal subtotal, decimal iva, decimal impto_consumo, decimal rte_ica)
		{
			try
			{
				string cufe_encriptado = Ctl_CalculoCufe.CufeFactura(clave_tecnica, prefijo, numero_factura, fecha_factura, nit_facturador, tipo_identificacion_adquiriente, nit_adquiriente, total, subtotal, iva, impto_consumo, rte_ica, true);
				return cufe_encriptado;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Calcula el código CUFE de la factura
		/// </summary>     
		/// <param name="clave_tecnica">Clave técnica de la resolución</param>
		/// <param name="numero_factura">Número de la factura</param>
		/// <param name="fecha_factura">Fecha de elaboración de la factura</param>
		/// <param name="nit_facturador">Documento de identificación del facturador electrónico</param>
		/// <param name="ambiente">Ambiente a donde se va enviar el documento </param>
		/// <param name="nit_adquiriente">Número de identificación del adquiriente</param>
		/// <param name="total">Total de la factura</param>
		/// <param name="subtotal">Subtotal de la factura</param>
		/// <param name="iva">Iva de la factura</param>
		/// <param name="impto_consumo">Impuesto al consumo de la factura</param>
		/// <param name="rte_ica">Retención del ICA de la factura</param>
		/// <returns>Texto con la encriptación del CUFE</returns>
		public static string CalcularCUFEV2(string clave_tecnica, string prefijo, string numero_factura, DateTime fecha_factura, string nit_facturador, string ambiente, string nit_adquiriente, decimal total, decimal subtotal, decimal iva, decimal impto_consumo, decimal rte_ica)
		{
			try
			{
				if (string.IsNullOrEmpty(ambiente))
					throw new Exception("Ambiente de Envío del documento no valido");

				string fecha = fecha_factura.AddHours(5).ToString("HH:mm:sszzz");
				string cufe_encriptado = Ctl_CalculoCufe.CufeFacturaV2(clave_tecnica, prefijo, numero_factura, fecha, nit_facturador, ambiente, nit_adquiriente, total, subtotal, iva, impto_consumo, rte_ica, true);
				return cufe_encriptado;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


	}
}
