using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ExtensionReasonType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ExtensionAgencyNameType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ZoneType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(XPathType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(WarrantyInformationType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ValueType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(UnitType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TradingRestrictionsType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TitleType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TimingComplaintType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TimezoneOffsetType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TierRangeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TextType1))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TermsType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TelephoneType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TelefaxType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TaxExemptionReasonType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TariffDescriptionType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(SummaryDescriptionType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(StatusReasonType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(SpecialTermsType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(SpecialInstructionsType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(SignatureMethodType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ShippingMarksType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(SealingPartyTypeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(RoomType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(RemarksType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(RejectReasonType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(RejectionNoteType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(RegistrationNationalityType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(RegionType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ReferenceType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ReasonType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(QualifierType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PriorityType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PrintQualifierType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PriceTypeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PriceChangeReasonType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PostboxType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PostalZoneType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PlotIdentificationType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PlacardNotationType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PlacardEndorsementType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PaymentOrderReferenceType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PaymentNoteType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PayerReferenceType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PackingMaterialType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(OutstandingReasonType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(OtherInstructionType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganizationDepartmentType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(OrderableUnitType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(NoteType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(NationalityType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(NameSuffixType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(MarksType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(MarkCareType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(MarkAttentionType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(MailType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(LossRiskType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(LocationType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(LineType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(KeywordType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(JobTitleType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(InvoicingPartyReferenceType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(InstructionsType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(InstructionNoteType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(InstructionType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(InhouseMailType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(InformationType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(HandlingInstructionsType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(FloorType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ExtensionType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ExemptionReasonType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ElectronicMailType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(DocumentTypeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(DocumentHashType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(DistrictType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(DescriptionType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(DepartmentType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(DeliveryInstructionsType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(DataSendingCapabilityType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(DamageRemarksType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(CustomerReferenceType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(CountrySubentityType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ContractTypeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ContractSubdivisionType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ConditionsType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ConditionType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(CodeValueType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ChannelType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(CertificateTypeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(CanonicalizationMethodType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(CancellationNoteType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(BuildingNumberType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(BackorderReasonType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ApprovalStatusType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(AllowanceChargeReasonType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(AgencyNameType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(AdditionalInformationType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(AccountingCostType))]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2")]
	public partial class TextType {
        
		private string languageIDField;
        
		private string valueField;
        
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
		[System.Xml.Serialization.XmlTextAttribute()]
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