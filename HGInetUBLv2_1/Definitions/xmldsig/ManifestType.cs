/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.w3.org/2000/09/xmldsig#")]
[System.Xml.Serialization.XmlRootAttribute("Manifest", Namespace="http://www.w3.org/2000/09/xmldsig#", IsNullable=false)]
public partial class ManifestType {
    
	private ReferenceType[] referenceField;
    
	private string idField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("Reference")]
	public ReferenceType[] Reference {
		get {
			return this.referenceField;
		}
		set {
			this.referenceField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="ID")]
	public string Id {
		get {
			return this.idField;
		}
		set {
			this.idField = value;
		}
	}
}