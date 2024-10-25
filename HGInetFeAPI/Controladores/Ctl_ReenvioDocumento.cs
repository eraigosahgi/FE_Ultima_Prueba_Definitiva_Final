using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace HGInetFeAPI
{

	/// <summary>
	/// Controlador para el reenvio de un documento a un correo especifico
	/// </summary>
	public class Ctl_ReenvioDocumento
	{

		// ruta donde se encuentra el servicio web
		private static string UrlWcf = "wcf/reenviodocumento.svc";

		/// <summary>
		/// 
		/// </summary>
		/// <param name="UrlWs"></param>
		/// <param name="Serial"></param>
		/// <param name="Identificacion"></param>
		/// <param name="RadicadoDocumento"></param>
		/// <param name="Email"></param>
		/// <returns></returns>
		public static List<ServicioReenvioDocumento.NotificacionCorreo> Recepcion(string UrlWs, string Serial, string Identificacion, List<ServicioReenvioDocumento.EnvioDocumento> datos_envio)
		{
			// valida si es un integrador o son pruebas para que obtenga la ruta que le corresponda
			if (!UrlWs.Contains("hgi"))
			{
				UrlWs = Ctl_Utilidades.ObtenerUrl(UrlWs, Identificacion);
			}

			// valida la URL del servicio web
			UrlWs = string.Format("{0}{1}", Ctl_Utilidades.ValidarUrl(UrlWs), UrlWcf);

			// valida el parámetro Serial
			if (string.IsNullOrEmpty(Serial))
				throw new ApplicationException("Parámetro Serial de tipo string inválido.");

			// valida el parámetro Identificacion
			if (string.IsNullOrEmpty(Identificacion))
				throw new ApplicationException("Parámetro Identificacion de tipo string inválido.");

			List<ServicioFactura.Factura> datos = new List<ServicioFactura.Factura>();
			
			ServicioReenvioDocumento.ServicioReenvioDocumentoClient cliente_ws = null;

			try
			{
				// conexión cliente para el servicio web
				EndpointAddress endpoint_address = new System.ServiceModel.EndpointAddress(UrlWs);
				cliente_ws = new ServicioReenvioDocumento.ServicioReenvioDocumentoClient(Ctl_Utilidades.ObtenerBinding(UrlWs), endpoint_address);
				cliente_ws.Endpoint.Address = new System.ServiceModel.EndpointAddress(UrlWs);

				// configura la cadena de autenticación para la ejecución del servicio web en SHA1
				string dataKey = Ctl_Utilidades.Encriptar_SHA512(string.Format("{0}{1}", Serial, Identificacion));


				foreach (ServicioReenvioDocumento.EnvioDocumento item in datos_envio)
				{

					if (item == null)
						throw new ApplicationException("No se encontró informacion en el ServicioFactura.Factura");
					//throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, datos_envio, "ServicioFactura.Factura"));

					if (string.IsNullOrEmpty(item.RadicadoDocumento))
						throw new Exception("El RadicadoDocumento es inválido.");

					if (string.IsNullOrEmpty(item.Email))
						throw new Exception("El Email es inválido.");

					item.DataKey = dataKey;
				}

				// datos para la petición
				ServicioReenvioDocumento.RecepcionRequest peticion = new ServicioReenvioDocumento.RecepcionRequest()
				{
					documentos = datos_envio
				};


				// ejecución del servicio web
				ServicioReenvioDocumento.RecepcionResponse respuesta = cliente_ws.Recepcion(peticion);

				// resultado del servicio web
				List<ServicioReenvioDocumento.NotificacionCorreo> result = respuesta.RecepcionResult;

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
