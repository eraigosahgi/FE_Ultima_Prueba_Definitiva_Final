using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("ReminderLine", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
	public partial class ReminderLineType {
        
		private IDType idField;
        
		private NoteType noteField;
        
		private UUIDType uUIDField;
        
		private BalanceBroughtForwardIndicatorType balanceBroughtForwardIndicatorField;
        
		private DebitLineAmountType debitLineAmountField;
        
		private CreditLineAmountType creditLineAmountField;
        
		private AccountingCostCodeType accountingCostCodeField;
        
		private AccountingCostType accountingCostField;
        
		private PeriodType[] reminderPeriodField;
        
		private BillingReferenceType[] billingReferenceField;
        
		private ExchangeRateType exchangeRateField;
        
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
		[System.Xml.Serialization.XmlElementAttribute("ReminderPeriod")]
		public PeriodType[] ReminderPeriod {
			get {
				return this.reminderPeriodField;
			}
			set {
				this.reminderPeriodField = value;
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
		public ExchangeRateType ExchangeRate {
			get {
				return this.exchangeRateField;
			}
			set {
				this.exchangeRateField = value;
			}
		}
	}
}