/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("ForecastExceptionCriterionLine", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class ForecastExceptionCriterionLineType {
    
	private ForecastPurposeCodeType forecastPurposeCodeField;
    
	private ForecastTypeCodeType forecastTypeCodeField;
    
	private ComparisonDataSourceCodeType comparisonDataSourceCodeField;
    
	private DataSourceCodeType dataSourceCodeField;
    
	private TimeDeltaDaysQuantityType timeDeltaDaysQuantityField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ForecastPurposeCodeType ForecastPurposeCode {
		get {
			return this.forecastPurposeCodeField;
		}
		set {
			this.forecastPurposeCodeField = value;
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
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ComparisonDataSourceCodeType ComparisonDataSourceCode {
		get {
			return this.comparisonDataSourceCodeField;
		}
		set {
			this.comparisonDataSourceCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public DataSourceCodeType DataSourceCode {
		get {
			return this.dataSourceCodeField;
		}
		set {
			this.dataSourceCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public TimeDeltaDaysQuantityType TimeDeltaDaysQuantity {
		get {
			return this.timeDeltaDaysQuantityField;
		}
		set {
			this.timeDeltaDaysQuantityField = value;
		}
	}
}