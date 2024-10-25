/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("ContractingParty", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class ContractingPartyType {
    
	private BuyerProfileURIType buyerProfileURIField;
    
	private ContractingPartyTypeType[] contractingPartyType1Field;
    
	private ContractingActivityType[] contractingActivityField;
    
	private PartyType partyField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public BuyerProfileURIType BuyerProfileURI {
		get {
			return this.buyerProfileURIField;
		}
		set {
			this.buyerProfileURIField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("ContractingPartyType")]
	public ContractingPartyTypeType[] ContractingPartyType1 {
		get {
			return this.contractingPartyType1Field;
		}
		set {
			this.contractingPartyType1Field = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("ContractingActivity")]
	public ContractingActivityType[] ContractingActivity {
		get {
			return this.contractingActivityField;
		}
		set {
			this.contractingActivityField = value;
		}
	}
    
	/// <comentarios/>
	public PartyType Party {
		get {
			return this.partyField;
		}
		set {
			this.partyField = value;
		}
	}
}