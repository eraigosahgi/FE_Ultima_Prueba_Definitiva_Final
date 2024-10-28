/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://uri.etsi.org/01903/v1.3.2#")]
public partial class CRLIdentifierType {
    
	private string issuerField;
    
	private System.DateTime issueTimeField;
    
	private string numberField;
    
	private string uRIField;
    
	/// <comentarios/>
	public string Issuer {
		get {
			return this.issuerField;
		}
		set {
			this.issuerField = value;
		}
	}
    
	/// <comentarios/>
	public System.DateTime IssueTime {
		get {
			return this.issueTimeField;
		}
		set {
			this.issueTimeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(DataType="integer")]
	public string Number {
		get {
			return this.numberField;
		}
		set {
			this.numberField = value;
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