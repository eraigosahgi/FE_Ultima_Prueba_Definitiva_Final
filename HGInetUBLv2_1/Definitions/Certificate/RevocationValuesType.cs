﻿/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://uri.etsi.org/01903/v1.3.2#")]
[System.Xml.Serialization.XmlRootAttribute("RevocationValues", Namespace="http://uri.etsi.org/01903/v1.3.2#", IsNullable=false)]
public partial class RevocationValuesType {
    
	private EncapsulatedPKIDataType[] cRLValuesField;
    
	private EncapsulatedPKIDataType[] oCSPValuesField;
    
	private AnyType[] otherValuesField;
    
	private string idField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlArrayItemAttribute("EncapsulatedCRLValue", IsNullable=false)]
	public EncapsulatedPKIDataType[] CRLValues {
		get {
			return this.cRLValuesField;
		}
		set {
			this.cRLValuesField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlArrayItemAttribute("EncapsulatedOCSPValue", IsNullable=false)]
	public EncapsulatedPKIDataType[] OCSPValues {
		get {
			return this.oCSPValuesField;
		}
		set {
			this.oCSPValuesField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlArrayItemAttribute("OtherValue", IsNullable=false)]
	public AnyType[] OtherValues {
		get {
			return this.otherValuesField;
		}
		set {
			this.otherValuesField = value;
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