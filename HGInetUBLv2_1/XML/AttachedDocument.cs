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
		public static FacturaE_Documento CrearDocumento(Guid id_documento, Tercero obligado, Tercero adquiriente, string ambiente, TblDocumentos documentoBd)
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

				attached.ProfileID = new ProfileIDType();
				attached.ProfileID.Value = "Factura Electrónica de Venta";

				//---Ambiente al que se va enviar el documento: 1 - Produccion, 2 - Pruebas
				attached.ProfileExecutionID = new ProfileExecutionIDType();
				attached.ProfileExecutionID.Value = ambiente;//"2";

				//Consecutivo del attached
				attached.ID = new IDType();
				attached.ID.Value = id_documento.ToString();

				#region Fecha Generacion del Attached
				IssueDateType IssueDate = new IssueDateType();
				IssueDate.Value = Convert.ToDateTime(documentoBd.DatFechaIngreso.ToString(Fecha.formato_fecha_hginet));
				attached.IssueDate = IssueDate;
				#endregion

				#region Hora Generacion del Attached
				//----Se debe enviar la hora de emision con -5 horas
				IssueTimeType IssueTime = new IssueTimeType();
				IssueTime.Value = documentoBd.DatFechaIngreso.AddHours(5).AddMinutes(2).ToString(Fecha.formato_hora_zona);//Convert.ToDateTime(hora_documento);//Convert.ToDateTime(documento.Fecha.ToString(Fecha.formato_hora_completa)).AddHours(5);//
				attached.IssueTime = IssueTime;
				#endregion

				attached.DocumentTypeCode = new DocumentTypeCodeType();
				attached.DocumentTypeCode.Value = "Contenedor de Factura Electrónica";

				//ID de la factura electrónica que origina el contenedor 
				attached.ParentDocumentID = new ParentDocumentIDType();
				attached.ParentDocumentID.Value = documento;

				//Persona que genera el contenedor 
				attached.SenderParty = new PartyType();
				attached.SenderParty.PartyTaxScheme = ObtenerTercero(obligado);

				//Persona que recibe el contenedor 
				attached.ReceiverParty = new PartyType();
				attached.ReceiverParty.PartyTaxScheme = ObtenerTercero(adquiriente);

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

				DescriptionType Description = new DescriptionType();
				Description.Value = string.Format("<![CDATA[{0}]]>", contenido_xml);

				ExternalReference.Description[0] = Description;
				attached.Attachment.ExternalReference = ExternalReference;


				attached.ParentDocumentLineReference = new LineReferenceType[1];

				LineReferenceType LineReference = new LineReferenceType();
				LineReference.LineID = new LineIDType();
				LineReference.LineID.Value = "1";
				LineReference.DocumentReference = new DocumentReferenceType();
				LineReference.DocumentReference.ID = new IDType();
				LineReference.DocumentReference.ID.Value = documento;
				LineReference.DocumentReference.UUID = new UUIDType();
				LineReference.DocumentReference.UUID.Value = documentoBd.StrCufe;
				if (documentoBd.IntDocTipo == TipoDocumento.Factura.GetHashCode())
				{
					LineReference.DocumentReference.UUID.schemeName = "CUFE-SHA384";
				}
				else
				{
					LineReference.DocumentReference.UUID.schemeName = "CUDE-SHA384";
				}

				LineReference.DocumentReference.IssueDate = new IssueDateType();
				LineReference.DocumentReference.IssueDate.Value = Convert.ToDateTime(documentoBd.DatFechaIngreso.ToString(Fecha.formato_fecha_hginet));
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

				Description = new DescriptionType();
				Description.Value = string.Format("<![CDATA[{0}]]>", contenido_xml_app);

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
				ResultOfVerification.ValidationTime.Value = documentoBd.DatFechaIngreso.AddHours(5).AddMinutes(1).ToString(Fecha.formato_hora_zona);//creacion_respuesta.AddHours(5).ToString(Fecha.formato_hora_zona);

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

				if (texto_xml.Contains("&lt;") && texto_xml.Contains("&gt;"))
				{
					texto_xml = texto_xml.Replace("&lt;", "<");
					texto_xml = texto_xml.Replace("&gt;", ">");
				}
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
