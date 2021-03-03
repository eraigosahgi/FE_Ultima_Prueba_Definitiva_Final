using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace HGInetFeAPI
{
	public class Ctl_Nomina
	{

		// ruta donde se encuentra el servicio web
		private static string UrlWcf = "wcf/nomina.svc";

		/// <summary>
		/// Permite enviar los documentos de tipo Factura por el Facturador Electrónico
		/// Manual Técnico: 5.1.1 Metodo Web: Crear Factura
		/// </summary>
		/// <param name="UrlWs">ruta principal de ejecución del servicio web HGI Facturación Electrónica (http)</param>
		/// <param name="Serial">serial de licenciamiento para HGI Facturación Electrónica</param>
		/// <param name="Identificacion">número de identificación del Facturador Electrónico</param>
		/// <param name="documentos_envio">documentos de tipo Factura</param>
		/// <returns>respuesta del proceso de los documentos</returns>
		public static List<ServicioNomina.DocumentoRespuesta> Enviar(string UrlWs, string Serial, string Identificacion, List<ServicioNomina.Nomina> documentos_envio, bool Obtener_ruta = true)
		{

			// valida si es un integrador o son pruebas para que obtenga la ruta que le corresponda
			if (!UrlWs.Contains("hgi") && Obtener_ruta)
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

			List<ServicioNomina.Nomina> datos = new List<ServicioNomina.Nomina>();

			ServicioNomina.ServicioNominaClient cliente_ws = null;

			try
			{
				// conexión cliente para el servicio web
				EndpointAddress endpoint_address = new System.ServiceModel.EndpointAddress(UrlWs);
				cliente_ws = new ServicioNomina.ServicioNominaClient(Ctl_Utilidades.ObtenerBinding(UrlWs, Obtener_ruta), endpoint_address);
				cliente_ws.Endpoint.Address = new System.ServiceModel.EndpointAddress(UrlWs);

				// configura la cadena de autenticación para la ejecución del servicio web en SHA1
				string dataKey = Ctl_Utilidades.Encriptar_SHA512(string.Format("{0}{1}", Serial, Identificacion));

				foreach (ServicioNomina.Nomina item in documentos_envio)
				{
					if (item == null)
						throw new ApplicationException("No se encontró informacion en el ServicioFactura.Factura");
					//throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, documentos_envio, "ServicioFactura.Factura"));

					if (item.DatosDevengados == null)
						throw new Exception("No se encontró Devengados en el documento.");

					if (item.DatosDeducciones == null)
						throw new Exception("No se encontró Deducciones en el documento.");

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
				ServicioNomina.RecepcionRequest peticion = new ServicioNomina.RecepcionRequest()
				{
					documentos = documentos_envio
				};

				// ejecución del servicio web
				ServicioNomina.RecepcionResponse respuesta = cliente_ws.Recepcion(peticion);

				// resultado del servicio web
				List<ServicioNomina.DocumentoRespuesta> result = respuesta.RecepcionResult;

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

	}
}
