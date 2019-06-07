/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("PromotionalSpecification", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class PromotionalSpecificationType {
    
	private SpecificationIDType specificationIDField;
    
	private PromotionalEventLineItemType[] promotionalEventLineItemField;
    
	private EventTacticType[] eventTacticField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public SpecificationIDType SpecificationID {
		get {
			return this.specificationIDField;
		}
		set {
			this.specificationIDField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("PromotionalEventLineItem")]
	public PromotionalEventLineItemType[] PromotionalEventLineItem {
		get {
			return this.promotionalEventLineItemField;
		}
		set {
			this.promotionalEventLineItemField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("EventTactic")]
	public EventTacticType[] EventTactic {
		get {
			return this.eventTacticField;
		}
		set {
			this.eventTacticField = value;
		}
	}
}