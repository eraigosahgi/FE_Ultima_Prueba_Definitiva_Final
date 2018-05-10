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
		private void EnviarEmail(string id_seguridad, bool uno_a_uno, string mensaje, string asunto, bool contenido_html, DestinatarioEmail correo_remitente, List<DestinatarioEmail> correos_destino, List<DestinatarioEmail> correos_copia = null, List<DestinatarioEmail> correos_copia_oculta = null, string ruta_plantilla_html = "", string tag_mensaje = "", List<Adjunto> rutas_adjuntos = null)
		{
			try
			{
                if(correo_remitente == null)
                    throw new ApplicationException("No se encontró información del Remitente");
                else if(string.IsNullOrWhiteSpace(correo_remitente.Email))
                    throw new ApplicationException("No se encontró información del Remitente");

                if(correos_destino == null && correos_copia == null && correos_copia_oculta == null)
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
				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;

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
						mensaje = mensaje.Replace("{RutaUrl}", string.Format("{0}{1}", plataforma.RutaPublica, Constantes.PaginaRestablecerClave.Replace("{id_seguridad}", usuario.StrIdCambioClave.ToString())));
						mensaje = mensaje.Replace("{RutaAcceso}", plataforma.RutaPublica);

						string asunto = "Acceso Plataforma Facturación Electrónica";

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
        /// <param name="documento"></param>
        /// <param name="telefono"></param>
        /// <param name="nuevo_email">indica el e-mail del destinatario (si es igual a null, se envía a el email de la empresa del adquiriente)</param>
        /// <returns></returns>
        public bool NotificacionDocumento(TblDocumentos documento, string telefono, string nuevo_email = "")
        {

            try
            {
                PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;

                string ruta_plantilla_html = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), Constantes.RutaPlantillaHtmlDocumentos);

                string asunto = "Factura Electrónica";

                // obtiene los datos del facturador electrónico
                Ctl_Empresa facturador_electronico = new Ctl_Empresa();
                TblEmpresas empresa_obligado = facturador_electronico.Obtener(documento.StrEmpresaFacturador);

                // envía como email de respuesta facturador electrónico
                DestinatarioEmail remitente = new DestinatarioEmail();
                remitente.Nombre = empresa_obligado.StrRazonSocial;
                remitente.Email = empresa_obligado.StrMail;


                // obtiene los datos del adquiriente
                Ctl_Empresa adquiriente = new Ctl_Empresa();
                TblEmpresas empresa_adquiriente = adquiriente.Obtener(documento.StrEmpresaAdquiriente);

                // recibe el email el adquiriente
                DestinatarioEmail destinatario = new DestinatarioEmail();
                destinatario.Nombre = empresa_adquiriente.StrRazonSocial;

                if (string.IsNullOrWhiteSpace(nuevo_email))
                    destinatario.Email = empresa_adquiriente.StrMail;
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
                        // Datos Facturador Electronico
                        mensaje = mensaje.Replace("{ImagenLogo}", "<img id='ImgLogo' src='" + "" + "' style='border: none; border-radius: 0px; display: block; outline: none; text-decoration: none; width: 100%; height: auto;' width='233' />");
                        mensaje = mensaje.Replace("{NombreFacturador}", empresa_obligado.StrRazonSocial);
                        mensaje = mensaje.Replace("{NitFacturador}", empresa_obligado.StrIdentificacion);
                        mensaje = mensaje.Replace("{EmailFacturador}", empresa_obligado.StrMail);


                        if (!string.IsNullOrWhiteSpace(telefono))
                            mensaje = mensaje.Replace("{TelefonoFacturador}", string.Format("Teléfono: {0}", telefono));
                        else
                            mensaje = mensaje.Replace("{TelefonoFacturador}", "");

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

                        string estado_factura = "Entregado DIAN";

                        mensaje = mensaje.Replace("{TipoDocumento}", titulo_factura);
                        mensaje = mensaje.Replace("{NumeroDocumento}", documento.IntNumero.ToString());
                        mensaje = mensaje.Replace("{FechaDocumento}", documento.DatFechaDocumento.ToString(Fecha.formato_fecha_hginet));
                        mensaje = mensaje.Replace("{TotalDocumento}", String.Format("{0:###,##0.}", documento.IntVlrTotal));
                        mensaje = mensaje.Replace("{EstadoDianDocumento}", estado_factura);

                        mensaje = mensaje.Replace("{RutaUrl}", string.Format("{0}{1}", plataforma.RutaPublica, Constantes.PaginaAcuseRecibo.Replace("{id_seguridad}", documento.StrIdSeguridad.ToString())));


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
                        EnviarEmail(documento.StrIdSeguridad.ToString(), false, mensaje, asunto, true, remitente, correos_destino, null, correos_copia_oculta, "", "", archivos);

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
		public bool RespuestaAcuse(TblDocumentos documento, TblEmpresas facturador, TblEmpresas adquiriente)
		{
			try
			{
				string ruta_plantilla_html = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), Constantes.RutaPlantillaAcuse);

				string asunto = "Respuesta Acuse de Recibo";

                // envía como email de respuesta el Adquiriente
                DestinatarioEmail remitente = new DestinatarioEmail();
                remitente.Nombre = adquiriente.StrRazonSocial;
                remitente.Email = adquiriente.StrMail;

                // recibe el email el Facturador Electrónico
                DestinatarioEmail destinatario = new DestinatarioEmail();
                destinatario.Nombre = facturador.StrRazonSocial;
                destinatario.Email = facturador.StrMail;

				List<DestinatarioEmail> correos_destino = new List<DestinatarioEmail>();
				correos_destino.Add(destinatario);                

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
						mensaje = mensaje.Replace("{EmailTercero}", adquiriente.StrMail);
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

                        if(documento.IntAdquirienteRecibo == 1 && !string.IsNullOrWhiteSpace(documento.StrAdquirienteMvoRechazo))
                            mensaje = mensaje.Replace("{MotivoRechazo}", "Observaciones: " + documento.StrAdquirienteMvoRechazo);
                        else if (documento.IntAdquirienteRecibo == 2)
							mensaje = mensaje.Replace("{MotivoRechazo}", "Motivo rechazo: " + documento.StrAdquirienteMvoRechazo);
						else
							mensaje = mensaje.Replace("{MotivoRechazo}", " ");

						mensaje = mensaje.Replace("{TipoDocumento}", titulo_factura);
						mensaje = mensaje.Replace("{NumeroDocumento}", documento.IntNumero.ToString());
						mensaje = mensaje.Replace("{Estadorespuesta}", estado_respuesta);


						// envía el correo electrónico
						EnviarEmail(documento.StrIdSeguridad.ToString(), false, mensaje, asunto, true, remitente, correos_destino, null, null, "", "");

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
