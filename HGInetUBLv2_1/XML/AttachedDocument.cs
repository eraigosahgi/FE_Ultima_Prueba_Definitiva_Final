using LibreriaGlobalHGInet.Objetos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using System.Xml.Serialization;
using HGInetMiFacturaElectonicaData.Modelo;
using LibreriaGlobalHGInet.Funciones;
using HGInetUBLv2_1.DianListas;
using LibreriaGlobalHGInet.General;
using System.Xml;
using System.IO;
using HGInetDIANServicios.DianWSValidacionPrevia;
using HGInetMiFacturaElectonicaData;
using HGInetUtilidadAzure.Almacenamiento;

namespace HGInetUBLv2_1
{
	public partial class AttachedDocument
	{


		/// <summary>
		/// Crea el documento Attach que contiene los eventos de la Factura y la respuesta de la DIAN
		/// </summary>
		/// <param name="id_documento">ID seguridad del Documento</param>
		/// <param name="obligado">Informacion del Obligado(Objeto)</param>
		/// <param name="adquiriente">Informacion del Adquiriente(Objeto)</param>
		/// <param name="ambiente">Ambiente de la DIAN 1 - Produccion, 2 - Habilitacion</param>
		/// <param name="documentoBd">Informacion del documento(Tbl)</param>
		/// <returns></returns>
		public static FacturaE_Documento CrearDocumento(string id_documento, Tercero obligado, Tercero adquiriente, string ambiente, TblDocumentos documentoBd, int evento_radian = 0, string url_evento = "", string cude_evento = "")
		{

			try
			{
				string documento = string.Format("{0}{1}", documentoBd.StrPrefijo, documentoBd.IntNumero);

				AttachedDocumentType attached = new AttachedDocumentType();
				XmlSerializerNamespaces namespaces_xml = NamespacesXML.ObtenerNamespaces(TipoDocumento.Attached);


				attached.UBLVersionID = new UBLVersionIDType();
				attached.UBLVersionID.Value = "UBL 2.1";

				attached.CustomizationID = new CustomizationIDType();
				attached.CustomizationID.Value = "Documentos adjuntos";

				//validacion para la informacion de los eventos
				attached.ProfileID = new ProfileIDType();
				if (evento_radian == 0)
					attached.ProfileID.Value = "Factura Electrónica de Venta";
				else
					attached.ProfileID.Value = "DIAN 2.1: ApplicationResponse de la Factura Electrónica de Venta";

				//---Ambiente al que se va enviar el documento: 1 - Produccion, 2 - Pruebas
				attached.ProfileExecutionID = new ProfileExecutionIDType();
				attached.ProfileExecutionID.Value = ambiente;//"2";

				//Consecutivo del attached
				attached.ID = new IDType();
				attached.ID.Value = string.Format("{0}{1}", documento, evento_radian);

				#region Fecha Generacion del Attached
				IssueDateType IssueDate = new IssueDateType();
				if (evento_radian == 0)
					IssueDate.Value = Convert.ToDateTime(documentoBd.DatFechaIngreso.ToString(Fecha.formato_fecha_hginet));
				else
					IssueDate.Value = Convert.ToDateTime(Fecha.GetFecha().ToString(Fecha.formato_fecha_hginet));
				attached.IssueDate = IssueDate;
				#endregion

				#region Hora Generacion del Attached
				//----Se debe enviar la hora de emision con -5 horas
				IssueTimeType IssueTime = new IssueTimeType();
				if (evento_radian == 0)
					IssueTime.Value = documentoBd.DatFechaIngreso.AddHours(5).AddMinutes(2).ToString(Fecha.formato_hora_zona);//Convert.ToDateTime(hora_documento);//Convert.ToDateTime(documento.Fecha.ToString(Fecha.formato_hora_completa)).AddHours(5);//
				else
					IssueTime.Value = Fecha.GetFecha().ToString(Fecha.formato_hora_zona);

				attached.IssueTime = IssueTime;
				#endregion

				attached.DocumentTypeCode = new DocumentTypeCodeType();
				attached.DocumentTypeCode.Value = "Contenedor de Factura Electrónica";

				//ID de la factura electrónica que origina el contenedor 
				attached.ParentDocumentID = new ParentDocumentIDType();
				if (evento_radian == 0)
					attached.ParentDocumentID.Value = documento;
				else
					attached.ParentDocumentID.Value = id_documento;

				//Persona que genera el contenedor 
				attached.SenderParty = new PartyType();
				if (evento_radian == 0)
					attached.SenderParty.PartyTaxScheme = ObtenerTercero(obligado);
				else
					attached.SenderParty.PartyTaxScheme = ObtenerTercero(adquiriente);

				//Persona que recibe el contenedor 
				attached.ReceiverParty = new PartyType();
				if (evento_radian == 0)
					attached.ReceiverParty.PartyTaxScheme = ObtenerTercero(adquiriente);
				else
					attached.ReceiverParty.PartyTaxScheme = ObtenerTercero(obligado);

				//Información del Documento Electrónico (Factura, NC, ND u otro enviado en el contenedor)
				attached.Attachment = new AttachmentType();
				ExternalReferenceType ExternalReference = new ExternalReferenceType();
				ExternalReference.MimeCode = new MimeCodeType();
				ExternalReference.MimeCode.Value = "text/xml";
				ExternalReference.EncodingCode = new EncodingCodeType();
				ExternalReference.EncodingCode.Value = "UTF-8";

				//![CDATA[Acá se coloca el DE en formato xml]]>
				ExternalReference.Description = new DescriptionType[1];

				// lee el archivo XML en UBL desde la ruta pública
				string contenido_xml = Archivo.ObtenerContenido(documentoBd.StrUrlArchivoUbl);

				if (!string.IsNullOrWhiteSpace(documentoBd.StrUrlArchivoUbl) && documentoBd.StrUrlArchivoUbl.Contains("hgidocs.blob") && string.IsNullOrWhiteSpace(contenido_xml))
				{
					AzureStorage conexion = HgiConfiguracion.GetConfiguration().AzureStorage;

					string nombre_contenedor = string.Format("files-hgidocs-{0}", documentoBd.DatFechaIngreso.Year);

					BlobController contenedor = new BlobController(conexion.connectionString, nombre_contenedor);

					contenido_xml = contenedor.LecturaBlob(Path.GetExtension(documentoBd.StrUrlArchivoUbl), Path.GetFileNameWithoutExtension(documentoBd.StrUrlArchivoUbl));
				}

				if (evento_radian > 0)
				{
					contenido_xml = Archivo.ObtenerContenido(url_evento);
				}

				DescriptionType Description = new DescriptionType();
				//Description.Value = string.Format("<![CDATA[{0}]]>", contenido_xml);
				Description.Value = "contenido_xml";

				ExternalReference.Description[0] = Description;
				attached.Attachment.ExternalReference = ExternalReference;


				attached.ParentDocumentLineReference = new LineReferenceType[1];

				LineReferenceType LineReference = new LineReferenceType();
				LineReference.LineID = new LineIDType();
				LineReference.LineID.Value = "1";
				LineReference.DocumentReference = new DocumentReferenceType();
				LineReference.DocumentReference.ID = new IDType();
				if (evento_radian == 0)
					LineReference.DocumentReference.ID.Value = documento;
				else
					LineReference.DocumentReference.ID.Value = id_documento;
				
				LineReference.DocumentReference.UUID = new UUIDType();
				if (evento_radian == 0)
					LineReference.DocumentReference.UUID.Value = documentoBd.StrCufe;
				else
					LineReference.DocumentReference.UUID.Value = cude_evento;
				if (documentoBd.IntDocTipo == TipoDocumento.Factura.GetHashCode() && evento_radian == 0)
				{
					LineReference.DocumentReference.UUID.schemeName = "CUFE-SHA384";
				}
				else
				{
					LineReference.DocumentReference.UUID.schemeName = "CUDE-SHA384";
				}

				LineReference.DocumentReference.IssueDate = new IssueDateType();
				if (evento_radian == 0)
					LineReference.DocumentReference.IssueDate.Value = Convert.ToDateTime(documentoBd.DatFechaIngreso.ToString(Fecha.formato_fecha_hginet));
				else
					LineReference.DocumentReference.IssueDate.Value = Convert.ToDateTime(documentoBd.DatAdquirienteFechaRecibo.Value.ToString(Fecha.formato_fecha_hginet));

				LineReference.DocumentReference.DocumentType = new DocumentTypeType();
				LineReference.DocumentReference.DocumentType.Value = "ApplicationResponse";

				ExternalReferenceType ExternalReferenceApp = new ExternalReferenceType();
				ExternalReferenceApp.MimeCode = new MimeCodeType();
				ExternalReferenceApp.MimeCode.Value = "text/xml";
				ExternalReferenceApp.EncodingCode = new EncodingCodeType();
				ExternalReferenceApp.EncodingCode.Value = "UTF-8";

				//![CDATA[Acá se coloca el DE en formato xml]]>
				ExternalReferenceApp.Description = new DescriptionType[1];

				// lee el archivo XML en UBL desde la ruta pública
				string contenido_xml_app = Archivo.ObtenerContenido(documentoBd.StrUrlArchivoUbl.Replace("FacturaEDian", "FacturaEConsultaDian"));

				//Se valida y si no tiene contenido se trata de buscar en la respuesta del servicio
				if (string.IsNullOrEmpty(contenido_xml_app))
				{
					
					try
					{
						string nombre_app = Path.GetFileNameWithoutExtension(documentoBd.StrUrlArchivoUbl);

						string appbase64 = string.Empty;

						if (!documentoBd.StrUrlArchivoUbl.Contains("hgidocs.blob"))
						{
							string nombre_archivo_resp = NombramientoArchivo.ObtenerZip(documentoBd.IntNumero.ToString(), obligado.Identificacion, TipoDocumento.Factura, documentoBd.StrPrefijo);

							XmlDocument xDoc = new XmlDocument();

							xDoc.Load(documentoBd.StrUrlArchivoUbl.Replace(nombre_app, nombre_archivo_resp));

							if (xDoc != null)
							{
								XmlNodeList xAttach = xDoc.GetElementsByTagName("DianResponse");

								foreach (XmlNode child in xAttach)
								{
									appbase64 = child["XmlBase64Bytes"].InnerText;
								}
							}
						}

						if (!string.IsNullOrEmpty(appbase64))
						{
							byte [] archivo_recu = Convert.FromBase64String(appbase64);

							contenido_xml_app = Encoding.UTF8.GetString(archivo_recu);

							//Se guarda esta respuesta en la ruta para que quede disponible en plataforma
							try
							{
								PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;

								Uri url_ruta = new Uri(documentoBd.StrUrlArchivoUbl.Replace("FacturaEDian", "FacturaEConsultaDian"));
								string ruta_p = url_ruta.LocalPath;

								string carpeta_xml = string.Format("{0}{1}", plataforma.RutaDmsFisica, ruta_p.Replace("/", "\\"));
								
								FileStream fs = null;
								Directorio.CrearDirectorio(carpeta_xml);
								using (fs = new FileStream(string.Format(@"{0}\{1}.xml", carpeta_xml, nombre_app),
									FileMode.Create, FileAccess.ReadWrite))
								{
									BinaryWriter bw = new BinaryWriter(fs, Encoding.Unicode);
									bw.Write(appbase64);
									bw.Close();
									fs.Close();
								}
							}
							catch (Exception)
							{

							}
						}

					}
					catch (Exception ex)
					{
						
					}


				}

				if (evento_radian > 0)
				{
					string nombre_evento = Path.GetFileNameWithoutExtension(url_evento);
					contenido_xml_app = Archivo.ObtenerContenido(url_evento.Replace(nombre_evento,string.Format("{0}-{1}",nombre_evento,evento_radian)));
				}

				Description = new DescriptionType();
				//Description.Value = string.Format("<![CDATA[{0}]]>", contenido_xml_app);
				Description.Value = "contenido_ApplicationResponse";

				ExternalReferenceApp.Description[0] = Description;
				LineReference.DocumentReference.Attachment = new AttachmentType();
				LineReference.DocumentReference.Attachment.ExternalReference = ExternalReferenceApp;

				ResultOfVerificationType ResultOfVerification = new ResultOfVerificationType();
				ResultOfVerification.ValidatorID = new ValidatorIDType();
				ResultOfVerification.ValidatorID.Value = "Unidad Especial Dirección de Impuestos y Aduanas Nacionales";

				/* caso 550364
				string contenido_resp = String.Empty;
				XmlReader xml_reader = null;
				XmlSerializer serializacion = null;
				List<DianResponse> respuesta = null;

				if (ambiente.Equals("2"))
				{
					string ruta = documentoBd.StrUrlArchivoUbl.Replace("FacturaEDian", "FacturaEConsultaDian");
					ruta = ruta.Replace(".xml", "-WS.xml");
					contenido_resp = Archivo.ObtenerContenido(ruta);
					// convierte el contenido de texto a xml
					xml_reader = XmlReader.Create(new StringReader(contenido_resp));
					// convierte el objeto de acuerdo con el tipo de documento
					serializacion = new XmlSerializer(typeof(List<DianResponse>));
					respuesta = (List<DianResponse>)serializacion.Deserialize(xml_reader);
				}
				else
				{
					contenido_resp = Archivo.ObtenerContenido(documentoBd.StrUrlArchivoUbl.Replace("face", "ws"));
					xml_reader = XmlReader.Create(new StringReader(contenido_resp));
					serializacion = new XmlSerializer(typeof(DianResponse));
					// convierte el objeto de acuerdo con el tipo de documento
					DianResponse resp_prod = (DianResponse)serializacion.Deserialize(xml_reader);
					respuesta = new List<DianResponse>();
					respuesta.Add(resp_prod);
				}*/

				ResultOfVerification.ValidationResultCode = new ValidationResultCodeType();
				ResultOfVerification.ValidationResultCode.Value = "02";//respuesta[0].StatusCode;

				ResultOfVerification.ValidationDate = new ValidationDateType();
				ResultOfVerification.ValidationDate.Value = Convert.ToDateTime(documentoBd.DatFechaIngreso.ToString(Fecha.formato_fecha_hginet));//Convert.ToDateTime(creacion_respuesta.ToString(Fecha.formato_fecha_hginet));

				ResultOfVerification.ValidationTime = new ValidationTimeType();
				if (evento_radian == 0)
					ResultOfVerification.ValidationTime.Value = documentoBd.DatFechaIngreso.AddHours(5).AddMinutes(1).ToString(Fecha.formato_hora_zona);//creacion_respuesta.AddHours(5).ToString(Fecha.formato_hora_zona);
				else
					ResultOfVerification.ValidationTime.Value = Fecha.GetFecha().AddHours(5).AddMinutes(1).ToString(Fecha.formato_hora_zona);//creacion_respuesta.AddHours(5).ToString(Fecha.formato_hora_zona);

				LineReference.DocumentReference.ResultOfVerification = ResultOfVerification;

				attached.ParentDocumentLineReference[0] = LineReference;

				XmlDocument doc = new XmlDocument();
				doc.LoadXml("<firma></firma>");

				List<UBLExtensionType> UBLExtensions = new List<UBLExtensionType>();

				// Extension de la firma
				UBLExtensionType UBLExtensionFirma = new UBLExtensionType();
				UBLExtensionFirma.ExtensionContent = doc.DocumentElement;
				UBLExtensions.Add(UBLExtensionFirma);

				attached.UBLExtensions = UBLExtensions.ToArray();

				// convierte los datos del objeto en texto XML 
				StringBuilder txt_xml = ConvertirXML.Convertir(attached, namespaces_xml, TipoDocumento.Attached);

				// valida el namespace xmlns:schemaLocation y lo reemplaza para Google Chrome
				TextReader textReader = new StringReader(txt_xml.ToString());
				string texto_xml = textReader.ReadToEnd();

				if (texto_xml.Contains("xmlns:schemaLocation"))
				{
					texto_xml = texto_xml.Replace("xmlns:schemaLocation", "xsi:schemaLocation");
				}

				if (texto_xml.Contains("&lt;") || texto_xml.Contains("&gt;"))
				{
					texto_xml = texto_xml.Replace("&lt;", "<");
					texto_xml = texto_xml.Replace("&gt;", ">");
				}

				texto_xml = texto_xml.Replace("contenido_xml", string.Format("<![CDATA[{0}]]>", contenido_xml));
				texto_xml = texto_xml.Replace("contenido_ApplicationResponse", string.Format("<![CDATA[{0}]]>", contenido_xml_app));

				txt_xml = new StringBuilder(texto_xml);

				FacturaE_Documento xml_sin_firma = new FacturaE_Documento();
				xml_sin_firma.Documento = documento;
				xml_sin_firma.DocumentoXml = txt_xml;

				return xml_sin_firma;
			}
			catch (Exception excepcion)
			{
				throw excepcion;
			}

		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="empresa"></param>
		/// <returns></returns>
		public static PartyTaxSchemeType[] ObtenerTercero(Tercero empresa)
		{
			PartyTaxSchemeType[] generador = new PartyTaxSchemeType[1];
			PartyTaxSchemeType PartyTaxScheme = new PartyTaxSchemeType();

			PartyTaxScheme.RegistrationName = new RegistrationNameType();
			PartyTaxScheme.RegistrationName.Value = empresa.RazonSocial;

			CompanyIDType CompanyID = new CompanyIDType();
			CompanyID.Value = empresa.Identificacion.ToString();
			CompanyID.schemeID = empresa.IdentificacionDv.ToString();
			CompanyID.schemeName = empresa.TipoIdentificacion.ToString();
			CompanyID.schemeAgencyID = "195";
			CompanyID.schemeAgencyName = "CO, DIAN (Dirección de Impuestos y Aduanas Nacionales)";
			PartyTaxScheme.CompanyID = CompanyID;

			TaxLevelCodeType TaxLevelCode = new TaxLevelCodeType();
			TaxLevelCode.listName = empresa.RegimenFiscal;

			if (empresa.Responsabilidades != null)
			{
				string list_responsabilidades = LibreriaGlobalHGInet.Formato.Coleccion.ConvertListToString(empresa.Responsabilidades, ";");
				TaxLevelCode.Value = list_responsabilidades;
				PartyTaxScheme.TaxLevelCode = TaxLevelCode;
			}

			TaxSchemeType TaxScheme = new TaxSchemeType();
			TaxScheme.ID = new IDType();
			TaxScheme.ID.Value = empresa.CodigoTributo;
			ListaTipoImpuestoTercero list_tipoImp = new ListaTipoImpuestoTercero();
			ListaItem tipoimp = list_tipoImp.Items.Where(d => d.Codigo.Equals(empresa.CodigoTributo)).FirstOrDefault();
			if (tipoimp == null)
			{
				tipoimp = list_tipoImp.Items.Where(d => d.Codigo.Equals("01")).FirstOrDefault();
			}
			TaxScheme.Name = new NameType1();
			TaxScheme.Name.Value = tipoimp.Nombre;
			PartyTaxScheme.TaxScheme = TaxScheme;
			generador[0] = PartyTaxScheme;

			return generador;
		}


	}
}
