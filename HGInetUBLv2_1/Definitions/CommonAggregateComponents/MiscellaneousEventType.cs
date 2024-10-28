/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("MiscellaneousEvent", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class MiscellaneousEventType {
    
	private MiscellaneousEventTypeCodeType miscellaneousEventTypeCodeField;
    
	private EventLineItemType[] eventLineItemField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public MiscellaneousEventTypeCodeType MiscellaneousEventTypeCode {
		get {
			return this.miscellaneousEventTypeCodeField;
		}
		set {
			this.miscellaneousEventTypeCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("EventLineItem")]
	public EventLineItemType[] EventLineItem {
		get {
			return this.eventLineItemField;
		}
		set {
			this.eventLineItemField = value;
		}
	}
}