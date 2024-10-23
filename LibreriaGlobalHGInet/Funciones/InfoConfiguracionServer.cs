using LibreriaGlobalHGInet.Formato;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibreriaGlobalHGInet.Funciones
{
	public class InfoFramework
	{
		/// <summary>
		/// Información del framework
		/// </summary>
		public string Codigo { get; set; }
		public string Nombre { get; set; }
		public string IndicadorInstalacion { get; set; }
	}

	/// <summary>
	/// Información del sistema operativo
	/// </summary>
	public class InfoOS
	{
		public string Version { get; set; }
		public string Descripción { get; set; }
	}

	public class InfoConfiguracionServer
	{

		/// <summary>
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

	}
}
