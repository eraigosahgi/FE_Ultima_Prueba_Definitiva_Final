using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades.Funciones
{
	public class Cl_Archivos
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
		/// Valida la descarga de los archivos.
		/// si no existe descarga el archivo.
		/// si existe, valida si actualiza (si actualiza reemplaza el archivo, sino existe retorna las url).
		/// </summary>
		/// <param name="ruta_url">ruta_url (Origen del archivo)</param>
		/// <param name="nit">nit</param>
		/// <param name="compania">compania</param>
		/// <param name="empresa">empresa</param>
		/// <param name="tipo">tipo de archivo</param>
		/// <param name="actualiza">actualiza</param>
		/// <returns></returns>
		public static List<string> ValidarDescargaArchivo(string ruta_url, string nit, string compania, short empresa, Cl_Dms.DmsTipo tipo, bool actualiza)
		{
			string ruta_fisica = "";

			try
			{
				List<string> rutas_retorno = new List<string>();
				string nombre = Path.GetFileName(ruta_url);

				ruta_fisica = Cl_Dms.ObtenerCarpeta("", nit, compania, empresa, tipo, true);

				if (!ruta_fisica.Substring(ruta_fisica.Length - 1, 1).Equals(@"\"))
					ruta_fisica = ruta_fisica + @"\";

				ruta_fisica = string.Format("{0}{1}", ruta_fisica, nombre);

				string ruta_virtual = string.Format("{0}/{1}", Cl_Dms.ObtenerUrl("", nit, compania, empresa, tipo), nombre);

				bool respuesta = false;

				if (!ValidarExistencia(ruta_fisica))
					respuesta = CopiarArchivo(ruta_url, ruta_fisica);
				else
				{
					if (actualiza)
						respuesta = CopiarArchivo(ruta_url, ruta_fisica);
					else
					{
						rutas_retorno.Add(ruta_fisica);
						rutas_retorno.Add(ruta_virtual);
						return rutas_retorno;
					}
				}

				if (respuesta)
				{
					rutas_retorno.Add(ruta_fisica);
					rutas_retorno.Add(ruta_virtual);
					return rutas_retorno;
				}
				else
					throw new Exception("No se descargó el archivo.");
			}
			catch (Exception exc)
			{
				throw new Exception(string.Format("El archivo {0} no fue procesado para almacenar en {1}. Error: {2}", ruta_url, ruta_fisica, exc.Message));

			}
		}

		/// <summary>
		/// Copia un arhchivo en directorio virtual a directorio fisico.
		/// </summary>
		/// <param name="ruta_origen"></param>
		/// <param name="ruta_destino"></param>
		/// <returns></returns>
		public static bool CopiarArchivo(string ruta_origen, string ruta_destino)
		{
			try
			{
				string directorio = Path.GetDirectoryName(ruta_destino);

				if (!Cl_Directorio.ValidarExistencia(directorio))
					Cl_Directorio.CrearDirectorio(directorio);

				WebClient wc = new WebClient();

				wc.DownloadFile(ruta_origen, ruta_destino);

				return true;
			}
			catch (Exception exc)
			{
				throw new ApplicationException(exc.Message, exc);
			}
		}

		/// <summary>
		/// Convierte una Url a base64
		/// </summary>
		/// <param name="url">ruta publica</param>
		/// <returns></returns>
		public static byte[] ObtenerWeb(string url)
		{
			Stream stream = null;
			byte[] buf;

			try
			{
				WebProxy myProxy = new WebProxy();
				HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

				HttpWebResponse response = (HttpWebResponse)req.GetResponse();
				stream = response.GetResponseStream();

				using (BinaryReader br = new BinaryReader(stream))
				{
					int len = (int)(response.ContentLength);
					buf = br.ReadBytes(len);
					br.Close();
				}

				stream.Close();
				response.Close();
			}
			catch (Exception exp)
			{
				buf = null;
			}

			return (buf);

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


		/// <summary>
		/// Obtiene un archivo de una URL http
		/// </summary>
		/// <param name="url">ruta publica</param>
		/// <returns>contenido del archivo en texto</returns>
		public static string ObtenerContenido(string url)
		{
			WebClient Client = null;
			StreamReader Reader = null;
			try
			{
				Client = new WebClient();
				Reader = new StreamReader(Client.OpenRead(url));
				string Contents = Reader.ReadToEnd();
				Reader.Close();
				return Contents;
			}
			catch
			{
				return "";
			}
			finally
			{
				if (Reader != null)
				{
					Reader.Close();
					Reader.Dispose();
				}
				if (Client != null)
				{
					Client.Dispose();
				}
			}
		}

		/// <summary>
		/// Valida la existencia de archivos virtuales.
		/// </summary>
		/// <param name="ruta"></param>
		/// <returns></returns>
		public static bool ValidarExistenciaArchivoUrl(string ruta)
		{
			try
			{
				var request = WebRequest.Create(new Uri(ruta));
				var response = request.GetResponse();
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

	}
}
