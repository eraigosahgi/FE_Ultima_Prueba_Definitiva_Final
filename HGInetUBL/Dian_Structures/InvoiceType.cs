using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.dian.gov.co/contratos/facturaelectronica/v1")]
	[System.Xml.Serialization.XmlRootAttribute("Invoice", Namespace = "http://www.dian.gov.co/contratos/facturaelectronica/v1", IsNullable = false)]

	public partial class InvoiceType
	{

		private UBLExtensionType[] uBLExtensionsField;

		private UBLVersionIDType uBLVersionIDField;

		private CustomizationIDType customizationIDField;

		private ProfileIDType profileIDField;

		private IDType idField;

		private UUIDType uUIDField;

		private IssueDateType issueDateField;

		private IssueTimeType issueTimeField;

		private InvoiceTypeCodeType invoiceTypeCodeField;

		private NoteType[] noteField;

		private DocumentCurrencyCodeType documentCurrencyCodeField;

		private AccountingCostCodeType accountingCostCodeField;

		private AccountingCostType accountingCostField;

		private LineCountNumericType lineCountNumericField;

		private PeriodType invoicePeriodField;

		private OrderReferenceType[] orderReferenceField;

		private BillingReferenceType[] billingReferenceField;

		private DocumentReferenceType[] despatchDocumentReferenceField;

		private DocumentReferenceType[] receiptDocumentReferenceField;

		private DocumentReferenceType[] originatorDocumentReferenceField;

		private DocumentReferenceType[] contractDocumentReferenceField;

		private DocumentReferenceType[] additionalDocumentReferenceField;

		private SupplierPartyType1 accountingSupplierPartyField;

		private CustomerPartyType1 accountingCustomerPartyField;

		private PartyType payeePartyField;

		private CustomerPartyType buyerCustomerPartyField;

		private SupplierPartyType sellerSupplierPartyField;

		private PartyType taxRepresentativePartyField;

		private DeliveryType1[] deliveryField;

		private DeliveryTermsType1 deliveryTermsField;

		private PaymentMeansType[] paymentMeansField;

		private PaymentTermsType[] paymentTermsField;

		private PaymentType1[] prepaidPaymentField;

		private AllowanceChargeType1[] allowanceChargeField;

		private ExchangeRateType paymentExchangeRateField;

		private ExchangeRateType paymentAlternativeExchangeRateField;

		private TaxTotalType1[] taxTotalField;

		private MonetaryTotalType1 legalMonetaryTotalField;

		private InvoiceLineType1[] invoiceLineField;


		/// <comentarios/>
		[System.Xml.Serialization.XmlArrayAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2")]
		[System.Xml.Serialization.XmlArrayItemAttribute("UBLExtension", IsNullable = false)]
		public UBLExtensionType[] UBLExtensions
		{
			get
			{
				return this.uBLExtensionsField;
			}
			set
			{
				this.uBLExtensionsField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public UBLVersionIDType UBLVersionID
		{
			get
			{
				return this.uBLVersionIDField;
			}
			set
			{
				this.uBLVersionIDField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public CustomizationIDType CustomizationID
		{
			get
			{
				return this.customizationIDField;
			}
			set
			{
				this.customizationIDField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public ProfileIDType ProfileID
		{
			get
			{
				return this.profileIDField;
			}
			set
			{
				this.profileIDField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public IDType ID
		{
			get
			{
				return this.idField;
			}
			set
			{
				this.idField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public UUIDType UUID
		{
			get
			{
				return this.uUIDField;
			}
			set
			{
				this.uUIDField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public IssueDateType IssueDate
		{
			get
			{
				return this.issueDateField;
			}
			set
			{
				this.issueDateField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public IssueTimeType IssueTime
		{
			get
			{
				return this.issueTimeField;
			}
			set
			{
				this.issueTimeField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public InvoiceTypeCodeType InvoiceTypeCode
		{
			get
			{
				return this.invoiceTypeCodeField;
			}
			set
			{
				this.invoiceTypeCodeField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("Note", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public NoteType[] Note
		{
			get
			{
				return this.noteField;
			}
			set
			{
				this.noteField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public DocumentCurrencyCodeType DocumentCurrencyCode
		{
			get
			{
				return this.documentCurrencyCodeField;
			}
			set
			{
				this.documentCurrencyCodeField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public AccountingCostCodeType AccountingCostCode
		{
			get
			{
				return this.accountingCostCodeField;
			}
			set
			{
				this.accountingCostCodeField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public AccountingCostType AccountingCost
		{
			get
			{
				return this.accountingCostField;
			}
			set
			{
				this.accountingCostField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public LineCountNumericType LineCountNumeric
		{
			get
			{
				return this.lineCountNumericField;
			}
			set
			{
				this.lineCountNumericField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public PeriodType InvoicePeriod
		{
			get
			{
				return this.invoicePeriodField;
			}
			set
			{
				this.invoicePeriodField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("OrderReference", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public OrderReferenceType[] OrderReference
		{
			get
			{
				return this.orderReferenceField;
			}
			set
			{
				this.orderReferenceField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("BillingReference", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public BillingReferenceType[] BillingReference
		{
			get
			{
				return this.billingReferenceField;
			}
			set
			{
				this.billingReferenceField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("DespatchDocumentReference", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public DocumentReferenceType[] DespatchDocumentReference
		{
			get
			{
				return this.despatchDocumentReferenceField;
			}
			set
			{
				this.despatchDocumentReferenceField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("ReceiptDocumentReference", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public DocumentReferenceType[] ReceiptDocumentReference
		{
			get
			{
				return this.receiptDocumentReferenceField;
			}
			set
			{
				this.receiptDocumentReferenceField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("OriginatorDocumentReference", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public DocumentReferenceType[] OriginatorDocumentReference
		{
			get
			{
				return this.originatorDocumentReferenceField;
			}
			set
			{
				this.originatorDocumentReferenceField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("ContractDocumentReference", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public DocumentReferenceType[] ContractDocumentReference
		{
			get
			{
				return this.contractDocumentReferenceField;
			}
			set
			{
				this.contractDocumentReferenceField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("AdditionalDocumentReference", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public DocumentReferenceType[] AdditionalDocumentReference
		{
			get
			{
				return this.additionalDocumentReferenceField;
			}
			set
			{
				this.additionalDocumentReferenceField = value;
			}
		}

		/// <comentarios/>
		public SupplierPartyType1 AccountingSupplierParty
		{
			get
			{
				return this.accountingSupplierPartyField;
			}
			set
			{
				this.accountingSupplierPartyField = value;
			}
		}

		/// <comentarios/>
		public CustomerPartyType1 AccountingCustomerParty
		{
			get
			{
				return this.accountingCustomerPartyField;
			}
			set
			{
				this.accountingCustomerPartyField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public PartyType PayeeParty
		{
			get
			{
				return this.payeePartyField;
			}
			set
			{
				this.payeePartyField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public CustomerPartyType BuyerCustomerParty
		{
			get
			{
				return this.buyerCustomerPartyField;
			}
			set
			{
				this.buyerCustomerPartyField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public SupplierPartyType SellerSupplierParty
		{
			get
			{
				return this.sellerSupplierPartyField;
			}
			set
			{
				this.sellerSupplierPartyField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public PartyType TaxRepresentativeParty
		{
			get
			{
				return this.taxRepresentativePartyField;
			}
			set
			{
				this.taxRepresentativePartyField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("Delivery")]
		public DeliveryType1[] Delivery
		{
			get
			{
				return this.deliveryField;
			}
			set
			{
				this.deliveryField = value;
			}
		}

		/// <comentarios/>
		public DeliveryTermsType1 DeliveryTerms
		{
			get
			{
				return this.deliveryTermsField;
			}
			set
			{
				this.deliveryTermsField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("PaymentMeans", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public PaymentMeansType[] PaymentMeans
		{
			get
			{
				return this.paymentMeansField;
			}
			set
			{
				this.paymentMeansField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("PaymentTerms", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public PaymentTermsType[] PaymentTerms
		{
			get
			{
				return this.paymentTermsField;
			}
			set
			{
				this.paymentTermsField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("PrepaidPayment")]
		public PaymentType1[] PrepaidPayment
		{
			get
			{
				return this.prepaidPaymentField;
			}
			set
			{
				this.prepaidPaymentField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("AllowanceCharge")]
		public AllowanceChargeType1[] AllowanceCharge
		{
			get
			{
				return this.allowanceChargeField;
			}
			set
			{
				this.allowanceChargeField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public ExchangeRateType PaymentExchangeRate
		{
			get
			{
				return this.paymentExchangeRateField;
			}
			set
			{
				this.paymentExchangeRateField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public ExchangeRateType PaymentAlternativeExchangeRate
		{
			get
			{
				return this.paymentAlternativeExchangeRateField;
			}
			set
			{
				this.paymentAlternativeExchangeRateField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("TaxTotal")]
		public TaxTotalType1[] TaxTotal
		{
			get
			{
				return this.taxTotalField;
			}
			set
			{
				this.taxTotalField = value;
			}
		}

		/// <comentarios/>
		public MonetaryTotalType1 LegalMonetaryTotal
		{
			get
			{
				return this.legalMonetaryTotalField;
			}
			set
			{
				this.legalMonetaryTotalField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("InvoiceLine")]
		public InvoiceLineType1[] InvoiceLine
		{
			get
			{
				return this.invoiceLineField;
			}
			set
			{
				this.invoiceLineField = value;
			}
		}
	}


}