/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("ForecastRevisionLine", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class ForecastRevisionLineType {
    
	private IDType idField;
    
	private NoteType[] noteField;
    
	private DescriptionType[] descriptionField;
    
	private RevisedForecastLineIDType revisedForecastLineIDField;
    
	private SourceForecastIssueDateType sourceForecastIssueDateField;
    
	private SourceForecastIssueTimeType sourceForecastIssueTimeField;
    
	private AdjustmentReasonCodeType adjustmentReasonCodeField;
    
	private PeriodType forecastPeriodField;
    
	private SalesItemType salesItemField;
    
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
	public RevisedForecastLineIDType RevisedForecastLineID {
		get {
			return this.revisedForecastLineIDField;
		}
		set {
			this.revisedForecastLineIDField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public SourceForecastIssueDateType SourceForecastIssueDate {
		get {
			return this.sourceForecastIssueDateField;
		}
		set {
			this.sourceForecastIssueDateField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public SourceForecastIssueTimeType SourceForecastIssueTime {
		get {
			return this.sourceForecastIssueTimeField;
		}
		set {
			this.sourceForecastIssueTimeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public AdjustmentReasonCodeType AdjustmentReasonCode {
		get {
			return this.adjustmentReasonCodeField;
		}
		set {
			this.adjustmentReasonCodeField = value;
		}
	}
    
	/// <comentarios/>
	public PeriodType ForecastPeriod {
		get {
			return this.forecastPeriodField;
		}
		set {
			this.forecastPeriodField = value;
		}
	}
    
	/// <comentarios/>
	public SalesItemType SalesItem {
		get {
			return this.salesItemField;
		}
		set {
			this.salesItemField = value;
		}
	}
}