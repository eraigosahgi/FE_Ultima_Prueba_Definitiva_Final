using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("LocationCoordinate", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
	public partial class LocationCoordinateType {
        
		private CoordinateSystemCodeType coordinateSystemCodeField;
        
		private LatitudeDegreesMeasureType latitudeDegreesMeasureField;
        
		private LatitudeMinutesMeasureType latitudeMinutesMeasureField;
        
		private LatitudeDirectionCodeType1 latitudeDirectionCodeField;
        
		private LongitudeDegreesMeasureType longitudeDegreesMeasureField;
        
		private LongitudeMinutesMeasureType longitudeMinutesMeasureField;
        
		private LongitudeDirectionCodeType1 longitudeDirectionCodeField;
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public CoordinateSystemCodeType CoordinateSystemCode {
			get {
				return this.coordinateSystemCodeField;
			}
			set {
				this.coordinateSystemCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public LatitudeDegreesMeasureType LatitudeDegreesMeasure {
			get {
				return this.latitudeDegreesMeasureField;
			}
			set {
				this.latitudeDegreesMeasureField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public LatitudeMinutesMeasureType LatitudeMinutesMeasure {
			get {
				return this.latitudeMinutesMeasureField;
			}
			set {
				this.latitudeMinutesMeasureField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public LatitudeDirectionCodeType1 LatitudeDirectionCode {
			get {
				return this.latitudeDirectionCodeField;
			}
			set {
				this.latitudeDirectionCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public LongitudeDegreesMeasureType LongitudeDegreesMeasure {
			get {
				return this.longitudeDegreesMeasureField;
			}
			set {
				this.longitudeDegreesMeasureField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public LongitudeMinutesMeasureType LongitudeMinutesMeasure {
			get {
				return this.longitudeMinutesMeasureField;
			}
			set {
				this.longitudeMinutesMeasureField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public LongitudeDirectionCodeType1 LongitudeDirectionCode {
			get {
				return this.longitudeDirectionCodeField;
			}
			set {
				this.longitudeDirectionCodeField = value;
			}
		}
	}
}