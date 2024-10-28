using HGInetFirmaDigital;
using HGInetMiFacturaElectonicaController.Auditorias;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using LibreriaGlobalHGInet.RegistroLog;
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
        /// <param name="cadena_cufe">Se guarda el la cadena de informacion con que se calcula el CUFE</param>
        /// <returns>información adicional de respuesta del documento</returns>
        public static DocumentoRespuesta UblGenerar(object documento_obj, TipoDocumento tipo_doc, TblEmpresasResoluciones resolucion, TblDocumentos documentoBd, TblEmpresas empresa, ref DocumentoRespuesta respuesta, ref FacturaE_Documento documento_result, ref string cadena_cufe)
        {   
            try
            {
                respuesta.DescripcionProceso = "Genera información en estandar UBL.";
                respuesta.FechaUltimoProceso = Fecha.GetFecha();
                respuesta.IdProceso = ProcesoEstado.UBL.GetHashCode();
				respuesta.IdEstado = Ctl_Documento.ObtenerCategoria(respuesta.IdProceso);

				//Genera Ubl
				if (tipo_doc == TipoDocumento.Factura)
                    documento_result = Ctl_Ubl.Generar(documento_result.IdSeguridadDocumento, (Factura)documento_obj, tipo_doc, empresa, resolucion, ref cadena_cufe);
                else if (tipo_doc == TipoDocumento.NotaCredito)
                    documento_result = Ctl_Ubl.Generar(documento_result.IdSeguridadDocumento, (NotaCredito)documento_obj, tipo_doc, empresa, resolucion, ref cadena_cufe);
				else if (tipo_doc == TipoDocumento.NotaDebito)
					documento_result = Ctl_Ubl.Generar(documento_result.IdSeguridadDocumento, (NotaDebito)documento_obj, tipo_doc, empresa, resolucion, ref cadena_cufe);
				else if (tipo_doc == TipoDocumento.Nomina)
					documento_result = Ctl_Ubl.Generar(documento_result.IdSeguridadDocumento, (Nomina)documento_obj, tipo_doc, empresa, resolucion, ref cadena_cufe);
				else if (tipo_doc == TipoDocumento.NominaAjuste)
					documento_result = Ctl_Ubl.Generar(documento_result.IdSeguridadDocumento, (NominaAjuste)documento_obj, tipo_doc, empresa, resolucion, ref cadena_cufe);
			}
			catch (Exception excepcion)
            {
                respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en la generación del estandar UBL del documento. Detalle: {0} ", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Archivos, MensajeTipo.Error, MensajeAccion.creacion);
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
				respuesta.IdEstado = Ctl_Documento.ObtenerCategoria(respuesta.IdProceso);

				// almacena el xml
				documento_result = Ctl_Ubl.Almacenar(documento_result);

                PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData; 

                // url pública del xml
                string url_ppal = string.Format("{0}/{1}/{2}", plataforma_datos.RutaDmsPublica, Constantes.CarpetaFacturaElectronica, documento_result.IdSeguridadTercero.ToString());
				respuesta.UrlXmlUbl = string.Format(@"{0}/{1}/{2}.xml", url_ppal, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaXmlFacturaE, documento_result.NombreXml);

				//Actualiza Documento en Base de Datos
				documentoBd.DatFechaActualizaEstado = respuesta.FechaUltimoProceso;
                documentoBd.IntIdEstado = Convert.ToInt16(respuesta.IdProceso); 
				documentoBd.IdCategoriaEstado = respuesta.IdEstado;
				documentoBd.StrUrlArchivoUbl = respuesta.UrlXmlUbl;
				documentoBd.StrEmpresaAdquiriente = respuesta.Identificacion;
				documentoBd.DatFechaActualizaEstado = respuesta.FechaUltimoProceso;
                documentoBd.StrCufe = documento_result.CUFE;

				//Ctl_Documento documento_tmp = new Ctl_Documento();
				//documento_tmp.Actualizar(documentoBd);
			}
            catch (Exception excepcion)
            {
				string mensaje_error = excepcion.Message;
				//Caso 775723 - 776934
				if (excepcion.Message.Contains("El proceso no puede obtener acceso al archivo"))
				{
					mensaje_error = "No fue posible generar el archivo XML del documento, por favor no modifique la información del documento y envíelo de nuevo.";
					excepcion = new ApplicationException();
				}

				respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en el almacenamiento del documento UBL en XML. Detalle: {0} ", mensaje_error), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Archivos, MensajeTipo.Error, MensajeAccion.creacion);
			}
            return respuesta;
        }

		/// <summary>
		/// Realiza el proceso de firmado del documento XML en estandar UBL
		/// </summary>
		/// <param name="empresa">información del obligado</param>
		/// <param name="documentoBd">información del documento en base de datos</param>
		/// <param name="respuesta">datos de respuesta del documento</param>
		/// <param name="documento_result">información del proceso interno del documento</param>
		/// <returns>información adicional de respuesta del documento</returns>
		public static DocumentoRespuesta UblFirmar(TblEmpresas empresa, TblDocumentos documentoBd, ref DocumentoRespuesta respuesta, ref FacturaE_Documento documento_result)
        {
            try
            {
                respuesta.DescripcionProceso = "Firma el archivo XML con la información en estandar UBL.";
                respuesta.FechaUltimoProceso = Fecha.GetFecha();
                respuesta.IdProceso = ProcesoEstado.FirmaXml.GetHashCode();
				respuesta.IdEstado = Ctl_Documento.ObtenerCategoria(respuesta.IdProceso);

				PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;
				
				bool firma_proveedor = true;
				string ruta_certificado = string.Empty;
				string certificado_clave = string.Empty;
				string certificado_serial = string.Empty;
				string certificado_nit = string.Empty;
				
				// obtiene la empresa certificadora
				EnumCertificadoras empresa_certificadora = EnumCertificadoras.Andes;

				// si firma HGI SAS como Proveedor Tecnológico
				if (empresa.IntCertFirma == 0 || empresa.IntVersionDian ==1)
				{
					// obtiene la información de configuración del certificado digital
					CertificadoDigital certificado = HgiConfiguracion.GetConfiguration().CertificadoDigitalData;
										
					if (certificado.Certificadora.Equals("andes"))
						empresa_certificadora = EnumCertificadoras.Andes;
					else if (certificado.Certificadora.Equals("gse"))
						empresa_certificadora = EnumCertificadoras.Gse;

					certificado_clave = certificado.Clave;
					certificado_nit = Constantes.NitResolucionconPrefijo;

					ruta_certificado = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), certificado.RutaLocal);
				}
				else
				{
					certificado_clave = empresa.StrCertClave;
					certificado_nit = empresa.StrIdentificacion;

					empresa_certificadora = Enumeracion.GetEnumObjectByValue<HGInetFirmaDigital.EnumCertificadoras>((int)empresa.IntCertProveedor);

					firma_proveedor = false;
					
					ruta_certificado = string.Format("{0}\\{1}\\{2}\\{3}.pfx", plataforma_datos.RutaDmsFisica, Constantes.CarpetaCertificadosDigitales, empresa.StrCertRuta,empresa.StrIdSeguridad);
				}
				
				// genera la firma del documento XML
				documento_result = Ctl_Firma.Generar(certificado_nit, ruta_certificado, certificado_serial, certificado_clave, empresa_certificadora, documento_result, firma_proveedor);

				if (documento_result.DocumentoTipo.GetHashCode() < TipoDocumento.AcuseRecibo.GetHashCode() || documento_result.DocumentoTipo == TipoDocumento.Nomina || documento_result.DocumentoTipo == TipoDocumento.NominaAjuste)
				{
					// ruta física del xml
					string carpeta_xml = string.Format("{0}\\{1}\\{2}", plataforma_datos.RutaDmsFisica,Constantes.CarpetaFacturaElectronica, documento_result.IdSeguridadTercero.ToString());
					carpeta_xml = string.Format(@"{0}\{1}", carpeta_xml,LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEConsultaDian);

					// url pública del xml
					string url_ppal = string.Format("{0}/{1}/{2}", plataforma_datos.RutaDmsPublica,Constantes.CarpetaFacturaElectronica, documento_result.IdSeguridadTercero.ToString());
					respuesta.UrlXmlUbl = string.Format(@"{0}/{1}/{2}.xml", url_ppal,LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian, documento_result.NombreXml);

					//Actualiza Documento en Base de Datos
					documentoBd.StrUrlArchivoUbl = respuesta.UrlXmlUbl;
					documentoBd.DatFechaActualizaEstado = respuesta.FechaUltimoProceso;
					documentoBd.IntIdEstado = Convert.ToInt16(respuesta.IdProceso);
					documentoBd.IdCategoriaEstado = respuesta.IdEstado;

					Ctl_Documento documento_tmp = new Ctl_Documento();
					documentoBd = documento_tmp.Actualizar(documentoBd);
				}
            }
            catch (Exception excepcion)
            {
                respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en el firmado del documento UBL en XML. Detalle: {0} ", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Archivos, MensajeTipo.Error, MensajeAccion.creacion);
				documentoBd.IntEnvioMail = true;
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
                respuesta.DescripcionProceso = Enumeracion.GetDescription(ProcesoEstado.CompresionXml);
                respuesta.FechaUltimoProceso = Fecha.GetFecha();
                respuesta.IdProceso = ProcesoEstado.CompresionXml.GetHashCode();
				respuesta.IdEstado = Ctl_Documento.ObtenerCategoria(respuesta.IdProceso);

				documento_result.NombreZip = Ctl_Compresion.Comprimir(documento_result);

                //Actualiza Documento en Base de Datos
                documentoBd.DatFechaActualizaEstado = respuesta.FechaUltimoProceso;
                documentoBd.IntIdEstado = Convert.ToInt16(respuesta.IdProceso);
				documentoBd.IdCategoriaEstado = respuesta.IdEstado;

                Ctl_Documento documento_tmp = new Ctl_Documento();
                documento_tmp.Actualizar(documentoBd);
            }
            catch (Exception excepcion)
            {
                respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en la compresión del documento UBL en XML firmado. Detalle: {0}", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Archivos, MensajeTipo.Error, MensajeAccion.creacion);
				documentoBd.IntEnvioMail = true;
			}
            return respuesta;
        }


    }
}
