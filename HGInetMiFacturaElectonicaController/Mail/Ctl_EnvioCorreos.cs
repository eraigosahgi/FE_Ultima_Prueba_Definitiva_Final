
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
using LibreriaGlobalHGInet.Mail;
using LibreriaGlobalHGInet.Objetos;
using LibreriaGlobalHGInet.ObjetosComunes.Mensajeria;
using LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.Respuesta;
using LibreriaGlobalHGInet.Peticiones;
using LibreriaGlobalHGInet.RegistroLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
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

				string ruta_envio = string.Empty;

				if (correo_remitente == null)
					throw new ApplicationException("No se encontró información del Remitente");
				else if (string.IsNullOrWhiteSpace(correo_remitente.Email))
					throw new ApplicationException("No se encontró información del Remitente");

				if (correos_destino == null && correos_copia == null && correos_copia_oculta == null)
					throw new ApplicationException("No se encontró información del Destinatario");

				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;

				//Obtengo a la ruta de plataforma de servicio
				ruta_envio = ObtenerUrl();
				if (string.IsNullOrEmpty(ruta_envio))
					ruta_envio = plataforma.RutaHginetMail;

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

					respuesta_email = Ctl_CloudMensajeria.Enviar(ruta_envio, plataforma.LicenciaHGInetMail, plataforma.IdentificacionHGInetMail, mensajes, plataforma.IdAplicacionHGInetMail);

				}

				return respuesta_email;

			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, LibreriaGlobalHGInet.RegistroLog.MensajeCategoria.Servicio, LibreriaGlobalHGInet.RegistroLog.MensajeTipo.Error, LibreriaGlobalHGInet.RegistroLog.MensajeAccion.actualizacion, "No fue posible el envio del correo");
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Obtiene la configuración para el ambiente, la versión y la empresa solicita
		/// </summary>
		/// <param name="ambiente"></param>
		/// <param name="version"></param>
		/// <param name="identificacion_empresa"></param>
		/// <returns></returns>
		public string ObtenerUrl()
		{
			string StrUrl = "https://srvcloudservices#.hginet.co/";

			try
			{

				int intervalo = LibreriaGlobalHGInet.Funciones.Fecha.GetFecha().Second;

				int subdominio = (intervalo / 5);

				while (subdominio > 10)
				{
					subdominio = (intervalo / 10);
				}

				subdominio += 1;

				if (subdominio > 5)
					subdominio -= 5;

				StrUrl = StrUrl.Replace("#", subdominio.ToString());

				if (StrUrl.Contains("#"))
				{
					StrUrl = "https://cloudservices.hginet.co/";
				}
			}
			catch (Exception)
			{
				StrUrl = "https://cloudservices.hginet.co/";
			}

			return StrUrl;
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
		/// Envía mail de bienvenida al portal.
		/// </summary>
		/// <param name="empresa">Datos del Obligado o el Adquiriente</param>
		/// <param name="datos_usuario">datos del usuario</param>
		/// <returns></returns>
		public List<MensajeEnvio> BienvenidaPagos(TblEmpresas empresa, TblUsuariosPagos usuario, string nuevo_email = "", bool notifica_empresa = false)
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

				if (notifica_empresa)
				{
					fileName = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), Constantes.RutaPlantillaNuevoUsuarioPagos);
				}
				else
				{
					fileName = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), Constantes.RutaPlantillaBienvenidaAdquirientePagos);
				}


				// permite el cambio de contraseña para el usuario
				Ctl_UsuarioPagos _usuario = new Ctl_UsuarioPagos();
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
						mensaje = mensaje.Replace("{ImagenTercero}", string.Format(" src={0}{1}/Scripts/Images/Terceros/{2}.png{3} ", '"', plataforma.RutaPublica, empresa.StrIdSeguridad, '"'));
						mensaje = mensaje.Replace("{RutaAccesoIdentificacion}", string.Format("{0}/Views/Login/Pagos.aspx?serial={1}", plataforma.RutaPublica, empresa.StrIdSeguridad));

						if (notifica_empresa)
						{
							mensaje = mensaje.Replace("{DocumentoIdentificacion}", usuario.StrEmpresaAdquiriente);
							mensaje = mensaje.Replace("{Nombres}", string.Format("{0} {1}", usuario.StrNombres, usuario.StrApellidos));
							mensaje = mensaje.Replace("{RutaUrl}", string.Format("{0}{1}", plataforma.RutaPublica, Constantes.PaginaConfirmacionRegistroUsuarioPagos.Replace("{id_seguridad}", usuario.StrIdCambioClave.ToString())));
						}
						else
						{
							mensaje = mensaje.Replace("{DocumentoIdentificacion}", empresa.StrIdentificacion);
							mensaje = mensaje.Replace("{RutaUrl}", string.Format("{0}{1}", plataforma.RutaPublica, Constantes.PaginaRestablecerClavePagos.Replace("{id_seguridad}", usuario.StrIdCambioClave.ToString())));
						}

						mensaje = mensaje.Replace("{CodigoUsuario}", usuario.StrUsuario);

						mensaje = mensaje.Replace("{RutaAcceso}", plataforma.RutaPublica);

						string asunto = "";
						if (notifica_empresa)
						{
							//Empresa
							asunto = Constantes.AsuntoEmailNuevoUsuarioPagos;
						}
						else
						{
							//Usuarios
							asunto = Constantes.AsuntoEmailBienvenidaPagos;
						}

						DestinatarioEmail remitente = new DestinatarioEmail();
						remitente.Email = Constantes.EmailRemitente;

						if (notifica_empresa)
						{
							//Registro Usuario							
							remitente.Nombre = string.Format("{0} {1}", usuario.StrNombres, usuario.StrApellidos);
							remitente.Email = usuario.StrMail;

						}
						else
						{
							//Confirmar Empresa													
							remitente.Nombre = empresa.StrRazonSocial;
							remitente.Email = empresa.StrMailPagos;
						}

						DestinatarioEmail destinatario = new DestinatarioEmail();
						destinatario.Nombre = string.Format("{0} {1}", usuario.StrNombres, usuario.StrApellidos);
						if (notifica_empresa)
							destinatario.Email = empresa.StrMailPagos;
						else
							destinatario.Email = usuario.StrMail;

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
		public List<MensajeEnvio> NotificacionDocumento(TblDocumentos documento, string telefono, string nuevo_email = "", string id_peticion = "", Procedencia procedencia = Procedencia.Plataforma, string usuario = "", ProcesoEstado proceso = ProcesoEstado.EnvioEmailAcuse, string nombre_comercial = "", bool reenvio_documento = false, bool interoperabilidad = false)
		{
			Ctl_DocumentosAudit clase_auditoria = new Ctl_DocumentosAudit();
			List<MensajeEnvio> respuesta_email = new List<MensajeEnvio>();
			TblEmpresas empresa_obligado = new TblEmpresas();
			TblEmpresas empresa_adquiriente = new TblEmpresas();
			try
			{
				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;

				string ruta_plantilla_html = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), Constantes.RutaPlantillaHtmlDocumentos);

				//string asunto = string.Empty;

				// obtiene los datos del facturador electrónico
				Ctl_Empresa facturador_electronico = new Ctl_Empresa();
				empresa_obligado = facturador_electronico.Obtener(documento.StrEmpresaFacturador);

				#region Fecha de Validacion en PDF

				try
				{

					// ruta física del xml
					string carpeta_archivos = string.Format("{0}\\{1}\\{2}", plataforma.RutaDmsFisica, Constantes.CarpetaFacturaElectronica, empresa_obligado.StrIdSeguridad.ToString());
					carpeta_archivos = string.Format(@"{0}\{1}", carpeta_archivos, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEConsultaDian);

					// Nombre del archivo Xml 
					string nombre_archivo = NombramientoArchivo.ObtenerXml(documento.IntNumero.ToString(), documento.StrEmpresaFacturador, Enumeracion.ParseToEnum<TipoDocumento>(documento.IntDocTipo), documento.StrPrefijo);

					// ruta del xml
					string ruta_xml = string.Format(@"{0}\{1}.xml", carpeta_archivos, nombre_archivo);

					if (!Archivo.ValidarExistencia(ruta_xml))
						throw new ApplicationException("No se encontró ruta de archivo de respuesta de la DIAN");

					if (empresa_obligado.IntPdfCampoDian == true && interoperabilidad == false)
					{

						//Proceso para obtener la fecha y hora de la respuesta de la DIAN
						//string ruta_archivo = string.Format(@"{0}\{1}.xml", documento_result.RutaArchivosProceso.Replace("XmlFacturaE", LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEConsultaDian), documento_result.NombreXml);
						FileStream xml_reader_serializacion = new FileStream(ruta_xml, FileMode.Open);
						HGInetUBLv2_1.ApplicationResponseType obj_acuse_serializado = new HGInetUBLv2_1.ApplicationResponseType();
						XmlSerializer serializacion1 = new XmlSerializer(typeof(HGInetUBLv2_1.ApplicationResponseType));
						obj_acuse_serializado = (HGInetUBLv2_1.ApplicationResponseType)serializacion1.Deserialize(xml_reader_serializacion);
						string fecha_doc_resp = obj_acuse_serializado.IssueDate.Value.ToString("yyyy-MM-dd");
						string hora_doc_resp = obj_acuse_serializado.IssueTime.Value.ToString();
						xml_reader_serializacion.Close();

						// ruta física del archivo PDF 
						string ruta_pdf = string.Format(@"{0}\{1}.pdf", carpeta_archivos.Replace(LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEConsultaDian, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian), nombre_archivo);

						// texto para generar en el PDF, revisar si ponemos sólo hasta minutos la fecha
						string texto = string.Format("Fecha Validación DIAN: {0} {1}  DOCUMENTO ELECTRÓNICO GENERADO POR HGI S.A.S NIT 811021438-4", fecha_doc_resp, hora_doc_resp);

						// ejecución para poner el texto en el PDF
						string ruta_pdf_resultado = ruta_pdf.Replace(nombre_archivo, string.Format("{0}_resultado.pdf", nombre_archivo));
						ruta_pdf_resultado = LibreriaGlobalHGInet.Funciones.Pdf.AgregarTexto(ruta_pdf, ruta_pdf_resultado, texto, (float)empresa_obligado.IntPdfCampoDianPosX, (float)empresa_obligado.IntPdfCampoDianPosY, true);
					}

				}
				catch (Exception excepcion)
				{
					Guid peticion = (string.IsNullOrEmpty(id_peticion) ? Guid.Empty : Guid.Parse(id_peticion));
					int estado_doc = Ctl_Documento.ObtenerCategoria(documento.IntIdEstado);
					string mensaje = (string.IsNullOrEmpty(id_peticion) ? "Reenvio de Documento" : "Notificación Documento");
					clase_auditoria = new Ctl_DocumentosAudit();
					clase_auditoria.Crear(documento.StrIdSeguridad, peticion, documento.StrEmpresaFacturador, ProcesoEstado.EnvioEmailAcuse, TipoRegistro.Proceso, Procedencia.Plataforma, string.Empty, excepcion.Message, string.Empty, documento.StrPrefijo, Convert.ToString(documento.IntNumero), estado_doc);
					Ctl_Log.Guardar(excepcion, LibreriaGlobalHGInet.RegistroLog.MensajeCategoria.Servicio, LibreriaGlobalHGInet.RegistroLog.MensajeTipo.Error, LibreriaGlobalHGInet.RegistroLog.MensajeAccion.envio);
				}

				#endregion


				#region Asunto del correo
				//9. Entrega y recepción de los documentos electrónicos - Anexo Tecnico V1.8
				//Asunto: NIT del Facturador Electrónico; Nombre del Facturador Electrónico; Número del Documento Electrónico (campo cbc:ID);
				//Código del tipo de documento según tabla 13.1.3; Nombre comercial del facturador; 

				string numero_doc = string.Format("{0}{1}", documento.StrPrefijo, documento.IntNumero.ToString());

				var objeto = (dynamic)null;
				var documento_obj = (dynamic)null;
				if (string.IsNullOrEmpty(nombre_comercial))
				{
					objeto = Ctl_Documento.ConvertirServicio(documento, true);
				}

				// obtiene el tipo de documento
				TipoDocumento tipo_documento = Enumeracion.ParseToEnum<TipoDocumento>(documento.IntDocTipo);
				string tipodoc_asunto = string.Empty;
				if (tipo_documento == TipoDocumento.Factura)
				{
					tipodoc_asunto = "01";
					if (string.IsNullOrEmpty(nombre_comercial))
					{
						nombre_comercial = string.IsNullOrEmpty(objeto.DatosFactura.DatosObligado.NombreComercial) ? objeto.DatosFactura.DatosObligado.RazonSocial : objeto.DatosFactura.DatosObligado.NombreComercial;
					}

					if (objeto != null)
					{
						if (string.IsNullOrEmpty(telefono))
							telefono = objeto.DatosFactura.DatosObligado.Telefono;

						documento_obj = objeto.DatosFactura;
					}

				}
				else if (tipo_documento == TipoDocumento.NotaCredito)
				{
					tipodoc_asunto = "91";
					if (string.IsNullOrEmpty(nombre_comercial))
					{
						nombre_comercial = string.IsNullOrEmpty(objeto.DatosNotaCredito.DatosObligado.NombreComercial) ? objeto.DatosNotaCredito.DatosObligado.RazonSocial : objeto.DatosNotaCredito.DatosObligado.NombreComercial;
					}

					if (objeto != null)
					{
						if (string.IsNullOrEmpty(telefono))
							telefono = objeto.DatosNotaCredito.DatosObligado.Telefono;

						documento_obj = objeto.DatosNotaCredito;
					}

				}
				else if (tipo_documento == TipoDocumento.NotaDebito)
				{
					tipodoc_asunto = "92";
					if (string.IsNullOrEmpty(nombre_comercial))
					{
						nombre_comercial = string.IsNullOrEmpty(objeto.DatosNotaDebito.DatosObligado.NombreComercial) ? objeto.DatosNotaDebito.DatosObligado.RazonSocial : objeto.DatosNotaDebito.DatosObligado.NombreComercial;
					}

					if (objeto != null)
					{
						if (string.IsNullOrEmpty(telefono))
							telefono = objeto.DatosNotaDebito.DatosObligado.Telefono;

						documento_obj = objeto.DatosNotaDebito;
					}


				}

				string asunto = string.Empty;

				//Si tiene linea de negocio se agrega en el asunto
				if (!string.IsNullOrEmpty(documento.StrLineaNegocio))
				{
					asunto = string.Format("{0};{1};{2};{3};{4};{5}", empresa_obligado.StrIdentificacion, empresa_obligado.StrRazonSocial, numero_doc, tipodoc_asunto, nombre_comercial, documento.StrLineaNegocio);
				}
				else
				{
					asunto = string.Format("{0};{1};{2};{3};{4}", empresa_obligado.StrIdentificacion, empresa_obligado.StrRazonSocial, numero_doc, tipodoc_asunto, nombre_comercial);
				}


				/*
				if (empresa_obligado.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
				{
					asunto = string.Format("{0} (HABILITACIÓN)", Constantes.AsuntoNotificacionDocumento);
				}
				else
				{
					asunto = Constantes.AsuntoNotificacionDocumento;
				} */
				#endregion

				// envía como email de respuesta facturador electrónico
				DestinatarioEmail remitente = new DestinatarioEmail();
				remitente.Nombre = empresa_obligado.StrRazonSocial;
				remitente.Email = empresa_obligado.StrMailEnvio;

				//Si el facturador es de interoperabilidad y no tiene correo se pone el de la plataforma
				if (string.IsNullOrEmpty(remitente.Email) && interoperabilidad == true)
				{
					remitente.Email = Constantes.EmailRemitente;
				}

				// obtiene los datos del adquiriente
				Ctl_Empresa adquiriente = new Ctl_Empresa();
				empresa_adquiriente = adquiriente.Obtener(documento.StrEmpresaAdquiriente);

				List<DestinatarioEmail> correos_destino = new List<DestinatarioEmail>();

				// recibe el email el adquiriente
				DestinatarioEmail destinatario = new DestinatarioEmail();
				destinatario.Nombre = empresa_adquiriente.StrRazonSocial;

				//*********Agregar validacion si el documento es de y para clientes de HGI para que no sobreescriba el correo
				string correo_registrado = string.Empty;
				//Se valida si es un documento de produccion para sobrescribir el correo registrado en la DIAN resolucion 042
				if (empresa_obligado.IntHabilitacion == Habilitacion.Produccion.GetHashCode() && reenvio_documento == false && interoperabilidad == false)
				{
					//se obtiene correo registrado para enviar el documento a este(Resolucion 042)
					Ctl_ObtenerCorreos correo_recep = new Ctl_ObtenerCorreos();
					correo_registrado = correo_recep.Obtener(empresa_adquiriente.StrIdentificacion);
				}



				if (string.IsNullOrWhiteSpace(nuevo_email) && interoperabilidad == false)
				{
					//Se valida si tiene correo registrado para enviar el documento a este(Resolucion 042)
					if (!string.IsNullOrEmpty(correo_registrado))
					{
						destinatario.Email = correo_registrado;
					}
					else
					{
						destinatario.Email = empresa_adquiriente.StrMailRecepcion;
					}

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

						//Se valida si tiene correo registrado para enviar el documento a este(Resolucion 042)
						if (!string.IsNullOrEmpty(correo_registrado) && !nuevo_email.Contains(correo_registrado))
						{
							destinatario.Email = correo_registrado;
							correos_destino.Add(destinatario);
						}


					}
					else
					{
						//Se valida si tiene correo registrado para enviar el documento a este(Resolucion 042)
						if (!string.IsNullOrEmpty(correo_registrado))
						{
							destinatario.Email = correo_registrado;
							correos_destino.Add(destinatario);

							if (!nuevo_email.Equals(correo_registrado))
							{
								// recibe el email el adquiriente
								destinatario = new DestinatarioEmail();
								destinatario.Nombre = empresa_adquiriente.StrRazonSocial;
								destinatario.Email = nuevo_email;
								correos_destino.Add(destinatario);
							}


						}
						else
						{
							destinatario.Email = nuevo_email;
							correos_destino.Add(destinatario);
						}

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
						mensaje = mensaje.Replace("{NombreComercial}", nombre_comercial);
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
							IdPago = (empresa_obligado.IntManejaPagoE) ? true : false;

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

						bool contiene_pdf = true;

						if (interoperabilidad == false && string.IsNullOrEmpty(documento.StrUrlArchivoPdf))
						{
							throw new ApplicationException("No se encontró ruta de archivo pdf");
						}
						else if (interoperabilidad == true && string.IsNullOrEmpty(documento.StrUrlArchivoPdf))
						{
							contiene_pdf = false;
						}
						//Se quita que el pdf vaya como archivo segun resolucion 042 de mayo de 2020
						/*
						byte[] bytes_pdf = Archivo.ObtenerWeb(documento.StrUrlArchivoPdf);
						string ruta_fisica_pdf = Convert.ToBase64String(bytes_pdf);
						


						if (!string.IsNullOrEmpty(ruta_fisica_pdf))
						{
							Adjunto adjunto = new Adjunto();
							adjunto.ContenidoB64 = ruta_fisica_pdf;
							adjunto.Nombre = nombre_pdf;
							archivos.Add(adjunto);
						}*/

						

						string nombre_xml = Path.GetFileName(documento.StrUrlArchivoUbl);

						//Se anexa el XML-UBL solo para esta version,si no cumple segun Anexo solo se debe enviar el AttachedDocument y el PDF
						if (documento.IntVersionDian == 1)
						{

							if (string.IsNullOrEmpty(documento.StrUrlArchivoUbl))
								throw new ApplicationException("No se encontró ruta de archivo xml");

							byte[] bytes_xml = Archivo.ObtenerWeb(documento.StrUrlArchivoUbl);
							string ruta_fisica_xml = Convert.ToBase64String(bytes_xml);
							//string nombre_xml = Path.GetFileName(documento.StrUrlArchivoUbl);

							if (!string.IsNullOrEmpty(ruta_fisica_xml))
							{
								Adjunto adjunto = new Adjunto();
								adjunto.ContenidoB64 = ruta_fisica_xml;
								adjunto.Nombre = nombre_xml;
								archivos.Add(adjunto);
							}
						}

						//****Proceso donde se hace el zip que contiene un AttachDocument tipo xml, una Representacion grafica tipo pdf y un archivo Anexo tipo zip
						// ruta física del xml
						string carpeta_xml = string.Format("{0}\\{1}\\{2}", plataforma.RutaDmsFisica, Constantes.CarpetaFacturaElectronica, empresa_obligado.StrIdSeguridad.ToString());
						carpeta_xml = string.Format(@"{0}\{1}", carpeta_xml, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian);

						string nombre_archivo = nombre_xml.Replace("face", "attach");

						bool archivo_attach = Archivo.ValidarExistencia(string.Format(@"{0}\{1}", carpeta_xml, nombre_archivo));

						//if (archivo_attach == false)
						//{
						//	archivo_attach = Archivo.ValidarExistencia(string.Format(@"{0}\{1}", carpeta_xml, nombre_archivo.Replace("xml", "zip")));
						//}

						bool attached = false;

						//Se agrega validacion por ajustes en el namespace de los documentos electronicos
						DateTime fecha_valid = new DateTime(2021, 5, 10, 0, 0, 0);
						if (reenvio_documento == true && documento.DatFechaIngreso.Date <= fecha_valid.Date)
							archivo_attach = false;

						if (archivo_attach == false && documento.IntVersionDian == 2)
						{
							if (documento_obj != null)
							{
								attached = Ctl_Documento.ConvertirAttachedDoc(documento_obj, documento, empresa_obligado);
							}
							else
							{
								attached = Ctl_Documento.ConvertirAttachedDoc(null, documento, empresa_obligado);
							}

						}
						else if (documento.IntVersionDian == 2)
						{
							attached = true;
						}

						// ruta del zip
						string ruta_zip = string.Format(@"{0}\{1}", carpeta_xml, nombre_archivo.Replace("xml", "zip"));

						if (attached == true && documento.IntVersionDian == 2)
						{

							//Proceso para agregar la respuesta de la DIAN
							try
							{
								if (Archivo.ValidarExistencia(ruta_zip))
									Archivo.Borrar(ruta_zip);

								// genera la compresión del archivo en zip
								using (ZipArchive archive = ZipFile.Open(ruta_zip, ZipArchiveMode.Update))
								{
									archive.CreateEntryFromFile(string.Format(@"{0}\{1}", carpeta_xml, nombre_archivo), Path.GetFileName(nombre_archivo));

									if (contiene_pdf == true)
									{
										string nombre_pdf = Path.GetFileName(documento.StrUrlArchivoPdf);
										archive.CreateEntryFromFile(string.Format(@"{0}\{1}", carpeta_xml, nombre_pdf), Path.GetFileName(nombre_pdf));
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
													// ruta física del xml
													string carpeta_anexo = string.Format("{0}\\{1}\\{2}", plataforma.RutaDmsFisica, Constantes.CarpetaFacturaElectronica, empresa_obligado.StrIdSeguridad.ToString());
													carpeta_anexo = string.Format(@"{0}\{1}", carpeta_anexo, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEAnexos);

													archive.CreateEntryFromFile(string.Format(@"{0}\{1}", carpeta_anexo, nombre_anexo), Path.GetFileName(nombre_anexo));

													//Adjunto adjunto = new Adjunto();
													//adjunto.ContenidoB64 = ruta_fisica_anexo;
													//adjunto.Nombre = nombre_anexo;
													//archivos.Add(adjunto);
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

									archive.Dispose();

								}

								byte[] bytes_applications = null;

								if (!string.IsNullOrEmpty(documento.StrUrlArchivoZip))
								{
									bytes_applications = Archivo.ObtenerWeb(documento.StrUrlArchivoZip.Replace("ws", "attach"));
								}
								else
								{
									bytes_applications = Archivo.ObtenerBytes(ruta_zip);
								}

								string ruta_fisica_appl = Convert.ToBase64String(bytes_applications);

								string nombre_xml_app = nombre_archivo.Replace("xml", "zip");

								if (!string.IsNullOrEmpty(ruta_fisica_appl))
								{
									Adjunto adjunto = new Adjunto();
									adjunto.ContenidoB64 = ruta_fisica_appl;
									adjunto.Nombre = nombre_xml_app;
									archivos.Add(adjunto);

								}

								if (Archivo.ValidarExistencia(string.Format(@"{0}\{1}", carpeta_xml, nombre_archivo)))
									Archivo.Borrar(string.Format(@"{0}\{1}", carpeta_xml, nombre_archivo));

							}
							catch (Exception excepcion)
							{
								RegistroLog.EscribirLog(new ApplicationException("Excepción", excepcion), MensajeCategoria.Convertir, MensajeTipo.Error, MensajeAccion.ninguna);

								try
								{
									Ctl_Log.Guardar(excepcion, MensajeCategoria.Archivos, MensajeTipo.Error, MensajeAccion.creacion, "No fue posible guardar el attach en archivo zip");
									string msg_excepcion = Excepcion.Mensaje(excepcion);
								}
								catch (Exception) { }

								//try
								//{
								//	int estado_doc = Ctl_Documento.ObtenerCategoria(documento.IntIdEstado);
								//	clase_auditoria.Crear(documento.StrIdSeguridad, Guid.Empty, empresa_obligado.StrIdentificacion, proceso, TipoRegistro.Proceso, procedencia, usuario, "No fue posible guardar el attach en archivo zip", string.Format("{0}", excepcion.InnerException), documento.StrPrefijo, Convert.ToString(documento.IntNumero), estado_doc);
								//}
								//catch (Exception) {}
							}
						}

						// envía el correo electrónico
						respuesta_email = EnviarEmail(documento.StrIdSeguridad.ToString(), false, mensaje, asunto, true, remitente, correos_destino, null, null, "", "", archivos);

						if (respuesta_email.Count > 0)
						{
							try
							{
								Ctl_ProcesosCorreos proceso_correo = new Ctl_ProcesosCorreos();
								TblProcesoCorreo correo_doc = proceso_correo.Obtener(documento.StrIdSeguridad);
								if (correo_doc == null)
									correo_doc = proceso_correo.Crear(documento.StrIdSeguridad);

								//Valida que el registro sea nuevo para que lo haga solo en el proceso principal
								if (correo_doc.IntEnvioMail == false && string.IsNullOrEmpty(correo_doc.StrIdMensaje))
								{

									correo_doc.IntEnvioMail = true;

									//obtengo el primer MessageID
									try
									{
										correo_doc.StrIdMensaje = respuesta_email.FirstOrDefault().Data.FirstOrDefault().MessageID.ToString();
										correo_doc.StrMailEnviado = respuesta_email.FirstOrDefault().Data.FirstOrDefault().Email;
									}
									catch (Exception excepcion)
									{
										Ctl_Log.Guardar(excepcion, LibreriaGlobalHGInet.RegistroLog.MensajeCategoria.Servicio, LibreriaGlobalHGInet.RegistroLog.MensajeTipo.Error, LibreriaGlobalHGInet.RegistroLog.MensajeAccion.actualizacion, "Error asiganando el MessageID y Email");
										correo_doc.IntEnvioMail = false;
									}

								}
								else if (correo_doc.IntEnvioMail == true)
								{
									correo_doc.IntEnvioMail = false;
								}
								else
								{
									correo_doc.IntEnvioMail = true;
								}

								//Actualizo tabla de correos
								correo_doc = proceso_correo.Actualizar(correo_doc);
							}
							catch (Exception e)
							{
								Ctl_Log.Guardar(e, LibreriaGlobalHGInet.RegistroLog.MensajeCategoria.Servicio, LibreriaGlobalHGInet.RegistroLog.MensajeTipo.Error, LibreriaGlobalHGInet.RegistroLog.MensajeAccion.actualizacion, "Error Tratando de Actualizar la TblProcesoCorreo");
							}

						}

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
					clase_auditoria.Crear(documento.StrIdSeguridad, Guid.Empty, empresa_obligado.StrIdentificacion, proceso, TipoRegistro.Proceso, procedencia, usuario, "No fue posible el envio del documento, favor validar con el Adquiriente ó hacer el reenvío del documento desde nuestra Plataforma ", string.Format("{0}", excepcion.InnerException), documento.StrPrefijo, Convert.ToString(documento.IntNumero), estado_doc);
				}
				catch (Exception) { }

				//if (respuesta_email == null || respuesta_email.Count == 0)
				//{
				//	try
				//	{
				//		Ctl_EnvioCorreos email = new Ctl_EnvioCorreos();
				//		List<MensajeEnvio> notificacion = email.NotificacionCorreofacturador(documento, empresa_adquiriente.StrTelefono, nuevo_email, "Error enviando Correo", documento.StrIdSeguridad.ToString());

				//	}
				//	catch (Exception) { }
				//}

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
							IdPago = (documento.TblEmpresasFacturador.IntManejaPagoE) ? true : false;

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
		/// Envía mail para restablecer contraseña al usuario de Pagos
		/// </summary>
		/// <param name="empresa">Datos del Obligado o del Adquiriente</param>
		/// <param name="usuario">Datos del usuario guardado en BD</param>
		public void RestablecerClavePagos(TblEmpresas empresa, TblUsuariosPagos usuario)
		{

			try
			{
				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;

				if (empresa == null)
					throw new ApplicationException(string.Format("No se encontró información del Nit {0} ", empresa.StrIdentificacion));

				if (usuario == null)
					throw new ApplicationException(string.Format("No se encontró información del usuario {0}", usuario));

				string fileName = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), Constantes.RutaPlantillaRestablecerPagos);

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
						mensaje = mensaje.Replace("{RutaUrl}", string.Format("{0}{1}", plataforma.RutaPublica, Constantes.PaginaRestablecerClavePagos.Replace("{id_seguridad}", usuario.StrIdCambioClave.ToString())));
						mensaje = mensaje.Replace("{ImagenTercero}", string.Format(" src={0}{1}/Scripts/Images/Terceros/{2}.png{3} ", '"', plataforma.RutaPublica, empresa.StrIdSeguridad, '"'));
						mensaje = mensaje.Replace("{RutaAcceso}", plataforma.RutaPublica);

						string asunto = "Restablecimiento de Contraseña";

						DestinatarioEmail remitente = new DestinatarioEmail();

						//Confirmar Empresa													
						remitente.Nombre = empresa.StrRazonSocial;
						remitente.Email = empresa.StrMailPagos;

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
								estado_respuesta = "Acuse";
								break;
							case 2:
								estado_respuesta = "Rechazada";
								break;
							case 3:
								estado_respuesta = "Aprobado Tácito";
								break;
							case 4:
								estado_respuesta = "Aprobada";
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

			//Obtengo a la ruta de plataforma de servicio
			string ruta_envio = string.Empty;
			ruta_envio = ObtenerUrl();
			if (string.IsNullOrEmpty(ruta_envio))
				ruta_envio = plataforma_datos.RutaHginetMail;

			MensajeResumenGlobal obj_peticion = new MensajeResumenGlobal();
			obj_peticion.identificacion = plataforma_datos.IdentificacionHGInetMail;
			obj_peticion.serial = plataforma_datos.LicenciaHGInetMail;
			obj_peticion.id_mensaje = MessageID;

			ClienteRest<MensajeResumen> cliente = new ClienteRest<MensajeResumen>(string.Format("{0}/Api/ObtenerResumenMensaje", ruta_envio), TipoContenido.Applicationjson.GetHashCode(), "");
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
		public List<MensajeEnvio> EnviaNotificacionAlertaDIAN(string Facturador, string Documento, List<String> ListaNotificacion, int Proceso, bool Resultado, string mail, int tipo_asunto)
		{
			try
			{
				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;
				List<MensajeEnvio> RespuestaMail = new List<MensajeEnvio>();

				string fileName = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), Constantes.RutaPlantillaAlertaDocumentoDIAN);

				string asunto = string.Empty;
				string titulo = string.Empty;

				switch (tipo_asunto)
				{
					case 1:
						asunto = "ALERTA DE INCONSISTENCIAS DE DOCUMENTO ELECTRÓNICO EN LA DIAN";
						titulo = "DOCUMENTO ELECTRÓNICO CON INCONSISTENCIAS EN LA DIAN";
						break;
					case 2:
						asunto = "ALERTA DE INCONSISTENCIAS DE PROCESAMIENTO DE CORREO DE INTEROPERABILIDAD";
						titulo = "CORREO DE INTEROPERABILIDAD CON INCONSISTENCIAS EN EL PROCESAMIENTO ";
						break;
					case 3:
						asunto = "ALERTA DE INCONSISTENCIAS EN VALIDACION DEL CORREO ENVIADO";
						titulo = "CORREO ENVIADO CON INCONSISTENCIAS EN LA VALIDACION";
						break;
					default:
						asunto = "ALERTA DE INCONSISTENCIAS DE DOCUMENTO ELECTRÓNICO EN LA DIAN";
						titulo = "DOCUMENTO ELECTRÓNICO CON INCONSISTENCIAS EN LA DIAN";
						break;
				}


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

						mensaje = mensaje.Replace("{Titulo}", titulo);
						mensaje = mensaje.Replace("{TablaHtml}", detalle);
						mensaje = mensaje.Replace("{Facturador}", facturador.StrRazonSocial);
						mensaje = mensaje.Replace("{Documento}", Documento);
						mensaje = mensaje.Replace("{Estado}", (Resultado) ? "Recibido" : "No Recibido");
						mensaje = mensaje.Replace("{Proceso}", (Proceso == 0) ? "Desconocido" : (Proceso == 1) ? "Envío" : (Proceso == 2) ? "Consulta" : "Interoperabilidad");


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
