/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://uri.etsi.org/01903/v1.3.2#")]
[System.Xml.Serialization.XmlRootAttribute("SigningCertificate", Namespace="http://uri.etsi.org/01903/v1.3.2#", IsNullable=false)]
public partial class CertIDListType {
    
	private CertIDType[] certField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("Cert")]
	public CertIDType[] Cert {
		get {
			return this.certField;
		}
		set {
			this.certField = value;
		}
	}
}