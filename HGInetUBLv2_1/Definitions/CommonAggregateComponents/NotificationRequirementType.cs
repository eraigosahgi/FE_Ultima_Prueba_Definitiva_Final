/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("NotificationRequirement", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class NotificationRequirementType {
    
	private NotificationTypeCodeType notificationTypeCodeField;
    
	private PostEventNotificationDurationMeasureType postEventNotificationDurationMeasureField;
    
	private PreEventNotificationDurationMeasureType preEventNotificationDurationMeasureField;
    
	private PartyType[] notifyPartyField;
    
	private PeriodType[] notificationPeriodField;
    
	private LocationType1[] notificationLocationField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public NotificationTypeCodeType NotificationTypeCode {
		get {
			return this.notificationTypeCodeField;
		}
		set {
			this.notificationTypeCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public PostEventNotificationDurationMeasureType PostEventNotificationDurationMeasure {
		get {
			return this.postEventNotificationDurationMeasureField;
		}
		set {
			this.postEventNotificationDurationMeasureField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public PreEventNotificationDurationMeasureType PreEventNotificationDurationMeasure {
		get {
			return this.preEventNotificationDurationMeasureField;
		}
		set {
			this.preEventNotificationDurationMeasureField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("NotifyParty")]
	public PartyType[] NotifyParty {
		get {
			return this.notifyPartyField;
		}
		set {
			this.notifyPartyField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("NotificationPeriod")]
	public PeriodType[] NotificationPeriod {
		get {
			return this.notificationPeriodField;
		}
		set {
			this.notificationPeriodField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("NotificationLocation")]
	public LocationType1[] NotificationLocation {
		get {
			return this.notificationLocationField;
		}
		set {
			this.notificationLocationField = value;
		}
	}
}