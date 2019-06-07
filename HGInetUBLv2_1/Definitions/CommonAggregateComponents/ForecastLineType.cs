/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("ForecastLine", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class ForecastLineType {
    
	private IDType idField;
    
	private NoteType[] noteField;
    
	private FrozenDocumentIndicatorType frozenDocumentIndicatorField;
    
	private ForecastTypeCodeType forecastTypeCodeField;
    
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
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public FrozenDocumentIndicatorType FrozenDocumentIndicator {
		get {
			return this.frozenDocumentIndicatorField;
		}
		set {
			this.frozenDocumentIndicatorField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ForecastTypeCodeType ForecastTypeCode {
		get {
			return this.forecastTypeCodeField;
		}
		set {
			this.forecastTypeCodeField = value;
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