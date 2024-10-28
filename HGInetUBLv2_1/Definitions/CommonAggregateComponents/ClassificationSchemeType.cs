/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("BusinessClassificationScheme", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class ClassificationSchemeType {
    
	private IDType idField;
    
	private UUIDType uUIDField;
    
	private LastRevisionDateType lastRevisionDateField;
    
	private LastRevisionTimeType lastRevisionTimeField;
    
	private NoteType[] noteField;
    
	private NameType1 nameField;
    
	private DescriptionType[] descriptionField;
    
	private AgencyIDType agencyIDField;
    
	private AgencyNameType agencyNameField;
    
	private VersionIDType versionIDField;
    
	private URIType uRIField;
    
	private SchemeURIType schemeURIField;
    
	private LanguageIDType languageIDField;
    
	private ClassificationCategoryType[] classificationCategoryField;
    
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
	public LastRevisionDateType LastRevisionDate {
		get {
			return this.lastRevisionDateField;
		}
		set {
			this.lastRevisionDateField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public LastRevisionTimeType LastRevisionTime {
		get {
			return this.lastRevisionTimeField;
		}
		set {
			this.lastRevisionTimeField = value;
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
	public NameType1 Name {
		get {
			return this.nameField;
		}
		set {
			this.nameField = value;
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
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public AgencyIDType AgencyID {
		get {
			return this.agencyIDField;
		}
		set {
			this.agencyIDField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public AgencyNameType AgencyName {
		get {
			return this.agencyNameField;
		}
		set {
			this.agencyNameField = value;
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
	public SchemeURIType SchemeURI {
		get {
			return this.schemeURIField;
		}
		set {
			this.schemeURIField = value;
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
	[System.Xml.Serialization.XmlElementAttribute("ClassificationCategory")]
	public ClassificationCategoryType[] ClassificationCategory {
		get {
			return this.classificationCategoryField;
		}
		set {
			this.classificationCategoryField = value;
		}
	}
}