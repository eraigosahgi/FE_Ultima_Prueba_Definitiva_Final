using HGICtrlUtilidades;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{

	public class Cl_InfoConfiguracionServer
	{       /// <summary>
			/// Obtiene los framework instalados en la máquina.
			/// </summary>
			/// <returns></returns>
		public static List<InfoFramework> Frameworks()
		{
			const string subkey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\";
			List<InfoFramework> version_framework = new List<InfoFramework>();

			using (var ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subkey))
			{
				//Coleccion.ConvertArrayToString(ndpKey.GetSubKeyNames());

				InfoFramework obj_frm = new InfoFramework();

				foreach (var versionKeyName in ndpKey.GetSubKeyNames())
				{
					RegistryKey versionKey = ndpKey.OpenSubKey(versionKeyName);

					//Obtiene el valor de versión de .NET Framework.
					var name = (string)versionKey.GetValue("Version", "");
					// indicador de instalación
					string install = versionKey.GetValue("Install", "").ToString();

					if (!string.IsNullOrEmpty(name))
					{
						obj_frm = new InfoFramework();
						obj_frm.Codigo = versionKeyName;
						obj_frm.Nombre = name;
						obj_frm.IndicadorInstalacion = install;
						version_framework.Add(obj_frm);
						continue;
					}

					foreach (var subKeyName in versionKey.GetSubKeyNames())
					{
						RegistryKey subKey = versionKey.OpenSubKey(subKeyName);
						name = (string)subKey.GetValue("Version", "");
						// indicador de instalación
						install = subKey.GetValue("Install", "").ToString();

						if (!string.IsNullOrEmpty(name))
						{
							obj_frm = new InfoFramework();
							obj_frm.Codigo = versionKeyName;
							obj_frm.Nombre = name;
							obj_frm.IndicadorInstalacion = install;
							version_framework.Add(obj_frm);
							continue;
						}
					}


					if (string.IsNullOrEmpty(name))
					{
						obj_frm = new InfoFramework();
						obj_frm.Codigo = versionKeyName;
						version_framework.Add(obj_frm);
					}
				}
			}

			return version_framework;
		}

		/// <summary>
		/// Obtiene el sistema operativo de la máquina
		/// </summary>
		/// <returns></returns>
		public static InfoOS GetOsInfo()
		{
			OperatingSystem info_os = System.Environment.OSVersion;
			InfoOS info = new InfoOS();

			if (info_os != null)
			{
				info.Version = string.Format("{0}.{1}.{2}", info_os.Version.Major, info_os.Version.Minor, info_os.Version.Build);
				info.Descripción = info_os.VersionString;
			}

			return info;
		}


		private static readonly object _lock = new object();

		public static string ObtenerAppSettings2(string parametro_nombre)
		{
			try
			{
				lock (_lock)
				{
					return ConfigurationManager.AppSettings[parametro_nombre];
				}
			}
			catch (Exception excepcion)
			{
				Utilitario.Almacenar("Error ObtenerAppSettings: " + excepcion.Message, parametro_nombre);
				throw new ApplicationException(string.Format("Error al obtener el parámetro '{0}' - {1}", parametro_nombre, excepcion.Message), excepcion.InnerException);
			}			
		}

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
                Utilitario.Almacenar("ObtenerAppSettings parametro_valor: " + parametro_valor, parametro_nombre);

            }
            catch (Exception excepcion)
			{
                Utilitario.Almacenar("Error ObtenerAppSettings: " + excepcion.Message, parametro_nombre);
				//throw new ExcepcionHgi(excepcion, NotificacionCodigo.ERROR_EN_SERVIDOR, string.Format("Error al obtener el parámetro '{0}'.", parametro_nombre));
				throw new ApplicationException(string.Format("Error al obtener el parámetro '{0}' - {1}", parametro_nombre, excepcion.Message), excepcion.InnerException);
			}

			return parametro_valor;
		}

	}
}
