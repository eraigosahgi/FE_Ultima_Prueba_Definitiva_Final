﻿/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.w3.org/2000/09/xmldsig#")]
[System.Xml.Serialization.XmlRootAttribute("Object", Namespace="http://www.w3.org/2000/09/xmldsig#", IsNullable=false)]
public partial class ObjectType {
    
	private System.Xml.XmlNode[] anyField;
    
	private string idField;
    
	private string mimeTypeField;
    
	private string encodingField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlTextAttribute()]
	[System.Xml.Serialization.XmlAnyElementAttribute()]
	public System.Xml.XmlNode[] Any {
		get {
			return this.anyField;
		}
		set {
			this.anyField = value;
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
	public string MimeType {
		get {
			return this.mimeTypeField;
		}
		set {
			this.mimeTypeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="anyURI")]
	public string Encoding {
		get {
			return this.encodingField;
		}
		set {
			this.encodingField = value;
		}
	}
}