/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://uri.etsi.org/01903/v1.3.2#")]
public partial class CRLRefType {
    
	private DigestAlgAndValueType digestAlgAndValueField;
    
	private CRLIdentifierType cRLIdentifierField;
    
	/// <comentarios/>
	public DigestAlgAndValueType DigestAlgAndValue {
		get {
			return this.digestAlgAndValueField;
		}
		set {
			this.digestAlgAndValueField = value;
		}
	}
    
	/// <comentarios/>
	public CRLIdentifierType CRLIdentifier {
		get {
			return this.cRLIdentifierField;
		}
		set {
			this.cRLIdentifierField = value;
		}
	}
}