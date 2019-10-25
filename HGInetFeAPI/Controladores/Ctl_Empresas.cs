using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace HGInetFeAPI
{
	public class Ctl_Empresas
	{
		// ruta donde se encuentra el servicio web
		private static string UrlWcf = "wcf/empresas.svc";

		public static ServicioEmpresas.Empresa Obtener(string UrlWs, string Serial, string Identificacion)
		{
			// valida la URL del servicio web
			UrlWs = string.Format("{0}{1}", Ctl_Utilidades.ValidarUrl(UrlWs), UrlWcf);

			// valida el parámetro Serial
			if (string.IsNullOrEmpty(Serial))
				throw new ApplicationException("Parámetro Serial de tipo string inválido.");

			// valida el parámetro Identificacion
			if (string.IsNullOrEmpty(Identificacion))
				throw new ApplicationException("Parámetro Identificacion de tipo string inválido.");

			List<ServicioEmpresas.Empresa> datos = new List<ServicioEmpresas.Empresa>();

			ServicioEmpresas.ServicioEmpresasClient cliente_ws = null;

			try
			{
				// conexión cliente para el servicio web
				EndpointAddress endpoint_address = new System.ServiceModel.EndpointAddress(UrlWs);
				cliente_ws = new ServicioEmpresas.ServicioEmpresasClient(Ctl_Utilidades.ObtenerBinding(UrlWs), endpoint_address);
				cliente_ws.Endpoint.Address = new System.ServiceModel.EndpointAddress(UrlWs);

				// configura la cadena de autenticación para la ejecución del servicio web en SHA1
				string dataKey = Ctl_Utilidades.Encriptar_SHA512(string.Format("{0}{1}", Serial, Identificacion));

				// datos para la petición
				ServicioEmpresas.ObtenerRequest peticion = new ServicioEmpresas.ObtenerRequest()
				{
					DataKey = dataKey,
					Identificacion = Identificacion,
				};

				// ejecución del servicio web
				ServicioEmpresas.ObtenerResponse respuesta = cliente_ws.Obtener(peticion);

				// resultado del servicio web
				ServicioEmpresas.Empresa result = respuesta.ObtenerResult;

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
