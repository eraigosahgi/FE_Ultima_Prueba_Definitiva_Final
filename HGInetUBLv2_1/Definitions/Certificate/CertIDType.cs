/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://uri.etsi.org/01903/v1.3.2#")]
public partial class CertIDType {
    
	private DigestAlgAndValueType certDigestField;
    
	private X509IssuerSerialType issuerSerialField;
    
	private string uRIField;
    
	/// <comentarios/>
	public DigestAlgAndValueType CertDigest {
		get {
			return this.certDigestField;
		}
		set {
			this.certDigestField = value;
		}
	}
    
	/// <comentarios/>
	public X509IssuerSerialType IssuerSerial {
		get {
			return this.issuerSerialField;
		}
		set {
			this.issuerSerialField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="anyURI")]
	public string URI {
		get {
			return this.uRIField;
		}
		set {
			this.uRIField = value;
		}
	}
}