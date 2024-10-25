using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.dian.gov.co/contratos/facturaelectronica/v1/Structures")]
	[System.Xml.Serialization.XmlRootAttribute("DianExtensions", Namespace="http://www.dian.gov.co/contratos/facturaelectronica/v1/Structures", IsNullable=false)]
	public partial class DianExtensionsType {
        
		private InvoiceControl invoiceControlField;
        
		private CountryType invoiceSourceField;
        
		private SoftwareProvider softwareProviderField;
        
		private IdentifierType softwareSecurityCodeField;
        
		private AdditionalMonetaryTotal additionalMonetaryTotalField;
        
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
		public IdentifierType SoftwareSecurityCode {
			get {
				return this.softwareSecurityCodeField;
			}
			set {
				this.softwareSecurityCodeField = value;
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
		public FinancialInformation FinancialInformation {
			get {
				return this.financialInformationField;
			}
			set {
				this.financialInformationField = value;
			}
		}
	}
}