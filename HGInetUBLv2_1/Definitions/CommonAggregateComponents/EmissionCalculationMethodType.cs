﻿/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("EmissionCalculationMethod", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class EmissionCalculationMethodType {
    
	private CalculationMethodCodeType calculationMethodCodeField;
    
	private FullnessIndicationCodeType fullnessIndicationCodeField;
    
	private LocationType1 measurementFromLocationField;
    
	private LocationType1 measurementToLocationField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public CalculationMethodCodeType CalculationMethodCode {
		get {
			return this.calculationMethodCodeField;
		}
		set {
			this.calculationMethodCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public FullnessIndicationCodeType FullnessIndicationCode {
		get {
			return this.fullnessIndicationCodeField;
		}
		set {
			this.fullnessIndicationCodeField = value;
		}
	}
    
	/// <comentarios/>
	public LocationType1 MeasurementFromLocation {
		get {
			return this.measurementFromLocationField;
		}
		set {
			this.measurementFromLocationField = value;
		}
	}
    
	/// <comentarios/>
	public LocationType1 MeasurementToLocation {
		get {
			return this.measurementToLocationField;
		}
		set {
			this.measurementToLocationField = value;
		}
	}
}