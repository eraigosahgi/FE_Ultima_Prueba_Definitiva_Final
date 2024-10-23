using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public class Cl_JsonLog
	{
		/// <summary>
		/// Obtiene los datos de un archivo XML
		/// </summary>
		/// <typeparam name="T" type="Object">objeto</typeparam>
		/// <param name="ruta" type="string">ruta física del archivo XML</param>
		/// <returnstype="Object">objeto</returns>
		public static LogArchivo ObtenerLog(string ruta)
		{
			LogArchivo archivo = new LogArchivo();

			StreamReader file = null;
			string ruta_directorio = AppDomain.CurrentDomain.BaseDirectory;

			if (!ruta.Contains(ruta_directorio))
				ruta = string.Format("{0}{1}", ruta_directorio, ruta);

			if (Cl_Archivo.ValidarExistencia(ruta))
			{
				try
				{

					// lee el archivo Json de Log
					using (file = File.OpenText(ruta))
					using (JsonTextReader reader = new JsonTextReader(file))
					{
						JObject o2 = (JObject)JToken.ReadFrom(reader);

						JArray a = (JArray)o2["Logs"];

						List<LogClase> logs = a.ToObject<List<LogClase>>();

						archivo.Logs = logs;
					}
				}
				catch (Exception exec)
				{
					throw new ApplicationException(string.Format("Error al obtener los datos de {0} - Detalle: {1}", ruta, exec));
				}
				finally
				{
					if (file != null)
						file.Close();
				}

				return archivo;
			}
			else
				throw new ArgumentException(string.Format("La ruta {0} no existe o es inválida", ruta));
		}


		/// <summary>
		/// Guarda los datos del objeto enviado en un JSON en la ruta específica
		/// </summary>
		/// <typeparam name="T" type="Object">objeto</typeparam>
		/// <param name="objeto" type="Object">objeto</param>
		/// <param name="ruta_directorio" type="string">ruta física del archivo JSON</param>
		/// <param name="archivo_json" type="string">nombre archivo JSON</param>
		public static void GuardarObjetoJson<T>(T objeto, string ruta_directorio, string archivo_json)
		{
			try
			{
				ruta_directorio = AppDomain.CurrentDomain.BaseDirectory + ruta_directorio;

				if (!Cl_Directorio.ValidarExistencia(ruta_directorio))
					Cl_Directorio.CrearDirectorio(ruta_directorio);

				string ruta_xml = string.Format("{0}\\{1}", ruta_directorio, archivo_json);

				System.IO.File.WriteAllText(ruta_xml, Newtonsoft.Json.JsonConvert.SerializeObject(objeto));
			}
			catch (Exception exec)
			{
				throw new ApplicationException(string.Format("Error al guardar el objeto {0} : {1}", typeof(T).Name, exec));
			}
		}
	}
}
