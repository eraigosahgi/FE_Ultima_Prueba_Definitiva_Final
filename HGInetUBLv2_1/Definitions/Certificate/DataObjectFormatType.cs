/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://uri.etsi.org/01903/v1.3.2#")]
[System.Xml.Serialization.XmlRootAttribute("DataObjectFormat", Namespace="http://uri.etsi.org/01903/v1.3.2#", IsNullable=false)]
public partial class DataObjectFormatType {
    
	private string descriptionField;
    
	private ObjectIdentifierType objectIdentifierField;
    
	private string mimeTypeField;
    
	private string encodingField;
    
	private string objectReferenceField;
    
	/// <comentarios/>
	public string Description {
		get {
			return this.descriptionField;
		}
		set {
			this.descriptionField = value;
		}
	}
    
	/// <comentarios/>
	public ObjectIdentifierType ObjectIdentifier {
		get {
			return this.objectIdentifierField;
		}
		set {
			this.objectIdentifierField = value;
		}
	}
    
	/// <comentarios/>
	public string MimeType {
		get {
			return this.mimeTypeField;
		}
		set {
			this.mimeTypeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(DataType="anyURI")]
	public string Encoding {
		get {
			return this.encodingField;
		}
		set {
			this.encodingField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="anyURI")]
	public string ObjectReference {
		get {
			return this.objectReferenceField;
		}
		set {
			this.objectReferenceField = value;
		}
	}
}