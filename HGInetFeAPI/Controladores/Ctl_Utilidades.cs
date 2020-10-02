using LibreriaGlobalHGInet.Peticiones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Xml;

namespace HGInetFeAPI
{
	class Ctl_Utilidades
	{
	
		/// <summary>
		/// Valida la ruta del servicio web
		/// </summary>
		/// <param name="rutaUrl">ruta del servicio web remoto</param>
		/// <returns>ruta del servicio web</returns>
		public static string ValidarUrl(string rutaUrl)
		{
			if (string.IsNullOrEmpty(rutaUrl))
				throw new Exception("Ruta remota de servicios web no encontrada.");

			if (!rutaUrl[rutaUrl.Length - 1].Equals("/"))
				rutaUrl = string.Format("{0}/", rutaUrl);

			return rutaUrl;
		}

		/// <summary>
		/// Encripta un string usando SHA1 
		/// </summary>
		/// <param name="texto">Texto que se va a encriptar en SHA1</param>
		/// <returns>Texto encriptado como string hexadecimal</returns>
		public static string Encriptar_SHA1(string texto)
		{
			SHA1 sha1 = SHA1Managed.Create();
			ASCIIEncoding encoding = new ASCIIEncoding();
			byte[] stream = null;
			StringBuilder sb = new StringBuilder();
			stream = sha1.ComputeHash(encoding.GetBytes(texto));
			for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
			return sb.ToString();
		}

		public static string Encriptar_SHA512(string texto)
		{
			SHA512 sha512 = SHA512Managed.Create();
			ASCIIEncoding encoding = new ASCIIEncoding();
			byte[] stream = null;
			StringBuilder sb = new StringBuilder();
			stream = sha512.ComputeHash(encoding.GetBytes(texto));
			for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
			return sb.ToString();
		}

		public static System.ServiceModel.Channels.Binding ObtenerBinding(string url, string binding_name)
        {

            string http_tipo = url.ToLowerInvariant().Split(':')[0];

            if (http_tipo.Equals("https"))
            {
                return new System.ServiceModel.WSHttpBinding();

            }
            else
            {
                System.ServiceModel.BasicHttpBinding binding = new System.ServiceModel.BasicHttpBinding();

                if (!string.IsNullOrEmpty(binding_name))
                    binding = new System.ServiceModel.BasicHttpBinding(binding_name);
                
                return binding;
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public static BasicHttpBinding ObtenerBinding(string url)
		{
			try
			{
				/*

				<system.serviceModel>
				<bindings>
				  <basicHttpBinding>
					<binding name="soapBinding" closeTimeout="00:10:00" openTimeout="00:10:00"
							 receiveTimeout="00:10:00" sendTimeout="00:10:00" bypassProxyOnLocal="false"
							 maxBufferPoolSize="2147483647" maxReceivedMessageSize="2147483647"
							 transferMode="Streamed" useDefaultWebProxy="true" messageEncoding="Text">
					  <readerQuotas maxDepth="64" maxStringContentLength="2147483647"
								 maxArrayLength="2147483647" maxBytesPerRead="4096" maxNameTableCharCount="16384" />
					</binding>

					<binding name="Soaphttps" closeTimeout="00:10:00" openTimeout="00:10:00"
											 receiveTimeout="00:10:00" sendTimeout="00:10:00" bypassProxyOnLocal="false"
											 maxBufferPoolSize="2147483647" maxReceivedMessageSize="2147483647"
											 transferMode="Streamed" useDefaultWebProxy="true" messageEncoding="Text">
					  <readerQuotas maxDepth="64" maxStringContentLength="2147483647"
								 maxArrayLength="2147483647" maxBytesPerRead="4096" maxNameTableCharCount="16384" />
					  <security mode="Transport" />
					</binding>

				  </basicHttpBinding>
				</bindings>

				*/
				
				BasicHttpBinding basic_http_binding = new BasicHttpBinding(BasicHttpSecurityMode.None);

				basic_http_binding.Name = "SoapHttpBinding";

				// Obtiene o establece el intervalo de tiempo proporcionado para que una conexión se cierre antes de que el transporte genere una excepción.
				basic_http_binding.CloseTimeout = new TimeSpan(0, 10, 0);

				// Obtiene o establece el intervalo de tiempo proporcionado para que una conexión se abra antes de que el transporte genere una excepción.
				basic_http_binding.OpenTimeout = new TimeSpan(0, 1, 0);

				// Obtiene o establece el intervalo de tiempo que una conexión puede permanecer inactiva, durante el cual no se recibe ningún mensaje de la aplicación, antes de interrumpir la conexión.
				basic_http_binding.ReceiveTimeout = new TimeSpan(0, 10, 0);

				// Obtiene o establece el intervalo de tiempo proporcionado para que una operación de escritura se complete antes de que el transporte genere una excepción.
				basic_http_binding.SendTimeout = new TimeSpan(0, 10, 0);
				
				basic_http_binding.BypassProxyOnLocal = false;				
				basic_http_binding.MaxBufferPoolSize = 2147483647;
				basic_http_binding.MaxReceivedMessageSize = 2147483647;
				basic_http_binding.TransferMode = TransferMode.Streamed;
				basic_http_binding.UseDefaultWebProxy = true;
				basic_http_binding.MessageEncoding = WSMessageEncoding.Text;
				basic_http_binding.ReaderQuotas = new XmlDictionaryReaderQuotas()
				{
					MaxDepth = 64,
					MaxStringContentLength = 2147483647,
					MaxArrayLength = 2147483647,
					MaxBytesPerRead = 4096,
					MaxNameTableCharCount = 16384
				};
				
				// conexión segura https
				if (url.Contains("https://"))
				{
					basic_http_binding.Security.Mode = BasicHttpSecurityMode.Transport;
					basic_http_binding.Name = "SoapHttpsBinding";
				}
				
				return basic_http_binding;
			}
			catch (Exception excepcion)
			{
				throw excepcion;
			}
		}

		/// <summary>
		/// Obtiene la Ruta para emitir o consultar los documentos de FE
		/// </summary>
		/// <param name="ruta">ruta de produccion o habilitacion</param>
		/// <param name="identificacion">identificacion del facturador</param>
		/// <returns></returns>
		public static string ObtenerUrl(string ruta, string identificacion)
		{

			int ambiente = 1;
			int version = 2;
			string url_retorno = string.Empty;
			try
			{
				
				string url_plataforma = "https://cloudservices.hginet.co/";
				

				//si la ruta contiene esta informacion cambio al ambiente de pruebas
				if (ruta.Contains("habilitacion"))
				{
					ambiente = 2;
				}

				//Se consulta la ruta disponible para el integrador
				ClienteRest<string> cliente_rest = new ClienteRest<string>(string.Format("{0}/api/facturae/ObtenerServidorFE?ambiente={1}&version={2}&identificacion_empresa={3}", url_plataforma, ambiente, version, identificacion), 1, "");
				try
				{
					url_retorno = cliente_rest.GET();
				}
				catch (Exception ex)
				{
					//Se prueba consultando a otra ruta
					string url_plataforma2 = "https://cloudservices2.hginet.co/";

					ClienteRest<string> cliente_rest2 = new ClienteRest<string>(string.Format("{0}/api/facturae/ObtenerServidorFE?ambiente={1}&version={2}&identificacion_empresa={3}", url_plataforma2, ambiente, version, identificacion), 1, "");
					try
					{
						url_retorno = cliente_rest.GET();
					}
					catch (Exception)
					{
						var cod = cliente_rest.CodHttp;
						throw new ApplicationException(ex.Message, ex.InnerException);
					}

					//var cod = cliente_rest.CodHttp;
					//throw new ApplicationException(ex.Message, ex.InnerException);
				}

				if (string.IsNullOrWhiteSpace(url_retorno))
					throw new ApplicationException("Ruta principal de licencia vacía.");

				return url_retorno;
			}
			catch (Exception e)
			{

				throw new ApplicationException(e.Message, e.InnerException);
			}
		}


	}
}
