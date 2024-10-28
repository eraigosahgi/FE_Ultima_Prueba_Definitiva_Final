﻿using HGInetDIANServicios.DianFactura;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using HGInetDIANServicios.DianWSValidacionPrevia;
using LibreriaGlobalHGInet.RegistroLog;
using LibreriaGlobalHGInet.Objetos;

namespace HGInetDIANServicios
{
	public partial class Ctl_Factura
	{

		/// <summary>
		/// Envía el documento por medio del servicio web de la DIAN
		/// </summary>
		/// <param name="ruta_archivo_zip">Ruta del archivo .zip</param>
		/// <param name="nombre_archivo">nombre del archivo</param>
		/// <param name="ruta_certificado">Ruta del archivo del certificado digital .pfx</param>
		/// <param name="clave_certificado">Clave del certificado digital</param>
		/// <param name="clave_dian">Clave proporcionada en la plataforma de la Dian</param>
		/// <param name="ruta_servicio_web">Url del servicio web de la DIAN</param>
		/// <returns></returns>
		public static AcuseRecibo Enviar_v2(string ruta_zip, string nombre_archivo, string ruta_certificado, string clave_certificado, string clave_dian, string ruta_servicio_web, string ambiente, int proceso_acuse = 0)
		{

			MensajeCategoria log_categoria = MensajeCategoria.Certificado;
			MensajeAccion log_accion = MensajeAccion.lectura;

			try
			{

				X509Certificate2 cert = new X509Certificate2(ruta_certificado, clave_certificado);

				//Convertir archivo a bytes para su envio
				Byte[] bytes = File.ReadAllBytes(ruta_zip);

				DianWSValidacionPrevia.UploadDocumentResponse resultadoHab = null;

				AcuseRecibo acuse_recibo = new AcuseRecibo();

				try
				{
					log_categoria = MensajeCategoria.ServicioDian;
					log_accion = MensajeAccion.envio;

					DianWSValidacionPrevia.WcfDianCustomerServicesClient webServiceHab = new DianWSValidacionPrevia.WcfDianCustomerServicesClient();
					webServiceHab.Endpoint.Address = new System.ServiceModel.EndpointAddress(ruta_servicio_web);
					webServiceHab.ClientCredentials.ClientCertificate.Certificate = cert;

					//Se agrega instruccion para habilitar la seguridad en el envio
					System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

					if (ambiente.Equals("2"))
					{
						//Ejecución de prueba DIAN Enviando archivo ZIP	y IdsetDian
						resultadoHab = webServiceHab.SendTestSetAsync(nombre_archivo, bytes, clave_dian);
					}
					else
					{

						if (proceso_acuse == 0)
						{
							//Ejecución de produccion DIAN Enviando archivo ZIP	
							resultadoHab = webServiceHab.SendBillAsync(nombre_archivo, bytes);
						}
					}

					acuse_recibo.ReceivedDateTime = Fecha.GetFecha();
					acuse_recibo.ResponseDateTime = Fecha.GetFecha();
					acuse_recibo.MessagesFieldV2 = resultadoHab.ErrorMessageList;
					acuse_recibo.KeyV2 = resultadoHab.ZipKey;
					acuse_recibo.Version = "2";
				}
				catch (Exception excepcion)
				{
					RegistroLog.EscribirLog(excepcion, log_categoria, MensajeTipo.Error, log_accion);
					throw new ApplicationException(excepcion.Message, excepcion.InnerException);
				}

				try
				{
					if (!string.IsNullOrWhiteSpace(resultadoHab.ZipKey))
					{
						acuse_recibo.Response = 200;
						acuse_recibo.Comments = "Documento Electrónico recibido exitosamente";
					}

					log_categoria = MensajeCategoria.Archivos;
					log_accion = MensajeAccion.creacion;

					string carpeta = Path.GetDirectoryName(ruta_zip) + @"\";

					string archivo = string.Empty;
					if (proceso_acuse == 0)
					{
						archivo = Path.GetFileNameWithoutExtension(ruta_zip) + ".xml";
					}
					else
					{
						archivo = string.Format("{0}-{1}.xml", Path.GetFileNameWithoutExtension(ruta_zip), proceso_acuse);
					}

					// almacena el mensaje de respuesta del servicio web
					archivo = Xml.GuardarObjeto(resultadoHab, carpeta, archivo);

				}
				catch (Exception excepcion)
				{
					RegistroLog.EscribirLog(excepcion, log_categoria, MensajeTipo.Error, log_accion);
				}


				return acuse_recibo;

			}
			catch (Exception excepcion)
			{
				RegistroLog.EscribirLog(excepcion, log_categoria, MensajeTipo.Error, log_accion);

				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		/// <summary>
		/// Envía el documento por medio del servicio web de la DIAN
		/// </summary>
		/// <param name="ruta_archivo_zip">Ruta del archivo .zip</param>
		/// <param name="nombre_archivo">nombre del archivo</param>
		/// <param name="ruta_certificado">Ruta del archivo del certificado digital .pfx</param>
		/// <param name="clave_certificado">Clave del certificado digital</param>
		/// <param name="clave_dian">Clave proporcionada en la plataforma de la Dian</param>
		/// <param name="ruta_servicio_web">Url del servicio web de la DIAN</param>
		/// <returns></returns>
		public static AcuseRecibo EnviarSync_v2(string ruta_zip, string nombre_archivo, string ruta_xml, string ruta_certificado, string clave_certificado, string ruta_servicio_web, string ambiente, ref List<DianWSValidacionPrevia.DianResponse> respuesta_dian, string cufe_doc, int tipo_doc, int proceso_acuse = 0)
		{

			MensajeCategoria log_categoria = MensajeCategoria.Certificado;
			MensajeAccion log_accion = MensajeAccion.lectura;

			try
			{

				X509Certificate2 cert = new X509Certificate2(ruta_certificado, clave_certificado);

				//Convertir archivo a bytes para su envio
				Byte[] bytes = File.ReadAllBytes(ruta_zip);

				DianWSValidacionPrevia.DianResponse respuesta = null;

				AcuseRecibo acuse_recibo = new AcuseRecibo();

				respuesta_dian = new List<DianResponse>();

				//Contingencia de la DIAN 2024-03-09 desde las 6:00 am hasta las 6:00 PM
				DateTime fecha_ini_cont = new DateTime(2024, 03, 09, 6, 0, 0);
				DateTime fecha_fin_cont = new DateTime(2024, 03, 09, 18, 0, 0);

				try
				{
					log_categoria = MensajeCategoria.ServicioDian;
					log_accion = MensajeAccion.envio;

					DianWSValidacionPrevia.WcfDianCustomerServicesClient webServiceHab = new DianWSValidacionPrevia.WcfDianCustomerServicesClient();
					webServiceHab.Endpoint.Address = new System.ServiceModel.EndpointAddress(ruta_servicio_web);
					webServiceHab.ClientCredentials.ClientCertificate.Certificate = cert;

					//Se agrega instruccion para habilitar la seguridad en el envio
					System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

					//if (ambiente.Equals("1"))
					//{
					acuse_recibo.Version = "2";
					acuse_recibo.ReceivedDateTime = Fecha.GetFecha();

					string archivo = "", carpeta = "";

					//Ejecución de produccion DIAN Enviando archivo ZIP	
					try
					{
						//Se agrega validacion hasta una hora antes para que no vaya a los servicios de la DIAN y no demore el proceso
						if (Fecha.GetFecha() >= fecha_ini_cont && Fecha.GetFecha() < fecha_fin_cont.AddHours(-1))
						{
							throw new ApplicationException("No se pudo establecer una relación de confianza para el canal seguro SSL/TLS con la autoridad 'vpfe.dian.gov.co'");
						}
						else
						{

							if (tipo_doc < TipoDocumento.AcuseRecibo.GetHashCode())
								respuesta = webServiceHab.SendBillSync(nombre_archivo, bytes);
							else if (tipo_doc == TipoDocumento.AcuseRecibo.GetHashCode())
								respuesta = webServiceHab.SendEventUpdateStatus(bytes);
							else
								respuesta = webServiceHab.SendNominaSync(bytes);

						}

						log_categoria = MensajeCategoria.Archivos;
						log_accion = MensajeAccion.creacion;

						//Consulta de un documento por el cufe con sus eventos
						//DianWSValidacionPrevia.DianResponse respuesta2 = webServiceHab.GetStatusEvent("258c0c621305fb3f389553eca6f7151fdc7637d6b42fd420bfb3d1ac2c157748f6db9c8c866304ce5586812715c819e5");

						carpeta = Path.GetDirectoryName(ruta_zip) + @"\";

						archivo = Path.GetFileNameWithoutExtension(ruta_zip) + ".xml";

						if (tipo_doc == TipoDocumento.AcuseRecibo.GetHashCode())
						{
							archivo = string.Format("{0}-ws.xml", Path.GetFileNameWithoutExtension(ruta_zip));
						}

						// almacena el mensaje de respuesta del servicio web
						archivo = Xml.GuardarObjeto(respuesta, carpeta, archivo);

					}
					catch (Exception excepcion)
					{
						DateTime fecha_excepcion = Fecha.GetFecha();
						string msg_custom = string.Format("Error Exec WS => Carpeta: {0} - Archivo: {1} - Fecha Envio: {2} - Fecha Excepcion: {3}", carpeta, archivo, acuse_recibo.ReceivedDateTime, fecha_excepcion);

						RegistroLog.EscribirLog(excepcion, log_categoria, MensajeTipo.Error, log_accion, msg_custom);
					}
					finally
					{
						try
						{

							if (webServiceHab != null)
								if (webServiceHab.State != CommunicationState.Closed)
									webServiceHab.Close();
						}
						catch (Exception excepcion)
						{
							DateTime fecha_excepcion = Fecha.GetFecha();
							string msg_custom = string.Format("Error Close WS Envio => Carpeta: {0} - Archivo: {1} - Fecha Envio: {2} - Fecha Excepcion: {3}", carpeta, archivo, acuse_recibo.ReceivedDateTime, fecha_excepcion);

							RegistroLog.EscribirLog(excepcion, log_categoria, MensajeTipo.Error, log_accion, msg_custom);
						}

					}

					acuse_recibo.ResponseDateTime = Fecha.GetFecha();

					log_accion = MensajeAccion.lectura;
					if (respuesta != null)
					{
						//se valida si ya se envio para consultarlo con el cufe que retorna
						try
						{
							if (respuesta.ErrorMessage != null && respuesta.ErrorMessage.Count() > 0)
							{
								bool consultar_nuevamente = (cufe_doc.Equals(respuesta.XmlDocumentKey) && tipo_doc != TipoDocumento.AcuseRecibo.GetHashCode()) ? true : false;

								if ((respuesta.ErrorMessage.Length > 0) && (!string.IsNullOrEmpty(respuesta.ErrorMessage.FirstOrDefault())) && consultar_nuevamente == true)
								{
									if (respuesta.ErrorMessage.FirstOrDefault().Contains("Regla: 90, Rechazo: Documento"))
									{
										webServiceHab = new DianWSValidacionPrevia.WcfDianCustomerServicesClient();
										webServiceHab.Endpoint.Address =
											new System.ServiceModel.EndpointAddress(ruta_servicio_web);
										webServiceHab.ClientCredentials.ClientCertificate.Certificate = cert;

										string key = respuesta.XmlDocumentKey;

										acuse_recibo.ResponseDateTime = Fecha.GetFecha();
										DianWSValidacionPrevia.DianResponse respuesta_consulta = webServiceHab.GetStatus(key);

										if (respuesta_consulta.IsValid == true)
										{
											if (respuesta_consulta.XmlBase64Bytes == null && respuesta.XmlBase64Bytes != null)
												respuesta_consulta.XmlBase64Bytes = respuesta.XmlBase64Bytes;

											//respuesta.StatusCode = "94";
											respuesta = respuesta_consulta;

											string nom_archivo = Path.GetFileNameWithoutExtension(ruta_zip) + ".xml";

											// almacena el mensaje de respuesta de la consulta del servicio web
											archivo = Xml.GuardarObjeto(respuesta, carpeta, nom_archivo);

										}
									}
								}
							}
						}
						catch (Exception excepcion)
						{
							DateTime fecha_excepcion = Fecha.GetFecha();
							string msg_custom = string.Format("[0] - Error conversion mensaje respuesta DIAN {0}, CUFE: {1} - Fecha Envio: {2} - Fecha Excepcion: {3}", nombre_archivo, respuesta.XmlDocumentKey, acuse_recibo.ResponseDateTime, fecha_excepcion);
							RegistroLog.EscribirLog(excepcion, log_categoria, MensajeTipo.Error, log_accion, msg_custom);

						}
						finally
						{
							try
							{
								if (webServiceHab != null)
									if (webServiceHab.State != CommunicationState.Closed)
										webServiceHab.Close();
							}
							catch (Exception excepcion)
							{
								string msg_custom = string.Format("Error Close WS Consulta => Carpeta: {0} - Archivo: {1}", carpeta, archivo);

								RegistroLog.EscribirLog(excepcion, log_categoria, MensajeTipo.Error, log_accion, msg_custom);
							}
						}

						log_accion = MensajeAccion.cargando;
						respuesta_dian.Add(respuesta);

						log_accion = MensajeAccion.creacion;
						if (respuesta.StatusCode.Equals("0") || respuesta.StatusCode.Equals("00") || respuesta.StatusCode.Equals("99") || (respuesta.StatusCode.Equals("66") && respuesta.XmlBase64Bytes != null && tipo_doc.GetHashCode() >= TipoDocumento.Nomina.GetHashCode()))
						{
							if (respuesta.XmlBase64Bytes != null)
							{

								if (tipo_doc == TipoDocumento.AcuseRecibo.GetHashCode())
								{
									nombre_archivo = string.Format("{0}-{1}", nombre_archivo, proceso_acuse);
								}

								//FileStream fs = null;

								try
								{

									//Guardo el Base64 de la Respuesta
									Directorio.CrearDirectorio(ruta_xml);
									// Uso de using para garantizar que los recursos se liberen automáticamente
									using (FileStream fs = new FileStream(string.Format(@"{0}\{1}.xml", ruta_xml, nombre_archivo), FileMode.Create, FileAccess.Write))
									{
										using (BinaryWriter bw = new BinaryWriter(fs, Encoding.Unicode))
										{
											// Escribir los bytes del archivo Base64
											bw.Write(respuesta.XmlBase64Bytes);
										}
									}

									//using (fs = new FileStream(string.Format(@"{0}\{1}.xml", ruta_xml, nombre_archivo),
									//	FileMode.Create, FileAccess.ReadWrite))
									//{
									//	BinaryWriter bw = new BinaryWriter(fs, Encoding.Unicode);
									//	bw.Write(respuesta.XmlBase64Bytes);
									//	bw.Close();
									//	fs.Close();
									//}

									nombre_archivo = string.Format("{0}-WS-1", nombre_archivo);

								}
								catch (Exception excepcion)
								{
									//respuesta.StatusCode = "94";
									//respuesta.ErrorMessage = LibreriaGlobalHGInet.Formato.Coleccion.ConvertirLista("No se guardo respuesta de la DIAN enviando el documento. Por favor no hacer modificaciones al documento y enviarlo de nuevo a la plataforma.").ToArray();
									//respuesta.IsValid = false;


									string msg_custom = string.Format("Error FileB64 => Carpeta: {0} - Archivo: {1}", ruta_xml, nombre_archivo);

									RegistroLog.EscribirLog(excepcion, log_categoria, MensajeTipo.Error, log_accion, msg_custom);

								}
								//finally
								//{
								//	if (fs != null)
								//		fs.Close();
								//}
								log_categoria = MensajeCategoria.ServicioDian;
								log_accion = MensajeAccion.consulta;
							}
							else if (respuesta.StatusDescription.Equals("En proceso de validación"))
							{
								try
								{
									respuesta.StatusCode = "94";
									respuesta.IsValid = false;

									if (respuesta.ErrorMessage == null)
										respuesta.ErrorMessage = LibreriaGlobalHGInet.Formato.Coleccion.ConvertirLista("No se obtuvo respuesta de la DIAN enviando el documento. Por favor no hacer modificaciones al documento y enviarlo de nuevo a la plataforma.").ToArray();

								}
								catch (Exception excepcion)
								{
									string msg_custom = string.Format("[1] - Error conversion mensaje respuesta DIAN {0}", nombre_archivo);
									RegistroLog.EscribirLog(excepcion, log_categoria, MensajeTipo.Error, log_accion, msg_custom);

								}
							}
							else
							{
								try
								{
									respuesta.StatusCode = "99";
									respuesta.IsValid = false;

									if (respuesta.ErrorMessage == null)
										respuesta.ErrorMessage = LibreriaGlobalHGInet.Formato.Coleccion.ConvertirLista("No se obtuvo respuesta de la DIAN enviando el documento. Por favor no hacer modificaciones al documento y enviarlo de nuevo a la plataforma.").ToArray();

								}
								catch (Exception excepcion)
								{
									string msg_custom = string.Format("[2] - Error conversion mensaje respuesta DIAN {0}", nombre_archivo);
									RegistroLog.EscribirLog(excepcion, log_categoria, MensajeTipo.Error, log_accion, msg_custom);

								}
							}

							if (respuesta.ErrorMessage != null && respuesta.ErrorMessage.Count() > 0)
							{
								try
								{
									acuse_recibo.MessagesFieldV2 = new HGInetDIANServicios.DianWSValidacionPrevia.XmlParamsResponseTrackId[1];
									acuse_recibo.MessagesFieldV2[0] = new DianWSValidacionPrevia.XmlParamsResponseTrackId();
									acuse_recibo.MessagesFieldV2[0].ProcessedMessage = LibreriaGlobalHGInet.Formato.Coleccion.ConvertListToString(respuesta.ErrorMessage.ToList(), ",");
								}
								catch (Exception excepcion)
								{
									string msg_custom = string.Format("[3] - Error conversion mensaje respuesta DIAN {0}", nombre_archivo);
									RegistroLog.EscribirLog(excepcion, log_categoria, MensajeTipo.Error, log_accion, msg_custom);

									acuse_recibo.MessagesFieldV2[0].ProcessedMessage = String.Empty;

								}
							}
						}
						else
						{
							try
							{
								respuesta.StatusCode = "94";
								respuesta.IsValid = false;
								if (respuesta.ErrorMessage == null)
									respuesta.ErrorMessage = LibreriaGlobalHGInet.Formato.Coleccion.ConvertirLista("No se obtuvo respuesta de la DIAN enviando el documento. Por favor no hacer modificaciones al documento y enviarlo de nuevo a la plataforma.").ToArray();

							}
							catch (Exception excepcion)
							{
								string msg_custom = string.Format("[4] - Error conversion mensaje respuesta DIAN {0}", nombre_archivo);
								RegistroLog.EscribirLog(excepcion, log_categoria, MensajeTipo.Error, log_accion, msg_custom);

							}
						}
					}
					else
					{
						try
						{

							log_categoria = MensajeCategoria.ServicioDian;
							log_accion = MensajeAccion.consulta;

							respuesta = new DianResponse();
							respuesta.StatusCode = "94";
							respuesta.ErrorMessage = LibreriaGlobalHGInet.Formato.Coleccion.ConvertirLista("No se obtuvo respuesta de la DIAN consultando el estado del documento,Por favor no hacer modificaciones al documento y enviarlo de nuevo a la plataforma").ToArray();
							

							if (Fecha.GetFecha() >= fecha_ini_cont && Fecha.GetFecha() < fecha_fin_cont)
							{
								respuesta.ErrorMessage = LibreriaGlobalHGInet.Formato.Coleccion.ConvertirLista("Nos permitimos informar que el 09 de marzo de 2024, a partir de las 06:00 am y hasta las 6:00 pm, se realizará una ventana de mantenimiento en el Sistema de Facturación Electrónica DIAN, por lo que durante este tiempo no estará disponible este servicio informático,Por favor no hacer modificaciones al documento y enviarlo de nuevo a la plataforma unas horas despues pasada la contingencia de la DIAN").ToArray();
							}
							respuesta.IsValid = false;
							respuesta_dian.Add(respuesta);
						}
						catch (Exception excepcion)
						{
							string msg_custom = string.Format("[5] - Error conversion mensaje respuesta DIAN {0}", nombre_archivo);
							RegistroLog.EscribirLog(excepcion, log_categoria, MensajeTipo.Error, log_accion, msg_custom);

						}


					}
					//}
				}
				catch (Exception excepcion)
				{
					string msg_custom = string.Format("Error Validaciones Estado {0}", nombre_archivo);


					RegistroLog.EscribirLog(excepcion, log_categoria, MensajeTipo.Error, log_accion, msg_custom);
					throw excepcion;
				}

				if (!respuesta.StatusCode.Equals("94"))
				{
					acuse_recibo.Response = 201;
					acuse_recibo.Comments = "Documento Electrónico recibido exitosamente";
				}

				return acuse_recibo;

			}
			catch (Exception excepcion)
			{

				string msg_custom = string.Format("Error Principal {0}", nombre_archivo);


				RegistroLog.EscribirLog(excepcion, log_categoria, MensajeTipo.Error, log_accion, msg_custom);

				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


	}
}
