using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.Peticion
{
	/// <summary>
	/// Archivo adjunto para el mensaje
	/// </summary>
	public class MensajeAdjunto
	{
		/// <summary>
		/// Nombre del archivo con extensión
		/// </summary>
		public string Nombre { get; set; }

		/// <summary>
		/// Datos del archivo en base 64
		/// </summary>
		public string ContenidoB64 { get; set; }

		/// <summary>
		/// Asigna el contenido desde una lectura de bytes
		/// </summary>
		/// <param name="archivo">archivo en bytes</param>
		public void AsignarContenido(byte[] archivo)
		{
			decimal tamano_kb = Math.Round(((decimal)archivo.Length / (decimal)1024), 2);

			if (tamano_kb > 15052)
				throw new ApplicationException(string.Format("El tamaño {0}KB del archivo excede el tamaño de 15MB permitido.", tamano_kb));

			this.ContenidoB64 = Convert.ToBase64String(archivo.ToArray());
		}

		/// <summary>
		/// Asigna el contenido desde una ruta e archivo físico
		/// </summary>
		/// <param name="ruta">ruta local del archivo</param>
		public void AsignarContenido(string ruta)
		{
			if (File.Exists(ruta))
			{
				Byte[] bytes = File.ReadAllBytes(ruta);

				this.AsignarContenido(bytes);
			}
			else
				throw new ApplicationException(string.Format("No se encontró el archivo indicado {0}.", ruta));
		}

	}
}
