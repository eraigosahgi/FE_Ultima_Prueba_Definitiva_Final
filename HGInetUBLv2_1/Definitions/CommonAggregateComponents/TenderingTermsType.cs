﻿/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("TenderingTerms", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class TenderingTermsType {
    
	private AwardingMethodTypeCodeType awardingMethodTypeCodeField;
    
	private PriceEvaluationCodeType priceEvaluationCodeField;
    
	private MaximumVariantQuantityType maximumVariantQuantityField;
    
	private VariantConstraintIndicatorType variantConstraintIndicatorField;
    
	private AcceptedVariantsDescriptionType[] acceptedVariantsDescriptionField;
    
	private PriceRevisionFormulaDescriptionType[] priceRevisionFormulaDescriptionField;
    
	private FundingProgramCodeType fundingProgramCodeField;
    
	private FundingProgramType[] fundingProgramField;
    
	private MaximumAdvertisementAmountType maximumAdvertisementAmountField;
    
	private NoteType[] noteField;
    
	private PaymentFrequencyCodeType paymentFrequencyCodeField;
    
	private EconomicOperatorRegistryURIType economicOperatorRegistryURIField;
    
	private RequiredCurriculaIndicatorType requiredCurriculaIndicatorField;
    
	private OtherConditionsIndicatorType otherConditionsIndicatorField;
    
	private AdditionalConditionsType[] additionalConditionsField;
    
	private LatestSecurityClearanceDateType latestSecurityClearanceDateField;
    
	private DocumentationFeeAmountType documentationFeeAmountField;
    
	private ClauseType[] penaltyClauseField;
    
	private FinancialGuaranteeType[] requiredFinancialGuaranteeField;
    
	private DocumentReferenceType procurementLegislationDocumentReferenceField;
    
	private DocumentReferenceType fiscalLegislationDocumentReferenceField;
    
	private DocumentReferenceType environmentalLegislationDocumentReferenceField;
    
	private DocumentReferenceType employmentLegislationDocumentReferenceField;
    
	private DocumentReferenceType[] contractualDocumentReferenceField;
    
	private DocumentReferenceType callForTendersDocumentReferenceField;
    
	private PeriodType warrantyValidityPeriodField;
    
	private PaymentTermsType[] paymentTermsField;
    
	private TendererQualificationRequestType[] tendererQualificationRequestField;
    
	private SubcontractTermsType[] allowedSubcontractTermsField;
    
	private TenderPreparationType[] tenderPreparationField;
    
	private ContractExecutionRequirementType[] contractExecutionRequirementField;
    
	private AwardingTermsType awardingTermsField;
    
	private PartyType additionalInformationPartyField;
    
	private PartyType documentProviderPartyField;
    
	private PartyType tenderRecipientPartyField;
    
	private PartyType contractResponsiblePartyField;
    
	private PartyType[] tenderEvaluationPartyField;
    
	private PeriodType tenderValidityPeriodField;
    
	private PeriodType contractAcceptancePeriodField;
    
	private AppealTermsType appealTermsField;
    
	private LanguageType[] languageField;
    
	private BudgetAccountLineType[] budgetAccountLineField;
    
	private DocumentReferenceType replacedNoticeDocumentReferenceField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public AwardingMethodTypeCodeType AwardingMethodTypeCode {
		get {
			return this.awardingMethodTypeCodeField;
		}
		set {
			this.awardingMethodTypeCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public PriceEvaluationCodeType PriceEvaluationCode {
		get {
			return this.priceEvaluationCodeField;
		}
		set {
			this.priceEvaluationCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public MaximumVariantQuantityType MaximumVariantQuantity {
		get {
			return this.maximumVariantQuantityField;
		}
		set {
			this.maximumVariantQuantityField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public VariantConstraintIndicatorType VariantConstraintIndicator {
		get {
			return this.variantConstraintIndicatorField;
		}
		set {
			this.variantConstraintIndicatorField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("AcceptedVariantsDescription", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public AcceptedVariantsDescriptionType[] AcceptedVariantsDescription {
		get {
			return this.acceptedVariantsDescriptionField;
		}
		set {
			this.acceptedVariantsDescriptionField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("PriceRevisionFormulaDescription", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public PriceRevisionFormulaDescriptionType[] PriceRevisionFormulaDescription {
		get {
			return this.priceRevisionFormulaDescriptionField;
		}
		set {
			this.priceRevisionFormulaDescriptionField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public FundingProgramCodeType FundingProgramCode {
		get {
			return this.fundingProgramCodeField;
		}
		set {
			this.fundingProgramCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("FundingProgram", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public FundingProgramType[] FundingProgram {
		get {
			return this.fundingProgramField;
		}
		set {
			this.fundingProgramField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public MaximumAdvertisementAmountType MaximumAdvertisementAmount {
		get {
			return this.maximumAdvertisementAmountField;
		}
		set {
			this.maximumAdvertisementAmountField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("Note", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public NoteType[] Note {
		get {
			return this.noteField;
		}
		set {
			this.noteField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public PaymentFrequencyCodeType PaymentFrequencyCode {
		get {
			return this.paymentFrequencyCodeField;
		}
		set {
			this.paymentFrequencyCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public EconomicOperatorRegistryURIType EconomicOperatorRegistryURI {
		get {
			return this.economicOperatorRegistryURIField;
		}
		set {
			this.economicOperatorRegistryURIField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public RequiredCurriculaIndicatorType RequiredCurriculaIndicator {
		get {
			return this.requiredCurriculaIndicatorField;
		}
		set {
			this.requiredCurriculaIndicatorField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public OtherConditionsIndicatorType OtherConditionsIndicator {
		get {
			return this.otherConditionsIndicatorField;
		}
		set {
			this.otherConditionsIndicatorField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("AdditionalConditions", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public AdditionalConditionsType[] AdditionalConditions {
		get {
			return this.additionalConditionsField;
		}
		set {
			this.additionalConditionsField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public LatestSecurityClearanceDateType LatestSecurityClearanceDate {
		get {
			return this.latestSecurityClearanceDateField;
		}
		set {
			this.latestSecurityClearanceDateField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public DocumentationFeeAmountType DocumentationFeeAmount {
		get {
			return this.documentationFeeAmountField;
		}
		set {
			this.documentationFeeAmountField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("PenaltyClause")]
	public ClauseType[] PenaltyClause {
		get {
			return this.penaltyClauseField;
		}
		set {
			this.penaltyClauseField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("RequiredFinancialGuarantee")]
	public FinancialGuaranteeType[] RequiredFinancialGuarantee {
		get {
			return this.requiredFinancialGuaranteeField;
		}
		set {
			this.requiredFinancialGuaranteeField = value;
		}
	}
    
	/// <comentarios/>
	public DocumentReferenceType ProcurementLegislationDocumentReference {
		get {
			return this.procurementLegislationDocumentReferenceField;
		}
		set {
			this.procurementLegislationDocumentReferenceField = value;
		}
	}
    
	/// <comentarios/>
	public DocumentReferenceType FiscalLegislationDocumentReference {
		get {
			return this.fiscalLegislationDocumentReferenceField;
		}
		set {
			this.fiscalLegislationDocumentReferenceField = value;
		}
	}
    
	/// <comentarios/>
	public DocumentReferenceType EnvironmentalLegislationDocumentReference {
		get {
			return this.environmentalLegislationDocumentReferenceField;
		}
		set {
			this.environmentalLegislationDocumentReferenceField = value;
		}
	}
    
	/// <comentarios/>
	public DocumentReferenceType EmploymentLegislationDocumentReference {
		get {
			return this.employmentLegislationDocumentReferenceField;
		}
		set {
			this.employmentLegislationDocumentReferenceField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("ContractualDocumentReference")]
	public DocumentReferenceType[] ContractualDocumentReference {
		get {
			return this.contractualDocumentReferenceField;
		}
		set {
			this.contractualDocumentReferenceField = value;
		}
	}
    
	/// <comentarios/>
	public DocumentReferenceType CallForTendersDocumentReference {
		get {
			return this.callForTendersDocumentReferenceField;
		}
		set {
			this.callForTendersDocumentReferenceField = value;
		}
	}
    
	/// <comentarios/>
	public PeriodType WarrantyValidityPeriod {
		get {
			return this.warrantyValidityPeriodField;
		}
		set {
			this.warrantyValidityPeriodField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("PaymentTerms")]
	public PaymentTermsType[] PaymentTerms {
		get {
			return this.paymentTermsField;
		}
		set {
			this.paymentTermsField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("TendererQualificationRequest")]
	public TendererQualificationRequestType[] TendererQualificationRequest {
		get {
			return this.tendererQualificationRequestField;
		}
		set {
			this.tendererQualificationRequestField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("AllowedSubcontractTerms")]
	public SubcontractTermsType[] AllowedSubcontractTerms {
		get {
			return this.allowedSubcontractTermsField;
		}
		set {
			this.allowedSubcontractTermsField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("TenderPreparation")]
	public TenderPreparationType[] TenderPreparation {
		get {
			return this.tenderPreparationField;
		}
		set {
			this.tenderPreparationField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("ContractExecutionRequirement")]
	public ContractExecutionRequirementType[] ContractExecutionRequirement {
		get {
			return this.contractExecutionRequirementField;
		}
		set {
			this.contractExecutionRequirementField = value;
		}
	}
    
	/// <comentarios/>
	public AwardingTermsType AwardingTerms {
		get {
			return this.awardingTermsField;
		}
		set {
			this.awardingTermsField = value;
		}
	}
    
	/// <comentarios/>
	public PartyType AdditionalInformationParty {
		get {
			return this.additionalInformationPartyField;
		}
		set {
			this.additionalInformationPartyField = value;
		}
	}
    
	/// <comentarios/>
	public PartyType DocumentProviderParty {
		get {
			return this.documentProviderPartyField;
		}
		set {
			this.documentProviderPartyField = value;
		}
	}
    
	/// <comentarios/>
	public PartyType TenderRecipientParty {
		get {
			return this.tenderRecipientPartyField;
		}
		set {
			this.tenderRecipientPartyField = value;
		}
	}
    
	/// <comentarios/>
	public PartyType ContractResponsibleParty {
		get {
			return this.contractResponsiblePartyField;
		}
		set {
			this.contractResponsiblePartyField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("TenderEvaluationParty")]
	public PartyType[] TenderEvaluationParty {
		get {
			return this.tenderEvaluationPartyField;
		}
		set {
			this.tenderEvaluationPartyField = value;
		}
	}
    
	/// <comentarios/>
	public PeriodType TenderValidityPeriod {
		get {
			return this.tenderValidityPeriodField;
		}
		set {
			this.tenderValidityPeriodField = value;
		}
	}
    
	/// <comentarios/>
	public PeriodType ContractAcceptancePeriod {
		get {
			return this.contractAcceptancePeriodField;
		}
		set {
			this.contractAcceptancePeriodField = value;
		}
	}
    
	/// <comentarios/>
	public AppealTermsType AppealTerms {
		get {
			return this.appealTermsField;
		}
		set {
			this.appealTermsField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("Language")]
	public LanguageType[] Language {
		get {
			return this.languageField;
		}
		set {
			this.languageField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("BudgetAccountLine")]
	public BudgetAccountLineType[] BudgetAccountLine {
		get {
			return this.budgetAccountLineField;
		}
		set {
			this.budgetAccountLineField = value;
		}
	}
    
	/// <comentarios/>
	public DocumentReferenceType ReplacedNoticeDocumentReference {
		get {
			return this.replacedNoticeDocumentReferenceField;
		}
		set {
			this.replacedNoticeDocumentReferenceField = value;
		}
	}
}