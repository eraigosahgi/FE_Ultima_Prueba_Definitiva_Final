using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public class Cl_CloudMensajeria
	{/// <summary>
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
				string dataKey = Cl_Seguridad.Encriptar_SHA1(string.Format("{0}{1}", Serial, Identificacion));

				MensajeContenidoGlobal.identificacion = Identificacion;
				MensajeContenidoGlobal.serial = Serial;
				MensajeContenidoGlobal.MensajeContenido = mensajes;
				MensajeContenidoGlobal.Aplicacion = Aplicacion;

				Cl_ClienteRest<List<MensajeEnvio>> cliente = new Cl_ClienteRest<List<MensajeEnvio>>(string.Format("{0}/api/Enviar", UrlApi), TipoContenido.Applicationjson.GetHashCode(), "");
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

						Cl_RegistroLog.Guardar(Identificacion, "mails", datos_parametros, ex.Message);

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
	}
}
