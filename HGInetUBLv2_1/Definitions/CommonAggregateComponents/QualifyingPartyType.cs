/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("AdditionalQualifyingParty", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class QualifyingPartyType {
    
	private ParticipationPercentType participationPercentField;
    
	private PersonalSituationType[] personalSituationField;
    
	private OperatingYearsQuantityType operatingYearsQuantityField;
    
	private EmployeeQuantityType employeeQuantityField;
    
	private BusinessClassificationEvidenceIDType businessClassificationEvidenceIDField;
    
	private BusinessIdentityEvidenceIDType businessIdentityEvidenceIDField;
    
	private TendererRoleCodeType tendererRoleCodeField;
    
	private ClassificationSchemeType businessClassificationSchemeField;
    
	private CapabilityType[] technicalCapabilityField;
    
	private CapabilityType[] financialCapabilityField;
    
	private CompletedTaskType[] completedTaskField;
    
	private DeclarationType[] declarationField;
    
	private PartyType partyField;
    
	private EconomicOperatorRoleType economicOperatorRoleField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ParticipationPercentType ParticipationPercent {
		get {
			return this.participationPercentField;
		}
		set {
			this.participationPercentField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("PersonalSituation", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public PersonalSituationType[] PersonalSituation {
		get {
			return this.personalSituationField;
		}
		set {
			this.personalSituationField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public OperatingYearsQuantityType OperatingYearsQuantity {
		get {
			return this.operatingYearsQuantityField;
		}
		set {
			this.operatingYearsQuantityField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public EmployeeQuantityType EmployeeQuantity {
		get {
			return this.employeeQuantityField;
		}
		set {
			this.employeeQuantityField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public BusinessClassificationEvidenceIDType BusinessClassificationEvidenceID {
		get {
			return this.businessClassificationEvidenceIDField;
		}
		set {
			this.businessClassificationEvidenceIDField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public BusinessIdentityEvidenceIDType BusinessIdentityEvidenceID {
		get {
			return this.businessIdentityEvidenceIDField;
		}
		set {
			this.businessIdentityEvidenceIDField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public TendererRoleCodeType TendererRoleCode {
		get {
			return this.tendererRoleCodeField;
		}
		set {
			this.tendererRoleCodeField = value;
		}
	}
    
	/// <comentarios/>
	public ClassificationSchemeType BusinessClassificationScheme {
		get {
			return this.businessClassificationSchemeField;
		}
		set {
			this.businessClassificationSchemeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("TechnicalCapability")]
	public CapabilityType[] TechnicalCapability {
		get {
			return this.technicalCapabilityField;
		}
		set {
			this.technicalCapabilityField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("FinancialCapability")]
	public CapabilityType[] FinancialCapability {
		get {
			return this.financialCapabilityField;
		}
		set {
			this.financialCapabilityField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("CompletedTask")]
	public CompletedTaskType[] CompletedTask {
		get {
			return this.completedTaskField;
		}
		set {
			this.completedTaskField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("Declaration")]
	public DeclarationType[] Declaration {
		get {
			return this.declarationField;
		}
		set {
			this.declarationField = value;
		}
	}
    
	/// <comentarios/>
	public PartyType Party {
		get {
			return this.partyField;
		}
		set {
			this.partyField = value;
		}
	}
    
	/// <comentarios/>
	public EconomicOperatorRoleType EconomicOperatorRole {
		get {
			return this.economicOperatorRoleField;
		}
		set {
			this.economicOperatorRoleField = value;
		}
	}
}