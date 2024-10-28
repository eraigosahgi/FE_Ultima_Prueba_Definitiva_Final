using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(WeightMeasureType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(VolumeMeasureType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(NetWeightMeasureType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(NetVolumeMeasureType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(NetNetWeightMeasureType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(MinutesMeasureType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(MinimumMeasureType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(MeasureType1))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(MaximumMeasureType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(LongitudeMinutesMeasureType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(LongitudeDegreesMeasureType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(LoadingLengthMeasureType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(LengthMeasureType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(LeadTimeMeasureType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(LatitudeMinutesMeasureType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(LatitudeDegreesMeasureType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(GrossWeightMeasureType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(GrossVolumeMeasureType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(DurationMeasureType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(DegreesMeasureType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ChargeableWeightMeasureType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(BaseUnitMeasureType))]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2")]
	public partial class MeasureType {
        
		private UnitCodeContentType unitCodeField;
        
		private decimal valueField;
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public UnitCodeContentType unitCode {
			get {
				return this.unitCodeField;
			}
			set {
				this.unitCodeField = value;
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
}