using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("TransportEquipment", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
	public partial class TransportEquipmentType {
        
		private IDType idField;
        
		private TransportEquipmentTypeCodeType1 transportEquipmentTypeCodeField;
        
		private ProviderTypeCodeType providerTypeCodeField;
        
		private OwnerTypeCodeType ownerTypeCodeField;
        
		private SizeTypeCodeType sizeTypeCodeField;
        
		private DispositionCodeType dispositionCodeField;
        
		private FullnessIndicationCodeType fullnessIndicationCodeField;
        
		private RefrigerationOnIndicatorType refrigerationOnIndicatorField;
        
		private InformationType informationField;
        
		private ReturnabilityIndicatorType returnabilityIndicatorField;
        
		private LegalStatusIndicatorType legalStatusIndicatorField;
        
		private DimensionType[] measurementDimensionField;
        
		private TransportEquipmentSealType[] transportEquipmentSealField;
        
		private TemperatureType minimumTemperatureField;
        
		private TemperatureType maximumTemperatureField;
        
		private PartyType providerPartyField;
        
		private PartyType loadingProofPartyField;
        
		private LocationType1 loadingLocationField;
        
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
		public TransportEquipmentTypeCodeType1 TransportEquipmentTypeCode {
			get {
				return this.transportEquipmentTypeCodeField;
			}
			set {
				this.transportEquipmentTypeCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public ProviderTypeCodeType ProviderTypeCode {
			get {
				return this.providerTypeCodeField;
			}
			set {
				this.providerTypeCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public OwnerTypeCodeType OwnerTypeCode {
			get {
				return this.ownerTypeCodeField;
			}
			set {
				this.ownerTypeCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public SizeTypeCodeType SizeTypeCode {
			get {
				return this.sizeTypeCodeField;
			}
			set {
				this.sizeTypeCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public DispositionCodeType DispositionCode {
			get {
				return this.dispositionCodeField;
			}
			set {
				this.dispositionCodeField = value;
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
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public RefrigerationOnIndicatorType RefrigerationOnIndicator {
			get {
				return this.refrigerationOnIndicatorField;
			}
			set {
				this.refrigerationOnIndicatorField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public InformationType Information {
			get {
				return this.informationField;
			}
			set {
				this.informationField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public ReturnabilityIndicatorType ReturnabilityIndicator {
			get {
				return this.returnabilityIndicatorField;
			}
			set {
				this.returnabilityIndicatorField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public LegalStatusIndicatorType LegalStatusIndicator {
			get {
				return this.legalStatusIndicatorField;
			}
			set {
				this.legalStatusIndicatorField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("MeasurementDimension")]
		public DimensionType[] MeasurementDimension {
			get {
				return this.measurementDimensionField;
			}
			set {
				this.measurementDimensionField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("TransportEquipmentSeal")]
		public TransportEquipmentSealType[] TransportEquipmentSeal {
			get {
				return this.transportEquipmentSealField;
			}
			set {
				this.transportEquipmentSealField = value;
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
		public PartyType ProviderParty {
			get {
				return this.providerPartyField;
			}
			set {
				this.providerPartyField = value;
			}
		}
        
		/// <comentarios/>
		public PartyType LoadingProofParty {
			get {
				return this.loadingProofPartyField;
			}
			set {
				this.loadingProofPartyField = value;
			}
		}
        
		/// <comentarios/>
		public LocationType1 LoadingLocation {
			get {
				return this.loadingLocationField;
			}
			set {
				this.loadingLocationField = value;
			}
		}
	}
}