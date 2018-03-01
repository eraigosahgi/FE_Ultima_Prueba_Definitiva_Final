using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("BillingReference", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
	public partial class BillingReferenceType {
        
		private DocumentReferenceType invoiceDocumentReferenceField;
        
		private DocumentReferenceType selfBilledInvoiceDocumentReferenceField;
        
		private DocumentReferenceType creditNoteDocumentReferenceField;
        
		private DocumentReferenceType selfBilledCreditNoteDocumentReferenceField;
        
		private DocumentReferenceType debitNoteDocumentReferenceField;
        
		private DocumentReferenceType reminderDocumentReferenceField;
        
		private DocumentReferenceType additionalDocumentReferenceField;
        
		private BillingReferenceLineType[] billingReferenceLineField;
        
		/// <comentarios/>
		public DocumentReferenceType InvoiceDocumentReference {
			get {
				return this.invoiceDocumentReferenceField;
			}
			set {
				this.invoiceDocumentReferenceField = value;
			}
		}
        
		/// <comentarios/>
		public DocumentReferenceType SelfBilledInvoiceDocumentReference {
			get {
				return this.selfBilledInvoiceDocumentReferenceField;
			}
			set {
				this.selfBilledInvoiceDocumentReferenceField = value;
			}
		}
        
		/// <comentarios/>
		public DocumentReferenceType CreditNoteDocumentReference {
			get {
				return this.creditNoteDocumentReferenceField;
			}
			set {
				this.creditNoteDocumentReferenceField = value;
			}
		}
        
		/// <comentarios/>
		public DocumentReferenceType SelfBilledCreditNoteDocumentReference {
			get {
				return this.selfBilledCreditNoteDocumentReferenceField;
			}
			set {
				this.selfBilledCreditNoteDocumentReferenceField = value;
			}
		}
        
		/// <comentarios/>
		public DocumentReferenceType DebitNoteDocumentReference {
			get {
				return this.debitNoteDocumentReferenceField;
			}
			set {
				this.debitNoteDocumentReferenceField = value;
			}
		}
        
		/// <comentarios/>
		public DocumentReferenceType ReminderDocumentReference {
			get {
				return this.reminderDocumentReferenceField;
			}
			set {
				this.reminderDocumentReferenceField = value;
			}
		}
        
		/// <comentarios/>
		public DocumentReferenceType AdditionalDocumentReference {
			get {
				return this.additionalDocumentReferenceField;
			}
			set {
				this.additionalDocumentReferenceField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("BillingReferenceLine")]
		public BillingReferenceLineType[] BillingReferenceLine {
			get {
				return this.billingReferenceLineField;
			}
			set {
				this.billingReferenceLineField = value;
			}
		}
	}
}