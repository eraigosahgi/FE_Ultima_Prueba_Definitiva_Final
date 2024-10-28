/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("ExternalReference", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class ExternalReferenceType {
    
	private URIType uRIField;
    
	private DocumentHashType documentHashField;
    
	private HashAlgorithmMethodType hashAlgorithmMethodField;
    
	private ExpiryDateType expiryDateField;
    
	private ExpiryTimeType expiryTimeField;
    
	private MimeCodeType mimeCodeField;
    
	private FormatCodeType formatCodeField;
    
	private EncodingCodeType encodingCodeField;
    
	private CharacterSetCodeType characterSetCodeField;
    
	private FileNameType fileNameField;
    
	private DescriptionType[] descriptionField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public URIType URI {
		get {
			return this.uRIField;
		}
		set {
			this.uRIField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public DocumentHashType DocumentHash {
		get {
			return this.documentHashField;
		}
		set {
			this.documentHashField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public HashAlgorithmMethodType HashAlgorithmMethod {
		get {
			return this.hashAlgorithmMethodField;
		}
		set {
			this.hashAlgorithmMethodField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ExpiryDateType ExpiryDate {
		get {
			return this.expiryDateField;
		}
		set {
			this.expiryDateField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ExpiryTimeType ExpiryTime {
		get {
			return this.expiryTimeField;
		}
		set {
			this.expiryTimeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public MimeCodeType MimeCode {
		get {
			return this.mimeCodeField;
		}
		set {
			this.mimeCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public FormatCodeType FormatCode {
		get {
			return this.formatCodeField;
		}
		set {
			this.formatCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public EncodingCodeType EncodingCode {
		get {
			return this.encodingCodeField;
		}
		set {
			this.encodingCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public CharacterSetCodeType CharacterSetCode {
		get {
			return this.characterSetCodeField;
		}
		set {
			this.characterSetCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public FileNameType FileName {
		get {
			return this.fileNameField;
		}
		set {
			this.fileNameField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("Description", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public DescriptionType[] Description {
		get {
			return this.descriptionField;
		}
		set {
			this.descriptionField = value;
		}
	}
}