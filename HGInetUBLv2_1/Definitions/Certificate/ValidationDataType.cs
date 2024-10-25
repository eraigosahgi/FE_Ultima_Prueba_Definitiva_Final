/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://uri.etsi.org/01903/v1.4.1#")]
[System.Xml.Serialization.XmlRootAttribute("TimeStampValidationData", Namespace="http://uri.etsi.org/01903/v1.4.1#", IsNullable=false)]
public partial class ValidationDataType {
    
	private CertificateValuesType certificateValuesField;
    
	private RevocationValuesType revocationValuesField;
    
	private string idField;
    
	private string urField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="http://uri.etsi.org/01903/v1.3.2#")]
	public CertificateValuesType CertificateValues {
		get {
			return this.certificateValuesField;
		}
		set {
			this.certificateValuesField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="http://uri.etsi.org/01903/v1.3.2#")]
	public RevocationValuesType RevocationValues {
		get {
			return this.revocationValuesField;
		}
		set {
			this.revocationValuesField = value;
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
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="anyURI")]
	public string UR {
		get {
			return this.urField;
		}
		set {
			this.urField = value;
		}
	}
}