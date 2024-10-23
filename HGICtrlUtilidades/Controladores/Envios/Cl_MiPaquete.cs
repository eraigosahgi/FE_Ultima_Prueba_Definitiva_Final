using HGICtrlUtilidades.Objetos.Envios;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public class NotificacionError
	{
		/*{"status": 404, "message": {"code": 309, "detail": "there was an error in the query"}}*/

		public DateTime timestamp { get; set; }
		public int status { get; set; }
		public string title { get; set; }
		public string path { get; set; }
		public message message { get; set; }
	}

	public class message
	{
		public string code { get; set; }
		public string title { get; set; }
		public string detail { get; set; }
	}
	public class Cl_MiPaquete
	{
		#region ApiKey

		/// <summary>
		/// Genera el token de usabiliad para mipaquete.com
		/// </summary>
		/// <param name="url_plataforma"></param>
		/// <returns></returns>
		public ApiKeyResponse GenerarApiKey(string url_plataforma, ApiKey datos_acceso)
		{
			try
			{
				ApiKeyResponse api_key = new ApiKeyResponse();

				string url_peticion = string.Format("{0}/generateapikey", url_plataforma).Replace("//generateapikey", "/generateapikey");

				HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url_peticion);

				request.Method = "POST";
				request.ContentType = "application/json";
				request.Headers["session-tracker"] = Guid.NewGuid().ToString();

				string ObjeDatos = JsonConvert.SerializeObject(datos_acceso);

				System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
				byte[] bytes = encoding.GetBytes(ObjeDatos);

				request.ContentLength = bytes.Length;

				using (Stream requestStream = request.GetRequestStream())
				{
					requestStream.Write(bytes, 0, bytes.Length);
				}

				HttpWebResponse response = request.GetResponse() as HttpWebResponse;

				using (StreamReader reader = new StreamReader(response.GetResponseStream()))
				{
					api_key.APIKey = JsonConvert.DeserializeObject<ApiKeyResponse>(reader.ReadToEnd()).APIKey;
				}

				return api_key;
			}
			catch (Exception exception)
			{
				throw new ApplicationException(exception.Message, exception.InnerException);
			}
		}

		#endregion

		#region Obtener Transportadoras (getDeliveryCompanies)

		public List<getDeliveryCompanies> getDeliveryCompanies(ApiKeyResponse datos_acceso)
		{
			try
			{
				List<getDeliveryCompanies> datos_respuesta = new List<getDeliveryCompanies>();

				string url_peticion = string.Format("{0}/getDeliveryCompanies", datos_acceso.URL).Replace("//getDeliveryCompanies", "/getDeliveryCompanies");
				HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url_peticion);

				request.Method = "GET";
				request.ContentType = "application/json";
				request.Headers["session-tracker"] = Guid.NewGuid().ToString();
				request.Headers["apikey"] = datos_acceso.APIKey;


				try
				{
					HttpWebResponse response = request.GetResponse() as HttpWebResponse;
					using (StreamReader reader = new StreamReader(response.GetResponseStream()))
					{
						datos_respuesta = JsonConvert.DeserializeObject<List<getDeliveryCompanies>>(reader.ReadToEnd());
					}
				}
				catch (WebException ex)
				{
					if (ex.Response == null)
						throw ex;

					HttpWebResponse responseExc = (HttpWebResponse)ex.Response;
					using (StreamReader reader = new StreamReader(responseExc.GetResponseStream()))
					{
						try
						{
							var result = JsonConvert.DeserializeObject<NotificacionError>(reader.ReadToEnd());
							throw new ApplicationException(string.Format("getDeliveryCompanies - {0} / {1}", result.message.code, result.message.detail));
						}
						catch (Exception exception)
						{
							throw new ApplicationException(exception.Message, exception.InnerException);
						}
					}
				}

				return datos_respuesta;
			}
			catch (Exception exception)
			{
				throw new ApplicationException(exception.Message, exception.InnerException);
			}
		}

		#endregion

		#region Obtener Historial Envios (getSendings)

		public getSendingsResponse getSendings(ApiKeyResponse datos_acceso, getSendings datos_peticion)
		{
			try
			{
				getSendingsResponse datos_respuesta = new getSendingsResponse();

				string url_peticion = string.Format("{0}/getSendings/1", datos_acceso.URL).Replace("//getSendings/1", "/getSendings/1");
				HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url_peticion);

				request.Method = "POST";
				request.ContentType = "application/json";
				request.Headers["session-tracker"] = Guid.NewGuid().ToString();
				request.Headers["apikey"] = datos_acceso.APIKey;

				string ObjeDatos = JsonConvert.SerializeObject(datos_peticion);

				UTF8Encoding encoding = new UTF8Encoding();
				byte[] bytes = encoding.GetBytes(ObjeDatos);

				request.ContentLength = bytes.Length;

				using (Stream requestStream = request.GetRequestStream())
				{
					requestStream.Write(bytes, 0, bytes.Length);
				}

				try
				{
					HttpWebResponse response = request.GetResponse() as HttpWebResponse;
					using (StreamReader reader = new StreamReader(response.GetResponseStream()))
					{
						datos_respuesta = JsonConvert.DeserializeObject<getSendingsResponse>(reader.ReadToEnd());
					}
				}
				catch (WebException ex)
				{
					if (ex.Response == null)
						throw ex;

					HttpWebResponse responseExc = (HttpWebResponse)ex.Response;
					using (StreamReader reader = new StreamReader(responseExc.GetResponseStream()))
					{
						try
						{
							var result = JsonConvert.DeserializeObject<NotificacionError>(reader.ReadToEnd());
							throw new ApplicationException(string.Format("getSendings - {0} / {1}", result.message.code, result.message.detail));
						}
						catch (Exception exception)
						{
							throw new ApplicationException(exception.Message, exception.InnerException);
						}
					}
				}

				return datos_respuesta;
			}
			catch (Exception exception)
			{
				throw new ApplicationException(exception.Message, exception.InnerException);
			}
		}

		#endregion

		#region Obtener Seguimiento de Envío (getSendingTracking)

		public getSendingTracking getSendingTracking(ApiKeyResponse datos_acceso, int codigo_envio)
		{
			try
			{
				getSendingTracking datos_respuesta = new getSendingTracking();

				string url_peticion = string.Format("{0}/getSendingTracking?mpCode={1}", datos_acceso.URL, codigo_envio).Replace("//getSendingTracking", "/getSendingTracking");
				HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url_peticion);

				request.Method = "GET";
				request.ContentType = "application/json";
				request.Headers["session-tracker"] = Guid.NewGuid().ToString();
				request.Headers["apikey"] = datos_acceso.APIKey;

				try
				{
					HttpWebResponse response = request.GetResponse() as HttpWebResponse;
					using (StreamReader reader = new StreamReader(response.GetResponseStream()))
					{
						datos_respuesta = JsonConvert.DeserializeObject<getSendingTracking>(reader.ReadToEnd());
					}
				}
				catch (WebException ex)
				{
					if (ex.Response == null)
						throw ex;

					HttpWebResponse responseExc = (HttpWebResponse)ex.Response;
					using (StreamReader reader = new StreamReader(responseExc.GetResponseStream()))
					{
						try
						{
							var result = JsonConvert.DeserializeObject<NotificacionError>(reader.ReadToEnd());
							throw new ApplicationException(string.Format("getSendingTracking - {0} / {1}", result.message.code, result.message.detail));
						}
						catch (Exception exception)
						{
							throw new ApplicationException(exception.Message, exception.InnerException);
						}
					}
				}

				return datos_respuesta;
			}
			catch (Exception exception)
			{
				throw new ApplicationException(exception.Message, exception.InnerException);
			}
		}

		#endregion

		#region Cancelar Envío (cancelSending)

		public cancelSendingResponse cancelSending(ApiKeyResponse datos_acceso, cancelSending datos_peticion)
		{
			try
			{
				cancelSendingResponse datos_respuesta = new cancelSendingResponse();

				string url_peticion = string.Format("{0}/cancelSending", datos_acceso.URL).Replace("//cancelSending", "/cancelSending");
				HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url_peticion);

				request.Method = "PUT";
				request.ContentType = "application/json";
				request.Headers["session-tracker"] = Guid.NewGuid().ToString();
				request.Headers["apikey"] = datos_acceso.APIKey;

				string ObjeDatos = JsonConvert.SerializeObject(datos_peticion);

				UTF8Encoding encoding = new UTF8Encoding();
				byte[] bytes = encoding.GetBytes(ObjeDatos);

				request.ContentLength = bytes.Length;

				using (Stream requestStream = request.GetRequestStream())
				{
					requestStream.Write(bytes, 0, bytes.Length);
				}

				try
				{
					HttpWebResponse response = request.GetResponse() as HttpWebResponse;
					using (StreamReader reader = new StreamReader(response.GetResponseStream()))
					{
						datos_respuesta = JsonConvert.DeserializeObject<cancelSendingResponse>(reader.ReadToEnd());
					}
				}
				catch (WebException ex)
				{
					if (ex.Response == null)
						throw ex;

					HttpWebResponse responseExc = (HttpWebResponse)ex.Response;
					using (StreamReader reader = new StreamReader(responseExc.GetResponseStream()))
					{
						try
						{
							var result = JsonConvert.DeserializeObject<NotificacionError>(reader.ReadToEnd());

							if (!result.message.code.Equals("313"))
								throw new ApplicationException(string.Format("cancelSending - {0} / {1}", result.message.code, result.message.detail));
							else
								return new cancelSendingResponse();
						}
						catch (Exception exception)
						{
							throw new ApplicationException(exception.Message, exception.InnerException);
						}
					}
				}

				return datos_respuesta;
			}
			catch (Exception exception)
			{
				throw new ApplicationException(exception.Message, exception.InnerException);
			}
		}

		#endregion

		#region Cotizar Envío (quoteShipping)

		/// <summary>
		/// Cotización de envío
		/// </summary>
		/// <returns></returns>
		public List<quoteShippingResponse> quoteShipping(ApiKeyResponse datos_acceso, quoteShipping datos_peticion)
		{
			try
			{
				List<quoteShippingResponse> datos_respuesta = new List<quoteShippingResponse>();

				string url_peticion = string.Format("{0}/quoteShipping", datos_acceso.URL).Replace("//quoteShipping", "/quoteShipping");
				HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url_peticion);

				request.Method = "POST";
				request.ContentType = "application/json";
				request.Headers["session-tracker"] = Guid.NewGuid().ToString();
				request.Headers["apikey"] = datos_acceso.APIKey;

				string ObjeDatos = JsonConvert.SerializeObject(datos_peticion);

				UTF8Encoding encoding = new UTF8Encoding();
				byte[] bytes = encoding.GetBytes(ObjeDatos);

				request.ContentLength = bytes.Length;

				using (Stream requestStream = request.GetRequestStream())
				{
					requestStream.Write(bytes, 0, bytes.Length);
				}

				try
				{
					HttpWebResponse response = request.GetResponse() as HttpWebResponse;
					using (StreamReader reader = new StreamReader(response.GetResponseStream()))
					{
						datos_respuesta = JsonConvert.DeserializeObject<List<quoteShippingResponse>>(reader.ReadToEnd());
					}
				}
				catch (WebException ex)
				{
					if (ex.Response == null)
						throw ex;

					HttpWebResponse responseExc = (HttpWebResponse)ex.Response;
					using (StreamReader reader = new StreamReader(responseExc.GetResponseStream()))
					{
						try
						{
							var result = JsonConvert.DeserializeObject<NotificacionError>(reader.ReadToEnd());
							throw new ApplicationException(string.Format("quoteShipping - {0} / {1}", result.message.code, result.message.detail));
						}
						catch (Exception exception)
						{
							throw new ApplicationException(exception.Message, exception.InnerException);
						}
					}
				}

				return datos_respuesta;
			}
			catch (Exception exception)
			{
				throw new ApplicationException(exception.Message, exception.InnerException);
			}
		}

		#endregion

		#region Crear Envío (createSending)

		public createSendingResponse createSending(ApiKeyResponse datos_acceso, createSending datos_peticion)
		{
			try
			{
				createSendingResponse datos_respuesta = new createSendingResponse();

				string url_peticion = string.Format("{0}/createSending", datos_acceso.URL).Replace("//createSending", "/createSending");
				HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url_peticion);

				request.Method = "POST";
				request.ContentType = "application/json";
				request.Headers["session-tracker"] = Guid.NewGuid().ToString();
				request.Headers["apikey"] = datos_acceso.APIKey;

				string ObjeDatos = JsonConvert.SerializeObject(datos_peticion);

				UTF8Encoding encoding = new UTF8Encoding();
				byte[] bytes = encoding.GetBytes(ObjeDatos);

				request.ContentLength = bytes.Length;

				using (Stream requestStream = request.GetRequestStream())
				{
					requestStream.Write(bytes, 0, bytes.Length);
				}

				try
				{
					HttpWebResponse response = request.GetResponse() as HttpWebResponse;
					using (StreamReader reader = new StreamReader(response.GetResponseStream()))
					{
						datos_respuesta = JsonConvert.DeserializeObject<createSendingResponse>(reader.ReadToEnd());
					}
				}
				catch (WebException ex)
				{
					if (ex.Response == null)
						throw ex;

					HttpWebResponse responseExc = (HttpWebResponse)ex.Response;
					using (StreamReader reader = new StreamReader(responseExc.GetResponseStream()))
					{
						try
						{
							var result = JsonConvert.DeserializeObject<NotificacionError>(reader.ReadToEnd());
							throw new ApplicationException(string.Format("createSending - {0} / {1}", result.message.code, result.message.detail));
						}
						catch (Exception exception)
						{
							throw new ApplicationException(exception.Message, exception.InnerException);
						}
					}
				}

				return datos_respuesta;
			}
			catch (Exception exception)
			{
				throw new ApplicationException(exception.Message, exception.InnerException);
			}
		}

		#endregion

		#region Crear Usuario (createUser)

		public CreateUser createUser(string url_plataforma, CreateUser datos_peticion)
		{
			try
			{
				CreateUser datos_respuesta = new CreateUser();

				string url_peticion = string.Format("{0}/createUser", url_plataforma).Replace("//createUser", "/createUser");
				HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url_peticion);

				request.Method = "POST";
				request.ContentType = "application/json";
				request.Headers["session-tracker"] = Guid.NewGuid().ToString();

				string ObjeDatos = JsonConvert.SerializeObject(datos_peticion);

				UTF8Encoding encoding = new UTF8Encoding();
				byte[] bytes = encoding.GetBytes(ObjeDatos);

				request.ContentLength = bytes.Length;

				using (Stream requestStream = request.GetRequestStream())
				{
					requestStream.Write(bytes, 0, bytes.Length);
				}

				try
				{
					HttpWebResponse response = request.GetResponse() as HttpWebResponse;
					using (StreamReader reader = new StreamReader(response.GetResponseStream()))
					{
						datos_respuesta = JsonConvert.DeserializeObject<CreateUser>(reader.ReadToEnd());
					}
				}
				catch (WebException ex)
				{
					if (ex.Response == null)
						throw ex;

					HttpWebResponse responseExc = (HttpWebResponse)ex.Response;
					using (StreamReader reader = new StreamReader(responseExc.GetResponseStream()))
					{
						try
						{
							var result = JsonConvert.DeserializeObject<NotificacionError>(reader.ReadToEnd());
							throw new ApplicationException(string.Format("createUser - {0} / {1}", result.message.code, result.message.detail));
						}
						catch (Exception exception)
						{
							throw new ApplicationException(exception.Message, exception.InnerException);
						}
					}
				}

				return datos_respuesta;
			}
			catch (Exception exception)
			{
				throw new ApplicationException(exception.Message, exception.InnerException);
			}
		}

		#endregion

	}
}
