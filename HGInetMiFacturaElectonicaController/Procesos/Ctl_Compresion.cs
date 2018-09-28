using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;

namespace HGInetMiFacturaElectonicaController.Procesos
{
    /// <summary>
    /// Controlador para la compresión de archivos
    /// </summary>
    public class Ctl_Compresion
    {
        /// <summary>
        /// Comprime el archivo XML en ZIP
        /// </summary>
        /// <param name="documento">datos del documento procesado</param>
        public static string Comprimir(FacturaE_Documento documento)
        {
            string nit_obligado = string.Empty;

            switch (documento.DocumentoTipo)
            {
                case TipoDocumento.Factura:
                    Factura doc_factura = ((Factura)documento.Documento);
                    nit_obligado = doc_factura.DatosObligado.Identificacion;
                    break;
                case TipoDocumento.NotaCredito:
                    NotaCredito doc_nota_credito = ((NotaCredito)documento.Documento);
                    nit_obligado = doc_nota_credito.DatosObligado.Identificacion;
                    break;
                case TipoDocumento.NotaDebito:
                    NotaDebito doc_nota_debito = ((NotaDebito)documento.Documento);
                    nit_obligado = doc_nota_debito.DatosObligado.Identificacion;
                    break;
                default:
                    break;
            }

            // ruta del xml
            string ruta_xml = string.Format(@"{0}\{1}.xml", documento.RutaArchivosEnvio, documento.NombreXml);

            // valida la existencia de la carpeta
            documento.RutaArchivosEnvio = Directorio.CrearDirectorio(documento.RutaArchivosEnvio);

            // ruta del zip
            string ruta_zip = string.Format(@"{0}\{1}.zip", documento.RutaArchivosEnvio, documento.NombreZip);

            // elimina el archivo zip si existe
            if (Archivo.ValidarExistencia(ruta_zip))
                Archivo.Borrar(ruta_zip);

            // genera la compresión del archivo en zip
            using (ZipArchive archive = ZipFile.Open(ruta_zip, ZipArchiveMode.Update))
            {
                archive.CreateEntryFromFile(ruta_xml, Path.GetFileName(ruta_xml));
            }

            return documento.NombreZip;
        }

    }
}
