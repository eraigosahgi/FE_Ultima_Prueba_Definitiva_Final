﻿/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("AwardingTerms", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class AwardingTermsType {
    
	private WeightingAlgorithmCodeType weightingAlgorithmCodeField;
    
	private DescriptionType[] descriptionField;
    
	private TechnicalCommitteeDescriptionType[] technicalCommitteeDescriptionField;
    
	private LowTendersDescriptionType[] lowTendersDescriptionField;
    
	private PrizeIndicatorType prizeIndicatorField;
    
	private PrizeDescriptionType[] prizeDescriptionField;
    
	private PaymentDescriptionType[] paymentDescriptionField;
    
	private FollowupContractIndicatorType followupContractIndicatorField;
    
	private BindingOnBuyerIndicatorType bindingOnBuyerIndicatorField;
    
	private AwardingCriterionType[] awardingCriterionField;
    
	private PersonType[] technicalCommitteePersonField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public WeightingAlgorithmCodeType WeightingAlgorithmCode {
		get {
			return this.weightingAlgorithmCodeField;
		}
		set {
			this.weightingAlgorithmCodeField = value;
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
	[System.Xml.Serialization.XmlElementAttribute("TechnicalCommitteeDescription", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public TechnicalCommitteeDescriptionType[] TechnicalCommitteeDescription {
		get {
			return this.technicalCommitteeDescriptionField;
		}
		set {
			this.technicalCommitteeDescriptionField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("LowTendersDescription", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public LowTendersDescriptionType[] LowTendersDescription {
		get {
			return this.lowTendersDescriptionField;
		}
		set {
			this.lowTendersDescriptionField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public PrizeIndicatorType PrizeIndicator {
		get {
			return this.prizeIndicatorField;
		}
		set {
			this.prizeIndicatorField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("PrizeDescription", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public PrizeDescriptionType[] PrizeDescription {
		get {
			return this.prizeDescriptionField;
		}
		set {
			this.prizeDescriptionField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("PaymentDescription", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public PaymentDescriptionType[] PaymentDescription {
		get {
			return this.paymentDescriptionField;
		}
		set {
			this.paymentDescriptionField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public FollowupContractIndicatorType FollowupContractIndicator {
		get {
			return this.followupContractIndicatorField;
		}
		set {
			this.followupContractIndicatorField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public BindingOnBuyerIndicatorType BindingOnBuyerIndicator {
		get {
			return this.bindingOnBuyerIndicatorField;
		}
		set {
			this.bindingOnBuyerIndicatorField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("AwardingCriterion")]
	public AwardingCriterionType[] AwardingCriterion {
		get {
			return this.awardingCriterionField;
		}
		set {
			this.awardingCriterionField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("TechnicalCommitteePerson")]
	public PersonType[] TechnicalCommitteePerson {
		get {
			return this.technicalCommitteePersonField;
		}
		set {
			this.technicalCommitteePersonField = value;
		}
	}
}