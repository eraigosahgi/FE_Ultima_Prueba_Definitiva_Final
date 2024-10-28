/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("ProcurementProject", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class ProcurementProjectType {
    
	private IDType idField;
    
	private NameType1[] nameField;
    
	private DescriptionType[] descriptionField;
    
	private ProcurementTypeCodeType procurementTypeCodeField;
    
	private ProcurementSubTypeCodeType procurementSubTypeCodeField;
    
	private QualityControlCodeType qualityControlCodeField;
    
	private RequiredFeeAmountType requiredFeeAmountField;
    
	private FeeDescriptionType[] feeDescriptionField;
    
	private RequestedDeliveryDateType requestedDeliveryDateField;
    
	private EstimatedOverallContractQuantityType estimatedOverallContractQuantityField;
    
	private NoteType[] noteField;
    
	private RequestedTenderTotalType requestedTenderTotalField;
    
	private CommodityClassificationType mainCommodityClassificationField;
    
	private CommodityClassificationType[] additionalCommodityClassificationField;
    
	private LocationType1[] realizedLocationField;
    
	private PeriodType plannedPeriodField;
    
	private ContractExtensionType contractExtensionField;
    
	private RequestForTenderLineType[] requestForTenderLineField;
    
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
	[System.Xml.Serialization.XmlElementAttribute("Name", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public NameType1[] Name {
		get {
			return this.nameField;
		}
		set {
			this.nameField = value;
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
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ProcurementTypeCodeType ProcurementTypeCode {
		get {
			return this.procurementTypeCodeField;
		}
		set {
			this.procurementTypeCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ProcurementSubTypeCodeType ProcurementSubTypeCode {
		get {
			return this.procurementSubTypeCodeField;
		}
		set {
			this.procurementSubTypeCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public QualityControlCodeType QualityControlCode {
		get {
			return this.qualityControlCodeField;
		}
		set {
			this.qualityControlCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public RequiredFeeAmountType RequiredFeeAmount {
		get {
			return this.requiredFeeAmountField;
		}
		set {
			this.requiredFeeAmountField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("FeeDescription", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public FeeDescriptionType[] FeeDescription {
		get {
			return this.feeDescriptionField;
		}
		set {
			this.feeDescriptionField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public RequestedDeliveryDateType RequestedDeliveryDate {
		get {
			return this.requestedDeliveryDateField;
		}
		set {
			this.requestedDeliveryDateField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public EstimatedOverallContractQuantityType EstimatedOverallContractQuantity {
		get {
			return this.estimatedOverallContractQuantityField;
		}
		set {
			this.estimatedOverallContractQuantityField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("Note", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public NoteType[] Note {
		get {
			return this.noteField;
		}
		set {
			this.noteField = value;
		}
	}
    
	/// <comentarios/>
	public RequestedTenderTotalType RequestedTenderTotal {
		get {
			return this.requestedTenderTotalField;
		}
		set {
			this.requestedTenderTotalField = value;
		}
	}
    
	/// <comentarios/>
	public CommodityClassificationType MainCommodityClassification {
		get {
			return this.mainCommodityClassificationField;
		}
		set {
			this.mainCommodityClassificationField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("AdditionalCommodityClassification")]
	public CommodityClassificationType[] AdditionalCommodityClassification {
		get {
			return this.additionalCommodityClassificationField;
		}
		set {
			this.additionalCommodityClassificationField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("RealizedLocation")]
	public LocationType1[] RealizedLocation {
		get {
			return this.realizedLocationField;
		}
		set {
			this.realizedLocationField = value;
		}
	}
    
	/// <comentarios/>
	public PeriodType PlannedPeriod {
		get {
			return this.plannedPeriodField;
		}
		set {
			this.plannedPeriodField = value;
		}
	}
    
	/// <comentarios/>
	public ContractExtensionType ContractExtension {
		get {
			return this.contractExtensionField;
		}
		set {
			this.contractExtensionField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("RequestForTenderLine")]
	public RequestForTenderLineType[] RequestForTenderLine {
		get {
			return this.requestForTenderLineField;
		}
		set {
			this.requestForTenderLineField = value;
		}
	}
}