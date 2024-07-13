using HGInetDIANServicios;
using HGInetDIANServicios.DianFactura;
using HGInetFirmaDigital;
using HGInetMiFacturaElectonicaController.Auditorias;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaController.ServiciosDian;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetUBL;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using HGInetMiFacturaElectonicaController.Configuracion;
using LibreriaGlobalHGInet.HgiNet;
using LibreriaGlobalHGInet.RegistroLog;

namespace HGInetMiFacturaElectonicaController.Procesos
{
	public partial class Ctl_Documentos
	{

		/// <summary>
		/// Genera el xml con la información del documento en formato UBL
		/// </summary>        
		/// <param name="documentoBd">información del documento en base de datos</param>
		/// <param name="empresa">información del facturador electrónico en base de datos</param>
		/// <param name="respuesta">datos de respuesta del documento</param>
		/// <param name="documento_result">información del proceso interno del documento</param>
		/// <returns>información adicional de respuesta del documento</returns>
		public static AcuseRecibo EnviarDian(TblDocumentos documentoBd, TblEmpresas empresa, ref DocumentoRespuesta respuesta, ref FacturaE_Documento documento_result, string IdSetDian = "", bool proceso_sonda = false)
		{

			string msg_response = String.Empty;

			MensajeCategoria log_categoria = MensajeCategoria.Conexion;
			MensajeAccion log_accion = MensajeAccion.envio;

			HGInetDIANServicios.DianFactura.AcuseRecibo acuse = new HGInetDIANServicios.DianFactura.AcuseRecibo();
			Ctl_Documento documento_tmp = new Ctl_Documento();
			try
			{

				acuse = Ctl_DocumentoDian.Enviar(documento_result, documentoBd, empresa, ref respuesta, IdSetDian, proceso_sonda);

				if (acuse.Response.Equals(200))
				{
					respuesta.DescripcionProceso = Enumeracion.GetDescription(ProcesoEstado.EnvioZip);
					respuesta.FechaUltimoProceso = Fecha.GetFecha();
					respuesta.IdProceso = ProcesoEstado.EnvioZip.GetHashCode();
					//respuesta.Cufe = string.Empty;
					//respuesta.UrlXmlUbl = string.Empty;
					//respuesta.UrlPdf = string.Empty;
				}
				else if (acuse.Response.Equals(100))
				{
					documentoBd.IntEnvioMail = true;
				}
				else if (acuse.MessagesFieldV2 != null)
				{
					respuesta.FechaUltimoProceso = Fecha.GetFecha();
					if (acuse.MessagesFieldV2.FirstOrDefault().ProcessedMessage.Equals("Documento enviado previamente"))
					{
						//Se pone codigo 201 para identificar que el documento esta en la DIAN pero no se Tiene Radicado y no se haga consulta por este campo a la DIAN
						acuse.Response = 201;
						acuse.Comments = "Documento Electrónico recibido exitosamente";
						respuesta.DescripcionProceso = Enumeracion.GetDescription(ProcesoEstado.EnvioZip);
						respuesta.IdProceso = ProcesoEstado.EnvioZip.GetHashCode();

						respuesta.EstadoDian = new RespuestaDian();
						respuesta.EstadoDian.CodigoRespuesta = "0";
						respuesta.EstadoDian.Descripcion = "Procesado Correctamente";
						respuesta.EstadoDian.EstadoDocumento = EstadoDocumentoDian.Aceptado.GetHashCode();
						respuesta.EstadoDian.FechaConsulta = respuesta.FechaUltimoProceso;

						//Auditoria

						try
						{
							string respuestadian = Newtonsoft.Json.JsonConvert.SerializeObject(respuesta.EstadoDian);
							//valido la respuesta para saber que estado guardar en la Auditoria
							int estado = CategoriaEstado.ValidadoDian.GetHashCode();
							Ctl_DocumentosAudit clase_auditoria = new Ctl_DocumentosAudit();
							clase_auditoria.Crear(new Guid(respuesta.IdDocumento), respuesta.IdPeticion, respuesta.IdentificacionObligado, ProcesoEstado.ConsultaDian, TipoRegistro.Proceso, Procedencia.Plataforma, string.Empty, Enumeracion.GetDescription(ProcesoEstado.ConsultaDian), respuestadian, respuesta.Prefijo, Convert.ToString(respuesta.Documento), estado);
						}
						catch (Exception e) { }

					}
					else if (acuse.Response != 201)
					{
						log_categoria = MensajeCategoria.Servicio;
						log_accion = MensajeAccion.alarma;
						try
						{
							Ctl_Alertas alerta = new Ctl_Alertas();
							alerta.Alertas(empresa.StrIdentificacion, string.Format("{0}{1}", documentoBd.StrPrefijo, documentoBd.IntNumero), acuse.MessagesFieldV2.Select(_X => _X.ProcessedMessage).ToList(), 1, false);

						}
						catch (Exception excepcion)
						{
							Ctl_Log.Guardar(excepcion, log_categoria, MensajeTipo.Error, log_accion);
						}

						msg_response = LibreriaGlobalHGInet.Formato.Coleccion.ConvertListToString(acuse.MessagesFieldV2.Select(_X => _X.ProcessedMessage).ToList(), ";");
						respuesta.IdProceso = ProcesoEstado.PrevalidacionErrorDian.GetHashCode();
						documentoBd.StrCufe = respuesta.Cufe;
						documentoBd.DatFechaActualizaEstado = respuesta.FechaUltimoProceso;
						documentoBd.IntIdEstado = Convert.ToInt16(respuesta.IdProceso);

						documento_tmp.Actualizar(documentoBd);
						throw new ApplicationException(string.Format("Respuesta Dian: {0}", msg_response));
					}
				}
			}
			catch (Exception excepcion)
			{
				if (empresa.IntVersionDian == 1)
				{
					respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en el envío del archivo ZIP con el XML firmado a la DIAN. Detalle: {0}", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);
					documentoBd.IntEnvioMail = true;
				}
				else
				{
					if (documentoBd.IntIdEstado != ProcesoEstado.PrevalidacionErrorDian.GetHashCode())
					{
						respuesta.IdProceso = ProcesoEstado.PrevalidacionErrorDian.GetHashCode();

						documentoBd.StrCufe = respuesta.Cufe;
						documentoBd.DatFechaActualizaEstado = respuesta.FechaUltimoProceso;
						documentoBd.IntIdEstado = Convert.ToInt16(respuesta.IdProceso);

						documento_tmp.Actualizar(documentoBd);
					}
					msg_response = string.Format("{0} -{1}",msg_response,excepcion.Message);
					//respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("No es posible la comunicación con la Plataforma de la DIAN para el envío del archivo ZIP con el XML firmado"), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION);
					respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Se presentó inconsistencias en el envío del archivo ZIP con el XML firmado a la Plataforma de la DIAN: {0}", msg_response), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION);
				}
				Ctl_Log.Guardar(excepcion, log_categoria, MensajeTipo.Error, log_accion);
				//LogExcepcion.Guardar(excepcion);
				return null;
			}

			PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

			// url pública de los archivos
			string url_ppal = string.Format("{0}/{1}/{2}", plataforma_datos.RutaDmsPublica, Constantes.CarpetaFacturaElectronica, empresa.StrIdSeguridad.ToString());

			// url pública del zip
			string url_ppal_zip = string.Format(@"{0}/{1}/{2}.zip", url_ppal, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian, documento_result.NombreZip);

			documentoBd.StrCufe = respuesta.Cufe;
			documentoBd.StrUrlArchivoZip = url_ppal_zip;
			documentoBd.DatFechaActualizaEstado = respuesta.FechaUltimoProceso;
			documentoBd.IntIdEstado = Convert.ToInt16(respuesta.IdProceso);
			if (documentoBd.IntIdEstado != ProcesoEstado.PrevalidacionErrorDian.GetHashCode() && documentoBd.IntIdEstado != ProcesoEstado.PrevalidacionErrorPlataforma.GetHashCode())
			{
				documentoBd.StrCufe = respuesta.Cufe;
			}

			if (empresa.IntVersionDian == 2 && !string.IsNullOrEmpty(IdSetDian) && !string.IsNullOrEmpty(acuse.KeyV2))
			documentoBd.StrIdRadicadoDian = Guid.Parse(acuse.KeyV2);

			documento_tmp.Actualizar(documentoBd);

			//Actualiza la categoria con el nuevo estado
			respuesta.IdEstado = documentoBd.IdCategoriaEstado;
			respuesta.DescripcionEstado = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<CategoriaEstado>(documentoBd.IdCategoriaEstado));

			if (acuse.Response != 201)
			{
				//Auditoria
				try
				{
					Ctl_DocumentosAudit clase_auditoria = new Ctl_DocumentosAudit();
					clase_auditoria.Crear(new Guid(respuesta.IdDocumento), respuesta.IdPeticion, respuesta.IdentificacionObligado, ProcesoEstado.CompresionXml, TipoRegistro.Proceso, Procedencia.Plataforma, string.Empty, "Compresión Archivo Zip", url_ppal_zip, respuesta.Prefijo, Convert.ToString(respuesta.Documento));
				}
				catch (Exception e)
				{
				}

				//Se da una pausa en proceso para que el servicio de la DIAN termine la validacion del documento
				System.Threading.Thread.Sleep(5000);
			}

			return acuse;
		}


		/// <summary>
		/// Consulta estado de documentos en la DIAN
		/// </summary>
		/// <param name="documentoBd">Documento en BD</param>
		/// <param name="empresa">Obligado a facturar</param>
		/// <param name="respuesta">Objeto de respuesta</param>
		/// <returns>Segun la respuesta de la DIAN cambia el estado del documento</returns>
		public static DocumentoRespuesta Consultar(TblDocumentos documentoBd, TblEmpresas empresa, ref DocumentoRespuesta respuesta, string id_validacion_previa = "", int proceso_acuse = 0, bool actualizar_doc = true)
		{

			DateTime fecha_actual = Fecha.GetFecha();
			Ctl_Documento documento_tmp = new Ctl_Documento();


			try
			{
				string IdSoftware = null;
				string PinSoftware = null;
				string clave = null;
				string url_ws_consulta = null;
				string obligado_identificacion = string.Empty;

				TipoDocumento doc_tipo = Enumeracion.GetEnumObjectByValue<TipoDocumento>(documentoBd.IntDocTipo);

				PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

				// ruta física del xml
				string carpeta_xml = string.Format("{0}\\{1}\\{2}", plataforma_datos.RutaDmsFisica, Constantes.CarpetaFacturaElectronica, empresa.StrIdSeguridad.ToString());
				carpeta_xml = string.Format(@"{0}\{1}", carpeta_xml, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEConsultaDian);

				// valida la existencia de la carpeta
				carpeta_xml = Directorio.CrearDirectorio(carpeta_xml);

				// Nombre del archivo Xml 
				string archivo_xml = string.Format(@"{0}.xml", HGInetUBLv2_1.NombramientoArchivo.ObtenerXml(documentoBd.IntNumero.ToString(), documentoBd.StrEmpresaFacturador, doc_tipo, documentoBd.StrPrefijo));

				if (string.IsNullOrEmpty(respuesta.IdentificacionObligado) && (respuesta.DocumentoTipo == TipoDocumento.AcuseRecibo.GetHashCode()))
				{
					doc_tipo = TipoDocumento.AcuseRecibo;
					carpeta_xml = carpeta_xml.Replace("FacturaEConsultaDian", "XmlAcuse");
					archivo_xml = string.Format(@"{0}-{1}-2.xml", HGInetUBLv2_1.NombramientoArchivo.ObtenerXml(documentoBd.IntNumero.ToString(), documentoBd.StrEmpresaFacturador, doc_tipo, documentoBd.StrPrefijo), respuesta.Documento);
				}

				// ruta del xml
				string ruta_xml = string.Format(@"{0}\{1}", carpeta_xml, archivo_xml);

				// elimina el archivo xml si existe
				if (Archivo.ValidarExistencia(ruta_xml))
					Archivo.Borrar(ruta_xml);

				string ruta_certificado = string.Empty;
				CertificadoDigital certificado = null;

				if (empresa.IntVersionDian == 2)
				{
					// obtiene la información de configuración del certificado digital
					certificado = HgiConfiguracion.GetConfiguration().CertificadoDigitalData;

					// obtiene la empresa certificadora
					EnumCertificadoras empresa_certificadora = EnumCertificadoras.Andes;

					if (certificado.Certificadora.Equals("andes"))
						empresa_certificadora = EnumCertificadoras.Andes;
					else if (certificado.Certificadora.Equals("gse"))
						empresa_certificadora = EnumCertificadoras.Gse;

					// información del certificado digital
					ruta_certificado = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), certificado.RutaLocal);

					// obtiene los datos del proveedor tecnológico de la DIAN para Validación Previa
					DianProveedorV2 data_dian = HgiConfiguracion.GetConfiguration().DianProveedorV2;

					IdSoftware = data_dian.IdSoftware;
					PinSoftware = data_dian.Pin;
					//clave = data_dian.ClaveAmbiente;
					url_ws_consulta = data_dian.UrlWSConsultaTransacciones;
					obligado_identificacion = empresa.StrIdentificacion;
				}
				else
				{
					// sobre escribe los datos de la resolución si se encuentra en estado de habilitación
					if (empresa.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
					{
						// obtiene los datos de prueba del proveedor tecnológico de la DIAN
						DianProveedorTest data_dian_habilitacion = HgiConfiguracion.GetConfiguration().DianProveedorTest;

						IdSoftware = data_dian_habilitacion.IdSoftware;
						PinSoftware = data_dian_habilitacion.Pin;
						clave = data_dian_habilitacion.ClaveAmbiente;
						url_ws_consulta = data_dian_habilitacion.UrlWSConsultaTransacciones;
						obligado_identificacion = Constantes.NitResolucionsinPrefijo;
					}
					else
					{
						// obtiene los datos del proveedor tecnológico de la DIAN
						DianProveedor data_dian = HgiConfiguracion.GetConfiguration().DianProveedor;

						IdSoftware = data_dian.IdSoftware;
						PinSoftware = data_dian.Pin;
						clave = data_dian.ClaveAmbiente;
						url_ws_consulta = data_dian.UrlWSConsultaTransacciones;
						obligado_identificacion = empresa.StrIdentificacion;
					}
				}

				ConsultaDocumento resultado_doc = null;

				if (empresa.IntVersionDian == 2)
				{
					string xml_archivo = Path.GetFileNameWithoutExtension(archivo_xml);

					// Consulta del documento con validación previa
					List<HGInetDIANServicios.DianWSValidacionPrevia.DianResponse> resultado = null;
					
					//Depende del ambiente se consulta diferente en la DIAN
					if (empresa.IntHabilitacion < Habilitacion.Produccion.GetHashCode() && !string.IsNullOrEmpty(id_validacion_previa))
					{
						resultado = Ctl_ConsultaTransacciones.Consultar_v2(id_validacion_previa, carpeta_xml, ruta_certificado, certificado.Clave, url_ws_consulta, xml_archivo);
					}
					else
					{
						resultado = Ctl_ConsultaTransacciones.Consultar_v2(String.Empty, carpeta_xml, ruta_certificado, certificado.Clave, url_ws_consulta, xml_archivo, documentoBd.StrCufe);
					}
					
					resultado_doc = Ctl_ConsultaTransacciones.ValidarTransaccionV2(resultado);
				}
				else
				{
					HGInetDIANServicios.DianResultadoTransacciones.DocumentosRecibidos resultado = Ctl_ConsultaTransacciones.Consultar(Guid.NewGuid(), IdSoftware, clave, documentoBd.IntDocTipo, documentoBd.StrPrefijo, documentoBd.IntNumero.ToString(), obligado_identificacion, documentoBd.DatFechaDocumento, documentoBd.StrCufe, url_ws_consulta, ruta_xml);
					resultado_doc = Ctl_ConsultaTransacciones.ValidarTransaccion(resultado);
				}

				// proceso para validar la respuesta de la DIAN
				respuesta = ValidarRespuestaConsulta(resultado_doc, documentoBd, empresa, respuesta, archivo_xml, doc_tipo, actualizar_doc);

				return respuesta;
			}

			catch (Exception excepcion)
			{
				respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en la consulta del estado del documento en la DIAN. Detalle: {0}", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);
				Ctl_Log.Guardar(excepcion, MensajeCategoria.ServicioDian, MensajeTipo.Error, MensajeAccion.actualizacion);
				throw excepcion;
			}
			
		}

		/// <summary>
		/// Proceso para validar la respuesta de la DIAN
		/// </summary>
		/// <param name="resultado_doc"></param>
		/// <param name="documentoBd"></param>
		/// <param name="empresa"></param>
		/// <param name="respuesta"></param>
		/// <param name="archivo_xml"></param>
		/// <returns></returns>
		public static DocumentoRespuesta ValidarRespuestaConsulta(ConsultaDocumento resultado_doc, TblDocumentos documentoBd, TblEmpresas empresa, DocumentoRespuesta respuesta, string archivo_xml, TipoDocumento tipo_doc, bool actualizar_doc = true)
		{

			DateTime fecha_actual = Fecha.GetFecha();
			Ctl_Documento documento_tmp = new Ctl_Documento();


			try
			{
				PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

				// url pública del xml de respuesta de la DIAN 
				string url_ppal_respuesta = string.Format("{0}/{1}/{2}", plataforma_datos.RutaDmsPublica, Constantes.CarpetaFacturaElectronica, empresa.StrIdSeguridad.ToString());

				// se indica la respuesta de la DIAN
				respuesta.EstadoDian = new RespuestaDian();
				respuesta.EstadoDian.CodigoRespuesta = resultado_doc.CodigoEstadoDian;
				respuesta.EstadoDian.Descripcion = resultado_doc.EstadoDianDescripcion;
				respuesta.EstadoDian.EstadoDocumento = resultado_doc.Estado.GetHashCode();
				respuesta.EstadoDian.FechaConsulta = fecha_actual;
				respuesta.EstadoDian.UrlXmlRespuesta = string.Format(@"{0}/{1}/{2}", url_ppal_respuesta, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEConsultaDian, archivo_xml);


				//Auditoria

				try
				{
					string respuestadian = Newtonsoft.Json.JsonConvert.SerializeObject(respuesta.EstadoDian);
					List<string> list_errormsg = null;
					if (!string.IsNullOrEmpty(resultado_doc.Mensaje) && empresa.IntVersionDian == 2)
						list_errormsg = LibreriaGlobalHGInet.Formato.Coleccion.ConvertirLista(resultado_doc.Mensaje, ';');

					//valido la respuesta para saber que estado guardar en la Auditoria y generar notificacion
					int estado = 0;
					if (resultado_doc.Estado == EstadoDocumentoDian.Aceptado || resultado_doc.Estado == EstadoDocumentoDian.Rechazado)
					{
						// aceptado
						bool resultado = true;
						estado = CategoriaEstado.ValidadoDian.GetHashCode();

						// rechazado
						if (resultado_doc.Estado == EstadoDocumentoDian.Rechazado)
						{
							resultado = false;
							estado = CategoriaEstado.FallidoDian.GetHashCode();
						}

						//Se agrega validacion para que no notifique si salen estas dos reglas, son de la DIAN y no la arreglan aun
						bool alert = true;
						if ((list_errormsg.Count == 1) && ((resultado_doc.Mensaje.Contains("FAB10b")) || (resultado_doc.Mensaje.Contains("FAJ39")) || (resultado_doc.Mensaje.Contains("FAK61")) || (resultado_doc.Mensaje.Contains("RUT01"))))
						{
							alert = false;
						}

						if (tipo_doc == TipoDocumento.AcuseRecibo && resultado_doc.Mensaje.Contains("EL CUFE o Factura consultada no tiene a la fecha eventos asociados"))
							alert = false;

						//Si la respuesta es de V2 y llego con errores alerto para validar
						if ((list_errormsg != null) && (alert == true)) 
						{

							MensajeCategoria log_categoria = MensajeCategoria.Servicio;
							MensajeAccion log_accion = MensajeAccion.alarma;
							try
							{
								Ctl_Alertas alerta = new Ctl_Alertas();
								alerta.Alertas(empresa.StrIdentificacion, string.Format("{0}{1}", documentoBd.StrPrefijo, documentoBd.IntNumero), list_errormsg, 2, resultado);
							}
							catch (Exception excepcion)
							{
								Ctl_Log.Guardar(excepcion, log_categoria, MensajeTipo.Error, log_accion);
							}

						}

					}
					else
					{
						estado = Ctl_Documento.ObtenerCategoria(documentoBd.IntIdEstado);
					}

					//Se agrega validacion por si se esta quedando con este estado no presente inconsistencias
					if (documentoBd.IntIdEstado == ProcesoEstado.CompresionXml.GetHashCode() && estado == CategoriaEstado.ValidadoDian.GetHashCode())
					{
						try
						{
							documentoBd.IntIdEstado = (short)ProcesoEstado.EnvioZip.GetHashCode();
							Ctl_Documento ct_doc = new Ctl_Documento();
							ct_doc.Actualizar(documentoBd);
						}
						catch (Exception excepcion)
						{
							Ctl_Log.Guardar(excepcion, MensajeCategoria.ServicioDian, MensajeTipo.Error, MensajeAccion.actualizacion, string.Format("No se puedo actualizar estado"));
						}
					}

					Ctl_DocumentosAudit clase_auditoria = new Ctl_DocumentosAudit();
					clase_auditoria.Crear(new Guid(respuesta.IdDocumento), respuesta.IdPeticion, respuesta.IdentificacionObligado, Enumeracion.GetEnumObjectByValue<ProcesoEstado>(documentoBd.IntIdEstado) , TipoRegistro.Proceso, Procedencia.Plataforma, string.Empty, Enumeracion.GetDescription(ProcesoEstado.ConsultaDian), respuestadian, respuesta.Prefijo, Convert.ToString(respuesta.Documento), estado);
				}
				catch (Exception e) { }


				string detalle_dian = string.Empty;

				if (resultado_doc.Estado == EstadoDocumentoDian.Rechazado)
				{
					fecha_actual = Fecha.GetFecha();

					respuesta.FechaUltimoProceso = fecha_actual;
					respuesta.Error = new LibreriaGlobalHGInet.Error.Error();
					respuesta.Error.Codigo = LibreriaGlobalHGInet.Error.CodigoError.VALIDACION;
					respuesta.Error.Fecha = fecha_actual;

					if (empresa.IntVersionDian == 1)
					{
						respuesta.DescripcionProceso = Enumeracion.GetDescription(ProcesoEstado.FinalizacionErrorDian);
						respuesta.IdProceso = ProcesoEstado.FinalizacionErrorDian.GetHashCode();
						respuesta.ProcesoFinalizado = 1;
						respuesta.Error.Mensaje = string.Format("Documento rechazado DIAN: {0} - Cod. {1} ", resultado_doc.EstadoDianDescripcion, resultado_doc.CodigoEstadoDian);
					}
					else
					{
						respuesta.DescripcionProceso = Enumeracion.GetDescription(ProcesoEstado.PrevalidacionErrorDian);
						respuesta.IdProceso = ProcesoEstado.PrevalidacionErrorDian.GetHashCode();
						respuesta.Error.Mensaje = string.Format("Documento rechazado DIAN: {0} - Validar las siguientes inconsistencias de la DIAN: {1} ", resultado_doc.EstadoDianDescripcion, resultado_doc.Mensaje);
						//***se quita informacion para la respuesta
						respuesta.Cufe = string.Empty;
						respuesta.UrlXmlUbl = string.Empty;
						respuesta.UrlPdf = string.Empty;
					}


					//cuando es de recepcion no se debe actualizar el documento por que aun no existe en BD.
					if (tipo_doc != TipoDocumento.AcuseRecibo && actualizar_doc == true)
					{
						//Actualiza Documento en Base de Datos
						documentoBd.DatFechaActualizaEstado = respuesta.FechaUltimoProceso;
						documentoBd.IntIdEstado = Convert.ToInt16(respuesta.IdProceso);

						documento_tmp.Actualizar(documentoBd);
							
						//Actualiza la categoria con el nuevo estado
						respuesta.IdEstado = documentoBd.IdCategoriaEstado;
						respuesta.DescripcionEstado = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<CategoriaEstado>(documentoBd.IdCategoriaEstado));
					}

				}
				return respuesta;
			}

			catch (Exception excepcion)
			{
				respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en la consulta del estado del documento en la DIAN. Detalle: {0}", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);
				Ctl_Log.Guardar(excepcion, MensajeCategoria.ServicioDian, MensajeTipo.Error, MensajeAccion.actualizacion);
				throw excepcion;
			}



		}


		/// <summary>
		/// Agrega los campos de la Dian correspondientes al Proveedor Tecnológico
		/// </summary>
		/// <param name="objeto_des"></param>
		/// <param name="tipo"></param>
		/// <param name="facturador"></param>
		/// <returns></returns>
		public static object AgregarCamposDian(object objeto_des, TipoDocumento tipo, TblEmpresas facturador)
		{

			string IdSoftware = null;
			string PinSoftware = null;
			string archivo_xml = string.Empty;

			XmlSerializerNamespaces namespaces_xml = NamespacesXML.Obtener();

			// sobre escribe los datos de la resolución si se encuentra en estado de habilitación
			if (facturador.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
			{
				// obtiene los datos de prueba del proveedor tecnológico de la DIAN
				DianProveedorTest data_dian_habilitacion = HgiConfiguracion.GetConfiguration().DianProveedorTest;

				IdSoftware = data_dian_habilitacion.IdSoftware;
				PinSoftware = data_dian_habilitacion.Pin;
			}
			else
			{
				// obtiene los datos del proveedor tecnológico de la DIAN
				DianProveedor data_dian = HgiConfiguracion.GetConfiguration().DianProveedor;

				IdSoftware = data_dian.IdSoftware;
				PinSoftware = data_dian.Pin;
			}

			string software_security_code = string.Format("{0}{1}", IdSoftware, PinSoftware);
			string software_security_code_encriptado = Encriptar.Encriptar_SHA384(software_security_code);//Encriptación en SHA384 del string que contiene Identificador y el Pin del software

			dynamic objeto_valido = null;


			if (tipo == TipoDocumento.Factura)
			{
				objeto_valido = (InvoiceType)objeto_des;

			}
			else if (tipo == TipoDocumento.NotaDebito)
			{
				objeto_valido = (DebitNoteType)objeto_des;

			}
			else if (tipo == TipoDocumento.NotaCredito)
			{
				objeto_valido = (CreditNoteType)objeto_des;

			}

			//ingreso a los tag para llenar la informacion del Proveedor tecnologico segun el tipo de documento

			var archivo = (InvoiceType)objeto_des;

			foreach (XmlNode item in archivo.UBLExtensions[0].ExtensionContent.ChildNodes)
			{
				if (item.LocalName.Equals("SoftwareProvider"))
				{
					if (item.LocalName.Equals("SoftwareProvider"))
					{
						foreach (XmlNode item_child in item.ChildNodes)
						{
							if (item_child.LocalName.Equals("ProviderID"))
							{
								item_child.ChildNodes[0].Value = facturador.StrIdentificacion;
							}
							if (item_child.LocalName.Equals("SoftwareID"))
							{
								item_child.ChildNodes[0].Value = IdSoftware;
							}
						}
					}

				}
				if (item.LocalName.Equals("SoftwareSecurityCode"))
				{
					item.ChildNodes[0].Value = software_security_code_encriptado;

				}
			}


			return objeto_valido;

		}


		/// <summary>
		/// Llena el Ubl con los campos del Proveedor tecnologico que son requeridos de la DIAN
		/// </summary>
		/// <param name="documento_result"></param>
		/// <param name="facturador"></param>
		public static void CamposDian(ref FacturaE_Documento documento_result, TblEmpresas facturador, ref DocumentoRespuesta respuesta)
		{
			string IdSoftware = null;
			string PinSoftware = null;
			string archivo_xml = string.Empty;

			try
			{


				XmlSerializerNamespaces namespaces_xml = NamespacesXML.Obtener();

				// sobre escribe los datos de la resolución si se encuentra en estado de habilitación
				if (facturador.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
				{
					// obtiene los datos de prueba del proveedor tecnológico de la DIAN
					DianProveedorTest data_dian_habilitacion = HgiConfiguracion.GetConfiguration().DianProveedorTest;

					IdSoftware = data_dian_habilitacion.IdSoftware;
					PinSoftware = data_dian_habilitacion.Pin;
				}
				else
				{
					// obtiene los datos del proveedor tecnológico de la DIAN
					DianProveedor data_dian = HgiConfiguracion.GetConfiguration().DianProveedor;

					IdSoftware = data_dian.IdSoftware;
					PinSoftware = data_dian.Pin;
				}

				string software_security_code = string.Format("{0}{1}", IdSoftware, PinSoftware);
				string software_security_code_encriptado = Encriptar.Encriptar_SHA384(software_security_code);//Encriptación en SHA384 del string que contiene Identificador y el Pin del software

				archivo_xml = string.Format(@"{0}{1}.xml", documento_result.RutaArchivosProceso, documento_result.NombreXml);

				try
				{
					// Un FileStream es necesario para leer un XML document.
					FileStream fs = new FileStream(archivo_xml, FileMode.Open);
					// convierte el objeto de acuerdo con el tipo de documento
					XmlSerializer xml_ser = null;

					//ingreso a los tag para llenar la informacion del Proveedor tecnologico segun el tipo de documento
					if (documento_result.DocumentoTipo == TipoDocumento.Factura)
					{
						xml_ser = new XmlSerializer(typeof(InvoiceType));

						InvoiceType archivo = (InvoiceType)xml_ser.Deserialize(fs);

						foreach (XmlNode item in archivo.UBLExtensions[0].ExtensionContent.ChildNodes)
						{
							if (item.LocalName.Equals("SoftwareProvider"))
							{
								if (item.LocalName.Equals("SoftwareProvider"))
								{
									foreach (XmlNode item_child in item.ChildNodes)
									{
										if (item_child.LocalName.Equals("ProviderID"))
										{
											item_child.ChildNodes[0].Value = facturador.StrIdentificacion;
										}
										if (item_child.LocalName.Equals("SoftwareID"))
										{
											item_child.ChildNodes[0].Value = IdSoftware;
										}
									}
								}

							}
							if (item.LocalName.Equals("SoftwareSecurityCode"))
							{
								item.ChildNodes[0].Value = software_security_code_encriptado;

							}
						}
						// convierte los datos del objeto en texto XML 
						StringBuilder texto_xml = Xml.Convertir<InvoiceType>(archivo, namespaces_xml);
						documento_result.DocumentoXml = texto_xml;
					}

					else if (documento_result.DocumentoTipo == TipoDocumento.NotaCredito)
					{

						xml_ser = new XmlSerializer(typeof(CreditNoteType));

						CreditNoteType archivo = (CreditNoteType)xml_ser.Deserialize(fs);

						foreach (XmlNode item in archivo.UBLExtensions[0].ExtensionContent.ChildNodes)
						{
							if (item.LocalName.Equals("SoftwareProvider"))
							{
								if (item.LocalName.Equals("SoftwareProvider"))
								{
									foreach (XmlNode item_child in item.ChildNodes)
									{
										if (item_child.LocalName.Equals("ProviderID"))
										{
											item_child.ChildNodes[0].Value = facturador.StrIdentificacion;
										}
										if (item_child.LocalName.Equals("SoftwareID"))
										{
											item_child.ChildNodes[0].Value = IdSoftware;
										}
									}
								}

							}
							if (item.LocalName.Equals("SoftwareSecurityCode"))
							{
								item.ChildNodes[0].Value = software_security_code_encriptado;

							}
						}
						// convierte los datos del objeto en texto XML 
						StringBuilder texto_xml = Xml.Convertir<CreditNoteType>(archivo, namespaces_xml);
						documento_result.DocumentoXml = texto_xml;

					}
					else if (documento_result.DocumentoTipo == TipoDocumento.NotaDebito)
					{
						xml_ser = new XmlSerializer(typeof(DebitNoteType));

						DebitNoteType archivo = (DebitNoteType)xml_ser.Deserialize(fs);

						foreach (XmlNode item in archivo.UBLExtensions[0].ExtensionContent.ChildNodes)
						{
							if (item.LocalName.Equals("SoftwareProvider"))
							{
								if (item.LocalName.Equals("SoftwareProvider"))
								{
									foreach (XmlNode item_child in item.ChildNodes)
									{
										if (item_child.LocalName.Equals("ProviderID"))
										{
											item_child.ChildNodes[0].Value = facturador.StrIdentificacion;
										}
										if (item_child.LocalName.Equals("SoftwareID"))
										{
											item_child.ChildNodes[0].Value = IdSoftware;
										}
									}
								}

							}
							if (item.LocalName.Equals("SoftwareSecurityCode"))
							{
								item.ChildNodes[0].Value = software_security_code_encriptado;

							}
						}
						// convierte los datos del objeto en texto XML 
						StringBuilder texto_xml = Xml.Convertir<DebitNoteType>(archivo, namespaces_xml);
						documento_result.DocumentoXml = texto_xml;

					}
					fs.Close();
				}
				catch (Exception excepcion)
				{
					respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en la serializacion del documento. Detalle: {0} ", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);
					Ctl_Log.Guardar(excepcion, MensajeCategoria.ServicioDian, MensajeTipo.Error, MensajeAccion.actualizacion);
					throw excepcion;
				}
				try
				{
					PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

					// ruta física del xml
					string carpeta_xml = string.Format("{0}\\{1}\\{2}", plataforma_datos.RutaDmsFisica, Constantes.CarpetaFacturaElectronica, facturador.StrIdSeguridad.ToString());
					carpeta_xml = string.Format(@"{0}\{1}", carpeta_xml, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaXmlFacturaE);

					// valida la existencia de la carpeta
					carpeta_xml = Directorio.CrearDirectorio(carpeta_xml);

					// ruta del xml
					string nombre_archivo_xml = string.Format(@"{0}.xml", documento_result.NombreXml);

					// ruta del xml
					string ruta_xml = string.Format(@"{0}\{1}", carpeta_xml, nombre_archivo_xml);

					// mueve el archivo xml recibido
					string archivo_xml_recibido = string.Format(@"{0}recepcion_ws\{1}.xml", documento_result.RutaArchivosProceso, documento_result.NombreXml);
					if (Archivo.CopiarArchivo(ruta_xml, archivo_xml_recibido))
					{
						Archivo.Borrar(ruta_xml);

						// almacena el archivo xml firmado
						string carpeta_xml_firmado = string.Format("{0}\\{1}\\{2}", plataforma_datos.RutaDmsFisica, Constantes.CarpetaFacturaElectronica, facturador.StrIdSeguridad.ToString());
						carpeta_xml_firmado = string.Format(@"{0}\{1}", carpeta_xml_firmado, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian);

						string ruta_save = Xml.Guardar(documento_result.DocumentoXml, carpeta_xml_firmado, nombre_archivo_xml);
					}
				}
				catch (Exception excepcion)
				{
					respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error guardando archivo fisico con cambios en el documento. Detalle: {0} ", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);
					Ctl_Log.Guardar(excepcion, MensajeCategoria.ServicioDian, MensajeTipo.Error, MensajeAccion.actualizacion);
					throw excepcion;
				}

			}
			catch (Exception excepcion)
			{
				respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error agregando datos del Proveedor Tecnologico en el documento enviado. Detalle: {0}", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);
				Ctl_Log.Guardar(excepcion, MensajeCategoria.ServicioDian, MensajeTipo.Error, MensajeAccion.actualizacion);
				throw excepcion;
			}

		}


		/// <summary>
		/// Consulta eventos de un documento en la DIAN
		/// </summary>
		/// <param name="documentoBd">Documento en BD</param>
		/// <param name="empresa">Obligado a facturar</param>
		/// <param name="respuesta">Objeto de respuesta</param>
		/// <returns>Segun la respuesta de la DIAN cambia el estado del documento</returns>
		public static DocumentoRespuesta ConsultarEventos(TblDocumentos documentoBd, TblEmpresas empresa, ref DocumentoRespuesta respuesta)
		{

			DateTime fecha_actual = Fecha.GetFecha();
			Ctl_Documento documento_tmp = new Ctl_Documento();


			try
			{
				//string IdSoftware = null;
				//string PinSoftware = null;
				string url_ws_consulta = null;

				PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

				// ruta física del xml
				string carpeta_xml = string.Format("{0}\\{1}\\{2}", plataforma_datos.RutaDmsFisica, Constantes.CarpetaFacturaElectronica, empresa.StrIdSeguridad.ToString());
				carpeta_xml = string.Format(@"{0}\{1}", carpeta_xml, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEConsultaDian);
				carpeta_xml = carpeta_xml.Replace("FacturaEConsultaDian", "XmlAcuse");

				// valida la existencia de la carpeta
				carpeta_xml = Directorio.CrearDirectorio(carpeta_xml);

				// Nombre del archivo Xml 
				string archivo_xml = string.Format(@"{0}-2.xml", HGInetUBLv2_1.NombramientoArchivo.ObtenerXml(documentoBd.IntNumero.ToString(), documentoBd.StrEmpresaFacturador, TipoDocumento.AcuseRecibo, documentoBd.StrPrefijo));

				// ruta del xml
				string ruta_xml = string.Format(@"{0}\{1}", carpeta_xml, archivo_xml);

				// elimina el archivo xml si existe
				if (Archivo.ValidarExistencia(ruta_xml))
					Archivo.Borrar(ruta_xml);

				string ruta_certificado = string.Empty;

				// obtiene la información de configuración del certificado digital
				CertificadoDigital certificado = HgiConfiguracion.GetConfiguration().CertificadoDigitalData;

				// obtiene la empresa certificadora
				EnumCertificadoras empresa_certificadora = EnumCertificadoras.Andes;

				if (certificado.Certificadora.Equals("andes"))
					empresa_certificadora = EnumCertificadoras.Andes;
				else if (certificado.Certificadora.Equals("gse"))
					empresa_certificadora = EnumCertificadoras.Gse;

				// información del certificado digital
				ruta_certificado = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), certificado.RutaLocal);

				// obtiene los datos del proveedor tecnológico de la DIAN para Validación Previa
				DianProveedorV2 data_dian = HgiConfiguracion.GetConfiguration().DianProveedorV2;

				url_ws_consulta = data_dian.UrlWSConsultaTransacciones;

				ConsultaDocumento resultado_doc = null;

				string xml_archivo = Path.GetFileNameWithoutExtension(archivo_xml);

				// Consulta del documento con validación previa
				List<HGInetDIANServicios.DianWSValidacionPrevia.DianResponse> resultado = Ctl_ConsultaTransacciones.Consultar_v2(String.Empty, carpeta_xml, ruta_certificado, certificado.Clave, url_ws_consulta, xml_archivo, documentoBd.StrCufe, true);

				resultado_doc = Ctl_ConsultaTransacciones.ValidarTransaccionV2(resultado);
				
				// proceso para validar la respuesta de la DIAN
				respuesta = ValidarRespuestaConsulta(resultado_doc, documentoBd, empresa, respuesta, archivo_xml, TipoDocumento.AcuseRecibo);

				return respuesta;
			}

			catch (Exception excepcion)
			{
				respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en la consulta del estado del documento en la DIAN. Detalle: {0}", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);
				Ctl_Log.Guardar(excepcion, MensajeCategoria.ServicioDian, MensajeTipo.Error, MensajeAccion.actualizacion);
				throw excepcion;
			}

		}


	}
}
