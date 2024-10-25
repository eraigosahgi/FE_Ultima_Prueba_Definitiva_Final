/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://docs.oasis-open.org/codelist/ns/ContextValueAssociation/1.0/")]
public partial class ValueList {
    
	private Annotation annotationField;
    
	private Identification identificationField;
    
	private string idField;
    
	private string baseField;
    
	private string uriField;
    
	private string masqueradeUriField;
    
	private string keyField;
    
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
	[System.Xml.Serialization.XmlAttributeAttribute(Form=System.Xml.Schema.XmlSchemaForm.Qualified, Namespace="http://www.w3.org/XML/1998/namespace")]
	public string id {
		get {
			return this.idField;
		}
		set {
			this.idField = value;
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
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="anyURI")]
	public string uri {
		get {
			return this.uriField;
		}
		set {
			this.uriField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="anyURI")]
	public string masqueradeUri {
		get {
			return this.masqueradeUriField;
		}
		set {
			this.masqueradeUriField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="NMTOKEN")]
	public string key {
		get {
			return this.keyField;
		}
		set {
			this.keyField = value;
		}
	}
}