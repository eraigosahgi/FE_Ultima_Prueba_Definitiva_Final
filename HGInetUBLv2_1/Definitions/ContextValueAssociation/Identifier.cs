/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://docs.oasis-open.org/codelist/ns/ContextValueAssociation/1.0/")]
public partial class Identifier {
    
	private string langField;
    
	private string identifier1Field;
    
	private string valueField;
    
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
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute("Identifier", DataType="normalizedString")]
	public string Identifier1 {
		get {
			return this.identifier1Field;
		}
		set {
			this.identifier1Field = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlTextAttribute(DataType="normalizedString")]
	public string Value {
		get {
			return this.valueField;
		}
		set {
			this.valueField = value;
		}
	}
}