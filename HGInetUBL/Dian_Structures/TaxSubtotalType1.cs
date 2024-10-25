using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName="TaxSubtotalType", Namespace="http://www.dian.gov.co/contratos/facturaelectronica/v1")]
	[System.Xml.Serialization.XmlRootAttribute("TaxSubtotal", Namespace="http://www.dian.gov.co/contratos/facturaelectronica/v1", IsNullable=false)]
	public partial class TaxSubtotalType1 {
        
		private TaxableAmountType taxableAmountField;
        
		private TaxAmountType taxAmountField;
        
		private CalculationSequenceNumericType calculationSequenceNumericField;
        
		private TransactionCurrencyTaxAmountType transactionCurrencyTaxAmountField;
        
		private PercentType percentField;
        
		private BaseUnitMeasureType baseUnitMeasureField;
        
		private PerUnitAmountType perUnitAmountField;
        
		private TierRangeType tierRangeField;
        
		private TierRatePercentType tierRatePercentField;
        
		private TaxCategoryType taxCategoryField;
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public TaxableAmountType TaxableAmount {
			get {
				return this.taxableAmountField;
			}
			set {
				this.taxableAmountField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public TaxAmountType TaxAmount {
			get {
				return this.taxAmountField;
			}
			set {
				this.taxAmountField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public CalculationSequenceNumericType CalculationSequenceNumeric {
			get {
				return this.calculationSequenceNumericField;
			}
			set {
				this.calculationSequenceNumericField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public TransactionCurrencyTaxAmountType TransactionCurrencyTaxAmount {
			get {
				return this.transactionCurrencyTaxAmountField;
			}
			set {
				this.transactionCurrencyTaxAmountField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public PercentType Percent {
			get {
				return this.percentField;
			}
			set {
				this.percentField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public BaseUnitMeasureType BaseUnitMeasure {
			get {
				return this.baseUnitMeasureField;
			}
			set {
				this.baseUnitMeasureField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public PerUnitAmountType PerUnitAmount {
			get {
				return this.perUnitAmountField;
			}
			set {
				this.perUnitAmountField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public TierRangeType TierRange {
			get {
				return this.tierRangeField;
			}
			set {
				this.tierRangeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public TierRatePercentType TierRatePercent {
			get {
				return this.tierRatePercentField;
			}
			set {
				this.tierRatePercentField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public TaxCategoryType TaxCategory {
			get {
				return this.taxCategoryField;
			}
			set {
				this.taxCategoryField = value;
			}
		}
	}
}