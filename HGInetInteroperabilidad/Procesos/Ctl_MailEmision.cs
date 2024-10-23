using HGICtrlUtilidades;
using HGInetInteroperabilidad.Configuracion;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.RegistroLog;
using MailKit;
using MimeKit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HGInetInteroperabilidad.Procesos
{
	public class Ctl_MailEmision
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
				string usuario = Cl_InfoConfiguracionServer.ObtenerAppSettings("imap.usuarioEm");
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

				bool ejecutar_sonda = false;
				PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

				// procesa los correos electrónicos obtenidos
				foreach (UniqueId id_mensaje in ids_mensajes)
				{
					List<string> mensajes = new List<string>();
					bool correo_procesado = true;
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

								if (asunto.Contains("Return receipt"))
								{
									// mueve el mensaje a no procesado de la bandeja de entrada
									cliente_imap.MoverNoProcesado(id_mensaje);

									throw new ApplicationException("El Asunto del correo electrónico no es relacionado a Factura Electronica");
								}

								//Se valida que el correo no sea emitido por nosotros y no se quede en un ciclo 
								if ((mensaje.TextBody != null && mensaje.TextBody.Contains(Constantes.EmailRemitente)) || mensaje.From.Mailboxes.FirstOrDefault().Address.Equals(Constantes.EmailRemitente))
								{
									// mueve el mensaje a no procesado de la bandeja de entrada
									cliente_imap.MoverNoProcesado(id_mensaje);

									throw new ApplicationException("El correo electrónico fue emitido por la plataforma");
								}


								// información del remitente
								remitente = string.Format("{0} - {1}", mensaje.From.Mailboxes.FirstOrDefault().Name, mensaje.From.Mailboxes.FirstOrDefault().Address);

								fecha = mensaje.Date.DateTime;

								// obtiene el tamaño de los adjuntos del correo electrónico
								Cl_MailAdjuntos adjunto = new Cl_MailAdjuntos();
								try
								{
									List<string> extension = new List<string>();
									extension.Add("zip");
									extension.Add("ZIP");
									extension.Add("Zip");
									adjunto = cliente_imap.ObtenerPropiedadesAdjuntos(mensaje, extension);
								}
								catch (Exception excepcion)
								{
									mensajes.Add("No se pudo obtener archivo adjunto en el correo electrónico, valide mensaje original");

									//Se notifica al correo emisor que no se procesa el correo y la razon 
									try
									{
										//RenviarAlerta(empresa, mensajes, mensaje, asunto, cliente_imap, id_mensaje);
										Ctl_MailRecepcion.EnviarAlerta(mensaje, mensajes);
									}
									catch (Exception ex)
									{ }

									// mueve el mensaje a no procesado de la bandeja de entrada
									cliente_imap.MoverNoProcesado(id_mensaje);

									RegistroLog.EscribirLog(excepcion, LibreriaGlobalHGInet.RegistroLog.MensajeCategoria.Sonda, LibreriaGlobalHGInet.RegistroLog.MensajeTipo.Error, LibreriaGlobalHGInet.RegistroLog.MensajeAccion.lectura, "No se pudo obtener archivo adjunto en el correo electrónico, valide mensaje original");
									throw new ApplicationException(excepcion.Message, excepcion.InnerException);
								}

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

								//Se valida si es un evento para que no lo procese
								if (asunto_params[0].Contains("Evento"))
								{
									// mueve el mensaje a no procesado de la bandeja de entrada
									cliente_imap.MoverNoProcesado(id_mensaje);

									throw new ApplicationException("El Asunto del correo electrónico indica que es un evento");
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
											throw new ApplicationException(string.Format("No se encontró el facturador {0}", asunto_params[0]));
										}
										else if (empresa.IntObligado == false || empresa.IntRadian == false)
										{
											throw new ApplicationException(string.Format("El facturador Electrónico {0} no esta bien configurado en la plataforma", asunto_params[0]));
										}
									}

								}
								catch (Exception excepcion)
								{
									mensajes.Add("Error validando el Facturador electrónico.");
									mensajes.Add(excepcion.Message);
									correo_procesado = false;
									//throw excepcion;
								}

								// id de recepción
								string identificador_mail = Guid.NewGuid().ToString();//Cl_Fecha.GetFecha().ToString("yyyy-MM-dd-HH-mm-ss");

								if (correo_procesado)
								{
									string ruta_archivos = string.Format("{0}\\{1}{2}\\", plataforma_datos.RutaDmsFisica, Constantes.RutaInteroperabilidadEmision, empresa.StrIdentificacion);
									ruta_archivos = Directorio.CrearDirectorio(ruta_archivos);

									// almacena el correo electrónico temporalmente
									string ruta_mail = cliente_imap.Guardar(mensaje, ruta_archivos, identificador_mail);

									// almacena los adjuntos del correo electrónico temporalmente
									List<string> rutas_archivos = cliente_imap.GuardarAdjuntos(mensaje, ruta_archivos);

									// descomprime el zip adjunto
									string ruta_descomprimir = Path.Combine(Path.GetDirectoryName(ruta_mail), Path.GetFileNameWithoutExtension(ruta_mail));
									Ctl_Descomprimir.Procesar(rutas_archivos.First(x => x.Contains(".zip") || x.Contains(".ZIP") || x.Contains(".Zip")), ruta_descomprimir);

									// elimina el mensaje después de procesado de la bandeja de entrada
									try
									{
										cliente_imap.Eliminar(id_mensaje);
									}
									catch (Exception ex)
									{

									}

									ejecutar_sonda = true;
								}
								else
								{

									//Se notifica al correo emisor que no se procesa el correo y la razon 
									try
									{
										//RenviarAlerta(empresa, mensajes, mensaje, asunto, cliente_imap, id_mensaje);
										Ctl_MailRecepcion.EnviarAlerta(mensaje, mensajes);
									}
									catch (Exception ex)
									{ }

									// mueve el mensaje a no procesado de la bandeja de entrada
									cliente_imap.MoverNoProcesado(id_mensaje);
								}

								try
								{
									TblRegistroRecepcion registro = new TblRegistroRecepcion();

									registro.StrId = Guid.Parse(identificador_mail);
									registro.DatFechaCorreo = fecha;
									registro.DatFechaRegistro = Cl_Fecha.GetFecha();
									registro.StrRemitente = remitente;
									registro.StrAsunto = asunto;
									registro.IntProceso = 1;
									if (correo_procesado == true)
									{
										registro.IntEstado = 2;
										registro.StrObservaciones = "Correo Recibido, descargado y próximo a validación para procesar";
									}
									else
									{
										registro.IntEstado = 1;
										registro.StrObservaciones = JsonConvert.SerializeObject(mensajes);
									}


									Ctl_RegistroRecepcion _ctl = new Ctl_RegistroRecepcion();
									_ctl.Crear(registro);
								}
								catch (Exception excepcion)
								{
									string msg = string.Format("Error al guardar registro del correo electrónico: {0} - {1} - {2} - {3}", fecha.ToString(Cl_Fecha.formato_fecha_hora_completa), remitente, asunto, excepcion.Message);
									RegistroLog.EscribirLog(excepcion, LibreriaGlobalHGInet.RegistroLog.MensajeCategoria.Sonda, LibreriaGlobalHGInet.RegistroLog.MensajeTipo.Error, LibreriaGlobalHGInet.RegistroLog.MensajeAccion.importar, msg);
								}
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

				//Se valida si se tiene aun archivos por procesar para hacerlo asi no tenga correos nuevos descargados.
				if (ejecutar_sonda == false)
				{
					string ruta_archivos = string.Format(@"{0}\{1}", plataforma_datos.RutaDmsFisica, Constantes.RutaInteroperabilidadRecepcion);

					string[] directorios_Obligado = Directorio.ObtenerSubdirectoriosDirectorio(ruta_archivos);

					if (directorios_Obligado != null && directorios_Obligado.Length > 0)
						ejecutar_sonda = true;

				}

				//en el momento que termine de descargar los correos ejecuta la sonda para que los procese de inmediato
				if (ejecutar_sonda == true)
				{
					try
					{
						string url_sonda = "https://consultas.hgidocs.co/Views/Pages/SondaProcesarCorreo.aspx?Emision=true";
						var request = (HttpWebRequest)WebRequest.Create(url_sonda);
						request.GetResponse();
					}
					catch (Exception excepcion)
					{
						string msg = string.Format("Error al intentar ejecutar la sonda para procesar los archivos de los correos - detalle: {0} - {1}", excepcion.Message, excepcion.InnerException.Message);
						RegistroLog.EscribirLog(excepcion, LibreriaGlobalHGInet.RegistroLog.MensajeCategoria.Sonda, LibreriaGlobalHGInet.RegistroLog.MensajeTipo.Error, LibreriaGlobalHGInet.RegistroLog.MensajeAccion.seleccion, msg);
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
	}
}
