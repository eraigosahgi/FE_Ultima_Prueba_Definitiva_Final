/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://docs.oasis-open.org/codelist/ns/ContextValueAssociation/1.0/")]
public partial class Context {
    
	private Annotation annotationField;
    
	private Message[] messageField;
    
	private string metadataField;
    
	private string addressField;
    
	private string valuesField;
    
	private string markField;
    
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
	[System.Xml.Serialization.XmlElementAttribute("Message", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
	public Message[] Message {
		get {
			return this.messageField;
		}
		set {
			this.messageField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="IDREF")]
	public string metadata {
		get {
			return this.metadataField;
		}
		set {
			this.metadataField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public string address {
		get {
			return this.addressField;
		}
		set {
			this.addressField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="IDREFS")]
	public string values {
		get {
			return this.valuesField;
		}
		set {
			this.valuesField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="NMTOKEN")]
	public string mark {
		get {
			return this.markField;
		}
		set {
			this.markField = value;
		}
	}
}