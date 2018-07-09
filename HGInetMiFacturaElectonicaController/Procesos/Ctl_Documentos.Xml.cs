using HGInetFirmaDigital;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController.Procesos
{
    public partial class Ctl_Documentos
    {
        /// <summary>
        /// Genera el xml con la información del documento en formato UBL
        /// </summary>
        /// <param name="documento_obj">información del documento</param>
        /// <param name="tipo_doc">tipo de documento</param>
        /// <param name="resolucion">información de la resolución</param>
        /// <param name="documentoBd">información del documento en base de datos</param>
        /// <param name="empresa">información del facturador electrónico en base de datos</param>
        /// <param name="respuesta">datos de respuesta del documento</param>
        /// <param name="documento_result">información del proceso interno del documento</param>
        /// <returns>información adicional de respuesta del documento</returns>
        public static DocumentoRespuesta UblGenerar(object documento_obj, TipoDocumento tipo_doc, TblEmpresasResoluciones resolucion, TblDocumentos documentoBd, TblEmpresas empresa, ref DocumentoRespuesta respuesta, ref FacturaE_Documento documento_result)
        {   
            try
            {
                respuesta.DescripcionProceso = "Genera información en estandar UBL.";
                respuesta.FechaUltimoProceso = Fecha.GetFecha();
                respuesta.IdProceso = ProcesoEstado.UBL.GetHashCode();
				
                //Genera Ubl
                if (tipo_doc == TipoDocumento.Factura)
                    documento_result = Ctl_Ubl.Generar(documento_result.IdSeguridadDocumento, (Factura)documento_obj, tipo_doc, empresa, resolucion);
                else if (tipo_doc == TipoDocumento.NotaCredito)
                    documento_result = Ctl_Ubl.Generar(documento_result.IdSeguridadDocumento, (NotaCredito)documento_obj, tipo_doc, empresa, resolucion);
				else if (tipo_doc == TipoDocumento.NotaDebito)
					documento_result = Ctl_Ubl.Generar(documento_result.IdSeguridadDocumento, (NotaDebito)documento_obj, tipo_doc, empresa, resolucion);

			}
			catch (Exception excepcion)
            {
                respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en la generación del estandar UBL del documento. Detalle: {0} ", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);
				LogExcepcion.Guardar(excepcion);
			}

            return respuesta;
        }

        /// <summary>
        /// Almacena físicamente la información del documento en xml en formato UBL
        /// </summary>
        /// <param name="documentoBd">información del documento en base de datos</param>
        /// <param name="respuesta">datos de respuesta del documento</param>
        /// <param name="documento_result">información del proceso interno del documento</param>
        /// <returns>información adicional de respuesta del documento</returns>
        public static DocumentoRespuesta UblGuardar(TblDocumentos documentoBd, ref DocumentoRespuesta respuesta, ref FacturaE_Documento documento_result)
        {
            try
            {
                respuesta.DescripcionProceso = "Almacena el archivo XML con la información en estandar UBL.";
                respuesta.FechaUltimoProceso = Fecha.GetFecha();
                respuesta.IdProceso = ProcesoEstado.AlmacenaXML.GetHashCode();

                // almacena el xml
                documento_result = Ctl_Ubl.Almacenar(documento_result);


				// url pública del xml
				string url_ppal = LibreriaGlobalHGInet.Dms.ObtenerUrlPrincipal("", documento_result.IdSeguridadTercero.ToString());
				respuesta.UrlXmlUbl = string.Format(@"{0}{1}/{2}.xml", url_ppal, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian, documento_result.NombreXml);

				//Actualiza Documento en Base de Datos
				documentoBd.DatFechaActualizaEstado = respuesta.FechaUltimoProceso;
                documentoBd.IntIdEstado = Convert.ToInt16(respuesta.IdProceso);

				documentoBd.StrUrlArchivoUbl = respuesta.UrlXmlUbl;
				documentoBd.StrEmpresaAdquiriente = respuesta.Identificacion;
				documentoBd.DatFechaActualizaEstado = respuesta.FechaUltimoProceso;
                documentoBd.StrCufe = documento_result.CUFE;

				//Ctl_Documento documento_tmp = new Ctl_Documento();
				//documento_tmp.Actualizar(documentoBd);
			}
            catch (Exception excepcion)
            {
                respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en el almacenamiento del documento UBL en XML. Detalle: {0} ", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);
				LogExcepcion.Guardar(excepcion);
			}
            return respuesta;
        }

        /// <summary>
        /// Realiza el proceso de firmado del documento XML en estandar UBL
        /// </summary>
        /// <param name="documentoBd">información del documento en base de datos</param>
        /// <param name="respuesta">datos de respuesta del documento</param>
        /// <param name="documento_result">información del proceso interno del documento</param>
        /// <returns>información adicional de respuesta del documento</returns>
        public static DocumentoRespuesta UblFirmar(TblDocumentos documentoBd, ref DocumentoRespuesta respuesta, ref FacturaE_Documento documento_result)
        {
            try
            {
                respuesta.DescripcionProceso = "Firma el archivo XML con la información en estandar UBL.";
                respuesta.FechaUltimoProceso = Fecha.GetFecha();
                respuesta.IdProceso = ProcesoEstado.FirmaXml.GetHashCode();

                // obtiene la información de configuración del certificado digital
                CertificadoDigital certificado = HgiConfiguracion.GetConfiguration().CertificadoDigitalData;

                // obtiene la empresa certificadora
                EnumCertificadoras empresa_certificadora = EnumCertificadoras.Andes;

                if (certificado.Certificadora.Equals("andes"))
                    empresa_certificadora = EnumCertificadoras.Andes;
                else if (certificado.Certificadora.Equals("gse"))
                    empresa_certificadora = EnumCertificadoras.Gse;

                // información del certificado digital
                string ruta_certificado = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), certificado.RutaLocal);
                documento_result = Ctl_Firma.Generar(ruta_certificado, certificado.Serial, certificado.Clave, empresa_certificadora, documento_result);

                //Actualiza Documento en Base de Datos
                documentoBd.StrUrlArchivoPdf = respuesta.UrlPdf;
                documentoBd.DatFechaActualizaEstado = respuesta.FechaUltimoProceso;
                documentoBd.IntIdEstado = Convert.ToInt16(respuesta.IdProceso);

                Ctl_Documento documento_tmp = new Ctl_Documento();
                documentoBd = documento_tmp.Actualizar(documentoBd);
            }
            catch (Exception excepcion)
            {
                respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en el firmado del documento UBL en XML. Detalle: {0} ", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);
				LogExcepcion.Guardar(excepcion);
			}
            return respuesta;
        }

        /// <summary>
        /// Comprime el documento XML en estandar UBL firmado
        /// </summary>
        /// <param name="documentoBd">información del documento en base de datos</param>
        /// <param name="respuesta">datos de respuesta del documento</param>
        /// <param name="documento_result">información del proceso interno del documento</param>
        /// <returns>información adicional de respuesta del documento</returns>
        public static DocumentoRespuesta UblComprimir(TblDocumentos documentoBd, ref DocumentoRespuesta respuesta, ref FacturaE_Documento documento_result)
        {
            try
            {
                respuesta.DescripcionProceso = "Compresión del archivo XML firmado con la información en estandar UBL.";
                respuesta.FechaUltimoProceso = Fecha.GetFecha();
                respuesta.IdProceso = ProcesoEstado.CompresionXml.GetHashCode();

                documento_result.NombreZip = Ctl_Compresion.Comprimir(documento_result);

                //Actualiza Documento en Base de Datos
                documentoBd.DatFechaActualizaEstado = respuesta.FechaUltimoProceso;
                documentoBd.IntIdEstado = Convert.ToInt16(respuesta.IdProceso);

                Ctl_Documento documento_tmp = new Ctl_Documento();
                documento_tmp.Actualizar(documentoBd);
            }
            catch (Exception excepcion)
            {
                respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en la compresión del documento UBL en XML firmado. Detalle: {0}", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);
				LogExcepcion.Guardar(excepcion);
			}
            return respuesta;
        }


    }
}
