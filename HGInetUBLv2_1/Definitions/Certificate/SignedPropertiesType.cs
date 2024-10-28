/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://uri.etsi.org/01903/v1.3.2#")]
[System.Xml.Serialization.XmlRootAttribute("SignedProperties", Namespace="http://uri.etsi.org/01903/v1.3.2#", IsNullable=false)]
public partial class SignedPropertiesType {
    
	private SignedSignaturePropertiesType signedSignaturePropertiesField;
    
	private SignedDataObjectPropertiesType signedDataObjectPropertiesField;
    
	private string idField;
    
	/// <comentarios/>
	public SignedSignaturePropertiesType SignedSignatureProperties {
		get {
			return this.signedSignaturePropertiesField;
		}
		set {
			this.signedSignaturePropertiesField = value;
		}
	}
    
	/// <comentarios/>
	public SignedDataObjectPropertiesType SignedDataObjectProperties {
		get {
			return this.signedDataObjectPropertiesField;
		}
		set {
			this.signedDataObjectPropertiesField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="ID")]
	public string Id {
		get {
			return this.idField;
		}
		set {
			this.idField = value;
		}
	}
}