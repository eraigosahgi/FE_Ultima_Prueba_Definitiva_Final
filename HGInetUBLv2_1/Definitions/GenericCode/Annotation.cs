/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://docs.oasis-open.org/codelist/ns/genericode/1.0/")]
public partial class Annotation {
    
	private AnyOtherLanguageContent[] descriptionField;
    
	private AnyOtherContent appInfoField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("Description", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
	public AnyOtherLanguageContent[] Description {
		get {
			return this.descriptionField;
		}
		set {
			this.descriptionField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
	public AnyOtherContent AppInfo {
		get {
			return this.appInfoField;
		}
		set {
			this.appInfoField = value;
		}
	}
}