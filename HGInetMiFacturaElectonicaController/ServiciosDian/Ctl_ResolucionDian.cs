using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HGInetDIANServicios;
using HGInetDIANServicios.DianResolucion;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;

namespace HGInetMiFacturaElectonicaController.ServiciosDian
{
	public class Ctl_ResolucionDian
	{
		public static string Obtener(Guid id_peticion, string id_software, string clave, string nit_empresa, string nit_proveedor)
		{
			try
			{
				// Producción HGI SAS
				ResolucionesFacturacion resoluciones_produccion = Ctl_Resolucion.Obtener(id_peticion, id_software, clave, nit_empresa, nit_proveedor, Fecha.GetFecha());

				string carpeta = Directorio.CrearDirectorio(AppDomain.CurrentDomain.BaseDirectory + @"_LogFacturaE\");

				string archivo = carpeta + id_peticion.ToString() + ".xml";

				string xml_doc = System.IO.File.ReadAllText(archivo);
				
				return xml_doc;

			}
			catch (Exception exc)
			{
				throw exc;
			}


		}

	}
}
