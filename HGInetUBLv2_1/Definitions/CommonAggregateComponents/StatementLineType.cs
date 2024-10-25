/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("StatementLine", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class StatementLineType {
    
	private IDType idField;
    
	private NoteType[] noteField;
    
	private UUIDType uUIDField;
    
	private BalanceBroughtForwardIndicatorType balanceBroughtForwardIndicatorField;
    
	private DebitLineAmountType debitLineAmountField;
    
	private CreditLineAmountType creditLineAmountField;
    
	private BalanceAmountType balanceAmountField;
    
	private PaymentPurposeCodeType paymentPurposeCodeField;
    
	private PaymentMeansType paymentMeansField;
    
	private PaymentTermsType[] paymentTermsField;
    
	private CustomerPartyType buyerCustomerPartyField;
    
	private SupplierPartyType sellerSupplierPartyField;
    
	private CustomerPartyType originatorCustomerPartyField;
    
	private CustomerPartyType accountingCustomerPartyField;
    
	private SupplierPartyType accountingSupplierPartyField;
    
	private PartyType payeePartyField;
    
	private PeriodType[] invoicePeriodField;
    
	private BillingReferenceType[] billingReferenceField;
    
	private DocumentReferenceType[] documentReferenceField;
    
	private ExchangeRateType exchangeRateField;
    
	private AllowanceChargeType[] allowanceChargeField;
    
	private PaymentType[] collectedPaymentField;
    
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
	public UUIDType UUID {
		get {
			return this.uUIDField;
		}
		set {
			this.uUIDField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public BalanceBroughtForwardIndicatorType BalanceBroughtForwardIndicator {
		get {
			return this.balanceBroughtForwardIndicatorField;
		}
		set {
			this.balanceBroughtForwardIndicatorField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public DebitLineAmountType DebitLineAmount {
		get {
			return this.debitLineAmountField;
		}
		set {
			this.debitLineAmountField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public CreditLineAmountType CreditLineAmount {
		get {
			return this.creditLineAmountField;
		}
		set {
			this.creditLineAmountField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public BalanceAmountType BalanceAmount {
		get {
			return this.balanceAmountField;
		}
		set {
			this.balanceAmountField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public PaymentPurposeCodeType PaymentPurposeCode {
		get {
			return this.paymentPurposeCodeField;
		}
		set {
			this.paymentPurposeCodeField = value;
		}
	}
    
	/// <comentarios/>
	public PaymentMeansType PaymentMeans {
		get {
			return this.paymentMeansField;
		}
		set {
			this.paymentMeansField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("PaymentTerms")]
	public PaymentTermsType[] PaymentTerms {
		get {
			return this.paymentTermsField;
		}
		set {
			this.paymentTermsField = value;
		}
	}
    
	/// <comentarios/>
	public CustomerPartyType BuyerCustomerParty {
		get {
			return this.buyerCustomerPartyField;
		}
		set {
			this.buyerCustomerPartyField = value;
		}
	}
    
	/// <comentarios/>
	public SupplierPartyType SellerSupplierParty {
		get {
			return this.sellerSupplierPartyField;
		}
		set {
			this.sellerSupplierPartyField = value;
		}
	}
    
	/// <comentarios/>
	public CustomerPartyType OriginatorCustomerParty {
		get {
			return this.originatorCustomerPartyField;
		}
		set {
			this.originatorCustomerPartyField = value;
		}
	}
    
	/// <comentarios/>
	public CustomerPartyType AccountingCustomerParty {
		get {
			return this.accountingCustomerPartyField;
		}
		set {
			this.accountingCustomerPartyField = value;
		}
	}
    
	/// <comentarios/>
	public SupplierPartyType AccountingSupplierParty {
		get {
			return this.accountingSupplierPartyField;
		}
		set {
			this.accountingSupplierPartyField = value;
		}
	}
    
	/// <comentarios/>
	public PartyType PayeeParty {
		get {
			return this.payeePartyField;
		}
		set {
			this.payeePartyField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("InvoicePeriod")]
	public PeriodType[] InvoicePeriod {
		get {
			return this.invoicePeriodField;
		}
		set {
			this.invoicePeriodField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("BillingReference")]
	public BillingReferenceType[] BillingReference {
		get {
			return this.billingReferenceField;
		}
		set {
			this.billingReferenceField = value;
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
	public ExchangeRateType ExchangeRate {
		get {
			return this.exchangeRateField;
		}
		set {
			this.exchangeRateField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("AllowanceCharge")]
	public AllowanceChargeType[] AllowanceCharge {
		get {
			return this.allowanceChargeField;
		}
		set {
			this.allowanceChargeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("CollectedPayment")]
	public PaymentType[] CollectedPayment {
		get {
			return this.collectedPaymentField;
		}
		set {
			this.collectedPaymentField = value;
		}
	}
}