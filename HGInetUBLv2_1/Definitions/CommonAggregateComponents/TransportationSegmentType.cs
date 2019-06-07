/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("TransportationSegment", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class TransportationSegmentType {
    
	private SequenceNumericType sequenceNumericField;
    
	private TransportExecutionPlanReferenceIDType transportExecutionPlanReferenceIDField;
    
	private TransportationServiceType transportationServiceField;
    
	private PartyType transportServiceProviderPartyField;
    
	private ConsignmentType referencedConsignmentField;
    
	private ShipmentStageType[] shipmentStageField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public SequenceNumericType SequenceNumeric {
		get {
			return this.sequenceNumericField;
		}
		set {
			this.sequenceNumericField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public TransportExecutionPlanReferenceIDType TransportExecutionPlanReferenceID {
		get {
			return this.transportExecutionPlanReferenceIDField;
		}
		set {
			this.transportExecutionPlanReferenceIDField = value;
		}
	}
    
	/// <comentarios/>
	public TransportationServiceType TransportationService {
		get {
			return this.transportationServiceField;
		}
		set {
			this.transportationServiceField = value;
		}
	}
    
	/// <comentarios/>
	public PartyType TransportServiceProviderParty {
		get {
			return this.transportServiceProviderPartyField;
		}
		set {
			this.transportServiceProviderPartyField = value;
		}
	}
    
	/// <comentarios/>
	public ConsignmentType ReferencedConsignment {
		get {
			return this.referencedConsignmentField;
		}
		set {
			this.referencedConsignmentField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("ShipmentStage")]
	public ShipmentStageType[] ShipmentStage {
		get {
			return this.shipmentStageField;
		}
		set {
			this.shipmentStageField = value;
		}
	}
}