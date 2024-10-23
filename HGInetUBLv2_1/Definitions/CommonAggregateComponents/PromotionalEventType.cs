/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("PromotionalEvent", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class PromotionalEventType {
    
	private PromotionalEventTypeCodeType promotionalEventTypeCodeField;
    
	private SubmissionDateType submissionDateField;
    
	private FirstShipmentAvailibilityDateType firstShipmentAvailibilityDateField;
    
	private LatestProposalAcceptanceDateType latestProposalAcceptanceDateField;
    
	private PromotionalSpecificationType[] promotionalSpecificationField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public PromotionalEventTypeCodeType PromotionalEventTypeCode {
		get {
			return this.promotionalEventTypeCodeField;
		}
		set {
			this.promotionalEventTypeCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public SubmissionDateType SubmissionDate {
		get {
			return this.submissionDateField;
		}
		set {
			this.submissionDateField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public FirstShipmentAvailibilityDateType FirstShipmentAvailibilityDate {
		get {
			return this.firstShipmentAvailibilityDateField;
		}
		set {
			this.firstShipmentAvailibilityDateField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public LatestProposalAcceptanceDateType LatestProposalAcceptanceDate {
		get {
			return this.latestProposalAcceptanceDateField;
		}
		set {
			this.latestProposalAcceptanceDateField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("PromotionalSpecification")]
	public PromotionalSpecificationType[] PromotionalSpecification {
		get {
			return this.promotionalSpecificationField;
		}
		set {
			this.promotionalSpecificationField = value;
		}
	}
}