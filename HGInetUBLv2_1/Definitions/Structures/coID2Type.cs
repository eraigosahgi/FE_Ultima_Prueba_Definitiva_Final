/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="dian:gov:co:facturaelectronica:Structures-2-1")]
public partial class coID2Type {
    
	private coID2TypeSchemeAgencyID schemeAgencyIDField;
    
	private coID2TypeSchemeAgencyName schemeAgencyNameField;
    
	private coID2TypeSchemeID schemeIDField;
    
	private string schemeNameField;
    
	private string schemeVersionIDField;
    
	private string schemeDataURIField;
    
	private string schemeURIField;
    
	private string valueField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public coID2TypeSchemeAgencyID schemeAgencyID {
		get {
			return this.schemeAgencyIDField;
		}
		set {
			this.schemeAgencyIDField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public coID2TypeSchemeAgencyName schemeAgencyName {
		get {
			return this.schemeAgencyNameField;
		}
		set {
			this.schemeAgencyNameField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public coID2TypeSchemeID schemeID {
		get {
			return this.schemeIDField;
		}
		set {
			this.schemeIDField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public string schemeName {
		get {
			return this.schemeNameField;
		}
		set {
			this.schemeNameField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="normalizedString")]
	public string schemeVersionID {
		get {
			return this.schemeVersionIDField;
		}
		set {
			this.schemeVersionIDField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="anyURI")]
	public string schemeDataURI {
		get {
			return this.schemeDataURIField;
		}
		set {
			this.schemeDataURIField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="anyURI")]
	public string schemeURI {
		get {
			return this.schemeURIField;
		}
		set {
			this.schemeURIField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlTextAttribute(DataType="normalizedString")]
	public string Value {
		get {
			return this.valueField;
		}
		set {
			this.valueField = value;
		}
	}
}