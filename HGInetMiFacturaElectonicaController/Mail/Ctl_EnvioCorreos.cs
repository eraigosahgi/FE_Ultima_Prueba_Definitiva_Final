using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetMiFacturaElectonicaData.ModeloServicio.General;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
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


		public void EnviarEmail(bool uno_a_uno, string mensaje, string asunto, bool contenido_html, DestinatarioEmail correo_respuesta, List<DestinatarioEmail> correos_destino, List<DestinatarioEmail> correos_copia = null, List<DestinatarioEmail> correos_copia_oculta = null, string ruta_plantilla_html = "", string tag_mensaje = "", IEnumerable<string> rutas_adjuntos = null)
		{
			try
			{
				// convierte las direcciones de los destinatarios
				MailAddressCollection to = ConvertirDestinatarios(correos_destino);

				MailAddressCollection cc = ConvertirDestinatarios(correos_copia);

				MailAddressCollection bcc = ConvertirDestinatarios(correos_copia_oculta);

				MailAddress from = new MailAddress(correo_respuesta.Email, correo_respuesta.Nombre);

				// construye el correo para el envío
				Mail correo = new Mail(uno_a_uno, from, to, cc, bcc);

				// configura los datos de conexión smtp
				correo.ConfigurarSmtp("in-v3.mailjet.com", 587, "ad643914de5fcf81094ab97d7c181bb2", "0c65681e968ad77a15848ac5a9d40b13", true);

				// codificación UTF8 (por defecto)
				Encoding codificacion = Encoding.UTF8;

				// envía el correo electrónico
				correo.Enviar(mensaje, asunto, contenido_html, codificacion, tag_mensaje, ruta_plantilla_html, rutas_adjuntos);
				//}

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
		public bool Bienvenida(TblEmpresas empresa, TblUsuarios datos_usuario)
		{

			try
			{

				string fileName = string.Empty;

				if (empresa == null)
					throw new ApplicationException("No se encontró información del Usuario");

				if (datos_usuario == null)
					throw new ApplicationException("No se encontró información del Usuario");

				if (empresa.IntObligado)
				{
					fileName = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), "emails\\plantilla_bienvenida_facturador.html");
				}
				else if (empresa.IntAdquiriente)
					fileName = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), "emails\\plantilla_bienvenida_adquiriente.html");

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
						mensaje = mensaje.Replace("{CodigoUsuario}", empresa.TblUsuarios.FirstOrDefault().StrUsuario);
						mensaje = mensaje.Replace("{RutaUrl}", Constantes.RutaCambioContraseña.Replace("{cod_seguridad}", datos_usuario.StrIdCambioClave.ToString()));
						mensaje = mensaje.Replace("{RutaAcceso}", Constantes.RutaAccesoPlataforma);

						string asunto = "Acceso Plataforma Facturación Electrónica";

						DestinatarioEmail remitente = new DestinatarioEmail();
						remitente.Email = Constantes.EmailRemitente;
						remitente.Nombre = Constantes.NombreRemitenteEmail;

						DestinatarioEmail destinatario = new DestinatarioEmail();
						destinatario.Email = datos_usuario.StrMail;
						destinatario.Nombre = string.Format("{0} {1}", datos_usuario.StrNombres, datos_usuario.StrApellidos);

						List<DestinatarioEmail> correos_destino = new List<DestinatarioEmail>();
						correos_destino.Add(destinatario);

						Ctl_EnvioCorreos clase_email = new Ctl_EnvioCorreos();

						clase_email.EnviarEmail(false, mensaje, asunto, true, remitente, correos_destino, null, null, "", "");
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
		public bool NotificacionDocumento(TblDocumentos documento, List<string> archivos)
		{

			try
			{
				string ruta_plantilla_html = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), Constantes.RutaPlantillaHtmlDocumentos);

				string asunto = "Factura Electrónica";

				DestinatarioEmail adquiriente = new DestinatarioEmail();
				adquiriente.Nombre = documento.TblEmpresas1.StrRazonSocial;
				adquiriente.Email = documento.TblEmpresas1.StrMail;

				DestinatarioEmail facturador = new DestinatarioEmail();
				facturador.Nombre = documento.TblEmpresas.StrRazonSocial;
				facturador.Email = documento.TblEmpresas.StrMail;

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
						mensaje = mensaje.Replace("{NombreFacturador}", documento.TblEmpresas.StrRazonSocial);
						mensaje = mensaje.Replace("{NitFacturador}", documento.TblEmpresas.StrIdentificacion);
						mensaje = mensaje.Replace("{EmailFacturador}", documento.TblEmpresas.StrMail);

						//validar como llevar el telefono
						mensaje = mensaje.Replace("{TelefonoFacturador}", documento.TblEmpresas.StrMail);

						// Datos del Tercero
						mensaje = mensaje.Replace("{NombreTercero}", documento.TblEmpresas1.StrRazonSocial);
						mensaje = mensaje.Replace("{NitTercero}", documento.TblEmpresas1.StrIdentificacion);
						mensaje = mensaje.Replace("{Digitov}", documento.TblEmpresas1.IntIdentificacionDv.ToString());
						mensaje = mensaje.Replace("{CorreoTercero}", documento.TblEmpresas1.StrMail);
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

						mensaje = mensaje.Replace("{RutaUrl}", Properties.Constantes.RutaAcuse.Replace("{cod_seguridad}", documento.StrIdSeguridad.ToString()));

						// envía el correo electrónico
						EnviarEmail(false, mensaje, asunto, true, facturador, correos_destino, correos_copia, correos_copia_oculta, "", "", archivos);

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
					throw new ApplicationException(string.Format("No se encontró información del Nit {0} ",empresa.StrIdentificacion));

				if (usuario == null)
					throw new ApplicationException(string.Format("No se encontró información del usuario {0}",usuario));

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

						Ctl_EnvioCorreos clase_email = new Ctl_EnvioCorreos();

						clase_email.EnviarEmail(false, mensaje, asunto, true, remitente, correos_destino, null, null, "", "");

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
						EnviarEmail(false, mensaje, asunto, true, facturador, correos_destino, null, null, "", "");

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
