/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("PackagedTransportHandlingUnit", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class TransportHandlingUnitType {
    
	private IDType idField;
    
	private TransportHandlingUnitTypeCodeType transportHandlingUnitTypeCodeField;
    
	private HandlingCodeType handlingCodeField;
    
	private HandlingInstructionsType[] handlingInstructionsField;
    
	private HazardousRiskIndicatorType hazardousRiskIndicatorField;
    
	private TotalGoodsItemQuantityType totalGoodsItemQuantityField;
    
	private TotalPackageQuantityType totalPackageQuantityField;
    
	private DamageRemarksType[] damageRemarksField;
    
	private ShippingMarksType[] shippingMarksField;
    
	private TraceIDType traceIDField;
    
	private DespatchLineType[] handlingUnitDespatchLineField;
    
	private PackageType[] actualPackageField;
    
	private ReceiptLineType[] receivedHandlingUnitReceiptLineField;
    
	private TransportEquipmentType[] transportEquipmentField;
    
	private TransportMeansType[] transportMeansField;
    
	private HazardousGoodsTransitType[] hazardousGoodsTransitField;
    
	private DimensionType[] measurementDimensionField;
    
	private TemperatureType minimumTemperatureField;
    
	private TemperatureType maximumTemperatureField;
    
	private GoodsItemType[] goodsItemField;
    
	private DimensionType floorSpaceMeasurementDimensionField;
    
	private DimensionType palletSpaceMeasurementDimensionField;
    
	private DocumentReferenceType[] shipmentDocumentReferenceField;
    
	private StatusType[] statusField;
    
	private CustomsDeclarationType[] customsDeclarationField;
    
	private ShipmentType[] referencedShipmentField;
    
	private PackageType[] packageField;
    
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
	public TransportHandlingUnitTypeCodeType TransportHandlingUnitTypeCode {
		get {
			return this.transportHandlingUnitTypeCodeField;
		}
		set {
			this.transportHandlingUnitTypeCodeField = value;
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
	[System.Xml.Serialization.XmlElementAttribute("HandlingInstructions", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public HandlingInstructionsType[] HandlingInstructions {
		get {
			return this.handlingInstructionsField;
		}
		set {
			this.handlingInstructionsField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public HazardousRiskIndicatorType HazardousRiskIndicator {
		get {
			return this.hazardousRiskIndicatorField;
		}
		set {
			this.hazardousRiskIndicatorField = value;
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
	public TotalPackageQuantityType TotalPackageQuantity {
		get {
			return this.totalPackageQuantityField;
		}
		set {
			this.totalPackageQuantityField = value;
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
	[System.Xml.Serialization.XmlElementAttribute("ShippingMarks", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ShippingMarksType[] ShippingMarks {
		get {
			return this.shippingMarksField;
		}
		set {
			this.shippingMarksField = value;
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
	[System.Xml.Serialization.XmlElementAttribute("HandlingUnitDespatchLine")]
	public DespatchLineType[] HandlingUnitDespatchLine {
		get {
			return this.handlingUnitDespatchLineField;
		}
		set {
			this.handlingUnitDespatchLineField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("ActualPackage")]
	public PackageType[] ActualPackage {
		get {
			return this.actualPackageField;
		}
		set {
			this.actualPackageField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("ReceivedHandlingUnitReceiptLine")]
	public ReceiptLineType[] ReceivedHandlingUnitReceiptLine {
		get {
			return this.receivedHandlingUnitReceiptLineField;
		}
		set {
			this.receivedHandlingUnitReceiptLineField = value;
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
	[System.Xml.Serialization.XmlElementAttribute("TransportMeans")]
	public TransportMeansType[] TransportMeans {
		get {
			return this.transportMeansField;
		}
		set {
			this.transportMeansField = value;
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
	public DimensionType FloorSpaceMeasurementDimension {
		get {
			return this.floorSpaceMeasurementDimensionField;
		}
		set {
			this.floorSpaceMeasurementDimensionField = value;
		}
	}
    
	/// <comentarios/>
	public DimensionType PalletSpaceMeasurementDimension {
		get {
			return this.palletSpaceMeasurementDimensionField;
		}
		set {
			this.palletSpaceMeasurementDimensionField = value;
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
	[System.Xml.Serialization.XmlElementAttribute("Status")]
	public StatusType[] Status {
		get {
			return this.statusField;
		}
		set {
			this.statusField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("CustomsDeclaration")]
	public CustomsDeclarationType[] CustomsDeclaration {
		get {
			return this.customsDeclarationField;
		}
		set {
			this.customsDeclarationField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("ReferencedShipment")]
	public ShipmentType[] ReferencedShipment {
		get {
			return this.referencedShipmentField;
		}
		set {
			this.referencedShipmentField = value;
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
}