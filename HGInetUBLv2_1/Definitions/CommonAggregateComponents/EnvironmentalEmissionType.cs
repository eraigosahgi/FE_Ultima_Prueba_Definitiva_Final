﻿/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("EnvironmentalEmission", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class EnvironmentalEmissionType {
    
	private EnvironmentalEmissionTypeCodeType environmentalEmissionTypeCodeField;
    
	private ValueMeasureType valueMeasureField;
    
	private DescriptionType[] descriptionField;
    
	private EmissionCalculationMethodType[] emissionCalculationMethodField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public EnvironmentalEmissionTypeCodeType EnvironmentalEmissionTypeCode {
		get {
			return this.environmentalEmissionTypeCodeField;
		}
		set {
			this.environmentalEmissionTypeCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ValueMeasureType ValueMeasure {
		get {
			return this.valueMeasureField;
		}
		set {
			this.valueMeasureField = value;
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
	[System.Xml.Serialization.XmlElementAttribute("EmissionCalculationMethod")]
	public EmissionCalculationMethodType[] EmissionCalculationMethod {
		get {
			return this.emissionCalculationMethodField;
		}
		set {
			this.emissionCalculationMethodField = value;
		}
	}
}