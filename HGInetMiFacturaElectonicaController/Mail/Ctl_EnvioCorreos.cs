using HGInetEmailServicios.ServicioEnvio;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetMiFacturaElectonicaData.ModeloServicio.General;
using HGInetUBL;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using LibreriaGlobalHGInet.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController
{
	public class Ctl_EnvioCorreos
	{


		private void EnviarEmail(string id_seguridad, bool uno_a_uno, string mensaje, string asunto, bool contenido_html, DestinatarioEmail correo_respuesta, List<DestinatarioEmail> correos_destino, List<DestinatarioEmail> correos_copia = null, List<DestinatarioEmail> correos_copia_oculta = null, string ruta_plantilla_html = "", string tag_mensaje = "", List<Adjunto> rutas_adjuntos = null)
		{
			try
			{

				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;

				if (plataforma.Mailenvio.Equals("smtp"))
				{


					MailServer configuracion_server = HgiConfiguracion.GetConfiguration().MailServer;

					// convierte las direcciones de los destinatarios
					MailAddressCollection to = ConvertirDestinatarios(correos_destino);

					MailAddressCollection cc = ConvertirDestinatarios(correos_copia);

					MailAddressCollection bcc = ConvertirDestinatarios(correos_copia_oculta);

					MailAddress from = new MailAddress(correo_respuesta.Email, correo_respuesta.Nombre);

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
						Email = correo_respuesta.Email,
						Nombre = correo_respuesta.Nombre,
						Tipo = 0
					};

					emails.Add(destino);

					// To
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

					// CC
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

					// BCC
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


					HGInetEmailServicios.Ctl_Envio.Enviar(plataforma.RutaHginetMail, plataforma.LicenciaHGInetMail, plataforma.IdentificacionHGInetMail, mensajes);

				}

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
		public bool Bienvenida(TblEmpresas empresa, TblUsuarios usuario)
		{

			try
			{

				string fileName = string.Empty;

				if (empresa == null)
					throw new ApplicationException("No se encontró información del Usuario");

				if (empresa.IntObligado)
				{
					fileName = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), Constantes.RutaPlantillaBienvenidaObligado);
				}
				else if (empresa.IntAdquiriente)
					fileName = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), Constantes.RutaPlantillaBienvenidaAdquiriente);

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
						mensaje = mensaje.Replace("{RutaUrl}", Constantes.RutaCambioContraseña.Replace("{cod_seguridad}", usuario.StrIdCambioClave.ToString()));
						mensaje = mensaje.Replace("{RutaAcceso}", Constantes.RutaAccesoPlataforma);

						string asunto = "Acceso Plataforma Facturación Electrónica";

						DestinatarioEmail remitente = new DestinatarioEmail();
						remitente.Email = Constantes.EmailRemitente;
						remitente.Nombre = Constantes.NombreRemitenteEmail;

						DestinatarioEmail destinatario = new DestinatarioEmail();
						destinatario.Email = usuario.StrMail;
						destinatario.Nombre = string.Format("{0} {1}", usuario.StrNombres, usuario.StrApellidos);

						List<DestinatarioEmail> correos_destino = new List<DestinatarioEmail>();
						correos_destino.Add(destinatario);

						DestinatarioEmail copia_oculta = new DestinatarioEmail();
						copia_oculta.Nombre = " ";
						copia_oculta.Email = " ";

						List<DestinatarioEmail> correos_copia = new List<DestinatarioEmail>();
						correos_copia.Add(copia_oculta);

						List<DestinatarioEmail> correos_copia_oculta = new List<DestinatarioEmail>();
						correos_copia_oculta.Add(copia_oculta);


						Ctl_EnvioCorreos clase_email = new Ctl_EnvioCorreos();

						clase_email.EnviarEmail(empresa.StrIdSeguridad.ToString(), false, mensaje, asunto, true, remitente, correos_destino, correos_copia, correos_copia_oculta, "", "");
					}
				}
				return true;

			}
			catch (Exception ex)
			{
				throw new ApplicationException(ex.Message);
			}


		}

		/// <summary>
		/// Envía mail al Adquiriente con los archivos generados
		/// </summary>
		/// <param name="documento">Datos del documento enviado</param>
		/// <param name="archivos">archivos de envio</param>
		/// <param name="obj_respuesta">datos del objeto de respuesta</param>
		/// <returns></returns>
		public bool NotificacionDocumento(TblDocumentos documento)
		{

			try
			{
				string ruta_plantilla_html = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), Constantes.RutaPlantillaHtmlDocumentos);

				string asunto = "Factura Electrónica";

				Ctl_Empresa adquiriente_empresa = new Ctl_Empresa();
				TblEmpresas empresa_adquiriente = adquiriente_empresa.ObtenerId(documento.IntIdEmpresaAdquiriente);

				DestinatarioEmail adquiriente = new DestinatarioEmail();
				adquiriente.Nombre = empresa_adquiriente.StrRazonSocial;
				adquiriente.Email = empresa_adquiriente.StrMail;


				Ctl_Empresa obligado_empresa = new Ctl_Empresa();
				TblEmpresas empresa_obligado = obligado_empresa.ObtenerId(documento.IntIdEmpresa);

				DestinatarioEmail facturador = new DestinatarioEmail();
				facturador.Nombre = empresa_obligado.StrRazonSocial;
				facturador.Email = empresa_obligado.StrMail;

				DestinatarioEmail copia_oculta = new DestinatarioEmail();
				copia_oculta.Nombre = "Auditoria";
				copia_oculta.Email = "ealvarez@hgi.com.co";

				List<DestinatarioEmail> correos_destino = new List<DestinatarioEmail>();
				correos_destino.Add(adquiriente);

				List<DestinatarioEmail> correos_copia = new List<DestinatarioEmail>();
				correos_copia.Add(facturador);

				List<DestinatarioEmail> correos_copia_oculta = new List<DestinatarioEmail>();
				correos_copia_oculta.Add(copia_oculta);

				// plantilla Html
				if (!string.IsNullOrWhiteSpace(ruta_plantilla_html))
				{
					FileInfo file = new FileInfo(ruta_plantilla_html);

					string mensaje = file.OpenText().ReadToEnd();


					if (file != null)
					{
						// Datos Facturador Electronico
						mensaje = mensaje.Replace("{ImagenLogo}", "<img id='ImgLogo' src='" + "" + "' style='border: none; border-radius: 0px; display: block; outline: none; text-decoration: none; width: 100%; height: auto;' width='233' />");
						mensaje = mensaje.Replace("{NombreFacturador}", empresa_obligado.StrRazonSocial);
						mensaje = mensaje.Replace("{NitFacturador}", empresa_obligado.StrIdentificacion);
						mensaje = mensaje.Replace("{EmailFacturador}", empresa_obligado.StrMail);

						//validar como llevar el telefono
						mensaje = mensaje.Replace("{TelefonoFacturador}", empresa_obligado.StrMail);

						// Datos del Tercero
						mensaje = mensaje.Replace("{NombreTercero}", empresa_adquiriente.StrRazonSocial);
						mensaje = mensaje.Replace("{NitTercero}", empresa_adquiriente.StrIdentificacion);
						mensaje = mensaje.Replace("{Digitov}", empresa_adquiriente.IntIdentificacionDv.ToString());
						mensaje = mensaje.Replace("{CorreoTercero}", empresa_adquiriente.StrMail);
						mensaje = mensaje.Replace("{GuidDocumento}", documento.StrIdSeguridad.ToString());


						//Datos del Documento
						string titulo_factura = string.Empty;

						switch (documento.IntDocTipo)
						{
							case 1:
								titulo_factura = "Factura";
								break;
							case 2:
								titulo_factura = "Nota Crédito";
								break;
							case 3:
								titulo_factura = "Nota Débito";
								break;
						}

						string estado_factura = string.Empty;

						switch (documento.IntAdquirienteRecibo)
						{
							case 5:
								estado_factura = "Entregado DIAN";
								break;
							case 6:
								estado_factura = "Rechazado DIAN";
								break;
						}

						mensaje = mensaje.Replace("{TipoDocumento}", titulo_factura);
						mensaje = mensaje.Replace("{NumeroDocumento}", documento.IntNumero.ToString());
						mensaje = mensaje.Replace("{FechaDocumento}", documento.DatFechaDocumento.ToString(Fecha.formato_fecha_hginet));
						mensaje = mensaje.Replace("{TotalDocumento}", String.Format("{0:###,##0.}", documento.IntVlrTotal));
						mensaje = mensaje.Replace("{EstadoDianDocumento}", estado_factura);

						mensaje = mensaje.Replace("{RutaUrl}", Properties.Constantes.PaginaAcuseRecibo.Replace("{cod_seguridad}", documento.StrIdSeguridad.ToString()));


						List<Adjunto> archivos = new List<Adjunto>();

						//Archivos para enviar por smtp
						/*
						if (!string.IsNullOrEmpty(documento.StrUrlArchivoPdf))
							archivos.Add(documento.StrUrlArchivoPdf);

						if (!string.IsNullOrEmpty(documento.StrUrlArchivoUbl))
							archivos.Add(documento.StrUrlArchivoUbl);

							*/

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


						// envía el correo electrónico
						EnviarEmail(documento.StrIdSeguridad.ToString(), false, mensaje, asunto, true, facturador, correos_destino, correos_copia, correos_copia_oculta, "", "", archivos);

					}
				}

				return true;
			}
			catch (Exception excepcion)
			{
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
						mensaje = mensaje.Replace("{RutaUrl}", Constantes.RutaCambioContraseña.Replace("{cod_seguridad}", usuario.StrIdCambioClave.ToString()));
						mensaje = mensaje.Replace("{RutaAcceso}", Constantes.RutaAccesoPlataforma);

						string asunto = "Restablecimiento de Contraseña";

						DestinatarioEmail remitente = new DestinatarioEmail();
						remitente.Email = Constantes.EmailRemitente;
						remitente.Nombre = Constantes.NombreRemitenteEmail;

						DestinatarioEmail destinatario = new DestinatarioEmail();
						destinatario.Email = usuario.StrMail;
						destinatario.Nombre = string.Format("{0} {1}", usuario.StrNombres, usuario.StrApellidos);

						List<DestinatarioEmail> correos_destino = new List<DestinatarioEmail>();
						correos_destino.Add(destinatario);

						DestinatarioEmail copia_oculta = new DestinatarioEmail();
						copia_oculta.Nombre = " ";
						copia_oculta.Email = " ";

						List<DestinatarioEmail> correos_copia = new List<DestinatarioEmail>();
						correos_copia.Add(copia_oculta);

						List<DestinatarioEmail> correos_copia_oculta = new List<DestinatarioEmail>();
						correos_copia_oculta.Add(copia_oculta);

						Ctl_EnvioCorreos clase_email = new Ctl_EnvioCorreos();

						clase_email.EnviarEmail(empresa.StrIdSeguridad.ToString(), false, mensaje, asunto, true, remitente, correos_destino, correos_copia, correos_copia_oculta, "", "");

					}
				}


			}
			catch (Exception ex)
			{
				throw new ApplicationException(ex.Message);
			}


		}

		/// <summary>
		/// Envía acuse de recibo al Obligado
		/// </summary>
		/// <param name="documento">Datos del documento</param>
		/// <returns></returns>
		public bool RespuestaAcuse(TblDocumentos documento)
		{
			try
			{
				string ruta_plantilla_html = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), Constantes.RutaPlantillaAcuse);

				string asunto = "Respuesta Acuse de Recibo";

				DestinatarioEmail facturador = new DestinatarioEmail();
				facturador.Nombre = documento.TblEmpresas.StrRazonSocial;
				facturador.Email = documento.TblEmpresas.StrMail;

				List<DestinatarioEmail> correos_destino = new List<DestinatarioEmail>();
				correos_destino.Add(facturador);

				DestinatarioEmail copia_oculta = new DestinatarioEmail();
				copia_oculta.Nombre = " ";
				copia_oculta.Email = " ";

				List<DestinatarioEmail> correos_copia = new List<DestinatarioEmail>();
				correos_copia.Add(copia_oculta);

				List<DestinatarioEmail> correos_copia_oculta = new List<DestinatarioEmail>();
				correos_copia_oculta.Add(copia_oculta);

				// plantilla Html
				if (!string.IsNullOrWhiteSpace(ruta_plantilla_html))
				{
					FileInfo file = new FileInfo(ruta_plantilla_html);

					string mensaje = file.OpenText().ReadToEnd();
					if (file != null)
					{
						// Datos Facturador Electronico
						mensaje = mensaje.Replace("{ImagenLogo}", "<img id='ImgLogo' src='" + "" + "' style='border: none; border-radius: 0px; display: block; outline: none; text-decoration: none; width: 100%; height: auto;' width='233' />");
						mensaje = mensaje.Replace("{NombreFacturador}", documento.TblEmpresas.StrRazonSocial);
						mensaje = mensaje.Replace("{NitFacturador}", documento.TblEmpresas.StrIdentificacion);
						mensaje = mensaje.Replace("{EmailFacturador}", documento.TblEmpresas.StrMail);

						//Validar como llevar el telefono del facturador
						mensaje = mensaje.Replace("{TelefonoFacturador}", documento.TblEmpresas.StrMail);

						// Datos del Tercero
						mensaje = mensaje.Replace("{NombreTercero}", documento.TblEmpresas1.StrRazonSocial);
						mensaje = mensaje.Replace("{NitTercero}", documento.TblEmpresas1.StrIdentificacion);
						mensaje = mensaje.Replace("{EmailTercero}", documento.TblEmpresas1.StrMail);
						mensaje = mensaje.Replace("{GuidDocumento}", documento.StrObligadoIdRegistro.ToString());


						//Datos del Documento
						string titulo_factura = string.Empty;

						switch (documento.IntDocTipo)
						{
							case 1:
								titulo_factura = "Factura";
								break;
							case 2:
								titulo_factura = "Nota Crédito";
								break;
							case 3:
								titulo_factura = "Nota Débito";
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
						}

						if (documento.IntAdquirienteRecibo == 2)
							mensaje = mensaje.Replace("{MotivoRechazo}", "Motivo rechazo: " + documento.StrAdquirienteMvoRechazo);
						else
							mensaje = mensaje.Replace("{MotivoRechazo}", " ");

						mensaje = mensaje.Replace("{TipoDocumento}", titulo_factura);
						mensaje = mensaje.Replace("{NumeroDocumento}", documento.IntNumero.ToString());
						mensaje = mensaje.Replace("{Estadorespuesta}", estado_respuesta);


						// envía el correo electrónico
						EnviarEmail(documento.StrIdSeguridad.ToString(), false, mensaje, asunto, true, facturador, correos_destino, correos_copia, correos_copia_oculta, "", "");

					}
				}

				return true;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}


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


	}
}
