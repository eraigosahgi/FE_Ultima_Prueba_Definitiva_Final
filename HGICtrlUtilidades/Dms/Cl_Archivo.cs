using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public class Cl_Archivo
	{
		/// <summary>
		/// valida si exite o no el archivo
		/// </summary>
		public static bool ValidarExistencia(string ruta)
		{
			if (File.Exists(ruta))
				return true;
			else
				return false;
		}

		/// <summary>
		/// crea el archivo en la ruta indicada
		/// </summary>
		public static bool Crear(string ruta)
		{
			if (!File.Exists(ruta))
			{
				FileStream fs = File.Create(ruta);
				fs.Close();
			}
			return true;
		}


		/// <summary>
		/// mueve un archivo existente
		/// </summary>
		public static bool Mover(string ruta, string ruta_destino, string archivo_destino)
		{
			if (ValidarExistencia(ruta))
			{
				ruta_destino = Cl_Directorio.CrearDirectorio(ruta_destino) + archivo_destino;

				//mueve el archivo de ubicaciòn
				FileInfo file = new FileInfo(ruta);
				file.MoveTo(ruta_destino);
				return true;
			}
			else
				return false;
		}


		/// <summary>
		/// borra un archivo existente
		/// </summary>
		public static bool Borrar(string ruta)
		{
			if (ValidarExistencia(ruta))
			{
				//borra el archivo indicado
				FileInfo file = new FileInfo(ruta);
				file.Delete();
				return true;
			}
			else
				return false;
		}


		/// <summary>
		/// Obtiene un archivo de una URL http
		/// </summary>
		/// <param name="url">ruta publica</param>
		/// <returns></returns>
		public static ArchivoUrl Obtener(string url)
		{
			ArchivoUrl archivo = new ArchivoUrl();

			Stream stream = null;
			try
			{
				WebProxy myProxy = new WebProxy();
				HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

				HttpWebResponse response = (HttpWebResponse)req.GetResponse();
				stream = response.GetResponseStream();

				stream.Close();

				archivo.archivo = stream;
				archivo.contenido = new ContentType(response.ContentType);

				archivo.name = response.ResponseUri.Segments.LastOrDefault();
			}
			catch (Exception exp)
			{
				archivo = null;
			}

			return archivo;

		}


	}
}
