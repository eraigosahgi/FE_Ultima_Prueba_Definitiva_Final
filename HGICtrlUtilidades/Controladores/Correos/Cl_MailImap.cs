using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public class Cl_MailImap
	{

		public ImapClient cliente = null;
		public Cl_MailCliente ServidorImap { get; set; }
		public Cl_MailCliente ServidorSmtp { get; set; }

		/// <summary>
		/// Configurar los datos del servidor de correo electrónico
		/// </summary>
		/// <param name="servidor"></param>
		/// <param name="puerto"></param>
		/// <param name="usuario"></param>
		/// <param name="clave"></param>
		/// <param name="habilitar_ssl"></param>
		public Cl_MailImap(string servidor, int puerto, string usuario, string clave, bool habilitar_ssl = false)
		{
			try
			{
				ServidorImap = new Cl_MailCliente()
				{
					Servidor = servidor,
					Puerto = puerto,
					Usuario = usuario,
					Clave = clave,
					Habilitar_ssl = habilitar_ssl,
					TimeOut = 600000
				};

				this.Conectar();
			}
			catch (ExcepcionHgi ex)
			{
				string msg = string.Format("Error conectando con el servidor Imap. - Detalle: {0} - {1}", ex.MensajeAdicional, ex.MensajeResultado);
				throw new ExcepcionHgi(ex, NotificacionCodigo.ERROR_EN_SERVIDOR, msg);
			}
			catch (Exception excepcion)
			{
				string msg = string.Format("Error conectando con el servidor Imap. - Detalle: {0}", excepcion.Message);
				throw new ExcepcionHgi(excepcion, NotificacionCodigo.ERROR_EN_SERVIDOR, msg);
			}
			
		}


		/// <summary>
		/// Conectar el servidor de correo electrónico
		/// </summary>
		public void Conectar()
		{
			try
			{
				if (this.cliente == null)
					this.cliente = new ImapClient();

				if (!this.cliente.IsConnected)
				{
					this.cliente = new ImapClient();
					this.cliente.Connect(ServidorImap.Servidor, ServidorImap.Puerto, ServidorImap.Habilitar_ssl);
					this.cliente.Authenticate(ServidorImap.Usuario, ServidorImap.Clave);
					this.cliente.Timeout = 600000;
				}
			}
			catch (Exception excepcion)
			{
				string msg = string.Format("No se pudo conectar con el servidor Imap. - Detalle: {0}", excepcion.Message);
				throw new ExcepcionHgi(excepcion, NotificacionCodigo.ERROR_EN_SERVIDOR, msg);
			}
		}

		/// <summary>
		/// Desconectar el servidor de correo electrónico
		/// </summary>
		public void Desconectar()
		{
			try
			{
				if (cliente != null && cliente.IsConnected)
					cliente.Disconnect(true);

			}
			catch (Exception excepcion)
			{
				string msg = string.Format("Error desconectando con el servidor Imap");
				throw new ExcepcionHgi(excepcion, NotificacionCodigo.ERROR_EN_SERVIDOR, msg);
			}
		}

		/// <summary>
		/// Obtener los ids de los correos electrónicos
		/// </summary>
		/// <returns></returns>
		public List<UniqueId> ObtenerIds()
		{
			try
			{
				IMailFolder inbox = cliente.Inbox;
				inbox.Open(FolderAccess.ReadWrite);

				List<UniqueId> ids_mensajes = cliente.Inbox.Search(MailKit.Search.SearchQuery.All).ToList();

				return ids_mensajes;
			}
			catch (Exception excepcion)
			{
				string msg = string.Format("Error al obtener los correos electrónicos del servidor Imap");
				throw new ExcepcionHgi(excepcion, NotificacionCodigo.ERROR_EN_SERVIDOR, msg);
			}
		}


		/// <summary>
		/// Obtener los correos electrónicos
		/// </summary>
		/// <returns></returns>
		public List<MimeMessage> Obtener()
		{
			try
			{
				List<MimeMessage> mensajes = new List<MimeMessage>();

				IMailFolder inbox = cliente.Inbox;
				inbox.Open(FolderAccess.ReadWrite);

				var uids = cliente.Inbox.Search(MailKit.Search.SearchQuery.All);

				int messageCount = inbox.Count;

				foreach (var uid in uids)
				{
					var mensaje = cliente.Inbox.GetMessage(uid);
					mensajes.Add(mensaje);
				}

				return mensajes;
			}
			catch (Exception excepcion)
			{
				string msg = string.Format("Error al obtener los correos electrónicos del servidor Imap");
				throw new ExcepcionHgi(excepcion, NotificacionCodigo.ERROR_EN_SERVIDOR, msg);
			}
		}

		/// <summary>
		/// Obtener un correo electrónico por uid
		/// </summary>
		/// <param name="id_mensaje">id</param>
		/// <param name="marcar_leido">indica si lo marca como leído en el servidor</param>
		/// <returns></returns>
		public MimeMessage Obtener(UniqueId id_mensaje, bool marcar_leido = false)
		{
			try
			{
				IMailFolder inbox = cliente.Inbox;
				inbox.Open(FolderAccess.ReadWrite);

				MimeMessage mensaje = cliente.Inbox.GetMessage(id_mensaje);

				if (marcar_leido)
				{
					cliente.Inbox.AddFlags(new UniqueId[] { id_mensaje }, MessageFlags.Seen, false);
				}

				return mensaje;
			}
			catch (Exception excepcion)
			{
				string msg = string.Format("Error al obtener el correo electrónico '{0}' del servidor Imap", id_mensaje);
				throw new ExcepcionHgi(excepcion, NotificacionCodigo.ERROR_EN_SERVIDOR, msg);
			}
		}

		/// <summary>
		/// Reenviar un correo electrónico
		/// </summary>
		/// <param name="id_mensaje"></param>
		/// <param name="mensaje_original"></param>
		/// <param name="cliente_smtp"></param>
		/// <param name="correo_remitente"></param>
		/// <param name="correos_destino"></param>
		/// <param name="contenido_adicional">contenido texto y html</param>
		/// <param name="marcar_leido">indica si lo marca como leído en el servidor</param>
		/// <returns></returns>
		public MimeMessage Reenviar(UniqueId id_mensaje, MimeMessage mensaje_original, Cl_MailCliente cliente_smtp, MailboxAddress correo_remitente, List<MailboxAddress> correos_destino, BodyBuilder contenido_adicional, bool marcar_leido = false)
		{
			try
			{
				IMailFolder inbox = cliente.Inbox;
				inbox.Open(FolderAccess.ReadWrite);

				if (marcar_leido)
				{
					cliente.Inbox.AddFlags(new UniqueId[] { id_mensaje }, MessageFlags.Seen, false);
				}

				MimeMessage mensaje = new MimeMessage();
				mensaje.From.Add(correo_remitente);
				mensaje.To.AddRange(correos_destino);
				mensaje.Subject = "FWD: " + mensaje_original.Subject;

				BodyBuilder contenido = new BodyBuilder();
				contenido.HtmlBody = mensaje_original.HtmlBody ?? string.Empty;
				contenido.TextBody = mensaje_original.TextBody ?? string.Empty;

				List<MimeEntity> adjuntos = ObtenerAdjuntos(mensaje_original);
				foreach (MimeEntity adjunto in adjuntos)
				{
					if (adjunto.IsAttachment)
					{
						contenido.Attachments.Add(adjunto);
					}
				}

				var envia = mensaje_original.From.Mailboxes.FirstOrDefault() ?? mensaje_original.Sender;

				var destinatarios = mensaje_original.To.Mailboxes;
				string enviado_a = string.Empty;
				foreach (var item in destinatarios)
				{
					enviado_a = string.Format("{0} <{1}>, ", item.Name, item.Address);
				}

				if (!string.IsNullOrEmpty(mensaje_original.HtmlBody))
				{
					var contenido_cabecera = new StringWriter();
					contenido_cabecera.WriteLine("<br /><br /><br />");
					contenido_cabecera.WriteLine("<b>---------- Mensaje reenviado ---------</b><br />");
					contenido_cabecera.WriteLine("De: {0} <{1}><br />", envia.Name, envia.Address);
					contenido_cabecera.WriteLine("Fecha: {0}<br />", mensaje_original.Date);
					contenido_cabecera.WriteLine("Asunto: {0}<br />", mensaje_original.Subject);
					contenido_cabecera.WriteLine("Para: {0}<br />", enviado_a);
					contenido_cabecera.WriteLine("<br /><br />");

					using (var writer = new StringWriter())
					{
						if (contenido_adicional != null && !string.IsNullOrEmpty(contenido_adicional.HtmlBody))
							writer.WriteLine(contenido_adicional.HtmlBody);

						writer.WriteLine(contenido_cabecera);

						using (var reader = new StringReader(mensaje_original.HtmlBody))
						{
							string line;

							while ((line = reader.ReadLine()) != null)
							{
								writer.WriteLine(line);
							}
						}
						contenido.HtmlBody = writer.ToString();
					}
				}

				if (!string.IsNullOrEmpty(mensaje_original.TextBody))
				{
					var contenido_cabecera = new StringWriter();
					contenido_cabecera.WriteLine("\n\n");
					contenido_cabecera.WriteLine("---------- Mensaje reenviado ---------\n");
					contenido_cabecera.WriteLine("De: {0} <{1}>\n", envia.Name, envia.Address);
					contenido_cabecera.WriteLine("Fecha: {0}\n", mensaje_original.Date);
					contenido_cabecera.WriteLine("Asunto: {0}\n", mensaje_original.Subject);
					contenido_cabecera.WriteLine("Para: {0}\n", enviado_a);
					contenido_cabecera.WriteLine("\n\n");

					using (var writer = new StringWriter())
					{
						if (contenido_adicional != null && !string.IsNullOrEmpty(contenido_adicional.TextBody))
							writer.WriteLine(contenido_adicional.TextBody + "\n\n");

						writer.WriteLine(contenido_cabecera);

						using (var reader = new StringReader(mensaje_original.TextBody))
						{
							string line;

							while ((line = reader.ReadLine()) != null)
							{
								writer.Write("> ");
								writer.WriteLine(line);
							}
						}
						contenido.TextBody = writer.ToString();
					}
				}
				mensaje.Body = contenido.ToMessageBody();
				
				using (var client = new SmtpClient())
				{
					client.Connect(cliente_smtp.Servidor, cliente_smtp.Puerto, cliente_smtp.Habilitar_ssl);
					client.Authenticate(cliente_smtp.Usuario, cliente_smtp.Clave);
					client.Send(mensaje);
					client.Disconnect(true);
				}
				
				return mensaje;
			}
			catch (Exception excepcion)
			{
				string msg = string.Format("Error al reenviar el correo electrónico '{0}' del servidor Smtp", id_mensaje);
				throw new ExcepcionHgi(excepcion, NotificacionCodigo.ERROR_EN_SERVIDOR, msg);
			}
		}

		/// <summary>
		/// Obtiene los archivos de un mensaje
		/// </summary>
		/// <param name="mensaje">datos de mensaje</param>
		/// <returns></returns>
		public List<MimeEntity> ObtenerAdjuntos(MimeMessage mensaje)
		{
			try
			{
				List<MimeEntity> adjuntos = mensaje.BodyParts.ToList();

				return adjuntos;
			}
			catch (Exception excepcion)
			{
				string msg = string.Format("Error al obtener los adjuntos del mensaje con id:'{0}'", mensaje.MessageId);
				throw new ExcepcionHgi(excepcion, NotificacionCodigo.ERROR_EN_SERVIDOR, msg);
			}
		}


		/// <summary>
		/// Obtiene el tamaño total de los archivos de un mensaje
		/// </summary>
		/// <param name="mensaje">datos de mensaje</param>
		/// <returns></returns>
		public long ObtenerTamanoAdjuntos(MimeMessage mensaje)
		{
			try
			{
				long tamano = 0;

				List<MimeEntity> adjuntos = ObtenerAdjuntos(mensaje);

				foreach (MimeEntity adjunto in adjuntos)
				{

					if (adjunto.IsAttachment)
					{
						using (var stream = new MeasuringStream())
						{
							if (adjunto is MessagePart)
							{
								MessagePart rfc822 = (MessagePart)adjunto;
								rfc822.Message.WriteTo(stream);
							}
							else
							{
								MimePart part = (MimePart)adjunto;
								part.Content.DecodeTo(stream);
							}

							tamano += stream.Length;
						}
					}
				}

				return tamano;
			}
			catch (Exception excepcion)
			{
				string msg = string.Format("Error al obtener los adjuntos del mensaje con id:'{0}'", mensaje.MessageId);
				throw new ExcepcionHgi(excepcion, NotificacionCodigo.ERROR_EN_SERVIDOR, msg);
			}
		}


		/// <summary>
		/// Obtiene propiedades de los archivos adjuntos de un mensaje
		/// </summary>
		/// <param name="mensaje">datos de mensaje</param>
		/// <returns></returns>
		public Cl_MailAdjuntos ObtenerPropiedadesAdjuntos(MimeMessage mensaje, List<string> extensiones = null)
		{
			try
			{
				Cl_MailAdjuntos obj_adjunto = new Cl_MailAdjuntos();

				long tamano = 0;
				int cantidad = 0;

				if(extensiones == null || extensiones.Count == 0)
					extensiones = new List<string> { "zip" };

				List<MimeEntity> adjuntos = ObtenerAdjuntos(mensaje);

				foreach (MimeEntity adjunto in adjuntos)
				{
					if (adjunto.IsAttachment)
					{
						using (var stream = new MeasuringStream())
						{
							if (adjunto is MessagePart)
							{
								MessagePart rfc822 = (MessagePart)adjunto;
								rfc822.Message.WriteTo(stream);
							}
							else
							{
								MimePart part = (MimePart)adjunto;
								part.Content.DecodeTo(stream);
							}

							tamano += stream.Length;
						}

						string archivo = adjunto.ContentDisposition?.FileName ?? adjunto.ContentType.Name;

						string extension = Path.GetExtension(archivo).Replace(".", "");

						string extension_punto = Path.GetExtension(archivo);

						//Si encuentra un adjunto con esa extension lo suma
						if (extensiones.Contains(extension) || extensiones.Contains(extension_punto))
						{
							cantidad++;
						}
					}
				}

				obj_adjunto.Cantidad = cantidad;
				obj_adjunto.TamanoTotal = tamano;

				return obj_adjunto;
			}
			catch (Exception excepcion)
			{
				string msg = string.Format("Error al obtener los adjuntos del mensaje con id:'{0}'", mensaje.MessageId);
				throw new ExcepcionHgi(excepcion, NotificacionCodigo.ERROR_EN_SERVIDOR, msg);
			}
		}



		/// <summary>
		/// Valida las extensiones de los archivos de un mensaje
		/// </summary>
		/// <param name="mensaje">datos de mensaje</param>
		/// <param name="extensiones">extensiones permitidas</param>
		public void ValidarExtensionesAdjuntos(MimeMessage mensaje, List<string> extensiones)
		{

			List<MimeEntity> adjuntos = mensaje.BodyParts.ToList();

			foreach (MimeEntity adjunto in adjuntos)
			{
				if (adjunto.IsAttachment)
				{
					string archivo = adjunto.ContentDisposition?.FileName ?? adjunto.ContentType.Name;

					string extension = Path.GetExtension(archivo).Replace(".", "");

					string extension_punto = Path.GetExtension(archivo);

					if (!extensiones.Contains(extension) && !extensiones.Contains(extension_punto))
					{
						string msg = string.Format("La extensión {0} del archivo {1} no es permitida.", extension, Path.GetFileName(archivo));
						throw new ExcepcionHgi(msg);
					}
				}
			}
		}


		/// <summary>
		/// Almacena el mensaje
		/// </summary>
		/// <param name="mensaje">datos de mensaje</param>
		/// <param name="carpeta">nombre carpeta física de almacenamiento</param>
		/// <param name="archivo">nombre archivo de almacenamiento</param>
		/// <returns></returns>
		public string Guardar(MimeMessage mensaje, string carpeta, string archivo = "")
		{
			try
			{
				string ruta_fisica = "";

				if (string.IsNullOrEmpty(archivo))
					archivo = Guid.NewGuid().ToString();

				Cl_Directorio.CrearDirectorio(carpeta);

				ruta_fisica = string.Format(@"{0}{1}.mail", carpeta, archivo);

				mensaje.WriteTo(ruta_fisica);

				return ruta_fisica;
			}
			catch (Exception excepcion)
			{
				string msg = string.Format("Error almacenando el mensaje con id:'{0}' en la carpeta:'{1}'", mensaje.MessageId, carpeta);
				throw new ExcepcionHgi(excepcion, NotificacionCodigo.ERROR_EN_SERVIDOR, msg);
			}
		}

		/// <summary>
		/// Almacena los archivos de un mensaje
		/// </summary>
		/// <param name="mensaje">datos de mensaje</param>
		/// <param name="carpeta">carpeta física de almacenamiento</param>
		/// <returns></returns>
		public List<string> GuardarAdjuntos(MimeMessage mensaje, string carpeta)
		{
			try
			{
				List<string> rutas_fisicas = new List<string>();

				List<MimeEntity> adjuntos = ObtenerAdjuntos(mensaje);

				Cl_Directorio.CrearDirectorio(carpeta);

				foreach (MimeEntity adjunto in adjuntos)
				{
					string archivo = adjunto.ContentDisposition?.FileName ?? adjunto.ContentType.Name;

					archivo = string.Format(@"{0}{1}", carpeta, archivo);

					bool guarda = false;

					if (adjunto.IsAttachment)
					{
						using (var stream = File.Create(archivo))
						{
							if (adjunto is MessagePart)
							{
								MessagePart rfc822 = (MessagePart)adjunto;
								rfc822.Message.WriteTo(stream);
								guarda = true;
							}
							else
							{
								MimePart part = (MimePart)adjunto;
								part.Content.DecodeTo(stream);
								guarda = true;
							}
						}
					}

					if (guarda)
						rutas_fisicas.Add(archivo);
				}

				return rutas_fisicas;
			}
			catch (Exception excepcion)
			{
				string msg = string.Format("Error almacenando los adjuntos del mensaje con id:'{0}' en la carpeta:'{1}'", mensaje.MessageId, carpeta);
				throw new ExcepcionHgi(excepcion, NotificacionCodigo.ERROR_EN_SERVIDOR, msg);
			}
		}

		/// <summary>
		/// Eliminar mensaje por Id
		/// </summary>
		/// <param name="id_mensaje"></param>
		public bool Eliminar(UniqueId id_mensaje)
		{
			try
			{
				IMailFolder inbox = cliente.Inbox;
				inbox.Open(FolderAccess.ReadWrite);

				MimeMessage mensaje = cliente.Inbox.GetMessage(id_mensaje);

				cliente.Inbox.AddFlags(new UniqueId[] { id_mensaje }, MessageFlags.Deleted, false);

				if (cliente.Capabilities.HasFlag(ImapCapabilities.UidPlus))
					cliente.Inbox.Expunge(new UniqueId[] { id_mensaje });

				return true;
			}
			catch (Exception excepcion)
			{
				string msg = string.Format("Error al eliminar el correo electrónico '{0}' del servidor Imap", id_mensaje);
				throw new ExcepcionHgi(excepcion, NotificacionCodigo.ERROR_EN_SERVIDOR, msg);
			}
		}


		/// <summary>
		/// Elimina todos los correos electrónicos
		/// </summary>
		public void EliminarTodo()
		{
			try
			{
				IMailFolder inbox = cliente.Inbox;
				inbox.Open(FolderAccess.ReadWrite);

				var uids = cliente.Inbox.Search(MailKit.Search.SearchQuery.All);

				int messageCount = inbox.Count;

				foreach (var uid in uids)
				{
					cliente.Inbox.AddFlags(new UniqueId[] { uid }, MessageFlags.Deleted, false);

					if (cliente.Capabilities.HasFlag(ImapCapabilities.UidPlus))
						cliente.Inbox.Expunge(new UniqueId[] { uid });
				}

			}
			catch (Exception excepcion)
			{
				string msg = string.Format("Error al eliminar los correos electrónicos del servidor Imap");
				throw new ExcepcionHgi(excepcion, NotificacionCodigo.ERROR_EN_SERVIDOR, msg);
			}
		}

		/// <summary>
		/// Mueve mensaje por Id a la bandeja No Procesado
		/// </summary>
		/// <param name="id_mensaje"></param>
		public bool MoverNoProcesado(UniqueId id_mensaje)
		{
			try
			{
				IMailFolder inbox = cliente.Inbox;
				inbox.Open(FolderAccess.ReadWrite);

				//Se mueve el correo
				var newFolder = cliente.GetFolder("Correos no procesados");
				newFolder.Append(cliente.Inbox.GetMessage(id_mensaje));

				MimeMessage mensaje = cliente.Inbox.GetMessage(id_mensaje);

				cliente.Inbox.AddFlags(new UniqueId[] { id_mensaje }, MessageFlags.Deleted, false);

				//Se elimina el mansaje de la bandeja de entrada
				if (cliente.Capabilities.HasFlag(ImapCapabilities.UidPlus))
					cliente.Inbox.Expunge(new UniqueId[] { id_mensaje });


				return true;
			}
			catch (Exception excepcion)
			{
				string msg = string.Format("Error al mover el correo electrónico '{0}' del servidor Imap", id_mensaje);
				throw new ExcepcionHgi(excepcion, NotificacionCodigo.ERROR_EN_SERVIDOR, msg);
			}
		}


	}
}
