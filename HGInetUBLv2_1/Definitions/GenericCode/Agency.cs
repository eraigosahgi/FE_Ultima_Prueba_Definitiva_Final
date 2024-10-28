/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://docs.oasis-open.org/codelist/ns/genericode/1.0/")]
public partial class Agency {
    
	private ShortName shortNameField;
    
	private LongName[] longNameField;
    
	private GeneralIdentifier[] identifierField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
	public ShortName ShortName {
		get {
			return this.shortNameField;
		}
		set {
			this.shortNameField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("LongName", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
	public LongName[] LongName {
		get {
			return this.longNameField;
		}
		set {
			this.longNameField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("Identifier", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
	public GeneralIdentifier[] Identifier {
		get {
			return this.identifierField;
		}
		set {
			this.identifierField = value;
		}
	}
}