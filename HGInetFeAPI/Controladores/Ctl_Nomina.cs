using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
		/// Permite enviar los documentos de tipo Nomina por el Facturador Electrónico
		/// Manual Técnico: 4.10.1 Metodo Web: Crear Nomina
		/// </summary>
		/// <param name="UrlWs">ruta principal de ejecución del servicio web HGI Facturación Electrónica (http)</param>
		/// <param name="Serial">serial de licenciamiento para HGI Facturación Electrónica</param>
		/// <param name="Identificacion">número de identificación del Facturador Electrónico</param>
		/// <param name="documentos_envio">documentos de tipo Nomina</param>
		/// <returns>respuesta del proceso de los documentos</returns>
		public static List<ServicioNomina.DocumentoRespuesta> Enviar(string UrlWs, string Serial, string Identificacion, List<ServicioNomina.Nomina> documentos_envio, bool Obtener_ruta = true, bool Api = false)
		{
			// valida el parámetro Serial
			if (string.IsNullOrEmpty(Serial))
				throw new ApplicationException("Parámetro Serial de tipo string inválido.");

			// valida el parámetro Identificacion
			if (string.IsNullOrEmpty(Identificacion))
				throw new ApplicationException("Parámetro Identificacion de tipo string inválido.");

			// configura la cadena de autenticación para la ejecución del servicio web en SHA1
			string dataKey = Ctl_Utilidades.Encriptar_SHA512(string.Format("{0}{1}", Serial, Identificacion));

			foreach (ServicioNomina.Nomina item in documentos_envio)
			{
				if (item == null)
					throw new ApplicationException("No se encontró informacion en el ServicioNomina.Nomina");
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


			// valida si es un integrador o son pruebas para que obtenga la ruta que le corresponda
			if (!UrlWs.Contains("hgi") && Obtener_ruta)
			{
				UrlWs = Ctl_Utilidades.ObtenerUrl(UrlWs, Identificacion);
			}

			if (Api == true)
			{
				//Url Api
				UrlWs = string.Format("{0}Api/Nomina/Recepcion", Ctl_Utilidades.ValidarUrl(UrlWs));

				string vcData = JsonConvert.SerializeObject(documentos_envio);
				byte[] vtDataStream = Encoding.UTF8.GetBytes(vcData);

				List<ServicioNomina.DocumentoRespuesta> respuesta = new List<ServicioNomina.DocumentoRespuesta>();

				try
				{
					HttpWebRequest vtRequest = (HttpWebRequest)WebRequest.Create(UrlWs);

					vtRequest.Method = "POST";
					vtRequest.ContentType = "application/json";
					vtRequest.Accept = "application/json";
					vtRequest.ContentLength = vtDataStream.Length;

					//Se agrega instruccion para habilitar la seguridad en el envio
					System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

					Stream newStream = vtRequest.GetRequestStream();

					// Enviamos los datos
					newStream.Write(vtDataStream, 0, vtDataStream.Length);
					newStream.Close();

					// ejecución del servicio web
					HttpWebResponse vtHttpResponse = (HttpWebResponse)vtRequest.GetResponse();

					if (vtHttpResponse.StatusCode == HttpStatusCode.OK)
					{
						using (StreamReader vtStreamReader = new StreamReader(vtHttpResponse.GetResponseStream()))
						{
							// Leer el contenido de la respuesta como una cadena JSON
							string jsonResponse = vtStreamReader.ReadToEnd();

							// Deserializar la respuesta JSON en un objeto MiObjeto
							respuesta = JsonConvert.DeserializeObject<List<ServicioNomina.DocumentoRespuesta>>(jsonResponse);
						}

					}
					vtHttpResponse.Close();

					return respuesta;
				}
				catch (WebException ex)
				{
					string ex_message = string.Empty;
					// Manejar excepciones de WebException
					if (ex.Response != null)
					{
						using (HttpWebResponse errorResponse = (HttpWebResponse)ex.Response)
						{
							ex_message = ("Error de la API. Código de estado: " + errorResponse.StatusCode);
							using (StreamReader reader = new StreamReader(errorResponse.GetResponseStream()))
							{
								string errorText = reader.ReadToEnd();
								ex_message = string.Format("{0} - {1} - Error_Message: {2}", ex_message, ("Detalle del error: " + errorText), ex.Message);
							}
						}
					}
					else
					{
						ex_message = ("Error: " + ex.Message);
					}

					throw new Exception(ex_message, ex);
				}

			}
			else
			{
				// valida la URL del servicio web
				UrlWs = string.Format("{0}{1}", Ctl_Utilidades.ValidarUrl(UrlWs), UrlWcf);

				ServicioNomina.ServicioNominaClient cliente_ws = null;

				try
				{
					// conexión cliente para el servicio web
					EndpointAddress endpoint_address = new System.ServiceModel.EndpointAddress(UrlWs);
					cliente_ws = new ServicioNomina.ServicioNominaClient(Ctl_Utilidades.ObtenerBinding(UrlWs, Obtener_ruta), endpoint_address);
					cliente_ws.Endpoint.Address = new System.ServiceModel.EndpointAddress(UrlWs);



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
}
