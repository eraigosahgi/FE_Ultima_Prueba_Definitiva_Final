﻿/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("TendererQualificationRequest", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class TendererQualificationRequestType {
    
	private CompanyLegalFormCodeType companyLegalFormCodeField;
    
	private CompanyLegalFormType companyLegalFormField;
    
	private PersonalSituationType[] personalSituationField;
    
	private OperatingYearsQuantityType operatingYearsQuantityField;
    
	private EmployeeQuantityType employeeQuantityField;
    
	private DescriptionType[] descriptionField;
    
	private ClassificationSchemeType[] requiredBusinessClassificationSchemeField;
    
	private EvaluationCriterionType[] technicalEvaluationCriterionField;
    
	private EvaluationCriterionType[] financialEvaluationCriterionField;
    
	private TendererRequirementType[] specificTendererRequirementField;
    
	private EconomicOperatorRoleType[] economicOperatorRoleField;
    
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
	[System.Xml.Serialization.XmlElementAttribute("RequiredBusinessClassificationScheme")]
	public ClassificationSchemeType[] RequiredBusinessClassificationScheme {
		get {
			return this.requiredBusinessClassificationSchemeField;
		}
		set {
			this.requiredBusinessClassificationSchemeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("TechnicalEvaluationCriterion")]
	public EvaluationCriterionType[] TechnicalEvaluationCriterion {
		get {
			return this.technicalEvaluationCriterionField;
		}
		set {
			this.technicalEvaluationCriterionField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("FinancialEvaluationCriterion")]
	public EvaluationCriterionType[] FinancialEvaluationCriterion {
		get {
			return this.financialEvaluationCriterionField;
		}
		set {
			this.financialEvaluationCriterionField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("SpecificTendererRequirement")]
	public TendererRequirementType[] SpecificTendererRequirement {
		get {
			return this.specificTendererRequirementField;
		}
		set {
			this.specificTendererRequirementField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("EconomicOperatorRole")]
	public EconomicOperatorRoleType[] EconomicOperatorRole {
		get {
			return this.economicOperatorRoleField;
		}
		set {
			this.economicOperatorRoleField = value;
		}
	}
}