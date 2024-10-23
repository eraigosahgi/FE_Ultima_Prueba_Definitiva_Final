/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="dian:gov:co:facturaelectronica:Structures-2-1")]
public partial class InvoiceControl {
    
	private NumericType1 invoiceAuthorizationField;
    
	private PeriodType authorizationPeriodField;
    
	private AuthrorizedInvoices authorizedInvoicesField;
    
	/// <comentarios/>
	public NumericType1 InvoiceAuthorization {
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