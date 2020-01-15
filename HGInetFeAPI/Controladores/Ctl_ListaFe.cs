using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace HGInetFeAPI
{
	public class Ctl_ListaFe
	{
		// ruta donde se encuentra el servicio web
		private static string UrlWcf = "wcf/listaFe.svc";

		/// <summary>
		/// Permite Obtener las Listas que se utilizan en la Factura Electronica
		/// </summary>
		/// <param name="UrlWs">ruta principal de ejecución del servicio web HGInet Facturación Electrónica (http)</param>
		/// <param name="Serial">serial de licenciamiento para HGInet Facturación Electrónica</param>
		/// <param name="Identificacion">número de identificación del Facturador Electrónico</param>
		/// <param name="CodigoListas">números de codigos para consulta separados por el caracter coma (,) ó enviando el caracter (*) para obtenerlas todas</param>
		/// <returns>Objeto con las Lista de FE</returns>
		public static List<ServicioListaFe.ListaFE> Obtener(string UrlWs, string Serial, string Identificacion, string CodigoLista)
		{
			// valida la URL del servicio web
			UrlWs = string.Format("{0}{1}", Ctl_Utilidades.ValidarUrl(UrlWs), UrlWcf);

			// valida el parámetro Serial
			if (string.IsNullOrEmpty(Serial))
				throw new ApplicationException("Parámetro Serial de tipo string inválido.");

			// valida el parámetro Identificacion
			if (string.IsNullOrEmpty(Identificacion))
				throw new ApplicationException("Parámetro Identificacion de tipo string inválido.");

			List<ServicioListaFe.ListaFE> datos = new List<ServicioListaFe.ListaFE>();

			ServicioListaFe.ServicioListaFeClient cliente_ws = null;

			try
			{
				// conexión cliente para el servicio web
				EndpointAddress endpoint_address = new System.ServiceModel.EndpointAddress(UrlWs);
				cliente_ws = new ServicioListaFe.ServicioListaFeClient(Ctl_Utilidades.ObtenerBinding(UrlWs), endpoint_address);
				cliente_ws.Endpoint.Address = new System.ServiceModel.EndpointAddress(UrlWs);

				// configura la cadena de autenticación para la ejecución del servicio web en SHA1
				string dataKey = Ctl_Utilidades.Encriptar_SHA512(string.Format("{0}{1}", Serial, Identificacion));

				// datos para la petición
				ServicioListaFe.ObtenerRequest peticion = new ServicioListaFe.ObtenerRequest
				{
					DataKey = dataKey,
					Identificacion = Identificacion,
					CodigoLista = CodigoLista
				};

				// ejecución del servicio web
				ServicioListaFe.ObtenerResponse respuesta = cliente_ws.Obtener(peticion);

				// resultado del servicio web
				List<ServicioListaFe.ListaFE> result = respuesta.ObtenerResult;

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
