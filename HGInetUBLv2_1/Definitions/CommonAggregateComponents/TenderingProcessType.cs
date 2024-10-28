/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("TenderingProcess", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class TenderingProcessType {
    
	private IDType idField;
    
	private OriginalContractingSystemIDType originalContractingSystemIDField;
    
	private DescriptionType[] descriptionField;
    
	private NegotiationDescriptionType[] negotiationDescriptionField;
    
	private ProcedureCodeType procedureCodeField;
    
	private UrgencyCodeType urgencyCodeField;
    
	private ExpenseCodeType expenseCodeField;
    
	private PartPresentationCodeType partPresentationCodeField;
    
	private ContractingSystemCodeType contractingSystemCodeField;
    
	private SubmissionMethodCodeType submissionMethodCodeField;
    
	private CandidateReductionConstraintIndicatorType candidateReductionConstraintIndicatorField;
    
	private GovernmentAgreementConstraintIndicatorType governmentAgreementConstraintIndicatorField;
    
	private PeriodType documentAvailabilityPeriodField;
    
	private PeriodType tenderSubmissionDeadlinePeriodField;
    
	private PeriodType invitationSubmissionPeriodField;
    
	private PeriodType participationRequestReceptionPeriodField;
    
	private DocumentReferenceType[] noticeDocumentReferenceField;
    
	private DocumentReferenceType[] additionalDocumentReferenceField;
    
	private ProcessJustificationType[] processJustificationField;
    
	private EconomicOperatorShortListType economicOperatorShortListField;
    
	private EventType[] openTenderEventField;
    
	private AuctionTermsType auctionTermsField;
    
	private FrameworkAgreementType frameworkAgreementField;
    
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
	public OriginalContractingSystemIDType OriginalContractingSystemID {
		get {
			return this.originalContractingSystemIDField;
		}
		set {
			this.originalContractingSystemIDField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("Description", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public DescriptionType[] Description {
		get {
			return this.descriptionField;
		}
		set {
			this.descriptionField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("NegotiationDescription", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public NegotiationDescriptionType[] NegotiationDescription {
		get {
			return this.negotiationDescriptionField;
		}
		set {
			this.negotiationDescriptionField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ProcedureCodeType ProcedureCode {
		get {
			return this.procedureCodeField;
		}
		set {
			this.procedureCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public UrgencyCodeType UrgencyCode {
		get {
			return this.urgencyCodeField;
		}
		set {
			this.urgencyCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ExpenseCodeType ExpenseCode {
		get {
			return this.expenseCodeField;
		}
		set {
			this.expenseCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public PartPresentationCodeType PartPresentationCode {
		get {
			return this.partPresentationCodeField;
		}
		set {
			this.partPresentationCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ContractingSystemCodeType ContractingSystemCode {
		get {
			return this.contractingSystemCodeField;
		}
		set {
			this.contractingSystemCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public SubmissionMethodCodeType SubmissionMethodCode {
		get {
			return this.submissionMethodCodeField;
		}
		set {
			this.submissionMethodCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public CandidateReductionConstraintIndicatorType CandidateReductionConstraintIndicator {
		get {
			return this.candidateReductionConstraintIndicatorField;
		}
		set {
			this.candidateReductionConstraintIndicatorField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public GovernmentAgreementConstraintIndicatorType GovernmentAgreementConstraintIndicator {
		get {
			return this.governmentAgreementConstraintIndicatorField;
		}
		set {
			this.governmentAgreementConstraintIndicatorField = value;
		}
	}
    
	/// <comentarios/>
	public PeriodType DocumentAvailabilityPeriod {
		get {
			return this.documentAvailabilityPeriodField;
		}
		set {
			this.documentAvailabilityPeriodField = value;
		}
	}
    
	/// <comentarios/>
	public PeriodType TenderSubmissionDeadlinePeriod {
		get {
			return this.tenderSubmissionDeadlinePeriodField;
		}
		set {
			this.tenderSubmissionDeadlinePeriodField = value;
		}
	}
    
	/// <comentarios/>
	public PeriodType InvitationSubmissionPeriod {
		get {
			return this.invitationSubmissionPeriodField;
		}
		set {
			this.invitationSubmissionPeriodField = value;
		}
	}
    
	/// <comentarios/>
	public PeriodType ParticipationRequestReceptionPeriod {
		get {
			return this.participationRequestReceptionPeriodField;
		}
		set {
			this.participationRequestReceptionPeriodField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("NoticeDocumentReference")]
	public DocumentReferenceType[] NoticeDocumentReference {
		get {
			return this.noticeDocumentReferenceField;
		}
		set {
			this.noticeDocumentReferenceField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("AdditionalDocumentReference")]
	public DocumentReferenceType[] AdditionalDocumentReference {
		get {
			return this.additionalDocumentReferenceField;
		}
		set {
			this.additionalDocumentReferenceField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("ProcessJustification")]
	public ProcessJustificationType[] ProcessJustification {
		get {
			return this.processJustificationField;
		}
		set {
			this.processJustificationField = value;
		}
	}
    
	/// <comentarios/>
	public EconomicOperatorShortListType EconomicOperatorShortList {
		get {
			return this.economicOperatorShortListField;
		}
		set {
			this.economicOperatorShortListField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("OpenTenderEvent")]
	public EventType[] OpenTenderEvent {
		get {
			return this.openTenderEventField;
		}
		set {
			this.openTenderEventField = value;
		}
	}
    
	/// <comentarios/>
	public AuctionTermsType AuctionTerms {
		get {
			return this.auctionTermsField;
		}
		set {
			this.auctionTermsField = value;
		}
	}
    
	/// <comentarios/>
	public FrameworkAgreementType FrameworkAgreement {
		get {
			return this.frameworkAgreementField;
		}
		set {
			this.frameworkAgreementField = value;
		}
	}
}