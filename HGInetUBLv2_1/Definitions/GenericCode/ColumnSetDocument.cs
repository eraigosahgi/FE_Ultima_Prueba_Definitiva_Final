/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://docs.oasis-open.org/codelist/ns/genericode/1.0/")]
[System.Xml.Serialization.XmlRootAttribute("ColumnSet", Namespace="http://docs.oasis-open.org/codelist/ns/genericode/1.0/", IsNullable=false)]
public partial class ColumnSetDocument {
    
	private Annotation annotationField;
    
	private Identification identificationField;
    
	private Column[] columnField;
    
	private ColumnRef[] columnRefField;
    
	private Key[] keyField;
    
	private KeyRef[] keyRefField;
    
	private string datatypeLibraryField;
    
	private string baseField;
    
	public ColumnSetDocument() {
		this.datatypeLibraryField = "http://www.w3.org/2001/XMLSchema-datatypes";
	}
    
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
	public Identification Identification {
		get {
			return this.identificationField;
		}
		set {
			this.identificationField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("Column", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
	public Column[] Column {
		get {
			return this.columnField;
		}
		set {
			this.columnField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("ColumnRef", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
	public ColumnRef[] ColumnRef {
		get {
			return this.columnRefField;
		}
		set {
			this.columnRefField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("Key", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
	public Key[] Key {
		get {
			return this.keyField;
		}
		set {
			this.keyField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("KeyRef", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
	public KeyRef[] KeyRef {
		get {
			return this.keyRefField;
		}
		set {
			this.keyRefField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="anyURI")]
	[System.ComponentModel.DefaultValueAttribute("http://www.w3.org/2001/XMLSchema-datatypes")]
	public string DatatypeLibrary {
		get {
			return this.datatypeLibraryField;
		}
		set {
			this.datatypeLibraryField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(Form=System.Xml.Schema.XmlSchemaForm.Qualified, Namespace="http://www.w3.org/XML/1998/namespace")]
	public string @base {
		get {
			return this.baseField;
		}
		set {
			this.baseField = value;
		}
	}
}