using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("PaymentMeans", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
	public partial class PaymentMeansType {
        
		private IDType idField;
        
		private PaymentMeansCodeType1 paymentMeansCodeField;
        
		private PaymentDueDateType paymentDueDateField;
        
		private PaymentChannelCodeType paymentChannelCodeField;
        
		private InstructionIDType instructionIDField;
        
		private InstructionNoteType[] instructionNoteField;
        
		private PaymentIDType[] paymentIDField;
        
		private CardAccountType cardAccountField;
        
		private FinancialAccountType payerFinancialAccountField;
        
		private FinancialAccountType payeeFinancialAccountField;
        
		private CreditAccountType creditAccountField;
        
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
		public PaymentMeansCodeType1 PaymentMeansCode {
			get {
				return this.paymentMeansCodeField;
			}
			set {
				this.paymentMeansCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public PaymentDueDateType PaymentDueDate {
			get {
				return this.paymentDueDateField;
			}
			set {
				this.paymentDueDateField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public PaymentChannelCodeType PaymentChannelCode {
			get {
				return this.paymentChannelCodeField;
			}
			set {
				this.paymentChannelCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public InstructionIDType InstructionID {
			get {
				return this.instructionIDField;
			}
			set {
				this.instructionIDField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("InstructionNote", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public InstructionNoteType[] InstructionNote {
			get {
				return this.instructionNoteField;
			}
			set {
				this.instructionNoteField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("PaymentID", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public PaymentIDType[] PaymentID {
			get {
				return this.paymentIDField;
			}
			set {
				this.paymentIDField = value;
			}
		}
        
		/// <comentarios/>
		public CardAccountType CardAccount {
			get {
				return this.cardAccountField;
			}
			set {
				this.cardAccountField = value;
			}
		}
        
		/// <comentarios/>
		public FinancialAccountType PayerFinancialAccount {
			get {
				return this.payerFinancialAccountField;
			}
			set {
				this.payerFinancialAccountField = value;
			}
		}
        
		/// <comentarios/>
		public FinancialAccountType PayeeFinancialAccount {
			get {
				return this.payeeFinancialAccountField;
			}
			set {
				this.payeeFinancialAccountField = value;
			}
		}
        
		/// <comentarios/>
		public CreditAccountType CreditAccount {
			get {
				return this.creditAccountField;
			}
			set {
				this.creditAccountField = value;
			}
		}
	}
}