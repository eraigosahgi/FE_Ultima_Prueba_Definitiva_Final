/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("ConsumptionReportReference", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class ConsumptionReportReferenceType {
    
	private ConsumptionReportIDType consumptionReportIDField;
    
	private ConsumptionTypeType consumptionTypeField;
    
	private ConsumptionTypeCodeType consumptionTypeCodeField;
    
	private TotalConsumedQuantityType totalConsumedQuantityField;
    
	private PeriodType periodField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ConsumptionReportIDType ConsumptionReportID {
		get {
			return this.consumptionReportIDField;
		}
		set {
			this.consumptionReportIDField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ConsumptionTypeType ConsumptionType {
		get {
			return this.consumptionTypeField;
		}
		set {
			this.consumptionTypeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ConsumptionTypeCodeType ConsumptionTypeCode {
		get {
			return this.consumptionTypeCodeField;
		}
		set {
			this.consumptionTypeCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public TotalConsumedQuantityType TotalConsumedQuantity {
		get {
			return this.totalConsumedQuantityField;
		}
		set {
			this.totalConsumedQuantityField = value;
		}
	}
    
	/// <comentarios/>
	public PeriodType Period {
		get {
			return this.periodField;
		}
		set {
			this.periodField = value;
		}
	}
}