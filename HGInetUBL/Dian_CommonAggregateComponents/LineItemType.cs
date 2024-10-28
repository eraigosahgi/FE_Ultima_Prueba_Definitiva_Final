using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("BuyerProposedSubstituteLineItem", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
	public partial class LineItemType {
        
		private IDType idField;
        
		private SalesOrderIDType salesOrderIDField;
        
		private UUIDType uUIDField;
        
		private NoteType noteField;
        
		private LineStatusCodeType1 lineStatusCodeField;
        
		private QuantityType1 quantityField;
        
		private LineExtensionAmountType lineExtensionAmountField;
        
		private TotalTaxAmountType totalTaxAmountField;
        
		private MinimumQuantityType minimumQuantityField;
        
		private MaximumQuantityType maximumQuantityField;
        
		private MinimumBackorderQuantityType minimumBackorderQuantityField;
        
		private MaximumBackorderQuantityType maximumBackorderQuantityField;
        
		private InspectionMethodCodeType inspectionMethodCodeField;
        
		private PartialDeliveryIndicatorType partialDeliveryIndicatorField;
        
		private BackOrderAllowedIndicatorType backOrderAllowedIndicatorField;
        
		private AccountingCostCodeType accountingCostCodeField;
        
		private AccountingCostType accountingCostField;
        
		private DeliveryType[] deliveryField;
        
		private DeliveryTermsType deliveryTermsField;
        
		private PartyType originatorPartyField;
        
		private OrderedShipmentType[] orderedShipmentField;
        
		private PricingReferenceType pricingReferenceField;
        
		private AllowanceChargeType[] allowanceChargeField;
        
		private PriceType priceField;
        
		private ItemType itemField;
        
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
		public SalesOrderIDType SalesOrderID {
			get {
				return this.salesOrderIDField;
			}
			set {
				this.salesOrderIDField = value;
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
		public NoteType Note {
			get {
				return this.noteField;
			}
			set {
				this.noteField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public LineStatusCodeType1 LineStatusCode {
			get {
				return this.lineStatusCodeField;
			}
			set {
				this.lineStatusCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public QuantityType1 Quantity {
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
		public MinimumBackorderQuantityType MinimumBackorderQuantity {
			get {
				return this.minimumBackorderQuantityField;
			}
			set {
				this.minimumBackorderQuantityField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public MaximumBackorderQuantityType MaximumBackorderQuantity {
			get {
				return this.maximumBackorderQuantityField;
			}
			set {
				this.maximumBackorderQuantityField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public InspectionMethodCodeType InspectionMethodCode {
			get {
				return this.inspectionMethodCodeField;
			}
			set {
				this.inspectionMethodCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public PartialDeliveryIndicatorType PartialDeliveryIndicator {
			get {
				return this.partialDeliveryIndicatorField;
			}
			set {
				this.partialDeliveryIndicatorField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public BackOrderAllowedIndicatorType BackOrderAllowedIndicator {
			get {
				return this.backOrderAllowedIndicatorField;
			}
			set {
				this.backOrderAllowedIndicatorField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public AccountingCostCodeType AccountingCostCode {
			get {
				return this.accountingCostCodeField;
			}
			set {
				this.accountingCostCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public AccountingCostType AccountingCost {
			get {
				return this.accountingCostField;
			}
			set {
				this.accountingCostField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("Delivery")]
		public DeliveryType[] Delivery {
			get {
				return this.deliveryField;
			}
			set {
				this.deliveryField = value;
			}
		}
        
		/// <comentarios/>
		public DeliveryTermsType DeliveryTerms {
			get {
				return this.deliveryTermsField;
			}
			set {
				this.deliveryTermsField = value;
			}
		}
        
		/// <comentarios/>
		public PartyType OriginatorParty {
			get {
				return this.originatorPartyField;
			}
			set {
				this.originatorPartyField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("OrderedShipment")]
		public OrderedShipmentType[] OrderedShipment {
			get {
				return this.orderedShipmentField;
			}
			set {
				this.orderedShipmentField = value;
			}
		}
        
		/// <comentarios/>
		public PricingReferenceType PricingReference {
			get {
				return this.pricingReferenceField;
			}
			set {
				this.pricingReferenceField = value;
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
		public PriceType Price {
			get {
				return this.priceField;
			}
			set {
				this.priceField = value;
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
	}
}