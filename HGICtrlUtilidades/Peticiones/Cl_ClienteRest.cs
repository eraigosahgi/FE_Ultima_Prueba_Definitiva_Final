using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public class Cl_ClienteRest<T> where T : class
	{
		public class parametros
		{
			public string Codigo { get; set; }
			public string valor { get; set; }
		}

		private int _T_Contenido;
		private string _apiUrl;
		private string _Token;
		/// <summary>
		/// Constructor del cliente rest
		/// </summary>
		/// <param name="apiUrl">Direccion url del servicio rest</param>
		/// <param name="T_Contenido">Tipo contenico Aplition/Json, Text, ect</param>
		/// <param name="Token">Token de seguridad en caso de requerirlo</param>
		public Cl_ClienteRest(string apiUrl, int T_Contenido, string Token)
		{
			_T_Contenido = T_Contenido;
			_apiUrl = apiUrl;
			_Token = Token;
		}

		/// <summary>
		/// se asigna el codigo http de respuesta del servicio rest
		/// </summary>
		private int _CodHttp { get; set; }
		/// <summary>
		/// Retorna el codigo http de respuesta del servicio rest
		/// </summary>
		public int CodHttp
		{
			get { return _CodHttp; }
		}

		/// <summary>
		/// Metodo de configuración para todos los clientes api
		/// </summary>        
		/// <param name="Motodo"> Codigo enumerable (Peticion) GET, POST, PUT,DELETE        
		/// <param name="apiUrl">Direccion url del servicio rest a consumir</param>
		/// <param name="content">Tipo de contenido Applicaction/json, Text, etc</param>
		/// <param name="Token">Token de seguridad en caso de requerirlo</param>
		private HttpWebRequest SetupClient(int Motodo)
		{
			HttpWebRequest client = (HttpWebRequest)HttpWebRequest.Create(string.Format("{0}", _apiUrl));
			client.Method = Cl_Enumeracion.GetDescription(Cl_Enumeracion.GetEnumObjectByValue<TipoPeticion>(Motodo));
			client.ContentType = Cl_Enumeracion.GetDescription(Cl_Enumeracion.GetEnumObjectByValue<TipoContenido>(_T_Contenido));
			if (_Token != "")
				client.Headers.Add("Authorization", "Bearer " + _Token);

			return client;
		}

		/// <summary>
		/// Retorna un objeto con la respuesta del servicio rest solicitado
		/// </summary>        
		/// <returns></returns>
		public T GET()
		{
			T result = null;
			try
			{
				HttpWebRequest request = SetupClient(TipoPeticion.GET.GetHashCode());
				HttpWebResponse response = request.GetResponse() as HttpWebResponse;
				string resp;
				using (StreamReader reader = new StreamReader(response.GetResponseStream()))
				{
					resp = reader.ReadToEnd();
				}
				result = JsonConvert.DeserializeObject<T>(resp);
				_CodHttp = response.StatusCode.GetHashCode();
				return result;
			}
			catch (WebException ex)
			{
				if (ex.Response == null)
					throw ex;

				string resp;
				HttpWebResponse responseExc = (HttpWebResponse)ex.Response;
				using (StreamReader reader = new StreamReader(responseExc.GetResponseStream()))
				{
					resp = reader.ReadToEnd();
				}
				try
				{
					result = JsonConvert.DeserializeObject<T>(resp);
					_CodHttp = responseExc.StatusCode.GetHashCode();
				}
				catch (Exception)
				{
					if (!string.IsNullOrWhiteSpace(resp))
						throw new ApplicationException(resp);
					else
						throw ex;
				}
				return result;
			}
			catch (Exception ex)
			{
				throw new ApplicationException(string.Format("Error: {0}", ex.Message));
			}
		}

		/// <summary>
		/// Ejecuta un servicio rest 
		/// </summary>
		/// <param name="uri">Direccion webapi</param>
		/// <param name="parametros">string o ojbeto json con parametros</param>                
		/// <returns></returns>
		public T POST(object parametros)
		{
			T result = null;
			try
			{
				ServicePointManager.Expect100Continue = false;

				HttpWebRequest request = SetupClient(TipoPeticion.POST.GetHashCode());
				var datos_parametros = JsonConvert.SerializeObject(parametros);
				System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
				byte[] bytes = encoding.GetBytes(datos_parametros.ToString());
				request.ContentLength = bytes.Length;
				using (Stream requestStream = request.GetRequestStream())
				{
					requestStream.Write(bytes, 0, bytes.Length);
				}
				string resp = string.Empty;
				HttpWebResponse response = request.GetResponse() as HttpWebResponse;
				using (StreamReader reader = new StreamReader(response.GetResponseStream()))
				{
					resp = reader.ReadToEnd();
				}
				_CodHttp = response.StatusCode.GetHashCode();
				result = JsonConvert.DeserializeObject<T>(resp);
				return result;
			}
			catch (WebException ex)
			{
				if (ex.Response == null)
					throw ex;

				HttpWebResponse responseExc = (HttpWebResponse)ex.Response;
				string resp = string.Empty;
				using (StreamReader reader = new StreamReader(responseExc.GetResponseStream()))
				{
					resp = reader.ReadToEnd();
				}
				try
				{
					_CodHttp = responseExc.StatusCode.GetHashCode();
					result = JsonConvert.DeserializeObject<T>(resp);
				}
				catch (Exception)
				{
					if (!string.IsNullOrWhiteSpace(resp))
						throw new ApplicationException(resp);
					else
						throw ex;
				}
				return result;
			}
			catch (Exception ex)
			{
				throw new ApplicationException(string.Format("Error: {0}", ex.Message));
			}
		}
		/// <summary>
		/// Retorna un objeto con la respuesta del servicio rest
		/// </summary>        
		/// <param name="parametros">Lista de parametros del servicios Rest</param>        
		/// <returns></returns>
		public T PUT(List<parametros> parametros)
		{
			T result = null;
			try
			{
				HttpWebRequest request = SetupClient(TipoPeticion.PUT.GetHashCode());
				foreach (var item in parametros)
				{
					request.Headers.Add(string.Format("{0}:{1}", item.Codigo, item.valor));
				}
				System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
				byte[] bytes = encoding.GetBytes(request.Headers.ToString());
				request.ContentLength = bytes.Length;
				using (Stream requestStream = request.GetRequestStream())
				{
					requestStream.Write(bytes, 0, bytes.Length);
				}
				string resp = string.Empty;
				HttpWebResponse response = request.GetResponse() as HttpWebResponse;
				using (StreamReader reader = new StreamReader(response.GetResponseStream()))
				{
					resp = reader.ReadToEnd();
				}
				_CodHttp = response.StatusCode.GetHashCode();
				result = JsonConvert.DeserializeObject<T>(resp);
				return result;
			}
			catch (WebException ex)
			{
				if (ex.Response == null)
					throw ex;

				string resp = string.Empty;
				HttpWebResponse responseExc = (HttpWebResponse)ex.Response;
				using (StreamReader reader = new StreamReader(responseExc.GetResponseStream()))
				{
					resp = reader.ReadToEnd();
				}
				try
				{
					_CodHttp = responseExc.StatusCode.GetHashCode();
					result = JsonConvert.DeserializeObject<T>(resp);
				}
				catch (Exception)
				{
					if (!string.IsNullOrWhiteSpace(resp))
						throw new ApplicationException(resp);
					else
						throw ex;
				}
				return result;
			}
			catch (Exception ex)
			{
				throw new ApplicationException(string.Format("Error: {0}", ex.Message));
			}
		}
	}
}
