using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.RegistroLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using HGInetDIANServicios.DianWSValidacionPrevia;

namespace HGInetDIANServicios
{
	public partial class Ctl_ConsultaTransacciones
	{
		/// <summary>
		/// Metodo para consultar el estado de los documentos en V2 en la DIAN
		/// </summary>
		/// <param name="TrackId">Identificaro del documento entregado por la DIAN cuando lo recibio</param>
		/// <param name="ruta_xml">Ruta donde se va a guardar la respuesta</param>
		/// <param name="ruta_certificado">Ruta del certificado utilizado en el proceso</param>
		/// <param name="clave_certificado">Clave del certificado utilizado en el proceso</param>
		/// <param name="ruta_servicio_web">Ruta del Sw de la DIAN</param>
		/// <param name="xml_archivo">Nombre con el que se guarda el documento</param>
		/// <returns></returns>
		public static List<DianWSValidacionPrevia.DianResponse> Consultar_v2(string TrackId, string ruta_xml, string ruta_certificado, string clave_certificado, string ruta_servicio_web, string xml_archivo = "", string cufe = "", bool proceso_acuse = false)
		{

			MensajeCategoria log_categoria = MensajeCategoria.Certificado;
			MensajeAccion log_accion = MensajeAccion.lectura;

			try
			{


				DianWSValidacionPrevia.WcfDianCustomerServicesClient webServiceHab = new DianWSValidacionPrevia.WcfDianCustomerServicesClient();
				webServiceHab.Endpoint.Address = new System.ServiceModel.EndpointAddress(ruta_servicio_web);

				//Certificado de producción
				X509Certificate2 cert = new X509Certificate2(ruta_certificado, clave_certificado);
				webServiceHab.ClientCredentials.ClientCertificate.Certificate = cert;

				//Se agrega instruccion para habilitar la seguridad en el envio
				System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

				log_categoria = MensajeCategoria.ServicioDian;
				log_accion = MensajeAccion.consulta;

				List<DianWSValidacionPrevia.DianResponse> resultado = null;

				if (!string.IsNullOrEmpty(TrackId))
				{
					resultado = webServiceHab.GetStatusZip(TrackId).ToList();
					if (resultado != null && resultado.Count > 0 && string.IsNullOrEmpty(resultado[0].StatusCode))
					{
						if (resultado[0].IsValid == false)
						{
							resultado[0].StatusCode = "99";
						}
						else
						{
							resultado[0].StatusCode = "00";
						}
					}
				}
				else
				{
					if (!string.IsNullOrEmpty(cufe))
					{
						//DianResponse consulta = webServiceHab.GetStatus(cufe);
						DianResponse consulta = null;

						if (proceso_acuse == false)
							consulta = webServiceHab.GetStatus(cufe);
						else
							consulta = webServiceHab.GetStatusEvent(cufe);

						if (consulta != null && string.IsNullOrEmpty(consulta.StatusCode))
						{
							if (resultado[0].IsValid == false)
							{
								resultado[0].StatusCode = "99";
							}
							else
							{
								resultado[0].StatusCode = "00";
							}
						}

						resultado = new List<DianResponse>();
						resultado.Add(consulta);
					}
					else
					{
						log_categoria = MensajeCategoria.ServicioDian;
						log_accion = MensajeAccion.ninguna;
						throw new ApplicationException("No se encontro trackid");	
					}
				}

				if (resultado != null && resultado[0] != null)
					{

						if (string.IsNullOrEmpty(xml_archivo))
							xml_archivo = string.Format("{0}", TrackId);

						//Guardo la respuesta en XML
						foreach (var respuesta in resultado)
						{

							if (respuesta.StatusCode.Equals("0") || respuesta.StatusCode.Equals("00") || respuesta.StatusCode.Equals("99"))
							{
								if (respuesta.XmlBase64Bytes != null)
								{
									//if(string.IsNullOrEmpty(xml_archivo))
									//	xml_archivo = string.Format("{0}", respuesta.XmlFileName);

									FileStream fs = null;

									try
									{
										//Guardo el Base64 de la Respuesta 
										using (fs = new FileStream(string.Format(@"{0}\{1}.xml", ruta_xml, xml_archivo),
											FileMode.Create, FileAccess.ReadWrite))
										{
											BinaryWriter bw = new BinaryWriter(fs, Encoding.Unicode);
											bw.Write(respuesta.XmlBase64Bytes);
										bw.Close();
											fs.Close();
										}

										xml_archivo = string.Format("{0}-WS", xml_archivo);

									}
									catch (Exception excepcion)
									{
										respuesta.StatusCode = "94";
										respuesta.ErrorMessage = LibreriaGlobalHGInet.Formato.Coleccion
											.ConvertirLista(string.Format(
												"No se guardo respuesta de la DIAN consultando el estado del documento,Por favor no hacer modificaciones al documento y enviarlo de nuevo a la plataforma. Radicado:{0}",
												TrackId)).ToArray();
										respuesta.IsValid = false;
										log_categoria = MensajeCategoria.ServicioDian;
										log_accion = MensajeAccion.consulta;
										RegistroLog.EscribirLog(excepcion, log_categoria, MensajeTipo.Error, log_accion);
									}
									finally
									{
										if (fs != null)
											fs.Close();
									}
								}
								else if (respuesta.StatusDescription.Equals("En proceso de validación"))
								{
									respuesta.StatusCode = "94";
									respuesta.ErrorMessage = LibreriaGlobalHGInet.Formato.Coleccion.ConvertirLista(string.Format("No se obtuvo respuesta de la DIAN consultando el estado del documento,Por favor no hacer modificaciones al documento y enviarlo de nuevo a la plataforma. Radicado:{0}", TrackId)).ToArray();
									respuesta.IsValid = false;
								}
							}
							else if (respuesta.IsValid.Equals(false) && !string.IsNullOrEmpty(respuesta.StatusDescription) && !respuesta.StatusCode.Equals("66") && !respuesta.StatusDescription.Contains("Batch en proceso"))
							{
								respuesta.StatusCode = "99";
								respuesta.ErrorMessage = LibreriaGlobalHGInet.Formato.Coleccion.ConvertirLista(string.Format("Se generó inconsistencia en la Plataforma de la DIAN: {0}, Radicado:{1}", respuesta.StatusDescription, TrackId)).ToArray();
							}
							else
							{
								respuesta.StatusCode = "94";
								respuesta.ErrorMessage = LibreriaGlobalHGInet.Formato.Coleccion.ConvertirLista(string.Format("No se obtuvo respuesta de la DIAN consultando el estado del documento,Por favor no hacer modificaciones al documento y enviarlo de nuevo a la plataforma. Radicado:{0}", TrackId)).ToArray();
								respuesta.IsValid = false;
							}

						}

						// Guarda resultado del servicio web de consulta de la DIAN por TrackId
						TextWriter writer = null;
						try
						{
							using (writer = new StreamWriter(string.Format(@"{0}\{1}.xml", ruta_xml, xml_archivo)))
							{
								var ser = new XmlSerializer(typeof(List<DianWSValidacionPrevia.DianResponse>));
								ser.Serialize(writer, resultado);
								writer.Close();
							}
						}
						catch (Exception e)
						{
						}
						finally
						{
							if (writer != null)
								writer.Close();
						}



					}
					else
					{
						log_categoria = MensajeCategoria.ServicioDian;
						log_accion = MensajeAccion.consulta;
						if (resultado[0] == null)
						{
							DianResponse respuesta = new DianResponse();
							respuesta.StatusCode = "94";
							respuesta.ErrorMessage = LibreriaGlobalHGInet.Formato.Coleccion.ConvertirLista(string.Format("No se obtuvo respuesta de la DIAN consultando el estado del documento,Por favor no hacer modificaciones al documento y enviarlo de nuevo a la plataforma. Radicado:{0}",TrackId)).ToArray();
							respuesta.IsValid = false;
							resultado[0]=respuesta;
						}
						else
						{
							throw new ApplicationException("No se obtuvo respuesta de la Plataforma de la DIAN");
						}
					}


				return resultado;

			}
			catch (Exception excepcion)
			{
				RegistroLog.EscribirLog(excepcion, log_categoria, MensajeTipo.Error, log_accion);
				//LogExcepcion.Guardar(excepcion);
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		/// <summary>
		/// Valida la respuesta de la consulta de transacciones
		/// </summary>
		/// <param name="documento">respuesta del servicio web</param>
		/// <returns>validación propia de HGI</returns>
		public static ConsultaDocumento ValidarTransaccionV2(List<DianWSValidacionPrevia.DianResponse> documento)
		{

			MensajeCategoria log_categoria = MensajeCategoria.ServicioDian;
			MensajeAccion log_accion = MensajeAccion.lectura;

			try
			{
				ConsultaDocumento resultado = new ConsultaDocumento();
				resultado.RecepcionDocumento = ValidacionRespuestaDian.Pendiente;
				resultado.Mensaje = "";

				DianWSValidacionPrevia.DianResponse doc_valido = documento.Where(d => d.IsValid == true && (d.StatusCode == "0" || d.StatusCode == "00")).FirstOrDefault();

				if (doc_valido != null)
				{
					resultado.CodigoEstadoDian = doc_valido.StatusCode;
					resultado.EstadoDianDescripcion = doc_valido.StatusDescription;
					resultado.Estado = EstadoDocumentoDian.Aceptado;
					if (doc_valido.ErrorMessage != null)
						resultado.Mensaje = LibreriaGlobalHGInet.Formato.Coleccion.ConvertListToString(doc_valido.ErrorMessage.ToList(), ";");
				}
				else
				{
					DianWSValidacionPrevia.DianResponse doc_pendiente = documento.Where(d => d.IsValid == false && d.StatusCode == "94").FirstOrDefault();

					if (doc_pendiente != null)
					{
						resultado.CodigoEstadoDian = doc_pendiente.StatusCode;
						resultado.EstadoDianDescripcion = "DIAN: En proceso de validación";
						resultado.Estado = EstadoDocumentoDian.Pendiente;
						resultado.Mensaje = LibreriaGlobalHGInet.Formato.Coleccion.ConvertListToString(documento.FirstOrDefault().ErrorMessage.ToList(), ";");
					}
					else
					{

						resultado.CodigoEstadoDian = "99";
						resultado.EstadoDianDescripcion = "validaciones contienen errores en campos mandatorios";
						resultado.Estado = EstadoDocumentoDian.Rechazado;
						resultado.Mensaje = LibreriaGlobalHGInet.Formato.Coleccion.ConvertListToString(documento.FirstOrDefault().ErrorMessage.ToList(), ";");
					}
				}

				return resultado;
			}
			catch (Exception excepcion)
			{
				RegistroLog.EscribirLog(excepcion, log_categoria, MensajeTipo.Error, log_accion);
				//LogExcepcion.Guardar(excepcion);
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}

		}
	}
}
