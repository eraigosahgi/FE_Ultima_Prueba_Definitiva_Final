/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("AdditionalDocumentReference", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class DocumentReferenceType {
    
	private IDType idField;
    
	private CopyIndicatorType copyIndicatorField;
    
	private UUIDType uUIDField;
    
	private IssueDateType issueDateField;
    
	private IssueTimeType issueTimeField;
    
	private DocumentTypeCodeType documentTypeCodeField;
    
	private DocumentTypeType documentTypeField;
    
	private XPathType[] xPathField;
    
	private LanguageIDType languageIDField;
    
	private LocaleCodeType localeCodeField;
    
	private VersionIDType versionIDField;
    
	private DocumentStatusCodeType documentStatusCodeField;
    
	private DocumentDescriptionType[] documentDescriptionField;
    
	private AttachmentType attachmentField;
    
	private PeriodType validityPeriodField;
    
	private PartyType issuerPartyField;
    
	private ResultOfVerificationType resultOfVerificationField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public IDType ID {
		get {
			return this.idField;
		}
		set {
			this.idField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public CopyIndicatorType CopyIndicator {
		get {
			return this.copyIndicatorField;
		}
		set {
			this.copyIndicatorField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public UUIDType UUID {
		get {
			return this.uUIDField;
		}
		set {
			this.uUIDField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public IssueDateType IssueDate {
		get {
			return this.issueDateField;
		}
		set {
			this.issueDateField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public IssueTimeType IssueTime {
		get {
			return this.issueTimeField;
		}
		set {
			this.issueTimeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public DocumentTypeCodeType DocumentTypeCode {
		get {
			return this.documentTypeCodeField;
		}
		set {
			this.documentTypeCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public DocumentTypeType DocumentType {
		get {
			return this.documentTypeField;
		}
		set {
			this.documentTypeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("XPath", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public XPathType[] XPath {
		get {
			return this.xPathField;
		}
		set {
			this.xPathField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public LanguageIDType LanguageID {
		get {
			return this.languageIDField;
		}
		set {
			this.languageIDField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public LocaleCodeType LocaleCode {
		get {
			return this.localeCodeField;
		}
		set {
			this.localeCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public VersionIDType VersionID {
		get {
			return this.versionIDField;
		}
		set {
			this.versionIDField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public DocumentStatusCodeType DocumentStatusCode {
		get {
			return this.documentStatusCodeField;
		}
		set {
			this.documentStatusCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("DocumentDescription", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public DocumentDescriptionType[] DocumentDescription {
		get {
			return this.documentDescriptionField;
		}
		set {
			this.documentDescriptionField = value;
		}
	}
    
	/// <comentarios/>
	public AttachmentType Attachment {
		get {
			return this.attachmentField;
		}
		set {
			this.attachmentField = value;
		}
	}
    
	/// <comentarios/>
	public PeriodType ValidityPeriod {
		get {
			return this.validityPeriodField;
		}
		set {
			this.validityPeriodField = value;
		}
	}
    
	/// <comentarios/>
	public PartyType IssuerParty {
		get {
			return this.issuerPartyField;
		}
		set {
			this.issuerPartyField = value;
		}
	}
    
	/// <comentarios/>
	public ResultOfVerificationType ResultOfVerification {
		get {
			return this.resultOfVerificationField;
		}
		set {
			this.resultOfVerificationField = value;
		}
	}
}