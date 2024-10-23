using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("ApplicableTaxCategory", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
	public partial class TaxCategoryType {
        
		private IDType idField;
        
		private NameType1 nameField;
        
		private PercentType percentField;
        
		private BaseUnitMeasureType baseUnitMeasureField;
        
		private PerUnitAmountType perUnitAmountField;
        
		private TaxExemptionReasonCodeType taxExemptionReasonCodeField;
        
		private TaxExemptionReasonType taxExemptionReasonField;
        
		private TierRangeType tierRangeField;
        
		private TierRatePercentType tierRatePercentField;
        
		private TaxSchemeType taxSchemeField;
        
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
		public NameType1 Name {
			get {
				return this.nameField;
			}
			set {
				this.nameField = value;
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
		public TaxExemptionReasonCodeType TaxExemptionReasonCode {
			get {
				return this.taxExemptionReasonCodeField;
			}
			set {
				this.taxExemptionReasonCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public TaxExemptionReasonType TaxExemptionReason {
			get {
				return this.taxExemptionReasonField;
			}
			set {
				this.taxExemptionReasonField = value;
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
		public TaxSchemeType TaxScheme {
			get {
				return this.taxSchemeField;
			}
			set {
				this.taxSchemeField = value;
			}
		}
	}
}