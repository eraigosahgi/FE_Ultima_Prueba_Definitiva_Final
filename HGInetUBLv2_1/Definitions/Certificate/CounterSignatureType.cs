/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://uri.etsi.org/01903/v1.3.2#")]
[System.Xml.Serialization.XmlRootAttribute("CounterSignature", Namespace="http://uri.etsi.org/01903/v1.3.2#", IsNullable=false)]
public partial class CounterSignatureType {
    
	private SignatureType1 signatureField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="http://www.w3.org/2000/09/xmldsig#")]
	public SignatureType1 Signature {
		get {
			return this.signatureField;
		}
		set {
			this.signatureField = value;
		}
	}
}