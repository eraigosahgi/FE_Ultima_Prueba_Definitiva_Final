﻿/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("EvaluationCriterion", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class EvaluationCriterionType {
    
	private EvaluationCriterionTypeCodeType evaluationCriterionTypeCodeField;
    
	private DescriptionType[] descriptionField;
    
	private ThresholdAmountType thresholdAmountField;
    
	private ThresholdQuantityType thresholdQuantityField;
    
	private ExpressionCodeType expressionCodeField;
    
	private ExpressionType[] expressionField;
    
	private PeriodType durationPeriodField;
    
	private EvidenceType[] suggestedEvidenceField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public EvaluationCriterionTypeCodeType EvaluationCriterionTypeCode {
		get {
			return this.evaluationCriterionTypeCodeField;
		}
		set {
			this.evaluationCriterionTypeCodeField = value;
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
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ThresholdAmountType ThresholdAmount {
		get {
			return this.thresholdAmountField;
		}
		set {
			this.thresholdAmountField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ThresholdQuantityType ThresholdQuantity {
		get {
			return this.thresholdQuantityField;
		}
		set {
			this.thresholdQuantityField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ExpressionCodeType ExpressionCode {
		get {
			return this.expressionCodeField;
		}
		set {
			this.expressionCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("Expression", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ExpressionType[] Expression {
		get {
			return this.expressionField;
		}
		set {
			this.expressionField = value;
		}
	}
    
	/// <comentarios/>
	public PeriodType DurationPeriod {
		get {
			return this.durationPeriodField;
		}
		set {
			this.durationPeriodField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("SuggestedEvidence")]
	public EvidenceType[] SuggestedEvidence {
		get {
			return this.suggestedEvidenceField;
		}
		set {
			this.suggestedEvidenceField = value;
		}
	}
}