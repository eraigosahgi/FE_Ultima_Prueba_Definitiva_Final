/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("Contract", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class ContractType {
    
	private IDType idField;
    
	private IssueDateType issueDateField;
    
	private IssueTimeType issueTimeField;
    
	private NominationDateType nominationDateField;
    
	private NominationTimeType nominationTimeField;
    
	private ContractTypeCodeType contractTypeCodeField;
    
	private ContractTypeType contractType1Field;
    
	private NoteType[] noteField;
    
	private VersionIDType versionIDField;
    
	private DescriptionType[] descriptionField;
    
	private PeriodType validityPeriodField;
    
	private DocumentReferenceType[] contractDocumentReferenceField;
    
	private PeriodType nominationPeriodField;
    
	private DeliveryType contractualDeliveryField;
    
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
	public NominationDateType NominationDate {
		get {
			return this.nominationDateField;
		}
		set {
			this.nominationDateField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public NominationTimeType NominationTime {
		get {
			return this.nominationTimeField;
		}
		set {
			this.nominationTimeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ContractTypeCodeType ContractTypeCode {
		get {
			return this.contractTypeCodeField;
		}
		set {
			this.contractTypeCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("ContractType", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ContractTypeType ContractType1 {
		get {
			return this.contractType1Field;
		}
		set {
			this.contractType1Field = value;
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
	public VersionIDType VersionID {
		get {
			return this.versionIDField;
		}
		set {
			this.versionIDField = value;
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
	public PeriodType ValidityPeriod {
		get {
			return this.validityPeriodField;
		}
		set {
			this.validityPeriodField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("ContractDocumentReference")]
	public DocumentReferenceType[] ContractDocumentReference {
		get {
			return this.contractDocumentReferenceField;
		}
		set {
			this.contractDocumentReferenceField = value;
		}
	}
    
	/// <comentarios/>
	public PeriodType NominationPeriod {
		get {
			return this.nominationPeriodField;
		}
		set {
			this.nominationPeriodField = value;
		}
	}
    
	/// <comentarios/>
	public DeliveryType ContractualDelivery {
		get {
			return this.contractualDeliveryField;
		}
		set {
			this.contractualDeliveryField = value;
		}
	}
}