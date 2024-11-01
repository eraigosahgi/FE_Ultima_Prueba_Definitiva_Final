﻿/// <comentarios/>
[System.Xml.Serialization.XmlIncludeAttribute(typeof(VideoType))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(SoundType))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(PictureType))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(GraphicType))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(BinaryObjectType1))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(EmbeddedDocumentBinaryObjectType))]
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:un:unece:uncefact:data:specification:CoreComponentTypeSchemaModule:2")]
public partial class BinaryObjectType {
    
	private string formatField;
    
	private string mimeCodeField;
    
	private string encodingCodeField;
    
	private string characterSetCodeField;
    
	private string uriField;
    
	private string filenameField;
    
	private byte[] valueField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public string format {
		get {
			return this.formatField;
		}
		set {
			this.formatField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="normalizedString")]
	public string mimeCode {
		get {
			return this.mimeCodeField;
		}
		set {
			this.mimeCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="normalizedString")]
	public string encodingCode {
		get {
			return this.encodingCodeField;
		}
		set {
			this.encodingCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="normalizedString")]
	public string characterSetCode {
		get {
			return this.characterSetCodeField;
		}
		set {
			this.characterSetCodeField = value;
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
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public string filename {
		get {
			return this.filenameField;
		}
		set {
			this.filenameField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlTextAttribute(DataType="base64Binary")]
	public byte[] Value {
		get {
			return this.valueField;
		}
		set {
			this.valueField = value;
		}
	}
}