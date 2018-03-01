using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.dian.gov.co/contratos/facturaelectronica/v1/Structures")]
	public partial class FinancialInformation {
        
		private PartyType assigneeField;
        
		private FinancialAccountType paymentDetailsField;
        
		private TextType clauseField;
        
		private DocumentReferenceType[] documentReferenceField;
        
		/// <comentarios/>
		public PartyType Assignee {
			get {
				return this.assigneeField;
			}
			set {
				this.assigneeField = value;
			}
		}
        
		/// <comentarios/>
		public FinancialAccountType PaymentDetails {
			get {
				return this.paymentDetailsField;
			}
			set {
				this.paymentDetailsField = value;
			}
		}
        
		/// <comentarios/>
		public TextType Clause {
			get {
				return this.clauseField;
			}
			set {
				this.clauseField = value;
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