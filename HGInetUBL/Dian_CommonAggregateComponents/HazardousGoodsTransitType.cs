using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("HazardousGoodsTransit", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
	public partial class HazardousGoodsTransitType {
        
		private TransportEmergencyCardCodeType transportEmergencyCardCodeField;
        
		private PackingCriteriaCodeType packingCriteriaCodeField;
        
		private HazardousRegulationCodeType hazardousRegulationCodeField;
        
		private InhalationToxicityZoneCodeType inhalationToxicityZoneCodeField;
        
		private TransportAuthorizationCodeType transportAuthorizationCodeField;
        
		private TemperatureType maximumTemperatureField;
        
		private TemperatureType minimumTemperatureField;
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public TransportEmergencyCardCodeType TransportEmergencyCardCode {
			get {
				return this.transportEmergencyCardCodeField;
			}
			set {
				this.transportEmergencyCardCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public PackingCriteriaCodeType PackingCriteriaCode {
			get {
				return this.packingCriteriaCodeField;
			}
			set {
				this.packingCriteriaCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public HazardousRegulationCodeType HazardousRegulationCode {
			get {
				return this.hazardousRegulationCodeField;
			}
			set {
				this.hazardousRegulationCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public InhalationToxicityZoneCodeType InhalationToxicityZoneCode {
			get {
				return this.inhalationToxicityZoneCodeField;
			}
			set {
				this.inhalationToxicityZoneCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public TransportAuthorizationCodeType TransportAuthorizationCode {
			get {
				return this.transportAuthorizationCodeField;
			}
			set {
				this.transportAuthorizationCodeField = value;
			}
		}
        
		/// <comentarios/>
		public TemperatureType MaximumTemperature {
			get {
				return this.maximumTemperatureField;
			}
			set {
				this.maximumTemperatureField = value;
			}
		}
        
		/// <comentarios/>
		public TemperatureType MinimumTemperature {
			get {
				return this.minimumTemperatureField;
			}
			set {
				this.minimumTemperatureField = value;
			}
		}
	}
}