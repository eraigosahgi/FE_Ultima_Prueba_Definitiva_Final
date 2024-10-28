using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("ReportedShipment", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
	public partial class ShipmentType {
        
		private IDType idField;
        
		private ShippingPriorityLevelCodeType shippingPriorityLevelCodeField;
        
		private HandlingCodeType handlingCodeField;
        
		private HandlingInstructionsType handlingInstructionsField;
        
		private InformationType informationField;
        
		private GrossWeightMeasureType grossWeightMeasureField;
        
		private NetWeightMeasureType netWeightMeasureField;
        
		private NetNetWeightMeasureType netNetWeightMeasureField;
        
		private GrossVolumeMeasureType grossVolumeMeasureField;
        
		private NetVolumeMeasureType netVolumeMeasureField;
        
		private TotalGoodsItemQuantityType totalGoodsItemQuantityField;
        
		private TotalTransportHandlingUnitQuantityType totalTransportHandlingUnitQuantityField;
        
		private InsuranceValueAmountType insuranceValueAmountField;
        
		private DeclaredCustomsValueAmountType declaredCustomsValueAmountField;
        
		private DeclaredForCarriageValueAmountType declaredForCarriageValueAmountField;
        
		private DeclaredStatisticsValueAmountType declaredStatisticsValueAmountField;
        
		private FreeOnBoardValueAmountType freeOnBoardValueAmountField;
        
		private SpecialInstructionsType[] specialInstructionsField;
        
		private DeliveryInstructionsType[] deliveryInstructionsField;
        
		private SplitConsignmentIndicatorType splitConsignmentIndicatorField;
        
		private ConsignmentType consignmentField;
        
		private GoodsItemType[] goodsItemField;
        
		private ShipmentStageType[] shipmentStageField;
        
		private DeliveryType deliveryField;
        
		private TransportHandlingUnitType[] transportHandlingUnitField;
        
		private AddressType originAddressField;
        
		private LocationType1 firstArrivalPortLocationField;
        
		private LocationType1 lastExitPortLocationField;
        
		private CountryType exportCountryField;
        
		private AllowanceChargeType[] freightAllowanceChargeField;
        
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
		public ShippingPriorityLevelCodeType ShippingPriorityLevelCode {
			get {
				return this.shippingPriorityLevelCodeField;
			}
			set {
				this.shippingPriorityLevelCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public HandlingCodeType HandlingCode {
			get {
				return this.handlingCodeField;
			}
			set {
				this.handlingCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public HandlingInstructionsType HandlingInstructions {
			get {
				return this.handlingInstructionsField;
			}
			set {
				this.handlingInstructionsField = value;
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
		public NetWeightMeasureType NetWeightMeasure {
			get {
				return this.netWeightMeasureField;
			}
			set {
				this.netWeightMeasureField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public NetNetWeightMeasureType NetNetWeightMeasure {
			get {
				return this.netNetWeightMeasureField;
			}
			set {
				this.netNetWeightMeasureField = value;
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
		public NetVolumeMeasureType NetVolumeMeasure {
			get {
				return this.netVolumeMeasureField;
			}
			set {
				this.netVolumeMeasureField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public TotalGoodsItemQuantityType TotalGoodsItemQuantity {
			get {
				return this.totalGoodsItemQuantityField;
			}
			set {
				this.totalGoodsItemQuantityField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public TotalTransportHandlingUnitQuantityType TotalTransportHandlingUnitQuantity {
			get {
				return this.totalTransportHandlingUnitQuantityField;
			}
			set {
				this.totalTransportHandlingUnitQuantityField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public InsuranceValueAmountType InsuranceValueAmount {
			get {
				return this.insuranceValueAmountField;
			}
			set {
				this.insuranceValueAmountField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public DeclaredCustomsValueAmountType DeclaredCustomsValueAmount {
			get {
				return this.declaredCustomsValueAmountField;
			}
			set {
				this.declaredCustomsValueAmountField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public DeclaredForCarriageValueAmountType DeclaredForCarriageValueAmount {
			get {
				return this.declaredForCarriageValueAmountField;
			}
			set {
				this.declaredForCarriageValueAmountField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public DeclaredStatisticsValueAmountType DeclaredStatisticsValueAmount {
			get {
				return this.declaredStatisticsValueAmountField;
			}
			set {
				this.declaredStatisticsValueAmountField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public FreeOnBoardValueAmountType FreeOnBoardValueAmount {
			get {
				return this.freeOnBoardValueAmountField;
			}
			set {
				this.freeOnBoardValueAmountField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("SpecialInstructions", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public SpecialInstructionsType[] SpecialInstructions {
			get {
				return this.specialInstructionsField;
			}
			set {
				this.specialInstructionsField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("DeliveryInstructions", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public DeliveryInstructionsType[] DeliveryInstructions {
			get {
				return this.deliveryInstructionsField;
			}
			set {
				this.deliveryInstructionsField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public SplitConsignmentIndicatorType SplitConsignmentIndicator {
			get {
				return this.splitConsignmentIndicatorField;
			}
			set {
				this.splitConsignmentIndicatorField = value;
			}
		}
        
		/// <comentarios/>
		public ConsignmentType Consignment {
			get {
				return this.consignmentField;
			}
			set {
				this.consignmentField = value;
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
		public DeliveryType Delivery {
			get {
				return this.deliveryField;
			}
			set {
				this.deliveryField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("TransportHandlingUnit")]
		public TransportHandlingUnitType[] TransportHandlingUnit {
			get {
				return this.transportHandlingUnitField;
			}
			set {
				this.transportHandlingUnitField = value;
			}
		}
        
		/// <comentarios/>
		public AddressType OriginAddress {
			get {
				return this.originAddressField;
			}
			set {
				this.originAddressField = value;
			}
		}
        
		/// <comentarios/>
		public LocationType1 FirstArrivalPortLocation {
			get {
				return this.firstArrivalPortLocationField;
			}
			set {
				this.firstArrivalPortLocationField = value;
			}
		}
        
		/// <comentarios/>
		public LocationType1 LastExitPortLocation {
			get {
				return this.lastExitPortLocationField;
			}
			set {
				this.lastExitPortLocationField = value;
			}
		}
        
		/// <comentarios/>
		public CountryType ExportCountry {
			get {
				return this.exportCountryField;
			}
			set {
				this.exportCountryField = value;
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
	}
}