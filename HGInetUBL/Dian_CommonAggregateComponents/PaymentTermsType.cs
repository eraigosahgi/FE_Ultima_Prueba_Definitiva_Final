using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("PaymentTerms", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
	public partial class PaymentTermsType {
        
		private IDType idField;
        
		private PaymentMeansIDType paymentMeansIDField;
        
		private PrepaidPaymentReferenceIDType prepaidPaymentReferenceIDField;
        
		private NoteType[] noteField;
        
		private ReferenceEventCodeType referenceEventCodeField;
        
		private SettlementDiscountPercentType settlementDiscountPercentField;
        
		private PenaltySurchargePercentType penaltySurchargePercentField;
        
		private AmountType1 amountField;
        
		private PeriodType settlementPeriodField;
        
		private PeriodType penaltyPeriodField;
        
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
		public PaymentMeansIDType PaymentMeansID {
			get {
				return this.paymentMeansIDField;
			}
			set {
				this.paymentMeansIDField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public PrepaidPaymentReferenceIDType PrepaidPaymentReferenceID {
			get {
				return this.prepaidPaymentReferenceIDField;
			}
			set {
				this.prepaidPaymentReferenceIDField = value;
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
		public ReferenceEventCodeType ReferenceEventCode {
			get {
				return this.referenceEventCodeField;
			}
			set {
				this.referenceEventCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public SettlementDiscountPercentType SettlementDiscountPercent {
			get {
				return this.settlementDiscountPercentField;
			}
			set {
				this.settlementDiscountPercentField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public PenaltySurchargePercentType PenaltySurchargePercent {
			get {
				return this.penaltySurchargePercentField;
			}
			set {
				this.penaltySurchargePercentField = value;
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
		public PeriodType SettlementPeriod {
			get {
				return this.settlementPeriodField;
			}
			set {
				this.settlementPeriodField = value;
			}
		}
        
		/// <comentarios/>
		public PeriodType PenaltyPeriod {
			get {
				return this.penaltyPeriodField;
			}
			set {
				this.penaltyPeriodField = value;
			}
		}
	}
}