/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("WorkPhaseReference", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class WorkPhaseReferenceType {
    
	private IDType idField;
    
	private WorkPhaseCodeType workPhaseCodeField;
    
	private WorkPhaseType[] workPhaseField;
    
	private ProgressPercentType progressPercentField;
    
	private StartDateType startDateField;
    
	private EndDateType endDateField;
    
	private DocumentReferenceType[] workOrderDocumentReferenceField;
    
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
	public WorkPhaseCodeType WorkPhaseCode {
		get {
			return this.workPhaseCodeField;
		}
		set {
			this.workPhaseCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("WorkPhase", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public WorkPhaseType[] WorkPhase {
		get {
			return this.workPhaseField;
		}
		set {
			this.workPhaseField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ProgressPercentType ProgressPercent {
		get {
			return this.progressPercentField;
		}
		set {
			this.progressPercentField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public StartDateType StartDate {
		get {
			return this.startDateField;
		}
		set {
			this.startDateField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public EndDateType EndDate {
		get {
			return this.endDateField;
		}
		set {
			this.endDateField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("WorkOrderDocumentReference")]
	public DocumentReferenceType[] WorkOrderDocumentReference {
		get {
			return this.workOrderDocumentReferenceField;
		}
		set {
			this.workOrderDocumentReferenceField = value;
		}
	}
}