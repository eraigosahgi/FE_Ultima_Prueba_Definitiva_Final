/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("EventTacticEnumeration", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class EventTacticEnumerationType {
    
	private ConsumerIncentiveTacticTypeCodeType consumerIncentiveTacticTypeCodeField;
    
	private DisplayTacticTypeCodeType displayTacticTypeCodeField;
    
	private FeatureTacticTypeCodeType featureTacticTypeCodeField;
    
	private TradeItemPackingLabelingTypeCodeType tradeItemPackingLabelingTypeCodeField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ConsumerIncentiveTacticTypeCodeType ConsumerIncentiveTacticTypeCode {
		get {
			return this.consumerIncentiveTacticTypeCodeField;
		}
		set {
			this.consumerIncentiveTacticTypeCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public DisplayTacticTypeCodeType DisplayTacticTypeCode {
		get {
			return this.displayTacticTypeCodeField;
		}
		set {
			this.displayTacticTypeCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public FeatureTacticTypeCodeType FeatureTacticTypeCode {
		get {
			return this.featureTacticTypeCodeField;
		}
		set {
			this.featureTacticTypeCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public TradeItemPackingLabelingTypeCodeType TradeItemPackingLabelingTypeCode {
		get {
			return this.tradeItemPackingLabelingTypeCodeField;
		}
		set {
			this.tradeItemPackingLabelingTypeCodeField = value;
		}
	}
}