/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("ApplicableTransportMeans", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class TransportMeansType {
    
	private JourneyIDType journeyIDField;
    
	private RegistrationNationalityIDType registrationNationalityIDField;
    
	private RegistrationNationalityType[] registrationNationalityField;
    
	private DirectionCodeType directionCodeField;
    
	private TransportMeansTypeCodeType transportMeansTypeCodeField;
    
	private TradeServiceCodeType tradeServiceCodeField;
    
	private StowageType stowageField;
    
	private AirTransportType airTransportField;
    
	private RoadTransportType roadTransportField;
    
	private RailTransportType railTransportField;
    
	private MaritimeTransportType maritimeTransportField;
    
	private PartyType ownerPartyField;
    
	private DimensionType[] measurementDimensionField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public JourneyIDType JourneyID {
		get {
			return this.journeyIDField;
		}
		set {
			this.journeyIDField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public RegistrationNationalityIDType RegistrationNationalityID {
		get {
			return this.registrationNationalityIDField;
		}
		set {
			this.registrationNationalityIDField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("RegistrationNationality", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public RegistrationNationalityType[] RegistrationNationality {
		get {
			return this.registrationNationalityField;
		}
		set {
			this.registrationNationalityField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public DirectionCodeType DirectionCode {
		get {
			return this.directionCodeField;
		}
		set {
			this.directionCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public TransportMeansTypeCodeType TransportMeansTypeCode {
		get {
			return this.transportMeansTypeCodeField;
		}
		set {
			this.transportMeansTypeCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public TradeServiceCodeType TradeServiceCode {
		get {
			return this.tradeServiceCodeField;
		}
		set {
			this.tradeServiceCodeField = value;
		}
	}
    
	/// <comentarios/>
	public StowageType Stowage {
		get {
			return this.stowageField;
		}
		set {
			this.stowageField = value;
		}
	}
    
	/// <comentarios/>
	public AirTransportType AirTransport {
		get {
			return this.airTransportField;
		}
		set {
			this.airTransportField = value;
		}
	}
    
	/// <comentarios/>
	public RoadTransportType RoadTransport {
		get {
			return this.roadTransportField;
		}
		set {
			this.roadTransportField = value;
		}
	}
    
	/// <comentarios/>
	public RailTransportType RailTransport {
		get {
			return this.railTransportField;
		}
		set {
			this.railTransportField = value;
		}
	}
    
	/// <comentarios/>
	public MaritimeTransportType MaritimeTransport {
		get {
			return this.maritimeTransportField;
		}
		set {
			this.maritimeTransportField = value;
		}
	}
    
	/// <comentarios/>
	public PartyType OwnerParty {
		get {
			return this.ownerPartyField;
		}
		set {
			this.ownerPartyField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("MeasurementDimension")]
	public DimensionType[] MeasurementDimension {
		get {
			return this.measurementDimensionField;
		}
		set {
			this.measurementDimensionField = value;
		}
	}
}