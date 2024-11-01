﻿/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("ExceptionNotificationLine", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class ExceptionNotificationLineType {
    
	private IDType idField;
    
	private NoteType[] noteField;
    
	private DescriptionType[] descriptionField;
    
	private ExceptionStatusCodeType exceptionStatusCodeField;
    
	private CollaborationPriorityCodeType collaborationPriorityCodeField;
    
	private ResolutionCodeType resolutionCodeField;
    
	private ComparedValueMeasureType comparedValueMeasureField;
    
	private SourceValueMeasureType sourceValueMeasureField;
    
	private VarianceQuantityType varianceQuantityField;
    
	private SupplyChainActivityTypeCodeType supplyChainActivityTypeCodeField;
    
	private PerformanceMetricTypeCodeType performanceMetricTypeCodeField;
    
	private PeriodType exceptionObservationPeriodField;
    
	private DocumentReferenceType[] documentReferenceField;
    
	private ForecastExceptionType forecastExceptionField;
    
	private ItemType supplyItemField;
    
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
	public ExceptionStatusCodeType ExceptionStatusCode {
		get {
			return this.exceptionStatusCodeField;
		}
		set {
			this.exceptionStatusCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public CollaborationPriorityCodeType CollaborationPriorityCode {
		get {
			return this.collaborationPriorityCodeField;
		}
		set {
			this.collaborationPriorityCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ResolutionCodeType ResolutionCode {
		get {
			return this.resolutionCodeField;
		}
		set {
			this.resolutionCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ComparedValueMeasureType ComparedValueMeasure {
		get {
			return this.comparedValueMeasureField;
		}
		set {
			this.comparedValueMeasureField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public SourceValueMeasureType SourceValueMeasure {
		get {
			return this.sourceValueMeasureField;
		}
		set {
			this.sourceValueMeasureField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public VarianceQuantityType VarianceQuantity {
		get {
			return this.varianceQuantityField;
		}
		set {
			this.varianceQuantityField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public SupplyChainActivityTypeCodeType SupplyChainActivityTypeCode {
		get {
			return this.supplyChainActivityTypeCodeField;
		}
		set {
			this.supplyChainActivityTypeCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public PerformanceMetricTypeCodeType PerformanceMetricTypeCode {
		get {
			return this.performanceMetricTypeCodeField;
		}
		set {
			this.performanceMetricTypeCodeField = value;
		}
	}
    
	/// <comentarios/>
	public PeriodType ExceptionObservationPeriod {
		get {
			return this.exceptionObservationPeriodField;
		}
		set {
			this.exceptionObservationPeriodField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("DocumentReference")]
	public DocumentReferenceType[] DocumentReference {
		get {
			return this.documentReferenceField;
		}
		set {
			this.documentReferenceField = value;
		}
	}
    
	/// <comentarios/>
	public ForecastExceptionType ForecastException {
		get {
			return this.forecastExceptionField;
		}
		set {
			this.forecastExceptionField = value;
		}
	}
    
	/// <comentarios/>
	public ItemType SupplyItem {
		get {
			return this.supplyItemField;
		}
		set {
			this.supplyItemField = value;
		}
	}
}