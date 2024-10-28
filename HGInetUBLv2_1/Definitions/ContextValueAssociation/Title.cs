/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://docs.oasis-open.org/codelist/ns/ContextValueAssociation/1.0/")]
public partial class Title {
    
	private System.Xml.XmlNode[] anyField;
    
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
}