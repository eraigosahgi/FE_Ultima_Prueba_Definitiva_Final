/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("ConsumptionPoint", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class ConsumptionPointType {
    
	private IDType idField;
    
	private DescriptionType[] descriptionField;
    
	private SubscriberIDType subscriberIDField;
    
	private SubscriberTypeType subscriberTypeField;
    
	private SubscriberTypeCodeType subscriberTypeCodeField;
    
	private TotalDeliveredQuantityType totalDeliveredQuantityField;
    
	private AddressType addressField;
    
	private WebSiteAccessType webSiteAccessField;
    
	private MeterType[] utilityMeterField;
    
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
	public SubscriberIDType SubscriberID {
		get {
			return this.subscriberIDField;
		}
		set {
			this.subscriberIDField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public SubscriberTypeType SubscriberType {
		get {
			return this.subscriberTypeField;
		}
		set {
			this.subscriberTypeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public SubscriberTypeCodeType SubscriberTypeCode {
		get {
			return this.subscriberTypeCodeField;
		}
		set {
			this.subscriberTypeCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public TotalDeliveredQuantityType TotalDeliveredQuantity {
		get {
			return this.totalDeliveredQuantityField;
		}
		set {
			this.totalDeliveredQuantityField = value;
		}
	}
    
	/// <comentarios/>
	public AddressType Address {
		get {
			return this.addressField;
		}
		set {
			this.addressField = value;
		}
	}
    
	/// <comentarios/>
	public WebSiteAccessType WebSiteAccess {
		get {
			return this.webSiteAccessField;
		}
		set {
			this.webSiteAccessField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("UtilityMeter")]
	public MeterType[] UtilityMeter {
		get {
			return this.utilityMeterField;
		}
		set {
			this.utilityMeterField = value;
		}
	}
}