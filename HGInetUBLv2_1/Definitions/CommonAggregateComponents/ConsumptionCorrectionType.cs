﻿/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("ConsumptionCorrection", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class ConsumptionCorrectionType {
    
	private CorrectionTypeType correctionTypeField;
    
	private CorrectionTypeCodeType correctionTypeCodeField;
    
	private MeterNumberType meterNumberField;
    
	private GasPressureQuantityType gasPressureQuantityField;
    
	private ActualTemperatureReductionQuantityType actualTemperatureReductionQuantityField;
    
	private NormalTemperatureReductionQuantityType normalTemperatureReductionQuantityField;
    
	private DifferenceTemperatureReductionQuantityType differenceTemperatureReductionQuantityField;
    
	private DescriptionType[] descriptionField;
    
	private CorrectionUnitAmountType correctionUnitAmountField;
    
	private ConsumptionEnergyQuantityType consumptionEnergyQuantityField;
    
	private ConsumptionWaterQuantityType consumptionWaterQuantityField;
    
	private CorrectionAmountType correctionAmountField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public CorrectionTypeType CorrectionType {
		get {
			return this.correctionTypeField;
		}
		set {
			this.correctionTypeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public CorrectionTypeCodeType CorrectionTypeCode {
		get {
			return this.correctionTypeCodeField;
		}
		set {
			this.correctionTypeCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public MeterNumberType MeterNumber {
		get {
			return this.meterNumberField;
		}
		set {
			this.meterNumberField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public GasPressureQuantityType GasPressureQuantity {
		get {
			return this.gasPressureQuantityField;
		}
		set {
			this.gasPressureQuantityField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ActualTemperatureReductionQuantityType ActualTemperatureReductionQuantity {
		get {
			return this.actualTemperatureReductionQuantityField;
		}
		set {
			this.actualTemperatureReductionQuantityField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public NormalTemperatureReductionQuantityType NormalTemperatureReductionQuantity {
		get {
			return this.normalTemperatureReductionQuantityField;
		}
		set {
			this.normalTemperatureReductionQuantityField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public DifferenceTemperatureReductionQuantityType DifferenceTemperatureReductionQuantity {
		get {
			return this.differenceTemperatureReductionQuantityField;
		}
		set {
			this.differenceTemperatureReductionQuantityField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("Description", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public DescriptionType[] Description {
		get {
			return this.descriptionField;
		}
		set {
			this.descriptionField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public CorrectionUnitAmountType CorrectionUnitAmount {
		get {
			return this.correctionUnitAmountField;
		}
		set {
			this.correctionUnitAmountField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ConsumptionEnergyQuantityType ConsumptionEnergyQuantity {
		get {
			return this.consumptionEnergyQuantityField;
		}
		set {
			this.consumptionEnergyQuantityField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ConsumptionWaterQuantityType ConsumptionWaterQuantity {
		get {
			return this.consumptionWaterQuantityField;
		}
		set {
			this.consumptionWaterQuantityField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public CorrectionAmountType CorrectionAmount {
		get {
			return this.correctionAmountField;
		}
		set {
			this.correctionAmountField = value;
		}
	}
}