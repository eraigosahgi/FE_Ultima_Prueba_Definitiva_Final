using HGInetFacturaEReports.Facturas;
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

                Formato formato_documento = (Formato) (dynamic)documento_obj.DocumentoFormato;
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
                    }
                }
				else
				{
					Formato1 reporte_pdf = new Formato1();

					//XmlTextReader xml = new XmlTextReader(documentoBd.StrUrlArchivoUbl);

					FileStream xml = new FileStream(string.Format("{0}{1}.xml", documento_result.RutaArchivosProceso,documento_result.NombreXml), FileMode.Open);

					XmlSerializer serializacion = new XmlSerializer(typeof(InvoiceType));

					InvoiceType conversion = (InvoiceType)serializacion.Deserialize(xml);

					Factura datos_documento = FacturaXML.Convertir(conversion);

					reporte_pdf.DataSource = datos_documento;

					xml.Close();


					string url_ppal_pdf = LibreriaGlobalHGInet.Dms.ObtenerUrlPrincipal("", documento_result.IdSeguridadTercero.ToString());

					string ruta = string.Format(@"{0}{1}/", url_ppal_pdf, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian);

					HGInetFacturaEReports.Reporte x = new HGInetFacturaEReports.Reporte(documento_result.NombreXml, documento_result.RutaArchivosEnvio);
					x.GenerarPdf(reporte_pdf);

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
