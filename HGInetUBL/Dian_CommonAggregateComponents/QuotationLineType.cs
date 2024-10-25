using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("QuotationLine", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
	public partial class QuotationLineType {
        
		private IDType idField;
        
		private NoteType noteField;
        
		private QuantityType1 quantityField;
        
		private LineExtensionAmountType lineExtensionAmountField;
        
		private TotalTaxAmountType totalTaxAmountField;
        
		private DocumentReferenceType[] documentReferenceField;
        
		private LineItemType lineItemField;
        
		private LineItemType[] sellerProposedSubstituteLineItemField;
        
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
		[System.Xml.Serialization.XmlElementAttribute("DocumentReference")]
		public DocumentReferenceType[] DocumentReference {
			get {
				return this.documentReferenceField;
			}
			set {
				this.documentReferenceField = value;
			}
		}
        
		/// <comentarios/>
		public LineItemType LineItem {
			get {
				return this.lineItemField;
			}
			set {
				this.lineItemField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("SellerProposedSubstituteLineItem")]
		public LineItemType[] SellerProposedSubstituteLineItem {
			get {
				return this.sellerProposedSubstituteLineItemField;
			}
			set {
				this.sellerProposedSubstituteLineItemField = value;
			}
		}
	}
}