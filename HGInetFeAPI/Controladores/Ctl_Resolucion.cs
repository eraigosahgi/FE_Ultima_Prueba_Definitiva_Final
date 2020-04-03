using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace HGInetFeAPI
{
	/// <summary>
	/// Controlador para la gestión de Resoluciones
	/// </summary>
	public class Ctl_Resolucion
	{
		// ruta donde se encuentra el servicio web
		private static string UrlWcf = "wcf/resolucion.svc";

		/// <summary>
		/// Permite obtener las resoluciones registradas ante la DIAN por el Facturador Electrónico
		/// Manual Técnico: 5.3.1 Metodo Web: Consulta de Resolución
		/// </summary>
		/// <param name="UrlWs">ruta principal de ejecución del servicio web HGI Facturación Electrónica (http)</param>
		/// <param name="Serial">serial de licenciamiento para HGI Facturación Electrónica</param>
		/// <param name="Identificacion">número de identificación del Facturador Electrónico</param>
		/// <returns>datos de las resoluciones</returns>
		public static List<ServicioResolucion.Resolucion> Obtener(string UrlWs, string Serial, string Identificacion)
		{
			// valida la URL del servicio web
			UrlWs = string.Format("{0}{1}", Ctl_Utilidades.ValidarUrl(UrlWs), UrlWcf);

			// valida el parámetro Serial
			if (string.IsNullOrEmpty(Serial))
				throw new ApplicationException("Parámetro Serial de tipo string inválido.");

			// valida el parámetro Identificacion
			if (string.IsNullOrEmpty(Identificacion))
				throw new ApplicationException("Parámetro Identificacion de tipo string inválido.");

			List<ServicioResolucion.Resolucion> datos = new List<ServicioResolucion.Resolucion>();
			
			ServicioResolucion.ServicioResolucionClient cliente_ws = null;

			try
			{
				// conexión cliente para el servicio web
				EndpointAddress endpoint_address = new System.ServiceModel.EndpointAddress(UrlWs);
				cliente_ws = new ServicioResolucion.ServicioResolucionClient(Ctl_Utilidades.ObtenerBinding(UrlWs), endpoint_address);
				cliente_ws.Endpoint.Address = new System.ServiceModel.EndpointAddress(UrlWs);

				// configura la cadena de autenticación para la ejecución del servicio web en SHA1
				string dataKey = Ctl_Utilidades.Encriptar_SHA512(string.Format("{0}{1}", Serial, Identificacion));

				// datos para la petición
				ServicioResolucion.ConsultarRequest peticion = new ServicioResolucion.ConsultarRequest()
				{
					DataKey = dataKey,
					Identificacion = Identificacion
				};

				// ejecución del servicio web
				ServicioResolucion.ConsultarResponse respuesta = cliente_ws.Consultar(peticion);

				// resultado del servicio web
				List<ServicioResolucion.Resolucion> result = respuesta.ConsultarResult;

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
		/// Permite obtener las resoluciones registradas en Bd por el Facturador Electrónico
		/// o Crear una en el Ambiente de Habilitacion
		/// </summary>
		/// <param name="UrlWs">ruta principal de ejecución del servicio web HGI Facturación Electrónica (http)</param>
		/// <param name="Serial">serial de licenciamiento para HGI Facturación Electrónica</param>
		/// <param name="Identificacion">número de identificación del Facturador Electrónico</param>
		/// <param name="Resolucion">Objeto con la Resolucion Ingresada</param>
		/// <returns>datos de las resoluciones</returns>
		public static List<ServicioResolucion.Resolucion> ObtenerResHab(string UrlWs, string Serial, string Identificacion, ServicioResolucion.Resolucion Resolucion)
		{
			// valida la URL del servicio web
			UrlWs = string.Format("{0}{1}", Ctl_Utilidades.ValidarUrl(UrlWs), UrlWcf);

			// valida el parámetro Serial
			if (string.IsNullOrEmpty(Serial))
				throw new ApplicationException("Parámetro Serial de tipo string inválido.");

			// valida el parámetro Identificacion
			if (string.IsNullOrEmpty(Identificacion))
				throw new ApplicationException("Parámetro Identificacion de tipo string inválido.");

			if (Resolucion == null)
				throw new ApplicationException("Objeto Resolucion inválido");

			if (string.IsNullOrEmpty(Resolucion.SetIdDian))
				throw new ApplicationException("Parámetro SetIdDian de tipo string inválido.");

			List<ServicioResolucion.Resolucion> datos = new List<ServicioResolucion.Resolucion>();


			ServicioResolucion.ServicioResolucionClient cliente_ws = null;

			try
			{
				// conexión cliente para el servicio web
				EndpointAddress endpoint_address = new System.ServiceModel.EndpointAddress(UrlWs);
				cliente_ws = new ServicioResolucion.ServicioResolucionClient(Ctl_Utilidades.ObtenerBinding(UrlWs), endpoint_address);
				cliente_ws.Endpoint.Address = new System.ServiceModel.EndpointAddress(UrlWs);

				// configura la cadena de autenticación para la ejecución del servicio web en SHA1
				string dataKey = Ctl_Utilidades.Encriptar_SHA512(string.Format("{0}{1}", Serial, Identificacion));

				Resolucion.DataKey = dataKey;

				// datos para la petición
				ServicioResolucion.ConsultarResolucionRequest peticion = new ServicioResolucion.ConsultarResolucionRequest()
				{
					Resolucion = Resolucion
				};

				// ejecución del servicio web
				ServicioResolucion.ConsultarResolucionResponse respuesta = cliente_ws.ConsultarResolucion(peticion);

				// resultado del servicio web
				List<ServicioResolucion.Resolucion> result = respuesta.ConsultarResolucionResult;

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
		/// <param name="UrlWs">ruta principal de ejecución del servicio web HGI Facturación Electrónica (http)</param>
		/// <returns></returns>
		public static string Test(string UrlWs)
		{   // valida la URL del servicio web
			UrlWs = string.Format("{0}{1}", Ctl_Utilidades.ValidarUrl(UrlWs), UrlWcf);

			ServicioResolucion.ServicioResolucionClient cliente_ws = null;

			try
			{
				// conexión cliente para el servicio web
				EndpointAddress endpoint_address = new System.ServiceModel.EndpointAddress(UrlWs);
				cliente_ws = new ServicioResolucion.ServicioResolucionClient(Ctl_Utilidades.ObtenerBinding(UrlWs), endpoint_address);
				cliente_ws.Endpoint.Address = new System.ServiceModel.EndpointAddress(UrlWs);

				// datos para la petición
				ServicioResolucion.TestRequest peticion = new ServicioResolucion.TestRequest();

				// ejecución del servicio web
				ServicioResolucion.TestResponse respuesta = cliente_ws.Test(peticion);

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
