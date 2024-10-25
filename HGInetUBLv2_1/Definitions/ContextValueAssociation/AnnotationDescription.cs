/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://docs.oasis-open.org/codelist/ns/ContextValueAssociation/1.0/")]
public partial class AnnotationDescription {
    
	private System.Xml.XmlNode[] anyField;
    
	private string langField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlTextAttribute()]
	[System.Xml.Serialization.XmlAnyElementAttribute()]
	public System.Xml.XmlNode[] Any {
		get {
			return this.anyField;
		}
		set {
			this.anyField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(Form=System.Xml.Schema.XmlSchemaForm.Qualified, Namespace="http://www.w3.org/XML/1998/namespace")]
	public string lang {
		get {
			return this.langField;
		}
		set {
			this.langField = value;
		}
	}
}