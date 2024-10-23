/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://docs.oasis-open.org/codelist/ns/genericode/1.0/")]
public partial class Key {
    
	private Annotation annotationField;
    
	private ShortName shortNameField;
    
	private LongName[] longNameField;
    
	private string canonicalUriField;
    
	private string canonicalVersionUriField;
    
	private KeyColumnRef[] columnRefField;
    
	private string idField;
    
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
	[System.Xml.Serialization.XmlElementAttribute("ColumnRef", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
	public KeyColumnRef[] ColumnRef {
		get {
			return this.columnRefField;
		}
		set {
			this.columnRefField = value;
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