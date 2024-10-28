/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://docs.oasis-open.org/codelist/ns/genericode/1.0/")]
public partial class Column {
    
	private Annotation annotationField;
    
	private ShortName shortNameField;
    
	private LongName[] longNameField;
    
	private string canonicalUriField;
    
	private string canonicalVersionUriField;
    
	private Data dataField;
    
	private string idField;
    
	private UseType useField;
    
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
	[System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType="anyURI")]
	public string CanonicalUri {
		get {
			return this.canonicalUriField;
		}
		set {
			this.canonicalUriField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType="anyURI")]
	public string CanonicalVersionUri {
		get {
			return this.canonicalVersionUriField;
		}
		set {
			this.canonicalVersionUriField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
	public Data Data {
		get {
			return this.dataField;
		}
		set {
			this.dataField = value;
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
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public UseType Use {
		get {
			return this.useField;
		}
		set {
			this.useField = value;
		}
	}
}