﻿/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://uri.etsi.org/01903/v1.3.2#")]
public partial class DigestAlgAndValueType {
    
	private DigestMethodType digestMethodField;
    
	private byte[] digestValueField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="http://www.w3.org/2000/09/xmldsig#")]
	public DigestMethodType DigestMethod {
		get {
			return this.digestMethodField;
		}
		set {
			this.digestMethodField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="http://www.w3.org/2000/09/xmldsig#", DataType="base64Binary")]
	public byte[] DigestValue {
		get {
			return this.digestValueField;
		}
		set {
			this.digestValueField = value;
		}
	}
}