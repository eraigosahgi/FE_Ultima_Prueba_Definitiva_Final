﻿/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("AwardingCriterion", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class AwardingCriterionType {
    
	private IDType idField;
    
	private AwardingCriterionTypeCodeType awardingCriterionTypeCodeField;
    
	private DescriptionType[] descriptionField;
    
	private WeightNumericType weightNumericField;
    
	private WeightType[] weightField;
    
	private CalculationExpressionType[] calculationExpressionField;
    
	private CalculationExpressionCodeType calculationExpressionCodeField;
    
	private MinimumQuantityType minimumQuantityField;
    
	private MaximumQuantityType maximumQuantityField;
    
	private MinimumAmountType minimumAmountField;
    
	private MaximumAmountType maximumAmountField;
    
	private MinimumImprovementBidType[] minimumImprovementBidField;
    
	private AwardingCriterionType[] subordinateAwardingCriterionField;
    
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
	public AwardingCriterionTypeCodeType AwardingCriterionTypeCode {
		get {
			return this.awardingCriterionTypeCodeField;
		}
		set {
			this.awardingCriterionTypeCodeField = value;
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
	public WeightNumericType WeightNumeric {
		get {
			return this.weightNumericField;
		}
		set {
			this.weightNumericField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("Weight", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public WeightType[] Weight {
		get {
			return this.weightField;
		}
		set {
			this.weightField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("CalculationExpression", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public CalculationExpressionType[] CalculationExpression {
		get {
			return this.calculationExpressionField;
		}
		set {
			this.calculationExpressionField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public CalculationExpressionCodeType CalculationExpressionCode {
		get {
			return this.calculationExpressionCodeField;
		}
		set {
			this.calculationExpressionCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public MinimumQuantityType MinimumQuantity {
		get {
			return this.minimumQuantityField;
		}
		set {
			this.minimumQuantityField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public MaximumQuantityType MaximumQuantity {
		get {
			return this.maximumQuantityField;
		}
		set {
			this.maximumQuantityField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public MinimumAmountType MinimumAmount {
		get {
			return this.minimumAmountField;
		}
		set {
			this.minimumAmountField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public MaximumAmountType MaximumAmount {
		get {
			return this.maximumAmountField;
		}
		set {
			this.maximumAmountField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("MinimumImprovementBid", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public MinimumImprovementBidType[] MinimumImprovementBid {
		get {
			return this.minimumImprovementBidField;
		}
		set {
			this.minimumImprovementBidField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("SubordinateAwardingCriterion")]
	public AwardingCriterionType[] SubordinateAwardingCriterion {
		get {
			return this.subordinateAwardingCriterionField;
		}
		set {
			this.subordinateAwardingCriterionField = value;
		}
	}
}