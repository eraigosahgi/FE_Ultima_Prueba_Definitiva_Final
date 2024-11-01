﻿/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("Item", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class ItemType {
    
	private DescriptionType[] descriptionField;
    
	private PackQuantityType packQuantityField;
    
	private PackSizeNumericType packSizeNumericField;
    
	private CatalogueIndicatorType catalogueIndicatorField;
    
	private NameType1 nameField;
    
	private HazardousRiskIndicatorType hazardousRiskIndicatorField;
    
	private AdditionalInformationType[] additionalInformationField;
    
	private KeywordType[] keywordField;
    
	private BrandNameType[] brandNameField;
    
	private ModelNameType[] modelNameField;
    
	private ItemIdentificationType buyersItemIdentificationField;
    
	private ItemIdentificationType sellersItemIdentificationField;
    
	private ItemIdentificationType[] manufacturersItemIdentificationField;
    
	private ItemIdentificationType standardItemIdentificationField;
    
	private ItemIdentificationType catalogueItemIdentificationField;
    
	private ItemIdentificationType[] additionalItemIdentificationField;
    
	private DocumentReferenceType catalogueDocumentReferenceField;
    
	private DocumentReferenceType[] itemSpecificationDocumentReferenceField;
    
	private CountryType originCountryField;
    
	private CommodityClassificationType[] commodityClassificationField;
    
	private TransactionConditionsType[] transactionConditionsField;
    
	private HazardousItemType[] hazardousItemField;
    
	private TaxCategoryType[] classifiedTaxCategoryField;
    
	private ItemPropertyType[] additionalItemPropertyField;
    
	private PartyType[] manufacturerPartyField;
    
	private PartyType informationContentProviderPartyField;
    
	private AddressType[] originAddressField;
    
	private ItemInstanceType[] itemInstanceField;
    
	private CertificateType[] certificateField;
    
	private DimensionType[] dimensionField;
    
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
	public PackQuantityType PackQuantity {
		get {
			return this.packQuantityField;
		}
		set {
			this.packQuantityField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public PackSizeNumericType PackSizeNumeric {
		get {
			return this.packSizeNumericField;
		}
		set {
			this.packSizeNumericField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public CatalogueIndicatorType CatalogueIndicator {
		get {
			return this.catalogueIndicatorField;
		}
		set {
			this.catalogueIndicatorField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public NameType1 Name {
		get {
			return this.nameField;
		}
		set {
			this.nameField = value;
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
	[System.Xml.Serialization.XmlElementAttribute("AdditionalInformation", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public AdditionalInformationType[] AdditionalInformation {
		get {
			return this.additionalInformationField;
		}
		set {
			this.additionalInformationField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("Keyword", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public KeywordType[] Keyword {
		get {
			return this.keywordField;
		}
		set {
			this.keywordField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("BrandName", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public BrandNameType[] BrandName {
		get {
			return this.brandNameField;
		}
		set {
			this.brandNameField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("ModelName", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ModelNameType[] ModelName {
		get {
			return this.modelNameField;
		}
		set {
			this.modelNameField = value;
		}
	}
    
	/// <comentarios/>
	public ItemIdentificationType BuyersItemIdentification {
		get {
			return this.buyersItemIdentificationField;
		}
		set {
			this.buyersItemIdentificationField = value;
		}
	}
    
	/// <comentarios/>
	public ItemIdentificationType SellersItemIdentification {
		get {
			return this.sellersItemIdentificationField;
		}
		set {
			this.sellersItemIdentificationField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("ManufacturersItemIdentification")]
	public ItemIdentificationType[] ManufacturersItemIdentification {
		get {
			return this.manufacturersItemIdentificationField;
		}
		set {
			this.manufacturersItemIdentificationField = value;
		}
	}
    
	/// <comentarios/>
	public ItemIdentificationType StandardItemIdentification {
		get {
			return this.standardItemIdentificationField;
		}
		set {
			this.standardItemIdentificationField = value;
		}
	}
    
	/// <comentarios/>
	public ItemIdentificationType CatalogueItemIdentification {
		get {
			return this.catalogueItemIdentificationField;
		}
		set {
			this.catalogueItemIdentificationField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("AdditionalItemIdentification")]
	public ItemIdentificationType[] AdditionalItemIdentification {
		get {
			return this.additionalItemIdentificationField;
		}
		set {
			this.additionalItemIdentificationField = value;
		}
	}
    
	/// <comentarios/>
	public DocumentReferenceType CatalogueDocumentReference {
		get {
			return this.catalogueDocumentReferenceField;
		}
		set {
			this.catalogueDocumentReferenceField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("ItemSpecificationDocumentReference")]
	public DocumentReferenceType[] ItemSpecificationDocumentReference {
		get {
			return this.itemSpecificationDocumentReferenceField;
		}
		set {
			this.itemSpecificationDocumentReferenceField = value;
		}
	}
    
	/// <comentarios/>
	public CountryType OriginCountry {
		get {
			return this.originCountryField;
		}
		set {
			this.originCountryField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("CommodityClassification")]
	public CommodityClassificationType[] CommodityClassification {
		get {
			return this.commodityClassificationField;
		}
		set {
			this.commodityClassificationField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("TransactionConditions")]
	public TransactionConditionsType[] TransactionConditions {
		get {
			return this.transactionConditionsField;
		}
		set {
			this.transactionConditionsField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("HazardousItem")]
	public HazardousItemType[] HazardousItem {
		get {
			return this.hazardousItemField;
		}
		set {
			this.hazardousItemField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("ClassifiedTaxCategory")]
	public TaxCategoryType[] ClassifiedTaxCategory {
		get {
			return this.classifiedTaxCategoryField;
		}
		set {
			this.classifiedTaxCategoryField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("AdditionalItemProperty")]
	public ItemPropertyType[] AdditionalItemProperty {
		get {
			return this.additionalItemPropertyField;
		}
		set {
			this.additionalItemPropertyField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("ManufacturerParty")]
	public PartyType[] ManufacturerParty {
		get {
			return this.manufacturerPartyField;
		}
		set {
			this.manufacturerPartyField = value;
		}
	}
    
	/// <comentarios/>
	public PartyType InformationContentProviderParty {
		get {
			return this.informationContentProviderPartyField;
		}
		set {
			this.informationContentProviderPartyField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("OriginAddress")]
	public AddressType[] OriginAddress {
		get {
			return this.originAddressField;
		}
		set {
			this.originAddressField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("ItemInstance")]
	public ItemInstanceType[] ItemInstance {
		get {
			return this.itemInstanceField;
		}
		set {
			this.itemInstanceField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("Certificate")]
	public CertificateType[] Certificate {
		get {
			return this.certificateField;
		}
		set {
			this.certificateField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("Dimension")]
	public DimensionType[] Dimension {
		get {
			return this.dimensionField;
		}
		set {
			this.dimensionField = value;
		}
	}
}