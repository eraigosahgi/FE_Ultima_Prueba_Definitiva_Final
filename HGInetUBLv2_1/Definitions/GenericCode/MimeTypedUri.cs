/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://docs.oasis-open.org/codelist/ns/genericode/1.0/")]
public partial class MimeTypedUri {
    
	private string mimeTypeField;
    
	private string valueField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="normalizedString")]
	public string MimeType {
		get {
			return this.mimeTypeField;
		}
		set {
			this.mimeTypeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlTextAttribute(DataType="anyURI")]
	public string Value {
		get {
			return this.valueField;
		}
		set {
			this.valueField = value;
		}
	}
}