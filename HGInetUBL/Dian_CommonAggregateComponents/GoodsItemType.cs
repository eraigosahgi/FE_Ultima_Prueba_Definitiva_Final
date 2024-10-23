using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("ContainedGoodsItem", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
	public partial class GoodsItemType {
        
		private IDType idField;
        
		private SequenceNumberIDType sequenceNumberIDField;
        
		private DescriptionType[] descriptionField;
        
		private HazardousRiskIndicatorType hazardousRiskIndicatorField;
        
		private DeclaredCustomsValueAmountType declaredCustomsValueAmountField;
        
		private DeclaredForCarriageValueAmountType declaredForCarriageValueAmountField;
        
		private DeclaredStatisticsValueAmountType declaredStatisticsValueAmountField;
        
		private FreeOnBoardValueAmountType freeOnBoardValueAmountField;
        
		private InsuranceValueAmountType insuranceValueAmountField;
        
		private ValueAmountType valueAmountField;
        
		private GrossWeightMeasureType grossWeightMeasureField;
        
		private NetWeightMeasureType netWeightMeasureField;
        
		private NetNetWeightMeasureType netNetWeightMeasureField;
        
		private ChargeableWeightMeasureType chargeableWeightMeasureField;
        
		private GrossVolumeMeasureType grossVolumeMeasureField;
        
		private NetVolumeMeasureType netVolumeMeasureField;
        
		private QuantityType1 quantityField;
        
		private PreferenceCriterionCodeType preferenceCriterionCodeField;
        
		private RequiredCustomsIDType requiredCustomsIDField;
        
		private CustomsStatusCodeType customsStatusCodeField;
        
		private CustomsTariffQuantityType customsTariffQuantityField;
        
		private CustomsImportClassifiedIndicatorType customsImportClassifiedIndicatorField;
        
		private ItemType[] itemField;
        
		private GoodsItemContainerType[] goodsItemContainerField;
        
		private AllowanceChargeType[] freightAllowanceChargeField;
        
		private InvoiceLineType[] invoiceLineField;
        
		private TemperatureType[] temperatureField;
        
		private GoodsItemType[] containedGoodsItemField;
        
		private AddressType originAddressField;
        
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
		public SequenceNumberIDType SequenceNumberID {
			get {
				return this.sequenceNumberIDField;
			}
			set {
				this.sequenceNumberIDField = value;
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
		public DeclaredCustomsValueAmountType DeclaredCustomsValueAmount {
			get {
				return this.declaredCustomsValueAmountField;
			}
			set {
				this.declaredCustomsValueAmountField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public DeclaredForCarriageValueAmountType DeclaredForCarriageValueAmount {
			get {
				return this.declaredForCarriageValueAmountField;
			}
			set {
				this.declaredForCarriageValueAmountField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public DeclaredStatisticsValueAmountType DeclaredStatisticsValueAmount {
			get {
				return this.declaredStatisticsValueAmountField;
			}
			set {
				this.declaredStatisticsValueAmountField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public FreeOnBoardValueAmountType FreeOnBoardValueAmount {
			get {
				return this.freeOnBoardValueAmountField;
			}
			set {
				this.freeOnBoardValueAmountField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public InsuranceValueAmountType InsuranceValueAmount {
			get {
				return this.insuranceValueAmountField;
			}
			set {
				this.insuranceValueAmountField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public ValueAmountType ValueAmount {
			get {
				return this.valueAmountField;
			}
			set {
				this.valueAmountField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public GrossWeightMeasureType GrossWeightMeasure {
			get {
				return this.grossWeightMeasureField;
			}
			set {
				this.grossWeightMeasureField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public NetWeightMeasureType NetWeightMeasure {
			get {
				return this.netWeightMeasureField;
			}
			set {
				this.netWeightMeasureField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public NetNetWeightMeasureType NetNetWeightMeasure {
			get {
				return this.netNetWeightMeasureField;
			}
			set {
				this.netNetWeightMeasureField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public ChargeableWeightMeasureType ChargeableWeightMeasure {
			get {
				return this.chargeableWeightMeasureField;
			}
			set {
				this.chargeableWeightMeasureField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public GrossVolumeMeasureType GrossVolumeMeasure {
			get {
				return this.grossVolumeMeasureField;
			}
			set {
				this.grossVolumeMeasureField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public NetVolumeMeasureType NetVolumeMeasure {
			get {
				return this.netVolumeMeasureField;
			}
			set {
				this.netVolumeMeasureField = value;
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
		public PreferenceCriterionCodeType PreferenceCriterionCode {
			get {
				return this.preferenceCriterionCodeField;
			}
			set {
				this.preferenceCriterionCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public RequiredCustomsIDType RequiredCustomsID {
			get {
				return this.requiredCustomsIDField;
			}
			set {
				this.requiredCustomsIDField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public CustomsStatusCodeType CustomsStatusCode {
			get {
				return this.customsStatusCodeField;
			}
			set {
				this.customsStatusCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public CustomsTariffQuantityType CustomsTariffQuantity {
			get {
				return this.customsTariffQuantityField;
			}
			set {
				this.customsTariffQuantityField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public CustomsImportClassifiedIndicatorType CustomsImportClassifiedIndicator {
			get {
				return this.customsImportClassifiedIndicatorField;
			}
			set {
				this.customsImportClassifiedIndicatorField = value;
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
		[System.Xml.Serialization.XmlElementAttribute("GoodsItemContainer")]
		public GoodsItemContainerType[] GoodsItemContainer {
			get {
				return this.goodsItemContainerField;
			}
			set {
				this.goodsItemContainerField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("FreightAllowanceCharge")]
		public AllowanceChargeType[] FreightAllowanceCharge {
			get {
				return this.freightAllowanceChargeField;
			}
			set {
				this.freightAllowanceChargeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("InvoiceLine")]
		public InvoiceLineType[] InvoiceLine {
			get {
				return this.invoiceLineField;
			}
			set {
				this.invoiceLineField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("Temperature")]
		public TemperatureType[] Temperature {
			get {
				return this.temperatureField;
			}
			set {
				this.temperatureField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("ContainedGoodsItem")]
		public GoodsItemType[] ContainedGoodsItem {
			get {
				return this.containedGoodsItemField;
			}
			set {
				this.containedGoodsItemField = value;
			}
		}
        
		/// <comentarios/>
		public AddressType OriginAddress {
			get {
				return this.originAddressField;
			}
			set {
				this.originAddressField = value;
			}
		}
	}
}