/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("TradeFinancing", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class TradeFinancingType {
    
	private IDType idField;
    
	private FinancingInstrumentCodeType financingInstrumentCodeField;
    
	private DocumentReferenceType contractDocumentReferenceField;
    
	private DocumentReferenceType[] documentReferenceField;
    
	private PartyType financingPartyField;
    
	private FinancialAccountType financingFinancialAccountField;
    
	private ClauseType[] clauseField;
    
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
	public FinancingInstrumentCodeType FinancingInstrumentCode {
		get {
			return this.financingInstrumentCodeField;
		}
		set {
			this.financingInstrumentCodeField = value;
		}
	}
    
	/// <comentarios/>
	public DocumentReferenceType ContractDocumentReference {
		get {
			return this.contractDocumentReferenceField;
		}
		set {
			this.contractDocumentReferenceField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("DocumentReference")]
	public DocumentReferenceType[] DocumentReference {
		get {
			return this.documentReferenceField;
		}
		set {
			this.documentReferenceField = value;
		}
	}
    
	/// <comentarios/>
	public PartyType FinancingParty {
		get {
			return this.financingPartyField;
		}
		set {
			this.financingPartyField = value;
		}
	}
    
	/// <comentarios/>
	public FinancialAccountType FinancingFinancialAccount {
		get {
			return this.financingFinancialAccountField;
		}
		set {
			this.financingFinancialAccountField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("Clause")]
	public ClauseType[] Clause {
		get {
			return this.clauseField;
		}
		set {
			this.clauseField = value;
		}
	}
}