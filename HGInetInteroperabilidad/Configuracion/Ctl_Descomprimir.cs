using HGInetInteroperabilidad.Objetos;
using HGInetInteroperabilidad.Procesos;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.Modelo;
using LibreriaGlobalHGInet.Funciones;
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
    public class Ctl_Descomprimir
    {

        public static RegistroListaDocRespuesta Procesar(RegistroListaDoc datos, string proveedor_emisor)
        {

            RegistroListaDocRespuesta datos_respuesta = new RegistroListaDocRespuesta();

            if (datos == null)
            {
                datos_respuesta.timeStamp = Fecha.GetFecha();
                datos_respuesta.trackingIds = null;
                datos_respuesta.mensajeGlobal = string.Format("No se obtuvo informacion de {0} {1}", datos.nombre, Enumeracion.GetDescription(RespuestaInterOperabilidad.Zipvacio));

            }

            //Obtengo el proveedor emisor
            TblConfiguracionInteroperabilidad proveedor = new TblConfiguracionInteroperabilidad();

            Ctl_ConfiguracionInteroperabilidad configuracion = new Ctl_ConfiguracionInteroperabilidad();

            proveedor = configuracion.Obtener(proveedor_emisor);

            string destino = string.Empty;

            //Obtengo archivos para procesar
            if (!Directorio.ValidarExistenciaArchivoUrl(string.Format("{0}/{1}", proveedor.StrUrlFtp, datos.nombre)))
            {
                datos_respuesta.timeStamp = Fecha.GetFecha();
                datos_respuesta.trackingIds = null;
                datos_respuesta.mensajeGlobal = string.Format("No se obtuvo informacion de {0} {1}", datos.nombre, Enumeracion.GetDescription(RespuestaInterOperabilidad.Zipvacio));

            }
            else
            {
                PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

                string ruta_fisica_zip = string.Format("{0}\\{1}{2}\\{3}", plataforma_datos.RutaDmsFisica, Constantes.RutaInteroperabilidadFtp, proveedor.StrIdSeguridad, datos.nombre);
                
                string ruta_archivos = string.Format("{0}\\{1}\\{2}", plataforma_datos.RutaDmsFisica, Constantes.RutaInteroperabilidadRecepcion, proveedor.StrIdSeguridad);

                Directorio.CrearDirectorio(ruta_archivos);


                try
                {
                    // Ingreso a la Carpeta para validar informacion
                    using (ZipArchive file = ZipFile.OpenRead(ruta_fisica_zip))
                    {
                        if (file.Entries.Count > 100)
                        {
                            datos_respuesta.timeStamp = Fecha.GetFecha();
                            datos_respuesta.trackingIds = null;
                            datos_respuesta.mensajeGlobal = string.Format("No se obtuvo informacion de {0} {1}", datos.nombre, Enumeracion.GetDescription(RespuestaInterOperabilidad.ZipSuperaMaximo));

                        }
                        else
                        {
                            //Recorro la carpeta
                            foreach (ZipArchiveEntry archivo in file.Entries)
                            {
                                //Valida que el archivo no tenga ninguna de estas extensiones
                                if ((archivo.FullName.EndsWith(".exe", StringComparison.OrdinalIgnoreCase)) || (archivo.FullName.EndsWith(".bat", StringComparison.OrdinalIgnoreCase))
                                    || (archivo.FullName.EndsWith(".accdb", StringComparison.OrdinalIgnoreCase)) || (archivo.FullName.EndsWith(".mdb", StringComparison.OrdinalIgnoreCase)))
                                {
                                    archivo.Delete();
                                }
                            }
                            // Obtiene la ruta completa para garantizar que se eliminen los segmentos relativos.
                            destino = Path.GetFullPath(Path.Combine(ruta_archivos, Path.GetFileNameWithoutExtension(datos.nombre)));

                            // genera la descompresión del archivo zip
                            file.ExtractToDirectory(destino);
                        }
                        file.Dispose();
                    }

                    //Envia la informacion para procesarla
                    datos_respuesta = Ctl_Recepcion.Procesar(datos, destino, proveedor.StrIdentificacion);
                }
                catch (Exception excepcion)
                {
                    LogExcepcion.Guardar(excepcion);
                    datos_respuesta.timeStamp = Fecha.GetFecha();
                    datos_respuesta.trackingIds = null;
                    datos_respuesta.mensajeGlobal = string.Format("Error al descomprimir {0} {1}", datos.nombre, Enumeracion.GetDescription(RespuestaInterOperabilidad.ErrorInternoReceptor));
                    throw new ApplicationException(string.Format("Error al descomprimir {0} Detalle: {1}", datos.nombre, excepcion.Message));
                }
            }
            return datos_respuesta;
        }


    }
}
