/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("AdditionalTransportationService", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class TransportationServiceType {
    
	private TransportServiceCodeType transportServiceCodeField;
    
	private TariffClassCodeType tariffClassCodeField;
    
	private PriorityType priorityField;
    
	private FreightRateClassCodeType freightRateClassCodeField;
    
	private TransportationServiceDescriptionType[] transportationServiceDescriptionField;
    
	private TransportationServiceDetailsURIType transportationServiceDetailsURIField;
    
	private NominationDateType nominationDateField;
    
	private NominationTimeType nominationTimeField;
    
	private NameType1 nameField;
    
	private SequenceNumericType sequenceNumericField;
    
	private TransportEquipmentType[] transportEquipmentField;
    
	private TransportEquipmentType[] supportedTransportEquipmentField;
    
	private TransportEquipmentType[] unsupportedTransportEquipmentField;
    
	private CommodityClassificationType[] commodityClassificationField;
    
	private CommodityClassificationType[] supportedCommodityClassificationField;
    
	private CommodityClassificationType[] unsupportedCommodityClassificationField;
    
	private DimensionType totalCapacityDimensionField;
    
	private ShipmentStageType[] shipmentStageField;
    
	private TransportEventType[] transportEventField;
    
	private PartyType responsibleTransportServiceProviderPartyField;
    
	private EnvironmentalEmissionType[] environmentalEmissionField;
    
	private PeriodType estimatedDurationPeriodField;
    
	private ServiceFrequencyType[] scheduledServiceFrequencyField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public TransportServiceCodeType TransportServiceCode {
		get {
			return this.transportServiceCodeField;
		}
		set {
			this.transportServiceCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public TariffClassCodeType TariffClassCode {
		get {
			return this.tariffClassCodeField;
		}
		set {
			this.tariffClassCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public PriorityType Priority {
		get {
			return this.priorityField;
		}
		set {
			this.priorityField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public FreightRateClassCodeType FreightRateClassCode {
		get {
			return this.freightRateClassCodeField;
		}
		set {
			this.freightRateClassCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("TransportationServiceDescription", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public TransportationServiceDescriptionType[] TransportationServiceDescription {
		get {
			return this.transportationServiceDescriptionField;
		}
		set {
			this.transportationServiceDescriptionField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public TransportationServiceDetailsURIType TransportationServiceDetailsURI {
		get {
			return this.transportationServiceDetailsURIField;
		}
		set {
			this.transportationServiceDetailsURIField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public NominationDateType NominationDate {
		get {
			return this.nominationDateField;
		}
		set {
			this.nominationDateField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public NominationTimeType NominationTime {
		get {
			return this.nominationTimeField;
		}
		set {
			this.nominationTimeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public NameType1 Name {
		get {
			return this.nameField;
		}
		set {
			this.nameField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public SequenceNumericType SequenceNumeric {
		get {
			return this.sequenceNumericField;
		}
		set {
			this.sequenceNumericField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("TransportEquipment")]
	public TransportEquipmentType[] TransportEquipment {
		get {
			return this.transportEquipmentField;
		}
		set {
			this.transportEquipmentField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("SupportedTransportEquipment")]
	public TransportEquipmentType[] SupportedTransportEquipment {
		get {
			return this.supportedTransportEquipmentField;
		}
		set {
			this.supportedTransportEquipmentField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("UnsupportedTransportEquipment")]
	public TransportEquipmentType[] UnsupportedTransportEquipment {
		get {
			return this.unsupportedTransportEquipmentField;
		}
		set {
			this.unsupportedTransportEquipmentField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("CommodityClassification")]
	public CommodityClassificationType[] CommodityClassification {
		get {
			return this.commodityClassificationField;
		}
		set {
			this.commodityClassificationField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("SupportedCommodityClassification")]
	public CommodityClassificationType[] SupportedCommodityClassification {
		get {
			return this.supportedCommodityClassificationField;
		}
		set {
			this.supportedCommodityClassificationField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("UnsupportedCommodityClassification")]
	public CommodityClassificationType[] UnsupportedCommodityClassification {
		get {
			return this.unsupportedCommodityClassificationField;
		}
		set {
			this.unsupportedCommodityClassificationField = value;
		}
	}
    
	/// <comentarios/>
	public DimensionType TotalCapacityDimension {
		get {
			return this.totalCapacityDimensionField;
		}
		set {
			this.totalCapacityDimensionField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("ShipmentStage")]
	public ShipmentStageType[] ShipmentStage {
		get {
			return this.shipmentStageField;
		}
		set {
			this.shipmentStageField = value;
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
	public PartyType ResponsibleTransportServiceProviderParty {
		get {
			return this.responsibleTransportServiceProviderPartyField;
		}
		set {
			this.responsibleTransportServiceProviderPartyField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("EnvironmentalEmission")]
	public EnvironmentalEmissionType[] EnvironmentalEmission {
		get {
			return this.environmentalEmissionField;
		}
		set {
			this.environmentalEmissionField = value;
		}
	}
    
	/// <comentarios/>
	public PeriodType EstimatedDurationPeriod {
		get {
			return this.estimatedDurationPeriodField;
		}
		set {
			this.estimatedDurationPeriodField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("ScheduledServiceFrequency")]
	public ServiceFrequencyType[] ScheduledServiceFrequency {
		get {
			return this.scheduledServiceFrequencyField;
		}
		set {
			this.scheduledServiceFrequencyField = value;
		}
	}
}