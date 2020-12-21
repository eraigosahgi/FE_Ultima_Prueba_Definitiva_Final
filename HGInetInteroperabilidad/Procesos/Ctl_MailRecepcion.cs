using HGICtrlUtilidades;
using HGInetInteroperabilidad.Configuracion;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Modelo;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.RegistroLog;
using MailKit;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetInteroperabilidad.Procesos
{
	public class Ctl_MailRecepcion
	{


		public static void Procesar()
		{
			try
			{
				// https://webmail.mifacturaenlinea.com.co/
				// hostname: "mifacturaenlinea.com.co", username: "recepcion.dev@mifacturaenlinea.com.co", password: "gUx&819a#2ge", port: 995, isUseSsl: true

				// obtener los parámetros de configuración para la lectura POP
				string servidor = Cl_InfoConfiguracionServer.ObtenerAppSettings("imap.servidor");
				int puerto = Convert.ToInt32(Cl_InfoConfiguracionServer.ObtenerAppSettings("imap.puerto"));
				string usuario = Cl_InfoConfiguracionServer.ObtenerAppSettings("imap.usuario");
				string clave = Cl_InfoConfiguracionServer.ObtenerAppSettings("imap.clave");
				bool habilitar_ssl = Convert.ToBoolean(Cl_InfoConfiguracionServer.ObtenerAppSettings("imap.ssl"));

				Cl_MailImap cliente_imap = null;
				List<UniqueId> ids_mensajes = null;

				// obtener los correos electrónicos
				try
				{
					cliente_imap = new Cl_MailImap(servidor, puerto, usuario, clave, habilitar_ssl);
					ids_mensajes = cliente_imap.ObtenerIds();
				}
				catch (Exception excepcion)
				{
					string msg = string.Format("Error al obtener los correos electrónicos del servidor IMAP");
					throw new ExcepcionHgi(excepcion, HGICtrlUtilidades.NotificacionCodigo.ERROR_EN_SERVIDOR, msg);
				}

				// procesa los correos electrónicos obtenidos
				foreach (UniqueId id_mensaje in ids_mensajes)
				{
					List<string> mensajes = new List<string>();
					bool correo_procesado = true;

					try
					{
						// obtiene el mensaje por id
						MimeMessage mensaje = cliente_imap.Obtener(id_mensaje, true);

						if (mensaje != null)
						{
							string remitente = "";
							DateTime fecha = Cl_Fecha.GetFecha();
							string asunto = "";

							try
							{
								/*						 
									-	Tamaño máximo de 2MB
									-	Estructura asunto (separador por carácter punto y coma ; )
										NIT del Facturador Electrónico
										Nombre del Facturador Electrónico
										Número del Documento Electrónico
										Código del tipo de documento
										Nombre comercial del facturador
										Línea de negocio (este último opcional, acuerdo comercial entre las partes)
									-	Extensión archivo ZIP adjunto (Attached Document, PDF, ZIP anexo)

								*/

								// obtiene el asunto del correo electrónico
								asunto = mensaje.Subject;

								// información del remitente
								remitente = string.Format("{0} - {1}", mensaje.From.Mailboxes.FirstOrDefault().Name, mensaje.From.Mailboxes.FirstOrDefault().Address);

								fecha = mensaje.Date.DateTime;

								// obtiene el tamaño de los adjuntos del correo electrónico
								Cl_MailAdjuntos adjunto = cliente_imap.ObtenerPropiedadesAdjuntos(mensaje);

								// 0 Bytes
								if (adjunto.TamanoTotal == 0)
								{
									mensajes.Add("No se encontró archivo adjunto en el correo electrónico.");
									correo_procesado = false;
								}

								// 2097152 Bytes
								if (adjunto.TamanoTotal > 2097152)
								{
									mensajes.Add("Los archivos adjuntos del correo electrónico supera la capacidad máxima de 2MB.");
									correo_procesado = false;
								}

								// valida la cantidad de archivos adjuntos
								if (adjunto.Cantidad > 1)
								{
									mensajes.Add(string.Format("Los archivos ({0}) adjuntos del correo electrónico superan la cantidad permitida (1).", adjunto.Cantidad));
									correo_procesado = false;
								}

								List<string> asunto_params = asunto.Split(';').ToList();
								if (asunto_params.Count < 5)
								{
									mensajes.Add("El correo electrónico no cumple con los parámetros del asunto.");
									correo_procesado = false;
								}

								// validar y obtener la empresa
								TblEmpresas empresa = null;
								try
								{
									Ctl_Empresa _empresa = new Ctl_Empresa();
									empresa = _empresa.ValidarInteroperabilidad(asunto_params[0]);
								}
								catch (Exception excepcion)
								{
									mensajes.Add("Error validando el Adquiriente electrónico.");
									correo_procesado = false;
									throw excepcion;
								}

								try
								{   // valida las extensiones de archivos adjuntos
									List<string> extensiones = new List<string> { "zip" };
									cliente_imap.ValidarExtensionesAdjuntos(mensaje, extensiones);
								}
								catch (Exception excepcion)
								{
									mensajes.Add(excepcion.Message);
									correo_procesado = false;
									throw excepcion;
								}

								if (correo_procesado)
								{   // id de recepción
									Guid id_mail = Guid.NewGuid();

									// procesar archivo adjunto temporal
									PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;
									string ruta_archivos = string.Format("{0}\\{1}{2}\\", plataforma_datos.RutaDmsFisica, Constantes.RutaInteroperabilidadRecepcion, empresa.StrIdSeguridad);
									ruta_archivos = Directorio.CrearDirectorio(ruta_archivos);

									// almacena el correo electrónico temporalmente
									string ruta_mail = cliente_imap.Guardar(mensaje, ruta_archivos, id_mail.ToString());

									// almacena los adjuntos del correo electrónico temporalmente
									List<string> rutas_archivos = cliente_imap.GuardarAdjuntos(mensaje, ruta_archivos);

									// descomprime el zip adjunto
									string ruta_descomprimir = Path.Combine(Path.GetDirectoryName(ruta_mail), Path.GetFileNameWithoutExtension(ruta_mail));
									Ctl_Descomprimir.Procesar(empresa, rutas_archivos[0], ruta_descomprimir);

									// elimina el mensaje después de procesado de la bandeja de entrada
									cliente_imap.Eliminar(id_mensaje);
								}
								else
								{

									// notificar por correo electrónico
								}

								/*
								 -	Notificación al Adquiriente si se incumplen las validaciones anteriores de correo electrónico; reenviando el correo electrónico recibido desde el Facturador.

								-	Notificación al Facturador especificando detalladamente la inconsistencia y el evento generado (ApplicationResponse).

								* Se realizan todas las validaciones y se notifican completamente.

								-	Si el Adquiriente no cumple con las validaciones de Cliente HGI se notifica al Facturador (ApplicationResponse).

								 */
							}
							catch (Exception excepcion)
							{
								string msg = string.Format("Error al procesar el correo electrónico: {0} - {1} - {2}", fecha.ToString(Cl_Fecha.formato_fecha_hora_completa), remitente, asunto);
								RegistroLog.EscribirLog(excepcion, LibreriaGlobalHGInet.RegistroLog.MensajeCategoria.Sonda, LibreriaGlobalHGInet.RegistroLog.MensajeTipo.Error, LibreriaGlobalHGInet.RegistroLog.MensajeAccion.importar, msg);
							}
						}
					}
					catch (Exception excepcion)
					{
						string msg = string.Format("Error al obtener el correo electrónico: {0}", id_mensaje);
						RegistroLog.EscribirLog(excepcion, LibreriaGlobalHGInet.RegistroLog.MensajeCategoria.Sonda, LibreriaGlobalHGInet.RegistroLog.MensajeTipo.Error, LibreriaGlobalHGInet.RegistroLog.MensajeAccion.importar, msg);
					}
				}

				// desconectar el servidor Imap
				cliente_imap.Desconectar();
			}
			catch (Exception excepcion)
			{
				string msg = string.Format("Error al procesar los correos electrónicos");
				throw new ExcepcionHgi(excepcion, HGICtrlUtilidades.NotificacionCodigo.ERROR_EN_SERVIDOR, msg);
			}
		}


		public static void ProcesarPop3()
		{
			try
			{
				// https://webmail.mifacturaenlinea.com.co/
				// hostname: "mifacturaenlinea.com.co", username: "recepcion.dev@mifacturaenlinea.com.co", password: "gUx&819a#2ge", port: 995, isUseSsl: true

				// obtener los parámetros de configuración para la lectura POP
				string servidor = Cl_InfoConfiguracionServer.ObtenerAppSettings("pop.servidor");
				int puerto = Convert.ToInt32(Cl_InfoConfiguracionServer.ObtenerAppSettings("pop.puerto"));
				string usuario = Cl_InfoConfiguracionServer.ObtenerAppSettings("pop.usuario");
				string clave = Cl_InfoConfiguracionServer.ObtenerAppSettings("pop.clave");
				bool habilitar_ssl = Convert.ToBoolean(Cl_InfoConfiguracionServer.ObtenerAppSettings("pop.ssl"));

				Cl_MailPop cliente_pop = null;
				List<PopMessage> mensajes = null;

				// obtener los correos electrónicos
				try
				{
					cliente_pop = new Cl_MailPop(servidor, puerto, usuario, clave, habilitar_ssl);
					mensajes = cliente_pop.Obtener();
				}
				catch (Exception excepcion)
				{
					string msg = string.Format("Error al obtener los correos electrónicos del servidor POP3");
					throw new ExcepcionHgi(excepcion, HGICtrlUtilidades.NotificacionCodigo.ERROR_EN_SERVIDOR, msg);
				}

				// procesa los correos electrónicos obtenidos
				foreach (PopMessage mensaje in mensajes)
				{
					string remitente = "";
					DateTime fecha = Cl_Fecha.GetFecha();
					string asunto = "";

					try
					{
						/*						 
							-	Tamaño máximo de 2MB
							-	Estructura asunto (separador por carácter punto y coma ; )
								NIT del Facturador Electrónico
								Nombre del Facturador Electrónico
								Número del Documento Electrónico
								Código del tipo de documento
								Nombre comercial del facturador
								Línea de negocio (este último opcional, acuerdo comercial entre las partes)
							-	Extensión archivo ZIP adjunto (Attached Document, PDF, ZIP anexo)

						*/

						// obtiene el asunto del correo electrónico
						asunto = mensaje.Mensaje.Headers.Subject;

						// información del remitente
						remitente = string.Format("{0} - {1}", mensaje.Mensaje.Headers.From.DisplayName, mensaje.Mensaje.Headers.From.Address);

						fecha = mensaje.Mensaje.Headers.DateSent;

						// obtiene el tamaño de los adjuntos del correo electrónico
						long tamano = cliente_pop.ObtenerTamanoAdjuntos(mensaje);

						// 0 Bytes
						if (tamano == 0)
							throw new ApplicationException("No se encontró archivo adjunto en el correo electrónico.");

						// 2097152 Bytes
						if (tamano > 2097152)
							throw new ApplicationException("Los archivos adjuntos del correo electrónico supera la capacidad máxima de 2MB.");

						List<string> asunto_params = asunto.Split(';').ToList();

						if (asunto_params.Count < 5)
							throw new ApplicationException("El correo electrónico no cumple con los parámetros del asunto.");

						// validar y obtener la empresa
						Ctl_Empresa _empresa = new Ctl_Empresa();
						TblEmpresas empresa = _empresa.ValidarInteroperabilidad(asunto_params[0]);

						// valida las extensiones de archivos adjuntos
						List<string> extensiones = new List<string> { "zip" };
						cliente_pop.ValidarExtensionesAdjuntos(mensaje, extensiones);

						// procesar archivo adjunto temporal
						PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;
						string ruta_archivos = string.Format("{0}\\{1}{2}\\", plataforma_datos.RutaDmsFisica, Constantes.RutaInteroperabilidadRecepcion, empresa.StrIdSeguridad);

						ruta_archivos = Directorio.CrearDirectorio(ruta_archivos);

						Guid id_mail = Guid.NewGuid();

						// almacena el correo electrónico temporalmente
						string ruta_mail = cliente_pop.Guardar(mensaje, ruta_archivos, id_mail.ToString());

						// almacena los adjuntos del correo electrónico temporalmente
						List<string> rutas_archivos = cliente_pop.GuardarAdjuntos(mensaje, ruta_archivos);

						if (rutas_archivos.Count > 1)
							throw new ApplicationException("Los archivos adjuntos del correo electrónico superan la cantidad permitida.");

						// descomprime el zip adjunto
						string ruta_descomprimir = Path.Combine(Path.GetDirectoryName(ruta_mail), Path.GetFileNameWithoutExtension(ruta_mail));
						Ctl_Descomprimir.Procesar(empresa, rutas_archivos[0], ruta_descomprimir);

						// elimina el mensaje después de procesado de la bandeja de entrada
						cliente_pop.Eliminar(mensaje.Id);

					}
					catch (Exception excepcion)
					{
						string msg = string.Format("Error al procesar el correo electrónico: {0} - {1} - {2}", fecha.ToString(Cl_Fecha.formato_fecha_hora_completa), remitente, asunto);
						RegistroLog.EscribirLog(excepcion, LibreriaGlobalHGInet.RegistroLog.MensajeCategoria.Sonda, LibreriaGlobalHGInet.RegistroLog.MensajeTipo.Error, LibreriaGlobalHGInet.RegistroLog.MensajeAccion.importar, msg);
					}
				}
			}
			catch (Exception excepcion)
			{
				string msg = string.Format("Error al procesar los correos electrónicos");
				throw new ExcepcionHgi(excepcion, HGICtrlUtilidades.NotificacionCodigo.ERROR_EN_SERVIDOR, msg);
			}
		}


	}
}
