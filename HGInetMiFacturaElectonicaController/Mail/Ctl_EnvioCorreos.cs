
using HGInetMiFacturaElectonicaController.Auditorias;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.PagosElectronicos;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetMiFacturaElectonicaData.ModeloServicio.General;
using HGInetUBL;
using LibreriaGlobalHGInet.Formato;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.HgiNet.Controladores;
using LibreriaGlobalHGInet.Objetos;
using LibreriaGlobalHGInet.ObjetosComunes.Mensajeria;
using LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.Respuesta;
using LibreriaGlobalHGInet.Peticiones;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using static HGInetMiFacturaElectonicaController.Configuracion.Ctl_Alertas;

namespace HGInetMiFacturaElectonicaController
{
	public class Ctl_EnvioCorreos
	{

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id_seguridad"></param>
		/// <param name="uno_a_uno"></param>
		/// <param name="mensaje"></param>
		/// <param name="asunto"></param>
		/// <param name="contenido_html"></param>
		/// <param name="correo_remitente"></param>
		/// <param name="correos_destino"></param>
		/// <param name="correos_copia"></param>
		/// <param name="correos_copia_oculta"></param>
		/// <param name="ruta_plantilla_html"></param>
		/// <param name="tag_mensaje"></param>
		/// <param name="rutas_adjuntos"></param>
		private List<MensajeEnvio> EnviarEmail(string id_seguridad, bool uno_a_uno, string mensaje, string asunto, bool contenido_html, DestinatarioEmail correo_remitente, List<DestinatarioEmail> correos_destino, List<DestinatarioEmail> correos_copia = null, List<DestinatarioEmail> correos_copia_oculta = null, string ruta_plantilla_html = "", string tag_mensaje = "", List<Adjunto> rutas_adjuntos = null)
		{
			try
			{
				List<MensajeEnvio> respuesta_email = new List<MensajeEnvio>();

				if (correo_remitente == null)
					throw new ApplicationException("No se encontró información del Remitente");
				else if (string.IsNullOrWhiteSpace(correo_remitente.Email))
					throw new ApplicationException("No se encontró información del Remitente");

				if (correos_destino == null && correos_copia == null && correos_copia_oculta == null)
					throw new ApplicationException("No se encontró información del Destinatario");

				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;

				if (plataforma.Mailenvio.Equals("smtp"))
				{
					MailServer configuracion_server = HgiConfiguracion.GetConfiguration().MailServer;

					// convierte las direcciones de los destinatarios
					MailAddressCollection to = ConvertirDestinatarios(correos_destino);

					MailAddressCollection cc = ConvertirDestinatarios(correos_copia);

					MailAddressCollection bcc = ConvertirDestinatarios(correos_copia_oculta);

					MailAddress from = new MailAddress(correo_remitente.Email, correo_remitente.Nombre);

					// construye el correo para el envío
					Mail correo = new Mail(uno_a_uno, from, to, cc, bcc);

					// configura los datos de conexión smtp
					correo.ConfigurarSmtp(configuracion_server.Servidor, configuracion_server.Puerto, configuracion_server.Usuario, configuracion_server.Clave, true);

					// codificación UTF8 (por defecto)
					Encoding codificacion = Encoding.UTF8;

					// envía el correo electrónico
					correo.Enviar(mensaje, asunto, contenido_html, codificacion, tag_mensaje, ruta_plantilla_html, null, true);

				}
				else if (plataforma.Mailenvio.Equals("hgi"))
				{
					List<Destinatario> emails = new List<Destinatario>();

					// From
					Destinatario destino = new Destinatario()
					{
						Email = Constantes.EmailRemitente,
						Nombre = correo_remitente.Nombre,
						Tipo = 0
					};
					emails.Add(destino);

					// Reply-To
					Destinatario respuesta = new Destinatario()
					{
						Email = correo_remitente.Email,
						Nombre = correo_remitente.Nombre,
						Tipo = 4
					};
					emails.Add(respuesta);

					// To
					if (correos_destino != null)
					{
						foreach (DestinatarioEmail item in correos_destino)
						{
							Destinatario mail = new Destinatario()
							{
								Email = item.Email,
								Nombre = item.Nombre,
								Tipo = 1
							};
							emails.Add(mail);
						}
					}

					// CC
					if (correos_copia != null)
					{
						foreach (DestinatarioEmail item in correos_copia)
						{
							Destinatario mail = new Destinatario()
							{
								Email = item.Email,
								Nombre = item.Nombre,
								Tipo = 2
							};
							emails.Add(mail);
						}
					}

					// BCC
					if (correos_copia_oculta != null)
					{
						foreach (DestinatarioEmail item in correos_copia_oculta)
						{
							Destinatario mail = new Destinatario()
							{
								Email = item.Email,
								Nombre = item.Nombre,
								Tipo = 3
							};
							emails.Add(mail);
						}
					}


					MensajeContenido contenido = new MensajeContenido();
					contenido.Id = id_seguridad;
					contenido.Emails = emails;
					contenido.Adjuntos = rutas_adjuntos;
					contenido.Asunto = asunto;
					contenido.ContenidoHtml = mensaje;
					contenido.ContenidoTexto = "";
					contenido.UnoaUno = false;

					List<MensajeContenido> mensajes = new List<MensajeContenido>();
					mensajes.Add(contenido);

					respuesta_email = Ctl_CloudMensajeria.Enviar(plataforma.RutaHginetMail, plataforma.LicenciaHGInetMail, plataforma.IdentificacionHGInetMail, mensajes, plataforma.IdAplicacionHGInetMail);				
				}

				return respuesta_email;

			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Envía mail de bienvenida al portal.
		/// </summary>
		/// <param name="empresa">Datos del Obligado o el Adquiriente</param>
		/// <param name="datos_usuario">datos del usuario</param>
		/// <returns></returns>
		public List<MensajeEnvio> Bienvenida(TblEmpresas empresa, TblUsuarios usuario, string nuevo_email = "")
		{

			try
			{

				//Objeto de respuesta
				List<MensajeEnvio> Mensaje = new List<MensajeEnvio>();

				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;

				string fileName = string.Empty;

				if (empresa == null)
					throw new ApplicationException("No se encontró información de la empresa.");

				if (usuario == null)
					throw new ApplicationException("No se encontró información del usuario");

				if (empresa.IntObligado)
				{
					fileName = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), Constantes.RutaPlantillaBienvenidaObligado);
				}
				else if (empresa.IntAdquiriente)
					fileName = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), Constantes.RutaPlantillaBienvenidaAdquiriente);

				// permite el cambio de contraseña para el usuario
				Ctl_Usuario _usuario = new Ctl_Usuario();
				usuario.DatFechaCambioClave = Fecha.GetFecha();
				usuario.StrIdCambioClave = Guid.NewGuid();
				usuario = _usuario.Actualizar_usuario(usuario);


				if (!string.IsNullOrWhiteSpace(fileName))
				{
					FileInfo file = new FileInfo(fileName);

					string mensaje = file.OpenText().ReadToEnd();

					if (file != null)
					{

						mensaje = mensaje.Replace("{NombreTercero}", empresa.StrRazonSocial);
						mensaje = mensaje.Replace("{NitTercero}", empresa.StrIdentificacion);
						mensaje = mensaje.Replace("{Digitov}", empresa.IntIdentificacionDv.ToString());
						mensaje = mensaje.Replace("{DocumentoIdentificacion}", empresa.StrIdentificacion);
						mensaje = mensaje.Replace("{CodigoUsuario}", usuario.StrUsuario);
						mensaje = mensaje.Replace("{RutaUrl}", string.Format("{0}{1}", plataforma.RutaPublica, Constantes.PaginaRestablecerClave.Replace("{id_seguridad}", usuario.StrIdCambioClave.ToString())));
						mensaje = mensaje.Replace("{RutaAcceso}", plataforma.RutaPublica);

						string asunto = Constantes.AsuntoEmailBienvenida;

						DestinatarioEmail remitente = new DestinatarioEmail();
						remitente.Email = Constantes.EmailRemitente;
						remitente.Nombre = Constantes.NombreRemitenteEmail;

						DestinatarioEmail destinatario = new DestinatarioEmail();
						destinatario.Nombre = string.Format("{0} {1}", usuario.StrNombres, usuario.StrApellidos);
						if (string.IsNullOrWhiteSpace(nuevo_email))
							destinatario.Email = usuario.StrMail;
						else
							destinatario.Email = nuevo_email;

						List<DestinatarioEmail> correos_destino = new List<DestinatarioEmail>();
						correos_destino.Add(destinatario);

						// envía correo electrónico con copia de auditoría
						List<DestinatarioEmail> correos_copia_oculta = null;
						if (empresa.IntObligado && !string.IsNullOrWhiteSpace(Constantes.EmailCopiaOculta))
						{
							correos_copia_oculta = new List<DestinatarioEmail>();

							DestinatarioEmail copia_oculta = new DestinatarioEmail();
							copia_oculta.Nombre = "Auditoría";
							copia_oculta.Email = Constantes.EmailCopiaOculta;
							correos_copia_oculta.Add(copia_oculta);
						}

						Ctl_EnvioCorreos clase_email = new Ctl_EnvioCorreos();

						Mensaje = clase_email.EnviarEmail(empresa.StrIdSeguridad.ToString(), false, mensaje, asunto, true, remitente, correos_destino, null, null, "", "");

					}
				}
				return Mensaje;

			}
			catch (Exception ex)
			{
				throw new ApplicationException(ex.Message);
			}


		}

		/// <summary>
		/// Envia serial de Activacion al Facturador Electronico
		/// </summary>
		/// <param name="identificacion">Nit del Facturador</param>
		/// <param name="mail">email al que se va enviar el correo</param>
		/// <returns></returns>
		public List<MensajeEnvio> EnviaSerial(string identificacion, string mail)
		{
			try
			{
				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;
				List<MensajeEnvio> respuestamail = new List<MensajeEnvio>();

				if (string.IsNullOrEmpty(identificacion))
					throw new ApplicationException("No se encontró información de la empresa.");


				string fileName = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), Constantes.RutaPlantillaEnviarSerial);

				// obtiene los datos del Facturador
				Ctl_Empresa empresa = new Ctl_Empresa();
				TblEmpresas facturador = empresa.Obtener(identificacion);

				if (string.IsNullOrEmpty(facturador.StrSerial))
					throw new ApplicationException("No se encontró información del serial");

				if (!string.IsNullOrWhiteSpace(fileName))
				{
					FileInfo file = new FileInfo(fileName);

					string mensaje = file.OpenText().ReadToEnd();

					if (file != null)
					{

						mensaje = mensaje.Replace("{SerialActivacion}", facturador.StrSerial);

						mensaje = mensaje.Replace("{NombreTercero}", facturador.StrRazonSocial);
						mensaje = mensaje.Replace("{NitTercero}", facturador.StrIdentificacion);
						mensaje = mensaje.Replace("{Digitov}", facturador.IntIdentificacionDv.ToString());
						mensaje = mensaje.Replace("{RutaAcceso}", plataforma.RutaPublica);

						string asunto = Constantes.AsuntoEnviaSerial;

						DestinatarioEmail remitente = new DestinatarioEmail();
						remitente.Email = Constantes.EmailRemitente;
						remitente.Nombre = Constantes.NombreRemitenteEmail;

						DestinatarioEmail destinatario = new DestinatarioEmail();
						destinatario.Nombre = facturador.StrRazonSocial;
						destinatario.Email = mail;

						List<DestinatarioEmail> correos_destino = new List<DestinatarioEmail>();
						correos_destino.Add(destinatario);

						// envía correo electrónico con copia de auditoría
						List<DestinatarioEmail> correos_copia_oculta = null;
						if (!string.IsNullOrWhiteSpace(Constantes.EmailCopiaOculta))
						{
							correos_copia_oculta = new List<DestinatarioEmail>();

							DestinatarioEmail copia_oculta = new DestinatarioEmail();
							copia_oculta.Nombre = "Auditoría";
							copia_oculta.Email = Constantes.EmailCopiaOculta;
							correos_copia_oculta.Add(copia_oculta);
						}

						Ctl_EnvioCorreos clase_email = new Ctl_EnvioCorreos();

						respuestamail = clase_email.EnviarEmail(facturador.StrIdSeguridad.ToString(), false, mensaje, asunto, true, remitente, correos_destino, null, null, "", "");
					}
				}



				return respuestamail;
			}
			catch (Exception ex)
			{
				throw new ApplicationException(ex.Message);
			}
		}

		/// <summary>
		/// Envía mail al Adquiriente con los archivos generados
		/// </summary>
		/// <param name="documento"></param>
		/// <param name="telefono"></param>
		/// <param name="nuevo_email">indica el e-mail del destinatario (si es igual a null, se envía a el email de la empresa del adquiriente)</param>
		/// <returns></returns>
		public List<MensajeEnvio> NotificacionDocumento(TblDocumentos documento, string telefono, string nuevo_email = "", string id_peticion = "", Procedencia procedencia = Procedencia.Plataforma, string usuario = "", ProcesoEstado proceso = ProcesoEstado.EnvioEmailAcuse)
		{
			Ctl_DocumentosAudit clase_auditoria = new Ctl_DocumentosAudit();
			List<MensajeEnvio> respuesta_email = new List<MensajeEnvio>();

			try
			{
				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;

				string ruta_plantilla_html = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), Constantes.RutaPlantillaHtmlDocumentos);

				string asunto = string.Empty;

				// obtiene los datos del facturador electrónico
				Ctl_Empresa facturador_electronico = new Ctl_Empresa();
				TblEmpresas empresa_obligado = facturador_electronico.Obtener(documento.StrEmpresaFacturador);

				if (empresa_obligado.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
				{
					asunto = string.Format("{0} (HABILITACIÓN)", Constantes.AsuntoNotificacionDocumento);
				}
				else
				{
					asunto = Constantes.AsuntoNotificacionDocumento;
				}

				// envía como email de respuesta facturador electrónico
				DestinatarioEmail remitente = new DestinatarioEmail();
				remitente.Nombre = empresa_obligado.StrRazonSocial;
				remitente.Email = empresa_obligado.StrMailEnvio;

				// obtiene los datos del adquiriente
				Ctl_Empresa adquiriente = new Ctl_Empresa();
				TblEmpresas empresa_adquiriente = adquiriente.Obtener(documento.StrEmpresaAdquiriente);

				List<DestinatarioEmail> correos_destino = new List<DestinatarioEmail>();

				// recibe el email el adquiriente
				DestinatarioEmail destinatario = new DestinatarioEmail();
				destinatario.Nombre = empresa_adquiriente.StrRazonSocial;

				if (string.IsNullOrWhiteSpace(nuevo_email))
				{
					destinatario.Email = empresa_adquiriente.StrMailRecepcion;
					correos_destino.Add(destinatario);
				}
				else
				{
					if (nuevo_email.Contains(";"))
					{
						foreach (var item_mail in Coleccion.ConvertirLista(nuevo_email, ';'))
						{
							// recibe el email el adquiriente
							destinatario = new DestinatarioEmail();
							destinatario.Nombre = empresa_adquiriente.StrRazonSocial;
							destinatario.Email = item_mail;
							correos_destino.Add(destinatario);
						}
					}
					else
					{
						destinatario.Email = nuevo_email;
						correos_destino.Add(destinatario);
					}
				}

				// envía correo electrónico con copia de auditoría
				List<DestinatarioEmail> correos_copia_oculta = null;
				if (!string.IsNullOrWhiteSpace(Constantes.EmailCopiaOculta))
				{
					correos_copia_oculta = new List<DestinatarioEmail>();

					DestinatarioEmail copia_oculta = new DestinatarioEmail();
					copia_oculta.Nombre = "Auditoría";
					copia_oculta.Email = Constantes.EmailCopiaOculta;
					correos_copia_oculta.Add(copia_oculta);
				}


				// plantilla Html
				if (!string.IsNullOrWhiteSpace(ruta_plantilla_html))
				{
					FileInfo file = new FileInfo(ruta_plantilla_html);

					string mensaje = file.OpenText().ReadToEnd();


					if (file != null)
					{

						if (empresa_obligado.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
						{
							string div_prueba = "<div style='background:#E7F122;cursor:auto;color:#000000;font-family:Arial, sans-serif;font-size:13px;line-height:24px;text-align:left;'><span style ='font-family:Ubuntufont-size,Helvetica,Arial,sans-serif'><b>Este correo electrónico es exclusivo para pruebas y no tiene ninguna validez comercial y/o de soporte.</b></span></p></div>";

							mensaje = mensaje.Replace("{TextoHabilitacion}", div_prueba);
						}
						else
						{
							mensaje = mensaje.Replace("{TextoHabilitacion}", "");
						}

						// Datos Facturador Electronico
						mensaje = mensaje.Replace("{ImagenLogo}", "<img id='ImgLogo' src='" + "" + "' style='border: none; border-radius: 0px; display: block; outline: none; text-decoration: none; width: 100%; height: auto;' width='233' />");
						mensaje = mensaje.Replace("{NombreFacturador}", empresa_obligado.StrRazonSocial);
						mensaje = mensaje.Replace("{NitFacturador}", empresa_obligado.StrIdentificacion);
						mensaje = mensaje.Replace("{DigitovFacturador}", empresa_obligado.IntIdentificacionDv.ToString());
						mensaje = mensaje.Replace("{EmailFacturador}", empresa_obligado.StrMailEnvio);
						mensaje = mensaje.Replace("{CodigoCUFE}", documento.StrCufe);


						if (!string.IsNullOrWhiteSpace(telefono))
							mensaje = mensaje.Replace("{TelefonoFacturador}", telefono);
						else
							mensaje = mensaje.Replace("{TelefonoFacturador}", "");

						// Datos del Tercero
						if (empresa_adquiriente.StrTipoIdentificacion.Equals("31"))
							mensaje = mensaje.Replace("{TipoPersona}", "Señores");
						else
							mensaje = mensaje.Replace("{TipoPersona}", "Señor (a)");

						mensaje = mensaje.Replace("{NombreTercero}", empresa_adquiriente.StrRazonSocial);
						mensaje = mensaje.Replace("{NitTercero}", empresa_adquiriente.StrIdentificacion);
						mensaje = mensaje.Replace("{Digitov}", empresa_adquiriente.IntIdentificacionDv.ToString());
						mensaje = mensaje.Replace("{CorreoTercero}", empresa_adquiriente.StrMailRecepcion);
						mensaje = mensaje.Replace("{GuidDocumento}", documento.StrIdSeguridad.ToString());


						//Datos del Documento

						// obtiene el título para el tipo de documento
						TipoDocumento doc_tipo = Enumeracion.GetEnumObjectByValue<TipoDocumento>(documento.IntDocTipo);
						string titulo_documento = Enumeracion.GetDescription(doc_tipo);

						mensaje = mensaje.Replace("{TipoDocumento}", titulo_documento);
						mensaje = mensaje.Replace("{NumeroDocumento}", String.Format("{0}{1}", documento.StrPrefijo, documento.IntNumero.ToString()));
						mensaje = mensaje.Replace("{FechaDocumento}", documento.DatFechaDocumento.ToString(Fecha.formato_fecha_hginet));
						mensaje = mensaje.Replace("{TotalDocumento}", String.Format("{0:###,##0.}", documento.IntVlrTotal));

						string ruta_acuse = string.Format("{0}{1}", plataforma.RutaPublica, Constantes.PaginaAcuseRecibo.Replace("{id_seguridad}", documento.StrIdSeguridad.ToString()));

						mensaje = mensaje.Replace("{RutaUrl}", ruta_acuse);
						mensaje = mensaje.Replace("{RutaAcceso}", plataforma.RutaPublica);

						bool IdPago = false;

						if (documento.TblEmpresasResoluciones != null)
							IdPago = (documento.TblEmpresasResoluciones.IntComercioId == null) ? false : (documento.TblEmpresasResoluciones.IntComercioId > 0) ? true : false;

						if (doc_tipo == TipoDocumento.Factura && IdPago)
						{

							string ruta_pse = ruta_acuse + "&Zpago=true";

							mensaje = mensaje.Replace("{PSETexto}", "o el pago del documento");

							string otro_boton = "<td style='border:2px solid #b0afaf;border-radius:3px;color:#ffffff;cursor:auto;padding:10px 25px;' align='center' valign='middle' bgcolor='#040461'><a href='" + ruta_pse + "' style='text-decoration:none;background:#040461;color:#ffffff;font-family:Ubuntu, Helvetica, Arial, sans-serif;font-size:17px;font-weight:normal;line-height:120%;text-transform:none;margin:0px;' target='_blank'>Pagar</a></td>";

							mensaje = mensaje.Replace("{PSE}", otro_boton);

						}

						mensaje = mensaje.Replace("{PSE}", "");
						mensaje = mensaje.Replace("{PSETexto}", "");
						mensaje = mensaje.Replace("{ContenidoPagoPSE}", "");

						List<Adjunto> archivos = new List<Adjunto>();

						if (string.IsNullOrEmpty(documento.StrUrlArchivoPdf))
							throw new ApplicationException("No se encontró ruta de archivo pdf");


						byte[] bytes_pdf = Archivo.ObtenerWeb(documento.StrUrlArchivoPdf);
						string ruta_fisica_pdf = Convert.ToBase64String(bytes_pdf);
						string nombre_pdf = Path.GetFileName(documento.StrUrlArchivoPdf);


						if (!string.IsNullOrEmpty(ruta_fisica_pdf))
						{
							Adjunto adjunto = new Adjunto();
							adjunto.ContenidoB64 = ruta_fisica_pdf;
							adjunto.Nombre = nombre_pdf;
							archivos.Add(adjunto);
						}


						if (string.IsNullOrEmpty(documento.StrUrlArchivoUbl))
							throw new ApplicationException("No se encontró ruta de archivo xml");

						byte[] bytes_xml = Archivo.ObtenerWeb(documento.StrUrlArchivoUbl);
						string ruta_fisica_xml = Convert.ToBase64String(bytes_xml);
						string nombre_xml = Path.GetFileName(documento.StrUrlArchivoUbl);

						if (!string.IsNullOrEmpty(ruta_fisica_xml))
						{
							Adjunto adjunto = new Adjunto();
							adjunto.ContenidoB64 = ruta_fisica_xml;
							adjunto.Nombre = nombre_xml;
							archivos.Add(adjunto);
						}

						//Proceso para los anexos
						if (documento.StrUrlAnexo != null)
						{
							if (!string.IsNullOrEmpty(documento.StrUrlAnexo))
							{
								mensaje = mensaje.Replace("{Anexos}", "Anexos");
								mensaje = mensaje.Replace("{ObservacionAnexos}", documento.StrObservacionAnexo);
								mensaje = mensaje.Replace("{UrlAnexos}", documento.StrUrlAnexo);

								if (documento.IntPesoAnexo > 0)
								{

									byte[] bytes_anexo = Archivo.ObtenerWeb(documento.StrUrlAnexo);
									string ruta_fisica_anexo = Convert.ToBase64String(bytes_anexo);
									string nombre_anexo = Path.GetFileName(documento.StrUrlAnexo);

									if (!string.IsNullOrEmpty(ruta_fisica_anexo))
									{
										Adjunto adjunto = new Adjunto();
										adjunto.ContenidoB64 = ruta_fisica_anexo;
										adjunto.Nombre = nombre_anexo;
										archivos.Add(adjunto);
									}
								}

							}
							else
							{
								mensaje = mensaje.Replace("{Anexos}", "");
								mensaje = mensaje.Replace("{ObservacionAnexos}", "");
								mensaje = mensaje.Replace("{UrlAnexos}", "");
							}
						}
						else
						{
							mensaje = mensaje.Replace("{Anexos}", "");
							mensaje = mensaje.Replace("{ObservacionAnexos}", "");
							mensaje = mensaje.Replace("{UrlAnexos}", "");
						}
						// envía el correo electrónico
						respuesta_email = EnviarEmail(documento.StrIdSeguridad.ToString(), false, mensaje, asunto, true, remitente, correos_destino, null, null, "", "", archivos);
					}
				}
				try
				{
					Guid peticion = (string.IsNullOrEmpty(id_peticion) ? Guid.Empty : Guid.Parse(id_peticion));
					int estado_doc = Ctl_Documento.ObtenerCategoria(documento.IntIdEstado);
					string mensaje = (string.IsNullOrEmpty(id_peticion) ? "Reenvio de Documento" : "Notificación Documento");
					clase_auditoria.Crear(documento.StrIdSeguridad, peticion, empresa_obligado.StrIdentificacion, proceso, TipoRegistro.Proceso, procedencia, usuario, mensaje, string.Empty, respuesta_email, documento.StrPrefijo, Convert.ToString(documento.IntNumero), estado_doc);
				}
				catch (Exception) { throw; }
				return respuesta_email;
			}
			catch (Exception excepcion)
			{
				try
				{
					int estado_doc = Ctl_Documento.ObtenerCategoria(documento.IntIdEstado);
					clase_auditoria.Crear(documento.StrIdSeguridad, Guid.Empty, documento.StrObligadoIdRegistro, proceso, TipoRegistro.Proceso, procedencia, usuario, "No fue posible el envio del documento, favor validar con el Adquiriente ó hacer el reenvío del documento desde nuestra Plataforma ", string.Format("{0}", excepcion.InnerException), documento.StrPrefijo, Convert.ToString(documento.IntNumero), estado_doc);
				}
				catch (Exception) { throw; }

				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}


		}

		/// <summary>
		/// Envía mail al Adquiriente solo con el PDF 
		/// </summary>
		/// <param name="documento"></param>
		/// <param name="telefono"></param>
		/// <param name="nuevo_email">indica el e-mail del destinatario (si es igual a null, se envía a el email de la empresa del adquiriente)</param>
		/// <returns></returns>
		public List<MensajeEnvio> NotificacionBasica(TblDocumentos documento, string telefono, string nuevo_email = "", string id_peticion = "")
		{
			Ctl_DocumentosAudit clase_auditoria = new Ctl_DocumentosAudit();
			List<MensajeEnvio> respuesta_email = new List<MensajeEnvio>();

			try
			{
				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;

				string ruta_plantilla_html = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), Constantes.RutaPlantillaHtmlDocumentosBasica);

				string asunto = string.Empty;

				// obtiene los datos del facturador electrónico
				Ctl_Empresa facturador_electronico = new Ctl_Empresa();
				TblEmpresas empresa_obligado = facturador_electronico.Obtener(documento.StrEmpresaFacturador);

				if (empresa_obligado.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
				{
					asunto = string.Format("{0} (HABILITACIÓN)", Constantes.AsuntoNotificacionDocumento);
				}
				else
				{
					asunto = Constantes.AsuntoNotificacionDocumento;
				}

				// envía como email de respuesta facturador electrónico
				DestinatarioEmail remitente = new DestinatarioEmail();
				remitente.Nombre = empresa_obligado.StrRazonSocial;
				remitente.Email = empresa_obligado.StrMailAdmin;


				// obtiene los datos del adquiriente
				Ctl_Empresa adquiriente = new Ctl_Empresa();
				TblEmpresas empresa_adquiriente = adquiriente.Obtener(documento.StrEmpresaAdquiriente);

				// recibe el email el adquiriente
				DestinatarioEmail destinatario = new DestinatarioEmail();
				destinatario.Nombre = empresa_adquiriente.StrRazonSocial;

				if (string.IsNullOrWhiteSpace(nuevo_email))
					destinatario.Email = empresa_adquiriente.StrMailAdmin;
				else
					destinatario.Email = nuevo_email;

				List<DestinatarioEmail> correos_destino = new List<DestinatarioEmail>();
				correos_destino.Add(destinatario);


				// envía correo electrónico con copia de auditoría
				List<DestinatarioEmail> correos_copia_oculta = null;
				if (!string.IsNullOrWhiteSpace(Constantes.EmailCopiaOculta))
				{
					correos_copia_oculta = new List<DestinatarioEmail>();

					DestinatarioEmail copia_oculta = new DestinatarioEmail();
					copia_oculta.Nombre = "Auditoría";
					copia_oculta.Email = Constantes.EmailCopiaOculta;
					correos_copia_oculta.Add(copia_oculta);
				}


				// plantilla Html
				if (!string.IsNullOrWhiteSpace(ruta_plantilla_html))
				{
					FileInfo file = new FileInfo(ruta_plantilla_html);

					string mensaje = file.OpenText().ReadToEnd();


					if (file != null)
					{

						if (empresa_obligado.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
						{
							string div_prueba = "<div style='background:#E7F122;cursor:auto;color:#000000;font-family:Arial, sans-serif;font-size:13px;line-height:24px;text-align:left;'><span style ='font-family:Ubuntufont-size,Helvetica,Arial,sans-serif'><b>Este correo electrónico es exclusivo para pruebas y no tiene ninguna validez comercial y/o de soporte.</b></span></p></div>";

							mensaje = mensaje.Replace("{TextoHabilitacion}", div_prueba);
						}
						else
						{
							mensaje = mensaje.Replace("{TextoHabilitacion}", "");
						}

						// Datos Facturador Electronico
						mensaje = mensaje.Replace("{ImagenLogo}", "<img id='ImgLogo' src='" + "" + "' style='border: none; border-radius: 0px; display: block; outline: none; text-decoration: none; width: 100%; height: auto;' width='233' />");
						mensaje = mensaje.Replace("{NombreFacturador}", empresa_obligado.StrRazonSocial);
						mensaje = mensaje.Replace("{NitFacturador}", empresa_obligado.StrIdentificacion);
						mensaje = mensaje.Replace("{DigitovFacturador}", empresa_obligado.IntIdentificacionDv.ToString());
						mensaje = mensaje.Replace("{EmailFacturador}", empresa_obligado.StrMailAdmin);


						if (!string.IsNullOrWhiteSpace(telefono))
							mensaje = mensaje.Replace("{TelefonoFacturador}", telefono);
						else
							mensaje = mensaje.Replace("{TelefonoFacturador}", "");

						// Datos del Tercero
						if (empresa_adquiriente.StrTipoIdentificacion.Equals("31"))
							mensaje = mensaje.Replace("{TipoPersona}", "Señores");
						else
							mensaje = mensaje.Replace("{TipoPersona}", "Señor (a)");

						mensaje = mensaje.Replace("{NombreTercero}", empresa_adquiriente.StrRazonSocial);
						mensaje = mensaje.Replace("{NitTercero}", empresa_adquiriente.StrIdentificacion);
						mensaje = mensaje.Replace("{Digitov}", empresa_adquiriente.IntIdentificacionDv.ToString());
						mensaje = mensaje.Replace("{CorreoTercero}", empresa_adquiriente.StrMailAdmin);
						mensaje = mensaje.Replace("{GuidDocumento}", documento.StrIdSeguridad.ToString());


						//Datos del Documento

						// obtiene el título para el tipo de documento
						TipoDocumento doc_tipo = Enumeracion.GetEnumObjectByValue<TipoDocumento>(documento.IntDocTipo);
						string titulo_documento = Enumeracion.GetDescription(doc_tipo);

						mensaje = mensaje.Replace("{TipoDocumento}", titulo_documento);
						mensaje = mensaje.Replace("{NumeroDocumento}", String.Format("{0}{1}", documento.StrPrefijo, documento.IntNumero.ToString()));
						mensaje = mensaje.Replace("{FechaDocumento}", documento.DatFechaDocumento.ToString(Fecha.formato_fecha_hginet));
						mensaje = mensaje.Replace("{TotalDocumento}", String.Format("{0:###,##0.}", documento.IntVlrTotal));
						mensaje = mensaje.Replace("{CodigoCUFE}", documento.StrCufe);

						string ruta_acuse = string.Format("{0}{1}", plataforma.RutaPublica, Constantes.PaginaAcuseRecibo.Replace("{id_seguridad}", documento.StrIdSeguridad.ToString()));

						mensaje = mensaje.Replace("{RutaUrl}", string.Format("{0}/Views/Login/Default.aspx?nit={1}", plataforma.RutaPublica, empresa_adquiriente.StrIdentificacion));
						mensaje = mensaje.Replace("{RutaAcceso}", plataforma.RutaPublica);

						bool IdPago = false;

						if (documento.TblEmpresasResoluciones != null)
							IdPago = (documento.TblEmpresasResoluciones.IntComercioId == null) ? false : (documento.TblEmpresasResoluciones.IntComercioId > 0) ? true : false;

						if (doc_tipo == TipoDocumento.Factura && IdPago)
						{

							string ruta_pse = ruta_acuse + "&Zpago=true";

							mensaje = mensaje.Replace("{PSETexto}", "Para realizar el pago del documento presione el botón");

							string otro_boton = "<td style='border:2px solid #b0afaf;border-radius:3px;color:#ffffff;cursor:auto;padding:10px 25px;' align='center' valign='middle' bgcolor='#040461'><a href='" + ruta_pse + "' style='text-decoration:none;background:#040461;color:#ffffff;font-family:Ubuntu, Helvetica, Arial, sans-serif;font-size:17px;font-weight:normal;line-height:120%;text-transform:none;margin:0px;' target='_blank'>Pagar</a></td>";

							mensaje = mensaje.Replace("{PSE}", otro_boton);

						}

						mensaje = mensaje.Replace("{PSE}", "");
						mensaje = mensaje.Replace("{PSETexto}", "");
						mensaje = mensaje.Replace("{ContenidoPagoPSE}", "");

						List<Adjunto> archivos = new List<Adjunto>();

						if (string.IsNullOrEmpty(documento.StrUrlArchivoPdf))
							throw new ApplicationException("No se encontró ruta de archivo pdf");


						byte[] bytes_pdf = Archivo.ObtenerWeb(documento.StrUrlArchivoPdf);
						string ruta_fisica_pdf = Convert.ToBase64String(bytes_pdf);
						string nombre_pdf = Path.GetFileName(documento.StrUrlArchivoPdf);


						if (!string.IsNullOrEmpty(ruta_fisica_pdf))
						{
							Adjunto adjunto = new Adjunto();
							adjunto.ContenidoB64 = ruta_fisica_pdf;
							adjunto.Nombre = nombre_pdf;
							archivos.Add(adjunto);
						}

						//Proceso para los anexos
						if (documento.StrUrlAnexo != null)
						{
							if (!string.IsNullOrEmpty(documento.StrUrlAnexo))
							{
								mensaje = mensaje.Replace("{Anexos}", "Anexos");
								mensaje = mensaje.Replace("{ObservacionAnexos}", documento.StrObservacionAnexo);
								mensaje = mensaje.Replace("{UrlAnexos}", documento.StrUrlAnexo);

								if (documento.IntPesoAnexo > 0)
								{

									byte[] bytes_anexo = Archivo.ObtenerWeb(documento.StrUrlAnexo);
									string ruta_fisica_anexo = Convert.ToBase64String(bytes_anexo);
									string nombre_anexo = Path.GetFileName(documento.StrUrlAnexo);

									if (!string.IsNullOrEmpty(ruta_fisica_anexo))
									{
										Adjunto adjunto = new Adjunto();
										adjunto.ContenidoB64 = ruta_fisica_anexo;
										adjunto.Nombre = nombre_anexo;
										archivos.Add(adjunto);
									}
								}

							}
							else
							{
								mensaje = mensaje.Replace("{Anexos}", "");
								mensaje = mensaje.Replace("{ObservacionAnexos}", "");
								mensaje = mensaje.Replace("{UrlAnexos}", "");
							}
						}
						else
						{
							mensaje = mensaje.Replace("{Anexos}", "");
							mensaje = mensaje.Replace("{ObservacionAnexos}", "");
							mensaje = mensaje.Replace("{UrlAnexos}", "");
						}
						// envía el correo electrónico
						respuesta_email = EnviarEmail(documento.StrIdSeguridad.ToString(), false, mensaje, asunto, true, remitente, correos_destino, null, null, "", "", archivos);

					}
				}

				try
				{
					Guid peticion = (string.IsNullOrEmpty(id_peticion) ? Guid.Empty : Guid.Parse(id_peticion));
					string mensaje = (string.IsNullOrEmpty(id_peticion) ? "Reenvio de Documento" : "Notificación Documento");
					clase_auditoria.Crear(documento.StrIdSeguridad, peticion, empresa_obligado.StrIdentificacion, Enumeracion.GetEnumObjectByValue<ProcesoEstado>(documento.IntIdEstado), TipoRegistro.Proceso, Procedencia.Mail, string.Empty, mensaje, string.Empty, respuesta_email, documento.StrPrefijo, Convert.ToString(documento.IntNumero));
					//clase_auditoria.Crear(documento.StrIdSeguridad, Guid.Empty, documento.StrObligadoIdRegistro, Enumeracion.GetEnumObjectByValue<ProcesoEstado>(documento.IntIdEstado), Enumeracion.GetEnumObjectByValue<CategoriaEstado>(documento.IdCategoriaEstado), TipoRegistro.Proceso, Procedencia.Mail, string.Empty, "NotificacionBasica", string.Empty, respuesta_email, documento.StrPrefijo, Convert.ToString(documento.IntNumero));
				}
				catch (Exception) { throw; }

				return respuesta_email;

			}
			catch (Exception excepcion)
			{
				try
				{
					clase_auditoria.Crear(documento.StrIdSeguridad, Guid.Empty, documento.StrObligadoIdRegistro, Enumeracion.GetEnumObjectByValue<ProcesoEstado>(documento.IntIdEstado), TipoRegistro.Proceso, Procedencia.Mail, string.Empty, string.Format("NotificacionBasica - {0}", excepcion.Message), string.Format("{0}", excepcion.InnerException), documento.StrPrefijo, Convert.ToString(documento.IntNumero));
				}
				catch (Exception) { throw; }

				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}


		}

		/// <summary>
		/// Envía mail para restablecer contraseña
		/// </summary>
		/// <param name="empresa">Datos del Obligado o del Adquiriente</param>
		/// <param name="usuario">Datos del usuario guardado en BD</param>
		public void RestablecerClave(TblEmpresas empresa, TblUsuarios usuario)
		{

			try
			{
				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;

				if (empresa == null)
					throw new ApplicationException(string.Format("No se encontró información del Nit {0} ", empresa.StrIdentificacion));

				if (usuario == null)
					throw new ApplicationException(string.Format("No se encontró información del usuario {0}", usuario));

				string fileName = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), Constantes.RutaPlantillaRestablecer);

				if (!string.IsNullOrWhiteSpace(fileName))
				{
					FileInfo file = new FileInfo(fileName);

					string mensaje = file.OpenText().ReadToEnd();

					if (file != null)
					{
						mensaje = mensaje.Replace("{NombreTercero}", empresa.StrRazonSocial);
						mensaje = mensaje.Replace("{NitTercero}", empresa.StrIdentificacion);
						mensaje = mensaje.Replace("{Digitov}", empresa.IntIdentificacionDv.ToString());
						mensaje = mensaje.Replace("{NombresUsuario}", usuario.StrNombres);
						mensaje = mensaje.Replace("{ApellidosUsuario}", usuario.StrApellidos);
						mensaje = mensaje.Replace("{CodigoUsuario}", usuario.StrUsuario);
						mensaje = mensaje.Replace("{RutaUrl}", string.Format("{0}{1}", plataforma.RutaPublica, Constantes.PaginaRestablecerClave.Replace("{id_seguridad}", usuario.StrIdCambioClave.ToString())));
						mensaje = mensaje.Replace("{RutaAcceso}", plataforma.RutaPublica);

						string asunto = "Restablecimiento de Contraseña";

						DestinatarioEmail remitente = new DestinatarioEmail();
						remitente.Email = Constantes.EmailRemitente;
						remitente.Nombre = Constantes.NombreRemitenteEmail;

						DestinatarioEmail destinatario = new DestinatarioEmail();
						destinatario.Email = usuario.StrMail;
						destinatario.Nombre = string.Format("{0} {1}", usuario.StrNombres, usuario.StrApellidos);

						List<DestinatarioEmail> correos_destino = new List<DestinatarioEmail>();
						correos_destino.Add(destinatario);

						Ctl_EnvioCorreos clase_email = new Ctl_EnvioCorreos();

						clase_email.EnviarEmail(empresa.StrIdSeguridad.ToString(), false, mensaje, asunto, true, remitente, correos_destino, null, null, "", "");

					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// Envía acuse de recibo al Obligado
		/// </summary>
		/// <param name="documento">Datos del documento</param>
		/// <returns></returns>
		public List<MensajeEnvio> RespuestaAcuse(TblDocumentos documento, TblEmpresas facturador, TblEmpresas adquiriente, string ruta_archivo, string mail = "", Procedencia procedencia = Procedencia.Plataforma, string usuario = "")
		{
			Ctl_DocumentosAudit clase_auditoria = new Ctl_DocumentosAudit();
			List<MensajeEnvio> respuesta_email = new List<MensajeEnvio>();

			try
			{
				string ruta_plantilla_html = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), Constantes.RutaPlantillaAcuse);

				string asunto = "Respuesta Acuse de Recibo";

				// envía como email de respuesta el Adquiriente
				DestinatarioEmail remitente = new DestinatarioEmail();
				remitente.Nombre = adquiriente.StrRazonSocial;
				remitente.Email = Constantes.EmailRemitente; //(!string.IsNullOrEmpty(adquiriente.StrMailEnvio) ? adquiriente.StrMailEnvio : adquiriente.StrMailAdmin);

				//// recibe el email el Facturador Electrónico
				//DestinatarioEmail destinatario = new DestinatarioEmail();
				//if (mail == "")
				//{
				//	destinatario.Nombre = facturador.StrRazonSocial;
				//	destinatario.Email = facturador.StrMailAdmin;
				//}
				//else
				//{
				//	destinatario.Nombre = mail;
				//	destinatario.Email = mail;
				//}


				//correos_destino.Add(destinatario);

				// recibe el email el adquiriente
				List<DestinatarioEmail> correos_destino = new List<DestinatarioEmail>();
				DestinatarioEmail destinatario = new DestinatarioEmail();
				destinatario.Nombre = facturador.StrRazonSocial;

				if (string.IsNullOrWhiteSpace(mail))
				{
					destinatario.Email = facturador.StrMailAcuse;
					correos_destino.Add(destinatario);
				}
				else
				{
					if (mail.Contains(";"))
					{
						foreach (var item_mail in Coleccion.ConvertirLista(mail, ';'))
						{
							// recibe el email el adquiriente
							destinatario = new DestinatarioEmail();
							destinatario.Nombre = facturador.StrRazonSocial;
							destinatario.Email = item_mail;
							correos_destino.Add(destinatario);
						}
					}
					else
					{
						destinatario.Email = mail;
						correos_destino.Add(destinatario);
					}
				}


				// plantilla Html
				if (!string.IsNullOrWhiteSpace(ruta_plantilla_html))
				{
					FileInfo file = new FileInfo(ruta_plantilla_html);

					string mensaje = file.OpenText().ReadToEnd();
					if (file != null)
					{
						// Datos Facturador Electronico
						mensaje = mensaje.Replace("{ImagenLogo}", "<img id='ImgLogo' src='" + "" + "' style='border: none; border-radius: 0px; display: block; outline: none; text-decoration: none; width: 100%; height: auto;' width='233' />");
						mensaje = mensaje.Replace("{NombreFacturador}", facturador.StrRazonSocial);
						mensaje = mensaje.Replace("{NitFacturador}", facturador.StrIdentificacion);

						// Datos del Tercero
						mensaje = mensaje.Replace("{NombreTercero}", adquiriente.StrRazonSocial);
						mensaje = mensaje.Replace("{NitTercero}", adquiriente.StrIdentificacion);
						mensaje = mensaje.Replace("{EmailTercero}", adquiriente.StrMailEnvio);
						mensaje = mensaje.Replace("{GuidDocumento}", documento.StrIdSeguridad.ToString());


						//Datos del Documento
						string titulo_factura = string.Empty;

						switch (documento.IntDocTipo)
						{
							case 1:
								titulo_factura = "Factura";
								break;
							case 2:
								titulo_factura = "Nota Débito";
								break;
							case 3:
								titulo_factura = "Nota Crédito";
								break;
						}

						string estado_respuesta = string.Empty;

						switch (documento.IntAdquirienteRecibo)
						{
							case 1:
								estado_respuesta = "Aprobada";
								break;
							case 2:
								estado_respuesta = "Rechazada";
								break;
							case 3:
								estado_respuesta = "Aprobado Tácito";
								break;
						}

						if (documento.IntAdquirienteRecibo == 1 && !string.IsNullOrWhiteSpace(documento.StrAdquirienteMvoRechazo))
							mensaje = mensaje.Replace("{MotivoRechazo}", "Observaciones: " + documento.StrAdquirienteMvoRechazo);
						else if (documento.IntAdquirienteRecibo == 2)
							mensaje = mensaje.Replace("{MotivoRechazo}", "Motivo rechazo: " + documento.StrAdquirienteMvoRechazo);
						else
							mensaje = mensaje.Replace("{MotivoRechazo}", " ");

						mensaje = mensaje.Replace("{TipoDocumento}", titulo_factura);
						mensaje = mensaje.Replace("{NumeroDocumento}", string.Format("{0}{1}", documento.StrPrefijo, documento.IntNumero.ToString()));
						mensaje = mensaje.Replace("{Estadorespuesta}", estado_respuesta);
						mensaje = mensaje.Replace("{FechaDocumento}", documento.DatAdquirienteFechaRecibo.Value.ToString(Fecha.formato_fecha_hora));


						List<Adjunto> archivos = new List<Adjunto>();

						if (string.IsNullOrEmpty(ruta_archivo))
							throw new ApplicationException("No se encontró ruta de archivo xml");

						byte[] bytes_xml = Archivo.ObtenerWeb(ruta_archivo);
						string ruta_fisica_xml = Convert.ToBase64String(bytes_xml);
						string nombre_xml = Path.GetFileName(ruta_archivo);

						if (!string.IsNullOrEmpty(ruta_fisica_xml))
						{
							Adjunto adjunto = new Adjunto();
							adjunto.ContenidoB64 = ruta_fisica_xml;
							adjunto.Nombre = nombre_xml;
							archivos.Add(adjunto);
						}

						// envía el correo electrónico
						respuesta_email = EnviarEmail(documento.StrIdSeguridad.ToString(), false, mensaje, asunto, true, remitente, correos_destino, null, null, "", "", archivos);

						try
						{
							Guid peticion = Guid.Empty;
							string strmensaje = asunto;
							int estado = Ctl_Documento.ObtenerCategoria(documento.IntIdEstado);
							clase_auditoria.Crear(documento.StrIdSeguridad, peticion, facturador.StrIdentificacion, ProcesoEstado.EnvioRespuestaAcuse, TipoRegistro.Proceso, procedencia, usuario, Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.ProcesoEstado>(ProcesoEstado.EnvioRespuestaAcuse.GetHashCode())), string.Empty, respuesta_email, documento.StrPrefijo, Convert.ToString(documento.IntNumero), estado);
						}
						catch (Exception) { throw; }
					}
				}

				return respuesta_email;
			}
			catch (Exception excepcion)
			{
				try
				{
					int estado = Ctl_Documento.ObtenerCategoria(documento.IntIdEstado);
					clase_auditoria.Crear(documento.StrIdSeguridad, Guid.Empty, documento.StrObligadoIdRegistro, Enumeracion.GetEnumObjectByValue<ProcesoEstado>(documento.IntIdEstado), TipoRegistro.Proceso, Procedencia.Mail, string.Empty, string.Format("RespuestaAcuse - {0}", excepcion.Message), string.Format("{0}", excepcion.InnerException), documento.StrPrefijo, Convert.ToString(documento.IntNumero), estado);
				}
				catch (Exception) { throw; }

				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}

			try
			{
				clase_auditoria.Crear(documento.StrIdSeguridad, Guid.Empty, documento.StrObligadoIdRegistro, Enumeracion.GetEnumObjectByValue<ProcesoEstado>(documento.IntIdEstado), TipoRegistro.Proceso, Procedencia.Mail, string.Empty, "RespuestaAcuse", string.Empty, respuesta_email, documento.StrPrefijo, Convert.ToString(documento.IntNumero));
			}
			catch (Exception) { throw; }

		}

		/// <summary>
		/// Convierte los correos electrónicos de objeto a MailAddressCollection
		/// </summary>
		/// <param name="correos_destino">correos electrónicos</param>
		/// <returns>correos electrónicos</returns>
		public static MailAddressCollection ConvertirDestinatarios(List<DestinatarioEmail> correos_destino)
		{
			try
			{
				MailAddressCollection destinatarios = new MailAddressCollection();

				if (correos_destino != null)
				{
					foreach (DestinatarioEmail item in correos_destino)
					{
						if (!string.IsNullOrWhiteSpace(item.Email))
						{
							destinatarios.Add(new MailAddress(item.Email, item.Nombre));
						}
					}
				}
				return destinatarios;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}

		}

		/// <summary>
		/// Envia Correo Electronico con información de recarga
		/// </summary>
		/// <param name="identificacion">Nit del Facturador</param>
		/// <param name="mail">email al que se va enviar el correo</param>
		/// <returns></returns>
		public List<MensajeEnvio> EnviaNotificacionRecarga(string identificacion, string mail, TblPlanesTransacciones plan)
		{
			try
			{
				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;
				List<MensajeEnvio> RespuestaMail = new List<MensajeEnvio>();

				if (string.IsNullOrEmpty(identificacion))
					throw new ApplicationException("No se encontró información de la empresa.");


				string fileName = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), Constantes.RutaPlantillaRecarga);

				// obtiene los datos del Facturador
				Ctl_Empresa empresa = new Ctl_Empresa();
				TblEmpresas facturador = empresa.Obtener(identificacion);

				//if (string.IsNullOrEmpty(facturador.StrSerial))
				//    throw new ApplicationException("No se encontró información del serial");

				if (!string.IsNullOrWhiteSpace(fileName))
				{
					FileInfo file = new FileInfo(fileName);

					string mensaje = file.OpenText().ReadToEnd();

					if (file != null)
					{
						if (facturador.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
						{
							string div_prueba = "<div style='background:#E7F122;cursor:auto;color:#000000;font-family:Arial, sans-serif;font-size:13px;line-height:24px;text-align:left;'><span style ='font-family:Ubuntufont-size,Helvetica,Arial,sans-serif'><b>Este correo electrónico es exclusivo para pruebas y no tiene ninguna validez comercial y/o de soporte.</b></span></p></div>";

							mensaje = mensaje.Replace("{TextoHabilitacion}", div_prueba);
						}
						else
						{
							mensaje = mensaje.Replace("{TextoHabilitacion}", "");
						}

						// Datos del Tercero
						if (facturador.StrTipoIdentificacion.Equals("31"))
							mensaje = mensaje.Replace("{TipoPersona}", "Señores");
						else
							mensaje = mensaje.Replace("{TipoPersona}", "Señor (a)");

						mensaje = mensaje.Replace("{NombreTercero}", facturador.StrRazonSocial);
						mensaje = mensaje.Replace("{NitTercero}", facturador.StrIdentificacion);
						mensaje = mensaje.Replace("{Digitov}", facturador.IntIdentificacionDv.ToString());
						mensaje = mensaje.Replace("{RutaAcceso}", plataforma.RutaPublica);

						mensaje = mensaje.Replace("{Tipo}", (Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoCompra>(plan.IntTipoProceso))));
						mensaje = mensaje.Replace("{Estado}", (Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<EstadoPlan>(plan.IntEstado))));
						mensaje = mensaje.Replace("{Costo}", plan.IntValor.ToString("C"));
						mensaje = mensaje.Replace("{Transacciones}", plan.IntNumTransaccCompra.ToString("N0"));
						mensaje = mensaje.Replace("{Meses}", (plan.IntMesesVence > 0) ? string.Format("<td class='tg-yzt1'>Vencimiento:</td> <td class='tg-3we0'>Esta recarga tiene {0} meses de vigencia a partir de la recepción del primer documento que consuma la misma.</td>", plan.IntMesesVence.ToString()) : "");
																							
						string asunto = "RECARGA DE SALDO DE DOCUMENTOS ELECTRÓNICOS";

						DestinatarioEmail remitente = new DestinatarioEmail();
						remitente.Email = Constantes.EmailRemitente;
						remitente.Nombre = Constantes.NombreRemitenteEmail;

						DestinatarioEmail destinatario = new DestinatarioEmail();
						destinatario.Nombre = facturador.StrRazonSocial;
						destinatario.Email = mail;

						List<DestinatarioEmail> correos_destino = new List<DestinatarioEmail>();
						correos_destino.Add(destinatario);

						// envía correo electrónico con copia de auditoría
						List<DestinatarioEmail> correos_copia_oculta = null;
						if (!string.IsNullOrWhiteSpace(Constantes.EmailCopiaOculta))
						{
							correos_copia_oculta = new List<DestinatarioEmail>();

							DestinatarioEmail copia_oculta = new DestinatarioEmail();
							copia_oculta.Nombre = "Auditoría";
							copia_oculta.Email = Constantes.EmailCopiaOculta;
							correos_copia_oculta.Add(copia_oculta);
						}

						Ctl_EnvioCorreos clase_email = new Ctl_EnvioCorreos();

						RespuestaMail = clase_email.EnviarEmail(facturador.StrIdSeguridad.ToString(), false, mensaje, asunto, true, remitente, correos_destino, null, null, "", "");

					}
				}

				return RespuestaMail;
			}
			catch (Exception ex)
			{
				throw new ApplicationException(ex.Message);
			}
		}




		/// <summary>
		/// Envia Correo Electronico con información sobre alguna alerta, puede ser al facturador, un usuario de algún facturador o a personal de HGI
		/// </summary>
		/// <param name="identificacion">Nit del Facturador</param>
		/// <param name="mail">email al que se va enviar el correo</param>
		/// <returns></returns>
		public List<MensajeEnvio> EnviaNotificacionAlerta(string identificacion, string mail, double adquiridos, double procesados, double disponibles, double Porcentaje)
		{
			try
			{
				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;
				List<MensajeEnvio> RespuestaMail = new List<MensajeEnvio>();

				if (string.IsNullOrEmpty(identificacion))
					throw new ApplicationException("No se encontró información de la empresa.");


				string fileName = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), Constantes.RutaPlantillaPlanPorcentaje);

				// obtiene los datos del Facturador
				Ctl_Empresa empresa = new Ctl_Empresa();
				TblEmpresas facturador = empresa.Obtener(identificacion);

				//if (string.IsNullOrEmpty(facturador.StrSerial))
				//    throw new ApplicationException("No se encontró información del serial");

				if (!string.IsNullOrWhiteSpace(fileName))
				{
					FileInfo file = new FileInfo(fileName);

					string mensaje = file.OpenText().ReadToEnd();

					if (file != null)
					{
						CultureInfo elGR = CultureInfo.CreateSpecificCulture("el-GR");

						if (facturador.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
						{
							string div_prueba = "<div style='background:#E7F122;cursor:auto;color:#000000;font-family:Arial, sans-serif;font-size:13px;line-height:24px;text-align:left;'><span style ='font-family:Ubuntufont-size,Helvetica,Arial,sans-serif'><b>Este correo electrónico es exclusivo para pruebas y no tiene ninguna validez comercial y/o de soporte.</b></span></p></div>";

							mensaje = mensaje.Replace("{TextoHabilitacion}", div_prueba);
						}
						else
						{
							mensaje = mensaje.Replace("{TextoHabilitacion}", "");
						}

						// Datos del Tercero
						if (facturador.StrTipoIdentificacion.Equals("31"))
							mensaje = mensaje.Replace("{TipoPersona}", "Señores");
						else
							mensaje = mensaje.Replace("{TipoPersona}", "Señor (a)");


						mensaje = mensaje.Replace("{NombreTercero}", facturador.StrRazonSocial);
						mensaje = mensaje.Replace("{NitTercero}", facturador.StrIdentificacion);
						mensaje = mensaje.Replace("{Digitov}", facturador.IntIdentificacionDv.ToString());
						mensaje = mensaje.Replace("{RutaAcceso}", plataforma.RutaPublica);

						mensaje = mensaje.Replace("{Adquiridos}", adquiridos.ToString());
						mensaje = mensaje.Replace("{Procesados}", procesados.ToString());
						mensaje = mensaje.Replace("{Disponibles}", disponibles.ToString());
						mensaje = mensaje.Replace("{Porcentaje}", string.Format("{0}%", Porcentaje.ToString()));


						string asunto = "ALERTA CONSUMO DE DOCUMENTOS ELECTRÓNICOS";

						DestinatarioEmail remitente = new DestinatarioEmail();
						remitente.Email = Constantes.EmailRemitente;
						remitente.Nombre = Constantes.NombreRemitenteEmail;

						DestinatarioEmail destinatario = new DestinatarioEmail();
						List<DestinatarioEmail> correos_destino = new List<DestinatarioEmail>();
						if (mail.Contains(";"))
						{
							foreach (var item_mail in Coleccion.ConvertirLista(mail, ';'))
							{
								// recibe el email el adquiriente
								destinatario = new DestinatarioEmail();
								destinatario.Nombre = facturador.StrRazonSocial;
								destinatario.Email = item_mail;
								correos_destino.Add(destinatario);
							}
						}
						else
						{
							destinatario.Nombre = facturador.StrRazonSocial;
							destinatario.Email = mail;
						}




						correos_destino.Add(destinatario);


						// envía correo electrónico con copia de auditoría
						List<DestinatarioEmail> correos_copia_oculta = null;
						if (!string.IsNullOrWhiteSpace(Constantes.EmailCopiaOculta))
						{
							correos_copia_oculta = new List<DestinatarioEmail>();

							DestinatarioEmail copia_oculta = new DestinatarioEmail();
							copia_oculta.Nombre = "Auditoría";
							copia_oculta.Email = Constantes.EmailCopiaOculta;
							correos_copia_oculta.Add(copia_oculta);
						}

						Ctl_EnvioCorreos clase_email = new Ctl_EnvioCorreos();

						RespuestaMail = clase_email.EnviarEmail(facturador.StrIdSeguridad.ToString(), false, mensaje, asunto, true, remitente, correos_destino, null, null, "", "");

					}
				}

				return RespuestaMail;
			}
			catch (Exception ex)
			{
				throw new ApplicationException(ex.Message);
			}

		}

		/// <summary>
		/// Envia Correo Electronico con información sobre alguna alerta, puede ser al facturador, un usuario de algún  a personal de HGI
		/// </summary>
		/// <param name="identificacion">Nit del Facturador</param>
		/// <param name="mail">email al que se va enviar el correo</param>
		/// <returns></returns>
		public List<MensajeEnvio> EnviaNotificacionAlertaConsumoHGI(string identificacion, string mail, double adquiridos, double procesados, double disponibles, double Porcentaje)
		{
			try
			{
				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;
				List<MensajeEnvio> RespuestaMail = new List<MensajeEnvio>();

				if (string.IsNullOrEmpty(identificacion))
					throw new ApplicationException("No se encontró información de la empresa.");


				string fileName = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), Constantes.RutaPlantillaPlanPorcentajehgi);

				// obtiene los datos del Facturador
				Ctl_Empresa empresa = new Ctl_Empresa();
				TblEmpresas facturador = empresa.Obtener(identificacion);

				if (!string.IsNullOrWhiteSpace(fileName))
				{
					FileInfo file = new FileInfo(fileName);

					string mensaje = file.OpenText().ReadToEnd();

					if (file != null)
					{
						CultureInfo elGR = CultureInfo.CreateSpecificCulture("el-GR");

						if (facturador.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
						{
							string div_prueba = "<div style='background:#E7F122;cursor:auto;color:#000000;font-family:Arial, sans-serif;font-size:13px;line-height:24px;text-align:left;'><span style ='font-family:Ubuntufont-size,Helvetica,Arial,sans-serif'><b>Este correo electrónico es exclusivo para pruebas y no tiene ninguna validez comercial y/o de soporte.</b></span></p></div>";

							mensaje = mensaje.Replace("{TextoHabilitacion}", div_prueba);
						}
						else
						{
							mensaje = mensaje.Replace("{TextoHabilitacion}", "");
						}

						// Datos del Tercero
						if (facturador.StrTipoIdentificacion.Equals("31"))
							mensaje = mensaje.Replace("{TipoPersona}", "Señores");
						else
							mensaje = mensaje.Replace("{TipoPersona}", "Señor (a)");


						mensaje = mensaje.Replace("{NombreFacturador}", facturador.StrRazonSocial);
						mensaje = mensaje.Replace("{NitFacturador}", facturador.StrIdentificacion);
						mensaje = mensaje.Replace("{DigitovFacturador}", facturador.IntIdentificacionDv.ToString());
						mensaje = mensaje.Replace("{TelefonoFacturador}", (facturador.StrTelefono == null) ? "" : facturador.StrTelefono.ToString());
						mensaje = mensaje.Replace("{EmailFacturador}", facturador.StrMailAdmin);


						mensaje = mensaje.Replace("{NombreTercero}", facturador.StrRazonSocial);
						mensaje = mensaje.Replace("{NitTercero}", facturador.StrIdentificacion);
						mensaje = mensaje.Replace("{Digitov}", facturador.IntIdentificacionDv.ToString());
						mensaje = mensaje.Replace("{RutaAcceso}", plataforma.RutaPublica);

						mensaje = mensaje.Replace("{Adquiridos}", adquiridos.ToString());
						mensaje = mensaje.Replace("{Procesados}", procesados.ToString());
						mensaje = mensaje.Replace("{Disponibles}", disponibles.ToString());
						mensaje = mensaje.Replace("{Porcentaje}", string.Format("{0}%", Porcentaje.ToString()));


						string asunto = "ALERTA CONSUMO DE DOCUMENTOS ELECTRÓNICOS";

						DestinatarioEmail remitente = new DestinatarioEmail();
						remitente.Email = Constantes.EmailRemitente;
						remitente.Nombre = Constantes.NombreRemitenteEmail;

						DestinatarioEmail destinatario = new DestinatarioEmail();
						List<DestinatarioEmail> correos_destino = new List<DestinatarioEmail>();
						if (mail.Contains(";"))
						{
							foreach (var item_mail in Coleccion.ConvertirLista(mail, ';'))
							{
								// recibe el email el adquiriente
								destinatario = new DestinatarioEmail();
								destinatario.Nombre = facturador.StrRazonSocial;
								destinatario.Email = item_mail;
								correos_destino.Add(destinatario);
							}
						}
						else
						{
							destinatario.Nombre = facturador.StrRazonSocial;
							destinatario.Email = mail;
						}




						correos_destino.Add(destinatario);


						// envía correo electrónico con copia de auditoría
						List<DestinatarioEmail> correos_copia_oculta = null;
						if (!string.IsNullOrWhiteSpace(Constantes.EmailCopiaOculta))
						{
							correos_copia_oculta = new List<DestinatarioEmail>();

							DestinatarioEmail copia_oculta = new DestinatarioEmail();
							copia_oculta.Nombre = "Auditoría";
							copia_oculta.Email = Constantes.EmailCopiaOculta;
							correos_copia_oculta.Add(copia_oculta);
						}

						Ctl_EnvioCorreos clase_email = new Ctl_EnvioCorreos();

						RespuestaMail = clase_email.EnviarEmail(facturador.StrIdSeguridad.ToString(), false, mensaje, asunto, true, remitente, correos_destino, null, null, "", "");

					}
				}

				return RespuestaMail;
			}
			catch (Exception ex)
			{
				throw new ApplicationException(ex.Message);
			}

		}



		/// <summary>
		/// Envia Correo Electronico Indicando que no dispone de saldo
		/// </summary>
		/// <param name="identificacion">Nit del Facturador</param>
		/// <param name="mail">email al que se va enviar el correo</param>
		/// <returns></returns>
		public List<MensajeEnvio> EnviaNotificacionSinSaldo(string identificacion, string mail, int tipo)//1 cliente, 2 HGI
		{
			try
			{
				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;
				List<MensajeEnvio> RespuestaMail = new List<MensajeEnvio>();

				if (string.IsNullOrEmpty(identificacion))
					throw new ApplicationException("No se encontró información de la empresa.");
				string fileName = string.Empty;

				if (tipo == 1)
				{
					fileName = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), Constantes.RutaplantillaSinSaldo);
				}
				else
				{
					fileName = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), Constantes.RutaplantillaSinSaldoHGI);
				}

				// obtiene los datos del Facturador
				Ctl_Empresa empresa = new Ctl_Empresa();
				TblEmpresas facturador = empresa.Obtener(identificacion);

				//if (string.IsNullOrEmpty(facturador.StrSerial))
				//    throw new ApplicationException("No se encontró información del serial");

				if (!string.IsNullOrWhiteSpace(fileName))
				{
					FileInfo file = new FileInfo(fileName);

					string mensaje = file.OpenText().ReadToEnd();

					if (file != null)
					{

						if (facturador.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
						{
							string div_prueba = "<div style='background:#E7F122;cursor:auto;color:#000000;font-family:Arial, sans-serif;font-size:13px;line-height:24px;text-align:left;'><span style ='font-family:Ubuntufont-size,Helvetica,Arial,sans-serif'><b>Este correo electrónico es exclusivo para pruebas y no tiene ninguna validez comercial y/o de soporte.</b></span></p></div>";

							mensaje = mensaje.Replace("{TextoHabilitacion}", div_prueba);
						}
						else
						{
							mensaje = mensaje.Replace("{TextoHabilitacion}", "");
						}

						// Datos del Tercero
						if (facturador.StrTipoIdentificacion.Equals("31"))
							mensaje = mensaje.Replace("{TipoPersona}", "Señores");
						else
							mensaje = mensaje.Replace("{TipoPersona}", "Señor (a)");


						mensaje = mensaje.Replace("{NombreTercero}", facturador.StrRazonSocial);
						mensaje = mensaje.Replace("{NitTercero}", facturador.StrIdentificacion);
						mensaje = mensaje.Replace("{Digitov}", facturador.IntIdentificacionDv.ToString());

						string asunto = "ALERTA CONSUMO DE DOCUMENTOS ELECTRÓNICOS";

						DestinatarioEmail remitente = new DestinatarioEmail();
						remitente.Email = Constantes.EmailRemitente;
						remitente.Nombre = Constantes.NombreRemitenteEmail;

						DestinatarioEmail destinatario = new DestinatarioEmail();
						List<DestinatarioEmail> correos_destino = new List<DestinatarioEmail>();
						if (mail.Contains(";"))
						{
							foreach (var item_mail in Coleccion.ConvertirLista(mail, ';'))
							{
								// recibe el email el adquiriente
								destinatario = new DestinatarioEmail();
								destinatario.Nombre = facturador.StrRazonSocial;
								destinatario.Email = item_mail;
								correos_destino.Add(destinatario);
							}
						}
						else
						{
							destinatario.Nombre = facturador.StrRazonSocial;
							destinatario.Email = mail;
						}




						correos_destino.Add(destinatario);


						// envía correo electrónico con copia de auditoría
						List<DestinatarioEmail> correos_copia_oculta = null;
						if (!string.IsNullOrWhiteSpace(Constantes.EmailCopiaOculta))
						{
							correos_copia_oculta = new List<DestinatarioEmail>();

							DestinatarioEmail copia_oculta = new DestinatarioEmail();
							copia_oculta.Nombre = "Auditoría";
							copia_oculta.Email = Constantes.EmailCopiaOculta;
							correos_copia_oculta.Add(copia_oculta);
						}

						Ctl_EnvioCorreos clase_email = new Ctl_EnvioCorreos();

						RespuestaMail = clase_email.EnviarEmail(facturador.StrIdSeguridad.ToString(), false, mensaje, asunto, true, remitente, correos_destino, null, null, "", "");

					}
				}

				return RespuestaMail;
			}
			catch (Exception ex)
			{
				throw new ApplicationException(ex.Message);
			}

		}



		/// <summary>
		/// Envia Correo Electronico con información sobre alguna alerta, puede ser al facturador, un usuario de algún facturador o a personal de HGI
		/// </summary>
		/// <param name="identificacion">Nit del Facturador</param>
		/// <param name="mail">email al que se va enviar el correo</param>
		/// <returns></returns>
		public List<MensajeEnvio> EnviaNotificacionAlertaPorvencer(string mail, List<NotificacionAlertaporVencer> ListNotificacion)
		{
			try
			{
				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;
				List<MensajeEnvio> RespuestaMail = new List<MensajeEnvio>();

				string fileName = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), Constantes.RutaPlantillaPlanporVencer);


				string asunto = "SALDO POR VENCER DE DOCUMENTOS ELECTRÓNICOS";


				// obtiene los datos del Facturador
				Ctl_Empresa empresa = new Ctl_Empresa();
				TblEmpresas facturador = empresa.Obtener(ListNotificacion.FirstOrDefault().identificacion);

				//if (string.IsNullOrEmpty(facturador.StrSerial))
				//    throw new ApplicationException("No se encontró información del serial");

				if (!string.IsNullOrWhiteSpace(fileName))
				{
					FileInfo file = new FileInfo(fileName);

					string mensaje = file.OpenText().ReadToEnd();

					if (file != null)
					{
						CultureInfo elGR = CultureInfo.CreateSpecificCulture("el-GR");
						if (facturador.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
						{
							string div_prueba = "<div style='background:#E7F122;cursor:auto;color:#000000;font-family:Arial, sans-serif;font-size:13px;line-height:24px;text-align:left;'><span style ='font-family:Ubuntufont-size,Helvetica,Arial,sans-serif'><b>Este correo electrónico es exclusivo para pruebas y no tiene ninguna validez comercial y/o de soporte.</b></span></p></div>";

							mensaje = mensaje.Replace("{TextoHabilitacion}", div_prueba);
						}
						else
						{
							mensaje = mensaje.Replace("{TextoHabilitacion}", "");
						}

						// Datos del Tercero
						if (facturador.StrTipoIdentificacion.Equals("31"))
							mensaje = mensaje.Replace("{TipoPersona}", "Señores");
						else
							mensaje = mensaje.Replace("{TipoPersona}", "Señor (a)");


						mensaje = mensaje.Replace("{NombreTercero}", facturador.StrRazonSocial);
						mensaje = mensaje.Replace("{NitTercero}", facturador.StrIdentificacion);
						mensaje = mensaje.Replace("{Digitov}", facturador.IntIdentificacionDv.ToString());

						string detalle = string.Empty;

						foreach (var item in ListNotificacion)
						{
							detalle = string.Format("{0}<tr><td>{1}</td><td>{2}</td><td>{3} Documentos</td><td>{4}</td><td>{5}</td></tr>", detalle, item.identificacion, item.facturador, item.tdisponibles, Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoCompra>(item.intIdtipo)), item.DatFechaVencimiento.Value.ToString(Fecha.formato_fecha_hginet));
						}

						mensaje = mensaje.Replace("{TablaHtml}", detalle);

						DestinatarioEmail remitente = new DestinatarioEmail();
						remitente.Email = Constantes.EmailRemitente;
						remitente.Nombre = Constantes.NombreRemitenteEmail;

						DestinatarioEmail destinatario = new DestinatarioEmail();
						List<DestinatarioEmail> correos_destino = new List<DestinatarioEmail>();
						if (mail.Contains(";"))
						{
							foreach (var item_mail in Coleccion.ConvertirLista(mail, ';'))
							{
								// recibe el email el adquiriente
								destinatario = new DestinatarioEmail();
								destinatario.Nombre = "ADMINISTRACIÓN";
								destinatario.Email = item_mail;
								correos_destino.Add(destinatario);
							}
						}
						else
						{
							destinatario.Nombre = "ADMINISTRACIÓN";
							destinatario.Email = mail;
						}




						correos_destino.Add(destinatario);


						// envía correo electrónico con copia de auditoría
						List<DestinatarioEmail> correos_copia_oculta = null;
						if (!string.IsNullOrWhiteSpace(Constantes.EmailCopiaOculta))
						{
							correos_copia_oculta = new List<DestinatarioEmail>();

							DestinatarioEmail copia_oculta = new DestinatarioEmail();
							copia_oculta.Nombre = "Auditoría";
							copia_oculta.Email = Constantes.EmailCopiaOculta;
							correos_copia_oculta.Add(copia_oculta);
						}

						Ctl_EnvioCorreos clase_email = new Ctl_EnvioCorreos();

						RespuestaMail = clase_email.EnviarEmail("ADMINISTRACIÓN", false, mensaje, asunto, true, remitente, correos_destino, null, null, "", "");

					}
				}

				return RespuestaMail;
			}
			catch (Exception ex)
			{
				throw new ApplicationException(ex.Message);
			}

		}


		/// <summary>
		/// Envia Correo Electronico con información sobre alguna alerta, a personal de HGI
		/// </summary>
		/// <param name="identificacion">Nit del Facturador</param>
		/// <param name="mail">email al que se va enviar el correo</param>
		/// <returns></returns>
		public List<MensajeEnvio> EnviaNotificacionAlertaPorvencerHGI(string mail, List<NotificacionAlertaporVencer> ListNotificacion)
		{
			try
			{
				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;
				List<MensajeEnvio> RespuestaMail = new List<MensajeEnvio>();

				string fileName = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), Constantes.RutaPlantillaPlanporVencerHGI);


				string asunto = "SALDO POR VENCER DE DOCUMENTOS ELECTRÓNICOS";

				// obtiene los datos del Facturador
				Ctl_Empresa empresa = new Ctl_Empresa();
				TblEmpresas facturador = empresa.Obtener(ListNotificacion.FirstOrDefault().identificacion);


				if (!string.IsNullOrWhiteSpace(fileName))
				{
					FileInfo file = new FileInfo(fileName);

					string mensaje = file.OpenText().ReadToEnd();

					if (file != null)
					{
						CultureInfo elGR = CultureInfo.CreateSpecificCulture("el-GR");
						if (facturador.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
						{
							string div_prueba = "<div style='background:#E7F122;cursor:auto;color:#000000;font-family:Arial, sans-serif;font-size:13px;line-height:24px;text-align:left;'><span style ='font-family:Ubuntufont-size,Helvetica,Arial,sans-serif'><b>Este correo electrónico es exclusivo para pruebas y no tiene ninguna validez comercial y/o de soporte.</b></span></p></div>";

							mensaje = mensaje.Replace("{TextoHabilitacion}", div_prueba);
						}
						else
						{
							mensaje = mensaje.Replace("{TextoHabilitacion}", "");
						}

						string detalle = string.Empty;

						foreach (var item in ListNotificacion)
						{
							detalle = string.Format("{0}<tr><td>{1}</td><td>{2}</td><td>{3} Documentos</td><td>{4}</td><td>{5}</td></tr>", detalle, item.identificacion, item.facturador, item.tdisponibles, Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoCompra>(item.intIdtipo)), item.DatFechaVencimiento.Value.ToString(Fecha.formato_fecha_hginet));
						}

						mensaje = mensaje.Replace("{TablaHtml}", detalle);

						DestinatarioEmail remitente = new DestinatarioEmail();
						remitente.Email = Constantes.EmailRemitente;
						remitente.Nombre = Constantes.NombreRemitenteEmail;

						DestinatarioEmail destinatario = new DestinatarioEmail();
						List<DestinatarioEmail> correos_destino = new List<DestinatarioEmail>();
						if (mail.Contains(";"))
						{
							foreach (var item_mail in Coleccion.ConvertirLista(mail, ';'))
							{
								// recibe el email el adquiriente
								destinatario = new DestinatarioEmail();
								destinatario.Nombre = "ADMINISTRACIÓN";
								destinatario.Email = item_mail;
								correos_destino.Add(destinatario);
							}
						}
						else
						{
							destinatario.Nombre = "ADMINISTRACIÓN";
							destinatario.Email = mail;
						}




						correos_destino.Add(destinatario);


						// envía correo electrónico con copia de auditoría
						List<DestinatarioEmail> correos_copia_oculta = null;
						if (!string.IsNullOrWhiteSpace(Constantes.EmailCopiaOculta))
						{
							correos_copia_oculta = new List<DestinatarioEmail>();

							DestinatarioEmail copia_oculta = new DestinatarioEmail();
							copia_oculta.Nombre = "Auditoría";
							copia_oculta.Email = Constantes.EmailCopiaOculta;
							correos_copia_oculta.Add(copia_oculta);
						}

						Ctl_EnvioCorreos clase_email = new Ctl_EnvioCorreos();

						RespuestaMail = clase_email.EnviarEmail("ADMINISTRACIÓN", false, mensaje, asunto, true, remitente, correos_destino, null, null, "", "");

					}
				}

				return RespuestaMail;
			}
			catch (Exception ex)
			{
				throw new ApplicationException(ex.Message);
			}

		}

		/// <summary>
		/// Realiza el envío del correo electrónico notificando los procesos de los formatos PDF.
		/// </summary>
		/// <param name="empresa_solicita"></param>
		/// <param name="datos_empresa_formato"></param>
		/// <param name="datos_formato"></param>
		/// <param name="usuario"></param>
		/// <param name="observaciones_solicitud"></param>
		/// <param name="tipo_proceso"></param>
		/// <param name="correos_destino"></param>
		/// <returns></returns>
		public List<MensajeEnvio> EnviarNotificacionProcesosFormato(TblEmpresas empresa_solicita, TblEmpresas datos_empresa_formato, TblFormatos datos_formato, TblUsuarios usuario, string observaciones_solicitud, TiposProceso tipo_proceso, List<DestinatarioEmail> correos_destino)
		{
			try
			{
				string fileName = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), Constantes.RutaPlantillaSolicitudAprobacionFormato);

				List<MensajeEnvio> Mensaje = new List<MensajeEnvio>();

				string mail_envio = Constantes.EmailCopiaOculta;

				if (!string.IsNullOrWhiteSpace(fileName))
				{
					FileInfo file = new FileInfo(fileName);

					string mensaje = file.OpenText().ReadToEnd();

					if (file != null)
					{
						CultureInfo elGR = CultureInfo.CreateSpecificCulture("el-GR");
						if (empresa_solicita.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
						{
							string div_prueba = "<div style='background:#E7F122;cursor:auto;color:#000000;font-family:Arial, sans-serif;font-size:13px;line-height:24px;text-align:left;'><span style ='font-family:Ubuntufont-size,Helvetica,Arial,sans-serif'><b>Este correo electrónico es exclusivo para pruebas y no tiene ninguna validez comercial y/o de soporte.</b></span></p></div>";

							mensaje = mensaje.Replace("{TextoHabilitacion}", div_prueba);
						}
						else
						{
							mensaje = mensaje.Replace("{TextoHabilitacion}", "");
						}

						string texto_proceso = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TiposProceso>(tipo_proceso.GetHashCode()));

						mensaje = mensaje.Replace("{TituloNotificacion}", texto_proceso.ToUpper());

						mensaje = mensaje.Replace("{TextoProceso}", texto_proceso.ToLower());
						//DATOS DEL FORMATO.
						mensaje = mensaje.Replace("{CodigoFormato}", datos_formato.IntCodigoFormato.ToString());

						mensaje = mensaje.Replace("{NombreEmpresaFormato}", datos_empresa_formato.StrRazonSocial);
						mensaje = mensaje.Replace("{NitEmpresaFormato}", datos_empresa_formato.StrIdentificacion);
						mensaje = mensaje.Replace("{DigitoDvEmpresaFormato}", datos_empresa_formato.IntIdentificacionDv.ToString());
						mensaje = mensaje.Replace("{FechaCreacion}", datos_formato.DatFechaRegistro.ToString("yyyy-MM-dd"));
						mensaje = mensaje.Replace("{EstadoFormato}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<EstadosFormato>(datos_formato.IntEstado)));

						//DATOS SOLICITUD.
						mensaje = mensaje.Replace("{EmpresaSolicita}", empresa_solicita.StrRazonSocial);
						mensaje = mensaje.Replace("{NitSolicitante}", empresa_solicita.StrIdentificacion);
						mensaje = mensaje.Replace("{DigitoDvSolicitante}", empresa_solicita.IntIdentificacionDv.ToString());
						mensaje = mensaje.Replace("{UsuarioSolicita}", string.Format("{0} {1}", usuario.StrNombres, usuario.StrApellidos));
						mensaje = mensaje.Replace("{FechaSolicitud}", Fecha.GetFecha().ToString("yyyy-MM-dd HH:mm"));

						//Observaciones Solicitud:&nbsp;&nbsp;
						if (!string.IsNullOrWhiteSpace(observaciones_solicitud))
							mensaje = mensaje.Replace("{ObservacionesSolicitud}", observaciones_solicitud);
						else
							mensaje = mensaje.Replace("{ObservacionesSolicitud}", "");

						DestinatarioEmail remitente = new DestinatarioEmail();
						remitente.Email = Constantes.EmailRemitente;
						remitente.Nombre = Constantes.NombreRemitenteEmail;


						// envía correo electrónico con copia de auditoría
						List<DestinatarioEmail> correos_copia_oculta = null;
						if (!string.IsNullOrWhiteSpace(Constantes.EmailCopiaOculta))
						{
							correos_copia_oculta = new List<DestinatarioEmail>();

							DestinatarioEmail copia_oculta = new DestinatarioEmail();
							copia_oculta.Nombre = "Auditoría";
							copia_oculta.Email = Constantes.EmailCopiaOculta;
							correos_copia_oculta.Add(copia_oculta);
						}


						Ctl_EnvioCorreos clase_email = new Ctl_EnvioCorreos();

						Mensaje = clase_email.EnviarEmail(empresa_solicita.StrIdSeguridad.ToString(), false, mensaje, texto_proceso.ToUpper(), true, remitente, correos_destino, null, null, "", "");

					}
				}
				return Mensaje;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Metodo para consultar el Estado del Envio de correo a Mailjet
		/// </summary>
		/// <param name="MessageID">Id del Mensaje retornado por la plataforma de Mailjet</param>
		/// <returns></returns>
		public MensajeResumen ConsultarCorreo(long MessageID)
		{

			MensajeResumen datos_retorno = new MensajeResumen();

			PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

			MensajeResumenGlobal obj_peticion = new MensajeResumenGlobal();
			obj_peticion.identificacion = plataforma_datos.IdentificacionHGInetMail;
			obj_peticion.serial = plataforma_datos.LicenciaHGInetMail;
			obj_peticion.id_mensaje = MessageID;

			ClienteRest<MensajeResumen> cliente = new ClienteRest<MensajeResumen>(string.Format("{0}/Api/ObtenerResumenMensaje", plataforma_datos.RutaHginetMail), TipoContenido.Applicationjson.GetHashCode(), "");
			try
			{
				datos_retorno = cliente.POST(obj_peticion);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
			return (datos_retorno);

		}



		/// <summary>
		/// Notificacion que se envia al Facturador cuando no es efectivo la entrega del correo al adquiriente
		/// </summary>
		/// <param name="documento">Informacion del documento</param>
		/// <param name="telefono">telefono del Adquiriente</param>
		/// <param name="email_adquiriente">Email al que fue enviado el correo</param>
		/// <param name="estado_correo">Estado entregado por Mailjet del correo enviado</param>
		/// <param name="id_peticion">Peticion generada por la Platforma de Factura Electronica</param>
		/// <param name="procedencia">Indica quien genera la informacion para la Auditoria</param>
		/// <param name="usuario"></param>
		/// <param name="proceso"></param>
		/// <returns></returns>
		public List<MensajeEnvio> NotificacionCorreofacturador(TblDocumentos documento, string telefono, string email_adquiriente, string estado_correo, string id_peticion = "", Procedencia procedencia = Procedencia.Plataforma, string usuario = "", ProcesoEstado proceso = ProcesoEstado.RecepcionAcuse)
		{
			Ctl_DocumentosAudit clase_auditoria = new Ctl_DocumentosAudit();
			List<MensajeEnvio> respuesta_email = new List<MensajeEnvio>();

			try
			{
				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;

				string ruta_plantilla_html = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), Constantes.RutaplantillaCorreoNoEntregado);

				string asunto = string.Empty;

				// obtiene los datos del facturador electrónico
				Ctl_Empresa facturador_electronico = new Ctl_Empresa();
				TblEmpresas empresa_obligado = facturador_electronico.Obtener(documento.StrEmpresaFacturador);

				if (empresa_obligado.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
				{
					asunto = string.Format("{0} (HABILITACIÓN)", Constantes.AsuntoNotificacionCorreoNoEntregado);
				}
				else
				{
					asunto = Constantes.AsuntoNotificacionCorreoNoEntregado;
				}

				// envía como email de respuesta facturador electrónico
				DestinatarioEmail remitente = new DestinatarioEmail();
				remitente.Nombre = Constantes.NombreRemitenteEmail;
				remitente.Email = Constantes.EmailRemitente;

				// obtiene los datos del adquiriente
				Ctl_Empresa adquiriente = new Ctl_Empresa();
				TblEmpresas empresa_adquiriente = adquiriente.Obtener(documento.StrEmpresaAdquiriente);

				List<DestinatarioEmail> correos_destino = new List<DestinatarioEmail>();

				// recibe el email el adquiriente
				DestinatarioEmail destinatario = new DestinatarioEmail();
				destinatario.Nombre = empresa_obligado.StrRazonSocial;
				destinatario.Email = empresa_obligado.StrMailAcuse;
				correos_destino.Add(destinatario);

				// envía correo electrónico con copia de auditoría
				List<DestinatarioEmail> correos_copia_oculta = null;
				if (!string.IsNullOrWhiteSpace(Constantes.EmailCopiaOculta))
				{
					correos_copia_oculta = new List<DestinatarioEmail>();

					DestinatarioEmail copia_oculta = new DestinatarioEmail();
					copia_oculta.Nombre = "Auditoría";
					copia_oculta.Email = Constantes.EmailCopiaOculta;
					correos_copia_oculta.Add(copia_oculta);
				}


				// plantilla Html
				if (!string.IsNullOrWhiteSpace(ruta_plantilla_html))
				{
					FileInfo file = new FileInfo(ruta_plantilla_html);

					string mensaje = file.OpenText().ReadToEnd();


					if (file != null)
					{

						if (empresa_obligado.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
						{
							string div_prueba = "<div style='background:#E7F122;cursor:auto;color:#000000;font-family:Arial, sans-serif;font-size:13px;line-height:24px;text-align:left;'><span style ='font-family:Ubuntufont-size,Helvetica,Arial,sans-serif'><b>Este correo electrónico es exclusivo para pruebas y no tiene ninguna validez comercial y/o de soporte.</b></span></p></div>";

							mensaje = mensaje.Replace("{TextoHabilitacion}", div_prueba);
						}
						else
						{
							mensaje = mensaje.Replace("{TextoHabilitacion}", "");
						}

						// Datos del Tercero
						mensaje = mensaje.Replace("{ImagenLogo}", "<img id='ImgLogo' src='" + "" + "' style='border: none; border-radius: 0px; display: block; outline: none; text-decoration: none; width: 100%; height: auto;' width='233' />");
						mensaje = mensaje.Replace("{NombreAdquiriente}", empresa_adquiriente.StrRazonSocial);
						mensaje = mensaje.Replace("{NitAdquiriente}", empresa_adquiriente.StrIdentificacion);
						mensaje = mensaje.Replace("{DigitovAdquiriente}", empresa_adquiriente.IntIdentificacionDv.ToString());
						mensaje = mensaje.Replace("{EmailAdquiriente}", email_adquiriente);
						if (!string.IsNullOrEmpty(estado_correo))
							mensaje = mensaje.Replace("{EstadoCorreo}", estado_correo);
						else
							mensaje = mensaje.Replace("{EstadoCorreo}", "Desconocido");

						if (!string.IsNullOrWhiteSpace(telefono))
							mensaje = mensaje.Replace("{TelefonoAdquiriente}", telefono);
						else
							mensaje = mensaje.Replace("{TelefonoAdquiriente}", "");

						// Datos Facturador Electronico
						if (empresa_obligado.StrTipoIdentificacion.Equals("31"))
							mensaje = mensaje.Replace("{TipoPersona}", "Señores");
						else
							mensaje = mensaje.Replace("{TipoPersona}", "Señor (a)");

						mensaje = mensaje.Replace("{NombreTercero}", empresa_obligado.StrRazonSocial);
						mensaje = mensaje.Replace("{NitTercero}", empresa_obligado.StrIdentificacion);
						mensaje = mensaje.Replace("{Digitov}", empresa_obligado.IntIdentificacionDv.ToString());


						//Datos del Documento

						// obtiene el título para el tipo de documento
						TipoDocumento doc_tipo = Enumeracion.GetEnumObjectByValue<TipoDocumento>(documento.IntDocTipo);
						string titulo_documento = Enumeracion.GetDescription(doc_tipo);

						mensaje = mensaje.Replace("{TipoDocumento}", titulo_documento);
						mensaje = mensaje.Replace("{NumeroDocumento}", String.Format("{0}{1}", documento.StrPrefijo, documento.IntNumero.ToString()));
						mensaje = mensaje.Replace("{FechaDocumento}", documento.DatFechaDocumento.ToString(Fecha.formato_fecha_hginet));
						mensaje = mensaje.Replace("{TotalDocumento}", String.Format("{0:###,##0.}", documento.IntVlrTotal));
						mensaje = mensaje.Replace("{CodigoCUFE}", documento.StrCufe);
						mensaje = mensaje.Replace("{GuidDocumento}", documento.StrIdSeguridad.ToString());

						mensaje = mensaje.Replace("{RutaAcceso}", plataforma.RutaPublica);

						List<Adjunto> archivos = new List<Adjunto>();

						if (string.IsNullOrEmpty(documento.StrUrlArchivoPdf))
							throw new ApplicationException("No se encontró ruta de archivo pdf");


						byte[] bytes_pdf = Archivo.ObtenerWeb(documento.StrUrlArchivoPdf);
						string ruta_fisica_pdf = Convert.ToBase64String(bytes_pdf);
						string nombre_pdf = Path.GetFileName(documento.StrUrlArchivoPdf);


						if (!string.IsNullOrEmpty(ruta_fisica_pdf))
						{
							Adjunto adjunto = new Adjunto();
							adjunto.ContenidoB64 = ruta_fisica_pdf;
							adjunto.Nombre = nombre_pdf;
							archivos.Add(adjunto);
						}


						if (string.IsNullOrEmpty(documento.StrUrlArchivoUbl))
							throw new ApplicationException("No se encontró ruta de archivo xml");

						byte[] bytes_xml = Archivo.ObtenerWeb(documento.StrUrlArchivoUbl);
						string ruta_fisica_xml = Convert.ToBase64String(bytes_xml);
						string nombre_xml = Path.GetFileName(documento.StrUrlArchivoUbl);

						if (!string.IsNullOrEmpty(ruta_fisica_xml))
						{
							Adjunto adjunto = new Adjunto();
							adjunto.ContenidoB64 = ruta_fisica_xml;
							adjunto.Nombre = nombre_xml;
							archivos.Add(adjunto);
						}

						//Proceso para los anexos
						if (documento.StrUrlAnexo != null)
						{
							if (!string.IsNullOrEmpty(documento.StrUrlAnexo))
							{
								mensaje = mensaje.Replace("{Anexos}", "Anexos");
								mensaje = mensaje.Replace("{ObservacionAnexos}", documento.StrObservacionAnexo);
								mensaje = mensaje.Replace("{UrlAnexos}", documento.StrUrlAnexo);

								if (documento.IntPesoAnexo > 0)
								{

									byte[] bytes_anexo = Archivo.ObtenerWeb(documento.StrUrlAnexo);
									string ruta_fisica_anexo = Convert.ToBase64String(bytes_anexo);
									string nombre_anexo = Path.GetFileName(documento.StrUrlAnexo);

									if (!string.IsNullOrEmpty(ruta_fisica_anexo))
									{
										Adjunto adjunto = new Adjunto();
										adjunto.ContenidoB64 = ruta_fisica_anexo;
										adjunto.Nombre = nombre_anexo;
										archivos.Add(adjunto);
									}
								}

							}
							else
							{
								mensaje = mensaje.Replace("{Anexos}", "");
								mensaje = mensaje.Replace("{ObservacionAnexos}", "");
								mensaje = mensaje.Replace("{UrlAnexos}", "");
							}
						}
						else
						{
							mensaje = mensaje.Replace("{Anexos}", "");
							mensaje = mensaje.Replace("{ObservacionAnexos}", "");
							mensaje = mensaje.Replace("{UrlAnexos}", "");
						}
						// envía el correo electrónico
						respuesta_email = EnviarEmail(documento.StrIdSeguridad.ToString(), false, mensaje, asunto, true, remitente, correos_destino, null, null, "", "", archivos);
					}
				}
				try
				{
					Guid peticion = (string.IsNullOrEmpty(id_peticion) ? Guid.Empty : Guid.Parse(id_peticion));
					int estado = Ctl_Documento.ObtenerCategoria(documento.IntIdEstado);
					string mensaje = (string.IsNullOrEmpty(id_peticion) ? "Reenvio de Documento" : "Notificación Correo no entregado");
					clase_auditoria.Crear(documento.StrIdSeguridad, peticion, empresa_obligado.StrIdentificacion, proceso, TipoRegistro.Proceso, procedencia, usuario, mensaje, string.Empty, respuesta_email, documento.StrPrefijo, Convert.ToString(documento.IntNumero), estado);
				}
				catch (Exception) { throw; }
				return respuesta_email;
			}
			catch (Exception excepcion)
			{
				try
				{
					int estado = Ctl_Documento.ObtenerCategoria(documento.IntIdEstado);
					clase_auditoria.Crear(documento.StrIdSeguridad, Guid.Empty, documento.StrObligadoIdRegistro, proceso, TipoRegistro.Proceso, procedencia, usuario, string.Format("NotificacionDocumento - {0}", excepcion.Message), string.Format("{0}", excepcion.InnerException), documento.StrPrefijo, Convert.ToString(documento.IntNumero), estado);
				}
				catch (Exception) { throw; }

				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}


		}


		public List<MensajeEnvio> EnvioFormatoPrueba(TblEmpresas empresa_solicita, TblFormatos datos_formato, List<Adjunto> adjuntos, string mail_destino)
		{
			try
			{
				string fileName = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), Constantes.RutaPlantillaPruebaFormato);

				List<MensajeEnvio> respuesta_envio = new List<MensajeEnvio>();

				//obtiene los datos de la empresa asociada al formato.
				Ctl_Empresa clase_empresa = new Ctl_Empresa();
				TblEmpresas datos_empresa_formato = clase_empresa.Obtener(datos_formato.StrEmpresa);

				string mail_envio = Constantes.EmailCopiaOculta;

				if (!string.IsNullOrWhiteSpace(fileName))
				{
					FileInfo file = new FileInfo(fileName);

					string mensaje = file.OpenText().ReadToEnd();

					if (file != null)
					{
						CultureInfo elGR = CultureInfo.CreateSpecificCulture("el-GR");
						if (empresa_solicita.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
						{
							string div_prueba = "<div style='background:#E7F122;cursor:auto;color:#000000;font-family:Arial, sans-serif;font-size:13px;line-height:24px;text-align:left;'><span style ='font-family:Ubuntufont-size,Helvetica,Arial,sans-serif'><b>Este correo electrónico es exclusivo para pruebas y no tiene ninguna validez comercial y/o de soporte.</b></span></p></div>";

							mensaje = mensaje.Replace("{TextoHabilitacion}", div_prueba);
						}
						else
							mensaje = mensaje.Replace("{TextoHabilitacion}", "");

						//DATOS DEL FORMATO.
						mensaje = mensaje.Replace("{CodigoFormato}", datos_formato.IntCodigoFormato.ToString());

						mensaje = mensaje.Replace("{NombreEmpresaFormato}", datos_empresa_formato.StrRazonSocial);
						mensaje = mensaje.Replace("{NitEmpresaFormato}", datos_empresa_formato.StrIdentificacion);
						mensaje = mensaje.Replace("{DigitoDvEmpresaFormato}", datos_empresa_formato.IntIdentificacionDv.ToString());
						mensaje = mensaje.Replace("{FechaCreacion}", datos_formato.DatFechaRegistro.ToString("yyyy-MM-dd"));
						mensaje = mensaje.Replace("{EstadoFormato}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<EstadosFormato>(datos_formato.IntEstado)));

						// recibe el email el adquiriente
						DestinatarioEmail remitente = new DestinatarioEmail();
						remitente.Email = Constantes.EmailRemitente;
						remitente.Nombre = Constantes.NombreRemitenteEmail;

						// envía correo electrónico con copia de auditoría
						List<DestinatarioEmail> correos_copia_oculta = null;
						if (!string.IsNullOrWhiteSpace(Constantes.EmailCopiaOculta))
						{
							correos_copia_oculta = new List<DestinatarioEmail>();

							DestinatarioEmail copia_oculta = new DestinatarioEmail();
							copia_oculta.Nombre = "Auditoría";
							copia_oculta.Email = Constantes.EmailCopiaOculta;
							correos_copia_oculta.Add(copia_oculta);
						}

						List<DestinatarioEmail> correos_destino = new List<DestinatarioEmail>();
						DestinatarioEmail destinatario = new DestinatarioEmail();
						destinatario.Nombre = "ADMINISTRACIÓN";
						destinatario.Email = mail_destino;
						correos_destino.Add(destinatario);

						Ctl_EnvioCorreos clase_email = new Ctl_EnvioCorreos();
						respuesta_envio = clase_email.EnviarEmail(empresa_solicita.StrIdSeguridad.ToString(), false, mensaje, "NOTIFICACIÓN FORMATO DE PRUEBA", true, remitente, correos_destino, null, null, "", "", adjuntos);

					}
				}

				return respuesta_envio;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}





		#region Alertas Documentos DIAN		
		/// <summary>
		/// Envia Correo Electronico con información sobre alguna alerta, a personal de HGI
		/// </summary>
		/// <param name="identificacion">Nit del Facturador</param>
		/// <param name="mail">email al que se va enviar el correo</param>
		/// <returns></returns>
		public List<MensajeEnvio> EnviaNotificacionAlertaDIAN(string Facturador, string Documento, List<String> ListaNotificacion, int Proceso, bool Resultado,string mail)
		{
			try
			{
				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;
				List<MensajeEnvio> RespuestaMail = new List<MensajeEnvio>();

				string fileName = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), Constantes.RutaPlantillaAlertaDocumentoDIAN);

				string asunto = "ALERTA DE INCONSISTENCIAS DE DOCUMENTO ELECTRÓNICO EN LA DIAN";

				// obtiene los datos del Facturador
				Ctl_Empresa empresa = new Ctl_Empresa();
				TblEmpresas facturador = empresa.Obtener(Facturador);


				if (!string.IsNullOrWhiteSpace(fileName))
				{
					FileInfo file = new FileInfo(fileName);

					string mensaje = file.OpenText().ReadToEnd();

					if (file != null)
					{
						CultureInfo elGR = CultureInfo.CreateSpecificCulture("el-GR");
						if (facturador.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
						{
							string div_prueba = "<div style='background:#E7F122;cursor:auto;color:#000000;font-family:Arial, sans-serif;font-size:13px;line-height:24px;text-align:left;'><span style ='font-family:Ubuntufont-size,Helvetica,Arial,sans-serif'><b>Este correo electrónico es exclusivo para pruebas y no tiene ninguna validez comercial y/o de soporte.</b></span></p></div>";

							mensaje = mensaje.Replace("{TextoHabilitacion}", div_prueba);
						}
						else
						{
							mensaje = mensaje.Replace("{TextoHabilitacion}", "");
						}

						string detalle = string.Empty;

						foreach (var item in ListaNotificacion)
						{
							//	detalle = string.Format("{0}<tr><td>{1}</td><td>{2}</td><td>{3} Documentos</td><td>{4}</td><td>{5}</td></tr>", detalle, item.identificacion, item.facturador, item.tdisponibles, Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoCompra>(item.intIdtipo)), item.DatFechaVencimiento.Value.ToString(Fecha.formato_fecha_hginet));
								detalle += string.Format("<tr><td>{0}</td></tr>", item);
						}

						mensaje = mensaje.Replace("{TablaHtml}", detalle);
						mensaje = mensaje.Replace("{Facturador}", facturador.StrRazonSocial);
						mensaje = mensaje.Replace("{Documento}", Documento);
						mensaje = mensaje.Replace("{Estado}", (Resultado)?"Recibido": "No Recibido");
						mensaje = mensaje.Replace("{Proceso}", (Proceso==0) ? "Desconocido" : (Proceso == 1) ? "Envío" : "Consulta");


						DestinatarioEmail remitente = new DestinatarioEmail();
						remitente.Email = Constantes.EmailRemitente;
						remitente.Nombre = Constantes.NombreRemitenteEmail;

						DestinatarioEmail destinatario = new DestinatarioEmail();
						List<DestinatarioEmail> correos_destino = new List<DestinatarioEmail>();
						if (mail.Contains(";"))
						{
							foreach (var item_mail in Coleccion.ConvertirLista(mail, ';'))
							{
								// recibe el email el adquiriente
								destinatario = new DestinatarioEmail();
								destinatario.Nombre = "ADMINISTRACIÓN";
								destinatario.Email = item_mail;
								correos_destino.Add(destinatario);
							}
						}
						else
						{
							destinatario.Nombre = "ADMINISTRACIÓN";
							destinatario.Email = mail;
						}

						correos_destino.Add(destinatario);

						// envía correo electrónico con copia de auditoría
						List<DestinatarioEmail> correos_copia_oculta = null;
						if (!string.IsNullOrWhiteSpace(Constantes.EmailCopiaOculta))
						{
							correos_copia_oculta = new List<DestinatarioEmail>();

							DestinatarioEmail copia_oculta = new DestinatarioEmail();
							copia_oculta.Nombre = "Auditoría";
							copia_oculta.Email = Constantes.EmailCopiaOculta;
							correos_copia_oculta.Add(copia_oculta);
						}

						Ctl_EnvioCorreos clase_email = new Ctl_EnvioCorreos();

						RespuestaMail = clase_email.EnviarEmail("ADMINISTRACIÓN", false, mensaje, asunto, true, remitente, correos_destino, null, null, "", "");

					}
				}

				return RespuestaMail;
			}
			catch (Exception ex)
			{
				throw new ApplicationException(ex.Message);
			}

		}

		#endregion



		#region Confirmación de correos
		/// <summary>
		/// Envia Correo Electronico con información sobre alguna alerta, a personal de HGI
		/// </summary>
		/// <param name="identificacion">Nit del Facturador</param>
		/// <param name="mail">email al que se va enviar el correo</param>
		/// <returns></returns>
		public List<MensajeEnvio> EnviaConfirmacionEmail(string Facturador, string mail)
		{
			try
			{
				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;
				List<MensajeEnvio> RespuestaMail = new List<MensajeEnvio>();

				string fileName = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), Constantes.RutaPlantillaConfirmacionEmail);

				string asunto = "CONFIRMACIÓN DE EMAIL FACTURA ELECTRÓNICA";

				// obtiene los datos del Facturador
				Ctl_Empresa empresa = new Ctl_Empresa();
				TblEmpresas facturador = empresa.Obtener(Facturador);


				if (!string.IsNullOrWhiteSpace(fileName))
				{
					FileInfo file = new FileInfo(fileName);

					string mensaje = file.OpenText().ReadToEnd();

					if (file != null)
					{
						CultureInfo elGR = CultureInfo.CreateSpecificCulture("el-GR");
						if (facturador.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
						{
							string div_prueba = "<div style='background:#E7F122;cursor:auto;color:#000000;font-family:Arial, sans-serif;font-size:13px;line-height:24px;text-align:left;'><span style ='font-family:Ubuntufont-size,Helvetica,Arial,sans-serif'><b>Este correo electrónico es exclusivo para pruebas y no tiene ninguna validez comercial y/o de soporte.</b></span></p></div>";

							mensaje = mensaje.Replace("{TextoHabilitacion}", div_prueba);
						}
						else
						{
							mensaje = mensaje.Replace("{TextoHabilitacion}", "");
						}

						string RutaUrl = string.Empty;
						
						mensaje = mensaje.Replace("{RutaUrl}", string.Format("{0}/Views/Pages/ConfirmacionEmail.aspx?ID={1}&Mail={2}", plataforma.RutaPublica, facturador.StrIdSeguridad, mail));
						mensaje = mensaje.Replace("{Facturador}", facturador.StrRazonSocial);
						
						DestinatarioEmail remitente = new DestinatarioEmail();
						remitente.Email = Constantes.EmailRemitente;
						remitente.Nombre = Constantes.NombreRemitenteEmail;

						DestinatarioEmail destinatario = new DestinatarioEmail();
						List<DestinatarioEmail> correos_destino = new List<DestinatarioEmail>();
						if (mail.Contains(";"))
						{
							foreach (var item_mail in Coleccion.ConvertirLista(mail, ';'))
							{
								// recibe el email el adquiriente
								destinatario = new DestinatarioEmail();
								destinatario.Nombre = "ADMINISTRACIÓN";
								destinatario.Email = item_mail;
								correos_destino.Add(destinatario);
							}
						}
						else
						{
							destinatario.Nombre = "ADMINISTRACIÓN";
							destinatario.Email = mail;
						}

						correos_destino.Add(destinatario);

						// envía correo electrónico con copia de auditoría
						List<DestinatarioEmail> correos_copia_oculta = null;
						if (!string.IsNullOrWhiteSpace(Constantes.EmailCopiaOculta))
						{
							correos_copia_oculta = new List<DestinatarioEmail>();

							DestinatarioEmail copia_oculta = new DestinatarioEmail();
							copia_oculta.Nombre = "Auditoría";
							copia_oculta.Email = Constantes.EmailCopiaOculta;
							correos_copia_oculta.Add(copia_oculta);
						}

						Ctl_EnvioCorreos clase_email = new Ctl_EnvioCorreos();

						RespuestaMail = clase_email.EnviarEmail("ADMINISTRACIÓN", false, mensaje, asunto, true, remitente, correos_destino, null, null, "", "");

					}
				}

				return RespuestaMail;
			}
			catch (Exception ex)
			{
				throw new ApplicationException(ex.Message);
			}

		}

		#endregion



	}

}
