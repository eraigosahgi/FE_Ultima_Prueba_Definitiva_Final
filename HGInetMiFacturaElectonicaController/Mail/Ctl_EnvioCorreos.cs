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
using HGInetUtilidadAzure.Almacenamiento;
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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
		public List<MensajeEnvio> EnviarEmail(string id_seguridad, bool uno_a_uno, string mensaje, string asunto, bool contenido_html, DestinatarioEmail correo_remitente, List<DestinatarioEmail> correos_destino, List<DestinatarioEmail> correos_copia = null, List<DestinatarioEmail> correos_copia_oculta = null, string ruta_plantilla_html = "", string tag_mensaje = "", List<Adjunto> rutas_adjuntos = null)
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
				//ruta_envio = "";
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
						Tipo = LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.TipoDestinatario.Desde
					};
					emails.Add(destino);

					// Reply-To
					//***Se agrega validacion para que solo pueda responder al primer remitente
					if (correo_remitente.Email.Contains(",") || correo_remitente.Email.Contains(";"))
					{
						//744333 - vienen 2 correos en el facturador y es un documento de otro proveedor
						if (correo_remitente.Email.Contains(";"))
						{
							correo_remitente.Email = correo_remitente.Email.Replace(";", ",");
						}
						List<string> List_rem = Coleccion.ConvertirLista(correo_remitente.Email);
						correo_remitente.Email = List_rem[0];
					}


					Destinatario respuesta = new Destinatario()
					{
						Email = correo_remitente.Email,
						Nombre = correo_remitente.Nombre,
						Tipo = LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.TipoDestinatario.Responder
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
								Tipo = LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.TipoDestinatario.Para
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
								Tipo = LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.TipoDestinatario.Copia
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
								Tipo = LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.TipoDestinatario.CopiaOculta
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

					respuesta_email = Enviar(ruta_envio, plataforma.LicenciaHGInetMail, plataforma.IdentificacionHGInetMail, mensajes, plataforma.IdAplicacionHGInetMail);


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
			// obtiene el tipo de documento
			TipoDocumento tipo_documento = Enumeracion.ParseToEnum<TipoDocumento>(documento.IntDocTipo);
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

					if (empresa_obligado.IntPdfCampoDian == true && interoperabilidad == false && tipo_documento != TipoDocumento.Nomina && tipo_documento != TipoDocumento.NominaAjuste)
					{

						Ctl_Documento _documento = new Ctl_Documento();
						_documento.GeneracionFechaDian(empresa_obligado, documento);
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
				if (string.IsNullOrEmpty(nombre_comercial) && tipo_documento.GetHashCode() < TipoDocumento.AcuseRecibo.GetHashCode())
				{
					objeto = Ctl_Documento.ConvertirServicio(documento, true);
				}

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

					//739793 - Se agrega codigo de Exportacion en el asunto.
					if (documento.IntTipoOperacion == 2)
						tipodoc_asunto = "02";
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
				//Se solicita por parte de gerencia que el correo se envie solo a los correos que lleguen en el objeto indpendiente del listado de la DIAN 2023-02-15
				//Se valida si es un documento de produccion para sobrescribir el correo registrado en la DIAN resolucion 042
				//if (empresa_obligado.IntHabilitacion == Habilitacion.Produccion.GetHashCode() && reenvio_documento == false && interoperabilidad == false)
				//{
				//	//se obtiene correo registrado para enviar el documento a este(Resolucion 042)
				//	Ctl_ObtenerCorreos correo_recep = new Ctl_ObtenerCorreos();
				//	correo_registrado = correo_recep.Obtener(empresa_adquiriente.StrIdentificacion);
				//}



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

					//Si el adquiriente es de HGI es por que es Facturador, esta habilitado y se puede generar eventos basicos en la DIAN
					bool adquiriente_hgi = (empresa_adquiriente.IntHabilitacion > Habilitacion.Valida_Objeto.GetHashCode() && empresa_adquiriente.IntObligado == true && empresa_adquiriente.IntAdquiriente == true && empresa_adquiriente.IntIdEstado == EstadoEmpresa.ACTIVA.GetHashCode()) ? true : false;

					if (file != null)
					{

						if (plataforma.RutaPublica.Contains("habilitacion") || plataforma.RutaPublica.Contains("localhost"))
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
						//TipoDocumento doc_tipo = Enumeracion.GetEnumObjectByValue<TipoDocumento>(documento.IntDocTipo);
						string titulo_documento = Enumeracion.GetDescription(tipo_documento);

						mensaje = mensaje.Replace("{TipoDocumento}", titulo_documento);
						mensaje = mensaje.Replace("{NumeroDocumento}", String.Format("{0}{1}", documento.StrPrefijo, documento.IntNumero.ToString()));
						mensaje = mensaje.Replace("{FechaDocumento}", documento.DatFechaDocumento.ToString(Fecha.formato_fecha_hginet));
						mensaje = mensaje.Replace("{TotalDocumento}", String.Format("{0:###,##0.}", documento.IntVlrTotal));

						string ruta_acuse = string.Format("{0}{1}", plataforma.RutaPublica, Constantes.PaginaAcuseRecibo.Replace("{id_seguridad}", documento.StrIdSeguridad.ToString()));

						mensaje = mensaje.Replace("{RutaAcceso}", plataforma.RutaPublica);

						//Si el adquiriente no es cliente de HGI no se permite hacer acuse por medio del correo puesto que lo debe hacer por medio del proveedor que tenga
						if (tipo_documento != TipoDocumento.Nomina && tipo_documento != TipoDocumento.NominaAjuste)
						{

							mensaje = mensaje.Replace("{RutaUrl}", ruta_acuse);

							if (adquiriente_hgi == false)
							{
								mensaje = mensaje.Replace("Para continuar con el proceso debes realizar el acuse de recibo {PSETexto}:", "");
							}

							string boton_acuse = "<td style='border:2px solid #b0afaf;border-radius:3px;color:#ffffff;cursor:auto;padding:10px 25px;' align='center' valign='middle' bgcolor='#040461'><a href='" + ruta_acuse + "' style='text-decoration:none;background:#040461;color:#ffffff;font-family:Ubuntu, Helvetica, Arial, sans-serif;font-size:17px;font-weight:normal;line-height:120%;text-transform:none;margin:0px;' target='_blank'>Ver Documentos</a></td>";

							mensaje = mensaje.Replace("{Acuse}", boton_acuse);

						}


						bool IdPago = false;

						if (documento.TblEmpresasResoluciones != null)
							IdPago = (empresa_obligado.IntManejaPagoE) ? true : false;

						if (tipo_documento == TipoDocumento.Factura && IdPago)
						{
							string total_a_pagar = String.Format(@"<tr>
															<td class=""tg-yzt1"">Total a Pagar:</td>
															<td class=""tg-3we0"">$ {0}</td>
													</tr>", String.Format("{0:###,##0.}", documento.IntValorPagar));

							mensaje = mensaje.Replace("{TotalPagar}", total_a_pagar);

							string ruta_pse = ruta_acuse + "&Zpago=true";

							mensaje = mensaje.Replace("{PSETexto}", "o el pago del documento");

							string otro_boton = "<td style='border:2px solid #b0afaf;border-radius:3px;color:#ffffff;cursor:auto;padding:10px 25px;' align='center' valign='middle' bgcolor='#040461'><a href='" + ruta_pse + "' style='text-decoration:none;background:#040461;color:#ffffff;font-family:Ubuntu, Helvetica, Arial, sans-serif;font-size:17px;font-weight:normal;line-height:120%;text-transform:none;margin:0px;' target='_blank'>Pagar</a></td>";

							mensaje = mensaje.Replace("{PSE}", otro_boton);

						}

						if (tipo_documento == TipoDocumento.Nomina || tipo_documento == TipoDocumento.NominaAjuste)
						{

							mensaje = mensaje.Replace("Adjunto encontrará los archivos relacionados con el documento generado. Para continuar con el proceso debes realizar el acuse de recibo {PSETexto}:", "Adjunto encontrará la representación gráfica relacionada con el documento generado.");

							mensaje = mensaje.Replace("Si el enlace anterior no funciona, copie y pegue la siguiente URL en su navegador web:", "Si el adjunto no funciona, copie y pegue la siguiente URL en su navegador web:");

							mensaje = mensaje.Replace("{RutaUrl}", documento.StrUrlArchivoPdf);

							mensaje = mensaje.Replace("{Acuse}", "");
						}

						mensaje = mensaje.Replace("{TotalPagar}", "");
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
						string ruta_fisica_pdf = Convert.ToBase64String(bytes_pdf);
						
						if (!string.IsNullOrEmpty(ruta_fisica_pdf))
						{
							Adjunto adjunto = new Adjunto();
							adjunto.ContenidoB64 = ruta_fisica_pdf;
							adjunto.Nombre = nombre_pdf;
							archivos.Add(adjunto);
						}*/

						//****Proceso donde se hace el zip que contiene un AttachDocument tipo xml, una Representacion grafica tipo pdf y un archivo Anexo tipo zip
						//Para documentos diferentes a la Nomina
						if (tipo_documento != TipoDocumento.Nomina && tipo_documento != TipoDocumento.NominaAjuste)
						{

							string nombre_xml = HGInetUBLv2_1.NombramientoArchivo.ObtenerXml(documento.IntNumero.ToString(), documento.StrEmpresaFacturador, tipo_documento, documento.StrPrefijo);//Path.GetFileName(documento.StrUrlArchivoUbl);
                            
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

							bool generar_attach = true;

							byte[] bytes_applications = null;

							bool generar_att_zip = true;

							// ruta física del xml
							string carpeta_xml = string.Format("{0}\\{1}\\{2}", plataforma.RutaDmsFisica, Constantes.CarpetaFacturaElectronica, empresa_obligado.StrIdSeguridad.ToString());
							carpeta_xml = string.Format(@"{0}\{1}", carpeta_xml, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian);

							string nombre_archivo = HGInetUBLv2_1.NombramientoArchivo.ObtenerXml(documento.IntNumero.ToString(), documento.StrEmpresaFacturador, TipoDocumento.Attached, documento.StrPrefijo);//nombre_xml.Replace("face", "attach");

							bool archivo_attach = Archivo.ValidarExistencia(string.Format(@"{0}\{1}.xml", carpeta_xml, nombre_archivo));

							//Contingencia de la DIAN 2024-03-09 desde las 6:00 am hasta las 6:00 PM
							DateTime fecha_ini_cont = new DateTime(2024, 03, 09, 6, 0, 0);
							DateTime fecha_fin_cont = new DateTime(2024, 03, 09, 18, 0, 0);

							if (documento.DatFechaIngreso >= fecha_ini_cont && documento.DatFechaIngreso < fecha_fin_cont && archivo_attach == true)
							{
								Archivo.Borrar(string.Format(@"{0}\{1}.xml", carpeta_xml, nombre_archivo));
								archivo_attach = false;
							}

							if (archivo_attach == true && reenvio_documento == false)
								generar_attach = false;

							if (archivo_attach == true && reenvio_documento == true)
								generar_attach = false;

							//Variable que indica si el adjunto lo toma del blob
							bool zip_attach_blob = false;

							//Se agrega fecha de validacion para que haga la carpeta cuando sean documentos que son nuevos o estan fisicos
							DateTime fecha_cambio_ser = new DateTime(2022, 11, 09, 20, 59, 59);

							if (generar_attach == true)
							{
								try
								{
                                    archivo_attach = false;
                                    zip_attach_blob = false;

                                    if (documento.StrUrlArchivoUbl.Contains("hgidocs.blob"))
									{
										Ctl_AlmacenamientoDocs ctl_alm = new Ctl_AlmacenamientoDocs();
										List<TblAlmacenamientoDocs> list_docs_alm = ctl_alm.Obtener(documento.StrIdSeguridad);

										//valido si tengo guardado el zip del attach con el PDF como blob
										if (list_docs_alm.Where(x => x.IntConsecutivo.Equals(TipoArchivoStorage.ZIPAttached.GetHashCode())).FirstOrDefault() != null)
										{
											TblAlmacenamientoDocs zip_attach = list_docs_alm.Where(x => x.IntConsecutivo.Equals(TipoArchivoStorage.ZIPAttached.GetHashCode())).FirstOrDefault();

											AzureStorage conexion = HgiConfiguracion.GetConfiguration().AzureStorage;

											string nombre_contenedor = string.Format("files-hgidocs-{0}", documento.DatFechaIngreso.Year);

											BlobController contenedor = new BlobController(conexion.connectionString, nombre_contenedor);

											byte[] bytes_applications_b = contenedor.LecturaBlobBase64(Path.GetExtension(zip_attach.StrUrlActual), Path.GetFileNameWithoutExtension(zip_attach.StrUrlActual));
                                                                                      
                                            string zip_blob = Convert.ToBase64String(bytes_applications_b);                                            

                                            if (bytes_applications_b.Length > 25)
                                            {
                                                if (!string.IsNullOrEmpty(zip_blob))
                                                {
                                                    Adjunto adjunto = new Adjunto();
                                                    adjunto.ContenidoB64 = zip_blob;
                                                    adjunto.Nombre = Path.GetFileName(zip_attach.StrUrlActual);
                                                    archivos.Add(adjunto);

                                                    archivo_attach = true;
                                                    zip_attach_blob = true;
                                                }
                                            }
                                            else
                                            {
                                                string ruta_fisica_zip = string.Format(@"{0}\{1}.zip", carpeta_xml, nombre_archivo);

                                                if (Archivo.ValidarExistencia(ruta_fisica_zip))
                                                {
                                                    bytes_applications = Archivo.ObtenerBytes(ruta_fisica_zip);

                                                    if (bytes_applications.Length < 25)
                                                    {
                                                        Archivo.Borrar(ruta_fisica_zip);
                                                        archivo_attach = false;
                                                        generar_att_zip = true;
                                                    }
                                                    else
                                                    {
                                                        archivo_attach = true;
                                                        zip_attach_blob = false;
                                                    }
                                                }
                                                else
                                                {
                                                    Archivo.Borrar(ruta_fisica_zip);
                                                    archivo_attach = false;
                                                    generar_att_zip = true;
                                                }
                                            }                                                                     
                                         }
									}
									else
									{

										//Si es un documento emitido por nosotros este archivo lo tenemos, si es de otro proveedor no lo tenemos
										if (!string.IsNullOrEmpty(documento.StrUrlArchivoZip))
										{
											string nombre_cambio = Path.GetFileName(documento.StrUrlArchivoZip);
											bytes_applications = Archivo.ObtenerWeb(documento.StrUrlArchivoZip.Replace(nombre_cambio, string.Format("{0}.zip", nombre_archivo)));
										}
										else
										{
											string nombre_cambio = Path.GetFileName(documento.StrUrlArchivoUbl);
											bytes_applications = Archivo.ObtenerWeb(documento.StrUrlArchivoUbl.Replace(nombre_cambio, string.Format("{0}.zip", nombre_archivo)));
										}

										
										if (bytes_applications != null)
										{
											archivo_attach = true;
											generar_att_zip = false;
										}



									}



									mensaje = mensaje.Replace("{Anexos}", "");
									mensaje = mensaje.Replace("{ObservacionAnexos}", "");
									mensaje = mensaje.Replace("{UrlAnexos}", "");
								}
								catch (Exception exception)
								{
									RegistroLog.EscribirLog(exception, MensajeCategoria.Archivos, MensajeTipo.Error, MensajeAccion.consulta, string.Format("Enviando mail del Facturador {0}, Documento {1} y Descargando Zip-attach como Blob", documento.StrEmpresaFacturador, documento.IntNumero));
								}
							}

							bool attached = false;

							//Se agrega validacion por ajustes en el namespace de los documentos electronicos
							//DateTime fecha_valid = new DateTime(2021, 5, 10, 0, 0, 0);
							//if (reenvio_documento == true && documento.DatFechaIngreso.Date <= fecha_valid.Date)
							//	archivo_attach = false;

							// ruta del zip
							string ruta_zip = string.Format(@"{0}\{1}.zip", carpeta_xml, nombre_archivo);

                            if (archivo_attach == false && documento.IntVersionDian == 2 && zip_attach_blob == false && !Archivo.ValidarExistencia(ruta_zip))
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

							if (attached == true && documento.IntVersionDian == 2)
							{

								//Proceso para agregar la respuesta de la DIAN
								try
								{

									if (zip_attach_blob == false)
									{

										if (!Archivo.ValidarExistencia(string.Format(@"{0}\{1}", carpeta_xml, Path.GetFileName(documento.StrUrlArchivoPdf))) && documento.DatFechaIngreso < fecha_cambio_ser)
										{

											string nombre_cambio = Path.GetFileName(documento.StrUrlArchivoZip);
											bytes_applications = Archivo.ObtenerWeb(documento.StrUrlArchivoZip.Replace(nombre_cambio, string.Format("{0}.zip", nombre_archivo)));

											if (bytes_applications != null)
												generar_att_zip = false;

											if (reenvio_documento == true && generar_att_zip == true)
											{
												byte[] bytes_pdf = Archivo.ObtenerWeb(documento.StrUrlArchivoPdf);
												if (bytes_pdf != null)
												{
													string ruta_pdf_copia = string.Format(@"{0}\{1}.pdf", carpeta_xml, Path.GetFileNameWithoutExtension(documento.StrUrlArchivoPdf));
													File.WriteAllBytes(ruta_pdf_copia, bytes_pdf);
												}
												else
												{
													contiene_pdf = false;
												}
											}
										}
										else
										{
											if (Archivo.ValidarExistencia(ruta_zip))
											{
												generar_att_zip = false;
											}

                                            if (reenvio_documento == true && generar_att_zip == true)
                                            {
                                                byte[] bytes_pdf = Archivo.ObtenerWeb(documento.StrUrlArchivoPdf);
                                                if (bytes_pdf != null)
                                                {
                                                    string ruta_pdf_copia = string.Format(@"{0}\{1}.pdf", carpeta_xml, Path.GetFileNameWithoutExtension(documento.StrUrlArchivoPdf));
                                                    File.WriteAllBytes(ruta_pdf_copia, bytes_pdf);
                                                }
                                                else
                                                {
                                                    contiene_pdf = false;
                                                }
                                            }

                                        }

										//if (reenvio_documento == true && Archivo.ValidarExistencia(ruta_zip))
										//{
										//	Archivo.Borrar(ruta_zip);
										//}

										if (generar_att_zip == true)
										{
											Archivo.Borrar(ruta_zip);



											// genera la compresión del archivo en zip
											using (ZipArchive archive = ZipFile.Open(ruta_zip, ZipArchiveMode.Update))
											{
												if (contiene_pdf == true)
												{
													string nombre_pdf = Path.GetFileName(documento.StrUrlArchivoPdf);
													archive.CreateEntryFromFile(string.Format(@"{0}\{1}", carpeta_xml, nombre_pdf), Path.GetFileName(nombre_pdf));
												}

												//Se adjunta al zip el attach en xml
												archive.CreateEntryFromFile(string.Format(@"{0}\{1}.xml", carpeta_xml, nombre_archivo), string.Format("{0}.xml", nombre_archivo));

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

											if (!string.IsNullOrEmpty(documento.StrUrlArchivoZip) && !documento.StrUrlArchivoUbl.Contains("hgidocs.blob") && generar_att_zip == false)
											{
												string nombre_cambio = Path.GetFileName(documento.StrUrlArchivoZip);
												bytes_applications = Archivo.ObtenerWeb(documento.StrUrlArchivoZip.Replace(nombre_cambio, string.Format("{0}.zip", nombre_archivo)));
											}
											else
											{
												bytes_applications = Archivo.ObtenerBytes(ruta_zip);
											}
										}
										else
										{
											if (bytes_applications == null && Archivo.ValidarExistencia(ruta_zip))
												bytes_applications = Archivo.ObtenerBytes(ruta_zip);

											//string nombre_cambio = Path.GetFileName(documento.StrUrlArchivoZip);
											//bytes_applications = Archivo.ObtenerWeb(documento.StrUrlArchivoZip.Replace(nombre_cambio, string.Format("{0}.zip", nombre_archivo)));

										}

										string ruta_fisica_appl = Convert.ToBase64String(bytes_applications);

										string nombre_xml_app = string.Format("{0}.zip", nombre_archivo);

										if (!string.IsNullOrEmpty(ruta_fisica_appl))
										{
											Adjunto adjunto = new Adjunto();
											adjunto.ContenidoB64 = ruta_fisica_appl;
											adjunto.Nombre = nombre_xml_app;
											archivos.Add(adjunto);

										}

										//Se quita la aliminacion del attached como 
										//if (Archivo.ValidarExistencia(string.Format(@"{0}\{1}.xml", carpeta_xml, nombre_archivo)))
										//	Archivo.Borrar(string.Format(@"{0}\{1}.xml", carpeta_xml, nombre_archivo));
									}

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

						}
						else  //Adjunto solo el PDF, en la actualidad no hay normativa que indica que enviar, se hace por servicio del Proveedor
						{

							string nombre_pdf = Path.GetFileName(documento.StrUrlArchivoPdf);

							byte[] bytes_pdf = Archivo.ObtenerWeb(documento.StrUrlArchivoPdf);
							string ruta_fisica_pdf = Convert.ToBase64String(bytes_pdf);
							//string nombre_xml = Path.GetFileName(documento.StrUrlArchivoUbl);

							if (!string.IsNullOrEmpty(ruta_fisica_pdf))
							{
								Adjunto adjunto = new Adjunto();
								adjunto.ContenidoB64 = ruta_fisica_pdf;
								adjunto.Nombre = nombre_pdf;
								archivos.Add(adjunto);
							}

							mensaje = mensaje.Replace("{Anexos}", "");
							mensaje = mensaje.Replace("{ObservacionAnexos}", "");
							mensaje = mensaje.Replace("{UrlAnexos}", "");
						}


						// envía el correo electrónico
						respuesta_email = EnviarEmail(documento.StrIdSeguridad.ToString(), false, mensaje, asunto, true, remitente, correos_destino, null, null, "", "", archivos);

						if (respuesta_email.Count > 0)
						{
							try
							{
								Ctl_ProcesosCorreos proceso_correo = new Ctl_ProcesosCorreos();

								int cont = 0;

								bool correo_enviado = false;
								bool actualizar_doc = false;

								foreach (MensajeEnvioItem item in respuesta_email.FirstOrDefault().Data)
								{
									TblProcesoCorreo correo_env = null;
									if (cont == 0 && reenvio_documento == false)
									{
										correo_env = proceso_correo.Obtener(documento.StrIdSeguridad, false);
									}

									if (cont > 0 || correo_env == null)
									{
										correo_env = proceso_correo.Crear(documento.StrIdSeguridad);
									}
									
									try
									{
										correo_env.IntEnvioMail = true;

										//Se valida si no esta bloqueado el correo
										if (item.MessageID != null && !string.IsNullOrEmpty(item.MessageID.ToString()))
										{
											correo_env.StrIdMensaje = item.MessageID.ToString();
											correo_enviado = true;
										}
										else
										{
											try
											{


												string estado_correo = Enumeracion.GetDescription(LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.MensajeEstado.Blocked);
												if (interoperabilidad == false && reenvio_documento == false)
												{
													List<MensajeEnvio> notificacion = NotificacionCorreofacturador(documento, empresa_adquiriente.StrTelefono, item.Email, estado_correo, documento.StrIdSeguridad.ToString());
												}

											}
											catch (Exception excepcion)
											{
												Ctl_Log.Guardar(excepcion, LibreriaGlobalHGInet.RegistroLog.MensajeCategoria.Servicio, LibreriaGlobalHGInet.RegistroLog.MensajeTipo.Error, LibreriaGlobalHGInet.RegistroLog.MensajeAccion.actualizacion, "Error actualizando el documento cuando el correo esta bloqueado");
											}

											correo_env.IntValidadoMail = true;
											correo_env.DatFechaValidado = Fecha.GetFecha();
											correo_env.IntEventoResp = LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.MensajeEstado.Blocked.GetHashCode();
										}
										
										correo_env.StrMailEnviado = item.Email;

										//Actualizo tabla de correos
										correo_env = proceso_correo.Actualizar(correo_env);
									}
									catch (Exception excepcion)
									{
										Ctl_Log.Guardar(excepcion, LibreriaGlobalHGInet.RegistroLog.MensajeCategoria.Servicio, LibreriaGlobalHGInet.RegistroLog.MensajeTipo.Error, LibreriaGlobalHGInet.RegistroLog.MensajeAccion.actualizacion, "Error asiganando el MessageID y Email");
										correo_env.IntEnvioMail = false;
									}

									if (reenvio_documento == true && item.MessageID != null && !string.IsNullOrEmpty(item.MessageID.ToString()))
									{
										if (documento.IntEstadoEnvio == (short)EstadoEnvio.NoEntregado.GetHashCode())
										{

											documento.IntEstadoEnvio = (short)EstadoEnvio.Enviado.GetHashCode();
											documento.IntMensajeEnvio = (short)LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.MensajeEstado.Processed.GetHashCode();
											//documento.DatFechaActualizaEstado = Fecha.GetFecha();
											actualizar_doc = true;
										}
										correo_enviado = true;

									}
									else if (reenvio_documento == true && item.MessageID == null && documento.IntEstadoEnvio != (short)EstadoEnvio.NoEntregado.GetHashCode())
									{
										correo_enviado = true;
									}

									//Se hace el registro del evento de recepcion del documento siempre y cuando el correo no presente bloqueo y el adquiriente sea cliente de HGI
									if ((plataforma.RutaPublica.Contains("habilitacion") || plataforma.RutaPublica.Contains("localhost")) && correo_env.IntEnvioMail == true && adquiriente_hgi == true && empresa_obligado.IntRadian == true && empresa_obligado.StrIdentificacion.Equals(empresa_adquiriente.StrIdentificacion) && reenvio_documento == false && cont == 0)
									{
										Ctl_EventosRadian evento = new Ctl_EventosRadian();
										Task envio_acuse = evento.ProcesoCrearAcuseRecibo(correo_env.StrIdMensaje, documento.StrIdSeguridad);
									}

									cont++;
								}

								if (reenvio_documento == false && correo_enviado == true)
								{
									documento.IntEstadoEnvio = (short)EstadoEnvio.Enviado.GetHashCode();
									documento.IntMensajeEnvio = (short)LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.MensajeEstado.Processed.GetHashCode();
									documento.IntEnvioMail = true;
									//documento.DatFechaActualizaEstado = Fecha.GetFecha();
									actualizar_doc = true;

								}
								else if (correo_enviado == false)
								{
									documento.IntEstadoEnvio = (short)EstadoEnvio.NoEntregado.GetHashCode();
									documento.IntMensajeEnvio = (short)LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.MensajeEstado.Blocked.GetHashCode();
									documento.IntEnvioMail = true;
									actualizar_doc = true;
								}

								//Actualiza tabla de documentos con el estado
								if (actualizar_doc == true && documento.DatFechaIngreso.Year == 2024)
								{
									Ctl_Documento _doc = new Ctl_Documento();
									_doc.Actualizar(documento);
								}

								/*
								TblProcesoCorreo correo_doc = proceso_correo.Obtener(documento.StrIdSeguridad);
								if (correo_doc == null)
									correo_doc = proceso_correo.Crear(documento.StrIdSeguridad);

								MensajeEnvio error_mail = respuesta_email.Where(x => x.Error != null).FirstOrDefault();

								//logica cuando el correo esta bloqueado para informar al facturador
								if (error_mail == null)
								{
									//Valida que el registro sea nuevo para que lo haga solo en el proceso principal
									if (correo_doc.IntEnvioMail == false && string.IsNullOrEmpty(correo_doc.StrIdMensaje))
									{

										correo_doc.IntEnvioMail = true;

										

									}
									else if (correo_doc.IntEnvioMail == true && string.IsNullOrEmpty(correo_doc.StrIdMensaje))
									{
										correo_doc.IntEnvioMail = false;
									}
									else if (reenvio_documento == true)
									{
										correo_doc.IntEnvioMail = true;

										if (documento.IntEstadoEnvio == (short)EstadoEnvio.NoEntregado.GetHashCode())
										{
											try
											{
												correo_doc.StrIdMensaje = respuesta_email.FirstOrDefault().Data.FirstOrDefault().MessageID.ToString();
												correo_doc.StrMailEnviado = respuesta_email.FirstOrDefault().Data.FirstOrDefault().Email;
												correo_doc.DatFecha = Fecha.GetFecha();
												correo_doc.IntValidadoMail = false;
											}
											catch (Exception excepcion)
											{
												Ctl_Log.Guardar(excepcion, LibreriaGlobalHGInet.RegistroLog.MensajeCategoria.Servicio, LibreriaGlobalHGInet.RegistroLog.MensajeTipo.Error, LibreriaGlobalHGInet.RegistroLog.MensajeAccion.actualizacion, "Error asiganando el MessageID y Email");
												correo_doc.IntEnvioMail = false;
											}

											documento.IntEstadoEnvio = (short)EstadoEnvio.Enviado.GetHashCode();
											documento.IntMensajeEnvio = (short)LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.MensajeEstado.Sent.GetHashCode();
											//documento.DatFechaActualizaEstado = Fecha.GetFecha();

											//Actualiza tabla de documentos con el estado
											//Ctl_Documento _doc = new Ctl_Documento();
											//_doc.Actualizar(documento);
										}
									}

									//Actualizo tabla de correos
									correo_doc = proceso_correo.Actualizar(correo_doc);

									
								}
								else if (reenvio_documento == false)
								{
									//Valida que el registro sea nuevo para que lo haga solo en el proceso principal
									if (correo_doc.IntEnvioMail == false)
									{
										correo_doc.IntEnvioMail = true;
									}

									bool correo_en_mailjet = false;

									foreach (MensajeEnvioItem item in respuesta_email.FirstOrDefault().Data)
									{
										//Si no trae ID es por que esta en la lista de bloqueados y no se envio a Mailjet, ya se tiene en lista de bloqueados
										//Se gestiona y se informa al Facturador
										//if (item.MessageID == 0)
										if (string.IsNullOrEmpty(item.MessageID))
										{
											

										}
										else
										{
											try
											{
												correo_doc.StrIdMensaje = item.MessageID.ToString();
												correo_en_mailjet = true;
											}
											catch (Exception excepcion)
											{
												Ctl_Log.Guardar(excepcion, LibreriaGlobalHGInet.RegistroLog.MensajeCategoria.Servicio, LibreriaGlobalHGInet.RegistroLog.MensajeTipo.Error, LibreriaGlobalHGInet.RegistroLog.MensajeAccion.actualizacion, "Error asiganando el MessageID y Email");
												correo_doc.IntEnvioMail = false;
											}
										}

										correo_doc.StrMailEnviado = item.Email;

									}

									if (correo_en_mailjet == true)
									{
										documento.IntEstadoEnvio = (short)EstadoEnvio.Enviado.GetHashCode();
										documento.IntMensajeEnvio = (short)LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.MensajeEstado.Sent.GetHashCode();
										documento.DatFechaActualizaEstado = Fecha.GetFecha();
									}
									else
									{
										documento.IntEstadoEnvio = (short)EstadoEnvio.NoEntregado.GetHashCode();
										documento.IntMensajeEnvio = (short)LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.MensajeEstado.Blocked.GetHashCode();
										string estado_correo = Enumeracion.GetDescription(LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.MensajeEstado.Blocked);
										documento.DatFechaActualizaEstado = Fecha.GetFecha();
									}

									//Actualiza tabla de documentos con el estado
									//Ctl_Documento _doc = new Ctl_Documento();
									//_doc.Actualizar(documento);
								  

								}	 */

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
				catch (Exception) { }
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

				documento.IntEstadoEnvio = (short)EstadoEnvio.NoEntregado.GetHashCode();
				documento.IntMensajeEnvio = (short)LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.MensajeEstado.Blocked.GetHashCode();

				Ctl_Documento _doc = new Ctl_Documento();
				_doc.Actualizar(documento);

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
		public List<MensajeEnvio> RespuestaAcuse(TblDocumentos documento, TblEmpresas facturador, TblEmpresas adquiriente, string ruta_archivo, string mail = "", Procedencia procedencia = Procedencia.Plataforma, string usuario = "", int evento_radian = 0, string cude_evento = "")
		{
			Ctl_DocumentosAudit clase_auditoria = new Ctl_DocumentosAudit();
			List<MensajeEnvio> respuesta_email = new List<MensajeEnvio>();

			try
			{
				string ruta_plantilla_html = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), Constantes.RutaPlantillaAcuse);

				//Asunto: Debe informarse la palabra Evento; Número del documento referenciado; NIT del generador del
				//evento; Nombre del generador del evento; Número del documento electrónico; Código del tipo de
				//documento según tabla 13.1.6; Línea de negocio(este último opcional, acuerdo comercial entre las partes)


				string asunto = string.Empty;

				CodigoResponseV2 tipo_evento = Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.CodigoResponseV2>(evento_radian);

				string cod_acuse = Enumeracion.GetAmbiente(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.CodigoResponseV2>(evento_radian));

				string numero_evento = string.Format("{0}{1}", documento.IntNumero, evento_radian);

				//Si tiene linea de negocio se agrega en el asunto
				if (!string.IsNullOrEmpty(documento.StrLineaNegocio))
				{
					asunto = string.Format("{0};{1};{2};{3};{4};{5};{6}", "evento", documento.IntNumero, adquiriente.StrIdentificacion, adquiriente.StrRazonSocial, numero_evento, cod_acuse, documento.StrLineaNegocio);
				}
				else
				{
					asunto = string.Format("{0};{1};{2};{3};{4};{5}", "evento", documento.IntNumero, adquiriente.StrIdentificacion, adquiriente.StrRazonSocial, numero_evento, cod_acuse);
				}

				//string asunto = "Respuesta Acuse de Recibo";

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
						mensaje = mensaje.Replace("{GuidDocumento}", documento.StrCufe);


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

						string estado_respuesta = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.CodigoResponseV2>(evento_radian));

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

						if (adquiriente.IntRadian == true)
						{
							//Se agrega
							bool attached = Ctl_Documento.ConvertirAttachedDoc(null, documento, documento.TblEmpresasFacturador, evento_radian, cude_evento);

							PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;

							string carpeta_xml = string.Format("{0}\\{1}\\{2}", plataforma.RutaDmsFisica, Constantes.CarpetaFacturaElectronica, documento.TblEmpresasFacturador.StrIdSeguridad.ToString());
							carpeta_xml = string.Format(@"{0}\{1}", carpeta_xml, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian);

							string nombre_archivo = Path.GetFileNameWithoutExtension(documento.StrUrlAcuseUbl);

							// ruta del zip
							string ruta_zip = string.Format(@"{0}\{1}.zip", carpeta_xml, nombre_archivo);

							// genera la compresión del archivo en zip
							using (ZipArchive archive = ZipFile.Open(ruta_zip, ZipArchiveMode.Update))
							{
								archive.CreateEntryFromFile(string.Format(@"{0}\{1}.xml", carpeta_xml, nombre_archivo), string.Format("{0}.xml", nombre_archivo));

								archive.Dispose();
							}



							if (string.IsNullOrEmpty(ruta_zip))
								throw new ApplicationException("No se encontró ruta de archivo zip");

							byte[] bytes_applications = Archivo.ObtenerBytes(ruta_zip);
							string ruta_fisica_zip = Convert.ToBase64String(bytes_applications);
							string nombre_xml = Path.GetFileName(ruta_zip);

							if (!string.IsNullOrEmpty(ruta_fisica_zip))
							{
								Adjunto adjunto = new Adjunto();
								adjunto.ContenidoB64 = ruta_fisica_zip;
								adjunto.Nombre = nombre_xml;
								archivos.Add(adjunto);
							}
						}
						else
						{
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
		public MensajeResumen ConsultarCorreo(string MessageID)
		{

			MensajeResumen datos_retorno = new MensajeResumen();

			PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

			//Obtengo a la ruta de plataforma de servicio
			string ruta_envio = string.Empty;
			ruta_envio = ObtenerUrl();
			//ruta_envio = "";
			if (string.IsNullOrEmpty(ruta_envio))
				ruta_envio = plataforma_datos.RutaHginetMail;

			MensajeResumenGlobal obj_peticion = new MensajeResumenGlobal();
			obj_peticion.identificacion = plataforma_datos.IdentificacionHGInetMail;
			obj_peticion.serial = plataforma_datos.LicenciaHGInetMail;
			obj_peticion.id_mensaje = MessageID;

			//ClienteRest<MensajeResumen> cliente = new ClienteRest<MensajeResumen>(string.Format("{0}/Api/ObtenerResumenMensaje", ruta_envio), TipoContenido.Applicationjson.GetHashCode(), "");
			//try
			//{
			//	datos_retorno = cliente.POST(obj_peticion);
			//}
			//catch (Exception excepcion)
			//{
			//	throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			//}
			//return (datos_retorno);

			return ConsultarCorreo2(string.Format("{0}/Api/ObtenerResumenMensaje", ruta_envio), obj_peticion);

		}

		public static MensajeResumen ConsultarCorreo2(string url, MensajeResumenGlobal Datos)
		{

			try
			{
				ServicePointManager.Expect100Continue = true;
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

				HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);

				request.Method = "POST";
				request.ContentType = "application/json";

				string ObjeDatos = JsonConvert.SerializeObject(Datos);

				System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
				byte[] bytes = encoding.GetBytes(ObjeDatos);

				request.ContentLength = bytes.Length;

				using (Stream requestStream = request.GetRequestStream())
				{
					requestStream.Write(bytes, 0, bytes.Length);
				}
				string resp = string.Empty;

				HttpWebResponse response = request.GetResponse() as HttpWebResponse;

				using (StreamReader reader = new StreamReader(response.GetResponseStream()))
				{
					resp = reader.ReadToEnd();
				}

				MensajeResumen Result = new MensajeResumen();

				try
				{
					Result = JsonConvert.DeserializeObject<MensajeResumen>(resp);
				}
				catch (Exception ex)
				{
					string error = ex.Message;
					throw;
				}

				return Result;
			}
			catch (Exception excepcion)
			{

				throw new ApplicationException(string.Format("Error enviando correo: {0} - Ruta servicio: {1}", excepcion.Message, url));
			}

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

						//if (string.IsNullOrEmpty(documento.StrUrlArchivoPdf))
						//	throw new ApplicationException("No se encontró ruta de archivo pdf");


						//byte[] bytes_pdf = Archivo.ObtenerWeb(documento.StrUrlArchivoPdf);
						//string ruta_fisica_pdf = Convert.ToBase64String(bytes_pdf);
						//string nombre_pdf = Path.GetFileName(documento.StrUrlArchivoPdf);


						//if (!string.IsNullOrEmpty(ruta_fisica_pdf))
						//{
						//	Adjunto adjunto = new Adjunto();
						//	adjunto.ContenidoB64 = ruta_fisica_pdf;
						//	adjunto.Nombre = nombre_pdf;
						//	archivos.Add(adjunto);
						//}


						//if (string.IsNullOrEmpty(documento.StrUrlArchivoUbl))
						//	throw new ApplicationException("No se encontró ruta de archivo xml");

						//byte[] bytes_xml = Archivo.ObtenerWeb(documento.StrUrlArchivoUbl);
						//string ruta_fisica_xml = Convert.ToBase64String(bytes_xml);
						//string nombre_xml = Path.GetFileName(documento.StrUrlArchivoUbl);

						//if (!string.IsNullOrEmpty(ruta_fisica_xml))
						//{
						//	Adjunto adjunto = new Adjunto();
						//	adjunto.ContenidoB64 = ruta_fisica_xml;
						//	adjunto.Nombre = nombre_xml;
						//	archivos.Add(adjunto);
						//}


						byte[] bytes_applications = null;

						string carpeta_xml = string.Format("{0}\\{1}\\{2}", plataforma.RutaDmsFisica, Constantes.CarpetaFacturaElectronica, empresa_obligado.StrIdSeguridad.ToString());
						carpeta_xml = string.Format(@"{0}\{1}", carpeta_xml, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian);

						string nombre_archivo = HGInetUBLv2_1.NombramientoArchivo.ObtenerXml(documento.IntNumero.ToString(), documento.StrEmpresaFacturador, TipoDocumento.Attached, documento.StrPrefijo);//nombre_xml.Replace("face", "attach");

						// ruta del zip
						string ruta_zip = string.Format(@"{0}\{1}.zip", carpeta_xml, nombre_archivo);


						if (!string.IsNullOrEmpty(documento.StrUrlArchivoZip))
						{
							string nombre_cambio = Path.GetFileName(documento.StrUrlArchivoZip);
							bytes_applications = Archivo.ObtenerWeb(documento.StrUrlArchivoZip.Replace(nombre_cambio, string.Format("{0}.zip", nombre_archivo)));
						}
						else
						{
							bytes_applications = Archivo.ObtenerBytes(ruta_zip);
						}

						string ruta_fisica_appl = Convert.ToBase64String(bytes_applications);

						string nombre_xml_app = string.Format("{0}.zip", nombre_archivo);

						if (!string.IsNullOrEmpty(ruta_fisica_appl))
						{
							Adjunto adjunto = new Adjunto();
							adjunto.ContenidoB64 = ruta_fisica_appl;
							adjunto.Nombre = nombre_xml_app;
							archivos.Add(adjunto);

						}

						mensaje = mensaje.Replace("{Anexos}", "");
						mensaje = mensaje.Replace("{ObservacionAnexos}", "");
						mensaje = mensaje.Replace("{UrlAnexos}", "");

						//Proceso para los anexos
						//if (documento.StrUrlAnexo != null)
						//{
						//	if (!string.IsNullOrEmpty(documento.StrUrlAnexo))
						//	{
						//		mensaje = mensaje.Replace("{Anexos}", "Anexos");
						//		mensaje = mensaje.Replace("{ObservacionAnexos}", documento.StrObservacionAnexo);
						//		mensaje = mensaje.Replace("{UrlAnexos}", documento.StrUrlAnexo);

						//		if (documento.IntPesoAnexo > 0)
						//		{

						//			byte[] bytes_anexo = Archivo.ObtenerWeb(documento.StrUrlAnexo);
						//			string ruta_fisica_anexo = Convert.ToBase64String(bytes_anexo);
						//			string nombre_anexo = Path.GetFileName(documento.StrUrlAnexo);

						//			if (!string.IsNullOrEmpty(ruta_fisica_anexo))
						//			{
						//				Adjunto adjunto = new Adjunto();
						//				adjunto.ContenidoB64 = ruta_fisica_anexo;
						//				adjunto.Nombre = nombre_anexo;
						//				archivos.Add(adjunto);
						//			}
						//		}

						//	}
						//	else
						//	{
						//		mensaje = mensaje.Replace("{Anexos}", "");
						//		mensaje = mensaje.Replace("{ObservacionAnexos}", "");
						//		mensaje = mensaje.Replace("{UrlAnexos}", "");
						//	}
						//}
						//else
						//{
						//	mensaje = mensaje.Replace("{Anexos}", "");
						//	mensaje = mensaje.Replace("{ObservacionAnexos}", "");
						//	mensaje = mensaje.Replace("{UrlAnexos}", "");
						//}


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
					case 4:
						asunto = "RESUMEN MENSUAL DE DOCUMENTOS ELECTRÓNICOS";
						titulo = "DOCUMENTOS ELECTRÓNICOS ENVIADOS A PLATAFORMA DE FACTURA ELECTRÓNICA ";
						break;
					default:
						asunto = "ALERTA DE INCONSISTENCIAS DE DOCUMENTO ELECTRÓNICO EN LA DIAN";
						titulo = "DOCUMENTO ELECTRÓNICO CON INCONSISTENCIAS EN LA DIAN";
						break;
				}


				// obtiene los datos del Facturador
				Ctl_Empresa empresa = new Ctl_Empresa();
				if (string.IsNullOrEmpty(Facturador))
					Facturador = Constantes.NitResolucionconPrefijo;

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
						int cont = 1;

						foreach (var item in ListaNotificacion)
						{
							//	detalle = string.Format("{0}<tr><td>{1}</td><td>{2}</td><td>{3} Documentos</td><td>{4}</td><td>{5}</td></tr>", detalle, item.identificacion, item.facturador, item.tdisponibles, Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoCompra>(item.intIdtipo)), item.DatFechaVencimiento.Value.ToString(Fecha.formato_fecha_hginet));
							if (Proceso != 4)
							{
								detalle += string.Format("<tr><td>{0}</td></tr>", item);
							}
							else
							{
								if (cont > 2)
								{
									detalle += string.Format("<tr><td>{0}</td></tr>", item);
								}
								cont += 1;


							}

						}

						mensaje = mensaje.Replace("{Titulo}", titulo);
						mensaje = mensaje.Replace("{TablaHtml}", detalle);

						if (Proceso != 4)
						{
							mensaje = mensaje.Replace("{Estado}", (Resultado) ? "Recibido" : "No Recibido");
							mensaje = mensaje.Replace("{Proceso}", (Proceso == 0) ? "Desconocido" : (Proceso == 1) ? "Envío" : (Proceso == 2) ? "Consulta" : (Proceso == 3) ? "Interoperabilidad" : "Envio Documentos");
							mensaje = mensaje.Replace("{Facturador}", facturador.StrRazonSocial);
							mensaje = mensaje.Replace("{Documento}", Documento);

						}
						else
						{
							mensaje = mensaje.Replace("A continuación se muestra la lista de inconsistencias", string.Format("Hola {0},", facturador.StrRazonSocial));
							mensaje = mensaje.Replace("del Documento: {Documento} correspondiente al Facturador: {Facturador}", string.Format("{0} {1}", ListaNotificacion[0], ListaNotificacion[1]));
							mensaje = mensaje.Replace("<!--<tr><td", "<tr><td");
							mensaje = mensaje.Replace("</tr>-->", "</tr>");
							if (tipo_asunto == 4)
							{
								mensaje = mensaje.Replace("<p style='margin: 10px 0;'><span style='font-size:12px'>Estado : <strong>{Estado}</strong>  </span></p><p style='margin: 10px 0;'>", " ");
								mensaje = mensaje.Replace("<p style='margin: 10px 0;'><span style='font-size:12px'>Proceso : <strong>{Proceso}</strong>  </span></p><p style='margin: 10px 0;'>", "");
								mensaje = mensaje.Replace("Lista de Inconsistencias", "Resumen");
							}
							else
							{
								mensaje = mensaje.Replace("{Estado}", (Resultado) ? "Recibido" : "No Recibido");
								mensaje = mensaje.Replace("{Proceso}", (Proceso == 0) ? "Desconocido" : (Proceso == 1) ? "Envío" : (Proceso == 2) ? "Consulta" : (Proceso == 3) ? "Interoperabilidad" : "Envio Documentos");
							}
						}


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
							correos_destino.Add(destinatario);
						}

						//Se agrega el correo del gerente para monitorear por 2 semanas la informacion
						if (Proceso == 4)
						{
							destinatario = new DestinatarioEmail();
							destinatario.Nombre = "ADMINISTRACIÓN";
							destinatario.Email = "jbedoya@hgi.com.co";
							correos_destino.Add(destinatario);
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

						Ctl_EnvioCorreos clase_email = new Ctl_EnvioCorreos();

						RespuestaMail = clase_email.EnviarEmail("ADMINISTRACIÓN", false, mensaje, asunto, true, remitente, correos_destino, null, null, "", "");

					}
				}

				return RespuestaMail;
			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.envio, "Error Enviando Correos de Notificacion alerta");
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
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


		public static List<MensajeEnvio> Enviar(string UrlApi, string Serial, string Identificacion, List<MensajeContenido> mensajes, string Aplicacion = "")
		{
			// valida la URL del servicio web
			//  UrlApi = string.Format("{0}{1}", Ctl_Utilidades.ValidarUrl(UrlApi), UrlWcf);

			// valida el parámetro Serial
			if (string.IsNullOrEmpty(Serial))
				throw new ApplicationException("Parámetro Serial de tipo string inválido.");

			// valida el parámetro Identificacion
			if (string.IsNullOrEmpty(Identificacion))
				throw new ApplicationException("Parámetro Identificacion de tipo string inválido.");

			List<MensajeEnvio> datos = new List<MensajeEnvio>();

			List<MensajeEnvio> enviar = null;
			MensajeContenidoGlobal MensajeContenidoGlobal = new MensajeContenidoGlobal();
			try
			{
				// configura la cadena de autenticación para la ejecución del servicio web en SHA1                
				string dataKey = Encriptar.Encriptar_SHA1(string.Format("{0}{1}", Serial, Identificacion));

				MensajeContenidoGlobal.identificacion = Identificacion;
				MensajeContenidoGlobal.serial = Serial;
				MensajeContenidoGlobal.MensajeContenido = mensajes;
				MensajeContenidoGlobal.Aplicacion = Aplicacion;

				//ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
				////System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

				//ClienteRest<List<MensajeEnvio>> cliente = new ClienteRest<List<MensajeEnvio>>(string.Format("{0}/api/Enviar", UrlApi), LibreriaGlobalHGInet.Peticiones.TipoContenido.Applicationjson.GetHashCode(), "");
				//try
				//{
				//	enviar = cliente.POST(MensajeContenidoGlobal);
				//	if (enviar != null)
				//		if (enviar[0].Data[0].MessageID != 500)
				//		{
				//			return enviar;
				//		}
				//		else
				//		{
				//			throw new Exception(enviar[0].Data[0].Email);
				//		}

				//	else
				//		throw new Exception("Error al obtener los datos con los parámetros indicados.");

				//}
				//catch (Exception ex)
				//{
				//	try
				//	{
				//		string datos_parametros = JsonConvert.SerializeObject(MensajeContenidoGlobal);

				//		RegistroLog.Guardar(Identificacion, "mails", datos_parametros, ex.Message);

				//		var cod = cliente.CodHttp;
				//		throw new Exception(string.Format(" Error: {0}", ex));
				//	}
				//	catch (Exception)
				//	{
				//		throw new Exception(ex.Message.ToString());
				//	}
				//}

				return EnviarCorreo(string.Format("{0}/api/Enviar", UrlApi), MensajeContenidoGlobal);
			}
			catch (FaultException excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion);
			}
			catch (CommunicationException excepcion)
			{
				throw new Exception(string.Format("Error de comunicación: {0}", excepcion.Message), excepcion);
			}
			catch (Exception excepcion)
			{
				throw excepcion;
			}
		}

		public static List<MensajeEnvio> EnviarCorreo(string url, MensajeContenidoGlobal Datos)
		{

			try
			{
				ServicePointManager.Expect100Continue = true;
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

				HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);

				request.Method = "POST";
				request.ContentType = "application/json";

				string ObjeDatos = JsonConvert.SerializeObject(Datos);

				System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
				byte[] bytes = encoding.GetBytes(ObjeDatos);

				request.ContentLength = bytes.Length;

				using (Stream requestStream = request.GetRequestStream())
				{
					requestStream.Write(bytes, 0, bytes.Length);
				}
				string resp = string.Empty;

				HttpWebResponse response = request.GetResponse() as HttpWebResponse;

				using (StreamReader reader = new StreamReader(response.GetResponseStream()))
				{
					resp = reader.ReadToEnd();
				}

				List<MensajeEnvio> Result = new List<MensajeEnvio>();

				try
				{

					Result = JsonConvert.DeserializeObject<List<MensajeEnvio>>(resp);

				}
				catch (Exception ex)
				{
					string error = ex.Message;
					throw;
				}

				return Result;
			}
			catch (Exception excepcion)
			{

				throw new ApplicationException(string.Format("Error enviando correo: {0} - Ruta servicio: {1}", excepcion.Message, url));
			}

		}



	}

}
