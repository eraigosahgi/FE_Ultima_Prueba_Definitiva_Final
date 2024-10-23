using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("HazardousItem", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
	public partial class HazardousItemType {
        
		private IDType idField;
        
		private PlacardNotationType placardNotationField;
        
		private PlacardEndorsementType placardEndorsementField;
        
		private AdditionalInformationType additionalInformationField;
        
		private UNDGCodeType uNDGCodeField;
        
		private EmergencyProceduresCodeType emergencyProceduresCodeField;
        
		private MedicalFirstAidGuideCodeType medicalFirstAidGuideCodeField;
        
		private TechnicalNameType technicalNameField;
        
		private CategoryNameType categoryNameField;
        
		private HazardousCategoryCodeType hazardousCategoryCodeField;
        
		private UpperOrangeHazardPlacardIDType upperOrangeHazardPlacardIDField;
        
		private LowerOrangeHazardPlacardIDType lowerOrangeHazardPlacardIDField;
        
		private MarkingIDType markingIDField;
        
		private HazardClassIDType hazardClassIDField;
        
		private NetWeightMeasureType netWeightMeasureField;
        
		private NetVolumeMeasureType netVolumeMeasureField;
        
		private QuantityType1 quantityField;
        
		private PartyType contactPartyField;
        
		private SecondaryHazardType[] secondaryHazardField;
        
		private HazardousGoodsTransitType[] hazardousGoodsTransitField;
        
		private TemperatureType emergencyTemperatureField;
        
		private TemperatureType flashpointTemperatureField;
        
		private TemperatureType[] additionalTemperatureField;
        
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
		public PlacardNotationType PlacardNotation {
			get {
				return this.placardNotationField;
			}
			set {
				this.placardNotationField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public PlacardEndorsementType PlacardEndorsement {
			get {
				return this.placardEndorsementField;
			}
			set {
				this.placardEndorsementField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public AdditionalInformationType AdditionalInformation {
			get {
				return this.additionalInformationField;
			}
			set {
				this.additionalInformationField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public UNDGCodeType UNDGCode {
			get {
				return this.uNDGCodeField;
			}
			set {
				this.uNDGCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public EmergencyProceduresCodeType EmergencyProceduresCode {
			get {
				return this.emergencyProceduresCodeField;
			}
			set {
				this.emergencyProceduresCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public MedicalFirstAidGuideCodeType MedicalFirstAidGuideCode {
			get {
				return this.medicalFirstAidGuideCodeField;
			}
			set {
				this.medicalFirstAidGuideCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public TechnicalNameType TechnicalName {
			get {
				return this.technicalNameField;
			}
			set {
				this.technicalNameField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public CategoryNameType CategoryName {
			get {
				return this.categoryNameField;
			}
			set {
				this.categoryNameField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public HazardousCategoryCodeType HazardousCategoryCode {
			get {
				return this.hazardousCategoryCodeField;
			}
			set {
				this.hazardousCategoryCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public UpperOrangeHazardPlacardIDType UpperOrangeHazardPlacardID {
			get {
				return this.upperOrangeHazardPlacardIDField;
			}
			set {
				this.upperOrangeHazardPlacardIDField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public LowerOrangeHazardPlacardIDType LowerOrangeHazardPlacardID {
			get {
				return this.lowerOrangeHazardPlacardIDField;
			}
			set {
				this.lowerOrangeHazardPlacardIDField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public MarkingIDType MarkingID {
			get {
				return this.markingIDField;
			}
			set {
				this.markingIDField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public HazardClassIDType HazardClassID {
			get {
				return this.hazardClassIDField;
			}
			set {
				this.hazardClassIDField = value;
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
		public PartyType ContactParty {
			get {
				return this.contactPartyField;
			}
			set {
				this.contactPartyField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("SecondaryHazard")]
		public SecondaryHazardType[] SecondaryHazard {
			get {
				return this.secondaryHazardField;
			}
			set {
				this.secondaryHazardField = value;
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
		public TemperatureType EmergencyTemperature {
			get {
				return this.emergencyTemperatureField;
			}
			set {
				this.emergencyTemperatureField = value;
			}
		}
        
		/// <comentarios/>
		public TemperatureType FlashpointTemperature {
			get {
				return this.flashpointTemperatureField;
			}
			set {
				this.flashpointTemperatureField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("AdditionalTemperature")]
		public TemperatureType[] AdditionalTemperature {
			get {
				return this.additionalTemperatureField;
			}
			set {
				this.additionalTemperatureField = value;
			}
		}
	}
}