/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("EventTactic", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class EventTacticType {
    
	private CommentType commentField;
    
	private QuantityType2 quantityField;
    
	private EventTacticEnumerationType eventTacticEnumerationField;
    
	private PeriodType periodField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public CommentType Comment {
		get {
			return this.commentField;
		}
		set {
			this.commentField = value;
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
	public EventTacticEnumerationType EventTacticEnumeration {
		get {
			return this.eventTacticEnumerationField;
		}
		set {
			this.eventTacticEnumerationField = value;
		}
	}
    
	/// <comentarios/>
	public PeriodType Period {
		get {
			return this.periodField;
		}
		set {
			this.periodField = value;
		}
	}
}