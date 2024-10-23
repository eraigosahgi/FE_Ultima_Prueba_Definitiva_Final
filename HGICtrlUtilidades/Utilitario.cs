using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public static class Utilitario
	{
		public static void Almacenar(string mensaje, string nit, string nombre = "happgi")
		{
			StreamWriter sw = null;

			try
			{
				// obtiene la ruta del archivo de auditoria
				string ruta_log = string.Format(@"{0}\logs\{1}\", Cl_Directorio.ObtenerDirectorioRaiz(), nombre);

				// asegura la existencia del archivo
				Cl_Directorio.CrearDirectorio(ruta_log);

				// ruta completa del archivo de auditoria
				ruta_log = string.Format("{0}{1}_{2}.txt", ruta_log, nit, Cl_Fecha.GetFecha().ToString(Cl_Fecha.formato_fecha_hginet));

				// asegura la creación del archivo de auditoría
				Cl_Archivo.Crear(ruta_log);

				// valida la existencia del archivo
				if (!Cl_Archivo.ValidarExistencia(ruta_log))
					throw new ApplicationException("Error al obtener la ruta del archivo de auditoria.");

				sw = new StreamWriter(ruta_log, true);

				sw.WriteLine(string.Format("{0}", mensaje));
				// sw.Flush(); para borrar lo que ya esta escrito
				sw.Close();

			}
			catch (Exception )
			{
			}
		}
	}
}
