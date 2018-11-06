using HGInetDIANServicios;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetUBL;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Mail;
using LibreriaGlobalHGInet.Objetos;
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
		/// Continúa el procesamiento de varios documentos registrados en la base de datos de acuerdo con el estado actual del mismo
		/// </summary>
		/// <param name="documentos">datos de los documentos en base de datos</param>
		/// <returns>datos de resultado para los documentos</returns>
		public static List<DocumentoRespuesta> Procesar(List<TblDocumentos> documentos)
		{
			List<DocumentoRespuesta> documentos_respuestas = new List<DocumentoRespuesta>();

			foreach (TblDocumentos documento in documentos)
			{
				DocumentoRespuesta item_respuesta = new DocumentoRespuesta();

				// obtiene el proceso actual del documento
				ProcesoEstado proceso_actual = Enumeracion.ParseToEnum<ProcesoEstado>((int)documento.IntIdEstado);

				try
				{   // procesa el documento
					item_respuesta = Procesar(documento);
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
		/// <returns>datos de resultado para el documento</returns>
		public static DocumentoRespuesta Procesar(TblDocumentos documento)
		{
			// valida que la empresa se encuentre como facturador electrónico
			if (documento.TblEmpresasFacturador.IntHabilitacion == 0)
				throw new ArgumentException(string.Format("No se encuentra habilitado como Facturador Electrónico - {0} {1}", documento.StrEmpresaFacturador, documento.TblEmpresasFacturador.StrRazonSocial));

			// obtiene el proceso actual del documento
			ProcesoEstado proceso_actual = Enumeracion.ParseToEnum<ProcesoEstado>((int)documento.IntIdEstado);

			// valida que el proceso del documento cuente con el XML en UBL físicamente
			if (proceso_actual.GetHashCode() <= ProcesoEstado.AlmacenaXML.GetHashCode())
				throw new ArgumentException(string.Format("No se puede procesar el documento {0} en estado {1} del Facturador Electrónico - {2} {3}", documento.StrIdSeguridad, Enumeracion.GetDescription(proceso_actual), documento.StrEmpresaFacturador, documento.TblEmpresasFacturador.StrRazonSocial));

			// obtiene el tipo de documento
			TipoDocumento tipo_documento = Enumeracion.ParseToEnum<TipoDocumento>(documento.IntDocTipo);

			// genera el estado actual de respuesta del documento
			DocumentoRespuesta respuesta = new DocumentoRespuesta()
			{
				Aceptacion = documento.IntAdquirienteRecibo,
				CodigoRegistro = documento.StrObligadoIdRegistro,
				Cufe = documento.StrCufe,
				DescripcionProceso = Enumeracion.GetDescription(proceso_actual),
				DocumentoTipo = documento.IntDocTipo,
				Documento = documento.IntNumero,
				Error = null,
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

			if (respuesta.ProcesoFinalizado == 1)
				return respuesta;

			DateTime fecha_actual = Fecha.GetFecha();

			try
			{

				// representación de datos en objeto
				var documento_obj = (dynamic)null;

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

					documento_obj = FacturaXML.Convertir(conversion);
					documento.StrCufe = documento_obj.Cufe;
				}
				else if (tipo_documento == TipoDocumento.NotaCredito)
				{
					serializacion = new XmlSerializer(typeof(CreditNoteType));

					CreditNoteType conversion = (CreditNoteType)serializacion.Deserialize(xml_reader);

					documento_obj = NotaCreditoXML.Convertir(conversion);
					documento.StrCufe = documento_obj.Cufe;
				}
				else if (tipo_documento == TipoDocumento.NotaDebito)
				{
					serializacion = new XmlSerializer(typeof(DebitNoteType));

					DebitNoteType conversion = (DebitNoteType)serializacion.Deserialize(xml_reader);

					documento_obj = NotaDebitoXML.Convertir(conversion);
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
				TblEmpresas empresa = documento.TblEmpresasFacturador;

				PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

				// ruta física del xml
				string carpeta_xml = string.Format("{0}\\{1}\\{2}", plataforma_datos.RutaDmsFisica, Constantes.CarpetaFacturaElectronica, empresa.StrIdSeguridad.ToString());
				carpeta_xml = string.Format(@"{0}\{1}", carpeta_xml, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaXmlFacturaE);

				// carpeta del zip
				string carpeta_zip = string.Format("{0}\\{1}\\{2}", plataforma_datos.RutaDmsFisica, Constantes.CarpetaFacturaElectronica, empresa.StrIdSeguridad.ToString());
				carpeta_zip = string.Format(@"{0}\{1}", carpeta_zip, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian);

				// información del procesamiento del archivo
				FacturaE_Documento documento_result = new FacturaE_Documento()
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
				documento_result.NombreXml = NombramientoArchivo.ObtenerXml(documento_obj.Documento.ToString(), documento_obj.DatosObligado.Identificacion, tipo_documento, documento_obj.Prefijo);
				documento_result.NombrePdf = documento_result.NombreXml;

				// genera el nombre del archivo ZIP
				documento_result.NombreZip = NombramientoArchivo.ObtenerZip(documento_obj.Documento.ToString(), documento_obj.DatosObligado.Identificacion, tipo_documento);

				// firma el xml (valida si no ha realizado el envío a la DIAN vuelve a firmar)
				if (respuesta.IdProceso < ProcesoEstado.EnvioZip.GetHashCode())
				{
					respuesta = UblFirmar(documento, ref respuesta, ref documento_result);
					ValidarRespuesta(respuesta);
				}

				// comprime el archivo xml firmado
				if (respuesta.IdProceso < ProcesoEstado.EnvioZip.GetHashCode())
				{
					respuesta = UblComprimir(documento, ref respuesta, ref documento_result);
					ValidarRespuesta(respuesta);
				}

				// envía el archivo zip con el xml firmado a la DIAN
				if (respuesta.IdProceso < ProcesoEstado.EnvioZip.GetHashCode())
				{
					respuesta = Consultar(documento, empresa, ref respuesta);

					//Si no hay respuesta de la DIAN del documento enviado se procede a enviar de nuevo
					if (respuesta.EstadoDian.CodigoRespuesta == null)
					{
						HGInetDIANServicios.DianFactura.AcuseRecibo acuse = EnviarDian(documento, empresa, ref respuesta, ref documento_result);
						ValidarRespuesta(respuesta);
					}
				}


				//Valida estado del documento en la Plataforma de la DIAN
				if (respuesta.IdProceso == ProcesoEstado.EnvioZip.GetHashCode())
				{
					respuesta = Consultar(documento, empresa, ref respuesta);

					//Si no hay respuesta de la DIAN del documento enviado se procede a enviar de nuevo
					if (respuesta.EstadoDian.CodigoRespuesta == null)
					{
						HGInetDIANServicios.DianFactura.AcuseRecibo acuse = EnviarDian(documento, empresa, ref respuesta, ref documento_result);
						ValidarRespuesta(respuesta);

						//Valida de nuevo el estado del documento si fue enviado y hay respuesta de la DIAN
						respuesta = Consultar(documento, empresa, ref respuesta);
					}

					// envía el mail de documentos al adquiriente
					if (respuesta.EstadoDian.EstadoDocumento == EstadoDocumentoDian.Aceptado.GetHashCode())
					{
						if ((documento.StrProveedorReceptor == null) || documento.StrProveedorReceptor.Equals(Constantes.NitResolucionsinPrefijo))
						{
							respuesta = Envio(documento_obj, documento, empresa, ref respuesta, ref documento_result);
							ValidarRespuesta(respuesta);
						}
						else
						{
							//Se actualiza respuesta	
							respuesta.DescripcionProceso = "Documento Pendiente Envío Proveedor";
							respuesta.FechaUltimoProceso = Fecha.GetFecha();
							respuesta.IdProceso = ProcesoEstado.PendienteEnvioProveedorDoc.GetHashCode();

							//Actualiza Documento en Base de Datos
							documento.DatFechaActualizaEstado = Fecha.GetFecha();
							documento.IntIdEstado = (short)respuesta.IdProceso;

							//Actualizo el estado del documento para enviar al proveedor receptor
							Ctl_Documento documento_tmp = new Ctl_Documento();
							documento_tmp.Actualizar(documento);
						}
					}
				}

				// envía el mail de acuse de recibo al facturador electrónico
				if ((documento.IntAdquirienteRecibo > 0) && (documento.IntIdEstado != ProcesoEstado.Finalizacion.GetHashCode()))
				{

					Ctl_Documento ctl_documento = new Ctl_Documento();
					bool email = ctl_documento.ReenviarRespuestaAcuse(documento.StrIdSeguridad, documento.TblEmpresasFacturador.StrMail);

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

				LogExcepcion.Guardar(excepcion);
				// no se controla excepción
			}

			return respuesta;

		}

	}
}
