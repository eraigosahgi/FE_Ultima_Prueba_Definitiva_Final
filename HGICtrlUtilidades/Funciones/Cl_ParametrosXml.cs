using HGICtrlUtilidades.Recursos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HGICtrlUtilidades.Funciones
{
	public class Cl_ParametrosXml
	{
		public class ParametroRegistro
		{
			public string Nombre { get; set; }
			public string Valor { get; set; }
			public string Descripcion { get; set; }
		}

		[XmlRoot(ElementName = "Configuracion")]
		public class ParametroEstructura
		{
			[XmlArrayItem(ElementName = "Registro")]
			private List<ParametroRegistro> registros;

			public List<ParametroRegistro> Registros
			{
				get
				{
					if (registros == null)
						registros = new List<ParametroRegistro>();
					return registros;
				}
				set
				{
					registros = value;
				}
			}
		}

		private static readonly string directorio_archivos = string.Format("{0}\\configuracion", RecursoHGIServiciosWeb.DirectorioArchivosApp);

		/// <summary>
		/// Obtiene todos los parámetros del archivo de configuración
		/// </summary>
		/// <returns type="ParametroEstructura">datos del archivo de configuración</returns>
		public static ParametroEstructura ObtenerParametroEstructura(string nombre_archivo)
		{
			string ruta_xml = "";

			try
			{
				ruta_xml = string.Format("{0}\\{1}", Cl_Directorio.CrearDirectorio(Cl_Directorio.ObtenerDirectorioRaiz() + directorio_archivos), nombre_archivo);

				return Cl_Xml.ObtenerObjetoXml<ParametroEstructura>(ruta_xml);
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
				ruta_xml = string.Format("{0}\\{1}", Cl_Directorio.CrearDirectorio(Cl_Directorio.ObtenerDirectorioRaiz() + directorio_archivos), nombre_archivo);

				ParametroEstructura xml_configuracion = Cl_Xml.ObtenerObjetoXml<ParametroEstructura>(ruta_xml);

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


		/// <summary>
		/// ruta donde se encuentra del archivo .dat
		/// </summary>
		public static string GetRutaDat()
		{
			return ObtenerValorRegistroXml("ruta_dat", RecursoArchivosParametros.Configuracion);
		}

		public static string GetCodigoCia()
		{
			return ObtenerValorRegistroXml("codigo_cia", RecursoArchivosParametros.Configuracion);
		}

		/// <summary>
		/// conexión de base de datos
		/// Data Source=XXX;Initial Catalog=XXX;User ID=XXX;Password=XXX;Connect Timeout=600;Persist Security Info=True;
		/// </summary>
		public static string GetLicenciaConexionBD()
		{
			return ObtenerValorRegistroXml("conexion_licencias_bd", RecursoArchivosParametros.Configuracion);
		}

		public static string GetFacturaEConexionBD()
		{
			return ObtenerValorRegistroXml("conexion_facturaE_bd", RecursoArchivosParametros.Configuracion);
		}

		/// <summary>
		/// obtiene la ruta física del aplicativo
		/// </summary>
		public static string RutaServidor = GetRutaServidor();

		/// <summary>
		/// obtiene la ruta física del aplicativo
		/// </summary>
		/// <returns></returns>
		private static string GetRutaServidor()
		{
			return AppDomain.CurrentDomain.BaseDirectory;
		}
	}
}
