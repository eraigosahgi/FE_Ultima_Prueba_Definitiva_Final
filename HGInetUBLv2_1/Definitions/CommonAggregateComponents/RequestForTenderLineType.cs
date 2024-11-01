﻿/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("RequestForTenderLine", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class RequestForTenderLineType {
    
	private IDType idField;
    
	private UUIDType uUIDField;
    
	private NoteType[] noteField;
    
	private QuantityType2 quantityField;
    
	private MinimumQuantityType minimumQuantityField;
    
	private MaximumQuantityType maximumQuantityField;
    
	private TaxIncludedIndicatorType taxIncludedIndicatorField;
    
	private MinimumAmountType minimumAmountField;
    
	private MaximumAmountType maximumAmountField;
    
	private EstimatedAmountType estimatedAmountField;
    
	private DocumentReferenceType[] documentReferenceField;
    
	private PeriodType[] deliveryPeriodField;
    
	private ItemLocationQuantityType[] requiredItemLocationQuantityField;
    
	private PeriodType warrantyValidityPeriodField;
    
	private ItemType itemField;
    
	private RequestForTenderLineType[] subRequestForTenderLineField;
    
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
	public UUIDType UUID {
		get {
			return this.uUIDField;
		}
		set {
			this.uUIDField = value;
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
	public MinimumQuantityType MinimumQuantity {
		get {
			return this.minimumQuantityField;
		}
		set {
			this.minimumQuantityField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public MaximumQuantityType MaximumQuantity {
		get {
			return this.maximumQuantityField;
		}
		set {
			this.maximumQuantityField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public TaxIncludedIndicatorType TaxIncludedIndicator {
		get {
			return this.taxIncludedIndicatorField;
		}
		set {
			this.taxIncludedIndicatorField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public MinimumAmountType MinimumAmount {
		get {
			return this.minimumAmountField;
		}
		set {
			this.minimumAmountField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public MaximumAmountType MaximumAmount {
		get {
			return this.maximumAmountField;
		}
		set {
			this.maximumAmountField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public EstimatedAmountType EstimatedAmount {
		get {
			return this.estimatedAmountField;
		}
		set {
			this.estimatedAmountField = value;
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
	[System.Xml.Serialization.XmlElementAttribute("DeliveryPeriod")]
	public PeriodType[] DeliveryPeriod {
		get {
			return this.deliveryPeriodField;
		}
		set {
			this.deliveryPeriodField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("RequiredItemLocationQuantity")]
	public ItemLocationQuantityType[] RequiredItemLocationQuantity {
		get {
			return this.requiredItemLocationQuantityField;
		}
		set {
			this.requiredItemLocationQuantityField = value;
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
	public ItemType Item {
		get {
			return this.itemField;
		}
		set {
			this.itemField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("SubRequestForTenderLine")]
	public RequestForTenderLineType[] SubRequestForTenderLine {
		get {
			return this.subRequestForTenderLineField;
		}
		set {
			this.subRequestForTenderLineField = value;
		}
	}
}