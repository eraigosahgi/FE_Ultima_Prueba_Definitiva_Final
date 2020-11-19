using HGInetMonitorServController.Utilitarios;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.Respuesta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMonitorServController.Procesos
{
	public class Ctl_EnvioCorreo
	{
		public void EnviarAlerta( string mensaje)
		{
			try
			{
				List<MensajeEnvio> respuesta_email = new List<MensajeEnvio>();

				string asunto = "Alerta Plataformas";

				// convierte las direcciones de los destinatarios
				MailAddressCollection to = ConvertirDestinatarios();

				MailAddress from = new MailAddress(Ctl_Utilidades.ObtenerAppSettings("RemitenteMail"), Ctl_Utilidades.ObtenerAppSettings("RemitenteNombre"));

				// construye el correo para el envío
				Mail correo = new Mail(false, from, to);

				// configura los datos de conexión smtp
				correo.ConfigurarSmtp(Ctl_Utilidades.ObtenerAppSettings("ServidorMail"), Convert.ToInt32(Ctl_Utilidades.ObtenerAppSettings("PuertoMail")), Ctl_Utilidades.ObtenerAppSettings("UsuarioMail"), Ctl_Utilidades.ObtenerAppSettings("ClaveMail"), Convert.ToBoolean(Ctl_Utilidades.ObtenerAppSettings("HabilitaSsl")));

				// codificación UTF8 (por defecto)
				Encoding codificacion = Encoding.UTF8;

				// envía el correo electrónico
				correo.Enviar(mensaje, asunto, false, codificacion);

				//return respuesta_email;

			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Convierte los correos electrónicos de objeto a MailAddressCollection
		/// </summary>
		/// <param name="correos_destino">correos electrónicos</param>
		/// <returns>correos electrónicos</returns>
		public static MailAddressCollection ConvertirDestinatarios()
		{
			try
			{
				MailAddressCollection destinatarios = new MailAddressCollection();

				string lista_mail = Ctl_Utilidades.ObtenerAppSettings("DestintariosMail");
				string nombre = Ctl_Utilidades.ObtenerAppSettings("DestintarioNombre");

				List<string> correos = LibreriaGlobalHGInet.Formato.Coleccion.ConvertirLista(lista_mail, ';');

				foreach (string item in correos)
				{
					destinatarios.Add(new MailAddress(item, nombre));
				}

				return destinatarios;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}

		}

	}
}
