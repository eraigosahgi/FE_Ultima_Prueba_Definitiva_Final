/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("AdditionalInformationParty", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class PartyType {
    
	private MarkCareIndicatorType markCareIndicatorField;
    
	private MarkAttentionIndicatorType markAttentionIndicatorField;
    
	private WebsiteURIType websiteURIField;
    
	private LogoReferenceIDType logoReferenceIDField;
    
	private EndpointIDType endpointIDField;
    
	private IndustryClassificationCodeType industryClassificationCodeField;
    
	private PartyIdentificationType[] partyIdentificationField;
    
	private PartyNameType[] partyNameField;
    
	private LanguageType languageField;
    
	private AddressType postalAddressField;
    
	private LocationType1 physicalLocationField;
    
	private PartyTaxSchemeType[] partyTaxSchemeField;
    
	private PartyLegalEntityType[] partyLegalEntityField;
    
	private ContactType contactField;
    
	private PersonType[] personField;
    
	private PartyType agentPartyField;
    
	private ServiceProviderPartyType[] serviceProviderPartyField;
    
	private PowerOfAttorneyType[] powerOfAttorneyField;
    
	private FinancialAccountType financialAccountField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public MarkCareIndicatorType MarkCareIndicator {
		get {
			return this.markCareIndicatorField;
		}
		set {
			this.markCareIndicatorField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public MarkAttentionIndicatorType MarkAttentionIndicator {
		get {
			return this.markAttentionIndicatorField;
		}
		set {
			this.markAttentionIndicatorField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public WebsiteURIType WebsiteURI {
		get {
			return this.websiteURIField;
		}
		set {
			this.websiteURIField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public LogoReferenceIDType LogoReferenceID {
		get {
			return this.logoReferenceIDField;
		}
		set {
			this.logoReferenceIDField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public EndpointIDType EndpointID {
		get {
			return this.endpointIDField;
		}
		set {
			this.endpointIDField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public IndustryClassificationCodeType IndustryClassificationCode {
		get {
			return this.industryClassificationCodeField;
		}
		set {
			this.industryClassificationCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("PartyIdentification")]
	public PartyIdentificationType[] PartyIdentification {
		get {
			return this.partyIdentificationField;
		}
		set {
			this.partyIdentificationField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("PartyName")]
	public PartyNameType[] PartyName {
		get {
			return this.partyNameField;
		}
		set {
			this.partyNameField = value;
		}
	}
    
	/// <comentarios/>
	public LanguageType Language {
		get {
			return this.languageField;
		}
		set {
			this.languageField = value;
		}
	}
    
	/// <comentarios/>
	public AddressType PostalAddress {
		get {
			return this.postalAddressField;
		}
		set {
			this.postalAddressField = value;
		}
	}
    
	/// <comentarios/>
	public LocationType1 PhysicalLocation {
		get {
			return this.physicalLocationField;
		}
		set {
			this.physicalLocationField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("PartyTaxScheme")]
	public PartyTaxSchemeType[] PartyTaxScheme {
		get {
			return this.partyTaxSchemeField;
		}
		set {
			this.partyTaxSchemeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("PartyLegalEntity")]
	public PartyLegalEntityType[] PartyLegalEntity {
		get {
			return this.partyLegalEntityField;
		}
		set {
			this.partyLegalEntityField = value;
		}
	}
    
	/// <comentarios/>
	public ContactType Contact {
		get {
			return this.contactField;
		}
		set {
			this.contactField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("Person")]
	public PersonType[] Person {
		get {
			return this.personField;
		}
		set {
			this.personField = value;
		}
	}
    
	/// <comentarios/>
	public PartyType AgentParty {
		get {
			return this.agentPartyField;
		}
		set {
			this.agentPartyField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("ServiceProviderParty")]
	public ServiceProviderPartyType[] ServiceProviderParty {
		get {
			return this.serviceProviderPartyField;
		}
		set {
			this.serviceProviderPartyField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("PowerOfAttorney")]
	public PowerOfAttorneyType[] PowerOfAttorney {
		get {
			return this.powerOfAttorneyField;
		}
		set {
			this.powerOfAttorneyField = value;
		}
	}
    
	/// <comentarios/>
	public FinancialAccountType FinancialAccount {
		get {
			return this.financialAccountField;
		}
		set {
			this.financialAccountField = value;
		}
	}
}