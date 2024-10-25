/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("OrderedShipment", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class OrderedShipmentType {
    
	private ShipmentType shipmentField;
    
	private PackageType[] packageField;
    
	/// <comentarios/>
	public ShipmentType Shipment {
		get {
			return this.shipmentField;
		}
		set {
			this.shipmentField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("Package")]
	public PackageType[] Package {
		get {
			return this.packageField;
		}
		set {
			this.packageField = value;
		}
	}
}