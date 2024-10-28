/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://docs.oasis-open.org/codelist/ns/genericode/1.0/")]
public partial class DataRestrictions {
    
	private DatatypeFacet[] parameterField;
    
	private string langField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("Parameter", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
	public DatatypeFacet[] Parameter {
		get {
			return this.parameterField;
		}
		set {
			this.parameterField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="language")]
	public string Lang {
		get {
			return this.langField;
		}
		set {
			this.langField = value;
		}
	}
}