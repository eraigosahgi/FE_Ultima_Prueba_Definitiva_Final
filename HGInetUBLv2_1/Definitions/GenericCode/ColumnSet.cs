/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://docs.oasis-open.org/codelist/ns/genericode/1.0/")]
public partial class ColumnSet {
    
	private Column[] columnField;
    
	private ColumnRef[] columnRefField;
    
	private Key[] keyField;
    
	private KeyRef[] keyRefField;
    
	private string datatypeLibraryField;
    
	private string baseField;
    
	public ColumnSet() {
		this.datatypeLibraryField = "http://www.w3.org/2001/XMLSchema-datatypes";
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