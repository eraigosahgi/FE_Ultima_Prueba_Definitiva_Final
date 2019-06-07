/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("MainCarriageShipmentStage", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class ShipmentStageType {
    
	private IDType idField;
    
	private TransportModeCodeType transportModeCodeField;
    
	private TransportMeansTypeCodeType transportMeansTypeCodeField;
    
	private TransitDirectionCodeType transitDirectionCodeField;
    
	private PreCarriageIndicatorType preCarriageIndicatorField;
    
	private OnCarriageIndicatorType onCarriageIndicatorField;
    
	private EstimatedDeliveryDateType estimatedDeliveryDateField;
    
	private EstimatedDeliveryTimeType estimatedDeliveryTimeField;
    
	private RequiredDeliveryDateType requiredDeliveryDateField;
    
	private RequiredDeliveryTimeType requiredDeliveryTimeField;
    
	private LoadingSequenceIDType loadingSequenceIDField;
    
	private SuccessiveSequenceIDType successiveSequenceIDField;
    
	private InstructionsType[] instructionsField;
    
	private DemurrageInstructionsType[] demurrageInstructionsField;
    
	private CrewQuantityType crewQuantityField;
    
	private PassengerQuantityType passengerQuantityField;
    
	private PeriodType transitPeriodField;
    
	private PartyType[] carrierPartyField;
    
	private TransportMeansType transportMeansField;
    
	private LocationType1 loadingPortLocationField;
    
	private LocationType1 unloadingPortLocationField;
    
	private LocationType1 transshipPortLocationField;
    
	private TransportEventType loadingTransportEventField;
    
	private TransportEventType examinationTransportEventField;
    
	private TransportEventType availabilityTransportEventField;
    
	private TransportEventType exportationTransportEventField;
    
	private TransportEventType dischargeTransportEventField;
    
	private TransportEventType warehousingTransportEventField;
    
	private TransportEventType takeoverTransportEventField;
    
	private TransportEventType optionalTakeoverTransportEventField;
    
	private TransportEventType dropoffTransportEventField;
    
	private TransportEventType actualPickupTransportEventField;
    
	private TransportEventType deliveryTransportEventField;
    
	private TransportEventType receiptTransportEventField;
    
	private TransportEventType storageTransportEventField;
    
	private TransportEventType acceptanceTransportEventField;
    
	private PartyType terminalOperatorPartyField;
    
	private PartyType customsAgentPartyField;
    
	private PeriodType estimatedTransitPeriodField;
    
	private AllowanceChargeType[] freightAllowanceChargeField;
    
	private LocationType1 freightChargeLocationField;
    
	private TransportEventType[] detentionTransportEventField;
    
	private TransportEventType requestedDepartureTransportEventField;
    
	private TransportEventType requestedArrivalTransportEventField;
    
	private TransportEventType[] requestedWaypointTransportEventField;
    
	private TransportEventType plannedDepartureTransportEventField;
    
	private TransportEventType plannedArrivalTransportEventField;
    
	private TransportEventType[] plannedWaypointTransportEventField;
    
	private TransportEventType actualDepartureTransportEventField;
    
	private TransportEventType actualWaypointTransportEventField;
    
	private TransportEventType actualArrivalTransportEventField;
    
	private TransportEventType[] transportEventField;
    
	private TransportEventType estimatedDepartureTransportEventField;
    
	private TransportEventType estimatedArrivalTransportEventField;
    
	private PersonType[] passengerPersonField;
    
	private PersonType[] driverPersonField;
    
	private PersonType reportingPersonField;
    
	private PersonType[] crewMemberPersonField;
    
	private PersonType securityOfficerPersonField;
    
	private PersonType masterPersonField;
    
	private PersonType shipsSurgeonPersonField;
    
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
	public TransportModeCodeType TransportModeCode {
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
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public EstimatedDeliveryDateType EstimatedDeliveryDate {
		get {
			return this.estimatedDeliveryDateField;
		}
		set {
			this.estimatedDeliveryDateField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public EstimatedDeliveryTimeType EstimatedDeliveryTime {
		get {
			return this.estimatedDeliveryTimeField;
		}
		set {
			this.estimatedDeliveryTimeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public RequiredDeliveryDateType RequiredDeliveryDate {
		get {
			return this.requiredDeliveryDateField;
		}
		set {
			this.requiredDeliveryDateField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public RequiredDeliveryTimeType RequiredDeliveryTime {
		get {
			return this.requiredDeliveryTimeField;
		}
		set {
			this.requiredDeliveryTimeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public LoadingSequenceIDType LoadingSequenceID {
		get {
			return this.loadingSequenceIDField;
		}
		set {
			this.loadingSequenceIDField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public SuccessiveSequenceIDType SuccessiveSequenceID {
		get {
			return this.successiveSequenceIDField;
		}
		set {
			this.successiveSequenceIDField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("Instructions", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public InstructionsType[] Instructions {
		get {
			return this.instructionsField;
		}
		set {
			this.instructionsField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("DemurrageInstructions", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public DemurrageInstructionsType[] DemurrageInstructions {
		get {
			return this.demurrageInstructionsField;
		}
		set {
			this.demurrageInstructionsField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public CrewQuantityType CrewQuantity {
		get {
			return this.crewQuantityField;
		}
		set {
			this.crewQuantityField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public PassengerQuantityType PassengerQuantity {
		get {
			return this.passengerQuantityField;
		}
		set {
			this.passengerQuantityField = value;
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
    
	/// <comentarios/>
	public TransportEventType LoadingTransportEvent {
		get {
			return this.loadingTransportEventField;
		}
		set {
			this.loadingTransportEventField = value;
		}
	}
    
	/// <comentarios/>
	public TransportEventType ExaminationTransportEvent {
		get {
			return this.examinationTransportEventField;
		}
		set {
			this.examinationTransportEventField = value;
		}
	}
    
	/// <comentarios/>
	public TransportEventType AvailabilityTransportEvent {
		get {
			return this.availabilityTransportEventField;
		}
		set {
			this.availabilityTransportEventField = value;
		}
	}
    
	/// <comentarios/>
	public TransportEventType ExportationTransportEvent {
		get {
			return this.exportationTransportEventField;
		}
		set {
			this.exportationTransportEventField = value;
		}
	}
    
	/// <comentarios/>
	public TransportEventType DischargeTransportEvent {
		get {
			return this.dischargeTransportEventField;
		}
		set {
			this.dischargeTransportEventField = value;
		}
	}
    
	/// <comentarios/>
	public TransportEventType WarehousingTransportEvent {
		get {
			return this.warehousingTransportEventField;
		}
		set {
			this.warehousingTransportEventField = value;
		}
	}
    
	/// <comentarios/>
	public TransportEventType TakeoverTransportEvent {
		get {
			return this.takeoverTransportEventField;
		}
		set {
			this.takeoverTransportEventField = value;
		}
	}
    
	/// <comentarios/>
	public TransportEventType OptionalTakeoverTransportEvent {
		get {
			return this.optionalTakeoverTransportEventField;
		}
		set {
			this.optionalTakeoverTransportEventField = value;
		}
	}
    
	/// <comentarios/>
	public TransportEventType DropoffTransportEvent {
		get {
			return this.dropoffTransportEventField;
		}
		set {
			this.dropoffTransportEventField = value;
		}
	}
    
	/// <comentarios/>
	public TransportEventType ActualPickupTransportEvent {
		get {
			return this.actualPickupTransportEventField;
		}
		set {
			this.actualPickupTransportEventField = value;
		}
	}
    
	/// <comentarios/>
	public TransportEventType DeliveryTransportEvent {
		get {
			return this.deliveryTransportEventField;
		}
		set {
			this.deliveryTransportEventField = value;
		}
	}
    
	/// <comentarios/>
	public TransportEventType ReceiptTransportEvent {
		get {
			return this.receiptTransportEventField;
		}
		set {
			this.receiptTransportEventField = value;
		}
	}
    
	/// <comentarios/>
	public TransportEventType StorageTransportEvent {
		get {
			return this.storageTransportEventField;
		}
		set {
			this.storageTransportEventField = value;
		}
	}
    
	/// <comentarios/>
	public TransportEventType AcceptanceTransportEvent {
		get {
			return this.acceptanceTransportEventField;
		}
		set {
			this.acceptanceTransportEventField = value;
		}
	}
    
	/// <comentarios/>
	public PartyType TerminalOperatorParty {
		get {
			return this.terminalOperatorPartyField;
		}
		set {
			this.terminalOperatorPartyField = value;
		}
	}
    
	/// <comentarios/>
	public PartyType CustomsAgentParty {
		get {
			return this.customsAgentPartyField;
		}
		set {
			this.customsAgentPartyField = value;
		}
	}
    
	/// <comentarios/>
	public PeriodType EstimatedTransitPeriod {
		get {
			return this.estimatedTransitPeriodField;
		}
		set {
			this.estimatedTransitPeriodField = value;
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
	public LocationType1 FreightChargeLocation {
		get {
			return this.freightChargeLocationField;
		}
		set {
			this.freightChargeLocationField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("DetentionTransportEvent")]
	public TransportEventType[] DetentionTransportEvent {
		get {
			return this.detentionTransportEventField;
		}
		set {
			this.detentionTransportEventField = value;
		}
	}
    
	/// <comentarios/>
	public TransportEventType RequestedDepartureTransportEvent {
		get {
			return this.requestedDepartureTransportEventField;
		}
		set {
			this.requestedDepartureTransportEventField = value;
		}
	}
    
	/// <comentarios/>
	public TransportEventType RequestedArrivalTransportEvent {
		get {
			return this.requestedArrivalTransportEventField;
		}
		set {
			this.requestedArrivalTransportEventField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("RequestedWaypointTransportEvent")]
	public TransportEventType[] RequestedWaypointTransportEvent {
		get {
			return this.requestedWaypointTransportEventField;
		}
		set {
			this.requestedWaypointTransportEventField = value;
		}
	}
    
	/// <comentarios/>
	public TransportEventType PlannedDepartureTransportEvent {
		get {
			return this.plannedDepartureTransportEventField;
		}
		set {
			this.plannedDepartureTransportEventField = value;
		}
	}
    
	/// <comentarios/>
	public TransportEventType PlannedArrivalTransportEvent {
		get {
			return this.plannedArrivalTransportEventField;
		}
		set {
			this.plannedArrivalTransportEventField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("PlannedWaypointTransportEvent")]
	public TransportEventType[] PlannedWaypointTransportEvent {
		get {
			return this.plannedWaypointTransportEventField;
		}
		set {
			this.plannedWaypointTransportEventField = value;
		}
	}
    
	/// <comentarios/>
	public TransportEventType ActualDepartureTransportEvent {
		get {
			return this.actualDepartureTransportEventField;
		}
		set {
			this.actualDepartureTransportEventField = value;
		}
	}
    
	/// <comentarios/>
	public TransportEventType ActualWaypointTransportEvent {
		get {
			return this.actualWaypointTransportEventField;
		}
		set {
			this.actualWaypointTransportEventField = value;
		}
	}
    
	/// <comentarios/>
	public TransportEventType ActualArrivalTransportEvent {
		get {
			return this.actualArrivalTransportEventField;
		}
		set {
			this.actualArrivalTransportEventField = value;
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
	public TransportEventType EstimatedDepartureTransportEvent {
		get {
			return this.estimatedDepartureTransportEventField;
		}
		set {
			this.estimatedDepartureTransportEventField = value;
		}
	}
    
	/// <comentarios/>
	public TransportEventType EstimatedArrivalTransportEvent {
		get {
			return this.estimatedArrivalTransportEventField;
		}
		set {
			this.estimatedArrivalTransportEventField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("PassengerPerson")]
	public PersonType[] PassengerPerson {
		get {
			return this.passengerPersonField;
		}
		set {
			this.passengerPersonField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("DriverPerson")]
	public PersonType[] DriverPerson {
		get {
			return this.driverPersonField;
		}
		set {
			this.driverPersonField = value;
		}
	}
    
	/// <comentarios/>
	public PersonType ReportingPerson {
		get {
			return this.reportingPersonField;
		}
		set {
			this.reportingPersonField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("CrewMemberPerson")]
	public PersonType[] CrewMemberPerson {
		get {
			return this.crewMemberPersonField;
		}
		set {
			this.crewMemberPersonField = value;
		}
	}
    
	/// <comentarios/>
	public PersonType SecurityOfficerPerson {
		get {
			return this.securityOfficerPersonField;
		}
		set {
			this.securityOfficerPersonField = value;
		}
	}
    
	/// <comentarios/>
	public PersonType MasterPerson {
		get {
			return this.masterPersonField;
		}
		set {
			this.masterPersonField = value;
		}
	}
    
	/// <comentarios/>
	public PersonType ShipsSurgeonPerson {
		get {
			return this.shipsSurgeonPersonField;
		}
		set {
			this.shipsSurgeonPersonField = value;
		}
	}
}