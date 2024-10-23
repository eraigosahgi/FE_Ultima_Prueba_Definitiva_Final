/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("ActivityDataLine", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class ActivityDataLineType {
    
	private IDType idField;
    
	private SupplyChainActivityTypeCodeType supplyChainActivityTypeCodeField;
    
	private CustomerPartyType buyerCustomerPartyField;
    
	private SupplierPartyType sellerSupplierPartyField;
    
	private PeriodType activityPeriodField;
    
	private LocationType1 activityOriginLocationField;
    
	private LocationType1 activityFinalLocationField;
    
	private SalesItemType[] salesItemField;
    
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
	public SupplyChainActivityTypeCodeType SupplyChainActivityTypeCode {
		get {
			return this.supplyChainActivityTypeCodeField;
		}
		set {
			this.supplyChainActivityTypeCodeField = value;
		}
	}
    
	/// <comentarios/>
	public CustomerPartyType BuyerCustomerParty {
		get {
			return this.buyerCustomerPartyField;
		}
		set {
			this.buyerCustomerPartyField = value;
		}
	}
    
	/// <comentarios/>
	public SupplierPartyType SellerSupplierParty {
		get {
			return this.sellerSupplierPartyField;
		}
		set {
			this.sellerSupplierPartyField = value;
		}
	}
    
	/// <comentarios/>
	public PeriodType ActivityPeriod {
		get {
			return this.activityPeriodField;
		}
		set {
			this.activityPeriodField = value;
		}
	}
    
	/// <comentarios/>
	public LocationType1 ActivityOriginLocation {
		get {
			return this.activityOriginLocationField;
		}
		set {
			this.activityOriginLocationField = value;
		}
	}
    
	/// <comentarios/>
	public LocationType1 ActivityFinalLocation {
		get {
			return this.activityFinalLocationField;
		}
		set {
			this.activityFinalLocationField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("SalesItem")]
	public SalesItemType[] SalesItem {
		get {
			return this.salesItemField;
		}
		set {
			this.salesItemField = value;
		}
	}
}