using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("ReceiptLine", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
	public partial class ReceiptLineType {
        
		private IDType idField;
        
		private UUIDType uUIDField;
        
		private NoteType noteField;
        
		private ReceivedQuantityType receivedQuantityField;
        
		private ShortQuantityType shortQuantityField;
        
		private ShortageActionCodeType shortageActionCodeField;
        
		private RejectedQuantityType rejectedQuantityField;
        
		private RejectReasonCodeType rejectReasonCodeField;
        
		private RejectReasonType rejectReasonField;
        
		private RejectActionCodeType rejectActionCodeField;
        
		private OversupplyQuantityType oversupplyQuantityField;
        
		private ReceivedDateType receivedDateField;
        
		private TimingComplaintCodeType timingComplaintCodeField;
        
		private TimingComplaintType timingComplaintField;
        
		private OrderLineReferenceType orderLineReferenceField;
        
		private LineReferenceType[] despatchLineReferenceField;
        
		private DocumentReferenceType[] documentReferenceField;
        
		private ItemType[] itemField;
        
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
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public NoteType Note {
			get {
				return this.noteField;
			}
			set {
				this.noteField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public ReceivedQuantityType ReceivedQuantity {
			get {
				return this.receivedQuantityField;
			}
			set {
				this.receivedQuantityField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public ShortQuantityType ShortQuantity {
			get {
				return this.shortQuantityField;
			}
			set {
				this.shortQuantityField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public ShortageActionCodeType ShortageActionCode {
			get {
				return this.shortageActionCodeField;
			}
			set {
				this.shortageActionCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public RejectedQuantityType RejectedQuantity {
			get {
				return this.rejectedQuantityField;
			}
			set {
				this.rejectedQuantityField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public RejectReasonCodeType RejectReasonCode {
			get {
				return this.rejectReasonCodeField;
			}
			set {
				this.rejectReasonCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public RejectReasonType RejectReason {
			get {
				return this.rejectReasonField;
			}
			set {
				this.rejectReasonField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public RejectActionCodeType RejectActionCode {
			get {
				return this.rejectActionCodeField;
			}
			set {
				this.rejectActionCodeField = value;
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
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public ReceivedDateType ReceivedDate {
			get {
				return this.receivedDateField;
			}
			set {
				this.receivedDateField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public TimingComplaintCodeType TimingComplaintCode {
			get {
				return this.timingComplaintCodeField;
			}
			set {
				this.timingComplaintCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public TimingComplaintType TimingComplaint {
			get {
				return this.timingComplaintField;
			}
			set {
				this.timingComplaintField = value;
			}
		}
        
		/// <comentarios/>
		public OrderLineReferenceType OrderLineReference {
			get {
				return this.orderLineReferenceField;
			}
			set {
				this.orderLineReferenceField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("DespatchLineReference")]
		public LineReferenceType[] DespatchLineReference {
			get {
				return this.despatchLineReferenceField;
			}
			set {
				this.despatchLineReferenceField = value;
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
		[System.Xml.Serialization.XmlElementAttribute("Item")]
		public ItemType[] Item {
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
}