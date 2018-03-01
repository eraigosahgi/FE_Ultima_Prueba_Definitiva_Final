using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("TransportHandlingUnit", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
	public partial class TransportHandlingUnitType {
        
		private IDType idField;
        
		private TransportHandlingUnitTypeCodeType transportHandlingUnitTypeCodeField;
        
		private HandlingCodeType handlingCodeField;
        
		private HandlingInstructionsType handlingInstructionsField;
        
		private HazardousRiskIndicatorType hazardousRiskIndicatorField;
        
		private TotalGoodsItemQuantityType totalGoodsItemQuantityField;
        
		private TotalPackageQuantityType totalPackageQuantityField;
        
		private DamageRemarksType[] damageRemarksField;
        
		private ShippingMarksType[] shippingMarksField;
        
		private DespatchLineType[] handlingUnitDespatchLineField;
        
		private PackageType[] actualPackageField;
        
		private ReceiptLineType[] receivedHandlingUnitReceiptLineField;
        
		private TransportEquipmentType[] transportEquipmentField;
        
		private HazardousGoodsTransitType[] hazardousGoodsTransitField;
        
		private DimensionType[] measurementDimensionField;
        
		private TemperatureType minimumTemperatureField;
        
		private TemperatureType maximumTemperatureField;
        
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
		public TransportHandlingUnitTypeCodeType TransportHandlingUnitTypeCode {
			get {
				return this.transportHandlingUnitTypeCodeField;
			}
			set {
				this.transportHandlingUnitTypeCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public HandlingCodeType HandlingCode {
			get {
				return this.handlingCodeField;
			}
			set {
				this.handlingCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public HandlingInstructionsType HandlingInstructions {
			get {
				return this.handlingInstructionsField;
			}
			set {
				this.handlingInstructionsField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public HazardousRiskIndicatorType HazardousRiskIndicator {
			get {
				return this.hazardousRiskIndicatorField;
			}
			set {
				this.hazardousRiskIndicatorField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public TotalGoodsItemQuantityType TotalGoodsItemQuantity {
			get {
				return this.totalGoodsItemQuantityField;
			}
			set {
				this.totalGoodsItemQuantityField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public TotalPackageQuantityType TotalPackageQuantity {
			get {
				return this.totalPackageQuantityField;
			}
			set {
				this.totalPackageQuantityField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("DamageRemarks", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public DamageRemarksType[] DamageRemarks {
			get {
				return this.damageRemarksField;
			}
			set {
				this.damageRemarksField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("ShippingMarks", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public ShippingMarksType[] ShippingMarks {
			get {
				return this.shippingMarksField;
			}
			set {
				this.shippingMarksField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("HandlingUnitDespatchLine")]
		public DespatchLineType[] HandlingUnitDespatchLine {
			get {
				return this.handlingUnitDespatchLineField;
			}
			set {
				this.handlingUnitDespatchLineField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("ActualPackage")]
		public PackageType[] ActualPackage {
			get {
				return this.actualPackageField;
			}
			set {
				this.actualPackageField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("ReceivedHandlingUnitReceiptLine")]
		public ReceiptLineType[] ReceivedHandlingUnitReceiptLine {
			get {
				return this.receivedHandlingUnitReceiptLineField;
			}
			set {
				this.receivedHandlingUnitReceiptLineField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("TransportEquipment")]
		public TransportEquipmentType[] TransportEquipment {
			get {
				return this.transportEquipmentField;
			}
			set {
				this.transportEquipmentField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("HazardousGoodsTransit")]
		public HazardousGoodsTransitType[] HazardousGoodsTransit {
			get {
				return this.hazardousGoodsTransitField;
			}
			set {
				this.hazardousGoodsTransitField = value;
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
        
		/// <comentarios/>
		public TemperatureType MinimumTemperature {
			get {
				return this.minimumTemperatureField;
			}
			set {
				this.minimumTemperatureField = value;
			}
		}
        
		/// <comentarios/>
		public TemperatureType MaximumTemperature {
			get {
				return this.maximumTemperatureField;
			}
			set {
				this.maximumTemperatureField = value;
			}
		}
	}
}