using OpenPop.Mime;
using OpenPop.Pop3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public class Cl_MailPop
	{

		private Pop3Client cliente = null;
		private PopMessage mensaje = null;

		/// <summary>
		/// Configurar los datos del servidor de correo electrónico
		/// </summary>
		/// <param name="servidor"></param>
		/// <param name="puerto"></param>
		/// <param name="usuario"></param>
		/// <param name="clave"></param>
		/// <param name="habilitar_ssl"></param>
		public Cl_MailPop(string servidor, int puerto, string usuario, string clave, bool habilitar_ssl = false)
		{
			try
			{
				this.cliente = new Pop3Client();
				this.cliente.Connect(servidor, puerto, habilitar_ssl);
				this.cliente.Authenticate(usuario, clave);
			}
			catch (Exception excepcion)
			{
				string msg = string.Format("Error conectando con el servidor POP3");
				throw new ExcepcionHgi(excepcion, NotificacionCodigo.ERROR_EN_SERVIDOR, msg);
			}
		}

		/// <summary>
		/// Obtener los correos electrónicos
		/// </summary>
		/// <returns></returns>
		public List<PopMessage> Obtener()
		{
			try
			{
				List<PopMessage> mensajes = new List<PopMessage>();

				int messageCount = this.cliente.GetMessageCount();

				for (int i = messageCount; i > 0; i--)
				{	
					mensajes.Add(new PopMessage(i, this.cliente.GetMessage(i)));
				}

				return mensajes;
			}
			catch (Exception excepcion)
			{
				string msg = string.Format("Error al obtener los correos electrónicos del servidor POP3");
				throw new ExcepcionHgi(excepcion, NotificacionCodigo.ERROR_EN_SERVIDOR, msg);
			}
		}

		/// <summary>
		/// Obtener los correos electrónicos por dirección desde o para
		/// </summary>
		/// <param name="fromAddress">caracter asterisco para omitir filtro</param>
		/// <param name="toAddress">caracter asterisco para omitir filtro</param>
		/// <returns></returns>
		public List<PopMessage> Obtener(string fromAddress = "*", string toAddress = "*")
		{
			try
			{
				List<PopMessage> mensajes = Obtener();

				var relevantMail = mensajes.Where(m => (m.Mensaje.Headers.From.Address == fromAddress || fromAddress.Equals("*"))
				&& (m.Mensaje.Headers.To.Any(n => n.Address == toAddress || toAddress.Equals("*")))).ToList();

				return relevantMail;
			}
			catch (Exception excepcion)
			{
				string msg = string.Format("Error filtrando los mensajes - FromAddress:'{0}' ToAddress:'{1}'", fromAddress, toAddress);
				throw new ExcepcionHgi(excepcion, NotificacionCodigo.ERROR_EN_SERVIDOR, msg);
			}
		}

		/// <summary>
		/// Obtiene el mensaje desde un archivo físico
		/// </summary>
		/// <param name="archivo">nombre archivo de almacenamiento</param>
		/// <returns>datos de mensaje</returns>
		public PopMessage Obtener(string ruta_archivo)
		{
			try
			{
				if (!Cl_Archivo.ValidarExistencia(ruta_archivo))
					throw new ApplicationException("No se encontró el archivo.");

				byte[] archivo_mail = File.ReadAllBytes(ruta_archivo);

				PopMessage mensaje = new PopMessage(0, new Message(archivo_mail));
				
				return mensaje;
			}
			catch (Exception excepcion)
			{
				string msg = string.Format("Error obteniendo el mensaje desde el archivo:'{0}'", mensaje.Id, ruta_archivo);
				throw new ExcepcionHgi(excepcion, NotificacionCodigo.ERROR_EN_SERVIDOR, msg);
			}
		}

		/// <summary>
		/// Obtiene los archivos de un mensaje
		/// </summary>
		/// <param name="mensaje">datos de mensaje</param>
		/// <returns></returns>
		public List<MessagePart> ObtenerAdjuntos(PopMessage mensaje)
		{
			try
			{
				List<MessagePart> adjuntos = mensaje.Mensaje.FindAllAttachments();

				return adjuntos;
			}
			catch (Exception excepcion)
			{
				string msg = string.Format("Error al obtener los adjuntos del mensaje con id:'{0}'", mensaje.Id);
				throw new ExcepcionHgi(excepcion, NotificacionCodigo.ERROR_EN_SERVIDOR, msg);
			}
		}


		/// <summary>
		/// Obtiene el tamaño total de los archivos de un mensaje
		/// </summary>
		/// <param name="mensaje">datos de mensaje</param>
		/// <returns></returns>
		public long ObtenerTamanoAdjuntos(PopMessage mensaje)
		{
			try
			{
				long tamano = 0;

				List<MessagePart> adjuntos = mensaje.Mensaje.FindAllAttachments();

				foreach (var adjunto in adjuntos)
				{
					tamano += adjunto.Body.Length;
				}

				return tamano;
			}
			catch (Exception excepcion)
			{
				string msg = string.Format("Error al obtener los adjuntos del mensaje con id:'{0}'", mensaje.Id);
				throw new ExcepcionHgi(excepcion, NotificacionCodigo.ERROR_EN_SERVIDOR, msg);
			}
		}


		/// <summary>
		/// Valida las extensiones de los archivos de un mensaje
		/// </summary>
		/// <param name="mensaje">datos de mensaje</param>
		/// <param name="extensiones">extensiones permitidas</param>
		public void ValidarExtensionesAdjuntos(PopMessage mensaje, List<string> extensiones)
		{

			List<MessagePart> adjuntos = mensaje.Mensaje.FindAllAttachments();

			foreach (var adjunto in adjuntos)
			{
				string extension = Path.GetExtension(adjunto.FileName).Replace(".","");

				string extension_punto = Path.GetExtension(adjunto.FileName);

				if (!extensiones.Contains(extension) && !extensiones.Contains(extension_punto))
				{
					string msg = string.Format("La extensión {0} del archivo {1} no es permitida.", extension, Path.GetFileName(adjunto.FileName));
					throw new ExcepcionHgi(msg);
				}
			}

		}


		/// <summary>
		/// Almacena los archivos de un mensaje
		/// </summary>
		/// <param name="mensaje">datos de mensaje</param>
		/// <param name="carpeta">carpeta física de almacenamiento</param>
		/// <returns></returns>
		public List<string> GuardarAdjuntos(PopMessage mensaje, string carpeta)
		{
			try
			{
				List<string> rutas_fisicas = new List<string>();

				List<MessagePart> adjuntos = ObtenerAdjuntos(mensaje);

				Cl_Directorio.CrearDirectorio(carpeta);

				foreach (var adjunto in adjuntos)
				{
					string filename = string.Format(@"{0}{1}{2}", carpeta, Path.GetFileNameWithoutExtension(adjunto.FileName), Path.GetExtension(adjunto.FileName));
					adjunto.Save(new FileInfo(filename));

					rutas_fisicas.Add(filename);
				}

				return rutas_fisicas;
			}
			catch (Exception excepcion)
			{
				string msg = string.Format("Error almacenando los adjuntos del mensaje con id:'{0}' en la carpeta:'{1}'", mensaje.Id, carpeta);
				throw new ExcepcionHgi(excepcion, NotificacionCodigo.ERROR_EN_SERVIDOR, msg);
			}
		}

		/// <summary>
		/// Almacena el mensaje
		/// </summary>
		/// <param name="mensaje">datos de mensaje</param>
		/// <param name="carpeta">nombre carpeta física de almacenamiento</param>
		/// <param name="archivo">nombre archivo de almacenamiento</param>
		/// <returns></returns>
		public string Guardar(PopMessage mensaje, string carpeta, string archivo = "")
		{
			try
			{
				string ruta_fisica = "";
				
				if (string.IsNullOrEmpty(archivo))
					archivo = Guid.NewGuid().ToString();

				Cl_Directorio.CrearDirectorio(carpeta);

				ruta_fisica = string.Format(@"{0}{1}.mail", carpeta, archivo);

				mensaje.Mensaje.Save(new FileInfo(ruta_fisica));
				
				return ruta_fisica;
			}
			catch (Exception excepcion)
			{
				string msg = string.Format("Error almacenando el mensaje con id:'{0}' en la carpeta:'{1}'", mensaje.Id, carpeta);
				throw new ExcepcionHgi(excepcion, NotificacionCodigo.ERROR_EN_SERVIDOR, msg);
			}
		}

		/// <summary>
		/// Eliminar mensaje por Id
		/// </summary>
		/// <param name="id_mensaje"></param>
		public void Eliminar(int id_mensaje)
		{
			this.cliente.DeleteMessage(id_mensaje);
		}


		/// <summary>
		/// Elimina todos los correos electrónicos
		/// </summary>
		public void EliminarTodo()
		{
			this.cliente.DeleteAllMessages();
		}
	}

	public class PopMessage
	{
		public int Id { get; set; }
		public Message Mensaje { get; set; }
		System.Net.Mail.MailMessage MensajeMail { get; set; }

		public PopMessage(int id, Message mensaje)
		{
			this.Id = id;
			this.Mensaje = mensaje;
		}

	}

}
