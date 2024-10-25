/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("EnergyWaterSupply", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class EnergyWaterSupplyType {
    
	private ConsumptionReportType[] consumptionReportField;
    
	private EnergyTaxReportType[] energyTaxReportField;
    
	private ConsumptionAverageType[] consumptionAverageField;
    
	private ConsumptionCorrectionType[] energyWaterConsumptionCorrectionField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("ConsumptionReport")]
	public ConsumptionReportType[] ConsumptionReport {
		get {
			return this.consumptionReportField;
		}
		set {
			this.consumptionReportField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("EnergyTaxReport")]
	public EnergyTaxReportType[] EnergyTaxReport {
		get {
			return this.energyTaxReportField;
		}
		set {
			this.energyTaxReportField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("ConsumptionAverage")]
	public ConsumptionAverageType[] ConsumptionAverage {
		get {
			return this.consumptionAverageField;
		}
		set {
			this.consumptionAverageField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("EnergyWaterConsumptionCorrection")]
	public ConsumptionCorrectionType[] EnergyWaterConsumptionCorrection {
		get {
			return this.energyWaterConsumptionCorrectionField;
		}
		set {
			this.energyWaterConsumptionCorrectionField = value;
		}
	}
}