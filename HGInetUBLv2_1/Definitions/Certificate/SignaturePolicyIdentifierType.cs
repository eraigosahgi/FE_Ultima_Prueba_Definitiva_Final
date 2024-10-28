/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://uri.etsi.org/01903/v1.3.2#")]
[System.Xml.Serialization.XmlRootAttribute("SignaturePolicyIdentifier", Namespace="http://uri.etsi.org/01903/v1.3.2#", IsNullable=false)]
public partial class SignaturePolicyIdentifierType {
    
	private object itemField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("SignaturePolicyId", typeof(SignaturePolicyIdType))]
	[System.Xml.Serialization.XmlElementAttribute("SignaturePolicyImplied", typeof(object))]
	public object Item {
		get {
			return this.itemField;
		}
		set {
			this.itemField = value;
		}
	}
}