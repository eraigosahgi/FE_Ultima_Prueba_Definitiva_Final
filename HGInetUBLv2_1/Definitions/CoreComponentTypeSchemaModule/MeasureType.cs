﻿/// <comentarios/>
[System.Xml.Serialization.XmlIncludeAttribute(typeof(MeasureType1))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(ValueMeasureType))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(TareWeightMeasureType))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(SourceValueMeasureType))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(PreEventNotificationDurationMeasureType))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(PostEventNotificationDurationMeasureType))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(NetWeightMeasureType))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(NetVolumeMeasureType))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(NetTonnageMeasureType))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(NetNetWeightMeasureType))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(MinimumMeasureType))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(MeasureType2))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(MaximumMeasureType))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(LongitudeMinutesMeasureType))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(LongitudeDegreesMeasureType))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(LoadingLengthMeasureType))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(LeadTimeMeasureType))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(LatitudeMinutesMeasureType))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(LatitudeDegreesMeasureType))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(GrossWeightMeasureType))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(GrossVolumeMeasureType))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(GrossTonnageMeasureType))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(DurationMeasureType))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(ComparedValueMeasureType))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(ChargeableWeightMeasureType))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(BaseUnitMeasureType))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(AltitudeMeasureType))]
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:un:unece:uncefact:data:specification:CoreComponentTypeSchemaModule:2")]
public partial class MeasureType {
    
	private string unitCodeField;
    
	private string unitCodeListVersionIDField;
    
	private decimal valueField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="normalizedString")]
	public string unitCode {
		get {
			return this.unitCodeField;
		}
		set {
			this.unitCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="normalizedString")]
	public string unitCodeListVersionID {
		get {
			return this.unitCodeListVersionIDField;
		}
		set {
			this.unitCodeListVersionIDField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlTextAttribute()]
	public decimal Value {
		get {
			return this.valueField;
		}
		set {
			this.valueField = value;
		}
	}
}