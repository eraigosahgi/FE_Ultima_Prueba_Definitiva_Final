/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("EndorserParty", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class EndorserPartyType {
    
	private RoleCodeType roleCodeField;
    
	private SequenceNumericType sequenceNumericField;
    
	private PartyType partyField;
    
	private ContactType signatoryContactField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public RoleCodeType RoleCode {
		get {
			return this.roleCodeField;
		}
		set {
			this.roleCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public SequenceNumericType SequenceNumeric {
		get {
			return this.sequenceNumericField;
		}
		set {
			this.sequenceNumericField = value;
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
    
	/// <comentarios/>
	public ContactType SignatoryContact {
		get {
			return this.signatoryContactField;
		}
		set {
			this.signatoryContactField = value;
		}
	}
}