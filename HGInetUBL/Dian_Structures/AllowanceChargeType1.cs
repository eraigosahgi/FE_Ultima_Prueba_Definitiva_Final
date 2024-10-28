using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName="AllowanceChargeType", Namespace="http://www.dian.gov.co/contratos/facturaelectronica/v1")]
	[System.Xml.Serialization.XmlRootAttribute("AllowanceCharge", Namespace="http://www.dian.gov.co/contratos/facturaelectronica/v1", IsNullable=false)]
	public partial class AllowanceChargeType1 {
        
		private IDType idField;
        
		private ChargeIndicatorType chargeIndicatorField;
        
		private AllowanceChargeReasonCodeType1 allowanceChargeReasonCodeField;
        
		private AllowanceChargeReasonType allowanceChargeReasonField;
        
		private MultiplierFactorNumericType multiplierFactorNumericField;
        
		private PrepaidIndicatorType prepaidIndicatorField;
        
		private SequenceNumericType sequenceNumericField;
        
		private AmountType1 amountField;
        
		private BaseAmountType baseAmountField;
        
		private AccountingCostCodeType accountingCostCodeField;
        
		private AccountingCostType accountingCostField;
        
		private TaxCategoryType[] taxCategoryField;
        
		private TaxTotalType taxTotalField;
        
		private PaymentMeansType[] paymentMeansField;
        
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
		public ChargeIndicatorType ChargeIndicator {
			get {
				return this.chargeIndicatorField;
			}
			set {
				this.chargeIndicatorField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public AllowanceChargeReasonCodeType1 AllowanceChargeReasonCode {
			get {
				return this.allowanceChargeReasonCodeField;
			}
			set {
				this.allowanceChargeReasonCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public AllowanceChargeReasonType AllowanceChargeReason {
			get {
				return this.allowanceChargeReasonField;
			}
			set {
				this.allowanceChargeReasonField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public MultiplierFactorNumericType MultiplierFactorNumeric {
			get {
				return this.multiplierFactorNumericField;
			}
			set {
				this.multiplierFactorNumericField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public PrepaidIndicatorType PrepaidIndicator {
			get {
				return this.prepaidIndicatorField;
			}
			set {
				this.prepaidIndicatorField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public SequenceNumericType SequenceNumeric {
			get {
				return this.sequenceNumericField;
			}
			set {
				this.sequenceNumericField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public AmountType1 Amount {
			get {
				return this.amountField;
			}
			set {
				this.amountField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public BaseAmountType BaseAmount {
			get {
				return this.baseAmountField;
			}
			set {
				this.baseAmountField = value;
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
		[System.Xml.Serialization.XmlElementAttribute("TaxCategory", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public TaxCategoryType[] TaxCategory {
			get {
				return this.taxCategoryField;
			}
			set {
				this.taxCategoryField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public TaxTotalType TaxTotal {
			get {
				return this.taxTotalField;
			}
			set {
				this.taxTotalField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("PaymentMeans", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public PaymentMeansType[] PaymentMeans {
			get {
				return this.paymentMeansField;
			}
			set {
				this.paymentMeansField = value;
			}
		}
	}
}