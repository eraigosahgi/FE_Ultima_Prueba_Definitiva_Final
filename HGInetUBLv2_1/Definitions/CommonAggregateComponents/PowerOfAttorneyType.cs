/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("PowerOfAttorney", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class PowerOfAttorneyType {
    
	private IDType idField;
    
	private IssueDateType issueDateField;
    
	private IssueTimeType issueTimeField;
    
	private DescriptionType[] descriptionField;
    
	private PartyType notaryPartyField;
    
	private PartyType agentPartyField;
    
	private PartyType[] witnessPartyField;
    
	private DocumentReferenceType[] mandateDocumentReferenceField;
    
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
	public PartyType NotaryParty {
		get {
			return this.notaryPartyField;
		}
		set {
			this.notaryPartyField = value;
		}
	}
    
	/// <comentarios/>
	public PartyType AgentParty {
		get {
			return this.agentPartyField;
		}
		set {
			this.agentPartyField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("WitnessParty")]
	public PartyType[] WitnessParty {
		get {
			return this.witnessPartyField;
		}
		set {
			this.witnessPartyField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("MandateDocumentReference")]
	public DocumentReferenceType[] MandateDocumentReference {
		get {
			return this.mandateDocumentReferenceField;
		}
		set {
			this.mandateDocumentReferenceField = value;
		}
	}
}