using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ValueAmountType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TransactionCurrencyTaxAmountType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TotalTaxAmountType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TotalPaymentAmountType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TotalInvoiceAmountType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TotalDebitAmountType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TotalCreditAmountType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TotalBalanceAmountType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TaxInclusiveAmountType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TaxExclusiveAmountType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TaxAmountType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TaxableAmountType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(RoundingAmountType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PriceAmountType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PrepaidAmountType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PerUnitAmountType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PaymentAmountType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PayableRoundingAmountType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PayableAmountType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PaidAmountType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(LineExtensionAmountType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(LineAmountType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(InvoiceAmountType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(InsuranceValueAmountType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(InsurancePremiumAmountType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(FreeOnBoardValueAmountType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(DeclaredStatisticsValueAmountType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(DeclaredForCarriageValueAmountType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(DeclaredCustomsValueAmountType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(DeclaredCarriageValueAmountType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(DebitLineAmountType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(DebitAmountType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(CreditLineAmountType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(CreditAmountType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ChargeTotalAmountType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(BaseAmountType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(BalanceAmountType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(AmountType1))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(AllowanceTotalAmountType))]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2")]
	public partial class AmountType {
        
		private CurrencyCodeContentType currencyIDField;
        
		private decimal valueField;
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public CurrencyCodeContentType currencyID {
			get {
				return this.currencyIDField;
			}
			set {
				this.currencyIDField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlTextAttribute()]
		public decimal Value {
			get {
				return this.valueField;
			}
			set {
				this.valueField = value;
			}
		}
	}
}