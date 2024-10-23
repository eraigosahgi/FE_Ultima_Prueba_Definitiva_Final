/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("Signature", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class SignatureType {
    
	private IDType idField;
    
	private NoteType[] noteField;
    
	private ValidationDateType validationDateField;
    
	private ValidationTimeType validationTimeField;
    
	private ValidatorIDType validatorIDField;
    
	private CanonicalizationMethodType canonicalizationMethodField;
    
	private SignatureMethodType signatureMethodField;
    
	private PartyType signatoryPartyField;
    
	private AttachmentType digitalSignatureAttachmentField;
    
	private DocumentReferenceType originalDocumentReferenceField;
    
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
	[System.Xml.Serialization.XmlElementAttribute("Note", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public NoteType[] Note {
		get {
			return this.noteField;
		}
		set {
			this.noteField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ValidationDateType ValidationDate {
		get {
			return this.validationDateField;
		}
		set {
			this.validationDateField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ValidationTimeType ValidationTime {
		get {
			return this.validationTimeField;
		}
		set {
			this.validationTimeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ValidatorIDType ValidatorID {
		get {
			return this.validatorIDField;
		}
		set {
			this.validatorIDField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public CanonicalizationMethodType CanonicalizationMethod {
		get {
			return this.canonicalizationMethodField;
		}
		set {
			this.canonicalizationMethodField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public SignatureMethodType SignatureMethod {
		get {
			return this.signatureMethodField;
		}
		set {
			this.signatureMethodField = value;
		}
	}
    
	/// <comentarios/>
	public PartyType SignatoryParty {
		get {
			return this.signatoryPartyField;
		}
		set {
			this.signatoryPartyField = value;
		}
	}
    
	/// <comentarios/>
	public AttachmentType DigitalSignatureAttachment {
		get {
			return this.digitalSignatureAttachmentField;
		}
		set {
			this.digitalSignatureAttachmentField = value;
		}
	}
    
	/// <comentarios/>
	public DocumentReferenceType OriginalDocumentReference {
		get {
			return this.originalDocumentReferenceField;
		}
		set {
			this.originalDocumentReferenceField = value;
		}
	}
}