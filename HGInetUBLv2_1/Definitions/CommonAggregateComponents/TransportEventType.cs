/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("AcceptanceTransportEvent", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class TransportEventType {
    
	private IdentificationIDType identificationIDField;
    
	private OccurrenceDateType occurrenceDateField;
    
	private OccurrenceTimeType occurrenceTimeField;
    
	private TransportEventTypeCodeType transportEventTypeCodeField;
    
	private DescriptionType[] descriptionField;
    
	private CompletionIndicatorType completionIndicatorField;
    
	private ShipmentType reportedShipmentField;
    
	private StatusType[] currentStatusField;
    
	private ContactType[] contactField;
    
	private LocationType1 locationField;
    
	private SignatureType signatureField;
    
	private PeriodType[] periodField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public IdentificationIDType IdentificationID {
		get {
			return this.identificationIDField;
		}
		set {
			this.identificationIDField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public OccurrenceDateType OccurrenceDate {
		get {
			return this.occurrenceDateField;
		}
		set {
			this.occurrenceDateField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public OccurrenceTimeType OccurrenceTime {
		get {
			return this.occurrenceTimeField;
		}
		set {
			this.occurrenceTimeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public TransportEventTypeCodeType TransportEventTypeCode {
		get {
			return this.transportEventTypeCodeField;
		}
		set {
			this.transportEventTypeCodeField = value;
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
	public CompletionIndicatorType CompletionIndicator {
		get {
			return this.completionIndicatorField;
		}
		set {
			this.completionIndicatorField = value;
		}
	}
    
	/// <comentarios/>
	public ShipmentType ReportedShipment {
		get {
			return this.reportedShipmentField;
		}
		set {
			this.reportedShipmentField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("CurrentStatus")]
	public StatusType[] CurrentStatus {
		get {
			return this.currentStatusField;
		}
		set {
			this.currentStatusField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("Contact")]
	public ContactType[] Contact {
		get {
			return this.contactField;
		}
		set {
			this.contactField = value;
		}
	}
    
	/// <comentarios/>
	public LocationType1 Location {
		get {
			return this.locationField;
		}
		set {
			this.locationField = value;
		}
	}
    
	/// <comentarios/>
	public SignatureType Signature {
		get {
			return this.signatureField;
		}
		set {
			this.signatureField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("Period")]
	public PeriodType[] Period {
		get {
			return this.periodField;
		}
		set {
			this.periodField = value;
		}
	}
}