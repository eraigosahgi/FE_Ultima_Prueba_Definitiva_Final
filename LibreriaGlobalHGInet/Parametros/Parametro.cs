using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibreriaGlobalHGInet.Parametros
{
	public class Parametro
	{
		
		/// <summary>
		/// ruta donde se encuentra del archivo .dat
		/// </summary>
		public static string GetRutaDat()
		{	return ParametroXml.ObtenerValorRegistroXml("ruta_dat", LibreriaGlobalHGInet.Properties.RecursoArchivosParametros.Configuracion);
		}

		/// <summary>
		/// conexión de base de datos
		/// Data Source=XXX;Initial Catalog=XXX;User ID=XXX;Password=XXX;Connect Timeout=600;Persist Security Info=True;
		/// </summary>
		public static string GetLicenciaConexionBD()
		{
			return ParametroXml.ObtenerValorRegistroXml("conexion_licencias_bd", LibreriaGlobalHGInet.Properties.RecursoArchivosParametros.Configuracion);
		}

		public static string GetFacturaEConexionBD()
		{
			return ParametroXml.ObtenerValorRegistroXml("conexion_facturaE_bd", LibreriaGlobalHGInet.Properties.RecursoArchivosParametros.Configuracion);
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
