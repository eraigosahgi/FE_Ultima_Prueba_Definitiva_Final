using HGInetDIANServicios;
using HGInetInteroperabilidad.Objetos;
using HGInetMiFacturaElectonicaController;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
//using HGInetUBL;
using HGInetUBLv2_1;
using LibreriaGlobalHGInet.Enumerables;
using LibreriaGlobalHGInet.Formato;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using LibreriaGlobalHGInet.Properties;
using LibreriaGlobalHGInet.RegistroLog;
using MailKit;
using MimeKit;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using static HGInetMiFacturaElectonicaController.Configuracion.Ctl_PlanesTransacciones;

namespace HGInetInteroperabilidad.Procesos
{
	public class Ctl_Recepcion
	{

		/// <summary>
		/// Procesa peticion de proveedor emisor
		/// </summary>
		/// <param name="datos">Objeto de la Peticion</param>
		/// <param name="ruta_ftp">Ruta publica de los documentos</param>
		/// <param name="proveedor_emisor">Identificacion Proveedor Emisor</param>
		/// <returns></returns>
		public static RegistroListaDocRespuesta Procesar(RegistroListaDoc datos, string ruta_ftp, string proveedor_emisor)
		{

			bool error_proceso = false;

			RegistroListaDocRespuesta datos_respuesta = new RegistroListaDocRespuesta();

			if (datos.documentos == null)
			{
				datos_respuesta.timeStamp = Fecha.FechaUtc(DateTime.Now);
				datos_respuesta.trackingIds = null;
				datos_respuesta.mensajeGlobal = string.Format("{2}|Documento {0} {1}", datos.nombre, Enumeracion.GetDescription(RespuestaInterOperabilidad.Zipvacio), RespuestaInterOperabilidad.Zipvacio.GetHashCode());
				//throw new ApplicationException("No se encontraron datos");
			}
			else
			{
				List<Documentos> documentos = datos.documentos;

				if (documentos == null)
					throw new ApplicationException("No se encontraron datos");

				if (string.IsNullOrEmpty(ruta_ftp))
					throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "ruta_ftp", "string"));

				List<RegistroListaDetalleDocRespuesta> respuesta = new List<RegistroListaDetalleDocRespuesta>();


				foreach (Documentos objeto in documentos)
				{

					RegistroListaDetalleDocRespuesta item_respuesta = new RegistroListaDetalleDocRespuesta();

					try
					{

						// obtiene los datos del Adquiriente enviado como facturador nuestro
						Ctl_Empresa empresa = new Ctl_Empresa();
						TblEmpresas facturador_receptor = empresa.Obtener(objeto.identificacionDestinatario);

						if (facturador_receptor == null)
						{
							item_respuesta.nombreDocumento = objeto.nombre;
							item_respuesta.codigoError = RespuestaInterOperabilidad.ClienteNoEncontrado.GetHashCode().ToString();
							item_respuesta.mensaje = string.Format("{0} en el ZIP {1}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.Enumerables.RespuestaInterOperabilidad>(Convert.ToInt16(item_respuesta.codigoError))), datos.nombre);
							error_proceso = true;
							throw new ApplicationException(string.Format("No se encontro el Facturador Receptor {0} en nuestra Plataforma ", objeto.identificacionDestinatario));
						}

						string nombre_archivo = Path.GetFileNameWithoutExtension(objeto.nombre);

						string ruta_archivo_xml = string.Format(@"{0}\{1}", ruta_ftp, objeto.nombre);

						//Se valida que el archivo xml si exista en la ruta y almacena en carpeta del Facturador emisor
						if (!Archivo.ValidarExistencia(ruta_archivo_xml))
						{
							item_respuesta.nombreDocumento = objeto.nombre;
							item_respuesta.codigoError = RespuestaInterOperabilidad.DocumentoNoEncontradoZip.GetHashCode().ToString();
							item_respuesta.mensaje = string.Format("{0} en el ZIP {1}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.Enumerables.RespuestaInterOperabilidad>(Convert.ToInt16(item_respuesta.codigoError))), datos.nombre);
							error_proceso = true;
							throw new ApplicationException(string.Format("No se encontro el archivo {0} ", ruta_archivo_xml));
						}

						// representación de datos en objeto
						var documento_obj = (dynamic)null;

						DocumentType tipo_doc = new DocumentType();

						//Convierte el UBL en objeto
						try
						{
							tipo_doc = (DocumentType)Enumeracion.ParseToEnum<DocumentType>(objeto.tipo);

							//documento_obj = ObtenerDocumento(ruta_archivo_xml, tipo_doc);

						}
						catch (Exception excepcion)
						{

							RegistroLog.EscribirLog(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion);
							item_respuesta.nombreDocumento = objeto.nombre;
							item_respuesta.codigoError = RespuestaInterOperabilidad.ErrorInternoReceptor.GetHashCode().ToString();
							item_respuesta.mensaje = string.Format("{0} en el documento {1}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.Enumerables.RespuestaInterOperabilidad>(Convert.ToInt16(item_respuesta.codigoError))), objeto.nombre);
							error_proceso = true;
							throw new ApplicationException(string.Format("Error al convertir xml a objeto el archivo {0} Detalle: {1}", documento_obj.Documento, excepcion.Message));

						}

						//Valida que el nit de la peticion sea el mismo que el nit del adquiriente en el ubl
						if (documento_obj.DatosAdquiriente.Identificacion != objeto.identificacionDestinatario)
						{
							item_respuesta.nombreDocumento = objeto.nombre;
							item_respuesta.codigoError = RespuestaInterOperabilidad.ClienteNoEncontrado.GetHashCode().ToString();
							item_respuesta.mensaje = string.Format("{0} en el ZIP {1}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.Enumerables.RespuestaInterOperabilidad>(Convert.ToInt16(item_respuesta.codigoError))), datos.nombre);
							error_proceso = true;
							throw new ApplicationException(string.Format("El adquiriente {0} del archivo {1} no coincide con el destinatario {2} de la peticion ", documento_obj.DatosAdquiriente.Identificacion, objeto.nombre, objeto.identificacionDestinatario));
						}

						Ctl_Documento num_doc = new Ctl_Documento();

						TblDocumentos documento_bd = new TblDocumentos();

						if (tipo_doc == DocumentType.AcuseDeRecibo)
						{

							item_respuesta = ProcesarAcuse(documento_obj, ruta_archivo_xml, nombre_archivo, facturador_receptor);
							item_respuesta.nombreDocumento = objeto.nombre;

							if ((item_respuesta.codigoError.Equals(RespuestaInterOperabilidad.ErrorInternoReceptor.GetHashCode().ToString())) || ((item_respuesta.codigoError.Equals(RespuestaInterOperabilidad.ProcesamientoParcial.GetHashCode().ToString()))))
							{
								error_proceso = true;
								throw new ApplicationException(string.Format("Error al procesar el documento {0}", objeto.nombre));
							}

						}
						else
						{
							//Obtiene de la BD el documento enviado
							try
							{
								documento_bd = num_doc.Obtener(documento_obj.DatosObligado.Identificacion, documento_obj.Documento, documento_obj.Prefijo);
							}
							catch (Exception excepcion)
							{

								RegistroLog.EscribirLog(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion);
								item_respuesta.nombreDocumento = objeto.nombre;
								item_respuesta.codigoError = RespuestaInterOperabilidad.ErrorInternoReceptor.GetHashCode().ToString();
								item_respuesta.mensaje = string.Format("{0} en el documento {1}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.Enumerables.RespuestaInterOperabilidad>(Convert.ToInt16(item_respuesta.codigoError))), objeto.nombre);
								error_proceso = true;
								throw new ApplicationException(string.Format("Error al obtener el documento {0} Detalle: {1}", documento_obj.Documento, excepcion.Message));

							}

							//valida si el Documento ya existe en Base de Datos
							if (documento_bd != null)
							{
								item_respuesta.nombreDocumento = objeto.nombre;
								item_respuesta.codigoError = RespuestaInterOperabilidad.ProcesamientoParcial.GetHashCode().ToString();
								item_respuesta.mensaje = string.Format("{0} El documento {1} se encuentra registrado con el Tacking ID: {2}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<RespuestaInterOperabilidad>(Convert.ToInt16(item_respuesta.codigoError))), datos.nombre, documento_bd.StrIdInteroperabilidad);
								item_respuesta.uuid = documento_bd.StrIdInteroperabilidad.ToString();
								error_proceso = true;
								throw new ApplicationException(string.Format("El documento {0} con prefijo {1} ya xiste para el Facturador Electrónico {2}", documento_obj.Documento, documento_obj.Prefijo, documento_obj.DatosObligado.Identificacion));
							}

							//Creacion Facturador Emisor del documento 
							TblEmpresas facturador_emisor = new TblEmpresas();

							try
							{
								facturador_emisor = CrearFacturadorEmisor(documento_obj, tipo_doc.GetHashCode());


								AlmacenarArchivo(ruta_archivo_xml, string.Format("{0}.xml", nombre_archivo), facturador_emisor);
							}
							catch (Exception excepcion)
							{

								RegistroLog.EscribirLog(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion);
								item_respuesta.nombreDocumento = objeto.nombre;
								item_respuesta.codigoError = RespuestaInterOperabilidad.ErrorInternoReceptor.GetHashCode().ToString();
								item_respuesta.mensaje = string.Format("{0} en el documento {1}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.Enumerables.RespuestaInterOperabilidad>(Convert.ToInt16(item_respuesta.codigoError))), objeto.nombre);
								error_proceso = true;
								throw new ApplicationException(string.Format("Error al almacenar el archivo {0} Detalle: {1}", documento_obj.Documento, excepcion.Message));

							}

							string url_ppal_pdf = null;

							//se valida que exista el archivo pdf y almacena en carpeta del Facturador emisor
							if (objeto.representacionGraficas)
							{
								string ruta_archivo_pdf = string.Format(@"{0}\{1}.pdf", ruta_ftp, nombre_archivo);

								string ruta_url_pdf = string.Format(@"{0}\{1}.pdf.url", ruta_ftp, nombre_archivo);

								if (!Archivo.ValidarExistencia(ruta_url_pdf))
								{
									if (!Archivo.ValidarExistencia(ruta_archivo_pdf))
									{
										item_respuesta.nombreDocumento = string.Format("{0}.pdf", objeto.nombre);
										item_respuesta.codigoError = RespuestaInterOperabilidad.DocumentoNoEncontrado.GetHashCode().ToString();
										item_respuesta.mensaje = string.Format("{0} en el ZIP {1}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.Enumerables.RespuestaInterOperabilidad>(Convert.ToInt16(item_respuesta.codigoError))), datos.nombre);
										error_proceso = true;
										throw new ApplicationException(string.Format("No se encontro el archivo {0}", ruta_archivo_pdf));
									}

									try
									{
										AlmacenarArchivo(ruta_archivo_pdf, string.Format("{0}.pdf", nombre_archivo), facturador_emisor);
									}
									catch (Exception excepcion)
									{

										RegistroLog.EscribirLog(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion);
										item_respuesta.nombreDocumento = objeto.nombre;
										item_respuesta.codigoError = RespuestaInterOperabilidad.ErrorInternoReceptor.GetHashCode().ToString();
										item_respuesta.mensaje = string.Format("{0} en el documento {1}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.Enumerables.RespuestaInterOperabilidad>(Convert.ToInt16(item_respuesta.codigoError))), objeto.representacionGraficas);
										error_proceso = true;
										throw new ApplicationException(string.Format("Error al almacenar el archivo {0} Detalle: {1}", documento_obj.Documento, excepcion.Message));

									}
								}
								else
								{
									//Lee el archivo enviado
									StreamReader ruta_publica_pdf = new StreamReader(ruta_url_pdf);

									url_ppal_pdf = ruta_publica_pdf.ReadLine();

									ruta_publica_pdf.Close();

								}
							}
							//se valida que exista adjuntos como zip y almacena en carpeta del Facturador emisor
							if (objeto.adjuntos)
							{
								string ruta_archivo_zip = string.Format(@"{0}\{1}.zip", ruta_ftp, nombre_archivo);

								if (!Archivo.ValidarExistencia(ruta_archivo_zip))
								{
									item_respuesta.nombreDocumento = string.Format("{0}.zip", objeto.nombre);
									item_respuesta.codigoError = RespuestaInterOperabilidad.DocumentoNoEncontrado.GetHashCode().ToString();
									item_respuesta.mensaje = string.Format("{0} en el ZIP {1}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.Enumerables.RespuestaInterOperabilidad>(Convert.ToInt16(item_respuesta.codigoError))), datos.nombre);
									error_proceso = true;
									throw new ApplicationException(string.Format("No se encontro el archivo {0}", ruta_archivo_zip));
								}

								FileInfo adjunto = new FileInfo(ruta_archivo_zip);

								//Valida que el archivo no supere el peso de 2MB
								if (adjunto.Length < Convert.ToInt32(Constantes.TamanoAnexo))
								{
									try
									{
										AlmacenarArchivo(ruta_archivo_zip, string.Format("{0}.zip", nombre_archivo), facturador_emisor, false, true);
									}
									catch (Exception excepcion)
									{

										RegistroLog.EscribirLog(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion);
										item_respuesta.nombreDocumento = objeto.nombre;
										item_respuesta.codigoError = RespuestaInterOperabilidad.ErrorInternoReceptor.GetHashCode().ToString();
										item_respuesta.mensaje = string.Format("{0} en el documento {1}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.Enumerables.RespuestaInterOperabilidad>(Convert.ToInt16(item_respuesta.codigoError))), objeto.adjuntos);
										error_proceso = true;
										throw new ApplicationException(string.Format("Error al almacenar el archivo {0} Detalle: {1}", documento_obj.Documento, excepcion.Message));

									}
								}
							}

							//Convierto el Objeto a Tbl
							try
							{
								//documento_bd = Convertir(documento_obj, tipo_doc, facturador_emisor, nombre_archivo, proveedor_emisor, url_ppal_pdf);
							}
							catch (Exception excepcion)
							{

								RegistroLog.EscribirLog(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion);
								item_respuesta.nombreDocumento = objeto.nombre;
								item_respuesta.codigoError = RespuestaInterOperabilidad.ErrorInternoReceptor.GetHashCode().ToString();
								item_respuesta.mensaje = string.Format("{0} en el documento {1}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.Enumerables.RespuestaInterOperabilidad>(Convert.ToInt16(item_respuesta.codigoError))), objeto.nombre);
								error_proceso = true;
								throw new ApplicationException(string.Format("Error al convertir objeto {0} Detalle: {1}", documento_obj.Documento, excepcion.Message));

							}

							//Guardo el documento en BD
							Ctl_Documento documento_tmp = new Ctl_Documento();

							try
							{
								documento_bd = documento_tmp.Crear(documento_bd);
								item_respuesta.uuid = documento_bd.StrIdInteroperabilidad.ToString();
							}
							catch (Exception excepcion)
							{

								RegistroLog.EscribirLog(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion);
								item_respuesta.nombreDocumento = objeto.nombre;
								item_respuesta.codigoError = RespuestaInterOperabilidad.ErrorInternoReceptor.GetHashCode().ToString();
								item_respuesta.mensaje = string.Format("{0} en el ZIP {1}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.Enumerables.RespuestaInterOperabilidad>(Convert.ToInt16(item_respuesta.codigoError))), datos.nombre);
								error_proceso = true;
								throw new ApplicationException(string.Format("Error al guardar el documento {0} Detalle: {1}", objeto.nombre, excepcion.Message));

							}

							try
							{   // envía el correo del documento al Adquiriente(Facturador Receptor)
								Ctl_EnvioCorreos email = new Ctl_EnvioCorreos();
								email.NotificacionDocumento(documento_bd, documento_obj.DatosObligado.Telefono, documento_obj.DatosAdquiriente.Email);
								//documento_bd.IntIdEstado = Convert.ToInt16(ProcesoEstado.Finalizacion.GetHashCode());
							}
							catch (Exception excepcion)
							{
								RegistroLog.EscribirLog(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion);
							}
						}

						item_respuesta.nombreDocumento = objeto.nombre;
						item_respuesta.codigoError = RespuestaInterOperabilidad.PendienteProcesamiento.GetHashCode().ToString();
						item_respuesta.mensaje = string.Format("{0} en el ZIP {1}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.Enumerables.RespuestaInterOperabilidad>(Convert.ToInt16(item_respuesta.codigoError))), datos.nombre);
						error_proceso = false;


					}
					catch (Exception ex)
					{
						RegistroLog.EscribirLog(ex, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion);

					}

					respuesta.Add(item_respuesta);

				}
				datos_respuesta.timeStamp = Fecha.FechaUtc(DateTime.Now);
				datos_respuesta.trackingIds = respuesta;
				if (!error_proceso)
					datos_respuesta.mensajeGlobal = string.Format("{2}|Documento {0} {1}", datos.nombre, Enumeracion.GetDescription(RespuestaInterOperabilidad.ZipRadicado), RespuestaInterOperabilidad.ZipRadicado.GetHashCode());
				else
					datos_respuesta.mensajeGlobal = string.Format("{2}|Documento {0} {1}", datos.nombre, Enumeracion.GetDescription(RespuestaInterOperabilidad.ProcesamientoParcial), RespuestaInterOperabilidad.ProcesamientoParcial.GetHashCode());
			}

			//Aqui elimino el archivo que se descomprimio
			try
			{
				Archivo.Borrar(ruta_ftp);
			}
			catch (Exception excepcion)
			{
				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion);

			}

			return datos_respuesta;
		}


		/// <summary>
		/// Procesa el Acuse
		/// </summary>
		/// <param name="documento_obj">Objeto Acuse</param>
		/// <param name="ruta_archivo_xml">Ruta donde esta almacenado el archivo</param>
		/// <param name="nombre_archivo">Nombre del archivo del Acuse</param>
		/// <param name="facturador_receptor">Datos del Facturador Receptor</param>
		/// <returns></returns>
		public static RegistroListaDetalleDocRespuesta ProcesarAcuse(Acuse documento_obj, string ruta_archivo_xml, string nombre_archivo, TblEmpresas facturador_receptor)
		{

			RegistroListaDetalleDocRespuesta item_respuesta = new RegistroListaDetalleDocRespuesta();

			try
			{
				Ctl_Documento num_doc = new Ctl_Documento();

				TblDocumentos documento_bd = new TblDocumentos();


				//valida si el Documento ya existe en Base de Datos para actualizar la informacion
				try
				{
					documento_bd = num_doc.ObtenerPorIdInteroperabilidad(Guid.Parse(documento_obj.IdSeguridad));
				}
				catch (Exception excepcion)
				{

					RegistroLog.EscribirLog(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion);
					item_respuesta.codigoError = RespuestaInterOperabilidad.ErrorInternoReceptor.GetHashCode().ToString();
					item_respuesta.mensaje = string.Format("{0} en el documento {1}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.Enumerables.RespuestaInterOperabilidad>(Convert.ToInt16(item_respuesta.codigoError))), documento_obj.Documento);
					throw new ApplicationException(string.Format("Error al obtener el documento {0} Detalle: {1}", documento_obj.Documento, excepcion.Message));


				}

				if (documento_bd == null)
				{
					item_respuesta.codigoError = RespuestaInterOperabilidad.ProcesamientoParcial.GetHashCode().ToString();
					item_respuesta.mensaje = string.Format("{0} El documento {1} no se encuentra registrado en nuestra plataforma", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<RespuestaInterOperabilidad>(Convert.ToInt16(item_respuesta.codigoError))), documento_obj.Documento);
					item_respuesta.uuid = documento_obj.IdSeguridad;
					throw new ApplicationException(string.Format("El documento {0} no existe en la platforma a nombre del Facturador Electrónico {1} con IdSeguridad {2}", documento_obj.Documento, documento_obj.DatosObligado.Identificacion, documento_obj.IdSeguridad));
				}
				else
				{
					//Proceso para Almacenar el acuse
					try
					{
						AlmacenarArchivo(ruta_archivo_xml, string.Format("{0}.xml", nombre_archivo), facturador_receptor, true);
					}
					catch (Exception excepcion)
					{

						RegistroLog.EscribirLog(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.actualizacion);
						item_respuesta.codigoError = RespuestaInterOperabilidad.ErrorInternoReceptor.GetHashCode().ToString();
						item_respuesta.mensaje = string.Format("{0} en el documento {1}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.Enumerables.RespuestaInterOperabilidad>(Convert.ToInt16(item_respuesta.codigoError))), documento_obj.Documento);
						throw new ApplicationException(string.Format("Error al almacenar el archivo {0} Detalle: {1}", documento_obj.Documento, excepcion.Message));
					}

					PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

					// url pública
					string url_ppal = string.Format(@"{0}/{1}/{2}/", plataforma_datos.RutaDmsPublica, Constantes.CarpetaFacturaElectronica, facturador_receptor.StrIdSeguridad);

					//string url_email = string.Format(@"{0}/{1}/{2}/", plataforma_datos.RutaDmsPublica, "dms", facturador_receptor.StrIdSeguridad);

					// url pública del xml
					string UrlAcuseUbl = string.Format(@"{0}{1}/{2}.xml", url_ppal, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaXmlAcuse, nombre_archivo);

					try
					{
						ResponseCode cod_resp = Enumeracion.GetValueFromDescription<ResponseCode>(documento_obj.CodigoRespuesta);

						//Actualizacion de Campos de Acuse
						documento_bd.IntAdquirienteRecibo = (short)cod_resp.GetHashCode();
						documento_bd.DatAdquirienteFechaRecibo = documento_obj.Fecha;
						documento_bd.StrAdquirienteMvoRechazo = documento_obj.MvoRespuesta;
						documento_bd.StrUrlAcuseUbl = UrlAcuseUbl;
						documento_bd.IntIdEstado = Convert.ToInt16(ProcesoEstado.RecepcionAcuse.GetHashCode());
						documento_bd.DatFechaActualizaEstado = Fecha.GetFecha();

						try
						{   // envía el correo del acuse de recibo al facturador electrónico
							Ctl_EnvioCorreos email = new Ctl_EnvioCorreos();
							Ctl_Empresa empresa = new Ctl_Empresa();
							TblEmpresas adquiriente = empresa.Obtener(documento_obj.DatosAdquiriente.Identificacion);
							email.RespuestaAcuse(documento_bd, facturador_receptor, adquiriente, UrlAcuseUbl);
							documento_bd.IntIdEstado = Convert.ToInt16(ProcesoEstado.Finalizacion.GetHashCode());
						}
						catch (Exception) { }

						documento_bd = num_doc.Actualizar(documento_bd);
						item_respuesta.codigoError = RespuestaInterOperabilidad.PendienteProcesamiento.GetHashCode().ToString();
						item_respuesta.mensaje = string.Format("{0} en el documento {1}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.Enumerables.RespuestaInterOperabilidad>(Convert.ToInt16(item_respuesta.codigoError))), documento_obj.Documento);
						item_respuesta.uuid = documento_bd.StrIdInteroperabilidad.ToString();
					}
					catch (Exception excepcion)
					{

						RegistroLog.EscribirLog(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.actualizacion);
						item_respuesta.codigoError = RespuestaInterOperabilidad.ErrorInternoReceptor.GetHashCode().ToString();
						item_respuesta.mensaje = string.Format("{0} en el documento {1}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.Enumerables.RespuestaInterOperabilidad>(Convert.ToInt16(item_respuesta.codigoError))), documento_obj.Documento);
						throw new ApplicationException(string.Format("Error al actualizar el documento {0} Detalle: {1}", documento_obj.Documento, excepcion.Message));
					}

				}
			}
			catch (Exception excepcion)
			{
				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.actualizacion);
			}

			return item_respuesta;

		}


		/// <summary>
		/// Genera documento a partir del Ubl
		/// </summary>
		/// <param name="objeto_cadena"></param>
		/// <param name="tipo_documento"></param>
		/// <returns></returns>
		public static dynamic ObtenerDocumento(string objeto_cadena, TipoDocumento tipo_documento)
		{

			try
			{
				// representación de datos en objeto
				var documento_obj = (dynamic)null;

				//FileStream xml_reader = new FileStream(objeto_cadena, FileMode.Open);

				// convierte el contenido de texto a xml
				XmlReader xml_reader = XmlReader.Create(new StringReader(objeto_cadena.Trim()));

				XmlSerializer serializacion = null;

				if (tipo_documento == TipoDocumento.Factura)
				{
					serializacion = new XmlSerializer(typeof(InvoiceType));

					InvoiceType conversion = (InvoiceType)serializacion.Deserialize(xml_reader);

					documento_obj = FacturaXMLv2_1.Convertir(conversion, null);

				}
				else if (tipo_documento == TipoDocumento.NotaCredito)
				{
					serializacion = new XmlSerializer(typeof(CreditNoteType));

					CreditNoteType conversion = (CreditNoteType)serializacion.Deserialize(xml_reader);

					documento_obj = NotaCreditoXMLv2_1.Convertir(conversion, null);
				}
				else if (tipo_documento == TipoDocumento.NotaDebito)
				{
					serializacion = new XmlSerializer(typeof(DebitNoteType));

					DebitNoteType conversion = (DebitNoteType)serializacion.Deserialize(xml_reader);

					documento_obj = NotaDebitoXMLv2_1.Convertir(conversion, null);
				}
				else if (tipo_documento == TipoDocumento.AcuseRecibo)
				{
					serializacion = new XmlSerializer(typeof(ApplicationResponseType));

					ApplicationResponseType conversion = (ApplicationResponseType)serializacion.Deserialize(xml_reader);

					documento_obj = AcuseReciboXMLv2_1.Convertir(conversion).FirstOrDefault();
				}

				// cerrar la lectura del archivo xml
				xml_reader.Close();

				return documento_obj;
			}
			catch (Exception ex)
			{
				RegistroLog.EscribirLog(ex, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.consulta);
				throw new ApplicationException(ex.Message, ex.InnerException);
			}


		}

		/// <summary>
		/// Creacion del Facturador Emisor si no existe con las respectiva Resolucion
		/// </summary>
		/// <param name="emisor"></param>
		/// <returns>Un objeto BD de Empresa</returns>
		public static TblEmpresas CrearFacturadorEmisor(dynamic documento_obj, int tipo_documento, byte ambiente = 0)
		{


			try
			{
				TblEmpresas facturador_emisor = new TblEmpresas();

				Ctl_Empresa empresa = new Ctl_Empresa();

				try
				{
					facturador_emisor = empresa.Obtener(documento_obj.DatosObligado.Identificacion);
				}
				catch (Exception ex)
				{
					RegistroLog.EscribirLog(ex, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion);
				}

				bool actualizar_emisor = false;

				if (facturador_emisor == null)
				{
					empresa = new Ctl_Empresa();

					if (string.IsNullOrWhiteSpace(documento_obj.DatosObligado.Email))
						documento_obj.DatosObligado.Email = string.Empty;

					facturador_emisor = empresa.Crear(documento_obj.DatosObligado, false);
				}
				
				if (string.IsNullOrEmpty(facturador_emisor.StrSerial))
				{
					facturador_emisor.StrSerial = Guid.NewGuid().ToString();
					if (string.IsNullOrEmpty(facturador_emisor.StrMailEnvio) && !string.IsNullOrEmpty(facturador_emisor.StrMailAdmin))
						facturador_emisor.StrMailEnvio = facturador_emisor.StrMailAdmin;

					actualizar_emisor = true;
				}

				if ((!string.IsNullOrWhiteSpace(documento_obj.DatosObligado.Email)) && !facturador_emisor.StrMailAdmin.Equals(documento_obj.DatosObligado.Email))
				{
					facturador_emisor.StrMailAdmin = documento_obj.DatosObligado.Email;
					facturador_emisor.StrMailEnvio = facturador_emisor.StrMailAdmin;
					facturador_emisor.StrMailAcuse = facturador_emisor.StrMailAdmin;
					facturador_emisor.StrMailRecepcion = facturador_emisor.StrMailAdmin;
					actualizar_emisor = true;
				}

				if (actualizar_emisor == true)
				{
					//Se coloca el mismo ambiente en el que este el facturador receptor(0 y 1 Habilitacion, 99 - Produccion)
					facturador_emisor.IntHabilitacion = ambiente;
					empresa.Actualizar(facturador_emisor);
				}


				/*
				PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;
				if (!plataforma_datos.RutaPublica.Contains("habilitacion") && facturador_emisor.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
				{
					facturador_emisor.IntHabilitacion = (byte)Habilitacion.Produccion.GetHashCode();
				}*/

				//Se crea Resolucion
				TblEmpresasResoluciones resolucion = new TblEmpresasResoluciones();

				Ctl_EmpresaResolucion ctl_resolucion = new Ctl_EmpresaResolucion();

				try
				{

					string num_resolucion = string.Empty;
					if (tipo_documento != TipoDocumento.Factura.GetHashCode())
					{
						if (documento_obj.NumeroResolucion == null)
							num_resolucion = "*";
					}
					else
					{
						num_resolucion = documento_obj.NumeroResolucion;
					}
					resolucion = ctl_resolucion.Obtener(documento_obj.DatosObligado.Identificacion, num_resolucion, documento_obj.Prefijo);
				}
				catch (Exception ex)
				{
					//LogExcepcion.Guardar(new Exception(string.Format("Error al Obtener Resolución {0}", ex), ex));
					RegistroLog.EscribirLog(ex, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.consulta);
				}

				if (resolucion == null)
				{

					try
					{
						resolucion = Ctl_EmpresaResolucion.Convertir(documento_obj.DatosObligado.Identificacion, documento_obj.Prefijo, tipo_documento, facturador_emisor.IntVersionDian, documento_obj.NumeroResolucion);
					}
					catch (Exception ex)
					{
						//LogExcepcion.Guardar(new Exception(string.Format("Error al guardar Resolución {0}", ex), ex));
						RegistroLog.EscribirLog(ex, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion);
					}

					// crea el registro en base de datos
					ctl_resolucion = new Ctl_EmpresaResolucion();
					resolucion = ctl_resolucion.Crear(resolucion);
				}
				if (tipo_documento != TipoDocumento.Factura.GetHashCode())
				{
					documento_obj.NumeroResolucion = resolucion.StrNumResolucion;

				}

				return facturador_emisor;
			}
			catch (Exception excepcion)
			{
				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion);
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Convierte un objeto de Servicio en Objeto de BD
		/// </summary>
		/// <param name="documento">Objeto de Servicio</param>
		/// <param name="tipo_doc">Tipo del Documento</param>
		/// <returns>Objeto de BD</returns>
		public static TblDocumentos Convertir(object documento, TipoDocumento tipo_doc, TblEmpresas facturador_emisor, string nombre_archivo, string proveedor_emisor, bool archivo_pdf, bool archivo_adj)
		{
			try
			{
				PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

				// url pública
				//string url_ppal = LibreriaGlobalHGInet.Dms.ObtenerUrlPrincipal("", facturador_emisor.StrIdSeguridad.ToString());
				string url_ppal = string.Format(@"{0}/{1}/{2}/", plataforma_datos.RutaDmsPublica, Constantes.CarpetaFacturaElectronica, facturador_emisor.StrIdSeguridad);

				// url pública del xml
				string UrlXmlUbl = string.Format(@"{0}{1}/{2}.xml", url_ppal, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian, nombre_archivo);

				string url_ppal_zip = string.Empty;

				// url pública del zip de Anexos
				if (archivo_adj == true)
				{
					url_ppal_zip = string.Format(@"{0}{1}/{2}.zip", url_ppal, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEAnexos, nombre_archivo);

					//Valida que si exista el archivo en la ruta
					if (!Directorio.ValidarExistenciaArchivoUrl(url_ppal_zip))
						url_ppal_zip = string.Empty;
				}

				// url pública del pdf
				string url_ppal_pdf = string.Format(@"{0}{1}/{2}.pdf", url_ppal, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian, nombre_archivo);

				//Valida que si exista el archivo en la ruta
				if (!Directorio.ValidarExistenciaArchivoUrl(url_ppal_pdf))
				{
					string carpeta_fisica = string.Format(@"{0}/{1}/{2}/", plataforma_datos.RutaDmsFisica, Constantes.CarpetaFacturaElectronica, facturador_emisor.StrIdSeguridad);
					string ruta_fisica = string.Format(@"{0}{1}/{2}.pdf", carpeta_fisica, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian, nombre_archivo);

					if (!Archivo.ValidarExistencia(ruta_fisica))
						url_ppal_pdf = string.Empty;
				}
					

				//Generacion del tracking
				Guid tracking = Guid.NewGuid();

				//Asigna a un objeto dinamico el objeto enviado por el usuario
				var documento_obj = (dynamic)null;
				documento_obj = documento;

				TblDocumentos tbl_documento = new TblDocumentos();

				tbl_documento.DatFechaIngreso = Fecha.GetFecha();
				tbl_documento.IntDocTipo = tipo_doc.GetHashCode();
				tbl_documento.IntNumero = documento_obj.Documento;
				tbl_documento.StrPrefijo = (!string.IsNullOrEmpty(documento_obj.Prefijo)) ? documento_obj.Prefijo : "";
				if (tipo_doc == TipoDocumento.NotaCredito || tipo_doc == TipoDocumento.NotaDebito)
				{
					tbl_documento.DatFechaVencDocumento = documento_obj.Fecha;
				}
				else
				{
					tbl_documento.DatFechaVencDocumento = documento_obj.FechaVence;
				}
				tbl_documento.DatFechaDocumento = documento_obj.Fecha;
				tbl_documento.StrObligadoIdRegistro = tracking.ToString();
				tbl_documento.StrNumResolucion = documento_obj.NumeroResolucion;
				tbl_documento.StrEmpresaFacturador = documento_obj.DatosObligado.Identificacion;
				tbl_documento.StrEmpresaAdquiriente = documento_obj.DatosAdquiriente.Identificacion;
				tbl_documento.StrCufe = documento_obj.Cufe;
				tbl_documento.IntVlrTotal = documento_obj.Total;
				tbl_documento.IntValorSubtotal = documento_obj.ValorSubtotal;
				tbl_documento.IntValorNeto = documento_obj.Neto;
				tbl_documento.StrIdSeguridad = tracking;
				tbl_documento.IntAdquirienteRecibo = 0;
				tbl_documento.IntIdEstado = Convert.ToInt16(ProcesoEstado.EnvioEmailAcuse.GetHashCode());
				tbl_documento.IdCategoriaEstado = Convert.ToInt16(CategoriaEstado.ValidadoDian.GetHashCode());
				tbl_documento.DatFechaActualizaEstado = Fecha.GetFecha();
				tbl_documento.StrIdInteroperabilidad = tracking;
				tbl_documento.StrUrlArchivoUbl = UrlXmlUbl;
				tbl_documento.StrUrlArchivoPdf = url_ppal_pdf;
				tbl_documento.StrUrlAnexo = url_ppal_zip;
				tbl_documento.StrProveedorReceptor = Constantes.NitResolucionsinPrefijo;
				tbl_documento.StrProveedorEmisor = proveedor_emisor;
				tbl_documento.IntVersionDian = 2;


				return tbl_documento;
			}
			catch (Exception excepcion)
			{
				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Convertir, MensajeTipo.Error, MensajeAccion.creacion);
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Almacena el archivo Xml físicamente
		/// </summary>
		/// <param name="documento">datos del documento</param>
		/// <returns>datos del documento</returns>
		public static void AlmacenarArchivo(string ruta_archivo, string nombre_archivo, TblEmpresas facturador_emisor, bool documento_acuse = false, bool adjuntos = false, bool acuse_dian = false)
		{
			try
			{

				PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

				// carpeta del facturador emisor
				string carpeta = string.Format("{0}\\{1}\\{2}", plataforma_datos.RutaDmsFisica, Constantes.CarpetaFacturaElectronica, facturador_emisor.StrIdSeguridad);

				if (documento_acuse)
				{
					carpeta = string.Format(@"{0}\{1}", carpeta, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaXmlAcuse);
				}
				else if (acuse_dian)
				{
					carpeta = string.Format(@"{0}\{1}", carpeta, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEConsultaDian);
				}
				else
				{
					if (!adjuntos)
					{
						carpeta = string.Format(@"{0}\{1}", carpeta, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian);
					}
					else
					{
						carpeta = string.Format(@"{0}\{1}", carpeta, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEAnexos);
					}
				}

				// valida la existencia de la carpeta
				carpeta = Directorio.CrearDirectorio(carpeta);

				// Nombre del Archivo
				string archivo = nombre_archivo;

				// ruta del xml
				string ruta = string.Format(@"{0}{1}", carpeta, archivo);

				//Copia el archivo de la ruta de origen a la ruta destino
				Archivo.CopiarArchivo(ruta_archivo, ruta);


			}
			catch (Exception excepcion)
			{
				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Archivos, MensajeTipo.Error, MensajeAccion.creacion);
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}


		}


		/// <summary>
		/// Metodo para procesar los archivos descargados del correo
		/// </summary>
		/// <param name="ruta_archivo">Ruta donde estan alojados los archivos a procesar</param>
		/// <returns></returns>
		public static bool ProcesarCorreo(string ruta_archivo, bool emision)
		{
			//Ubicacion del archivo del Mail original para re-enviar correo cuando se presente algun fallo si se requiere
			string Nombre_arch_mail = Path.GetFileName(ruta_archivo);
			string ruta_archi_mail = ruta_archivo.Replace(string.Format("\\{0}", Nombre_arch_mail), "");
			ruta_archi_mail = string.Format(@"{0}\{1}.mail", ruta_archi_mail, Nombre_arch_mail);
			List<string> mensajes = null;

			try
			{
				bool respuesta = false;
				string ruta_xml = string.Empty;

				try
				{
					if (string.IsNullOrEmpty(ruta_archivo))
						throw new ApplicationException("Ruta de archivo inválido.");

					var archivo_attach = Directorio.ObtenerArchivosDirectorio(ruta_archivo, "*.xml").FirstOrDefault();

					if (archivo_attach == null)
						throw new ApplicationException(string.Format("No se encontró en la ruta {0} un archivo con extension de xml para procesar", ruta_archivo));

					// ruta del xml
					ruta_xml = string.Format(@"{0}\{1}", ruta_archivo, archivo_attach.Name);

					if (!Archivo.ValidarExistencia(ruta_xml))
						throw new ApplicationException(string.Format("No se encontró en la ruta {0} un archivo attach con extension de xml para procesar", ruta_xml));
				}
				catch (Exception excepcion)
				{
					RegistroLog.EscribirLog(excepcion, MensajeCategoria.Archivos, MensajeTipo.Error, MensajeAccion.lectura, excepcion.Message);

					//Si pasa algo envio notificacion a tic para validar por que no se proceso
					mensajes = new List<string>();
					mensajes.Add(string.Format("Error al procesar Archivos, Ruta archivo: {0}", ruta_archivo));
					mensajes.Add(excepcion.Message);
					if (excepcion.InnerException != null && !string.IsNullOrEmpty(excepcion.InnerException.Message))
						mensajes.Add(excepcion.InnerException.Message);
					try
					{
						Ctl_EnvioCorreos email = new Ctl_EnvioCorreos();
						email.EnviaNotificacionAlertaDIAN(Constantes.NitResolucionconPrefijo, "0", mensajes, 3, false, Constantes.EmailCopiaOculta, 2);
					}
					catch (Exception)
					{ }

					try
					{
						mensajes = new List<string>();
						mensajes.Add(string.Format("No se encontró un archivo con extension de {0} de tipo Attached Document para procesar", Path.GetFileName(ruta_xml)));

						//ReEnviarCorreoError(ruta_archi_mail, mensajes);

						RechazarCorreo(mensajes, ruta_archi_mail, true, emision);

					}
					catch (Exception e)
					{
					}

					ruta_archivo = string.Format("{0}\\Archivo_errores.json", ruta_archivo);

					// almacena el objeto en archivo json
					File.WriteAllText(ruta_archivo, JsonConvert.SerializeObject(mensajes));

					throw new ApplicationException(excepcion.Message, excepcion.InnerException);
				}

				//Se valida que el archivo recibido si sea de tipo Attached Document
				XmlDocument xDoc = new XmlDocument();
				try
				{
					xDoc.Load(ruta_xml);
				}
				catch (Exception ex)
				{
					mensajes = new List<string>();
					mensajes.Add(string.Format("El archivo {0} no cumple con la estructura establecida en el Anexo técnico, se esperaba un XML-UBL tipo AttachedDocument", Path.GetFileName(ruta_xml)));
					mensajes.Add(ex.Message);

					RechazarCorreo(mensajes, ruta_archi_mail, true, emision);
				}

				XmlNodeList xAttach = xDoc.GetElementsByTagName("AttachedDocument");

				if (xAttach.Count == 0)
				{
					try
					{
						mensajes = new List<string>();
						mensajes.Add(string.Format("El archivo {0} no cumple con la estructura establecida en el Anexo técnico, se esperaba un XML-UBL tipo AttachedDocument", Path.GetFileName(ruta_xml)));

						//ReEnviarCorreoError(ruta_archi_mail, mensajes);

						RechazarCorreo(mensajes, ruta_archi_mail,true, emision);

					}
					catch (Exception e)
					{
					}

					throw new ApplicationException(string.Format("El archivo {0} no cumple con la estructura establecida en el Anexo técnico, se esperaba un XML-UBL tipo AttachedDocument", Path.GetFileName(ruta_xml)));
				}

				//Se lee XML
				FileStream xml_reader_serializacion = new FileStream(ruta_xml, FileMode.Open);

				XmlSerializer serializacion1 = null;
				AttachedDocumentType obj_attach_serializado = null;
				//Acuse respuesta_adquiriente = null;
				
				try
				{
					// convierte el objeto de acuerdo con el tipo de documento
					serializacion1 = new XmlSerializer(typeof(AttachedDocumentType));

					obj_attach_serializado = (AttachedDocumentType)serializacion1.Deserialize(xml_reader_serializacion);

					if (obj_attach_serializado.Attachment == null)
					{
						try
						{
							mensajes = new List<string>();
							mensajes.Add(string.Format("El archivo {0} no cumple con la estructura establecida en el Anexo técnico", Path.GetFileName(ruta_xml)));

							//ReEnviarCorreoError(ruta_archi_mail, mensajes);

							RechazarCorreo(mensajes, ruta_archi_mail, true, emision);

						}
						catch (Exception e)
						{
						}

						throw new ApplicationException(string.Format("El archivo {0} no cumple con la estructura establecida en el Anexo técnico", Path.GetFileName(ruta_xml)));
					}
				}
				catch (Exception ex)
				{
					//Si pasa algo envio notificacion a tic para validar por que no se proceso
					mensajes = new List<string>();
					mensajes.Add(string.Format("Error al procesar Archivos, Ruta archivo: {0}", ruta_archivo));
					mensajes.Add(ex.Message);
					if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
						mensajes.Add(ex.InnerException.Message);
					try
					{
						Ctl_EnvioCorreos email = new Ctl_EnvioCorreos();
						email.EnviaNotificacionAlertaDIAN(Constantes.NitResolucionconPrefijo, "0", mensajes, 3, false, Constantes.EmailCopiaOculta, 2);
					}
					catch (Exception)
					{ }

					ruta_archivo = string.Format("{0}\\Archivo_errores.json", ruta_archivo);

					// almacena el objeto en archivo json
					File.WriteAllText(ruta_archivo, JsonConvert.SerializeObject(mensajes));

					//try
					//{
					//	mensajes = new List<string>();
					//	string inner_exc = ex.InnerException != null ? ex.InnerException.Message : string.Empty;
					//	mensajes.Add(string.Format("El archivo {0} no se pudo procesar por la siguiente inconsistencia: {1} - {2}", Path.GetFileName(ruta_xml), ex.Message, inner_exc));

					//	//ReEnviarCorreoError(ruta_archi_mail, mensajes);

					mensajes = new List<string>();

					string mensaje = ex.Message;

					if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
						mensaje = string.Format("{0} - {1}", mensaje, ex.InnerException.Message);

					mensajes.Add(string.Format("Error al serializar el archivo Attached Document Detalle: {0}" , mensaje));

					RechazarCorreo(mensajes, ruta_archi_mail, false, emision);

					//}
					//catch (Exception e)
					//{
					//}

					//Cierro la lectura
					xml_reader_serializacion.Close();

					throw new ApplicationException(string.Format("Error al serializar el archivo en la ruta {0} Detalle: {1}", ruta_xml, mensaje));

					//try
					//{
					//	// convierte el objeto de acuerdo con el tipo de documento
					//	serializacion1 = new XmlSerializer(typeof(ApplicationResponseType));

					//	ApplicationResponseType conversion = (ApplicationResponseType)serializacion1.Deserialize(xml_reader_serializacion);

					//	respuesta_adquiriente = AcuseReciboXMLv2_1.Convertir(conversion).FirstOrDefault();

					//	tipo_doc_proceso = 1;
					//}
					//catch (Exception excepcion)
					//{
					//	RegistroLog.EscribirLog(excepcion, MensajeCategoria.Archivos, MensajeTipo.Error, MensajeAccion.lectura, "No se pudo procesar archivo tampoco como attach document de application response");
					//}

				}

				//Cierro la lectura
				xml_reader_serializacion.Close();

				// 0 - AttachDocument, 1 - ApplicationResponse(Acuse Cliente)
				//int tipo_doc_proceso = 0;

				//Se valida si no es un acuse de un cliente
				if (!obj_attach_serializado.Attachment.ExternalReference.Description.FirstOrDefault().Value.Contains("ApplicationResponse"))
				{
					respuesta = ProcesarAttach(obj_attach_serializado, ruta_archivo, ruta_archi_mail, emision);
				}
				else
				{
					//Falta definir proceso y metodo para cuando sea un acuse de un adquiriente de nuestro facturador
				}


				return respuesta;
			}
			catch (Exception excepcion)
			{
				try
				{
					Ctl_RegistroRecepcion _ctrl_registro = new Ctl_RegistroRecepcion();
					TblRegistroRecepcion tblregistro = _ctrl_registro.Obtener(Guid.Parse(Nombre_arch_mail));
					if (tblregistro != null && tblregistro.IntEstado != 1)
					{
						tblregistro.IntEstado = 1;
						if (mensajes == null)
							mensajes.Add(excepcion.Message);
						tblregistro.StrObservaciones = string.Format("{0} - {1}", tblregistro.StrObservaciones, JsonConvert.SerializeObject(mensajes));

						_ctrl_registro.Actualizar(tblregistro);
					}
				}
				catch (Exception)
				{

				}
				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Archivos, MensajeTipo.Error, MensajeAccion.creacion);
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}

		}

		public static void ReEnviarCorreoError(string ruta_archivo_mail, List<string> mensajes_inconsistencias, bool emision)
		{
			try
			{
				if (!Archivo.ValidarExistencia(ruta_archivo_mail))
				{
					PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

					string ruta_archivos = string.Format(@"{0}\{1}", plataforma_datos.RutaDmsFisica, Constantes.RutaInteroperabilidadRecepcion);

					string ruta_mail_noprocesado = ruta_archivos.Replace("recepcion\\", "no procesados\\Archivos");
					if (emision == true)
						ruta_mail_noprocesado = ruta_archivos.Replace("emision\\", "no procesados\\Archivos");

					string nombre_archivo_mail = string.Format("Mail - {0}", Path.GetFileName(ruta_archivo_mail));

					ruta_mail_noprocesado = string.Format(@"{0}\{1}", ruta_mail_noprocesado, nombre_archivo_mail);

					if (Archivo.ValidarExistencia(ruta_mail_noprocesado))
					{
						ruta_archivo_mail = ruta_mail_noprocesado;
					}
					else
					{
						RegistroLog.EscribirLog(null, MensajeCategoria.Archivos, MensajeTipo.Error, MensajeAccion.lectura, "inconsistencia validando ruta del correo original");
						throw new ApplicationException("inconsistencia validando ruta del correo original");
					}
				}

				MimeMessage mail_original = MimeMessage.Load(ruta_archivo_mail);

				Ctl_MailRecepcion.EnviarAlerta(mail_original, mensajes_inconsistencias);
			}
			catch (Exception excepcion)
			{
				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.envio, "inconsistencia tratando de enviar el correo original al remitente");
			}
		}


		public static void Validaciones(Attached attach_document, Acuse doc_acuse_Dian, object objeto_doc, TipoDocumento tipo_doc, ref bool validar_dian)
		{

			try
			{
				if (tipo_doc == TipoDocumento.Attached)
				{
					CodigoResponseV2 cod_respuestaDian = Enumeracion.GetValueFromAmbiente<CodigoResponseV2>(doc_acuse_Dian.CodigoRespuesta);

					if (!cod_respuestaDian.Equals(CodigoResponseV2.ValidadoDian))
						throw new ArgumentException(string.Format("El Documento Electrónico no esta validado por la DIAN, el application response indica: {0}", Enumeracion.GetDescription(cod_respuestaDian)));

					//if (!attach_document.CufeDocumentoElectronico.Equals(doc_acuse_Dian.CufeDocumento))
					//	throw new ArgumentException("El CUFE del Documento Electrónico no coincide con la respuesta de la DIAN recibida en el AttachDocument");

					if (!string.Format("{0}{1}", attach_document.Prefijo, attach_document.Documento).Equals(string.Format("{0}{1}", doc_acuse_Dian.Prefijo, doc_acuse_Dian.Documento)))
					{
						if (string.IsNullOrEmpty(attach_document.CufeDocumentoElectronico) || !attach_document.CufeDocumentoElectronico.Equals(doc_acuse_Dian.CufeDocumento))
							throw new ArgumentException("El CUFE del Documento Electrónico en el AttachDocument no coincide con la respuesta de la DIAN recibida ");
					}

					//if (!attach_document.IdentificacionFacturador.Equals(doc_acuse_Dian.DatosObligado.Identificacion))
					//	throw new ArgumentException("El facturador del AttachDocument no coincide con el de la respuesta de la DIAN");
				}
				else
				{
					// representación de datos en objeto
					var documento_obj = (dynamic)null;
					documento_obj = objeto_doc;

					if (!string.Format("{0}{1}", attach_document.Prefijo, attach_document.Documento).Equals(string.Format("{0}{1}", documento_obj.Prefijo, documento_obj.Documento)))
					{
						if (!attach_document.CufeDocumentoElectronico.Equals(documento_obj.Cufe))
							throw new ArgumentException("El CUFE del Documento Electrónico no coincide con el AttachDocument recibido");
					}

					if (!string.Format("{0}{1}", doc_acuse_Dian.Prefijo, doc_acuse_Dian.Documento).Equals(string.Format("{0}{1}", documento_obj.Prefijo, documento_obj.Documento)))
					{
						if (!doc_acuse_Dian.CufeDocumento.Equals(documento_obj.Cufe))
							validar_dian = true;
					}

					//if (!attach_document.IdentificacionFacturador.Equals(documento_obj.DatosObligado.Identificacion))
					//	throw new ArgumentException("El Facturador del Documento Electrónico no coincide con el del AttachDocument recibido");

					if (!attach_document.Identificacionadquiriente.Equals(documento_obj.DatosAdquiriente.Identificacion))
						throw new ArgumentException("El Adquiriente del Documento Electrónico no coincide con el del AttachDocument recibido");

					// convierte el contenido de texto a xml
					XmlReader xml_reader = XmlReader.Create(new StringReader(attach_document.DocumentoElectronico.Trim()));

					XmlSerializer serializacion = null;

					if (tipo_doc == TipoDocumento.Factura)
					{
						serializacion = new XmlSerializer(typeof(InvoiceType));

						InvoiceType conversion = (InvoiceType)serializacion.Deserialize(xml_reader);

						if (conversion.ProfileExecutionID.Value.Equals("2"))
							throw new ArgumentException("El Documento Electrónico tiene registrado que fue enviado al ambiente de pruebas");

					}
				}
				
				
				#region Proceso Validacion del Certificado
				//Captura la Firma del documento
				/*
				foreach (XmlNode item in conversion_acuse.UBLExtensions[1].ExtensionContent.ChildNodes)
				{

					if (item.LocalName.Equals("KeyInfo"))
					{
						foreach (XmlNode item_child in item.ChildNodes)
						{
							if (item_child.LocalName.Equals("X509Data"))
							{
								byte[] myBase64ret = Convert.FromBase64String(item_child.InnerText);
								X509Certificate2 firma_dian = new X509Certificate2(myBase64ret);

								if (!firma_dian.SubjectName.Name.Contains(doc_acuse_Dian.DatosAdquiriente.Identificacion))
									throw new ArgumentException("El certificado digital del acuse de la DIAN no corresponde a esta entidad");


								#region Falta Validar la Firma o que el documento no este modificado
								/*
														SignatureDocument signatureDocument = new SignatureDocument();
														signatureDocument.Document = xml_doc;

														SignatureParameters parametros = new SignatureParameters();
														// elemento donde se encontrará la firma de acuerdo con la raíz del documento xml
														parametros.SignatureDestination = new SignatureXPathExpression();
														parametros.SignatureDestination.XPathExpression = string.Format("/{0}/ext:UBLExtensions/ext:UBLExtension[1]/ext:ExtensionContent", xml_doc.DocumentElement.Name);
														//parametros.Signer.Certificate.GetPublicKey() = firma_dian;

														XadesService xadesService = new XadesService();



														// lee el certificado digital para firmar
														using (parametros.Signer = new Signer(firma_dian))
														{

															// firma el xml
															var docFirmado = xadesService.CounterSign(signatureDocument, parametros);

															// valida la firma del xml
															SignatureDocument.CheckSignatureDocument(docFirmado);
														}

														SignatureDocument.CheckSignatureDocument(signatureDocument);

														bool valido = VerificarXML(xml_doc,(RSA) firma_dian.GetRSAPublicKey());
														
								#endregion
							}

						}
					}
				}*/
				#endregion


			}
			catch (Exception excepcion)
			{
				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Archivos, MensajeTipo.Error, MensajeAccion.creacion);
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}

		}

		public static void ProcesarArchivos(string ruta_archivo, string nombre_archivo, TblEmpresas facturador_emisor, Attached attach_document, ref bool contiene_pdf, ref bool contiene_anexo)
		{

			try
			{
				//Obtengo los archivos originales para ponerlos en la carpeta del facturador con el nombre como lo maneja la plataforma
				var archivos = Directorio.ObtenerArchivosDirectorio(ruta_archivo);

				foreach (var item in archivos)
				{
					if (item.Extension.ToLowerInvariant().Equals(".pdf"))
					{
						AlmacenarArchivo(string.Format(@"{0}\{1}", ruta_archivo, item.Name), string.Format(@"{0}{1}", nombre_archivo, item.Extension), facturador_emisor);
						contiene_pdf = true;
					}
					else if (item.Extension.ToLowerInvariant().Equals(".zip"))
					{
						AlmacenarArchivo(string.Format(@"{0}\{1}", ruta_archivo, item.Name), string.Format(@"{0}{1}", nombre_archivo, item.Extension), facturador_emisor, false, true);
						contiene_anexo = true;
					}
					else
					{
						AlmacenarArchivo(string.Format(@"{0}\{1}", ruta_archivo, item.Name), string.Format(@"{0}{1}", nombre_archivo.Replace("fv", "ad"), item.Extension), facturador_emisor);
					}

				}

				//Convierto el Documento a XML para pasarlo a la ruta del facturador
				StringBuilder txt_xml = null;
				string ruta_save = string.Empty;
				string ruta_doc_xml = string.Empty;

				txt_xml = new StringBuilder(attach_document.DocumentoElectronico);

				// ruta del xml
				ruta_doc_xml = string.Format(@"{0}\{1}", ruta_archivo, string.Format("{0}.xml", nombre_archivo));

				// elimina el archivo xml si existe
				if (Archivo.ValidarExistencia(ruta_doc_xml))
					Archivo.Borrar(ruta_doc_xml);

				// almacena el archivo xml
				ruta_save = Xml.Guardar(txt_xml, string.Format(@"{0}\", ruta_archivo), string.Format("{0}.xml", nombre_archivo));

				AlmacenarArchivo(ruta_save, string.Format("{0}.xml", nombre_archivo), facturador_emisor);

				//Convierto la respuesta de la Dian en XML para pasarla a la ruta del facturador
				txt_xml = new StringBuilder(attach_document.RespuestaDianXml);

				// ruta del xml
				ruta_doc_xml = string.Format(@"{0}\{1}", ruta_archivo, string.Format("{0}.xml", nombre_archivo));

				// elimina el archivo xml si existe
				if (Archivo.ValidarExistencia(ruta_doc_xml))
					Archivo.Borrar(ruta_doc_xml);

				// almacena el archivo xml
				ruta_save = Xml.Guardar(txt_xml, string.Format(@"{0}\", ruta_archivo), string.Format("{0}.xml", nombre_archivo));

				//guardo respuesta de la DIAN en la carpeta del Facturador
				AlmacenarArchivo(ruta_save, string.Format("{0}.xml", nombre_archivo), facturador_emisor, false, false, true);

				Archivo.Borrar(ruta_doc_xml);

			}
			catch (Exception excepcion)
			{

				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion);
				throw new ApplicationException(string.Format("Error al almacenar el archivo {0} Detalle: {1}", attach_document.Documento, excepcion.Message));

			}

		}

		public static bool ProcesarAttach(AttachedDocumentType obj_attach_serializado, string ruta_archivo, string ruta_archivo_mail, bool emision)
		{

			bool respuesta = false;

			string nombre_archivo = string.Empty;

			Attached attach_document = new Attached();

			try
			{

				//Convierto XML-UBL en Objeto
				try
				{
					attach_document = AttachedDocument.Convertir(obj_attach_serializado);
				}
				catch (Exception excepcion)
				{
					string mensaje = string.Format("El Attached Document no cumple con la estructura indicada por la DIAN: {0}", excepcion.Message);
					List<string> mensajes = new List<string>();
					mensajes.Add(mensaje);

					//ReEnviarCorreoError(ruta_archivo_mail, mensajes);

					RechazarCorreo(mensajes, ruta_archivo_mail, true, emision);

					throw new ApplicationException(mensaje);
				}

				if (string.IsNullOrEmpty(attach_document.RespuestaDianXml))
				{
					string mensaje = "El Attached Document no cumple con la estructura indicada por la DIAN, no se encuentra el application response de la DIAN";

					List<string> mensajes = new List<string>();
					mensajes.Add(mensaje);

					RechazarCorreo(mensajes, ruta_archivo_mail, true, emision);

					throw new ArgumentException(mensaje);
				}

				//Se valida que el archivo recibido si sea de tipo Attached Document
				XmlDocument xDoc = new XmlDocument();
				try
				{
					xDoc.LoadXml(attach_document.RespuestaDianXml);
				}
				catch (Exception ex)
				{
					List<string> mensajes = new List<string>();
					mensajes.Add("El Attached Document no cumple con la estructura indicada por la DIAN en el Anexo técnico, se esperaba un XML-UBL tipo ApplicationResponse en su contenido");
					mensajes.Add(ex.Message);

					RechazarCorreo(mensajes, ruta_archivo_mail, true, emision);
				}

				XmlNodeList xApplication = xDoc.GetElementsByTagName("ApplicationResponse");

				if (xApplication.Count == 0)
				{
					try
					{
						List<string> mensajes = new List<string>();
						mensajes.Add("El Attached Document no cumple con la estructura indicada por la DIAN en el Anexo técnico, se esperaba un XML-UBL tipo ApplicationResponse en su contenido");

						//ReEnviarCorreoError(ruta_archi_mail, mensajes);

						RechazarCorreo(mensajes, ruta_archivo_mail, true, emision);

					}
					catch (Exception e)
					{
					}

					throw new ApplicationException("El Attached Document no cumple con la estructura indicada por la DIAN en el Anexo técnico, se esperaba un XML-UBL tipo ApplicationResponse en su contenido");
				}

				// representación de datos en objeto
				var documento_acuse = (dynamic)null;

				documento_acuse = ObtenerDocumento(attach_document.RespuestaDianXml, TipoDocumento.AcuseRecibo);

				Acuse doc_acuse_Dian = documento_acuse;

				bool validar_doc_dian = false;

				//Validaciones del Attach
				try
				{
					Validaciones(attach_document, doc_acuse_Dian, null, TipoDocumento.Attached, ref validar_doc_dian);
				}
				catch (Exception excepcion)
				{
					string mensaje = string.Format("Error validando el Attached Document recibido Detalle: {0}", excepcion.Message);
					List<string> mensajes = new List<string>();
					mensajes.Add(mensaje);

					//ReEnviarCorreoError(ruta_archivo_mail, mensajes);

					RechazarCorreo(mensajes, ruta_archivo_mail, true, emision);

					throw new ApplicationException(mensaje);
				}

				// obtiene los datos del Adquiriente enviado, que es nuestro facturador(Cliente)
				Ctl_Empresa empresa = new Ctl_Empresa();
				TblEmpresas facturador_receptor = null;
				try
				{
					if (emision == false)
						facturador_receptor = empresa.ValidarInteroperabilidad(attach_document.Identificacionadquiriente);
					else
						facturador_receptor = empresa.ValidarInteroperabilidad(attach_document.IdentificacionFacturador);
				}
				catch (Exception excepcion)
				{
					List<string> mensajes = new List<string>();
					mensajes.Add(string.Format("Error validando el Facturador receptor Detalle: {0}", excepcion.Message));

					//ReEnviarCorreoError(ruta_archivo_mail, mensajes);

					RechazarCorreo(mensajes, ruta_archivo_mail, true, emision);

					throw new ApplicationException(string.Format("Error validando el Facturador receptor Detalle: {0}", excepcion.Message));
				}

				//Se valida si el facturador cumple segun el proceso
				bool facturador_habilitado = false;

				if (emision == false && facturador_receptor.IntInteroperabilidad == true)
					facturador_habilitado = true;
				else if (facturador_receptor.IntRadian == true)
					facturador_habilitado = true;


				//Si tiene habilitado este servicio hace todo el proceso
				if (facturador_receptor != null && facturador_habilitado == true)
				{
					// representación de datos en objeto
					var documento_obj = (dynamic)null;

					int tipo_doc = 0;



					if (attach_document.DocumentoElectronico.Contains("<cbc:InvoiceTypeCode"))
					{
						try
						{
							tipo_doc = TipoDocumento.Factura.GetHashCode();
							documento_obj = ObtenerDocumento(attach_document.DocumentoElectronico, TipoDocumento.Factura);
						}
						catch (Exception excepcion)
						{
							string mensaje = string.Format("No es posible procesar el documento, se presenta inconsistencia conviertiendo el XMl-UBL de la Factura del documento recibido Detalle: {0}", excepcion.Message);
							List<string> mensajes = new List<string>();
							mensajes.Add(mensaje);

							//ReEnviarCorreoError(ruta_archivo_mail, mensajes);

							RechazarCorreo(mensajes, ruta_archivo_mail, true, emision);

							throw new ApplicationException(mensaje);
						}

						//Validaciones del objeto
						try
						{
							Validaciones(attach_document, doc_acuse_Dian, documento_obj, TipoDocumento.Factura, ref validar_doc_dian);
						}
						catch (Exception excepcion)
						{
							string mensaje = string.Format("No es posible procesar el documento, se presenta inconsistencia validando el XMl-UBL del documento recibido Detalle: {0}", excepcion.Message);
							List<string> mensajes = new List<string>();
							mensajes.Add(mensaje);

							//ReEnviarCorreoError(ruta_archivo_mail, mensajes);

							RechazarCorreo(mensajes, ruta_archivo_mail, true, emision);

							throw new ApplicationException(mensaje);
						}

						nombre_archivo = HGInetUBLv2_1.NombramientoArchivo.ObtenerXml(documento_obj.Documento.ToString(), documento_obj.DatosObligado.Identificacion, TipoDocumento.Factura, documento_obj.Prefijo);
					}
					else if (attach_document.DocumentoElectronico.Contains("<cbc:CreditNoteTypeCode"))
					{
						try
						{
							tipo_doc = TipoDocumento.NotaCredito.GetHashCode();
							documento_obj = ObtenerDocumento(attach_document.DocumentoElectronico, TipoDocumento.NotaCredito);
						}
						catch (Exception excepcion)
						{
							string mensaje = string.Format("No es posible procesar el documento, se presenta inconsistencia conviertiendo el XMl-UBL de la Nota Crédito del documento recibido Detalle: {0}", excepcion.Message);
							List<string> mensajes = new List<string>();
							mensajes.Add(mensaje);

							//ReEnviarCorreoError(ruta_archivo_mail, mensajes);

							RechazarCorreo(mensajes, ruta_archivo_mail, true, emision);

							throw new ApplicationException(mensaje);
						}

						//Validaciones del objeto
						try
						{
							Validaciones(attach_document, doc_acuse_Dian, documento_obj, TipoDocumento.NotaCredito, ref validar_doc_dian);
						}
						catch (Exception excepcion)
						{
							string mensaje = string.Format("No es posible procesar el documento, se presenta inconsistencia validando el XMl-UBL del documento recibido Detalle: {0}", excepcion.Message);
							List<string> mensajes = new List<string>();
							mensajes.Add(mensaje);

							//ReEnviarCorreoError(ruta_archivo_mail, mensajes);

							RechazarCorreo(mensajes, ruta_archivo_mail, true, emision);

							throw new ApplicationException(mensaje);
						}

						nombre_archivo = HGInetUBLv2_1.NombramientoArchivo.ObtenerXml(documento_obj.Documento.ToString(), documento_obj.DatosObligado.Identificacion, TipoDocumento.NotaCredito, documento_obj.Prefijo);
					}
					else if (attach_document.DocumentoElectronico.Contains("xsd:DebitNote-2"))
					{
						try
						{
							tipo_doc = TipoDocumento.NotaDebito.GetHashCode();
							documento_obj = ObtenerDocumento(attach_document.DocumentoElectronico, TipoDocumento.NotaDebito);
						}
						catch (Exception excepcion)
						{
							string mensaje = string.Format("No es posible procesar el documento, se presenta inconsistencia conviertiendo el XMl-UBL de la Nota Débito del documento recibido Detalle: {0}", excepcion.Message);
							List<string> mensajes = new List<string>();
							mensajes.Add(mensaje);

							//ReEnviarCorreoError(ruta_archivo_mail, mensajes);

							RechazarCorreo(mensajes, ruta_archivo_mail, true, emision);

							throw new ApplicationException(mensaje);
						}

						//Validaciones del objeto
						try
						{
							Validaciones(attach_document, doc_acuse_Dian, documento_obj, TipoDocumento.NotaDebito, ref validar_doc_dian);
						}
						catch (Exception excepcion)
						{
							string mensaje = string.Format("Error validando el XMl-UBL del documento recibido Detalle: {0}", excepcion.Message);
							List<string> mensajes = new List<string>();
							mensajes.Add(mensaje);

							//ReEnviarCorreoError(ruta_archivo_mail, mensajes);

							RechazarCorreo(mensajes, ruta_archivo_mail, true, emision);

							throw new ApplicationException(mensaje);
						}

						nombre_archivo = HGInetUBLv2_1.NombramientoArchivo.ObtenerXml(documento_obj.Documento.ToString(), documento_obj.DatosObligado.Identificacion, TipoDocumento.NotaDebito, documento_obj.Prefijo);

					}
					else
					{
						try
						{
							List<string> mensajes = new List<string>();
							mensajes.Add("El Attached Document no cumple con la estructura indicada por la DIAN en el Anexo técnico, se esperaba un XML-UBL tipo Invoice, CreditNoteType o DebitNote en su contenido");

							//ReEnviarCorreoError(ruta_archi_mail, mensajes);

							RechazarCorreo(mensajes, ruta_archivo_mail, true, emision);

						}
						catch (Exception e)
						{
						}

						throw new ApplicationException("El Attached Document no cumple con la estructura indicada por la DIAN en el Anexo técnico, se esperaba un XML-UBL tipo Invoice, CreditNoteType o DebitNote en su contenido");
					}

					//Creacion Facturador Emisor del documento 
					TblEmpresas facturador_emisor = new TblEmpresas();

					if (emision == false)
					{
						try
						{
							//Se valida que la identificacion del facturador del documento este bien formada
							if (!string.IsNullOrEmpty(documento_obj.DatosObligado.Identificacion))
							{
								if (documento_obj.DatosObligado.TipoIdentificacion.Equals(31) || documento_obj.DatosObligado.TipoIdentificacion.Equals(13))
								{
									if (!Texto.ValidarExpresion(TipoExpresion.Numero, documento_obj.DatosObligado.Identificacion) && !Texto.ValidarExpresion(TipoExpresion.Alfanumerico, documento_obj.DatosObligado.Identificacion))
										throw new ArgumentException(string.Format("La identificación {0} del Facturador Emisor no puede contener caracteres especiales", documento_obj.DatosObligado.Identificacion));

									// valida los ceros al inicio de la identificación
									if (!Texto.ValidarExpresion(TipoExpresion.NumeroNotStartZero, documento_obj.DatosObligado.Identificacion))
										throw new ArgumentException(string.Format("La identificación {0} del Facturador Emisor no puede contener ceros al inicio", documento_obj.DatosObligado.Identificacion));
								}
							}
							else
								throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "Identificacion", "Facturador Emisor").Replace("de tipo", "del"));

							facturador_emisor = CrearFacturadorEmisor(documento_obj, tipo_doc.GetHashCode(), facturador_receptor.IntHabilitacion.Value);


						}
						catch (Exception excepcion)
						{
							RegistroLog.EscribirLog(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion);
							throw new ApplicationException(string.Format("Error creando el Facturador emisor con identificacion {0} Detalle: {1}", documento_obj.DatosObligado.Identificacion, excepcion.Message));
						}
					}
					else
					{
						//Se hace validacion para la creacion de la Resolucion del facturador emisor que la tiene con otro proveedor
						facturador_emisor = CrearFacturadorEmisor(documento_obj, tipo_doc.GetHashCode(), facturador_receptor.IntHabilitacion.Value);

						Ctl_Empresa empresa_config = new Ctl_Empresa();

						TblEmpresas adquirienteBd = null;

						//Validacion de Adquiriente
						try
						{

							//Obtiene la informacion del Adquiriente que se tiene en BD
							adquirienteBd = empresa_config.Obtener(documento_obj.DatosAdquiriente.Identificacion);

							//Si no existe Adquiriente se crea en BD y se crea Usuario
							if (adquirienteBd == null)
							{
								empresa_config = new Ctl_Empresa();
								//Creacion del Adquiriente
								adquirienteBd = empresa_config.Crear(documento_obj.DatosAdquiriente);

							}
						}
						catch (Exception excepcion)
						{
							RegistroLog.EscribirLog(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion);
							throw new ApplicationException(string.Format("Error creando el Adquiriente con identificacion {0} Detalle: {1}", documento_obj.DatosAdquiriente.Identificacion, excepcion.Message));
						}
					}

					TblDocumentos documento_bd = new TblDocumentos();
					Ctl_Documento ctl_doc = new Ctl_Documento();
					bool doc_nuevo = true;

					//valida si el Documento ya existe en Base de Datos
					documento_bd = ctl_doc.Obtener(facturador_emisor.StrIdentificacion, documento_obj.Documento, documento_obj.Prefijo);

					if (documento_bd == null)
					{

						//Proceso para Gestionar los archivo recibidos y los convertidos en la ruta del facturador emisor
						bool contiene_pdf = false;
						bool contiene_anexo = false;

						try
						{
							ProcesarArchivos(ruta_archivo, nombre_archivo, facturador_emisor, attach_document, ref contiene_pdf, ref contiene_anexo);
						}
						catch (Exception excepcion)
						{

							RegistroLog.EscribirLog(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion);
							throw new ApplicationException(string.Format("Error al almacenar el archivo {0} Detalle: {1}", documento_obj.Documento, excepcion.Message));

						}

						//Convierto el Objeto a Tbl
						try
						{
							documento_bd = Convertir(documento_obj, Enumeracion.GetEnumObjectByValue<TipoDocumento>(tipo_doc), facturador_emisor, nombre_archivo, documento_obj.IdentificacionProveedor, contiene_pdf, contiene_anexo);
							documento_bd.IntEnvioMail = false;
							if (documento_bd.IntDocTipo == TipoDocumento.Factura.GetHashCode())
							{
								documento_bd.IntFormaPago = (documento_obj.FormaPago == 0) ? 1 : Convert.ToInt16(documento_obj.FormaPago);
							}
						}
						catch (Exception excepcion)
						{
							RegistroLog.EscribirLog(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion);
							throw new ApplicationException(string.Format("Error al convertir objeto {0} Detalle: {1}", documento_obj.Documento, excepcion.Message));
						}

						try
						{
							if (validar_doc_dian == true)
							{
								DocumentoRespuesta resp = new DocumentoRespuesta();

								HGInetMiFacturaElectonicaController.Procesos.Ctl_Documentos.Consultar(documento_bd, facturador_receptor, ref resp, string.Empty, 0 , false);

								// valida si la Respuesta de la DIAN es correcta, siginifica que tiene eventos
								if (resp.EstadoDian != null && resp.EstadoDian.EstadoDocumento != EstadoDocumentoDian.Aceptado.GetHashCode())
									throw new ArgumentException(string.Format("El CUFE {0} del Documento Electrónico recibido en el AttachDocument no existe en la DIAN", documento_bd.StrCufe));
								else if (resp.EstadoDian != null && resp.EstadoDian.EstadoDocumento == EstadoDocumentoDian.Pendiente.GetHashCode())
									throw new ArgumentException(string.Format("El CUFE {0} del Documento Electrónico recibido en el AttachDocument no se pudo validar en la DIAN", documento_bd.StrCufe));
								else if (resp.EstadoDian == null)
									throw new ArgumentException("El CUFE del Documento Electrónico no coincide con la respuesta de la DIAN recibida en el AttachDocument y no fue posible consultar el documento en la DIAN");

							}
						}
						catch (Exception excepcion)
						{
							string mensaje = string.Format("Error validando el XMl-UBL del documento recibido Detalle: {0}", excepcion.Message);
							List<string> mensajes = new List<string>();
							mensajes.Add(mensaje);

							//ReEnviarCorreoError(ruta_archivo_mail, mensajes);

							RechazarCorreo(mensajes, ruta_archivo_mail, false, emision);

							throw new ApplicationException(mensaje);
						}

						//Valida Proveedor Emisor
						try
						{
							Ctl_ConfiguracionInteroperabilidad config_inter = new Ctl_ConfiguracionInteroperabilidad();
							TblConfiguracionInteroperabilidad proveedor_emisor = config_inter.Obtener(documento_obj.IdentificacionProveedor);
							if (proveedor_emisor == null)
							{
								proveedor_emisor = config_inter.GuardarProveedor(documento_obj.IdentificacionProveedor, string.Empty, string.Format("Proveedor - {0}", facturador_emisor.StrRazonSocial), facturador_emisor.StrMailAdmin, facturador_emisor.StrTelefono, string.Format("Creado por documento recibido - {0}", documento_obj.Documento), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, true, Fecha.GetFecha(), Fecha.GetFecha());
							}
						}
						catch (Exception excepcion)
						{
							RegistroLog.EscribirLog(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion);
							throw new ApplicationException(string.Format("Error al crear proveedor Emisor {0} Detalle: {1}", documento_obj.IdentificacionProveedor, excepcion.Message));
						}

						Ctl_PlanesTransacciones Planestransacciones = new Ctl_PlanesTransacciones();
						List<ObjPlanEnProceso> ListaPlanes = null;

						//asignacion plan de documentos
						try
						{
							ListaPlanes = Planestransacciones.ObtenerPlanesActivos(facturador_receptor.StrIdentificacion, 1, TipoDocPlanes.Documento.GetHashCode());
						}
						catch (Exception excepcion)
						{
							RegistroLog.EscribirLog(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion, string.Format("Se genera inconsistencias en el proceso de planes. Detalle: {0}", excepcion.Message));
							throw new ApplicationException(string.Format("Se genera inconsistencias en el proceso de planes. Detalle: {0}", excepcion.Message));
						}

						if (ListaPlanes == null)
						{
							///Validación de alertas y notificaciones
							try
							{
								Ctl_Alertas controlador = new Ctl_Alertas();
								controlador.alertaSinSaldo(facturador_receptor.StrIdentificacion);
							}
							catch (Exception excepcion)
							{
								LogExcepcion.Guardar(excepcion);
							}

							string mensaje = "No se encontró saldo disponible para procesar los documentos";
							List<string> mensajes = new List<string>();
							mensajes.Add(mensaje);

							RechazarCorreo(mensajes, ruta_archivo_mail, false, emision);

							throw new ApplicationException(mensaje);
						}

						documento_bd.StrIdPlanTransaccion = Guid.Parse(ListaPlanes.FirstOrDefault().plan.ToString());
						ListaPlanes.FirstOrDefault().reservado += 1;

						//Guardo el documento en BD
						try
						{
							documento_bd = ctl_doc.Crear(documento_bd);

							//Se hace proceso de conciliacion de planes
							if (ListaPlanes.FirstOrDefault().reservado == 1)
							{
								ListaPlanes.FirstOrDefault().procesado += 1;
								var datos = Planestransacciones.ConciliarPlanProceso(ListaPlanes.FirstOrDefault());
							}
						}

						catch (Exception excepcion)
						{
							RegistroLog.EscribirLog(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion);
							throw new ApplicationException(string.Format("Error al guardar el documento {0} Detalle: {1}", documento_obj.Documento, excepcion.Message));
						}

					}
					else
					{
						//doc_nuevo = false;
						List<string> mensajes = new List<string>();
						try
						{
							mensajes.Add(string.Format("El Documento Electrónico {0} con prefijo {1} ya existe en nuestra plataforma", documento_bd.IntNumero, documento_bd.StrPrefijo));

							//ReEnviarCorreoError(ruta_archivo_mail, mensajes);

							RechazarCorreo(mensajes, ruta_archivo_mail, false, emision);
						}
						catch (Exception)
						{
						}


						if (emision == true)
						{
							Ctl_Documento ctl_documento = new Ctl_Documento();

							var Tarea1 = ctl_documento.SondaConsultareventos(false, documento_bd.StrIdSeguridad.ToString());
						}

						//try
						//{
						//	Ctl_RegistroRecepcion _ctrl_registro = new Ctl_RegistroRecepcion();
						//	TblRegistroRecepcion tblregistro = _ctrl_registro.Obtener(Guid.Parse(Path.GetFileNameWithoutExtension(ruta_archivo_mail)));
						//	if (tblregistro != null)
						//	{
						//		tblregistro.IntEstado = 1;
						//		tblregistro.StrObservaciones = JsonConvert.SerializeObject(mensajes);

						//		_ctrl_registro.Actualizar(tblregistro);
						//	}
						//}
						//catch (Exception excepcion)
						//{
						//	string msg = string.Format("Error al procesar correo electrónico: {0} - {1}", Path.GetFileNameWithoutExtension(ruta_archivo_mail), excepcion.Message);
						//	RegistroLog.EscribirLog(excepcion, LibreriaGlobalHGInet.RegistroLog.MensajeCategoria.Sonda, LibreriaGlobalHGInet.RegistroLog.MensajeTipo.Error, LibreriaGlobalHGInet.RegistroLog.MensajeAccion.actualizacion, msg);
						//}

						throw new ArgumentException(string.Format("El Documento Electrónico {0} con prefijo {1} ya existe en nuestra plataforma", documento_bd.IntNumero, documento_bd.StrPrefijo));
					}
					
					if ((doc_nuevo == true || documento_bd.IntEnvioMail == false) && emision == false)
					{
						try
						{   // envía el correo del documento al Adquiriente(Facturador Receptor)
							Ctl_EnvioCorreos email = new Ctl_EnvioCorreos();
							email.NotificacionDocumento(documento_bd, (string.IsNullOrEmpty(documento_obj.DatosObligado.Telefono)) ? "vacio" : documento_obj.DatosObligado.Telefono, facturador_receptor.StrMailRecepcion, string.Empty, Procedencia.Plataforma, string.Empty, ProcesoEstado.EnvioEmailAcuse, string.Empty, false, true);
							try
							{
								Ctl_ProcesosCorreos proceso_correo = new Ctl_ProcesosCorreos();
								TblProcesoCorreo correo_doc = proceso_correo.Obtener(documento_bd.StrIdSeguridad);
								if (correo_doc != null && correo_doc.IntEnvioMail == true)
								{
									documento_bd.IntEnvioMail = true;
									ctl_doc.Actualizar(documento_bd);
								}
							}
							catch (Exception excepcion)
							{
								RegistroLog.EscribirLog(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion, "Error validando el envio del correo al Adquiriente desde interoperabilidad");
							}
							//documento_bd.IntIdEstado = Convert.ToInt16(ProcesoEstado.Finalizacion.GetHashCode());
						}
						catch (Exception excepcion)
						{
							RegistroLog.EscribirLog(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion);
							throw new ApplicationException(string.Format("Error al notificar al facturador receptor la recepcion del documento {0} Detalle: {1}", documento_obj.Documento, excepcion.Message));
						}
					}
					else if (emision == true)
					{
						Ctl_Documento ctl_documento = new Ctl_Documento();

						var Tarea1 = ctl_documento.SondaConsultareventos(false, documento_bd.StrIdSeguridad.ToString());
					}

					respuesta = true;
				}
				else
				{
					RegistroLog.EscribirLog(new ApplicationException("El facturador no tiene habilitado el proceso de interoperabilidad"), MensajeCategoria.Archivos, MensajeTipo.Error, MensajeAccion.lectura, "El facturador no tiene habilitado el proceso de interoperabilidad");

					//Si pasa algo envio notificacion a tic para validar por que no se proceso
					List<string> mensajes = new List<string>();
					mensajes.Add(string.Format("El facturador {0} no tiene habilitado el proceso de interoperabilidad", facturador_receptor.StrIdentificacion));
					mensajes.Add("Por favor comunicarse con nuestra Area de Soporte para guiarlo en este proceso");

					string correo_destino = string.Format("{0};soporte@hgi.com.co", facturador_receptor.StrMailAdmin);

					try
					{
						Ctl_EnvioCorreos email = new Ctl_EnvioCorreos();
						email.EnviaNotificacionAlertaDIAN(facturador_receptor.StrIdentificacion, attach_document.Documento.ToString(), mensajes, 1, false, correo_destino, 2);
					}
					catch (Exception)
					{ }

					RechazarCorreo(mensajes, ruta_archivo_mail, false, emision);

					//try
					//{
					//	Ctl_RegistroRecepcion _ctrl_registro = new Ctl_RegistroRecepcion();
					//	TblRegistroRecepcion tblregistro = _ctrl_registro.Obtener(Guid.Parse(Path.GetFileNameWithoutExtension(ruta_archivo_mail)));
					//	if (tblregistro != null)
					//	{
					//		tblregistro.IntEstado = 1;
					//		tblregistro.StrObservaciones = JsonConvert.SerializeObject(mensajes);

					//		_ctrl_registro.Actualizar(tblregistro);
					//	}
					//}
					//catch (Exception excepcion)
					//{
					//	string msg = string.Format("Error al procesar correo electrónico: {0} - {1}", Path.GetFileNameWithoutExtension(ruta_archivo_mail), excepcion.Message);
					//	RegistroLog.EscribirLog(excepcion, LibreriaGlobalHGInet.RegistroLog.MensajeCategoria.Sonda, LibreriaGlobalHGInet.RegistroLog.MensajeTipo.Error, LibreriaGlobalHGInet.RegistroLog.MensajeAccion.importar, msg);
					//}
				}

			}
			catch (Exception excepcion)
			{
				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Archivos, MensajeTipo.Error, MensajeAccion.lectura, excepcion.Message);

				//Si pasa algo envio notificacion a tic para validar por que no se proceso
				List<string> mensajes = new List<string>();
				mensajes.Add(string.Format("Error al procesar correo, Ruta archivo: {0}",ruta_archivo));
				mensajes.Add(excepcion.Message);
				if (excepcion.InnerException != null && !string.IsNullOrEmpty(excepcion.InnerException.Message))
					mensajes.Add(excepcion.InnerException.Message);
				try
				{
					Ctl_EnvioCorreos email = new Ctl_EnvioCorreos();
					email.EnviaNotificacionAlertaDIAN(attach_document.IdentificacionFacturador, attach_document.Documento.ToString(), mensajes, 3, false, Constantes.EmailCopiaOculta, 2);
				}
				catch (Exception ex)
				{
					mensajes.Add(ex.Message);
				}

				ruta_archivo = string.Format("{0}\\Archivo_errores.json", ruta_archivo);

				// elimina el archivo json de error si existe
				if (Archivo.ValidarExistencia(ruta_archivo))
					Archivo.Borrar(ruta_archivo);

				// almacena el objeto en archivo json
				File.WriteAllText(ruta_archivo, JsonConvert.SerializeObject(mensajes));

				throw new ApplicationException(string.Format("Error al procesar el archivo {0} Detalle: {1}", attach_document.Documento, excepcion.Message));

			}

			return respuesta;

		}

		/// <summary>
		/// Metodo que actualiza tabla de recepcion y envia respuesta al remitente
		/// </summary>
		/// <param name="mensajes">Inconsistencia a notificar</param>
		/// <param name="ruta_archivo_mail">ruta del archivo y Id del registro del correo</param>
		/// <param name="notificar">si envia correo o no de la respuesta</param>
		public static void RechazarCorreo(List<string> mensajes , string ruta_archivo_mail, bool notificar = true, bool emision = false)
		{
			try
			{
				Ctl_RegistroRecepcion _ctrl_registro = new Ctl_RegistroRecepcion();
				TblRegistroRecepcion tblregistro = _ctrl_registro.Obtener(Guid.Parse(Path.GetFileNameWithoutExtension(ruta_archivo_mail)));
				if (tblregistro != null)
				{
					tblregistro.IntEstado = 1;
					tblregistro.StrObservaciones = JsonConvert.SerializeObject(mensajes);

					_ctrl_registro.Actualizar(tblregistro);
				}
			}
			catch (Exception excepcion)
			{
				string msg = string.Format("Error al procesar correo electrónico: {0} - {1}", Path.GetFileNameWithoutExtension(ruta_archivo_mail), excepcion.Message);
				RegistroLog.EscribirLog(excepcion, LibreriaGlobalHGInet.RegistroLog.MensajeCategoria.Sonda, LibreriaGlobalHGInet.RegistroLog.MensajeTipo.Error, LibreriaGlobalHGInet.RegistroLog.MensajeAccion.actualizacion, msg);
			}

			try
			{
				if (notificar == true)
					ReEnviarCorreoError(ruta_archivo_mail, mensajes, emision);
			}
			catch (Exception)
			{
			}
		}

	}
}
