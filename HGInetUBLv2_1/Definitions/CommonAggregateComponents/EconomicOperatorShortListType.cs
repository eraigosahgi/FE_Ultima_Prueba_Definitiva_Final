/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("EconomicOperatorShortList", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class EconomicOperatorShortListType {
    
	private LimitationDescriptionType[] limitationDescriptionField;
    
	private ExpectedQuantityType expectedQuantityField;
    
	private MaximumQuantityType maximumQuantityField;
    
	private MinimumQuantityType minimumQuantityField;
    
	private PartyType[] preSelectedPartyField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("LimitationDescription", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public LimitationDescriptionType[] LimitationDescription {
		get {
			return this.limitationDescriptionField;
		}
		set {
			this.limitationDescriptionField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ExpectedQuantityType ExpectedQuantity {
		get {
			return this.expectedQuantityField;
		}
		set {
			this.expectedQuantityField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public MaximumQuantityType MaximumQuantity {
		get {
			return this.maximumQuantityField;
		}
		set {
			this.maximumQuantityField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public MinimumQuantityType MinimumQuantity {
		get {
			return this.minimumQuantityField;
		}
		set {
			this.minimumQuantityField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("PreSelectedParty")]
	public PartyType[] PreSelectedParty {
		get {
			return this.preSelectedPartyField;
		}
		set {
			this.preSelectedPartyField = value;
		}
	}
}