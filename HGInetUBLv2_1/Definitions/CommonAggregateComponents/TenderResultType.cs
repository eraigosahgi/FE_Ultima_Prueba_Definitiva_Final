/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("TenderResult", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class TenderResultType {
    
	private TenderResultCodeType tenderResultCodeField;
    
	private DescriptionType[] descriptionField;
    
	private AdvertisementAmountType advertisementAmountField;
    
	private AwardDateType awardDateField;
    
	private AwardTimeType awardTimeField;
    
	private ReceivedTenderQuantityType receivedTenderQuantityField;
    
	private LowerTenderAmountType lowerTenderAmountField;
    
	private HigherTenderAmountType higherTenderAmountField;
    
	private StartDateType startDateField;
    
	private ReceivedElectronicTenderQuantityType receivedElectronicTenderQuantityField;
    
	private ReceivedForeignTenderQuantityType receivedForeignTenderQuantityField;
    
	private ContractType contractField;
    
	private TenderedProjectType awardedTenderedProjectField;
    
	private PeriodType contractFormalizationPeriodField;
    
	private SubcontractTermsType[] subcontractTermsField;
    
	private WinningPartyType[] winningPartyField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public TenderResultCodeType TenderResultCode {
		get {
			return this.tenderResultCodeField;
		}
		set {
			this.tenderResultCodeField = value;
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
	public AdvertisementAmountType AdvertisementAmount {
		get {
			return this.advertisementAmountField;
		}
		set {
			this.advertisementAmountField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public AwardDateType AwardDate {
		get {
			return this.awardDateField;
		}
		set {
			this.awardDateField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public AwardTimeType AwardTime {
		get {
			return this.awardTimeField;
		}
		set {
			this.awardTimeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ReceivedTenderQuantityType ReceivedTenderQuantity {
		get {
			return this.receivedTenderQuantityField;
		}
		set {
			this.receivedTenderQuantityField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public LowerTenderAmountType LowerTenderAmount {
		get {
			return this.lowerTenderAmountField;
		}
		set {
			this.lowerTenderAmountField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public HigherTenderAmountType HigherTenderAmount {
		get {
			return this.higherTenderAmountField;
		}
		set {
			this.higherTenderAmountField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public StartDateType StartDate {
		get {
			return this.startDateField;
		}
		set {
			this.startDateField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ReceivedElectronicTenderQuantityType ReceivedElectronicTenderQuantity {
		get {
			return this.receivedElectronicTenderQuantityField;
		}
		set {
			this.receivedElectronicTenderQuantityField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ReceivedForeignTenderQuantityType ReceivedForeignTenderQuantity {
		get {
			return this.receivedForeignTenderQuantityField;
		}
		set {
			this.receivedForeignTenderQuantityField = value;
		}
	}
    
	/// <comentarios/>
	public ContractType Contract {
		get {
			return this.contractField;
		}
		set {
			this.contractField = value;
		}
	}
    
	/// <comentarios/>
	public TenderedProjectType AwardedTenderedProject {
		get {
			return this.awardedTenderedProjectField;
		}
		set {
			this.awardedTenderedProjectField = value;
		}
	}
    
	/// <comentarios/>
	public PeriodType ContractFormalizationPeriod {
		get {
			return this.contractFormalizationPeriodField;
		}
		set {
			this.contractFormalizationPeriodField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("SubcontractTerms")]
	public SubcontractTermsType[] SubcontractTerms {
		get {
			return this.subcontractTermsField;
		}
		set {
			this.subcontractTermsField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("WinningParty")]
	public WinningPartyType[] WinningParty {
		get {
			return this.winningPartyField;
		}
		set {
			this.winningPartyField = value;
		}
	}
}