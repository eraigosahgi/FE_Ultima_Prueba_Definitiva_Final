/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://uri.etsi.org/01903/v1.3.2#")]
public partial class DocumentationReferencesType {
    
	private string[] documentationReferenceField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("DocumentationReference", DataType="anyURI")]
	public string[] DocumentationReference {
		get {
			return this.documentationReferenceField;
		}
		set {
			this.documentationReferenceField = value;
		}
	}
}