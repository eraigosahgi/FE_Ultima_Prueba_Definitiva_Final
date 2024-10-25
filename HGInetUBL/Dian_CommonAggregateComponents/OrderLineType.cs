using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("OrderLine", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
	public partial class OrderLineType {
        
		private SubstitutionStatusCodeType1 substitutionStatusCodeField;
        
		private NoteType noteField;
        
		private LineItemType lineItemField;
        
		private LineItemType[] sellerProposedSubstituteLineItemField;
        
		private LineItemType[] sellerSubstitutedLineItemField;
        
		private LineItemType[] buyerProposedSubstituteLineItemField;
        
		private LineReferenceType catalogueLineReferenceField;
        
		private LineReferenceType quotationLineReferenceField;
        
		private DocumentReferenceType[] documentReferenceField;
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public SubstitutionStatusCodeType1 SubstitutionStatusCode {
			get {
				return this.substitutionStatusCodeField;
			}
			set {
				this.substitutionStatusCodeField = value;
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
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("SellerSubstitutedLineItem")]
		public LineItemType[] SellerSubstitutedLineItem {
			get {
				return this.sellerSubstitutedLineItemField;
			}
			set {
				this.sellerSubstitutedLineItemField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("BuyerProposedSubstituteLineItem")]
		public LineItemType[] BuyerProposedSubstituteLineItem {
			get {
				return this.buyerProposedSubstituteLineItemField;
			}
			set {
				this.buyerProposedSubstituteLineItemField = value;
			}
		}
        
		/// <comentarios/>
		public LineReferenceType CatalogueLineReference {
			get {
				return this.catalogueLineReferenceField;
			}
			set {
				this.catalogueLineReferenceField = value;
			}
		}
        
		/// <comentarios/>
		public LineReferenceType QuotationLineReference {
			get {
				return this.quotationLineReferenceField;
			}
			set {
				this.quotationLineReferenceField = value;
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
	}
}