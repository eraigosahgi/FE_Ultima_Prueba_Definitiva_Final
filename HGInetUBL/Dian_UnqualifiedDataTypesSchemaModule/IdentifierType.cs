using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ExtensionVersionIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ExtensionURIType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ExtensionAgencyURIType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ExtensionAgencyIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(WebsiteURIType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(VesselIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(VersionIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ValidatorIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(UUIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(URIType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(UpperOrangeHazardPlacardIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(UBLVersionIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TrainIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TrackingIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(SupplierAssignedAccountIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ShippingOrderIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(SerialIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(SequenceNumberIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(SequenceIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(SchemeURIType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(SalesOrderLineIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(SalesOrderIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(RequiredCustomsIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(RegistrationNationalityIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(RegistrationIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ReferenceIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(RailCarIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ProfileIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ProductTraceIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PrimaryAccountNumberIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PreviousVersionIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PreviousJobIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PrepaidPaymentReferenceIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PaymentMeansIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PaymentIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ParentDocumentIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(OriginalJobIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(OrderIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(OrangeHazardPlacardIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(NumberIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(NetworkIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(NationalityIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(MarkingIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(LowerOrangeHazardPlacardIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(LotNumberIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(LogoReferenceIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(LocationIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(LineIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(LicensePlateIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(LanguageIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(JourneyIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(JobIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(IssuerIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(IssueNumberIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(InstructionIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(IdentificationIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(IDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(HazardClassIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ExtendedIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ExchangeMarketIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(EndpointIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(DocumentIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(CV2IDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(CustomsIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(CustomizationIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(CustomerAssignedAccountIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(CompanyIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ChipApplicationIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(CarrierAssignedIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(AttributeIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ApplicationIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(AircraftIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(AgencyIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(AdditionalAccountIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(AccountNumberIDType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(AccountIDType))]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2")]
	public partial class IdentifierType {
        
		private string schemeIDField;
        
		private string schemeNameField;
        
		private string schemeAgencyIDField;
        
		private string schemeAgencyNameField;
        
		private string schemeVersionIDField;
        
		private string schemeDataURIField;
        
		private string schemeURIField;
        
		private string valueField;
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlAttributeAttribute(DataType="normalizedString")]
		public string schemeID {
			get {
				return this.schemeIDField;
			}
			set {
				this.schemeIDField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string schemeName {
			get {
				return this.schemeNameField;
			}
			set {
				this.schemeNameField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlAttributeAttribute(DataType="normalizedString")]
		public string schemeAgencyID {
			get {
				return this.schemeAgencyIDField;
			}
			set {
				this.schemeAgencyIDField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string schemeAgencyName {
			get {
				return this.schemeAgencyNameField;
			}
			set {
				this.schemeAgencyNameField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlAttributeAttribute(DataType="normalizedString")]
		public string schemeVersionID {
			get {
				return this.schemeVersionIDField;
			}
			set {
				this.schemeVersionIDField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlAttributeAttribute(DataType="anyURI")]
		public string schemeDataURI {
			get {
				return this.schemeDataURIField;
			}
			set {
				this.schemeDataURIField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlAttributeAttribute(DataType="anyURI")]
		public string schemeURI {
			get {
				return this.schemeURIField;
			}
			set {
				this.schemeURIField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlTextAttribute(DataType="normalizedString")]
		public string Value {
			get {
				return this.valueField;
			}
			set {
				this.valueField = value;
			}
		}
	}
}