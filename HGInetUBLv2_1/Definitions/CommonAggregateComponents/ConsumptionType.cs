/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("Consumption", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class ConsumptionType {
    
	private UtilityStatementTypeCodeType utilityStatementTypeCodeField;
    
	private PeriodType mainPeriodField;
    
	private AllowanceChargeType[] allowanceChargeField;
    
	private TaxTotalType[] taxTotalField;
    
	private EnergyWaterSupplyType energyWaterSupplyField;
    
	private TelecommunicationsSupplyType telecommunicationsSupplyField;
    
	private MonetaryTotalType legalMonetaryTotalField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public UtilityStatementTypeCodeType UtilityStatementTypeCode {
		get {
			return this.utilityStatementTypeCodeField;
		}
		set {
			this.utilityStatementTypeCodeField = value;
		}
	}
    
	/// <comentarios/>
	public PeriodType MainPeriod {
		get {
			return this.mainPeriodField;
		}
		set {
			this.mainPeriodField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("AllowanceCharge")]
	public AllowanceChargeType[] AllowanceCharge {
		get {
			return this.allowanceChargeField;
		}
		set {
			this.allowanceChargeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("TaxTotal")]
	public TaxTotalType[] TaxTotal {
		get {
			return this.taxTotalField;
		}
		set {
			this.taxTotalField = value;
		}
	}
    
	/// <comentarios/>
	public EnergyWaterSupplyType EnergyWaterSupply {
		get {
			return this.energyWaterSupplyField;
		}
		set {
			this.energyWaterSupplyField = value;
		}
	}
    
	/// <comentarios/>
	public TelecommunicationsSupplyType TelecommunicationsSupply {
		get {
			return this.telecommunicationsSupplyField;
		}
		set {
			this.telecommunicationsSupplyField = value;
		}
	}
    
	/// <comentarios/>
	public MonetaryTotalType LegalMonetaryTotal {
		get {
			return this.legalMonetaryTotalField;
		}
		set {
			this.legalMonetaryTotalField = value;
		}
	}
}