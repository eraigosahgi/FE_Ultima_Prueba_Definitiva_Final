/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://docs.oasis-open.org/codelist/ns/genericode/1.0/")]
public partial class ColumnRef {
    
	private Annotation annotationField;
    
	private string canonicalVersionUriField;
    
	private string[] locationUriField;
    
	private DataRestrictions dataField;
    
	private string idField;
    
	private string externalRefField;
    
	private UseType useField;
    
	private bool useFieldSpecified;
    
	private string baseField;
    
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
	[System.Xml.Serialization.XmlElementAttribute("LocationUri", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType="anyURI")]
	public string[] LocationUri {
		get {
			return this.locationUriField;
		}
		set {
			this.locationUriField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
	public DataRestrictions Data {
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
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="NCName")]
	public string ExternalRef {
		get {
			return this.externalRefField;
		}
		set {
			this.externalRefField = value;
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
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlIgnoreAttribute()]
	public bool UseSpecified {
		get {
			return this.useFieldSpecified;
		}
		set {
			this.useFieldSpecified = value;
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