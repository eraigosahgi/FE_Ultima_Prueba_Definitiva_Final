/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("Evidence", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class EvidenceType {
    
	private IDType idField;
    
	private EvidenceTypeCodeType evidenceTypeCodeField;
    
	private DescriptionType[] descriptionField;
    
	private CandidateStatementType[] candidateStatementField;
    
	private PartyType evidenceIssuingPartyField;
    
	private DocumentReferenceType documentReferenceField;
    
	private LanguageType languageField;
    
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
	public EvidenceTypeCodeType EvidenceTypeCode {
		get {
			return this.evidenceTypeCodeField;
		}
		set {
			this.evidenceTypeCodeField = value;
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
	[System.Xml.Serialization.XmlElementAttribute("CandidateStatement", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public CandidateStatementType[] CandidateStatement {
		get {
			return this.candidateStatementField;
		}
		set {
			this.candidateStatementField = value;
		}
	}
    
	/// <comentarios/>
	public PartyType EvidenceIssuingParty {
		get {
			return this.evidenceIssuingPartyField;
		}
		set {
			this.evidenceIssuingPartyField = value;
		}
	}
    
	/// <comentarios/>
	public DocumentReferenceType DocumentReference {
		get {
			return this.documentReferenceField;
		}
		set {
			this.documentReferenceField = value;
		}
	}
    
	/// <comentarios/>
	public LanguageType Language {
		get {
			return this.languageField;
		}
		set {
			this.languageField = value;
		}
	}
}