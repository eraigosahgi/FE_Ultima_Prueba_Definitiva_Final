/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("AwardingCriterionResponse", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class AwardingCriterionResponseType {
    
	private IDType idField;
    
	private AwardingCriterionIDType awardingCriterionIDField;
    
	private AwardingCriterionDescriptionType[] awardingCriterionDescriptionField;
    
	private DescriptionType[] descriptionField;
    
	private QuantityType2 quantityField;
    
	private AmountType2 amountField;
    
	private AwardingCriterionResponseType[] subordinateAwardingCriterionResponseField;
    
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
	public AwardingCriterionIDType AwardingCriterionID {
		get {
			return this.awardingCriterionIDField;
		}
		set {
			this.awardingCriterionIDField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("AwardingCriterionDescription", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public AwardingCriterionDescriptionType[] AwardingCriterionDescription {
		get {
			return this.awardingCriterionDescriptionField;
		}
		set {
			this.awardingCriterionDescriptionField = value;
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
	public QuantityType2 Quantity {
		get {
			return this.quantityField;
		}
		set {
			this.quantityField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public AmountType2 Amount {
		get {
			return this.amountField;
		}
		set {
			this.amountField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("SubordinateAwardingCriterionResponse")]
	public AwardingCriterionResponseType[] SubordinateAwardingCriterionResponse {
		get {
			return this.subordinateAwardingCriterionResponseField;
		}
		set {
			this.subordinateAwardingCriterionResponseField = value;
		}
	}
}