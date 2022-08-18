using HGInetDIANServicios;
using HGInetMiFacturaElectonicaController.Auditorias;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using LibreriaGlobalHGInet.RegistroLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace HGInetMiFacturaElectonicaController.Procesos
{
	public partial class Ctl_Documentos
	{

		/// <summary>
		/// Continúa el procesamiento de un documento registrado en la base de datos de acuerdo con el estado actual del mismo
		/// </summary>
		/// <param name="documento">datos del documento en base de datos</param>
		/// <param name="consulta_documento">indica si antes de enviar el documento a la DIAN se debe consultar</param>
		/// <returns>datos de resultado para el documento</returns>
		public static DocumentoRespuesta ProcesarV2(TblDocumentos documento, bool consulta_documento)
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
				DescripcionAceptacion = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<CodigoResponseV2>(documento.IntAdquirienteRecibo)),
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
					serializacion = new XmlSerializer(typeof(InvoiceType));

					InvoiceType conversion = (InvoiceType)serializacion.Deserialize(xml_reader);

					documento_obj = HGInetUBLv2_1.FacturaXMLv2_1.Convertir(conversion, documento);
					documento.StrCufe = documento_obj.Cufe;
				}
				else if (tipo_documento == TipoDocumento.NotaCredito)
				{
					serializacion = new XmlSerializer(typeof(CreditNoteType));

					CreditNoteType conversion = (CreditNoteType)serializacion.Deserialize(xml_reader);

					documento_obj = HGInetUBLv2_1.NotaCreditoXMLv2_1.Convertir(conversion, documento);
					documento.StrCufe = documento_obj.Cufe;
				}
				else if (tipo_documento == TipoDocumento.NotaDebito)
				{
					serializacion = new XmlSerializer(typeof(DebitNoteType));

					DebitNoteType conversion = (DebitNoteType)serializacion.Deserialize(xml_reader);

					documento_obj = HGInetUBLv2_1.NotaDebitoXMLv2_1.Convertir(conversion, documento);
					documento.StrCufe = documento_obj.Cufe;
				}
				else if (tipo_documento == TipoDocumento.Nomina)
				{
					serializacion = new XmlSerializer(typeof(NominaIndividualType));

					NominaIndividualType conversion = (NominaIndividualType)serializacion.Deserialize(xml_reader);

					documento_obj = HGInetUBLv2_1.NominaXML.Convertir(conversion, documento);
					documento.StrCufe = documento_obj.Cune;
					Ctl_Empresa empresa_config = new Ctl_Empresa();
					documento_obj.DatosTrabajador.Email = empresa_config.Obtener(documento.StrEmpresaAdquiriente).StrMailAdmin;


				}
				else if (tipo_documento == TipoDocumento.NominaAjuste)
				{
					serializacion = new XmlSerializer(typeof(NominaIndividualDeAjusteType));

					NominaIndividualDeAjusteType conversion = (NominaIndividualDeAjusteType)serializacion.Deserialize(xml_reader);
					
					documento_obj = HGInetUBLv2_1.NominaAjusteXML.Convertir(conversion, documento);
					documento.StrCufe = documento_obj.Cune;
					if (documento_obj.TipoNota.Equals(1))
					{
						Ctl_Empresa empresa_config = new Ctl_Empresa();
						documento_obj.DatosTrabajador.Email = empresa_config.Obtener(documento.StrEmpresaAdquiriente).StrMailAdmin;
					}
					
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
					RutaArchivosEnvio = carpeta_zip,
					VersionDian = empresa.IntVersionDian
				};

				// genera el nombre del archivo XML y PDF
				documento_result.NombreXml = HGInetUBLv2_1.NombramientoArchivo.ObtenerXml(documento_obj.Documento.ToString(), empresa.StrIdentificacion, tipo_documento, documento_obj.Prefijo);
				documento_result.NombrePdf = documento_result.NombreXml;

				// genera el nombre del archivo ZIP
				documento_result.NombreZip = HGInetUBLv2_1.NombramientoArchivo.ObtenerZip(documento_obj.Documento.ToString(), empresa.StrIdentificacion, tipo_documento, documento_obj.Prefijo);

				// firma el xml (valida si no ha realizado el envío a la DIAN vuelve a firmar)
				if (respuesta.IdProceso < ProcesoEstado.FirmaXml.GetHashCode())
				{
					respuesta = UblFirmar(empresa, documento, ref respuesta, ref documento_result);
					ValidarRespuesta(respuesta, respuesta.UrlXmlUbl);
				}

				//// comprime el archivo xml firmado
				if (respuesta.IdProceso < ProcesoEstado.CompresionXml.GetHashCode())
				{
					respuesta = UblComprimir(documento, ref respuesta, ref documento_result);
					ValidarRespuesta(respuesta, respuesta.UrlXmlUbl);
				}

				//Se valida que no sea un documento para probar la respuesta de los servicios
				if (documento.IntTipoOperacion != 50 || documento_obj.TipoOperacion != 50)
				{

					// envía el archivo zip con el xml firmado a la DIAN
					if (respuesta.IdProceso < ProcesoEstado.EnvioZip.GetHashCode())
					{
						//valida si debe consultar el estado del documento en la DIAN
						if (consulta_documento)
						{
							string zipkey = string.Empty;

							if (empresa.IntHabilitacion < 99)
								zipkey = documento.StrIdRadicadoDian.ToString();

							respuesta = Consultar(documento, empresa, ref respuesta, zipkey);

							//Si no hay respuesta de la DIAN del documento enviado se procede a enviar de nuevo
							if (respuesta.EstadoDian.EstadoDocumento == EstadoDocumentoDian.Pendiente.GetHashCode())
							{
								HGInetDIANServicios.DianFactura.AcuseRecibo acuse = EnviarDian(documento, empresa, ref respuesta, ref documento_result, resolucion.StrIdSetDian, true);
								ValidarRespuesta(respuesta, (acuse != null) ? string.Format("{0} - {1}", acuse.Response, acuse.Comments) : "");
							}
							else
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
							HGInetDIANServicios.DianFactura.AcuseRecibo acuse = EnviarDian(documento, empresa, ref respuesta, ref documento_result, resolucion.StrIdSetDian, true);
							ValidarRespuesta(respuesta, (acuse != null) ? string.Format("{0} - {1}", acuse.Response, acuse.Comments) : "");
						}
					}


					//Valida estado del documento en la Plataforma de la DIAN
					if (respuesta.IdProceso == ProcesoEstado.EnvioZip.GetHashCode() || respuesta.IdProceso == ProcesoEstado.ProcesoPausadoPlataformaDian.GetHashCode())
					{
						if ((respuesta.EstadoDian == null || respuesta.EstadoDian.EstadoDocumento == EstadoDocumentoDian.Pendiente.GetHashCode()))
						{
							string zipkey = string.Empty;

							if (empresa.IntHabilitacion < 99)
								zipkey = documento.StrIdRadicadoDian.ToString();

							respuesta = Consultar(documento, empresa, ref respuesta, zipkey);


							//Si no hay respuesta de la DIAN del documento enviado se procede a enviar de nuevo
							if (respuesta.EstadoDian.EstadoDocumento == EstadoDocumentoDian.Pendiente.GetHashCode())
							{
								HGInetDIANServicios.DianFactura.AcuseRecibo acuse = EnviarDian(documento, empresa, ref respuesta, ref documento_result, resolucion.StrIdSetDian, true);
								ValidarRespuesta(respuesta, (acuse != null) ? string.Format("{0} - {1}", acuse.Response, acuse.Comments) : "");

								//Consulta si el envio es por habilitacion, produccion hace todo el proceso completo
								if (acuse.Response == 200)
									respuesta = Consultar(documento, empresa, ref respuesta, acuse.KeyV2);
							}

						}

						// envía el mail de documentos al adquiriente
						if (respuesta.EstadoDian.EstadoDocumento == EstadoDocumentoDian.Aceptado.GetHashCode())
						{
							if ((documento.StrProveedorReceptor == null) || documento.StrProveedorReceptor.Equals(Constantes.NitResolucionsinPrefijo))
							{
								bool enviar_correo = true;

								//Se hace validacion si es documento de nomina y no tiene habilitado el envio de correo
								if ((tipo_documento == TipoDocumento.Nomina || tipo_documento == TipoDocumento.NominaAjuste) && empresa.IntEnvioNominaMail == false || documento_obj.TipoOperacion == 3)
									enviar_correo = false;

								if ((documento.IntEnvioMail == null || documento.IntEnvioMail == false) && enviar_correo == true)
								{
									respuesta = Envio(documento_obj, documento, empresa, ref respuesta, ref documento_result);
									Ctl_Documento documento_tmp = new Ctl_Documento();
									documento_tmp.Actualizar(documento);
									//ValidarRespuesta(respuesta);
								}
								else
								{
									//Actualiza la respuesta
									if (tipo_documento != TipoDocumento.Nomina && tipo_documento != TipoDocumento.NominaAjuste)
									{
										respuesta.DescripcionProceso = Enumeracion.GetDescription(ProcesoEstado.EnvioEmailAcuse);
										respuesta.FechaUltimoProceso = Fecha.GetFecha();
										respuesta.IdProceso = ProcesoEstado.EnvioEmailAcuse.GetHashCode();

									}
									else
									{
										respuesta.DescripcionProceso = Enumeracion.GetDescription(ProcesoEstado.Finalizacion);
										respuesta.FechaUltimoProceso = Fecha.GetFecha();
										respuesta.IdProceso = ProcesoEstado.Finalizacion.GetHashCode();
									}
									

									//Actualiza Documento en Base de Datos
									documento.DatFechaActualizaEstado = respuesta.FechaUltimoProceso;
									documento.IntIdEstado = Convert.ToInt16(respuesta.IdProceso);

									Ctl_Documento documento_tmp = new Ctl_Documento();
									documento_tmp.Actualizar(documento);

									//Actualiza la categoria con el nuevo estado
									respuesta.IdEstado = documento.IdCategoriaEstado;
									respuesta.DescripcionEstado = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<CategoriaEstado>(documento.IdCategoriaEstado));
									if (tipo_documento != TipoDocumento.Nomina && tipo_documento != TipoDocumento.NominaAjuste)
										ValidarRespuesta(respuesta, respuesta.DescripcionEstado, null, false);
									else
										ValidarRespuesta(respuesta, respuesta.UrlPdf);
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
						else if (respuesta.EstadoDian.EstadoDocumento == EstadoDocumentoDian.Pendiente.GetHashCode())
						{
							/*respuesta = Envio(documento_obj, documento, empresa, ref respuesta, ref documento_result, true);
							Ctl_Documento documento_tmp = new Ctl_Documento();
							documento_tmp.Actualizar(documento);*/
							//ValidarRespuesta(respuesta);

							//Se actualiza respuesta
							respuesta.Error = new LibreriaGlobalHGInet.Error.Error(respuesta.EstadoDian.Descripcion, LibreriaGlobalHGInet.Error.CodigoError.VALIDACION);
							respuesta.DescripcionProceso = Enumeracion.GetDescription(ProcesoEstado.ProcesoPausadoPlataformaDian);
							respuesta.FechaUltimoProceso = Fecha.GetFecha();
							respuesta.IdProceso = ProcesoEstado.ProcesoPausadoPlataformaDian.GetHashCode();

							//Actualiza Documento en Base de Datos
							documento.DatFechaActualizaEstado = Fecha.GetFecha();
							documento.IntIdEstado = (short)respuesta.IdProceso;

							//Actualizo el estado del documento para enviar al proveedor receptor
							Ctl_Documento documento_tmp = new Ctl_Documento();
							documento_tmp.Actualizar(documento);

							//Actualiza la categoria con el nuevo estado
							respuesta.IdEstado = documento.IdCategoriaEstado;
							respuesta.DescripcionEstado = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<CategoriaEstado>(documento.IdCategoriaEstado));

						}
					}

					// envía el mail de acuse de recibo al facturador electrónico
					if ((documento.IntAdquirienteRecibo > CodigoResponseV2.Pendiente.GetHashCode() && documento.IntAdquirienteRecibo < CodigoResponseV2.AprobadoTacito.GetHashCode()) && (documento.IntIdEstado != ProcesoEstado.Finalizacion.GetHashCode()))
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
				else
				{
					//Se actualiza respuesta indicando que se completa el proceso
					respuesta.Error = new LibreriaGlobalHGInet.Error.Error("Respuesta efectiva de los servicios, proceso completo", LibreriaGlobalHGInet.Error.CodigoError.OK);
					respuesta.DescripcionProceso = Enumeracion.GetDescription(ProcesoEstado.PrevalidacionErrorPlataforma);
					respuesta.FechaUltimoProceso = Fecha.GetFecha();
					respuesta.IdProceso = ProcesoEstado.PrevalidacionErrorPlataforma.GetHashCode();

					//Actualiza Documento en Base de Datos
					documento.DatFechaActualizaEstado = Fecha.GetFecha();
					documento.IntIdEstado = (short)respuesta.IdProceso;

					//Actualizo el estado del documento para enviar al proveedor receptor
					Ctl_Documento documento_tmp = new Ctl_Documento();
					documento_tmp.Actualizar(documento);

					//Actualiza la categoria con el nuevo estado
					respuesta.IdEstado = documento.IdCategoriaEstado;
					respuesta.DescripcionEstado = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<CategoriaEstado>(documento.IdCategoriaEstado));
				}



			}
			catch (Exception excepcion)
			{
				respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error al procesar el documento. Detalle: {0} ", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.ERROR_NO_CONTROLADO, excepcion.InnerException);
				//valida si el documento ya fue enviado
				/*if ((documento.IntEnvioMail == null || documento.IntEnvioMail == false) && empresa.IntEnvioMailRecepcion == true)
				{
					respuesta = Envio(documento_obj, documento, empresa, ref respuesta, ref documento_result, true);
					Ctl_Documento documento_tmp = new Ctl_Documento();
					documento.IntEnvioMail = true;
					documento_tmp.Actualizar(documento);

				}*/
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion);
				// no se controla excepción
			}

			return respuesta;

		}




	}
}
