using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ExtensionReasonCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(UnitOfMeasureCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TransportModeCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TransportModeCodeType1))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TransportEquipmentTypeCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TransportEquipmentTypeCodeType1))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TransportationStatusCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ConditionCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(SubstitutionStatusCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(SubstitutionStatusCodeType1))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PortCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PaymentMeansCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PaymentMeansCodeType1))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PackagingTypeCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PackagingTypeCodeType1))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(OperatorCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(MathematicOperatorCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(LongitudeDirectionCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(LongitudeDirectionCodeType1))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(LineStatusCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(LineStatusCodeType1))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(LatitudeDirectionCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(LatitudeDirectionCodeType1))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(DocumentStatusCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(DocumentStatusCodeType1))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(CurrencyCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TaxCurrencyCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TargetCurrencyCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(SourceCurrencyCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(RequestedInvoiceCurrencyCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PricingCurrencyCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PaymentCurrencyCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PaymentAlternativeCurrencyCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(DocumentCurrencyCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(CurrencyCodeType1))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(CountryIdentificationCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(IdentificationCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ContainerSizeTypeCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ChipCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(CardChipCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ChannelCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ChannelCodeType1))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(AllowanceChargeReasonCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(AllowanceChargeReasonCodeType1))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(UNDGCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TransportServiceCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TransportMeansTypeCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TransportHandlingUnitTypeCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TransportEventTypeCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TransportEmergencyCardCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TransportAuthorizationCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TransitDirectionCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TimingComplaintCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TaxTypeCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TaxLevelCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TaxExemptionReasonCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TariffCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TariffClassCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(StatusReasonCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(StatusCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(SizeTypeCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ShortageActionCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ShippingPriorityLevelCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(SealStatusCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(SealIssuerTypeCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(RoleCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ResponseCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ReminderTypeCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(RejectReasonCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(RejectActionCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ReferenceEventCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ProviderTypeCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PriceTypeCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PreferenceCriterionCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PositionCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PaymentChannelCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ParentDocumentTypeCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PackLevelCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PackingCriteriaCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PackageLevelCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(OwnerTypeCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(NatureCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(MedicalFirstAidGuideCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(LossRiskResponsibilityCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(LocaleCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(LifeCycleStatusCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ItemClassificationCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(InvoiceTypeCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(InspectionMethodCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(InhalationToxicityZoneCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(HazardousRegulationCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(HazardousCategoryCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(HandlingCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(FullnessIndicationCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(FreightRateClassCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ExemptionReasonCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(EventCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(EmergencyProceduresCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(DocumentTypeCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(DispositionCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(DirectionCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(DespatchAdviceTypeCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(DescriptionCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(CustomsStatusCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(CountrySubentityCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(CorporateRegistrationTypeCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(CoordinateSystemCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ContractTypeCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(CommodityCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(CargoTypeCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(CardTypeCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ApplicationStatusCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(AddressTypeCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(AddressFormatCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ActionCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(AccountTypeCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(AccountingCostCodeType))]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2")]
	public partial class CodeType {
        
		private string listIDField;
        
		private string listAgencyIDField;
        
		private string listAgencyNameField;
        
		private string listNameField;
        
		private string listVersionIDField;
        
		private string nameField;
        
		private string languageIDField;
        
		private string listURIField;
        
		private string listSchemeURIField;
        
		private string valueField;
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlAttributeAttribute(DataType="normalizedString")]
		public string listID {
			get {
				return this.listIDField;
			}
			set {
				this.listIDField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlAttributeAttribute(DataType="normalizedString")]
		public string listAgencyID {
			get {
				return this.listAgencyIDField;
			}
			set {
				this.listAgencyIDField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string listAgencyName {
			get {
				return this.listAgencyNameField;
			}
			set {
				this.listAgencyNameField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string listName {
			get {
				return this.listNameField;
			}
			set {
				this.listNameField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlAttributeAttribute(DataType="normalizedString")]
		public string listVersionID {
			get {
				return this.listVersionIDField;
			}
			set {
				this.listVersionIDField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string name {
			get {
				return this.nameField;
			}
			set {
				this.nameField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlAttributeAttribute(DataType="language")]
		public string languageID {
			get {
				return this.languageIDField;
			}
			set {
				this.languageIDField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlAttributeAttribute(DataType="anyURI")]
		public string listURI {
			get {
				return this.listURIField;
			}
			set {
				this.listURIField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlAttributeAttribute(DataType="anyURI")]
		public string listSchemeURI {
			get {
				return this.listSchemeURIField;
			}
			set {
				this.listSchemeURIField = value;
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