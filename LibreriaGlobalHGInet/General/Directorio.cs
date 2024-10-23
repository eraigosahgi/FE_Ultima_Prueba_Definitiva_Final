using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace LibreriaGlobalHGInet.General
{
    public class Directorio
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
        public static bool ValidarExistenciaArchivo(string ruta_archivo)
        {
            bool existe = false;

            if (Directory.Exists(ruta_archivo))
                existe = true;

            return existe;
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

        /// <summary>
		/// Valida la ruta del servicio web
		/// </summary>
		/// <param name="rutaUrl">ruta del servicio web remoto</param>
		/// <returns>ruta del servicio web</returns>
		public static string ValidarUrl(string rutaUrl)
        {
            if (string.IsNullOrEmpty(rutaUrl))
                throw new Exception("Ruta remota de servicios web no encontrada.");
            var ruta = rutaUrl[rutaUrl.Length - 1].ToString();
            if (!ruta.Equals("/"))
                rutaUrl = string.Format("{0}/", rutaUrl);

            return rutaUrl;
        }


        /// <summary>
        /// busca los subdirectorios de un directorio con la posibilidad de enviar un patrón de bísqueda
        /// </summary>
        /// <param name="ruta">ruta física</param>
        /// <returns>archivos del directorio de acuerdo con el patrón enviado</returns>
        public static string [] ObtenerSubdirectoriosDirectorio(string ruta)
        {
	        try
	        {
				return System.IO.Directory.GetDirectories(ruta);
	        }
	        catch (Exception exec)
	        {
		        throw new ApplicationException(exec.Message);
	        }
        }

        /// <summary>
        /// borra el directorio existente
        /// </summary>
        /// <returns type="string">ruta física completa</returns>
        public static bool BorrarDirectorio(string ruta)
        {
	        if (Directory.Exists(ruta))
	        {
				try
				{
					Directory.Delete(ruta);
				}
				catch (Exception exec)
				{
					throw new ApplicationException(exec.Message);
				}

				return true;
	        }
	        else
		        return false;
        }

        /// <summary>
        /// Mueve en un directorio y su contenido
        /// </summary>
        /// <returns type="string">ruta física completa</returns>
        public static bool MoverDirectorio(string ruta, string ruta_destino)
        {
	        if (Directory.Exists(ruta))
	        {
		        try
		        {
			        Directory.Move(ruta, ruta_destino);
		        }
		        catch (Exception)
		        {
			        return false;
				}

				return true;
	        }
	        else
		        return false;
        }

	}
}