using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaData.Modelo;
using LibreriaGlobalHGInet.General;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetInteroperabilidad.Configuracion
{
    class ProcesarEnvioDoc
    {

        


        public class ArchivosAcomprimir
        {
            public string Nombre { get; set; }
        }




        /// <summary>
        /// Proceso para generar el archivo Zip que se debe enviar al proveedor receptor
        /// </summary>
        /// <param name="ruta_zip"></param>
        /// <param name="ruta_archivos"></param>
        /// <param name="NombreArchivo"></param>
        /// <returns></returns>
        public static string ComprimirLista(string ruta_zip, string ruta_archivos, string NombreArchivo)
        {



            try
            {
                // genera la compresión del archivo en zip
                ZipArchive archive = ZipFile.Open(ruta_zip, ZipArchiveMode.Update);
                //Aqui se debe iterear una lista que debe enviar el proceso anterior para poder ubicar los archivos que se van a comprimir
                for (int i = 1; i < 5; i++)
                {


                    string Arc = @"E:\Desarrollo\jflores\Proyectos\HGINetMiFacturaElectronica\Codigo\HGInetMiFacturaElectronicaWeb\dms\eb821fbe-02ba-4cfc-a7a9-248711513591\FacturaEDian\face_f0811021438003B0235A" + i + ".pdf";


                    string ss = Path.GetFileName(Arc);
                    // elimina el archivo zip si existe
                    //  if (!Archivo.ValidarExistencia(Arc))
                    //{
                    archive.CreateEntryFromFile(Arc, Path.GetFileName(Arc));
                    //}

                }

                archive.Dispose();


                return "";
            }
            catch (Exception excepcion)
            {
                return excepcion.ToString();

            }
        }
    }
}
