/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="dian:gov:co:facturaelectronica:Structures-2-1")]
[System.Xml.Serialization.XmlRootAttribute("DianExtensions", Namespace="dian:gov:co:facturaelectronica:Structures-2-1", IsNullable=false)]
public partial class DianExtensionsType {
    
	private InvoiceControl invoiceControlField;
    
	private CountryType invoiceSourceField;
    
	private SoftwareProvider softwareProviderField;
    
	private IdentifierType1 softwareSecurityCodeField;
    
	private AuthorizationProvider authorizationProviderField;
    
	private string qRCodeField;
    
	private AdditionalMonetaryTotal additionalMonetaryTotalField;
    
	private MonetaryTotalType legalMonetaryTotalField;
    
	private FinancialInformation financialInformationField;
    
	/// <comentarios/>
	public InvoiceControl InvoiceControl {
		get {
			return this.invoiceControlField;
		}
		set {
			this.invoiceControlField = value;
		}
	}
    
	/// <comentarios/>
	public CountryType InvoiceSource {
		get {
			return this.invoiceSourceField;
		}
		set {
			this.invoiceSourceField = value;
		}
	}
    
	/// <comentarios/>
	public SoftwareProvider SoftwareProvider {
		get {
			return this.softwareProviderField;
		}
		set {
			this.softwareProviderField = value;
		}
	}
    
	/// <comentarios/>
	public IdentifierType1 SoftwareSecurityCode {
		get {
			return this.softwareSecurityCodeField;
		}
		set {
			this.softwareSecurityCodeField = value;
		}
	}
    
	/// <comentarios/>
	public AuthorizationProvider AuthorizationProvider {
		get {
			return this.authorizationProviderField;
		}
		set {
			this.authorizationProviderField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(DataType="anyURI")]
	public string QRCode {
		get {
			return this.qRCodeField;
		}
		set {
			this.qRCodeField = value;
		}
	}
    
	/// <comentarios/>
	public AdditionalMonetaryTotal AdditionalMonetaryTotal {
		get {
			return this.additionalMonetaryTotalField;
		}
		set {
			this.additionalMonetaryTotalField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
	public MonetaryTotalType LegalMonetaryTotal {
		get {
			return this.legalMonetaryTotalField;
		}
		set {
			this.legalMonetaryTotalField = value;
		}
	}
    
	/// <comentarios/>
	public FinancialInformation FinancialInformation {
		get {
			return this.financialInformationField;
		}
		set {
			this.financialInformationField = value;
		}
	}
}