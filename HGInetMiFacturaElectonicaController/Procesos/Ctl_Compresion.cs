using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;

namespace HGInetMiFacturaElectonicaController.Procesos
{
	public class Ctl_Compresion
	{

		public static void Comprimir(FacturaE_Documento documento)
		{
			string ruta_xml = string.Format("{0}{1}.xml", Directorio.ObtenerDirectorioRaiz(), documento.NombreXml);

			string ruta_zip = string.Format("{0}{1}.zip", Directorio.ObtenerDirectorioRaiz(), documento.NombreZip);


			using (ZipArchive archive = ZipFile.Open(ruta_zip, ZipArchiveMode.Update))
			{
				archive.CreateEntryFromFile(ruta_xml, Path.GetFileName(ruta_xml));
			}
		}

	}
}
