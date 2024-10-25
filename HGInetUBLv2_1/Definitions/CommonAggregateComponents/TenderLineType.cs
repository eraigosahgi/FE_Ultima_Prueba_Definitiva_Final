/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("SubTenderLine", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class TenderLineType {
    
	private IDType idField;
    
	private NoteType[] noteField;
    
	private QuantityType2 quantityField;
    
	private LineExtensionAmountType lineExtensionAmountField;
    
	private TotalTaxAmountType totalTaxAmountField;
    
	private OrderableUnitType orderableUnitField;
    
	private ContentUnitQuantityType contentUnitQuantityField;
    
	private OrderQuantityIncrementNumericType orderQuantityIncrementNumericField;
    
	private MinimumOrderQuantityType minimumOrderQuantityField;
    
	private MaximumOrderQuantityType maximumOrderQuantityField;
    
	private WarrantyInformationType[] warrantyInformationField;
    
	private PackLevelCodeType packLevelCodeField;
    
	private DocumentReferenceType[] documentReferenceField;
    
	private ItemType itemField;
    
	private ItemLocationQuantityType[] offeredItemLocationQuantityField;
    
	private RelatedItemType[] replacementRelatedItemField;
    
	private PartyType warrantyPartyField;
    
	private PeriodType warrantyValidityPeriodField;
    
	private TenderLineType[] subTenderLineField;
    
	private LineReferenceType callForTendersLineReferenceField;
    
	private DocumentReferenceType callForTendersDocumentReferenceField;
    
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
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public QuantityType2 Quantity {
		get {
			return this.quantityField;
		}
		set {
			this.quantityField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public LineExtensionAmountType LineExtensionAmount {
		get {
			return this.lineExtensionAmountField;
		}
		set {
			this.lineExtensionAmountField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public TotalTaxAmountType TotalTaxAmount {
		get {
			return this.totalTaxAmountField;
		}
		set {
			this.totalTaxAmountField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public OrderableUnitType OrderableUnit {
		get {
			return this.orderableUnitField;
		}
		set {
			this.orderableUnitField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ContentUnitQuantityType ContentUnitQuantity {
		get {
			return this.contentUnitQuantityField;
		}
		set {
			this.contentUnitQuantityField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public OrderQuantityIncrementNumericType OrderQuantityIncrementNumeric {
		get {
			return this.orderQuantityIncrementNumericField;
		}
		set {
			this.orderQuantityIncrementNumericField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public MinimumOrderQuantityType MinimumOrderQuantity {
		get {
			return this.minimumOrderQuantityField;
		}
		set {
			this.minimumOrderQuantityField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public MaximumOrderQuantityType MaximumOrderQuantity {
		get {
			return this.maximumOrderQuantityField;
		}
		set {
			this.maximumOrderQuantityField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("WarrantyInformation", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public WarrantyInformationType[] WarrantyInformation {
		get {
			return this.warrantyInformationField;
		}
		set {
			this.warrantyInformationField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public PackLevelCodeType PackLevelCode {
		get {
			return this.packLevelCodeField;
		}
		set {
			this.packLevelCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("DocumentReference")]
	public DocumentReferenceType[] DocumentReference {
		get {
			return this.documentReferenceField;
		}
		set {
			this.documentReferenceField = value;
		}
	}
    
	/// <comentarios/>
	public ItemType Item {
		get {
			return this.itemField;
		}
		set {
			this.itemField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("OfferedItemLocationQuantity")]
	public ItemLocationQuantityType[] OfferedItemLocationQuantity {
		get {
			return this.offeredItemLocationQuantityField;
		}
		set {
			this.offeredItemLocationQuantityField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("ReplacementRelatedItem")]
	public RelatedItemType[] ReplacementRelatedItem {
		get {
			return this.replacementRelatedItemField;
		}
		set {
			this.replacementRelatedItemField = value;
		}
	}
    
	/// <comentarios/>
	public PartyType WarrantyParty {
		get {
			return this.warrantyPartyField;
		}
		set {
			this.warrantyPartyField = value;
		}
	}
    
	/// <comentarios/>
	public PeriodType WarrantyValidityPeriod {
		get {
			return this.warrantyValidityPeriodField;
		}
		set {
			this.warrantyValidityPeriodField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("SubTenderLine")]
	public TenderLineType[] SubTenderLine {
		get {
			return this.subTenderLineField;
		}
		set {
			this.subTenderLineField = value;
		}
	}
    
	/// <comentarios/>
	public LineReferenceType CallForTendersLineReference {
		get {
			return this.callForTendersLineReferenceField;
		}
		set {
			this.callForTendersLineReferenceField = value;
		}
	}
    
	/// <comentarios/>
	public DocumentReferenceType CallForTendersDocumentReference {
		get {
			return this.callForTendersDocumentReferenceField;
		}
		set {
			this.callForTendersDocumentReferenceField = value;
		}
	}
}