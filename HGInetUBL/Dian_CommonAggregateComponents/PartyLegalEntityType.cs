using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("PartyLegalEntity", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
	public partial class PartyLegalEntityType {
        
		private RegistrationNameType registrationNameField;
        
		private CompanyIDType companyIDField;
        
		private AddressType registrationAddressField;
        
		private CorporateRegistrationSchemeType corporateRegistrationSchemeField;
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public RegistrationNameType RegistrationName {
			get {
				return this.registrationNameField;
			}
			set {
				this.registrationNameField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public CompanyIDType CompanyID {
			get {
				return this.companyIDField;
			}
			set {
				this.companyIDField = value;
			}
		}
        
		/// <comentarios/>
		public AddressType RegistrationAddress {
			get {
				return this.registrationAddressField;
			}
			set {
				this.registrationAddressField = value;
			}
		}
        
		/// <comentarios/>
		public CorporateRegistrationSchemeType CorporateRegistrationScheme {
			get {
				return this.corporateRegistrationSchemeField;
			}
			set {
				this.corporateRegistrationSchemeField = value;
			}
		}
	}
}