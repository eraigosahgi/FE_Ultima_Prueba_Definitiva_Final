using HGInetDIANServicios.DianFactura;
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
		public static AcuseRecibo Enviar_v2(string ruta_zip, string nombre_archivo, string ruta_certificado, string clave_certificado, string clave_dian, string ruta_servicio_web, string ambiente)
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
						//Ejecución de produccion DIAN Enviando archivo ZIP	
						resultadoHab = webServiceHab.SendBillAsync(nombre_archivo, bytes);
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
					throw excepcion;
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

					string archivo = Path.GetFileNameWithoutExtension(ruta_zip) + ".xml";

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
		public static AcuseRecibo EnviarSync_v2(string ruta_zip, string nombre_archivo, string ruta_xml, string ruta_certificado, string clave_certificado, string ruta_servicio_web, string ambiente, ref List<DianWSValidacionPrevia.DianResponse> respuesta_dian)
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

				try
				{
					log_categoria = MensajeCategoria.ServicioDian;
					log_accion = MensajeAccion.envio;

					DianWSValidacionPrevia.WcfDianCustomerServicesClient webServiceHab = new DianWSValidacionPrevia.WcfDianCustomerServicesClient();
					webServiceHab.Endpoint.Address = new System.ServiceModel.EndpointAddress(ruta_servicio_web);
					webServiceHab.ClientCredentials.ClientCertificate.Certificate = cert;

					//Se agrega instruccion para habilitar la seguridad en el envio
					System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

					if (ambiente.Equals("1"))
					{
						acuse_recibo.Version = "2";
						acuse_recibo.ReceivedDateTime = Fecha.GetFecha();

						string archivo = "", carpeta = "";

						//Ejecución de produccion DIAN Enviando archivo ZIP	
						try
						{
							respuesta = webServiceHab.SendBillSync(nombre_archivo, bytes);

							log_categoria = MensajeCategoria.Archivos;
							log_accion = MensajeAccion.creacion;

							carpeta = Path.GetDirectoryName(ruta_zip) + @"\";

							archivo = Path.GetFileNameWithoutExtension(ruta_zip) + ".xml";

							// almacena el mensaje de respuesta del servicio web
							archivo = Xml.GuardarObjeto(respuesta, carpeta, archivo);

						}
						catch (Exception excepcion)
						{
							string msg_custom = string.Format("Error Exec WS => Carpeta: {0} - Archivo: {1}", carpeta, archivo);

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
								string msg_custom = string.Format("Error Close WS Envio => Carpeta: {0} - Archivo: {1}", carpeta, archivo);

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
								if (respuesta.ErrorMessage != null)
								{
									if ((respuesta.ErrorMessage.Length > 0) && (!string.IsNullOrEmpty(respuesta.ErrorMessage.FirstOrDefault())))
									{
										if (respuesta.ErrorMessage.FirstOrDefault().Equals("Regla: 90, Rechazo: Documento procesado anteriormente."))
										{
											respuesta.StatusCode = "94";

											webServiceHab = new DianWSValidacionPrevia.WcfDianCustomerServicesClient();
											webServiceHab.Endpoint.Address =
												new System.ServiceModel.EndpointAddress(ruta_servicio_web);
											webServiceHab.ClientCredentials.ClientCertificate.Certificate = cert;

											string key = respuesta.XmlDocumentKey;

											DianWSValidacionPrevia.DianResponse respuesta_consulta = webServiceHab.GetStatus(key);

											if (respuesta_consulta.XmlBase64Bytes == null && respuesta.XmlBase64Bytes != null)
												respuesta_consulta.XmlBase64Bytes = respuesta.XmlBase64Bytes;

												respuesta = respuesta_consulta;

											string nom_archivo = Path.GetFileNameWithoutExtension(ruta_zip) + ".xml";

											// almacena el mensaje de respuesta de la consulta del servicio web
											archivo = Xml.GuardarObjeto(respuesta, carpeta, nom_archivo);
										}
									}
								}
							}
							catch (Exception excepcion)
							{
								string msg_custom = string.Format("[0] - Error conversion mensaje respuesta DIAN {0}, CUFE: {1}", nombre_archivo, respuesta.XmlDocumentKey);
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
							if (respuesta.StatusCode.Equals("0") || respuesta.StatusCode.Equals("00") || respuesta.StatusCode.Equals("99"))
							{
								if (respuesta.XmlBase64Bytes != null)
								{
									FileStream fs = null;

									try
									{
										//Guardo el Base64 de la Respuesta
										Directorio.CrearDirectorio(ruta_xml);
										using (fs = new FileStream(string.Format(@"{0}\{1}.xml", ruta_xml, nombre_archivo),
											FileMode.Create, FileAccess.ReadWrite))
										{
											BinaryWriter bw = new BinaryWriter(fs, Encoding.Unicode);
											bw.Write(respuesta.XmlBase64Bytes);
											bw.Close();
											fs.Close();
										}

										nombre_archivo = string.Format("{0}-WS", nombre_archivo);

									}
									catch (Exception excepcion)
									{
										respuesta.StatusCode = "94";
										respuesta.ErrorMessage = LibreriaGlobalHGInet.Formato.Coleccion.ConvertirLista("No se guardo respuesta de la DIAN enviando el documento. Por favor no hacer modificaciones al documento y enviarlo de nuevo a la plataforma.").ToArray();
										respuesta.IsValid = false;


										string msg_custom = string.Format("Error FileB64 => Carpeta: {0} - Archivo: {1}", ruta_xml, nombre_archivo);

										RegistroLog.EscribirLog(excepcion, log_categoria, MensajeTipo.Error, log_accion, msg_custom);

									}
									finally
									{
										if (fs != null)
											fs.Close();
									}
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

								if (respuesta.ErrorMessage != null)
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
								respuesta.IsValid = false;
								respuesta_dian.Add(respuesta);
							}
							catch (Exception excepcion)
							{
								string msg_custom = string.Format("[5] - Error conversion mensaje respuesta DIAN {0}", nombre_archivo);
								RegistroLog.EscribirLog(excepcion, log_categoria, MensajeTipo.Error, log_accion, msg_custom);

							}


						}
					}
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
