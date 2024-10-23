using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("ShipmentStage", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
	public partial class ShipmentStageType {
        
		private IDType idField;
        
		private TransportModeCodeType1 transportModeCodeField;
        
		private TransportMeansTypeCodeType transportMeansTypeCodeField;
        
		private TransitDirectionCodeType transitDirectionCodeField;
        
		private PreCarriageIndicatorType preCarriageIndicatorField;
        
		private OnCarriageIndicatorType onCarriageIndicatorField;
        
		private PeriodType transitPeriodField;
        
		private PartyType[] carrierPartyField;
        
		private TransportMeansType transportMeansField;
        
		private LocationType1 loadingPortLocationField;
        
		private LocationType1 unloadingPortLocationField;
        
		private LocationType1 transshipPortLocationField;
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public IDType ID {
			get {
				return this.idField;
			}
			set {
				this.idField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public TransportModeCodeType1 TransportModeCode {
			get {
				return this.transportModeCodeField;
			}
			set {
				this.transportModeCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public TransportMeansTypeCodeType TransportMeansTypeCode {
			get {
				return this.transportMeansTypeCodeField;
			}
			set {
				this.transportMeansTypeCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public TransitDirectionCodeType TransitDirectionCode {
			get {
				return this.transitDirectionCodeField;
			}
			set {
				this.transitDirectionCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public PreCarriageIndicatorType PreCarriageIndicator {
			get {
				return this.preCarriageIndicatorField;
			}
			set {
				this.preCarriageIndicatorField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public OnCarriageIndicatorType OnCarriageIndicator {
			get {
				return this.onCarriageIndicatorField;
			}
			set {
				this.onCarriageIndicatorField = value;
			}
		}
        
		/// <comentarios/>
		public PeriodType TransitPeriod {
			get {
				return this.transitPeriodField;
			}
			set {
				this.transitPeriodField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("CarrierParty")]
		public PartyType[] CarrierParty {
			get {
				return this.carrierPartyField;
			}
			set {
				this.carrierPartyField = value;
			}
		}
        
		/// <comentarios/>
		public TransportMeansType TransportMeans {
			get {
				return this.transportMeansField;
			}
			set {
				this.transportMeansField = value;
			}
		}
        
		/// <comentarios/>
		public LocationType1 LoadingPortLocation {
			get {
				return this.loadingPortLocationField;
			}
			set {
				this.loadingPortLocationField = value;
			}
		}
        
		/// <comentarios/>
		public LocationType1 UnloadingPortLocation {
			get {
				return this.unloadingPortLocationField;
			}
			set {
				this.unloadingPortLocationField = value;
			}
		}
        
		/// <comentarios/>
		public LocationType1 TransshipPortLocation {
			get {
				return this.transshipPortLocationField;
			}
			set {
				this.transshipPortLocationField = value;
			}
		}
	}
}