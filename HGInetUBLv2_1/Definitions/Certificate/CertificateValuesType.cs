/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://uri.etsi.org/01903/v1.3.2#")]
[System.Xml.Serialization.XmlRootAttribute("CertificateValues", Namespace="http://uri.etsi.org/01903/v1.3.2#", IsNullable=false)]
public partial class CertificateValuesType {
    
	private object[] itemsField;
    
	private string idField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("EncapsulatedX509Certificate", typeof(EncapsulatedPKIDataType))]
	[System.Xml.Serialization.XmlElementAttribute("OtherCertificate", typeof(AnyType))]
	public object[] Items {
		get {
			return this.itemsField;
		}
		set {
			this.itemsField = value;
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