﻿/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("CrewMemberPerson", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class PersonType {
    
	private IDType idField;
    
	private FirstNameType firstNameField;
    
	private FamilyNameType familyNameField;
    
	private TitleType titleField;
    
	private MiddleNameType middleNameField;
    
	private OtherNameType otherNameField;
    
	private NameSuffixType nameSuffixField;
    
	private JobTitleType jobTitleField;
    
	private NationalityIDType nationalityIDField;
    
	private GenderCodeType genderCodeField;
    
	private BirthDateType birthDateField;
    
	private BirthplaceNameType birthplaceNameField;
    
	private OrganizationDepartmentType organizationDepartmentField;
    
	private ContactType contactField;
    
	private FinancialAccountType financialAccountField;
    
	private DocumentReferenceType[] identityDocumentReferenceField;
    
	private AddressType residenceAddressField;
    
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
	public FirstNameType FirstName {
		get {
			return this.firstNameField;
		}
		set {
			this.firstNameField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public FamilyNameType FamilyName {
		get {
			return this.familyNameField;
		}
		set {
			this.familyNameField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public TitleType Title {
		get {
			return this.titleField;
		}
		set {
			this.titleField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public MiddleNameType MiddleName {
		get {
			return this.middleNameField;
		}
		set {
			this.middleNameField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public OtherNameType OtherName {
		get {
			return this.otherNameField;
		}
		set {
			this.otherNameField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public NameSuffixType NameSuffix {
		get {
			return this.nameSuffixField;
		}
		set {
			this.nameSuffixField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public JobTitleType JobTitle {
		get {
			return this.jobTitleField;
		}
		set {
			this.jobTitleField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public NationalityIDType NationalityID {
		get {
			return this.nationalityIDField;
		}
		set {
			this.nationalityIDField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public GenderCodeType GenderCode {
		get {
			return this.genderCodeField;
		}
		set {
			this.genderCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public BirthDateType BirthDate {
		get {
			return this.birthDateField;
		}
		set {
			this.birthDateField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public BirthplaceNameType BirthplaceName {
		get {
			return this.birthplaceNameField;
		}
		set {
			this.birthplaceNameField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public OrganizationDepartmentType OrganizationDepartment {
		get {
			return this.organizationDepartmentField;
		}
		set {
			this.organizationDepartmentField = value;
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
	public FinancialAccountType FinancialAccount {
		get {
			return this.financialAccountField;
		}
		set {
			this.financialAccountField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("IdentityDocumentReference")]
	public DocumentReferenceType[] IdentityDocumentReference {
		get {
			return this.identityDocumentReferenceField;
		}
		set {
			this.identityDocumentReferenceField = value;
		}
	}
    
	/// <comentarios/>
	public AddressType ResidenceAddress {
		get {
			return this.residenceAddressField;
		}
		set {
			this.residenceAddressField = value;
		}
	}
}