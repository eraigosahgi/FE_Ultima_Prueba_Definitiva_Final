/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("DespatchLine", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class DespatchLineType {
    
	private IDType idField;
    
	private UUIDType uUIDField;
    
	private NoteType[] noteField;
    
	private LineStatusCodeType lineStatusCodeField;
    
	private DeliveredQuantityType deliveredQuantityField;
    
	private BackorderQuantityType backorderQuantityField;
    
	private BackorderReasonType[] backorderReasonField;
    
	private OutstandingQuantityType outstandingQuantityField;
    
	private OutstandingReasonType[] outstandingReasonField;
    
	private OversupplyQuantityType oversupplyQuantityField;
    
	private OrderLineReferenceType[] orderLineReferenceField;
    
	private DocumentReferenceType[] documentReferenceField;
    
	private ItemType itemField;
    
	private ShipmentType[] shipmentField;
    
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
	public UUIDType UUID {
		get {
			return this.uUIDField;
		}
		set {
			this.uUIDField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("Note", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public NoteType[] Note {
		get {
			return this.noteField;
		}
		set {
			this.noteField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public LineStatusCodeType LineStatusCode {
		get {
			return this.lineStatusCodeField;
		}
		set {
			this.lineStatusCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public DeliveredQuantityType DeliveredQuantity {
		get {
			return this.deliveredQuantityField;
		}
		set {
			this.deliveredQuantityField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public BackorderQuantityType BackorderQuantity {
		get {
			return this.backorderQuantityField;
		}
		set {
			this.backorderQuantityField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("BackorderReason", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public BackorderReasonType[] BackorderReason {
		get {
			return this.backorderReasonField;
		}
		set {
			this.backorderReasonField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public OutstandingQuantityType OutstandingQuantity {
		get {
			return this.outstandingQuantityField;
		}
		set {
			this.outstandingQuantityField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("OutstandingReason", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public OutstandingReasonType[] OutstandingReason {
		get {
			return this.outstandingReasonField;
		}
		set {
			this.outstandingReasonField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public OversupplyQuantityType OversupplyQuantity {
		get {
			return this.oversupplyQuantityField;
		}
		set {
			this.oversupplyQuantityField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("OrderLineReference")]
	public OrderLineReferenceType[] OrderLineReference {
		get {
			return this.orderLineReferenceField;
		}
		set {
			this.orderLineReferenceField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("DocumentReference")]
	public DocumentReferenceType[] DocumentReference {
		get {
			return this.documentReferenceField;
		}
		set {
			this.documentReferenceField = value;
		}
	}
    
	/// <comentarios/>
	public ItemType Item {
		get {
			return this.itemField;
		}
		set {
			this.itemField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("Shipment")]
	public ShipmentType[] Shipment {
		get {
			return this.shipmentField;
		}
		set {
			this.shipmentField = value;
		}
	}
}