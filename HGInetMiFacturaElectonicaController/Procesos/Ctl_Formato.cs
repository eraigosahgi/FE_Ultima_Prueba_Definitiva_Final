using HGInetMiFacturaElectonicaData.Modelo;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController.Procesos
{
	public class Ctl_Formato
	{

		public static string GuardarArchivo(string archivo, TblEmpresas empresa, FacturaE_Documento documento)
		{
			try
			{
				string nombre_pdf = string.Empty;

				if (string.IsNullOrWhiteSpace(archivo))
					throw new ApplicationException(string.Format("No se encontró información en el archivo. {0}", archivo));
					
				string carpeta_pdf = LibreriaGlobalHGInet.Dms.ObtenerCarpetaPrincipal(Directorio.ObtenerDirectorioRaiz(), empresa.StrIdentificacion);
				carpeta_pdf = string.Format(@"{0}{1}", carpeta_pdf, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian);

				// valida la existencia de la carpeta
				carpeta_pdf = Directorio.CrearDirectorio(carpeta_pdf);

				// ruta del pdf
				string ruta_pdf = string.Format(@"{0}{1}.pdf", carpeta_pdf, documento.NombreXml);

				//convierte el array de byte en archivo pdf
				File.WriteAllBytes(ruta_pdf, Convert.FromBase64String(archivo));

				nombre_pdf = documento.NombreXml;

				return nombre_pdf;

			}
			catch (Exception exec)
			{
				throw new ApplicationException(exec.Message);
			}



		}
	}
}
