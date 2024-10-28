/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("ConsumptionReport", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class ConsumptionReportType {
    
	private IDType idField;
    
	private ConsumptionTypeType consumptionTypeField;
    
	private ConsumptionTypeCodeType consumptionTypeCodeField;
    
	private DescriptionType[] descriptionField;
    
	private TotalConsumedQuantityType totalConsumedQuantityField;
    
	private BasicConsumedQuantityType basicConsumedQuantityField;
    
	private ResidentOccupantsNumericType residentOccupantsNumericField;
    
	private ConsumersEnergyLevelCodeType consumersEnergyLevelCodeField;
    
	private ConsumersEnergyLevelType consumersEnergyLevelField;
    
	private ResidenceTypeType residenceTypeField;
    
	private ResidenceTypeCodeType residenceTypeCodeField;
    
	private HeatingTypeType heatingTypeField;
    
	private HeatingTypeCodeType heatingTypeCodeField;
    
	private PeriodType periodField;
    
	private DocumentReferenceType guidanceDocumentReferenceField;
    
	private DocumentReferenceType documentReferenceField;
    
	private ConsumptionReportReferenceType[] consumptionReportReferenceField;
    
	private ConsumptionHistoryType[] consumptionHistoryField;
    
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
	public ConsumptionTypeType ConsumptionType {
		get {
			return this.consumptionTypeField;
		}
		set {
			this.consumptionTypeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ConsumptionTypeCodeType ConsumptionTypeCode {
		get {
			return this.consumptionTypeCodeField;
		}
		set {
			this.consumptionTypeCodeField = value;
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
	public TotalConsumedQuantityType TotalConsumedQuantity {
		get {
			return this.totalConsumedQuantityField;
		}
		set {
			this.totalConsumedQuantityField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public BasicConsumedQuantityType BasicConsumedQuantity {
		get {
			return this.basicConsumedQuantityField;
		}
		set {
			this.basicConsumedQuantityField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ResidentOccupantsNumericType ResidentOccupantsNumeric {
		get {
			return this.residentOccupantsNumericField;
		}
		set {
			this.residentOccupantsNumericField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ConsumersEnergyLevelCodeType ConsumersEnergyLevelCode {
		get {
			return this.consumersEnergyLevelCodeField;
		}
		set {
			this.consumersEnergyLevelCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ConsumersEnergyLevelType ConsumersEnergyLevel {
		get {
			return this.consumersEnergyLevelField;
		}
		set {
			this.consumersEnergyLevelField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ResidenceTypeType ResidenceType {
		get {
			return this.residenceTypeField;
		}
		set {
			this.residenceTypeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ResidenceTypeCodeType ResidenceTypeCode {
		get {
			return this.residenceTypeCodeField;
		}
		set {
			this.residenceTypeCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public HeatingTypeType HeatingType {
		get {
			return this.heatingTypeField;
		}
		set {
			this.heatingTypeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public HeatingTypeCodeType HeatingTypeCode {
		get {
			return this.heatingTypeCodeField;
		}
		set {
			this.heatingTypeCodeField = value;
		}
	}
    
	/// <comentarios/>
	public PeriodType Period {
		get {
			return this.periodField;
		}
		set {
			this.periodField = value;
		}
	}
    
	/// <comentarios/>
	public DocumentReferenceType GuidanceDocumentReference {
		get {
			return this.guidanceDocumentReferenceField;
		}
		set {
			this.guidanceDocumentReferenceField = value;
		}
	}
    
	/// <comentarios/>
	public DocumentReferenceType DocumentReference {
		get {
			return this.documentReferenceField;
		}
		set {
			this.documentReferenceField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("ConsumptionReportReference")]
	public ConsumptionReportReferenceType[] ConsumptionReportReference {
		get {
			return this.consumptionReportReferenceField;
		}
		set {
			this.consumptionReportReferenceField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("ConsumptionHistory")]
	public ConsumptionHistoryType[] ConsumptionHistory {
		get {
			return this.consumptionHistoryField;
		}
		set {
			this.consumptionHistoryField = value;
		}
	}
}