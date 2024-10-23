using HGInetDIANServicios;
using HGInetMiFacturaElectonicaController.Auditorias;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Enumerables;
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
		/// Procesa un archivo de documentos
		/// </summary>
		/// <param name="id_peticion">Peticion de la plataforma </param>
		/// <param name="archivo">Objeto don informacion del XML-UBL</param>
		/// <param name="facturador">Objeto BD del Facturador Electónico</param>
		/// <param name="resolucion">Objeto BD de las Resoluciones del Facturador Electronico</param>
		/// <returns>Objeto tipo DocumentoRespuesta</returns>
		public static DocumentoRespuesta Procesar(Guid id_peticion, DocumentoArchivo archivo, TblEmpresas facturador, TblEmpresasResoluciones resolucion)
		{

			DateTime fecha_actual = Fecha.GetFecha();

			// representación de datos en objeto
			var documento_obj = (dynamic)null;

			string contenido_xml = string.Empty;
			string cufe_calculado = string.Empty;

			try
			{
				//Convierte a string el array de byte
				contenido_xml = Encoding.UTF8.GetString(archivo.ArchivoXmlUbl);

				// valida el contenido del archivo
				if (string.IsNullOrWhiteSpace(contenido_xml))
					throw new ArgumentException("El archivo XML UBL se encuentra vacío.");

				// convierte el contenido de texto a xml
				XmlReader xml_reader = XmlReader.Create(new StringReader(contenido_xml));

				// convierte el objeto de acuerdo con el tipo de documento
				XmlSerializer serializacion = null;

				//Valida tipo de documento para hacer la deserializacion
				if (archivo.TipoDocumento == TipoDocumento.Factura.GetHashCode())
				{
					HGInetUBL.InvoiceType conversion = new HGInetUBL.InvoiceType();

					serializacion = new XmlSerializer(typeof(HGInetUBL.InvoiceType));

					conversion = (HGInetUBL.InvoiceType)serializacion.Deserialize(xml_reader);

					// agrega los campos de la Dian correspondientes al Proveedor Tecnológico
					//conversion = (InvoiceType)AgregarCamposDian(conversion, TipoDocumento.Factura, facturador);


					documento_obj = HGInetUBL.FacturaXML.Convertir(conversion,null);

					cufe_calculado = HGInetUBL.FacturaXML.CalcularCUFE(conversion, resolucion.StrClaveTecnica);

					if (!cufe_calculado.Equals(documento_obj.Cufe) || string.IsNullOrEmpty(documento_obj.Cufe))
						throw new ArgumentException("el CUFE enviado no esta bien formado");

					documento_obj.DataKey = archivo.DataKey;
					documento_obj.CodigoRegistro = archivo.CodigoRegistro;

				}
				else if (archivo.TipoDocumento == TipoDocumento.NotaCredito.GetHashCode())
				{

					HGInetUBL.CreditNoteType conversion = new HGInetUBL.CreditNoteType();

					serializacion = new XmlSerializer(typeof(HGInetUBL.CreditNoteType));

					conversion = (HGInetUBL.CreditNoteType)serializacion.Deserialize(xml_reader);

					// agrega los campos de la Dian correspondientes al Proveedor Tecnológico
					//conversion = (CreditNoteType)AgregarCamposDian(conversion, TipoDocumento.NotaCredito, facturador);


					documento_obj = HGInetUBL.NotaCreditoXML.Convertir(conversion, null);

					cufe_calculado = HGInetUBL.NotaCreditoXML.CalcularCUFE(conversion, resolucion.StrClaveTecnica, documento_obj.CufeFactura);

					if (!cufe_calculado.Equals(documento_obj.Cufe) || string.IsNullOrEmpty(documento_obj.Cufe))
						throw new ArgumentException("el CUFE enviado no esta bien formado");

					documento_obj.DataKey = archivo.DataKey;
					documento_obj.CodigoRegistro = archivo.CodigoRegistro;

				}
				else if (archivo.TipoDocumento == TipoDocumento.NotaDebito.GetHashCode())
				{
					HGInetUBL.DebitNoteType conversion = new HGInetUBL.DebitNoteType();

					serializacion = new XmlSerializer(typeof(HGInetUBL.DebitNoteType));

					conversion = (HGInetUBL.DebitNoteType)serializacion.Deserialize(xml_reader);


					// agrega los campos de la Dian correspondientes al Proveedor Tecnológico
					//conversion = (DebitNoteType)AgregarCamposDian(conversion, TipoDocumento.NotaDebito, facturador);


					documento_obj = HGInetUBL.NotaDebitoXML.Convertir(conversion, null);

					cufe_calculado = HGInetUBL.NotaDebitoXML.CalcularCUFE(conversion, resolucion.StrClaveTecnica, documento_obj.CufeFactura);

					if (!cufe_calculado.Equals(documento_obj.Cufe) || string.IsNullOrEmpty(documento_obj.Cufe))
						throw new ArgumentException("el CUFE enviado no esta bien formado");

					documento_obj.DataKey = archivo.DataKey;
					documento_obj.CodigoRegistro = archivo.CodigoRegistro;


				}

				// cerrar la lectura del archivo xml
				xml_reader.Close();

				// valida la conversión del objeto
				if (documento_obj == null)
					throw new ArgumentException("No se recibieron datos para realizar el proceso");


			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Archivos, MensajeTipo.Error, MensajeAccion.ninguna);
				throw new ArgumentException(string.Format("Error al convertir el XML-UBL: {0}", excepcion.Message), excepcion.InnerException);

			}

			// genera el estado actual de respuesta del documento
			DocumentoRespuesta respuesta = new DocumentoRespuesta()
			{
				Aceptacion = 0,
				CodigoRegistro = documento_obj.CodigoRegistro,
				Cufe = "",
				DescripcionProceso = "Recepción - Información del documento.",
				Documento = documento_obj.Documento,
				DocumentoTipo = archivo.TipoDocumento,
				Error = null,
				FechaRecepcion = fecha_actual,
				FechaUltimoProceso = fecha_actual,
				IdDocumento = Guid.NewGuid().ToString(),
				Identificacion = documento_obj.DatosAdquiriente.Identificacion,
				IdProceso = ProcesoEstado.Recepcion.GetHashCode(),
				MotivoRechazo = "",
				NumeroResolucion = documento_obj.NumeroResolucion,
				Prefijo = documento_obj.Prefijo,
				ProcesoFinalizado = 0,
				UrlPdf = "",
				UrlXmlUbl = "",
				IdPeticion = id_peticion,
				IdentificacionObligado = documento_obj.DatosObligado.Identificacion
				
			};

			FacturaE_Documento documento_result = new FacturaE_Documento();

			try
			{
				//Obtiene el tipo de Documento segun el Enumerable
				TipoDocumento tipo_doc = Enumeracion.GetEnumObjectByValue<TipoDocumento>(archivo.TipoDocumento);

				// valida la información del documento
				respuesta = Validar(documento_obj, tipo_doc, resolucion, ref respuesta, facturador);
				ValidarRespuesta(respuesta);


				if (facturador.IntHabilitacion > 0)
				{
					//Guarda la id de la Peticion con la que se esta haciendo el proceso
					documento_result.IdSeguridadPeticion = id_peticion;
					documento_result.IdSeguridadDocumento = Guid.Parse(respuesta.IdDocumento);
					documento_result.IdSeguridadTercero = facturador.StrIdSeguridad;
					documento_result.Documento = documento_obj;
					documento_result.DocumentoTipo = tipo_doc;

					//Calcula el Cufe con el UBL enviado
					documento_result.CUFE = cufe_calculado;

					// convierte los datos del objeto en texto XML 
					StringBuilder txt_xml = new StringBuilder();
					txt_xml.Append(contenido_xml);
					documento_result.DocumentoXml = txt_xml;

					//Obtiene el nombre del archivo XML
					documento_result.NombreXml = HGInetUBL.NombramientoArchivo.ObtenerXml(documento_obj.Documento.ToString(), facturador.StrIdentificacion, tipo_doc, documento_obj.Prefijo);
					documento_result.NombrePdf = documento_result.NombreXml;

					// genera el nombre del archivo ZIP
					documento_result.NombreZip = HGInetUBL.NombramientoArchivo.ObtenerZip(documento_obj.Documento.ToString(), facturador.StrIdentificacion, tipo_doc, documento_obj.Prefijo);

					Ctl_Documento documento_tmp = new Ctl_Documento();


					//guarda documento en BD
					TblDocumentos documentoBd = Ctl_Documento.Convertir(respuesta, documento_obj, resolucion, facturador, tipo_doc);


					// almacena el xml en ubl
					respuesta = UblGuardar(documentoBd, ref respuesta, ref documento_result);
					ValidarRespuesta(respuesta);

					//llena los datos de la extension DIAN en el XML enviado
					CamposDian(ref documento_result, facturador, ref respuesta);
					ValidarRespuesta(respuesta);


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
						respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error al obtener el Adquiriente Detalle. Detalle: ", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.ERROR_LICENCIA, excepcion.InnerException);
						Ctl_Log.Guardar(excepcion, MensajeCategoria.BaseDatos, MensajeTipo.Error, MensajeAccion.consulta);
						throw excepcion;
					}

					//Crea el documento en BD
					try
					{

						documento_tmp.Crear(documentoBd);

					}
					catch (Exception excepcion)
					{
						respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error al guardar el documento. Detalle: {0} ", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.ERROR_LICENCIA, excepcion.InnerException);
						Ctl_Log.Guardar(excepcion, MensajeCategoria.BaseDatos, MensajeTipo.Error, MensajeAccion.actualizacion);
						throw excepcion;
					}

					// almacena Formato
					respuesta = GuardarFormato(documento_obj, documentoBd, ref respuesta, ref documento_result, facturador);
					ValidarRespuesta(respuesta);


					// firma el xml
					respuesta = UblFirmar(facturador, documentoBd, ref respuesta, ref documento_result);
					if (documentoBd.IntEnvioMail == true && facturador.IntEnvioMailRecepcion == true)
					{
						respuesta = Envio(documento_obj, documentoBd, facturador, ref respuesta, ref documento_result, true);
						documento_tmp = new Ctl_Documento();
						documentoBd = documento_tmp.Actualizar(documentoBd);
					}
					ValidarRespuesta(respuesta);


					// comprime el archivo xml firmado                        
					respuesta = UblComprimir(documentoBd, ref respuesta, ref documento_result);
					if (documentoBd.IntEnvioMail == true && facturador.IntEnvioMailRecepcion == true)
					{
						respuesta = Envio(documento_obj, documentoBd, facturador, ref respuesta, ref documento_result, true);
						documento_tmp = new Ctl_Documento();
						documentoBd = documento_tmp.Actualizar(documentoBd);
					}
					ValidarRespuesta(respuesta);


					// envía el archivo zip con el xml firmado a la DIAN
					HGInetDIANServicios.DianFactura.AcuseRecibo acuse = EnviarDian(documentoBd, facturador, ref respuesta, ref documento_result);
					if (documentoBd.IntEnvioMail == true && facturador.IntEnvioMailRecepcion == true)
					{
						respuesta = Envio(documento_obj, documentoBd, facturador, ref respuesta, ref documento_result, true);
						documento_tmp = new Ctl_Documento();
						documentoBd = documento_tmp.Actualizar(documentoBd);
					}
					ValidarRespuesta(respuesta);


					//Valida estado del documento en la Plataforma de la DIAN
					respuesta = Consultar(documentoBd, facturador, ref respuesta);


					// envía el mail de documentos al adquiriente
					if (respuesta.EstadoDian.EstadoDocumento == EstadoDocumentoDian.Aceptado.GetHashCode())
					{
						if ((documentoBd.StrProveedorReceptor == null) || documentoBd.StrProveedorReceptor.Equals(Constantes.NitResolucionsinPrefijo))
						{
							respuesta = Envio(documento_obj, documentoBd, facturador, ref respuesta, ref documento_result);
							ValidarRespuesta(respuesta);
						}
						else
						{
							//Se actualiza respuesta	
							respuesta.DescripcionProceso = Enumeracion.GetDescription(ProcesoEstado.PendienteEnvioProveedorDoc);
							respuesta.FechaUltimoProceso = Fecha.GetFecha();
							respuesta.IdProceso = ProcesoEstado.PendienteEnvioProveedorDoc.GetHashCode();

							//Actualiza Documento en Base de Datos
							documentoBd.DatFechaActualizaEstado = Fecha.GetFecha();
							documentoBd.IntIdEstado = (short)respuesta.IdProceso;

							//Actualizo el estado del documento para enviar al proveedor receptor
							documento_tmp = new Ctl_Documento();
							documento_tmp.Actualizar(documentoBd);

							//Actualiza la categoria con el nuevo estado
							respuesta.IdEstado = documentoBd.IdCategoriaEstado;
							respuesta.DescripcionEstado = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<CategoriaEstado>(documentoBd.IdCategoriaEstado));
						}
					}

				}

			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.consulta);
			}
			return respuesta;

		}

	}
}
