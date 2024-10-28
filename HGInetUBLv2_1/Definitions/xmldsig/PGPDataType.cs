/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.w3.org/2000/09/xmldsig#")]
[System.Xml.Serialization.XmlRootAttribute("PGPData", Namespace="http://www.w3.org/2000/09/xmldsig#", IsNullable=false)]
public partial class PGPDataType {
    
	private object[] itemsField;
    
	private ItemsChoiceType1[] itemsElementNameField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAnyElementAttribute()]
	[System.Xml.Serialization.XmlElementAttribute("PGPKeyID", typeof(byte[]), DataType="base64Binary")]
	[System.Xml.Serialization.XmlElementAttribute("PGPKeyPacket", typeof(byte[]), DataType="base64Binary")]
	[System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
	public object[] Items {
		get {
			return this.itemsField;
		}
		set {
			this.itemsField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
	[System.Xml.Serialization.XmlIgnoreAttribute()]
	public ItemsChoiceType1[] ItemsElementName {
		get {
			return this.itemsElementNameField;
		}
		set {
			this.itemsElementNameField = value;
		}
	}
}