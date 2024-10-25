/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("MaritimeTransport", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class MaritimeTransportType {
    
	private VesselIDType vesselIDField;
    
	private VesselNameType vesselNameField;
    
	private RadioCallSignIDType radioCallSignIDField;
    
	private ShipsRequirementsType[] shipsRequirementsField;
    
	private GrossTonnageMeasureType grossTonnageMeasureField;
    
	private NetTonnageMeasureType netTonnageMeasureField;
    
	private DocumentReferenceType registryCertificateDocumentReferenceField;
    
	private LocationType1 registryPortLocationField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public VesselIDType VesselID {
		get {
			return this.vesselIDField;
		}
		set {
			this.vesselIDField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public VesselNameType VesselName {
		get {
			return this.vesselNameField;
		}
		set {
			this.vesselNameField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public RadioCallSignIDType RadioCallSignID {
		get {
			return this.radioCallSignIDField;
		}
		set {
			this.radioCallSignIDField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("ShipsRequirements", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ShipsRequirementsType[] ShipsRequirements {
		get {
			return this.shipsRequirementsField;
		}
		set {
			this.shipsRequirementsField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public GrossTonnageMeasureType GrossTonnageMeasure {
		get {
			return this.grossTonnageMeasureField;
		}
		set {
			this.grossTonnageMeasureField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public NetTonnageMeasureType NetTonnageMeasure {
		get {
			return this.netTonnageMeasureField;
		}
		set {
			this.netTonnageMeasureField = value;
		}
	}
    
	/// <comentarios/>
	public DocumentReferenceType RegistryCertificateDocumentReference {
		get {
			return this.registryCertificateDocumentReferenceField;
		}
		set {
			this.registryCertificateDocumentReferenceField = value;
		}
	}
    
	/// <comentarios/>
	public LocationType1 RegistryPortLocation {
		get {
			return this.registryPortLocationField;
		}
		set {
			this.registryPortLocationField = value;
		}
	}
}