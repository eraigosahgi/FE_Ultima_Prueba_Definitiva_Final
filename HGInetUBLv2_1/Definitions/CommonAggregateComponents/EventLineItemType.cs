/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("EventLineItem", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class EventLineItemType {
    
	private LineNumberNumericType lineNumberNumericField;
    
	private LocationType1 participatingLocationsLocationField;
    
	private RetailPlannedImpactType[] retailPlannedImpactField;
    
	private ItemType supplyItemField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public LineNumberNumericType LineNumberNumeric {
		get {
			return this.lineNumberNumericField;
		}
		set {
			this.lineNumberNumericField = value;
		}
	}
    
	/// <comentarios/>
	public LocationType1 ParticipatingLocationsLocation {
		get {
			return this.participatingLocationsLocationField;
		}
		set {
			this.participatingLocationsLocationField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("RetailPlannedImpact")]
	public RetailPlannedImpactType[] RetailPlannedImpact {
		get {
			return this.retailPlannedImpactField;
		}
		set {
			this.retailPlannedImpactField = value;
		}
	}
    
	/// <comentarios/>
	public ItemType SupplyItem {
		get {
			return this.supplyItemField;
		}
		set {
			this.supplyItemField = value;
		}
	}
}