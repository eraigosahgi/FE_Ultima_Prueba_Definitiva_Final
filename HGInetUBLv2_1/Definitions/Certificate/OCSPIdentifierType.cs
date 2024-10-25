/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://uri.etsi.org/01903/v1.3.2#")]
public partial class OCSPIdentifierType {
    
	private ResponderIDType responderIDField;
    
	private System.DateTime producedAtField;
    
	private string uRIField;
    
	/// <comentarios/>
	public ResponderIDType ResponderID {
		get {
			return this.responderIDField;
		}
		set {
			this.responderIDField = value;
		}
	}
    
	/// <comentarios/>
	public System.DateTime ProducedAt {
		get {
			return this.producedAtField;
		}
		set {
			this.producedAtField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="anyURI")]
	public string URI {
		get {
			return this.uRIField;
		}
		set {
			this.uRIField = value;
		}
	}
}