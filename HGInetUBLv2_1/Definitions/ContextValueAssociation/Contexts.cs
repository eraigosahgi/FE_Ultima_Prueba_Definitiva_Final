/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://docs.oasis-open.org/codelist/ns/ContextValueAssociation/1.0/")]
public partial class Contexts {
    
	private Annotation annotationField;
    
	private Context[] contextField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
	public Annotation Annotation {
		get {
			return this.annotationField;
		}
		set {
			this.annotationField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("Context", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
	public Context[] Context {
		get {
			return this.contextField;
		}
		set {
			this.contextField = value;
		}
	}
}