using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetUBL;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace HGInetMiFacturaElectonicaController.Procesos
{
    public partial class Ctl_Documentos
    {

        /// <summary>
        /// Guarda el formato PDF del documento
        /// </summary>
        /// <param name="documento_obj">información del documento</param>
        /// <param name="documentoBd">información del documento en base de datos</param>
        /// <param name="respuesta">datos de respuesta del documento</param>
        /// <param name="documento_result">información del proceso interno del documento</param>
        /// <returns>información adicional de respuesta del documento</returns>
        public static DocumentoRespuesta GuardarFormato(object documento, TblDocumentos documentoBd, ref DocumentoRespuesta respuesta, ref FacturaE_Documento documento_result)
        {
            try
            {
                var documento_obj = (dynamic)null;
                documento_obj = documento;

                bool generar_pdf = true;

                Formato formato_documento = (Formato)(dynamic)documento_obj.DocumentoFormato;
                if (formato_documento != null)
                {
                    if (!string.IsNullOrEmpty(formato_documento.ArchivoPdf))
                    {
                        // almacena archivo en base64
                        documento_result.NombrePdf = Ctl_Formato.GuardarArchivo(formato_documento.ArchivoPdf, documento_result);
                        respuesta.UrlPdf = string.Empty;

                        if (!string.IsNullOrWhiteSpace(documento_result.NombrePdf))
                        {
                            // url pública del pdf
                            string url_ppal_pdf = LibreriaGlobalHGInet.Dms.ObtenerUrlPrincipal("", documento_result.IdSeguridadTercero.ToString());

                            respuesta.UrlPdf = string.Format(@"{0}{1}/{2}.pdf", url_ppal_pdf, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian, documento_result.NombrePdf);

                            documentoBd.StrUrlArchivoPdf = respuesta.UrlPdf;

                            Ctl_Documento documento_tmp = new Ctl_Documento();
                            documentoBd = documento_tmp.Actualizar(documentoBd);
                        }

                        generar_pdf = false;
                    }
                }

                if (generar_pdf)
                {

                    string url_ppal_pdf = LibreriaGlobalHGInet.Dms.ObtenerUrlPrincipal("", documento_result.IdSeguridadTercero.ToString());
                    string ruta = string.Format(@"{0}{1}/", url_ppal_pdf, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian);

                    //Valida el tipo de documento para generar el formato PDF
                    if (documento_result.DocumentoTipo == TipoDocumento.Factura)
                    {
                        HGInetFacturaEReports.Facturas.Formato1 reporte_pdf = new HGInetFacturaEReports.Facturas.Formato1();
                        reporte_pdf.DataSource = documento;
                        HGInetFacturaEReports.Reporte x = new HGInetFacturaEReports.Reporte(documento_result.NombreXml, documento_result.RutaArchivosEnvio);
                        x.GenerarPdf(reporte_pdf);
                    }
                    else if (documento_result.DocumentoTipo == TipoDocumento.NotaCredito)
                    {
                        HGInetFacturaEReports.NotasCredito.Formato1 reporte_pdf = new HGInetFacturaEReports.NotasCredito.Formato1();
                        reporte_pdf.DataSource = documento;
                        HGInetFacturaEReports.Reporte x = new HGInetFacturaEReports.Reporte(documento_result.NombreXml, documento_result.RutaArchivosEnvio);
                        x.GenerarPdf(reporte_pdf);
                    }
                    else if (documento_result.DocumentoTipo == TipoDocumento.NotaDebito)
                    {

                    }

                    respuesta.UrlPdf = string.Format(@"{0}{1}/{2}.pdf", url_ppal_pdf, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian, documento_result.NombreXml);
                    documentoBd.StrUrlArchivoPdf = respuesta.UrlPdf;
                    Ctl_Documento documento_tmp = new Ctl_Documento();
                    documentoBd = documento_tmp.Actualizar(documentoBd);
                }



            }
            catch (Exception excepcion)
            {
                respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en el almacenamiento del documento PDF. Detalle: {0} ", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);
            }

            return respuesta;
        }

    }
}
