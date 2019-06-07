/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://uri.etsi.org/01903/v1.3.2#")]
[System.Xml.Serialization.XmlRootAttribute("CompleteCertificateRefs", Namespace="http://uri.etsi.org/01903/v1.3.2#", IsNullable=false)]
public partial class CompleteCertificateRefsType {
    
	private CertIDType[] certRefsField;
    
	private string idField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlArrayItemAttribute("Cert", IsNullable=false)]
	public CertIDType[] CertRefs {
		get {
			return this.certRefsField;
		}
		set {
			this.certRefsField = value;
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
}