/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://docs.oasis-open.org/codelist/ns/genericode/1.0/")]
public partial class Identification {
    
	private ShortName shortNameField;
    
	private LongName[] longNameField;
    
	private string versionField;
    
	private string canonicalUriField;
    
	private string canonicalVersionUriField;
    
	private string[] locationUriField;
    
	private MimeTypedUri[] alternateFormatLocationUriField;
    
	private Agency agencyField;
    
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
	[System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType="token")]
	public string Version {
		get {
			return this.versionField;
		}
		set {
			this.versionField = value;
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
	[System.Xml.Serialization.XmlElementAttribute("AlternateFormatLocationUri", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
	public MimeTypedUri[] AlternateFormatLocationUri {
		get {
			return this.alternateFormatLocationUriField;
		}
		set {
			this.alternateFormatLocationUriField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
	public Agency Agency {
		get {
			return this.agencyField;
		}
		set {
			this.agencyField = value;
		}
	}
}