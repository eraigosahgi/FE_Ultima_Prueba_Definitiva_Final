using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.ObjetosComunes.Mensajeria;
using LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.Respuesta;
using LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Respuesta;
using LibreriaGlobalHGInet.Peticiones;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaGlobalHGInet.HgiNet.Controladores
{
	public class Ctl_CloudMensajeria
	{
		/// <summary>
		/// Controlador para el envío de correos electrónicos por la plataforma HGInet Email
		/// </summary>
		/// <summary>
		/// Permite enviar contenido por Correo Electrónico de HGInet
		/// </summary>
		/// <param name="UrlApi">ruta principal de ejecución del servicio web HGInet Email (http)</param>
		/// <param name="Serial">serial de licenciamiento para HGInet Email</param>
		/// <param name="Identificacion">número de identificación del cliente</param>
		/// <param name="mensajes">mensajes de email para enviar</param>
		/// <returns>respuesta del proceso de los mensajes</returns>
		public static List<MensajeEnvio> Enviar(string UrlApi, string Serial, string Identificacion, List<MensajeContenido> mensajes, string Aplicacion = "")
		{
			// valida la URL del servicio web
			//  UrlApi = string.Format("{0}{1}", Ctl_Utilidades.ValidarUrl(UrlApi), UrlWcf);

			// valida el parámetro Serial
			if (string.IsNullOrEmpty(Serial))
				throw new ApplicationException("Parámetro Serial de tipo string inválido.");

			// valida el parámetro Identificacion
			if (string.IsNullOrEmpty(Identificacion))
				throw new ApplicationException("Parámetro Identificacion de tipo string inválido.");

			List<MensajeEnvio> datos = new List<MensajeEnvio>();

			List<MensajeEnvio> enviar = null;
			MensajeContenidoGlobal MensajeContenidoGlobal = new MensajeContenidoGlobal();
			try
			{
				// configura la cadena de autenticación para la ejecución del servicio web en SHA1                
				string dataKey = Encriptar.Encriptar_SHA1(string.Format("{0}{1}", Serial, Identificacion));

				MensajeContenidoGlobal.identificacion = Identificacion;
				MensajeContenidoGlobal.serial = Serial;
				MensajeContenidoGlobal.MensajeContenido = mensajes;
				MensajeContenidoGlobal.Aplicacion = Aplicacion;

				ClienteRest<List<MensajeEnvio>> cliente = new ClienteRest<List<MensajeEnvio>>(string.Format("{0}/api/Enviar", UrlApi), LibreriaGlobalHGInet.Peticiones.TipoContenido.Applicationjson.GetHashCode(), "");
				try
				{
					enviar = cliente.POST(MensajeContenidoGlobal);
					if (enviar != null)
						
						if (enviar[0].Data[0].MessageID != "500")
						{
							return enviar;
						}
						else
						{
							throw new Exception(enviar[0].Data[0].Email);
						}

					else
						throw new Exception("Error al obtener los datos con los parámetros indicados.");

				}
				catch (Exception ex)
				{
					try
					{
						string datos_parametros = JsonConvert.SerializeObject(MensajeContenidoGlobal);

						RegistroLog.RegistroLog.Guardar(Identificacion, "mails", datos_parametros, ex.Message);

						var cod = cliente.CodHttp;
						throw new Exception(string.Format(" Error: {0}", ex));
					}
					catch (Exception)
					{
						throw new Exception(ex.Message.ToString());
					}
				}
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
		}

		/// <summary>
		/// Permite enviar contenido por SMS desde HGInet
		/// </summary>
		/// <param name="UrlApi">ruta principal de ejecución del servicio web HGInet Email (http)</param>
		/// <param name="Serial">serial de licenciamiento para HGInet Email</param>
		/// <param name="Identificacion">número de identificación del cliente</param>
		/// <param name="mensajes">mensajes de sms para enviar</param>
		/// <returns>respuesta del proceso de los mensajes sms</returns>
		public static List<MensajeEnvioSms> EnviarSms(string UrlApi, string Serial, string Identificacion, List<MensajeContenidoSms> mensajes, string Aplicacion = "")
		{
			// valida la URL del servicio web
			//UrlApi = string.Format("{0}{1}", Ctl_Utilidades.ValidarUrl(UrlApi), UrlWcf);

			// valida el parámetro Serial
			if (string.IsNullOrEmpty(Serial))
				throw new ApplicationException("Parámetro Serial de tipo string inválido.");

			// valida el parámetro Identificacion
			if (string.IsNullOrEmpty(Identificacion))
				throw new ApplicationException("Parámetro Identificacion de tipo string inválido.");

			List<MensajeEnvio> datos = new List<MensajeEnvio>();
			List<MensajeEnvioSms> enviar = null;
			try
			{
				MensajeContenidoGlobalSms MensajeContenidoGlobal = new MensajeContenidoGlobalSms();
				MensajeContenidoGlobal.identificacion = Identificacion;
				MensajeContenidoGlobal.serial = Serial;
				MensajeContenidoGlobal.MensajeContenidoSms = mensajes;
				MensajeContenidoGlobal.Aplicacion = Aplicacion;

				ClienteRest<List<MensajeEnvioSms>> cliente = new ClienteRest<List<MensajeEnvioSms>>(string.Format("{0}/api/EnviarSms", UrlApi), LibreriaGlobalHGInet.Peticiones.TipoContenido.Applicationjson.GetHashCode(), "");
				try
				{
					enviar = cliente.POST(MensajeContenidoGlobal);
					if (enviar != null)
						if (enviar[0].resultado != 1)
						{
							return enviar;
						}
						else
						{
							throw new Exception(enviar[0].resultado_t);
						}
					else
						throw new Exception("Error al obtener los datos con los parámetros indicados.");






				}
				catch (Exception ex)
				{
					var cod = cliente.CodHttp;
					throw new Exception(string.Format(" Error: {0}", ex));
				}
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
		}

		/// <summary>
		/// Prueba del servicio web de la plataforma de Correos Electrónicos
		/// </summary>
		/// <param name="UrlApi">ruta principal de ejecución del servicio web HGInet Email (http)</param>
		/// <returns>texto de prueba</returns>
		public static string Test(string UrlApi)
		{   // valida la URL del servicio web
			// UrlApi = string.Format("{0}{1}", Ctl_Utilidades.ValidarUrl(UrlApi), UrlWcf);           
			try
			{
				string enviar = null;
				ClienteRest<string> cliente = new ClienteRest<string>(UrlApi, LibreriaGlobalHGInet.Peticiones.TipoContenido.Applicationjson.GetHashCode(), "");
				try
				{
					enviar = cliente.POST("");
					if (enviar != null)
						return enviar;
					else
						throw new Exception("Error al obtener los datos con los parámetros indicados.");

				}
				catch (Exception ex)
				{
					var cod = cliente.CodHttp;
					throw new Exception(string.Format(" Error: {0}", ex));
				}
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
		}




		/// <summary>
		/// Permite consultar el estado de un mensaje en la plataforma de HGInet Email
		/// </summary>
		/// <param name="UrlApi">ruta principal de ejecución del servicio web HGInet Email (http)</param>
		/// <param name="Serial">serial de licenciamiento para HGInet Email</param>
		/// <param name="Identificacion">número de identificación del cliente</param>
		/// <param name="id_mensaje">id del mensaje retornado por la plataforma en el envio</param>
		/// <returns>respuesta del proceso de consulta</returns>
		public static MensajeResumen Obtener(string UrlApi, string Serial, string Identificacion, string IdMensaje)
		{
			// valida la URL del servicio web
			// UrlApi = string.Format("{0}{1}", Ctl_Utilidades.ValidarUrl(UrlApi), UrlWcf);

			// valida el parámetro Serial
			if (string.IsNullOrEmpty(Serial))
				throw new ApplicationException("Parámetro Serial de tipo string inválido.");

			// valida el parámetro Identificacion
			if (string.IsNullOrEmpty(Identificacion))
				throw new ApplicationException("Parámetro Identificacion de tipo string inválido.");

			MensajeResumen enviar = null;
			try
			{
				MensajeResumenGlobal MensajeResumenGlobal = new MensajeResumenGlobal();
				MensajeResumenGlobal.identificacion = Identificacion;
				MensajeResumenGlobal.serial = Serial;
				MensajeResumenGlobal.id_mensaje = IdMensaje;

				ClienteRest<MensajeResumen> cliente = new ClienteRest<MensajeResumen>(string.Format("{0}/api/ObtenerResumenMensaje", UrlApi), LibreriaGlobalHGInet.Peticiones.TipoContenido.Applicationjson.GetHashCode(), "");
				try
				{
					enviar = cliente.POST(MensajeResumenGlobal);
					if (enviar != null)
						return enviar;
					else
						throw new Exception("Error al obtener los datos con los parámetros indicados.");

				}
				catch (Exception ex)
				{
					var cod = cliente.CodHttp;
					throw new Exception(string.Format(" Error: {0}", ex));
				}
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
		}




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

	}

}
