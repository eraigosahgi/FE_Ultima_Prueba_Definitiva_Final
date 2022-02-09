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
using HGInetMiFacturaElectonicaData.ModeloServicio;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace HGInetInteroperabilidad.Procesos
{
	public class Ctl_MailRecepcion
	{


		public static void Procesar()
		{
			try
			{
				// https://webmail.mifacturaenlinea.com.co/
				//Pruebas
				// hostname: "mifacturaenlinea.com.co", username: "recepcion.dev@mifacturaenlinea.com.co", password: "gUx&819a#2ge", port: 995, isUseSsl: true
				//Produccion
				// hostname: "mifacturaenlinea.com.co", username: "recepcion@mifacturaenlinea.com.co", password: "gUx&819a#2ge", port: 995, isUseSsl: true

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
				catch (ExcepcionHgi excepcion)
				{
					string msg = string.Format("Error al obtener los correos electrónicos del servidor IMAP Servidor:{0} - Puerto:{1} - Usuario:{2}, Clave:{3} - SSL:{4} -- Exception:{5} - {6} - Det_Excp {7}", servidor, puerto, usuario, clave, habilitar_ssl, excepcion.MensajeAdicional, excepcion.MensajeResultado, excepcion.Excepcion);
					RegistroLog.EscribirLog(excepcion, LibreriaGlobalHGInet.RegistroLog.MensajeCategoria.Sonda, LibreriaGlobalHGInet.RegistroLog.MensajeTipo.Error, LibreriaGlobalHGInet.RegistroLog.MensajeAccion.lectura, msg);
					throw new ExcepcionHgi(excepcion, HGICtrlUtilidades.NotificacionCodigo.ERROR_EN_SERVIDOR, msg);
				}

				// procesa los correos electrónicos obtenidos
				foreach (UniqueId id_mensaje in ids_mensajes)
				{
					List<string> mensajes = new List<string>();
					bool correo_procesado = true;
					bool empresa_valida = false;
					bool validacion_asunto = true;

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

								// valida la cantidad de archivos adjuntos como minimo sea 1 .zip
								if (adjunto.Cantidad == 0)
								{
									mensajes.Add(string.Format("En los archivos adjuntos del correo electrónico no se encontró como minimo (1) zip."));
									correo_procesado = false;
								}

								List<string> asunto_params = asunto.Split(';').ToList();
								if (asunto_params.Count < 5)
								{
									mensajes.Add("El correo electrónico no cumple con los parámetros del asunto.");
									correo_procesado = false;
									validacion_asunto = false;
								}

								//obtener la empresa emisora o crearla
								TblEmpresas empresa = null;
								try
								{
									if (validacion_asunto == true)
									{
										Ctl_Empresa _empresa = new Ctl_Empresa();

										Match numero_idebtificacion = Regex.Match(asunto_params[0], "\\d+");
										asunto_params[0] = numero_idebtificacion.Value;

										empresa = _empresa.Obtener(asunto_params[0]);

										if (empresa == null)
										{

											Tercero empresa_emisor = new Tercero();
											empresa_emisor.Identificacion = asunto_params[0];
											empresa_emisor.RazonSocial = asunto_params[1];
											empresa_emisor.NombreComercial = asunto_params[4];
											string mail_emisor = mensaje.From.Mailboxes.FirstOrDefault().Address;
											//Se valida que si el correo emisor es de la plataforma se tome el correo del facturador si no existe
											if (mensaje.From.Mailboxes.FirstOrDefault().Address.Equals(Constantes.EmailRemitente))
												mail_emisor = mensaje.ReplyTo.Mailboxes.FirstOrDefault().Address;
											empresa_emisor.Email = mail_emisor;

											empresa = _empresa.Crear(empresa_emisor, false);
										}
										else if (empresa.IntObligado == false && correo_procesado)
										{
											empresa.IntObligado = true;
											empresa.StrSerial = Guid.NewGuid().ToString();

											if (string.IsNullOrEmpty(empresa.StrMailAdmin))
												empresa.StrMailAdmin = mensaje.From.Mailboxes.FirstOrDefault().Address;

											_empresa.Actualizar(empresa);
										}
									}

								}
								catch (Exception excepcion)
								{
									mensajes.Add("Error validando el Adquiriente electrónico.");
									correo_procesado = false;
									//throw excepcion;
								}

								/*
								try
								{   // valida las extensiones de archivos adjuntos
									List<string> extensiones = new List<string> { "zip" };
									cliente_imap.ValidarExtensionesAdjuntos(mensaje, extensiones);
								}
								catch (Exception excepcion)
								{
									mensajes.Add(excepcion.Message);
									correo_procesado = false;
									//throw excepcion;
								}*/

								// procesar archivo adjunto temporal
								PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

								// id de recepción
								string identificador_mail = Cl_Fecha.GetFecha().ToString("yyyy-MM-dd-HH-mm-ss");

								if (correo_procesado)
								{  
									string ruta_archivos = string.Format("{0}\\{1}{2}\\", plataforma_datos.RutaDmsFisica, Constantes.RutaInteroperabilidadRecepcion, empresa.StrIdentificacion);
									ruta_archivos = Directorio.CrearDirectorio(ruta_archivos);

									// almacena el correo electrónico temporalmente
									string ruta_mail = cliente_imap.Guardar(mensaje, ruta_archivos, identificador_mail);

									// almacena los adjuntos del correo electrónico temporalmente
									List<string> rutas_archivos = cliente_imap.GuardarAdjuntos(mensaje, ruta_archivos);

									// descomprime el zip adjunto
									string ruta_descomprimir = Path.Combine(Path.GetDirectoryName(ruta_mail), Path.GetFileNameWithoutExtension(ruta_mail));
									Ctl_Descomprimir.Procesar(rutas_archivos.First(x => x.Contains(".zip")), ruta_descomprimir);

									// elimina el mensaje después de procesado de la bandeja de entrada
									try
									{
										cliente_imap.Eliminar(id_mensaje);
									}
									catch (Exception ex)
									{
									   
									}

									//se notifica al correo emisor y del facturador(Adquiriente) que recibio
									try
									{
										mensajes.Add("Correo cumple con parametros establecidos por la DIAN, los archivos pasan al proceso de importar a plataforma");
										RenviarAlerta(empresa, mensajes, mensaje, asunto, cliente_imap, id_mensaje);
									}
									catch (Exception)
									{

									}
								}
								else
								{

									string ruta_archivos = string.Format("{0}\\{1}Mail\\", plataforma_datos.RutaDmsFisica, Constantes.RutaInteroperabilidadRecepcion.Replace("recepcion", "no procesados"));
									ruta_archivos = Directorio.CrearDirectorio(ruta_archivos);

									string ruta_directorio_mail = string.Format("{0}\\{1}\\", ruta_archivos, string.Format("{0} - {1}", mensaje.From.Mailboxes.FirstOrDefault().Address.Substring(0,10), identificador_mail));
									ruta_directorio_mail = Directorio.CrearDirectorio(ruta_directorio_mail);

									// almacena el correo electrónico temporalmente
									string ruta_mail = cliente_imap.Guardar(mensaje, ruta_directorio_mail, string.Format("{0} - {1}", mensaje.From.Mailboxes.FirstOrDefault().Address.Substring(0, 10), identificador_mail));

									// almacena los adjuntos del correo electrónico temporalmente
									List<string> rutas_archivos = cliente_imap.GuardarAdjuntos(mensaje, ruta_directorio_mail);

									// descomprime el zip adjunto
									string ruta_descomprimir = Path.Combine(Path.GetDirectoryName(ruta_mail), Path.GetFileNameWithoutExtension(ruta_mail));
									try
									{
										Ctl_Descomprimir.Procesar(rutas_archivos.First(x => x.Contains(".zip")), ruta_descomprimir);
									}
									catch (Exception ex)
									{}

									try
									{
										ruta_descomprimir = string.Format("{0}\\Archivo_errores.json", ruta_descomprimir);

										// almacena el objeto en archivo json
										File.WriteAllText(ruta_descomprimir, JsonConvert.SerializeObject(mensajes));
									}
									catch (Exception)
									{}

									//Se notifica al correo emisor que no se procesa el correo y la razon 
									try
									{
										RenviarAlerta(empresa, mensajes, mensaje, asunto, cliente_imap, id_mensaje);
									}
									catch (Exception ex)
									{}

									// mueve el mensaje a no procesado de la bandeja de entrada
									cliente_imap.MoverNoProcesado(id_mensaje);


									//EnviarAlerta(id_mensaje,mensaje,empresa,mensajes,cliente_imap);
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
								string msg = string.Format("Error al procesar el correo electrónico: {0} - {1} - {2} - {3}", fecha.ToString(Cl_Fecha.formato_fecha_hora_completa), remitente, asunto, excepcion.Message);
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
			catch (ExcepcionHgi excepcion)
			{
				string msg = string.Format("Error al procesar los correos electrónicos - detalle: {0} - {1}", excepcion.MensajeAdicional, excepcion.MensajeResultado);
				RegistroLog.EscribirLog(excepcion, LibreriaGlobalHGInet.RegistroLog.MensajeCategoria.Sonda, LibreriaGlobalHGInet.RegistroLog.MensajeTipo.Error, LibreriaGlobalHGInet.RegistroLog.MensajeAccion.envio, msg);
				throw new ExcepcionHgi(excepcion, HGICtrlUtilidades.NotificacionCodigo.ERROR_EN_SERVIDOR, msg);
			}
		}


		/// <summary>
		/// Proceso para enviar Alerta en caso de inconsistencia con el correo original por no cumplir una condicion del Asunto o para notificar que pasa a procesar los archivos e importar a Plataforma
		/// </summary>
		/// <param name="empresa"></param>
		/// <param name="mensajes"></param>
		/// <param name="mensaje"></param>
		/// <param name="asunto"></param>
		/// <param name="cliente_imap"></param>
		/// <param name="id_mensaje"></param>
		public static void RenviarAlerta(TblEmpresas empresa, List<string> mensajes,MimeMessage mensaje, string asunto, Cl_MailImap cliente_imap, UniqueId id_mensaje)
		{


			MailServer configuracion_server = HgiConfiguracion.GetConfiguration().MailServer;

			// notificar por correo electrónico
			// -	Notificación al Adquiriente si se incumplen las validaciones anteriores de correo electrónico; reenviando el correo electrónico recibido desde el Facturador.
			Cl_MailCliente cliente_smtp = new Cl_MailCliente()
			{
				Servidor = configuracion_server.Servidor,
				Puerto = configuracion_server.Puerto,
				Habilitar_ssl = configuracion_server.HabilitaSsl,
				TimeOut = 120000,
				Usuario = configuracion_server.Usuario,
				Clave = configuracion_server.Clave,
			};

			List<MailboxAddress> correos_destino = new List<MailboxAddress>();

			MailboxAddress remitente_re = new MailboxAddress(Constantes.NombreRemitenteEmail, Constantes.EmailRemitente);

			MailboxAddress remitente_reply = mensaje.From.OfType<MailboxAddress>().Single();

			if (remitente_reply.Address.Equals(Constantes.EmailRemitente))
				remitente_reply.Address = Constantes.EmailCopiaOculta;


			correos_destino.Add(new MailboxAddress(asunto, remitente_reply.Address));

			try
			{
				MailboxAddress reply_to = mensaje.ReplyTo.OfType<MailboxAddress>().Single();

				correos_destino.Add(new MailboxAddress(asunto, reply_to.Address));
			}
			catch (Exception ex)
			{

			}

			//Si se sabe quien es la empresa receptora(Adquiriente) se hace envio del correo original y el por que no se proceso
			if (empresa != null)
			{
				correos_destino.Add(new MailboxAddress(empresa.StrRazonSocial, empresa.StrMailAdmin));
			}
			

			try
			{
				BodyBuilder contenido = NotificacionInconsistencias(empresa, mensajes);
				cliente_imap.Reenviar(id_mensaje, mensaje, cliente_smtp, remitente_re, correos_destino, contenido, true);
			}
			catch (Exception ex)
			{ }

		}



		/// <summary>
		/// Contenido del correo electrónico para notificación de inconsistencias
		/// </summary>
		/// <param name="empresa">adquiriente</param>
		/// <param name="mensajes">mensajes de inconsistencias</param>
		/// <returns></returns>
		public static BodyBuilder NotificacionInconsistencias(TblEmpresas empresa, List<string> mensajes)
		{
			try
			{
				//if (empresa == null)
				//	throw new ApplicationException("No se encontró información de la empresa.");
					
				BodyBuilder contenido = new BodyBuilder();
				
				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;

				string fileName = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), Constantes.RutaPlantillaHtmlNotifInconsistencias);

				if (!string.IsNullOrWhiteSpace(fileName))
				{
					FileInfo file = new FileInfo(fileName);

					string mensaje = file.OpenText().ReadToEnd();

					if (file != null)
					{
						// Datos del Tercero
						if (empresa == null)
						{
							mensaje = mensaje.Replace("{TipoPersona}", "");
							mensaje = mensaje.Replace("{NombreTercero}", "");
							mensaje = mensaje.Replace("{NitTercero} -", "");
							mensaje = mensaje.Replace("{Digitov}", "");
							mensaje = mensaje.Replace("Nit.", "");
						}
						else
						{
							if (empresa.StrTipoIdentificacion.Equals("31"))
								mensaje = mensaje.Replace("{TipoPersona}", "Señores");
							else
								mensaje = mensaje.Replace("{TipoPersona}", "Señor (a)");

							mensaje = mensaje.Replace("{NombreTercero}", empresa.StrRazonSocial);
							mensaje = mensaje.Replace("{NitTercero}", empresa.StrIdentificacion);
							mensaje = mensaje.Replace("{Digitov}", empresa.IntIdentificacionDv.ToString());
						}

						string detalle = string.Empty;

						foreach (string item in mensajes)
						{	detalle += string.Format("<tr><td class='tg - yzt1'>{0}</td></tr>", item);
						}

						mensaje = mensaje.Replace("{TablaHtml}", detalle);
						
						contenido.HtmlBody = mensaje;
					}
				}
				return contenido;
			}
			catch (Exception ex)
			{	throw new ApplicationException(ex.Message);
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
						Ctl_Descomprimir.Procesar(rutas_archivos[0], ruta_descomprimir);

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

		public static void EnviarAlerta(UniqueId id_mensaje, MimeMessage mensaje, TblEmpresas empresa, List<string> mensajes)
		{
			MailServer configuracion_server = HgiConfiguracion.GetConfiguration().MailServer;

			// notificar por correo electrónico
			// -	Notificación al Adquiriente si se incumplen las validaciones anteriores de correo electrónico; reenviando el correo electrónico recibido desde el Facturador.
			Cl_MailCliente cliente_smtp = new Cl_MailCliente()
			{
				Servidor = configuracion_server.Servidor,
				Puerto = configuracion_server.Puerto,
				Habilitar_ssl = configuracion_server.HabilitaSsl,
				TimeOut = 120000,
				Usuario = configuracion_server.Usuario,
				Clave = configuracion_server.Clave,
			};

			List<MailboxAddress> correos_destino = new List<MailboxAddress>();

			MailboxAddress remitente_re = new MailboxAddress(Constantes.NombreRemitenteEmail, Constantes.EmailRemitente);

			MailboxAddress remitente_reply = mensaje.From.OfType<MailboxAddress>().Single();

			if (remitente_reply.Address.Equals(Constantes.EmailRemitente))
				remitente_reply.Address = Constantes.EmailCopiaOculta;

			correos_destino.Add(new MailboxAddress(empresa.StrRazonSocial, remitente_reply.Address));

			//correos_destino.Add(new MailboxAddress("jzea@hgi.com.co"));
			correos_destino.Add(new MailboxAddress(empresa.StrRazonSocial, empresa.StrMailAdmin));

			try
			{
				MailboxAddress reply_to = mensaje.ReplyTo.OfType<MailboxAddress>().Single();

				if (!remitente_reply.Address.Equals(reply_to.Address))
					correos_destino.Add(new MailboxAddress(empresa.StrRazonSocial, reply_to.Address));
			}
			catch (Exception)
			{
			}

			// obtener los parámetros de configuración para la lectura POP
			string servidor = Cl_InfoConfiguracionServer.ObtenerAppSettings("imap.servidor");
			int puerto = Convert.ToInt32(Cl_InfoConfiguracionServer.ObtenerAppSettings("imap.puerto"));
			string usuario = Cl_InfoConfiguracionServer.ObtenerAppSettings("imap.usuario");
			string clave = Cl_InfoConfiguracionServer.ObtenerAppSettings("imap.clave");
			bool habilitar_ssl = Convert.ToBoolean(Cl_InfoConfiguracionServer.ObtenerAppSettings("imap.ssl"));

			Cl_MailImap cliente_imap = new Cl_MailImap(servidor, puerto, usuario, clave, habilitar_ssl);

			BodyBuilder contenido = NotificacionInconsistencias(empresa, mensajes);
			cliente_imap.Reenviar(id_mensaje, mensaje, cliente_smtp, remitente_re, correos_destino, contenido, true);
		}


		/// <summary>
		/// Sonda para descargar los correos de interoperabilidad y alojarlos en una ruta para procesarlos
		/// </summary>
		/// <returns></returns>
		public async Task SondaDescargarCorreos()
		{
			try
			{
				var Tarea = TareaDescargarCorreos();
				await Task.WhenAny(Tarea);
			}
			catch (Exception excepcion)
			{
				string msg = string.Format("Error ejecutando la sonda para descargar los correos electrónicos");
				RegistroLog.EscribirLog(excepcion, LibreriaGlobalHGInet.RegistroLog.MensajeCategoria.Sonda, LibreriaGlobalHGInet.RegistroLog.MensajeTipo.Error, LibreriaGlobalHGInet.RegistroLog.MensajeAccion.lectura, msg);
			}

		}

		public async Task TareaDescargarCorreos()
		{
			await Task.Factory.StartNew(() =>
			{
				Procesar();
			});
		}


	}
}
