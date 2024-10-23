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
    
	private RegistrationDateType registrationDateField;
    
	private RegistrationExpirationDateType registrationExpirationDateField;
    
	private CompanyLegalFormCodeType companyLegalFormCodeField;
    
	private CompanyLegalFormType companyLegalFormField;
    
	private SoleProprietorshipIndicatorType soleProprietorshipIndicatorField;
    
	private CompanyLiquidationStatusCodeType companyLiquidationStatusCodeField;
    
	private CorporateStockAmountType corporateStockAmountField;
    
	private FullyPaidSharesIndicatorType fullyPaidSharesIndicatorField;
    
	private AddressType registrationAddressField;
    
	private CorporateRegistrationSchemeType corporateRegistrationSchemeField;
    
	private PartyType headOfficePartyField;
    
	private ShareholderPartyType[] shareholderPartyField;
    
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
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public RegistrationDateType RegistrationDate {
		get {
			return this.registrationDateField;
		}
		set {
			this.registrationDateField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public RegistrationExpirationDateType RegistrationExpirationDate {
		get {
			return this.registrationExpirationDateField;
		}
		set {
			this.registrationExpirationDateField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public CompanyLegalFormCodeType CompanyLegalFormCode {
		get {
			return this.companyLegalFormCodeField;
		}
		set {
			this.companyLegalFormCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public CompanyLegalFormType CompanyLegalForm {
		get {
			return this.companyLegalFormField;
		}
		set {
			this.companyLegalFormField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public SoleProprietorshipIndicatorType SoleProprietorshipIndicator {
		get {
			return this.soleProprietorshipIndicatorField;
		}
		set {
			this.soleProprietorshipIndicatorField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public CompanyLiquidationStatusCodeType CompanyLiquidationStatusCode {
		get {
			return this.companyLiquidationStatusCodeField;
		}
		set {
			this.companyLiquidationStatusCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public CorporateStockAmountType CorporateStockAmount {
		get {
			return this.corporateStockAmountField;
		}
		set {
			this.corporateStockAmountField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public FullyPaidSharesIndicatorType FullyPaidSharesIndicator {
		get {
			return this.fullyPaidSharesIndicatorField;
		}
		set {
			this.fullyPaidSharesIndicatorField = value;
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
    
	/// <comentarios/>
	public PartyType HeadOfficeParty {
		get {
			return this.headOfficePartyField;
		}
		set {
			this.headOfficePartyField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("ShareholderParty")]
	public ShareholderPartyType[] ShareholderParty {
		get {
			return this.shareholderPartyField;
		}
		set {
			this.shareholderPartyField = value;
		}
	}
}