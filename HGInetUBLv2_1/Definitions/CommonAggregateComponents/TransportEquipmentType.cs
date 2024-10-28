/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("AttachedTransportEquipment", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class TransportEquipmentType {
    
	private IDType idField;
    
	private ReferencedConsignmentIDType[] referencedConsignmentIDField;
    
	private TransportEquipmentTypeCodeType transportEquipmentTypeCodeField;
    
	private ProviderTypeCodeType providerTypeCodeField;
    
	private OwnerTypeCodeType ownerTypeCodeField;
    
	private SizeTypeCodeType sizeTypeCodeField;
    
	private DispositionCodeType dispositionCodeField;
    
	private FullnessIndicationCodeType fullnessIndicationCodeField;
    
	private RefrigerationOnIndicatorType refrigerationOnIndicatorField;
    
	private InformationType[] informationField;
    
	private ReturnabilityIndicatorType returnabilityIndicatorField;
    
	private LegalStatusIndicatorType legalStatusIndicatorField;
    
	private AirFlowPercentType airFlowPercentField;
    
	private HumidityPercentType humidityPercentField;
    
	private AnimalFoodApprovedIndicatorType animalFoodApprovedIndicatorField;
    
	private HumanFoodApprovedIndicatorType humanFoodApprovedIndicatorField;
    
	private DangerousGoodsApprovedIndicatorType dangerousGoodsApprovedIndicatorField;
    
	private RefrigeratedIndicatorType refrigeratedIndicatorField;
    
	private CharacteristicsType characteristicsField;
    
	private DamageRemarksType[] damageRemarksField;
    
	private DescriptionType[] descriptionField;
    
	private SpecialTransportRequirementsType[] specialTransportRequirementsField;
    
	private GrossWeightMeasureType grossWeightMeasureField;
    
	private GrossVolumeMeasureType grossVolumeMeasureField;
    
	private TareWeightMeasureType tareWeightMeasureField;
    
	private TrackingDeviceCodeType trackingDeviceCodeField;
    
	private PowerIndicatorType powerIndicatorField;
    
	private TraceIDType traceIDField;
    
	private DimensionType[] measurementDimensionField;
    
	private TransportEquipmentSealType[] transportEquipmentSealField;
    
	private TemperatureType minimumTemperatureField;
    
	private TemperatureType maximumTemperatureField;
    
	private PartyType providerPartyField;
    
	private PartyType loadingProofPartyField;
    
	private SupplierPartyType supplierPartyField;
    
	private PartyType ownerPartyField;
    
	private PartyType operatingPartyField;
    
	private LocationType1 loadingLocationField;
    
	private LocationType1 unloadingLocationField;
    
	private LocationType1 storageLocationField;
    
	private TransportEventType[] positioningTransportEventField;
    
	private TransportEventType[] quarantineTransportEventField;
    
	private TransportEventType[] deliveryTransportEventField;
    
	private TransportEventType[] pickupTransportEventField;
    
	private TransportEventType[] handlingTransportEventField;
    
	private TransportEventType[] loadingTransportEventField;
    
	private TransportEventType[] transportEventField;
    
	private TransportMeansType applicableTransportMeansField;
    
	private TradingTermsType[] haulageTradingTermsField;
    
	private HazardousGoodsTransitType[] hazardousGoodsTransitField;
    
	private TransportHandlingUnitType[] packagedTransportHandlingUnitField;
    
	private AllowanceChargeType[] serviceAllowanceChargeField;
    
	private AllowanceChargeType[] freightAllowanceChargeField;
    
	private TransportEquipmentType[] attachedTransportEquipmentField;
    
	private DeliveryType deliveryField;
    
	private PickupType pickupField;
    
	private DespatchType despatchField;
    
	private DocumentReferenceType[] shipmentDocumentReferenceField;
    
	private TransportEquipmentType[] containedInTransportEquipmentField;
    
	private PackageType[] packageField;
    
	private GoodsItemType[] goodsItemField;
    
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
	[System.Xml.Serialization.XmlElementAttribute("ReferencedConsignmentID", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ReferencedConsignmentIDType[] ReferencedConsignmentID {
		get {
			return this.referencedConsignmentIDField;
		}
		set {
			this.referencedConsignmentIDField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public TransportEquipmentTypeCodeType TransportEquipmentTypeCode {
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
	[System.Xml.Serialization.XmlElementAttribute("Information", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public InformationType[] Information {
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
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public AirFlowPercentType AirFlowPercent {
		get {
			return this.airFlowPercentField;
		}
		set {
			this.airFlowPercentField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public HumidityPercentType HumidityPercent {
		get {
			return this.humidityPercentField;
		}
		set {
			this.humidityPercentField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public AnimalFoodApprovedIndicatorType AnimalFoodApprovedIndicator {
		get {
			return this.animalFoodApprovedIndicatorField;
		}
		set {
			this.animalFoodApprovedIndicatorField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public HumanFoodApprovedIndicatorType HumanFoodApprovedIndicator {
		get {
			return this.humanFoodApprovedIndicatorField;
		}
		set {
			this.humanFoodApprovedIndicatorField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public DangerousGoodsApprovedIndicatorType DangerousGoodsApprovedIndicator {
		get {
			return this.dangerousGoodsApprovedIndicatorField;
		}
		set {
			this.dangerousGoodsApprovedIndicatorField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public RefrigeratedIndicatorType RefrigeratedIndicator {
		get {
			return this.refrigeratedIndicatorField;
		}
		set {
			this.refrigeratedIndicatorField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public CharacteristicsType Characteristics {
		get {
			return this.characteristicsField;
		}
		set {
			this.characteristicsField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("DamageRemarks", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public DamageRemarksType[] DamageRemarks {
		get {
			return this.damageRemarksField;
		}
		set {
			this.damageRemarksField = value;
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
	[System.Xml.Serialization.XmlElementAttribute("SpecialTransportRequirements", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public SpecialTransportRequirementsType[] SpecialTransportRequirements {
		get {
			return this.specialTransportRequirementsField;
		}
		set {
			this.specialTransportRequirementsField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public GrossWeightMeasureType GrossWeightMeasure {
		get {
			return this.grossWeightMeasureField;
		}
		set {
			this.grossWeightMeasureField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public GrossVolumeMeasureType GrossVolumeMeasure {
		get {
			return this.grossVolumeMeasureField;
		}
		set {
			this.grossVolumeMeasureField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public TareWeightMeasureType TareWeightMeasure {
		get {
			return this.tareWeightMeasureField;
		}
		set {
			this.tareWeightMeasureField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public TrackingDeviceCodeType TrackingDeviceCode {
		get {
			return this.trackingDeviceCodeField;
		}
		set {
			this.trackingDeviceCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public PowerIndicatorType PowerIndicator {
		get {
			return this.powerIndicatorField;
		}
		set {
			this.powerIndicatorField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public TraceIDType TraceID {
		get {
			return this.traceIDField;
		}
		set {
			this.traceIDField = value;
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
	public SupplierPartyType SupplierParty {
		get {
			return this.supplierPartyField;
		}
		set {
			this.supplierPartyField = value;
		}
	}
    
	/// <comentarios/>
	public PartyType OwnerParty {
		get {
			return this.ownerPartyField;
		}
		set {
			this.ownerPartyField = value;
		}
	}
    
	/// <comentarios/>
	public PartyType OperatingParty {
		get {
			return this.operatingPartyField;
		}
		set {
			this.operatingPartyField = value;
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
    
	/// <comentarios/>
	public LocationType1 UnloadingLocation {
		get {
			return this.unloadingLocationField;
		}
		set {
			this.unloadingLocationField = value;
		}
	}
    
	/// <comentarios/>
	public LocationType1 StorageLocation {
		get {
			return this.storageLocationField;
		}
		set {
			this.storageLocationField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("PositioningTransportEvent")]
	public TransportEventType[] PositioningTransportEvent {
		get {
			return this.positioningTransportEventField;
		}
		set {
			this.positioningTransportEventField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("QuarantineTransportEvent")]
	public TransportEventType[] QuarantineTransportEvent {
		get {
			return this.quarantineTransportEventField;
		}
		set {
			this.quarantineTransportEventField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("DeliveryTransportEvent")]
	public TransportEventType[] DeliveryTransportEvent {
		get {
			return this.deliveryTransportEventField;
		}
		set {
			this.deliveryTransportEventField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("PickupTransportEvent")]
	public TransportEventType[] PickupTransportEvent {
		get {
			return this.pickupTransportEventField;
		}
		set {
			this.pickupTransportEventField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("HandlingTransportEvent")]
	public TransportEventType[] HandlingTransportEvent {
		get {
			return this.handlingTransportEventField;
		}
		set {
			this.handlingTransportEventField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("LoadingTransportEvent")]
	public TransportEventType[] LoadingTransportEvent {
		get {
			return this.loadingTransportEventField;
		}
		set {
			this.loadingTransportEventField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("TransportEvent")]
	public TransportEventType[] TransportEvent {
		get {
			return this.transportEventField;
		}
		set {
			this.transportEventField = value;
		}
	}
    
	/// <comentarios/>
	public TransportMeansType ApplicableTransportMeans {
		get {
			return this.applicableTransportMeansField;
		}
		set {
			this.applicableTransportMeansField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("HaulageTradingTerms")]
	public TradingTermsType[] HaulageTradingTerms {
		get {
			return this.haulageTradingTermsField;
		}
		set {
			this.haulageTradingTermsField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("HazardousGoodsTransit")]
	public HazardousGoodsTransitType[] HazardousGoodsTransit {
		get {
			return this.hazardousGoodsTransitField;
		}
		set {
			this.hazardousGoodsTransitField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("PackagedTransportHandlingUnit")]
	public TransportHandlingUnitType[] PackagedTransportHandlingUnit {
		get {
			return this.packagedTransportHandlingUnitField;
		}
		set {
			this.packagedTransportHandlingUnitField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("ServiceAllowanceCharge")]
	public AllowanceChargeType[] ServiceAllowanceCharge {
		get {
			return this.serviceAllowanceChargeField;
		}
		set {
			this.serviceAllowanceChargeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("FreightAllowanceCharge")]
	public AllowanceChargeType[] FreightAllowanceCharge {
		get {
			return this.freightAllowanceChargeField;
		}
		set {
			this.freightAllowanceChargeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("AttachedTransportEquipment")]
	public TransportEquipmentType[] AttachedTransportEquipment {
		get {
			return this.attachedTransportEquipmentField;
		}
		set {
			this.attachedTransportEquipmentField = value;
		}
	}
    
	/// <comentarios/>
	public DeliveryType Delivery {
		get {
			return this.deliveryField;
		}
		set {
			this.deliveryField = value;
		}
	}
    
	/// <comentarios/>
	public PickupType Pickup {
		get {
			return this.pickupField;
		}
		set {
			this.pickupField = value;
		}
	}
    
	/// <comentarios/>
	public DespatchType Despatch {
		get {
			return this.despatchField;
		}
		set {
			this.despatchField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("ShipmentDocumentReference")]
	public DocumentReferenceType[] ShipmentDocumentReference {
		get {
			return this.shipmentDocumentReferenceField;
		}
		set {
			this.shipmentDocumentReferenceField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("ContainedInTransportEquipment")]
	public TransportEquipmentType[] ContainedInTransportEquipment {
		get {
			return this.containedInTransportEquipmentField;
		}
		set {
			this.containedInTransportEquipmentField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("Package")]
	public PackageType[] Package {
		get {
			return this.packageField;
		}
		set {
			this.packageField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("GoodsItem")]
	public GoodsItemType[] GoodsItem {
		get {
			return this.goodsItemField;
		}
		set {
			this.goodsItemField = value;
		}
	}
}