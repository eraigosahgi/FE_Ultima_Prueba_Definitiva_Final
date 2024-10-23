using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public class Cl_Directorio
	{

		/// <summary>
		/// Obtiene el directorio raíz donde se encuentra el aplicativo instalado
		/// </summary>
		/// <returns type="string">ruta física completa</returns>
		public static string ObtenerDirectorioRaiz()
		{
			return AppDomain.CurrentDomain.BaseDirectory;
		}

		/// <summary>
		/// busca los archivos de un directorio con la posibilidad de enviar un patrón de bísqueda
		/// </summary>
		/// <param name="ruta">ruta física</param>
		/// <param name="patron_busqueda">patrón por ej: *.* o *.png</param>
		/// <returns>archivos del directorio de acuerdo con el patrón enviado</returns>
		public static IEnumerable<FileInfo> ObtenerArchivosDirectorio(string ruta, string patron_busqueda = "")
		{
			try
			{
				if (string.IsNullOrWhiteSpace(patron_busqueda))
					patron_busqueda = "*";

				return new DirectoryInfo(ruta).EnumerateFiles(patron_busqueda, SearchOption.TopDirectoryOnly);
			}
			catch (Exception exec)
			{
				throw new ApplicationException(exec.Message);
			}
		}

		/// <summary>
		/// Crea el directorio de la ruta física indicada
		/// </summary>
		/// <param name="ruta" type="string">ruta física completa de la carpeta</param>
		/// <returns type="string">ruta física completa</returns>
		public static string CrearDirectorio(string ruta)
		{
			if (!ruta.EndsWith(@"\"))
				ruta = string.Format(@"{0}\", ruta);

			if (!Directory.Exists(ruta))
				Directory.CreateDirectory(ruta);

			return ruta;
		}


		/// <summary>
		/// borra los archivos existente en un directorio
		/// </summary>
		/// <returns type="string">ruta física completa</returns>
		public static bool BorrarArchivos(string ruta, string patron_busqueda = "")
		{
			if (Directory.Exists(ruta))
			{
				List<FileInfo> archivos = ObtenerArchivosDirectorio(ruta, patron_busqueda).ToList();

				foreach (FileInfo item in archivos)
				{
					item.Delete();
				}

				return true;
			}
			else
				return false;
		}

		/// <summary>
		/// Indica si un archivo existe o no en la ruta física completa
		/// </summary>
		/// <param name="ruta_archivo" type="string">ruta física completa</param>
		/// <returns type="bool">indica si el archivo existe</returns>
		public static bool ValidarExistencia(string ruta_archivo)
		{
			bool existe = false;

			if (Directory.Exists(ruta_archivo))
				existe = true;

			return existe;
		}


	}
}
