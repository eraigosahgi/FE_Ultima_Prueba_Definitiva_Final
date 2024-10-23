using HGICtrlUtilidades.Recursos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public class Cl_MailSmtp
	{
		private bool uno_a_uno;
		private MailAddress correo_desde;
		private MailAddressCollection correos_destino;
		private MailAddressCollection correos_copia;
		private MailAddressCollection correos_copia_oculta;
		private SmtpClient cliente = null;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="uno_a_uno">indica si envía los correos independientes por destinatario</param>
		/// <param name="correo_desde">remitente</param>
		/// <param name="correos_destino">destinatarios principales</param>
		/// <param name="correos_copia">destinatarios de copia</param>
		/// <param name="correos_copia_oculta">destinatarios de copia oculta</param>
		public Cl_MailSmtp(bool uno_a_uno, MailAddress correo_desde, MailAddressCollection correos_destino, MailAddressCollection correos_copia = null, MailAddressCollection correos_copia_oculta = null)
		{
			this.uno_a_uno = uno_a_uno;
			this.correo_desde = correo_desde;
			this.correos_destino = correos_destino;
			this.correos_copia = correos_copia;
			this.correos_copia_oculta = correos_copia_oculta;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="uno_a_uno">indica si envía los correos independientes por destinatario</param>
		/// <param name="cliente">información smtp</param>
		/// <param name="correo_desde">remitente</param>
		/// <param name="correos_destino">destinatarios principales</param>
		/// <param name="correos_copia">destinatarios de copia</param>
		/// <param name="correos_copia_oculta">destinatarios de copia oculta</param>
		public Cl_MailSmtp(bool uno_a_uno, SmtpClient cliente, MailAddress correo_desde, MailAddressCollection correos_destino, MailAddressCollection correos_copia = null, MailAddressCollection correos_copia_oculta = null)
		{
			this.uno_a_uno = uno_a_uno;
			this.cliente = cliente;
			this.correo_desde = correo_desde;
			this.correos_destino = correos_destino;
			this.correos_copia = correos_copia;
			this.correos_copia_oculta = correos_copia_oculta;
		}

		/// <summary>
		/// Configurar los datos del servidor de correo electrónico
		/// </summary>
		/// <param name="servidor"></param>
		/// <param name="puerto"></param>
		/// <param name="usuario"></param>
		/// <param name="clave"></param>
		/// <param name="habilitar_ssl"></param>
		public void ConfigurarSmtp(string servidor, int puerto, string usuario, string clave, bool habilitar_ssl = false)
		{
			this.cliente = new SmtpClient(servidor, puerto);
			this.cliente.Credentials = new NetworkCredential(usuario, clave);
			this.cliente.EnableSsl = habilitar_ssl;
		}

		/// <summary>
		/// Ejecuta el envío del correo eletrónico
		/// </summary>
		/// <param name="correo">información del correo</param>
		private void Send(MailMessage correo)
		{
			using (cliente)
			{
				// Evita errores de seguridad en ejecución
				ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

				// Envía el mensaje
				cliente.Send(correo);
			}
		}

		/// <summary>
		/// Valida y asocia los destinatarios del correo electrónico
		/// </summary>
		/// <param name="correo">información del correo</param>
		private void ValidarDestinatarios(MailMessage correo)
		{
			bool sin_destinatarios = true;

			// elimina los correos de destino
			correo.To.Clear();
			correo.CC.Clear();
			correo.Bcc.Clear();

			// correos destino
			if (this.correos_destino != null && !this.uno_a_uno)
			{
				foreach (MailAddress item in this.correos_destino)
				{
					if (ValidarEmail(item.Address))
					{
						correo.To.Add(item);
						sin_destinatarios = false;
					}
				}
			}

			// correos destino copia
			if (this.correos_copia != null)
			{
				foreach (MailAddress item in this.correos_copia)
				{
					if (ValidarEmail(item.Address))
					{
						correo.CC.Add(item);
						sin_destinatarios = false;
					}
				}
			}

			// correos destino copia oculta
			if (this.correos_copia_oculta != null)
			{
				foreach (MailAddress item in this.correos_copia_oculta)
				{
					if (ValidarEmail(item.Address))
					{
						correo.Bcc.Add(item);
						sin_destinatarios = false;
					}
				}
			}

			if (sin_destinatarios)
				throw new ApplicationException("No se encuentra definido el destinatario.");

		}

		/// <summary>
		/// Envía el correo electrónico
		/// *** Se debe realizar la operación ConfigurarSmtp antes de esta ejecución
		/// </summary>
		/// <param name="mensaje">contenido del correo</param>
		/// <param name="asunto">asunto del correo</param>
		/// <param name="contenido_html">indica si el contenido del correo es html</param>
		/// <param name="codificacion" type="System.Text.Encoding">codificación de</param>
		/// <param name="tag_mensaje">indica el texto existente en la plantilla html para reemplazarlo por el contenido enviado, ejemplo: {mensaje}</param>
		/// <param name="ruta_plantilla_html">ruta del archivo HTML plantilla</param>
		/// <param name="rutas_adjuntos">ruta de archivos adjuntos</param>
		public void Enviar(string mensaje, string asunto, bool contenido_html, System.Text.Encoding codificacion, string tag_mensaje = "", string ruta_plantilla_html = "", IEnumerable<string> rutas_adjuntos = null, bool adjuntos_url = false)
		{
			try
			{
				// valida los datos del servidor
				if (this.cliente == null)
					throw new ApplicationException("No se encuentran definidos los datos del servidor de correo.");

				using (MailMessage correo = new MailMessage())
				{
					// asunto del correo
					correo.Subject = asunto;

					//señalamos la codificacion
					correo.BodyEncoding = codificacion;

					//cuerpo tipo HTML
					correo.IsBodyHtml = contenido_html;

					// datos remitente
					correo.Sender = correo_desde;
					correo.From = correo_desde;

					// plantilla Html
					if (!string.IsNullOrWhiteSpace(ruta_plantilla_html))
					{
						FileInfo file = new FileInfo(ruta_plantilla_html);

						mensaje = file.OpenText().ReadToEnd().Replace(tag_mensaje, mensaje);
					}

					//usando vistas alternativas cargamos las imagenes y el cuerpo del mensaje
					AlternateView objHTLMAltView = AlternateView.CreateAlternateViewFromString(mensaje, System.Text.Encoding.UTF8, MediaTypeNames.Text.Html);
					correo.AlternateViews.Add(objHTLMAltView);

					// archivos adjunto
					if (rutas_adjuntos != null)
					{
						if (!adjuntos_url)
						{
							foreach (string ruta_adjunto in rutas_adjuntos)
							{
								Attachment attachment = new System.Net.Mail.Attachment(ruta_adjunto);
								correo.Attachments.Add(attachment);
							}
						}
						else
						{
							foreach (string ruta_adjunto in rutas_adjuntos)
							{
								ArchivoUrl archivo_memoria = Cl_Archivo.Obtener(ruta_adjunto);

								Attachment attachment = new System.Net.Mail.Attachment(archivo_memoria.archivo, archivo_memoria.name, archivo_memoria.contenido.ToString());

								correo.Attachments.Add(attachment);
							}
						}
					}

					// destinatarios
					if (!this.uno_a_uno)
					{
						ValidarDestinatarios(correo);

						// ejecuta el envío del correo
						Send(correo);
					}
					else
					{
						// correos destino
						if (this.correos_destino != null)
						{
							foreach (MailAddress item in this.correos_destino)
							{
								if (ValidarEmail(item.Address))
								{
									correo.To.Add(item);
									Send(correo);
								}
							}
						}
					}
				}
			}
			catch (ExcepcionHgi excepcion)
			{
				throw excepcion;
			}
			catch (Exception exec)
			{
				throw new ApplicationException(RecursoMensajes.MailSendError, exec);
			}
		}

		#region Validaciones

		private bool correo_invalido = false;

		/// <summary>
		/// Valida el correo electrónico
		/// </summary>
		/// <param name="correo_electronico">email</param>
		/// <returns>inidica si es válido (true)</returns>
		public bool ValidarEmail(string correo_electronico)
		{
			correo_invalido = false;

			if (String.IsNullOrEmpty(correo_electronico))
				return false;

			try
			{
				correo_electronico = Regex.Replace(correo_electronico, @"(@)(.+)$", this.ValidarDominio, RegexOptions.None);
			}
			catch (Exception e)
			{
				return false;
			}

			if (correo_invalido)
				return false;

			try
			{
				// valida el correo electrónico
				return Regex.IsMatch(correo_electronico,
					  @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
					  @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
					  RegexOptions.IgnoreCase);
			}
			catch (Exception e)
			{
				return false;
			}
		}

		/// <summary>
		/// Valida el dominio del correo electrónico
		/// </summary>
		/// <param name="match"></param>
		/// <returns></returns>
		private string ValidarDominio(Match match)
		{
			string domainName = match.Groups[2].Value;

			try
			{
				IdnMapping idn = new IdnMapping();
				domainName = idn.GetAscii(domainName);
			}
			catch (ArgumentException)
			{
				correo_invalido = true;
			}

			return match.Groups[1].Value + domainName;
		}

		#endregion
	}
}
