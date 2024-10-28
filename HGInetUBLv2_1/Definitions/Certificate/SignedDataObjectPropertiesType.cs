/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://uri.etsi.org/01903/v1.3.2#")]
[System.Xml.Serialization.XmlRootAttribute("SignedDataObjectProperties", Namespace="http://uri.etsi.org/01903/v1.3.2#", IsNullable=false)]
public partial class SignedDataObjectPropertiesType {
    
	private DataObjectFormatType[] dataObjectFormatField;
    
	private CommitmentTypeIndicationType[] commitmentTypeIndicationField;
    
	private XAdESTimeStampType[] allDataObjectsTimeStampField;
    
	private XAdESTimeStampType[] individualDataObjectsTimeStampField;
    
	private string idField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("DataObjectFormat")]
	public DataObjectFormatType[] DataObjectFormat {
		get {
			return this.dataObjectFormatField;
		}
		set {
			this.dataObjectFormatField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("CommitmentTypeIndication")]
	public CommitmentTypeIndicationType[] CommitmentTypeIndication {
		get {
			return this.commitmentTypeIndicationField;
		}
		set {
			this.commitmentTypeIndicationField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("AllDataObjectsTimeStamp")]
	public XAdESTimeStampType[] AllDataObjectsTimeStamp {
		get {
			return this.allDataObjectsTimeStampField;
		}
		set {
			this.allDataObjectsTimeStampField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("IndividualDataObjectsTimeStamp")]
	public XAdESTimeStampType[] IndividualDataObjectsTimeStamp {
		get {
			return this.individualDataObjectsTimeStampField;
		}
		set {
			this.individualDataObjectsTimeStampField = value;
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