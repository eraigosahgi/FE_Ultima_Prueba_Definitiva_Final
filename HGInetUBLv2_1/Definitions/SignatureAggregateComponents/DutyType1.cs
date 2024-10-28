/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(TypeName="DutyType", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("CallDuty", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class DutyType1 {
    
	private AmountType2 amountField;
    
	private DutyType dutyField;
    
	private DutyCodeType dutyCodeField;
    
	private TaxCategoryType taxCategoryField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public AmountType2 Amount {
		get {
			return this.amountField;
		}
		set {
			this.amountField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public DutyType Duty {
		get {
			return this.dutyField;
		}
		set {
			this.dutyField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public DutyCodeType DutyCode {
		get {
			return this.dutyCodeField;
		}
		set {
			this.dutyCodeField = value;
		}
	}
    
	/// <comentarios/>
	public TaxCategoryType TaxCategory {
		get {
			return this.taxCategoryField;
		}
		set {
			this.taxCategoryField = value;
		}
	}
}