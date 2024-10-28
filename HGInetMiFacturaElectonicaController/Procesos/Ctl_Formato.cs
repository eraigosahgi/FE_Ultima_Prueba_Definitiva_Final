﻿using HGInetMiFacturaElectonicaController.Auditorias;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using LibreriaGlobalHGInet.RegistroLog;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="archivo"></param>
        /// <param name="documento"></param>
        /// <returns></returns>
        public static string GuardarArchivo(string archivo, FacturaE_Documento documento)
        {
            try
            {
                string nombre_pdf = string.Empty;

                if (string.IsNullOrWhiteSpace(archivo))
                    throw new ApplicationException(string.Format("No se encontró información en el archivo. {0}", archivo));
					
				PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

				// ruta física del xml
				string carpeta_pdf = string.Format("{0}\\{1}\\{2}", plataforma_datos.RutaDmsFisica, Constantes.CarpetaFacturaElectronica, documento.IdSeguridadTercero.ToString());
				carpeta_pdf = string.Format(@"{0}\{1}", carpeta_pdf, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian);

				// valida la existencia de la carpeta
				carpeta_pdf = Directorio.CrearDirectorio(carpeta_pdf);

                // ruta del pdf
                string ruta_pdf = string.Format(@"{0}\{1}.pdf", carpeta_pdf, documento.NombreXml);

                //convierte el array de byte en archivo pdf
                File.WriteAllBytes(ruta_pdf, Convert.FromBase64String(archivo));

                nombre_pdf = documento.NombreXml;

                return nombre_pdf;

            }
            catch (Exception excepcion)
            {
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Archivos, MensajeTipo.Error, MensajeAccion.creacion);
				throw excepcion;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="archivo"></param>
        /// <param name="directorio"></param>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public static string GuardarArchivo(Formato archivo, string directorio, string nombre)
        {
            try
            {
                string nombre_pdf = string.Empty;

                if (archivo == null)
                    throw new ApplicationException(string.Format("No se encontró información en el archivo."));

                if (string.IsNullOrWhiteSpace(archivo.ArchivoPdf))
                    throw new ApplicationException(string.Format("No se encontró información en el archivo. {0}", archivo));
					
				PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

				// ruta física del xml
				string carpeta_pdf = string.Format("{0}\\{1}\\{2}", plataforma_datos.RutaDmsFisica, Constantes.CarpetaFacturaElectronica, directorio);
				carpeta_pdf = string.Format(@"{0}\{1}", carpeta_pdf, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian);

				// valida la existencia de la carpeta
				carpeta_pdf = Directorio.CrearDirectorio(carpeta_pdf);

                // ruta del pdf
                string ruta_pdf = string.Format(@"{0}\{1}.pdf", carpeta_pdf, nombre);

                //convierte el array de byte en archivo pdf
                File.WriteAllBytes(ruta_pdf, Convert.FromBase64String(archivo.ArchivoPdf));
				
                // url pública del pdf
				string url_ppal_pdf = string.Format("{0}/{1}/{2}", plataforma_datos.RutaDmsPublica, Constantes.CarpetaFacturaElectronica, directorio);

				nombre_pdf = string.Format(@"{0}{1}/{2}.pdf", url_ppal_pdf, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian, nombre);

                return nombre_pdf;

            }
            catch (Exception excepcion)
            {
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Archivos, MensajeTipo.Error, MensajeAccion.creacion);
				throw excepcion;
            }



        }
    }
}
