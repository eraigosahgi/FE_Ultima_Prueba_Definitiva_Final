/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://uri.etsi.org/01903/v1.3.2#")]
public partial class SignaturePolicyIdType {
    
	private ObjectIdentifierType sigPolicyIdField;
    
	private TransformType[] transformsField;
    
	private DigestAlgAndValueType sigPolicyHashField;
    
	private AnyType[] sigPolicyQualifiersField;
    
	/// <comentarios/>
	public ObjectIdentifierType SigPolicyId {
		get {
			return this.sigPolicyIdField;
		}
		set {
			this.sigPolicyIdField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlArrayAttribute(Namespace="http://www.w3.org/2000/09/xmldsig#")]
	[System.Xml.Serialization.XmlArrayItemAttribute("Transform", IsNullable=false)]
	public TransformType[] Transforms {
		get {
			return this.transformsField;
		}
		set {
			this.transformsField = value;
		}
	}
    
	/// <comentarios/>
	public DigestAlgAndValueType SigPolicyHash {
		get {
			return this.sigPolicyHashField;
		}
		set {
			this.sigPolicyHashField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlArrayItemAttribute("SigPolicyQualifier", IsNullable=false)]
	public AnyType[] SigPolicyQualifiers {
		get {
			return this.sigPolicyQualifiersField;
		}
		set {
			this.sigPolicyQualifiersField = value;
		}
	}
}