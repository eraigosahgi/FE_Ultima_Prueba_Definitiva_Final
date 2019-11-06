using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HGInetDIANServicios;
using HGInetDIANServicios.DianFactura;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetFirmaDigital;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.RegistroLog;

namespace HGInetMiFacturaElectonicaController.ServiciosDian
{
	public class Ctl_DocumentoDian
	{

		public static AcuseRecibo Enviar(FacturaE_Documento documento, TblDocumentos documentoBd, TblEmpresas empresa, ref DocumentoRespuesta respuesta, string IdSetDian)
		{

			string IdSoftware = null;
			string PinSoftware = null;
			string clave = null;
			string UrlServicioWeb = null;
			//V2-Ambiente de la DIAN al que se va enviar el documento: 1 - Produccion, 2 - Pruebas
			string ambiente_dian = string.Empty;
			MensajeCategoria log_categoria = MensajeCategoria.BaseDatos;
			MensajeAccion log_accion = MensajeAccion.envio;

			try
			{
				string ruta_certificado = string.Empty;
				CertificadoDigital certificado = null;

				if (empresa.IntVersionDian == 2)
				{
					PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

					if (plataforma_datos.RutaPublica.Contains("app"))
					{
						ambiente_dian = "1";
					}
					else
					{
						ambiente_dian = "2";
						if (string.IsNullOrEmpty(IdSetDian))
							throw new ApplicationException("El campo IdSetDian de Pruebas no se encontró en la resolución");
					}

					log_categoria = MensajeCategoria.Certificado;
					log_accion = MensajeAccion.lectura;


					// obtiene la información de configuración del certificado digital
					certificado = HgiConfiguracion.GetConfiguration().CertificadoDigitalData;

					// obtiene la empresa certificadora
					EnumCertificadoras empresa_certificadora = EnumCertificadoras.Andes;

					if (certificado.Certificadora.Equals("andes"))
						empresa_certificadora = EnumCertificadoras.Andes;
					else if (certificado.Certificadora.Equals("gse"))
						empresa_certificadora = EnumCertificadoras.Gse;

					// información del certificado digital
					ruta_certificado = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), certificado.RutaLocal);

					log_categoria = MensajeCategoria.Conexion;
					log_accion = MensajeAccion.lectura;

					// obtiene los datos del proveedor tecnológico de la DIAN
					DianProveedorV2 data_dian = HgiConfiguracion.GetConfiguration().DianProveedorV2;

					IdSoftware = data_dian.IdSoftware;
					PinSoftware = data_dian.Pin;
					clave = IdSetDian;//data_dian.ClaveAmbiente;
					UrlServicioWeb = data_dian.UrlServicioWeb;
				}
				else
				{

					// sobrescribe los datos de la resolución si se encuentra en estado de habilitación
					if (empresa.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
					{
						// obtiene los datos de prueba del proveedor tecnológico de la DIAN
						DianProveedorTest data_dian_habilitacion = HgiConfiguracion.GetConfiguration().DianProveedorTest;

						IdSoftware = data_dian_habilitacion.IdSoftware;
						PinSoftware = data_dian_habilitacion.Pin;
						clave = data_dian_habilitacion.ClaveAmbiente;
						UrlServicioWeb = data_dian_habilitacion.UrlServicioWeb;
					}
					else
					{
						// obtiene los datos del proveedor tecnológico de la DIAN
						DianProveedor data_dian = HgiConfiguracion.GetConfiguration().DianProveedor;

						IdSoftware = data_dian.IdSoftware;
						PinSoftware = data_dian.Pin;
						clave = data_dian.ClaveAmbiente;
						UrlServicioWeb = data_dian.UrlServicioWeb;
					}
				}

				string prefijo = string.Empty;

				string numero = string.Empty;

				string nit_obligado = string.Empty;

				DateTime fecha = new DateTime();

				switch (documento.DocumentoTipo)
				{
					case TipoDocumento.Factura:
						Factura doc_factura= ((Factura)documento.Documento);
						prefijo = doc_factura.Prefijo;
						numero = doc_factura.Documento.ToString();
						nit_obligado = doc_factura.DatosObligado.Identificacion;
						fecha = doc_factura.Fecha;
						break;
					case TipoDocumento.NotaCredito:
						NotaCredito doc_nota_credito = ((NotaCredito)documento.Documento);
						prefijo = doc_nota_credito.Prefijo;
						numero = doc_nota_credito.Documento.ToString();
						nit_obligado = doc_nota_credito.DatosObligado.Identificacion;
						fecha = doc_nota_credito.Fecha;
						break;
					case TipoDocumento.NotaDebito:
						NotaDebito doc_nota_debito = ((NotaDebito)documento.Documento);
						prefijo = doc_nota_debito.Prefijo;
						numero = doc_nota_debito.Documento.ToString();
						nit_obligado = doc_nota_debito.DatosObligado.Identificacion;
						fecha = doc_nota_debito.Fecha;
						break;
					default:
						break;
				}

				log_categoria = MensajeCategoria.Archivos;
				log_accion = MensajeAccion.lectura;

				// ruta del zip
				string ruta_zip = string.Format(@"{0}\{1}.zip", documento.RutaArchivosEnvio, documento.NombreZip);

				// valida el archivo zip si existe
				if (!Archivo.ValidarExistencia(ruta_zip))
					throw new ApplicationException(string.Format("No se encuentra la ruta del archivo zip: {0}", ruta_zip));


				AcuseRecibo acuse = null;

				log_categoria = MensajeCategoria.ServicioDian;
				log_accion = MensajeAccion.envio;

				switch (empresa.IntVersionDian)
				{
					// envía el documento a través de los servicios web de la DIAN
					case 1:
						acuse = Ctl_Factura.Enviar(documento.IdSeguridadDocumento, IdSoftware, clave, prefijo, numero, fecha, nit_obligado, ruta_zip, UrlServicioWeb);
						break;

					case 2:
						acuse = new AcuseRecibo();
						if (ambiente_dian.Equals("2"))
						{
							acuse = Ctl_Factura.Enviar_v2(ruta_zip, documento.NombreZip, ruta_certificado,
								certificado.Clave, clave, UrlServicioWeb, ambiente_dian);
						}
						else
						{
								List<HGInetDIANServicios.DianWSValidacionPrevia.DianResponse> respuesta_dian = null;

								//Envio el documento y guardo la respuesta en archivo y en objeto respuesta_dian
								acuse = Ctl_Factura.EnviarSync_v2(ruta_zip, documento.NombreXml, documento.RutaArchivosProceso.Replace("XmlFacturaE", "FacturaEConsultaDian"), ruta_certificado,certificado.Clave, UrlServicioWeb, ambiente_dian, ref respuesta_dian);

								respuesta.DescripcionProceso = Enumeracion.GetDescription(ProcesoEstado.EnvioZip);
								respuesta.IdProceso = ProcesoEstado.EnvioZip.GetHashCode();

								//Se procesa la respuesta entregada
								ConsultaDocumento consulta_doc = Ctl_ConsultaTransacciones.ValidarTransaccionV2(respuesta_dian);

								//Se valida respuesta para indicar el estado de las validaciones que se le hicieron al documento
								HGInetMiFacturaElectonicaController.Procesos.Ctl_Documentos.ValidarRespuestaConsulta(consulta_doc, documentoBd, empresa, respuesta, string.Format("{0}.xml",documento.NombreXml));
						}
						break;

					default:
						acuse = Ctl_Factura.Enviar(documento.IdSeguridadDocumento, IdSoftware, clave, prefijo, numero, fecha, nit_obligado, ruta_zip, UrlServicioWeb);
						break;
				}

				return acuse;
			}
			catch (Exception excepcion)
			{
				RegistroLog.EscribirLog(excepcion, log_categoria, MensajeTipo.Error, log_accion);
				throw excepcion;
			}
		}
		
	}
}
