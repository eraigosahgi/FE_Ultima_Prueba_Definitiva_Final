﻿/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("ConsumptionHistory", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class ConsumptionHistoryType {
    
	private MeterNumberType meterNumberField;
    
	private QuantityType2 quantityField;
    
	private AmountType2 amountField;
    
	private ConsumptionLevelCodeType consumptionLevelCodeField;
    
	private ConsumptionLevelType consumptionLevelField;
    
	private DescriptionType[] descriptionField;
    
	private PeriodType periodField;
    
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
	public QuantityType2 Quantity {
		get {
			return this.quantityField;
		}
		set {
			this.quantityField = value;
		}
	}
    
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
	public ConsumptionLevelCodeType ConsumptionLevelCode {
		get {
			return this.consumptionLevelCodeField;
		}
		set {
			this.consumptionLevelCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ConsumptionLevelType ConsumptionLevel {
		get {
			return this.consumptionLevelField;
		}
		set {
			this.consumptionLevelField = value;
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
	public PeriodType Period {
		get {
			return this.periodField;
		}
		set {
			this.periodField = value;
		}
	}
}