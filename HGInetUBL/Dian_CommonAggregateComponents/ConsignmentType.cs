using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("Consignment", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
	public partial class ConsignmentType {
        
		private IDType idField;
        
		private SummaryDescriptionType[] summaryDescriptionField;
        
		private TotalInvoiceAmountType totalInvoiceAmountField;
        
		private DeclaredCustomsValueAmountType declaredCustomsValueAmountField;
        
		private TariffDescriptionType[] tariffDescriptionField;
        
		private TariffCodeType tariffCodeField;
        
		private InsurancePremiumAmountType insurancePremiumAmountField;
        
		private GrossWeightMeasureType grossWeightMeasureField;
        
		private NetWeightMeasureType netWeightMeasureField;
        
		private NetNetWeightMeasureType netNetWeightMeasureField;
        
		private ChargeableWeightMeasureType chargeableWeightMeasureField;
        
		private GrossVolumeMeasureType grossVolumeMeasureField;
        
		private NetVolumeMeasureType netVolumeMeasureField;
        
		private LoadingLengthMeasureType loadingLengthMeasureField;
        
		private RemarksType[] remarksField;
        
		private HazardousRiskIndicatorType hazardousRiskIndicatorField;
        
		private PartyType consigneePartyField;
        
		private PartyType exporterPartyField;
        
		private PartyType consignorPartyField;
        
		private PartyType importerPartyField;
        
		private PartyType carrierPartyField;
        
		private PartyType freightForwarderPartyField;
        
		private PartyType notifyPartyField;
        
		private PartyType originalDespatchPartyField;
        
		private PartyType finalDeliveryPartyField;
        
		private CountryType originalDepartureCountryField;
        
		private CountryType finalDestinationCountryField;
        
		private CountryType[] transitCountryField;
        
		private ContractType transportContractField;
        
		private TransportationServiceType originalDespatchTransportationServiceField;
        
		private TransportationServiceType finalDeliveryTransportationServiceField;
        
		private DeliveryTermsType deliveryTermsField;
        
		private PaymentTermsType paymentTermsField;
        
		private AllowanceChargeType[] freightAllowanceChargeField;
        
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
		[System.Xml.Serialization.XmlElementAttribute("SummaryDescription", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public SummaryDescriptionType[] SummaryDescription {
			get {
				return this.summaryDescriptionField;
			}
			set {
				this.summaryDescriptionField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public TotalInvoiceAmountType TotalInvoiceAmount {
			get {
				return this.totalInvoiceAmountField;
			}
			set {
				this.totalInvoiceAmountField = value;
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
		[System.Xml.Serialization.XmlElementAttribute("TariffDescription", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public TariffDescriptionType[] TariffDescription {
			get {
				return this.tariffDescriptionField;
			}
			set {
				this.tariffDescriptionField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public TariffCodeType TariffCode {
			get {
				return this.tariffCodeField;
			}
			set {
				this.tariffCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public InsurancePremiumAmountType InsurancePremiumAmount {
			get {
				return this.insurancePremiumAmountField;
			}
			set {
				this.insurancePremiumAmountField = value;
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
		public LoadingLengthMeasureType LoadingLengthMeasure {
			get {
				return this.loadingLengthMeasureField;
			}
			set {
				this.loadingLengthMeasureField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("Remarks", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public RemarksType[] Remarks {
			get {
				return this.remarksField;
			}
			set {
				this.remarksField = value;
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
		public PartyType ConsigneeParty {
			get {
				return this.consigneePartyField;
			}
			set {
				this.consigneePartyField = value;
			}
		}
        
		/// <comentarios/>
		public PartyType ExporterParty {
			get {
				return this.exporterPartyField;
			}
			set {
				this.exporterPartyField = value;
			}
		}
        
		/// <comentarios/>
		public PartyType ConsignorParty {
			get {
				return this.consignorPartyField;
			}
			set {
				this.consignorPartyField = value;
			}
		}
        
		/// <comentarios/>
		public PartyType ImporterParty {
			get {
				return this.importerPartyField;
			}
			set {
				this.importerPartyField = value;
			}
		}
        
		/// <comentarios/>
		public PartyType CarrierParty {
			get {
				return this.carrierPartyField;
			}
			set {
				this.carrierPartyField = value;
			}
		}
        
		/// <comentarios/>
		public PartyType FreightForwarderParty {
			get {
				return this.freightForwarderPartyField;
			}
			set {
				this.freightForwarderPartyField = value;
			}
		}
        
		/// <comentarios/>
		public PartyType NotifyParty {
			get {
				return this.notifyPartyField;
			}
			set {
				this.notifyPartyField = value;
			}
		}
        
		/// <comentarios/>
		public PartyType OriginalDespatchParty {
			get {
				return this.originalDespatchPartyField;
			}
			set {
				this.originalDespatchPartyField = value;
			}
		}
        
		/// <comentarios/>
		public PartyType FinalDeliveryParty {
			get {
				return this.finalDeliveryPartyField;
			}
			set {
				this.finalDeliveryPartyField = value;
			}
		}
        
		/// <comentarios/>
		public CountryType OriginalDepartureCountry {
			get {
				return this.originalDepartureCountryField;
			}
			set {
				this.originalDepartureCountryField = value;
			}
		}
        
		/// <comentarios/>
		public CountryType FinalDestinationCountry {
			get {
				return this.finalDestinationCountryField;
			}
			set {
				this.finalDestinationCountryField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("TransitCountry")]
		public CountryType[] TransitCountry {
			get {
				return this.transitCountryField;
			}
			set {
				this.transitCountryField = value;
			}
		}
        
		/// <comentarios/>
		public ContractType TransportContract {
			get {
				return this.transportContractField;
			}
			set {
				this.transportContractField = value;
			}
		}
        
		/// <comentarios/>
		public TransportationServiceType OriginalDespatchTransportationService {
			get {
				return this.originalDespatchTransportationServiceField;
			}
			set {
				this.originalDespatchTransportationServiceField = value;
			}
		}
        
		/// <comentarios/>
		public TransportationServiceType FinalDeliveryTransportationService {
			get {
				return this.finalDeliveryTransportationServiceField;
			}
			set {
				this.finalDeliveryTransportationServiceField = value;
			}
		}
        
		/// <comentarios/>
		public DeliveryTermsType DeliveryTerms {
			get {
				return this.deliveryTermsField;
			}
			set {
				this.deliveryTermsField = value;
			}
		}
        
		/// <comentarios/>
		public PaymentTermsType PaymentTerms {
			get {
				return this.paymentTermsField;
			}
			set {
				this.paymentTermsField = value;
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
	}
}