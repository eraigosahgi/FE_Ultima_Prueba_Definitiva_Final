using LibreriaGlobalHGInet.RegistroLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMonitorServController.Utilitarios
{
    public class Ctl_Utilidades
	{

		/// <summary>
		/// Obtiene el valor del archivo de configuración en AppSettings
		/// ConfigurationManager.AppSettings["ParametroNombre"].ToString();
		/// </summary>
		/// <param name="parametro_nombre">nombre del parámetro</param>
		/// <returns>calor del parámetro indicado</returns>
		public static string ObtenerAppSettings(string parametro_nombre)
		{
			string parametro_valor = string.Empty;

			try
			{
				parametro_valor = ConfigurationManager.AppSettings[parametro_nombre].ToString();
			}
			catch (Exception excepcion)
			{
				//throw new ExcepcionHgi(excepcion, NotificacionCodigo.ERROR_EN_SERVIDOR, string.Format("Error al obtener el parámetro '{0}'.", parametro_nombre));
				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Certificado, MensajeTipo.Error, MensajeAccion.lectura, string.Format("Error al obtener el parámetro '{0}'.", parametro_nombre));
				throw new ApplicationException(string.Format("Error al obtener el parámetro '{0}'.", parametro_nombre));
			}

			return parametro_valor;
		}

	}
}
