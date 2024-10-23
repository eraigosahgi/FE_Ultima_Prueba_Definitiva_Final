using LibreriaGlobalHGInet.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibreriaGlobalHGInet.Parametros
{
	/// <summary>
	/// Clase que permite la lectura del archivo de configuración xml del aplicativo
	/// </summary>
	public class ParametroXml
	{
		private static readonly string directorio_archivos = string.Format("{0}\\configuracion", Properties.RecursoArchivosParametros.DirectorioArchivosApp);
				
		/// <summary>
		/// Obtiene todos los parámetros del archivo de configuración
		/// </summary>
		/// <returns type="ParametroEstructura">datos del archivo de configuración</returns>
		public static ParametroEstructura ObtenerParametroEstructura(string nombre_archivo)
		{	string ruta_xml = "";
			
			try
			{
				ruta_xml = string.Format("{0}\\{1}", Directorio.CrearDirectorio(Directorio.ObtenerDirectorioRaiz() + directorio_archivos), nombre_archivo);

				return Xml.ObtenerObjetoXml<ParametroEstructura>(ruta_xml);
			}
			catch (Exception exec)
			{
				throw new ApplicationException(string.Format("Error al obtener el xml de configuración {0}", ruta_xml), exec);
			}
		}

		/// <summary>
		/// Obtiene el parámetro del archivo de configuración con el nombre
		/// </summary>
		/// <param name="nombre" type="string">nombre del parámetro</param>
		/// <returns type="string">valor del parámetro</returns>
		public static string ObtenerValorRegistroXml(string nombre, string nombre_archivo)
		{
			string ruta_xml = "";

			try
			{
				ruta_xml = string.Format("{0}\\{1}", Directorio.CrearDirectorio(Directorio.ObtenerDirectorioRaiz() + directorio_archivos), nombre_archivo);

				ParametroEstructura xml_configuracion = Xml.ObtenerObjetoXml<ParametroEstructura>(ruta_xml);

				if (xml_configuracion != null)
				{
					ParametroRegistro registro_valida = xml_configuracion.Registros.Where(registro_ => registro_.Nombre.Trim().ToUpper().Equals(nombre.Trim().ToUpper())).FirstOrDefault();

					if (registro_valida != null)
						return registro_valida.Valor;
					else
						throw new ApplicationException(string.Format("El parámetro {0} no existe", nombre));
				}
				else
					throw new ApplicationException(string.Format("Error al obtener el parámetro {0} en el xml de configuración en la ruta {1}", nombre, ruta_xml));
			}
			catch (Exception exec)
			{
				throw new ApplicationException(string.Format("Error al obtener el parámetro {0}.", nombre), exec);
			}
		}
	}
}
