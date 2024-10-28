/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.w3.org/2000/09/xmldsig#")]
[System.Xml.Serialization.XmlRootAttribute("RSAKeyValue", Namespace="http://www.w3.org/2000/09/xmldsig#", IsNullable=false)]
public partial class RSAKeyValueType {
    
	private byte[] modulusField;
    
	private byte[] exponentField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")]
	public byte[] Modulus {
		get {
			return this.modulusField;
		}
		set {
			this.modulusField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")]
	public byte[] Exponent {
		get {
			return this.exponentField;
		}
		set {
			this.exponentField = value;
		}
	}
}