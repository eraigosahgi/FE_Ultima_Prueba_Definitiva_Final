/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("PreviousPriceList", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class PriceListType {
    
	private IDType idField;
    
	private StatusCodeType statusCodeField;
    
	private PeriodType[] validityPeriodField;
    
	private PriceListType previousPriceListField;
    
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
	public StatusCodeType StatusCode {
		get {
			return this.statusCodeField;
		}
		set {
			this.statusCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("ValidityPeriod")]
	public PeriodType[] ValidityPeriod {
		get {
			return this.validityPeriodField;
		}
		set {
			this.validityPeriodField = value;
		}
	}
    
	/// <comentarios/>
	public PriceListType PreviousPriceList {
		get {
			return this.previousPriceListField;
		}
		set {
			this.previousPriceListField = value;
		}
	}
}