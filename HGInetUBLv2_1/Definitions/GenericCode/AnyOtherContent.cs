/// <comentarios/>
[System.Xml.Serialization.XmlIncludeAttribute(typeof(AnyOtherLanguageContent))]
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://docs.oasis-open.org/codelist/ns/genericode/1.0/")]
public partial class AnyOtherContent {
    
	private System.Xml.XmlElement[] anyField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAnyElementAttribute()]
	public System.Xml.XmlElement[] Any {
		get {
			return this.anyField;
		}
		set {
			this.anyField = value;
		}
	}
}