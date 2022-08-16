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
using HGInetMiFacturaElectonicaController.Auditorias;
using HGInetMiFacturaElectonicaController.Properties;

namespace HGInetMiFacturaElectonicaController.ServiciosDian
{
	public class Ctl_DocumentoDian
	{

		public static AcuseRecibo Enviar(FacturaE_Documento documento, TblDocumentos documentoBd, TblEmpresas empresa, ref DocumentoRespuesta respuesta, string IdSetDian, bool proceso_sonda, int proceso_acuse = 0)
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

					if (!plataforma_datos.RutaPublica.Contains("habilitacion") && !plataforma_datos.RutaPublica.Contains("localhost"))
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

						if (empresa.IntHabilitacionNomina == null)
							empresa.IntHabilitacionNomina = 0;

						//si son pruebas de documentos diferentes a nomina y es HGI que continue enviado los documentos para que los valide
						if (documento.DocumentoTipo.GetHashCode() < TipoDocumento.AcuseRecibo.GetHashCode() && empresa.IntHabilitacion == Habilitacion.PruebasDian.GetHashCode())
						{
							empresa.IntHabilitacionNomina = 2;
						}

						bool habilitar_set = false;

						//Si es documento electronico y esta habilitando el set de pruebas
						if (documento.DocumentoTipo.GetHashCode() < TipoDocumento.AcuseRecibo.GetHashCode() && empresa.IntHabilitacion < Habilitacion.PruebasDian.GetHashCode() && empresa.IntRadian == false)
							habilitar_set = true;

						//Si es un documento de nomina y esta habilitando el set de pruebas
						if (documento.DocumentoTipo.GetHashCode() > TipoDocumento.Attached.GetHashCode() && empresa.IntHabilitacionNomina < Habilitacion.PruebasDian.GetHashCode())
							habilitar_set = true;
						
						if (documento.DocumentoTipo.GetHashCode() == TipoDocumento.AcuseRecibo.GetHashCode() && ambiente_dian.Equals("2") && empresa.IntRadian == true && empresa.IntHabilitacion < Habilitacion.PruebasDian.GetHashCode())
							habilitar_set = true;

						//Si quiere que el documento sea validado por la DIAN aun si ya tiene el set de pruebas aceptado se pone en ambiente 2 las so propiedades de empresa 
						if (ambiente_dian.Equals("2") && habilitar_set == true)
						{
							acuse = Ctl_Factura.Enviar_v2(ruta_zip, documento.NombreZip, ruta_certificado,
								certificado.Clave, clave, UrlServicioWeb, ambiente_dian, proceso_acuse);
						}
						else
						{
								List<HGInetDIANServicios.DianWSValidacionPrevia.DianResponse> respuesta_dian = null;

								//Se agrega este proceso para evitar envio a la DIAN si hay intermitencia y demora en la respuesta 
								bool enviar_sincronico = true;

								//Si es proceso por la sonda no se valida 
								if (proceso_sonda == false && documento.DocumentoTipo.GetHashCode() < TipoDocumento.AcuseRecibo.GetHashCode())
								{
									//Esta empresa se usa solo para determinar si envia normal o por sonda posteriormente
									//*****Se debe cambiar por una tabla de configuracion y es provisional esta forma.
									Ctl_Empresa empresa_indicador = new Ctl_Empresa();
									TblEmpresas indicador_envio = empresa_indicador.Obtener("888989");
									if (indicador_envio.IntManejaAnexos == true)
										enviar_sincronico = false;
								}

								if (enviar_sincronico == true)
								{
									//Envio el documento y guardo la respuesta en archivo y en objeto respuesta_dian
									if (documento.DocumentoTipo.GetHashCode() != TipoDocumento.AcuseRecibo.GetHashCode())
									{
										acuse = Ctl_Factura.EnviarSync_v2(ruta_zip, documento.NombreXml, documento.RutaArchivosProceso.Replace("XmlFacturaE", "FacturaEConsultaDian"), ruta_certificado, certificado.Clave, UrlServicioWeb, ambiente_dian, ref respuesta_dian, documento.CUFE, documentoBd.IntDocTipo);
									}
									else
									{
										acuse = Ctl_Factura.EnviarSync_v2(ruta_zip, documento.NombreZip, documento.RutaArchivosEnvio, ruta_certificado, certificado.Clave, UrlServicioWeb, ambiente_dian, ref respuesta_dian, documento.CUFE, documento.DocumentoTipo.GetHashCode(), proceso_acuse);

										//Se agrega validacion por inconsistencias de la DIAN y actualizar los eventos ya recibidos
										try
										{
											if (respuesta_dian.FirstOrDefault().ErrorMessage.FirstOrDefault().Contains("LGC01") || respuesta_dian.FirstOrDefault().ErrorMessage.FirstOrDefault().Contains("LGC05") || respuesta_dian.FirstOrDefault().ErrorMessage.FirstOrDefault().Contains("Regla: 90, Rechazo: Documento"))
											{
												Registros.Ctl_Documento Controlador = new Registros.Ctl_Documento();
												Controlador.ConsultarEventosRadian(false, documentoBd.StrIdSeguridad.ToString());
											}
										}
										catch (Exception)
										{
											try
											{
												if (respuesta_dian.FirstOrDefault().StatusDescription.Contains("no autorizado a enviar documentos"))
												{
													respuesta_dian.FirstOrDefault().ErrorMessage = LibreriaGlobalHGInet.Formato.Coleccion.ConvertirLista(respuesta_dian.FirstOrDefault().StatusDescription).ToArray();
													respuesta_dian.FirstOrDefault().StatusCode = "99";
													acuse.MessagesFieldV2 = new HGInetDIANServicios.DianWSValidacionPrevia.XmlParamsResponseTrackId[1];
													acuse.MessagesFieldV2[0] = new HGInetDIANServicios.DianWSValidacionPrevia.XmlParamsResponseTrackId();
													acuse.MessagesFieldV2[0].ProcessedMessage = respuesta_dian.FirstOrDefault().StatusDescription;
												}
											}
											catch (Exception excepcion)
											{
												RegistroLog.EscribirLog(excepcion, MensajeCategoria.ServicioDian, MensajeTipo.Error, MensajeAccion.envio, string.Format("Enviando evento, no se pudo llenar el mensaje de error de la DIAN segun Status {0} y StatusDescription", respuesta_dian.FirstOrDefault().StatusCode));
											}
										}
									}


									
								}
								else
								{
									acuse.Version = "2";
									acuse.Response = 201;
									acuse.ReceivedDateTime = Fecha.GetFecha();
									acuse.ResponseDateTime = Fecha.GetFecha();

									HGInetDIANServicios.DianWSValidacionPrevia.DianResponse respuesta_asincronica = new HGInetDIANServicios.DianWSValidacionPrevia.DianResponse();
									respuesta_asincronica.StatusCode = "94";
									respuesta_asincronica.ErrorMessage = LibreriaGlobalHGInet.Formato.Coleccion.ConvertirLista("No se obtuvo respuesta de la DIAN consultando el estado del documento,Por favor no hacer modificaciones al documento y enviarlo de nuevo a la plataforma").ToArray();
									respuesta_asincronica.IsValid = false;
									respuesta_dian = new List<HGInetDIANServicios.DianWSValidacionPrevia.DianResponse>();
									respuesta_dian.Add(respuesta_asincronica);
									documentoBd.IntIdEstado = 7;
									respuesta.FechaUltimoProceso = Fecha.GetFecha();
								}

								respuesta.DescripcionProceso = Enumeracion.GetDescription(ProcesoEstado.EnvioZip);
								respuesta.IdProceso = ProcesoEstado.EnvioZip.GetHashCode();

								//Se agrega proceso de Validacion por intermitencias en la DIAN y problemas a procesar las nominas emitidas el 1 de abril de 2022
								//La respuesta del servicio de la DIAN es codigo 66, no valido pero en el base 64 que esta la respuesta de la DIAN dice que pasa correcto
								if (respuesta_dian.FirstOrDefault().StatusCode.Equals("66") && documentoBd.IntDocTipo >= TipoDocumento.Nomina.GetHashCode())
								{
									string url_respuesta = string.Format(@"{0}\{1}.xml", documento.RutaArchivosProceso.Replace("XmlFacturaE", "FacturaEConsultaDian"), documento.NombreXml);

									string contenido_xml = Archivo.ObtenerContenido(url_respuesta);

									// valida el contenido del archivo
									if (string.IsNullOrWhiteSpace(contenido_xml))
										throw new ArgumentException("El archivo XML UBL se encuentra vacío.");

									// convierte el contenido de texto a xml
									System.Xml.XmlReader xml_reader = System.Xml.XmlReader.Create(new System.IO.StringReader(contenido_xml));

									// convierte el objeto de acuerdo con el tipo de documento
									System.Xml.Serialization.XmlSerializer serializacion1 = new System.Xml.Serialization.XmlSerializer(typeof(HGInetUBLv2_1.ApplicationResponseType));

									HGInetUBLv2_1.ApplicationResponseType conversion = (HGInetUBLv2_1.ApplicationResponseType)serializacion1.Deserialize(xml_reader);

									List<Acuse> objeto_acuse = HGInetUBLv2_1.AcuseReciboXMLv2_1.Convertir(conversion);

									if (objeto_acuse != null)
									{
										foreach (Acuse item_acuse in objeto_acuse)
										{
											if (item_acuse.CodigoRespuesta.Equals("02"))
											{
												respuesta.Cufe = item_acuse.CufeDocumento;
												respuesta_dian.FirstOrDefault().StatusCode = "00";
												respuesta_dian.FirstOrDefault().StatusDescription = "Procesado Correctamente.";
												respuesta_dian.FirstOrDefault().StatusMessage = "Procesado Correctamente.";
												respuesta_dian.FirstOrDefault().IsValid = true;
												respuesta_dian.FirstOrDefault().ErrorMessage = new string[0];
												acuse.Response = 201;
												acuse.Comments = "Documento Electrónico recibido exitosamente";

										}
										}
									}
								}

								//Se procesa la respuesta entregada
								ConsultaDocumento consulta_doc = Ctl_ConsultaTransacciones.ValidarTransaccionV2(respuesta_dian);

								//Se valida respuesta para indicar el estado de las validaciones que se le hicieron al documento
								HGInetMiFacturaElectonicaController.Procesos.Ctl_Documentos.ValidarRespuestaConsulta(consulta_doc, documentoBd, empresa, respuesta, string.Format("{0}.xml",documento.NombreXml), documento.DocumentoTipo);
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
				Ctl_Log.Guardar(excepcion, log_categoria, MensajeTipo.Error, log_accion);
				throw excepcion;
			}
		}
		
	}
}
