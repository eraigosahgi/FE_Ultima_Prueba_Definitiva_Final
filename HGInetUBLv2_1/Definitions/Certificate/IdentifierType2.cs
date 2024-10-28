/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(TypeName="IdentifierType", Namespace="http://uri.etsi.org/01903/v1.3.2#")]
public partial class IdentifierType2 {
    
	private QualifierType qualifierField;
    
	private bool qualifierFieldSpecified;
    
	private string valueField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public QualifierType Qualifier {
		get {
			return this.qualifierField;
		}
		set {
			this.qualifierField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlIgnoreAttribute()]
	public bool QualifierSpecified {
		get {
			return this.qualifierFieldSpecified;
		}
		set {
			this.qualifierFieldSpecified = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlTextAttribute(DataType="anyURI")]
	public string Value {
		get {
			return this.valueField;
		}
		set {
			this.valueField = value;
		}
	}
}