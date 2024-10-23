using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibreriaGlobalHGInet.General
{
	public class WebHtml
	{
		/// <summary>
		/// Obtiene el tipo de contenido de acuerdo con la extensión
		/// </summary>
		/// <param name="extension">extesión del archivo (.ext)</param>
		/// <returns>tipo de contenido</returns>
		public static string TipoContenido(string extension)
		{
			string tipo = string.Empty;

			// imagen
			if (extension == ".png" || extension == ".jpg" || extension == ".jpeg")
				tipo = string.Format("image/{0}", (object)extension.Replace(".", ""));
			// documento pdf
			else if (extension == ".pdf")
				tipo = "application/pdf";
			// documento xml
			else if (extension == ".xml")
				tipo = "application/xml";
			// archivo zip
			else if (extension == ".zip")
				tipo = "application/zip";
			// archivo json
			else if (extension == ".json")
				tipo = "application/json";
			// archivo desconocido o binario
			else
				tipo = "application/octet-stream";

			return tipo;
		}

	}
}
