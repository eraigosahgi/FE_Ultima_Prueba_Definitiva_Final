using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace HGInetFacturaEServicios
{
	/// <summary>
	/// Controlador para el envío de objetos de Nota Crédito
	/// </summary>
	public class Ctl_NotaCredito
	{
		// ruta donde se encuentra el servicio web
		private static string UrlWcf = "wcf/notacredito.svc";

		/// <summary>
		/// Permite enviar los documentos de tipo NotaCredito por el Facturador Electrónico
		/// Manual Técnico: 5.1.2 Metodo Web: Crear Nota Crédito
		/// </summary>
		/// <param name="UrlWs">ruta principal de ejecución del servicio web HGInet Facturación Electrónica (http)</param>
		/// <param name="Serial">serial de licenciamiento para HGInet Facturación Electrónica</param>
		/// <param name="Identificacion">número de identificación del Facturador Electrónico</param>
		/// <param name="documentos_envio">documentos de tipo NotaCredito</param>
		/// <returns>respuesta del proceso de los documentos</returns>
		public static List<ServicioNotaCredito.DocumentoRespuesta> Enviar(string UrlWs, string Serial, string Identificacion, List<ServicioNotaCredito.NotaCredito> documentos_envio)
		{
			// valida la URL del servicio web
			UrlWs = string.Format("{0}{1}", Ctl_Utilidades.ValidarUrl(UrlWs), UrlWcf);

			// valida el parámetro Serial
			if (string.IsNullOrEmpty(Serial))
				throw new ApplicationException("Parámetro Serial de tipo string inválido.");

			// valida el parámetro Identificacion
			if (string.IsNullOrEmpty(Identificacion))
				throw new ApplicationException("Parámetro Identificacion de tipo string inválido.");

			List<ServicioNotaCredito.NotaCredito> datos = new List<ServicioNotaCredito.NotaCredito>();

			// conexión cliente para el servicio web
			ServicioNotaCredito.ServicioNotaCreditoClient cliente_ws = new ServicioNotaCredito.ServicioNotaCreditoClient();
			cliente_ws.Endpoint.Address = new System.ServiceModel.EndpointAddress(UrlWs);

			try
			{
				// configura la cadena de autenticación para la ejecución del servicio web en SHA1
				string dataKey = Ctl_Utilidades.Encriptar_SHA1(string.Format("{0}{1}", Serial, Identificacion));

				foreach (ServicioNotaCredito.NotaCredito item in documentos_envio)
				{	item.DataKey = dataKey;
				}

				// datos para la petición
				ServicioNotaCredito.RecepcionRequest peticion = new ServicioNotaCredito.RecepcionRequest()
				{
					documentos = documentos_envio.ToArray()
				};

				// ejecución del servicio web
				ServicioNotaCredito.RecepcionResponse respuesta = cliente_ws.Recepcion(peticion);

				// resultado del servicio web
				ServicioNotaCredito.DocumentoRespuesta[] result = respuesta.RecepcionResult;

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
		/// Prueba del servicio web de la plataforma de Facturación Electrónica
		/// </summary>
		/// <param name="UrlWs">ruta principal de ejecución del servicio web HGInet Facturación Electrónica (http)</param>
		/// <returns></returns>
		public static string Test(string UrlWs)
		{   // valida la URL del servicio web
			UrlWs = string.Format("{0}{1}", Ctl_Utilidades.ValidarUrl(UrlWs), UrlWcf);

			// conexión cliente para el servicio web
			ServicioNotaCredito.ServicioNotaCreditoClient cliente_ws = new ServicioNotaCredito.ServicioNotaCreditoClient();
			cliente_ws.Endpoint.Address = new System.ServiceModel.EndpointAddress(UrlWs);

			try
			{
				// datos para la petición
				ServicioNotaCredito.TestRequest peticion = new ServicioNotaCredito.TestRequest();

				// ejecución del servicio web
				ServicioNotaCredito.TestResponse respuesta = cliente_ws.Test(peticion);

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

	}
}
