using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.dian.gov.co/contratos/facturaelectronica/v1/Structures")]
	public partial class InvoiceControl {
        
		private decimal invoiceAuthorizationField;
        
		private PeriodType authorizationPeriodField;
        
		private AuthrorizedInvoices authorizedInvoicesField;
        
		/// <comentarios/>
		public decimal InvoiceAuthorization {
			get {
				return this.invoiceAuthorizationField;
			}
			set {
				this.invoiceAuthorizationField = value;
			}
		}
        
		/// <comentarios/>
		public PeriodType AuthorizationPeriod {
			get {
				return this.authorizationPeriodField;
			}
			set {
				this.authorizationPeriodField = value;
			}
		}
        
		/// <comentarios/>
		public AuthrorizedInvoices AuthorizedInvoices {
			get {
				return this.authorizedInvoicesField;
			}
			set {
				this.authorizedInvoicesField = value;
			}
		}
	}
}