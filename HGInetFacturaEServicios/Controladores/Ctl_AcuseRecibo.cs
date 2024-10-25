using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace HGInetFacturaEServicios
{
	/// <summary>
	/// Controlador para la gestión de Acuse de Recibo
	/// </summary>
	public class Ctl_AcuseRecibo
	{
		// ruta donde se encuentra el servicio web
		private static string UrlWcf = "wcf/acuserecibo.svc";


		/// <summary>
		/// Prueba del servicio web de la plataforma de Facturación Electrónica
		/// </summary>
		/// <param name="UrlWs">ruta principal de ejecución del servicio web HGInet Facturación Electrónica (http)</param>
		/// <returns></returns>
		public static string Test(string UrlWs)
		{   // valida la URL del servicio web
			UrlWs = string.Format("{0}{1}", Ctl_Utilidades.ValidarUrl(UrlWs), UrlWcf);

			// conexión cliente para el servicio web
			ServicioAcuseRecibo.ServicioAcuseReciboClient cliente_ws = new ServicioAcuseRecibo.ServicioAcuseReciboClient();
			cliente_ws.Endpoint.Address = new System.ServiceModel.EndpointAddress(UrlWs);

			try
			{
				// datos para la petición
				ServicioAcuseRecibo.TestRequest peticion = new ServicioAcuseRecibo.TestRequest();

				// ejecución del servicio web
				ServicioAcuseRecibo.TestResponse respuesta = cliente_ws.Test(peticion);

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
