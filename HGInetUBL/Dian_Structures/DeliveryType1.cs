using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName="DeliveryType", Namespace="http://www.dian.gov.co/contratos/facturaelectronica/v1")]
	[System.Xml.Serialization.XmlRootAttribute("Delivery", Namespace="http://www.dian.gov.co/contratos/facturaelectronica/v1", IsNullable=false)]
	public partial class DeliveryType1 {
        
		private IDType idField;
        
		private QuantityType1 quantityField;
        
		private MinimumQuantityType minimumQuantityField;
        
		private MaximumQuantityType maximumQuantityField;
        
		private ActualDeliveryDateType actualDeliveryDateField;
        
		private ActualDeliveryTimeType actualDeliveryTimeField;
        
		private LatestDeliveryDateType latestDeliveryDateField;
        
		private LatestDeliveryTimeType latestDeliveryTimeField;
        
		private TrackingIDType trackingIDField;
        
		private AddressType1 deliveryAddressField;
        
		private LocationType2 deliveryLocationField;
        
		private PeriodType requestedDeliveryPeriodField;
        
		private PeriodType promisedDeliveryPeriodField;
        
		private PeriodType estimatedDeliveryPeriodField;
        
		private PartyType1 deliveryPartyField;
        
		private DespatchType1 despatchField;
        
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
		public QuantityType1 Quantity {
			get {
				return this.quantityField;
			}
			set {
				this.quantityField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public MinimumQuantityType MinimumQuantity {
			get {
				return this.minimumQuantityField;
			}
			set {
				this.minimumQuantityField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public MaximumQuantityType MaximumQuantity {
			get {
				return this.maximumQuantityField;
			}
			set {
				this.maximumQuantityField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public ActualDeliveryDateType ActualDeliveryDate {
			get {
				return this.actualDeliveryDateField;
			}
			set {
				this.actualDeliveryDateField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public ActualDeliveryTimeType ActualDeliveryTime {
			get {
				return this.actualDeliveryTimeField;
			}
			set {
				this.actualDeliveryTimeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public LatestDeliveryDateType LatestDeliveryDate {
			get {
				return this.latestDeliveryDateField;
			}
			set {
				this.latestDeliveryDateField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public LatestDeliveryTimeType LatestDeliveryTime {
			get {
				return this.latestDeliveryTimeField;
			}
			set {
				this.latestDeliveryTimeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public TrackingIDType TrackingID {
			get {
				return this.trackingIDField;
			}
			set {
				this.trackingIDField = value;
			}
		}
        
		/// <comentarios/>
		public AddressType1 DeliveryAddress {
			get {
				return this.deliveryAddressField;
			}
			set {
				this.deliveryAddressField = value;
			}
		}
        
		/// <comentarios/>
		public LocationType2 DeliveryLocation {
			get {
				return this.deliveryLocationField;
			}
			set {
				this.deliveryLocationField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public PeriodType RequestedDeliveryPeriod {
			get {
				return this.requestedDeliveryPeriodField;
			}
			set {
				this.requestedDeliveryPeriodField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public PeriodType PromisedDeliveryPeriod {
			get {
				return this.promisedDeliveryPeriodField;
			}
			set {
				this.promisedDeliveryPeriodField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public PeriodType EstimatedDeliveryPeriod {
			get {
				return this.estimatedDeliveryPeriodField;
			}
			set {
				this.estimatedDeliveryPeriodField = value;
			}
		}
        
		/// <comentarios/>
		public PartyType1 DeliveryParty {
			get {
				return this.deliveryPartyField;
			}
			set {
				this.deliveryPartyField = value;
			}
		}
        
		/// <comentarios/>
		public DespatchType1 Despatch {
			get {
				return this.despatchField;
			}
			set {
				this.despatchField = value;
			}
		}
	}
}