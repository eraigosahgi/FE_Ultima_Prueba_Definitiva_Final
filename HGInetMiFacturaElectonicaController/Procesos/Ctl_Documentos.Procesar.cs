using HGInetDIANServicios;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace HGInetMiFacturaElectonicaController.Procesos
{
	public partial class Ctl_Documentos
	{
		/// <summary>
		/// Continúa el procesamiento de varios documentos registrados en la base de datos de acuerdo con el estado actual del mismo
		/// </summary>
		/// <param name="documentos">datos de los documentos en base de datos</param>
		/// <returns>datos de resultado para los documentos</returns>
		public static List<DocumentoRespuesta> Procesar(List<TblDocumentos> documentos, bool consulta_documento = true)
		{
			List<DocumentoRespuesta> documentos_respuestas = new List<DocumentoRespuesta>();

			foreach (TblDocumentos documento in documentos)
			{
				DocumentoRespuesta item_respuesta = new DocumentoRespuesta();

				// obtiene el proceso actual del documento
				ProcesoEstado proceso_actual = Enumeracion.ParseToEnum<ProcesoEstado>((int)documento.IntIdEstado);

				try
				{
					if (documento.IntVersionDian == 1)
					{
						// procesa el documento en V1
						item_respuesta = Procesar(documento, consulta_documento);
					}
					else
					{
						// procesa el documento en V2
						item_respuesta = ProcesarV2(documento, consulta_documento);
					}
				}
				catch (Exception excepcion)
				{
					item_respuesta = new DocumentoRespuesta()
					{
						Aceptacion = documento.IntAdquirienteRecibo,
						CodigoRegistro = documento.StrObligadoIdRegistro,
						Cufe = documento.StrCufe,
						DescripcionProceso = Enumeracion.GetDescription(proceso_actual),
						DocumentoTipo = documento.IntDocTipo,
						Documento = documento.IntNumero,
						Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error al procesar el documento. Detalle: {0} ", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.ERROR_NO_CONTROLADO, excepcion.InnerException),
						FechaRecepcion = documento.DatFechaIngreso,
						FechaUltimoProceso = documento.DatFechaActualizaEstado,
						IdDocumento = documento.StrIdSeguridad.ToString(),
						Identificacion = documento.StrEmpresaFacturador,
						IdProceso = proceso_actual.GetHashCode(),
						MotivoRechazo = documento.StrAdquirienteMvoRechazo,
						NumeroResolucion = documento.StrNumResolucion,
						Prefijo = documento.StrPrefijo,
						ProcesoFinalizado = (proceso_actual == ProcesoEstado.Finalizacion || proceso_actual == ProcesoEstado.FinalizacionErrorDian) ? (1) : 0,
						UrlPdf = documento.StrUrlArchivoPdf,
						UrlXmlUbl = documento.StrUrlArchivoUbl
					};
				}

				documentos_respuestas.Add(item_respuesta);
			}

			return documentos_respuestas;

		}

        /// <summary>
        /// Continúa el procesamiento de un documento registrado en la base de datos de acuerdo con el estado actual del mismo
        /// </summary>
        /// <param name="documento">datos del documento en base de datos</param>
        /// <param name="consulta_documento">indica si antes de enviar el documento a la DIAN se debe consultar</param>
        /// <returns>datos de resultado para el documento</returns>
        public static DocumentoRespuesta Procesar(TblDocumentos documento, bool consulta_documento)
		{

			// representación de datos en objeto
			var documento_obj = (dynamic)null;

			// facturador electrónico del documento
			TblEmpresas empresa = new TblEmpresas();

			// información del procesamiento del archivo
			FacturaE_Documento documento_result = new FacturaE_Documento();

			// valida que la empresa se encuentre como facturador electrónico
			if (documento.TblEmpresasFacturador.IntHabilitacion == 0)
				throw new ArgumentException(string.Format("No se encuentra habilitado como Facturador Electrónico - {0} {1}", documento.StrEmpresaFacturador, documento.TblEmpresasFacturador.StrRazonSocial));

			// obtiene el proceso actual del documento
			ProcesoEstado proceso_actual = Enumeracion.ParseToEnum<ProcesoEstado>((int)documento.IntIdEstado);

			// valida que el proceso del documento cuente con el XML en UBL físicamente
			if (proceso_actual.GetHashCode() < ProcesoEstado.AlmacenaXML.GetHashCode())
				throw new ArgumentException(string.Format("No se puede procesar el documento {0} en estado {1} del Facturador Electrónico - {2} {3}", documento.StrIdSeguridad, Enumeracion.GetDescription(proceso_actual), documento.StrEmpresaFacturador, documento.TblEmpresasFacturador.StrRazonSocial));

			// obtiene el tipo de documento
			TipoDocumento tipo_documento = Enumeracion.ParseToEnum<TipoDocumento>(documento.IntDocTipo);

			// genera el estado actual de respuesta del documento
			DocumentoRespuesta respuesta = new DocumentoRespuesta()
			{
				Aceptacion = documento.IntAdquirienteRecibo,
				DescripcionAceptacion = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<AdquirienteRecibo>(documento.IntAdquirienteRecibo)),
				CodigoRegistro = documento.StrObligadoIdRegistro,
				Cufe = documento.StrCufe,
				DescripcionProceso = Enumeracion.GetDescription(proceso_actual),
				DocumentoTipo = documento.IntDocTipo,
				Documento = documento.IntNumero,
				Error = null,
				FechaRecepcion = documento.DatFechaIngreso,
				FechaUltimoProceso = documento.DatFechaActualizaEstado,
				IdDocumento = documento.StrIdSeguridad.ToString(),
				Identificacion = documento.StrEmpresaAdquiriente,
				IdProceso = proceso_actual.GetHashCode(),
				MotivoRechazo = documento.StrAdquirienteMvoRechazo,
				NumeroResolucion = documento.StrNumResolucion,
				Prefijo = documento.StrPrefijo,
				ProcesoFinalizado = (proceso_actual == ProcesoEstado.Finalizacion || proceso_actual == ProcesoEstado.FinalizacionErrorDian) ? (1) : 0,
				UrlPdf = documento.StrUrlArchivoPdf,
				UrlXmlUbl = documento.StrUrlArchivoUbl,
                IdPlan = Guid.Parse(documento.StrIdPlanTransaccion.ToString()),
				IdentificacionObligado = documento.StrEmpresaFacturador
			};

			if (respuesta.ProcesoFinalizado == 1)
				return respuesta;

			DateTime fecha_actual = Fecha.GetFecha();

			try
			{


				// lee el archivo XML en UBL desde la ruta pública
				//XmlTextReader xml_reader = new XmlTextReader(documento.StrUrlArchivoUbl);

				// lee el archivo XML en UBL desde la ruta pública
				string contenido_xml = Archivo.ObtenerContenido(documento.StrUrlArchivoUbl);

				// valida el contenido del archivo
				if (string.IsNullOrWhiteSpace(contenido_xml))
					throw new ArgumentException("El archivo XML UBL se encuentra vacío.");

				// convierte el contenido de texto a xml
				XmlReader xml_reader = XmlReader.Create(new StringReader(contenido_xml));

				// convierte el objeto de acuerdo con el tipo de documento
				XmlSerializer serializacion = null;

				if (tipo_documento == TipoDocumento.Factura)
				{
					serializacion = new XmlSerializer(typeof(HGInetUBL.InvoiceType));

					HGInetUBL.InvoiceType conversion = (HGInetUBL.InvoiceType)serializacion.Deserialize(xml_reader);

					documento_obj = HGInetUBL.FacturaXML.Convertir(conversion, documento);
					documento.StrCufe = documento_obj.Cufe;
				}
				else if (tipo_documento == TipoDocumento.NotaCredito)
				{
					serializacion = new XmlSerializer(typeof(HGInetUBL.CreditNoteType));

					HGInetUBL.CreditNoteType conversion = (HGInetUBL.CreditNoteType)serializacion.Deserialize(xml_reader);

					documento_obj = HGInetUBL.NotaCreditoXML.Convertir(conversion, documento);
					documento.StrCufe = documento_obj.Cufe;
				}
				else if (tipo_documento == TipoDocumento.NotaDebito)
				{
					serializacion = new XmlSerializer(typeof(HGInetUBL.DebitNoteType));

					HGInetUBL.DebitNoteType conversion = (HGInetUBL.DebitNoteType)serializacion.Deserialize(xml_reader);

					documento_obj = HGInetUBL.NotaDebitoXML.Convertir(conversion, documento);
					documento.StrCufe = documento_obj.Cufe;
				}

				// convierte los datos del objeto en texto XML 
				StringBuilder txt_xml = new StringBuilder();
				txt_xml.Append(contenido_xml);

				// cerrar la lectura del archivo xml
				xml_reader.Close();

				// valida la conversión del objeto
				if (documento_obj == null)
					throw new ArgumentException("No se recibieron datos para realizar el proceso");

				// resolución asociada al documento
				TblEmpresasResoluciones resolucion = documento.TblEmpresasResoluciones;

				// facturador electrónico del documento
				empresa = documento.TblEmpresasFacturador;

				//Se pone la version que tiene el documento a la empresa para que los demas procesos lo haga por esta version
				empresa.IntVersionDian = documento.IntVersionDian;

				PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

				// ruta física del xml
				string carpeta_xml = string.Format("{0}\\{1}\\{2}", plataforma_datos.RutaDmsFisica, Constantes.CarpetaFacturaElectronica, empresa.StrIdSeguridad.ToString());
				carpeta_xml = string.Format(@"{0}\{1}", carpeta_xml, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaXmlFacturaE);

				// carpeta del zip
				string carpeta_zip = string.Format("{0}\\{1}\\{2}", plataforma_datos.RutaDmsFisica, Constantes.CarpetaFacturaElectronica, empresa.StrIdSeguridad.ToString());
				carpeta_zip = string.Format(@"{0}\{1}", carpeta_zip, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian);

				// información del procesamiento del archivo
				documento_result = new FacturaE_Documento()
				{
					IdSeguridadPeticion = Guid.NewGuid(),
					IdSeguridadDocumento = documento.StrIdSeguridad,
					IdSeguridadTercero = empresa.StrIdSeguridad,
					DocumentoTipo = tipo_documento,
					CUFE = documento.StrCufe,
					Documento = documento_obj,
					DocumentoXml = txt_xml,
					RutaArchivosProceso = carpeta_xml,
					RutaArchivosEnvio = carpeta_zip
				};

				// genera el nombre del archivo XML y PDF
				documento_result.NombreXml = HGInetUBL.NombramientoArchivo.ObtenerXml(documento_obj.Documento.ToString(), documento_obj.DatosObligado.Identificacion, tipo_documento, documento_obj.Prefijo);
				documento_result.NombrePdf = documento_result.NombreXml;

				// genera el nombre del archivo ZIP
				documento_result.NombreZip = HGInetUBL.NombramientoArchivo.ObtenerZip(documento_obj.Documento.ToString(), documento_obj.DatosObligado.Identificacion, tipo_documento, documento_obj.Prefijo);

				// firma el xml (valida si no ha realizado el envío a la DIAN vuelve a firmar)
				if (respuesta.IdProceso < ProcesoEstado.FirmaXml.GetHashCode())
				{
					respuesta = UblFirmar(documento, ref respuesta, ref documento_result);
					ValidarRespuesta(respuesta,respuesta.UrlXmlUbl);
				}

				//// comprime el archivo xml firmado
				if (respuesta.IdProceso < ProcesoEstado.CompresionXml.GetHashCode())
				{
					respuesta = UblComprimir(documento, ref respuesta, ref documento_result);
					ValidarRespuesta(respuesta, respuesta.UrlXmlUbl);
				}

				// envía el archivo zip con el xml firmado a la DIAN
				if (respuesta.IdProceso < ProcesoEstado.EnvioZip.GetHashCode())
				{
					//valida si debe consultar el estado del documento en la DIAN
					if (consulta_documento)
					{
						respuesta = Consultar(documento, empresa, ref respuesta);

						//Si no hay respuesta de la DIAN del documento enviado se procede a enviar de nuevo
						if (respuesta.EstadoDian.CodigoRespuesta == ValidacionRespuestaDian.NoRecibido.ToString())
						{
							HGInetDIANServicios.DianFactura.AcuseRecibo acuse = EnviarDian(documento, empresa, ref respuesta, ref documento_result);
							//Se valida si esta es la respuesta por que es un error de la DIAN pero se debe enviar el correo al adquiriente
							if (acuse.Response.Equals(100))
							{
								respuesta = Envio(documento_obj, documento, empresa, ref respuesta, ref documento_result);
								Ctl_Documento documento_tmp = new Ctl_Documento();
								documento_tmp.Actualizar(documento);
							}

							ValidarRespuesta(respuesta, (acuse != null) ? string.Format("{0} - {1}", acuse.Response, acuse.Comments) : "");
						}
						else if (respuesta.EstadoDian.EstadoDocumento != EstadoDocumentoDian.Pendiente.GetHashCode())
						{
							respuesta.IdProceso = ProcesoEstado.EnvioZip.GetHashCode();
							//Actualiza la respuesta
							respuesta.DescripcionProceso = Enumeracion.GetDescription(ProcesoEstado.EnvioZip);
							respuesta.FechaUltimoProceso = Fecha.GetFecha();

							//Actualiza Documento en Base de Datos
							documento.DatFechaActualizaEstado = respuesta.FechaUltimoProceso;
							documento.IntIdEstado = Convert.ToInt16(respuesta.IdProceso);

							Ctl_Documento documento_tmp = new Ctl_Documento();
							documento_tmp.Actualizar(documento);

							//Actualiza la categoria con el nuevo estado
							respuesta.IdEstado = documento.IdCategoriaEstado;
							respuesta.DescripcionEstado = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<CategoriaEstado>(documento.IdCategoriaEstado));
						}
					}
					else
					{
						HGInetDIANServicios.DianFactura.AcuseRecibo acuse = EnviarDian(documento, empresa, ref respuesta, ref documento_result);
						//Se valida si esta es la respuesta por que es un error de la DIAN pero se debe enviar el correo al adquiriente  
						if (acuse.Response.Equals(100))
						{
							respuesta = Envio(documento_obj, documento, empresa, ref respuesta, ref documento_result);
							Ctl_Documento documento_tmp = new Ctl_Documento();
							documento_tmp.Actualizar(documento);
						}
						ValidarRespuesta(respuesta, (acuse != null) ? string.Format("{0} - {1}", acuse.Response, acuse.Comments) : "");
					}
				}


				//Valida estado del documento en la Plataforma de la DIAN
				if (respuesta.IdProceso == ProcesoEstado.EnvioZip.GetHashCode())
				{
					if (respuesta.EstadoDian == null || respuesta.EstadoDian.EstadoDocumento == EstadoDocumentoDian.Pendiente.GetHashCode())
					{
						respuesta = Consultar(documento, empresa, ref respuesta);
					}

					// envía el mail de documentos al adquiriente
					if (respuesta.EstadoDian.EstadoDocumento == EstadoDocumentoDian.Aceptado.GetHashCode())
					{
						if ((documento.StrProveedorReceptor == null) || documento.StrProveedorReceptor.Equals(Constantes.NitResolucionsinPrefijo))
						{
							if (documento.IntEnvioMail == null || documento.IntEnvioMail == false) 
							{
								respuesta = Envio(documento_obj, documento, empresa, ref respuesta, ref documento_result);
								Ctl_Documento documento_tmp = new Ctl_Documento();
								documento_tmp.Actualizar(documento);
								//ValidarRespuesta(respuesta);
							}
							else
							{
								//Actualiza la respuesta
								respuesta.DescripcionProceso = Enumeracion.GetDescription(ProcesoEstado.EnvioEmailAcuse);
								respuesta.FechaUltimoProceso = Fecha.GetFecha();
								respuesta.IdProceso = ProcesoEstado.EnvioEmailAcuse.GetHashCode();

								//Actualiza Documento en Base de Datos
								documento.DatFechaActualizaEstado = respuesta.FechaUltimoProceso;
								documento.IntIdEstado = Convert.ToInt16(respuesta.IdProceso);

								Ctl_Documento documento_tmp = new Ctl_Documento();
								documento_tmp.Actualizar(documento);

								//Actualiza la categoria con el nuevo estado
								respuesta.IdEstado = documento.IdCategoriaEstado;
								respuesta.DescripcionEstado = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<CategoriaEstado>(documento.IdCategoriaEstado));
								ValidarRespuesta(respuesta, respuesta.DescripcionEstado,null,false);
							}

						}
						else
						{
							//Se actualiza respuesta	
							respuesta.DescripcionProceso = Enumeracion.GetDescription(ProcesoEstado.PendienteEnvioProveedorDoc);
							respuesta.FechaUltimoProceso = Fecha.GetFecha();
							respuesta.IdProceso = ProcesoEstado.PendienteEnvioProveedorDoc.GetHashCode();

							//Actualiza Documento en Base de Datos
							documento.DatFechaActualizaEstado = Fecha.GetFecha();
							documento.IntIdEstado = (short)respuesta.IdProceso;

							//Actualizo el estado del documento para enviar al proveedor receptor
							Ctl_Documento documento_tmp = new Ctl_Documento();
							documento_tmp.Actualizar(documento);

							//Actualiza la categoria con el nuevo estado
							respuesta.IdEstado = documento.IdCategoriaEstado;
							respuesta.DescripcionEstado = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<CategoriaEstado>(documento.IdCategoriaEstado));
							ValidarRespuesta(respuesta, respuesta.DescripcionEstado);
						}
					}
					else if((respuesta.EstadoDian.EstadoDocumento == EstadoDocumentoDian.Pendiente.GetHashCode() || respuesta.EstadoDian.EstadoDocumento == EstadoDocumentoDian.Recibido.GetHashCode()) && (documento.IntEnvioMail == null || documento.IntEnvioMail == false))
					{
						respuesta = Envio(documento_obj, documento, empresa, ref respuesta, ref documento_result, true);
						Ctl_Documento documento_tmp = new Ctl_Documento();
						documento_tmp.Actualizar(documento);
						//ValidarRespuesta(respuesta);

					}
				}

				// envía el mail de acuse de recibo al facturador electrónico
				if ((documento.IntAdquirienteRecibo > AdquirienteRecibo.Pendiente.GetHashCode() && documento.IntAdquirienteRecibo < AdquirienteRecibo.AprobadoTacito.GetHashCode()) && (documento.IntIdEstado != ProcesoEstado.Finalizacion.GetHashCode()))
				{

					Ctl_Documento ctl_documento = new Ctl_Documento();
					bool email = false;

					if (string.IsNullOrEmpty(documento.StrUrlAcuseUbl))
					{
						email = ctl_documento.ReenviarRespuestaAcuse(documento.StrIdSeguridad, documento.TblEmpresasFacturador.StrMailAdmin, "");
					}
					else if (documento.IntIdEstado > ProcesoEstado.EnvioZip.GetHashCode())
					{
						email = true;
					}

					if (email)
					{
						documento.IntIdEstado = Convert.ToInt16(ProcesoEstado.Finalizacion.GetHashCode());
						documento.DatFechaActualizaEstado = fecha_actual;
						ctl_documento.Actualizar(documento);
						respuesta.IdProceso = documento.IntIdEstado;
						respuesta.DescripcionProceso = Enumeracion.GetDescription(ProcesoEstado.Finalizacion);
						respuesta.MotivoRechazo = documento.StrAdquirienteMvoRechazo;
						respuesta.FechaUltimoProceso = fecha_actual;
						respuesta.ProcesoFinalizado = 1;
					}

				}
			}
			catch (Exception excepcion)
			{
				respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error al procesar el documento. Detalle: {0} ", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.ERROR_NO_CONTROLADO, excepcion.InnerException);
				//valida si el documento ya fue enviado
				if((documento.IntEnvioMail == null || documento.IntEnvioMail == false) && empresa.IntEnvioMailRecepcion == true)
				{
					respuesta = Envio(documento_obj, documento, empresa, ref respuesta, ref documento_result, true);
					Ctl_Documento documento_tmp = new Ctl_Documento();
					documento.IntEnvioMail = true;
					documento_tmp.Actualizar(documento);

				}
				LogExcepcion.Guardar(excepcion);
				// no se controla excepción
			}

			return respuesta;

		}

	}
}
