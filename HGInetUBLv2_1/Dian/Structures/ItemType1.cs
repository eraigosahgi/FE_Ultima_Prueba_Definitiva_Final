using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace HGInetUBLv2_1
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ItemType", Namespace = "http://www.dian.gov.co/contratos/facturaelectronica/v1")]
	[System.Xml.Serialization.XmlRootAttribute("Item", Namespace = "http://www.dian.gov.co/contratos/facturaelectronica/v1", IsNullable = false)]
	public partial class ItemType1
	{

		private DescriptionType[] descriptionField;

		private PackQuantityType packQuantityField;

		private PackSizeNumericType packSizeNumericField;

		private CatalogueIndicatorType catalogueIndicatorField;

		private NameType1 nameField;

		private HazardousRiskIndicatorType hazardousRiskIndicatorField;

		private AdditionalInformationType additionalInformationField;

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

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("Description", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public DescriptionType[] Description
		{
			get
			{
				return this.descriptionField;
			}
			set
			{
				this.descriptionField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public PackQuantityType PackQuantity
		{
			get
			{
				return this.packQuantityField;
			}
			set
			{
				this.packQuantityField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public PackSizeNumericType PackSizeNumeric
		{
			get
			{
				return this.packSizeNumericField;
			}
			set
			{
				this.packSizeNumericField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public CatalogueIndicatorType CatalogueIndicator
		{
			get
			{
				return this.catalogueIndicatorField;
			}
			set
			{
				this.catalogueIndicatorField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public NameType1 Name
		{
			get
			{
				return this.nameField;
			}
			set
			{
				this.nameField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public HazardousRiskIndicatorType HazardousRiskIndicator
		{
			get
			{
				return this.hazardousRiskIndicatorField;
			}
			set
			{
				this.hazardousRiskIndicatorField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public AdditionalInformationType AdditionalInformation
		{
			get
			{
				return this.additionalInformationField;
			}
			set
			{
				this.additionalInformationField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("Keyword", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public KeywordType[] Keyword
		{
			get
			{
				return this.keywordField;
			}
			set
			{
				this.keywordField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("BrandName", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public BrandNameType[] BrandName
		{
			get
			{
				return this.brandNameField;
			}
			set
			{
				this.brandNameField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("ModelName", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public ModelNameType[] ModelName
		{
			get
			{
				return this.modelNameField;
			}
			set
			{
				this.modelNameField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public ItemIdentificationType BuyersItemIdentification
		{
			get
			{
				return this.buyersItemIdentificationField;
			}
			set
			{
				this.buyersItemIdentificationField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public ItemIdentificationType SellersItemIdentification
		{
			get
			{
				return this.sellersItemIdentificationField;
			}
			set
			{
				this.sellersItemIdentificationField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("ManufacturersItemIdentification", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public ItemIdentificationType[] ManufacturersItemIdentification
		{
			get
			{
				return this.manufacturersItemIdentificationField;
			}
			set
			{
				this.manufacturersItemIdentificationField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public ItemIdentificationType StandardItemIdentification
		{
			get
			{
				return this.standardItemIdentificationField;
			}
			set
			{
				this.standardItemIdentificationField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public ItemIdentificationType CatalogueItemIdentification
		{
			get
			{
				return this.catalogueItemIdentificationField;
			}
			set
			{
				this.catalogueItemIdentificationField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("AdditionalItemIdentification", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public ItemIdentificationType[] AdditionalItemIdentification
		{
			get
			{
				return this.additionalItemIdentificationField;
			}
			set
			{
				this.additionalItemIdentificationField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public DocumentReferenceType CatalogueDocumentReference
		{
			get
			{
				return this.catalogueDocumentReferenceField;
			}
			set
			{
				this.catalogueDocumentReferenceField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("ItemSpecificationDocumentReference", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public DocumentReferenceType[] ItemSpecificationDocumentReference
		{
			get
			{
				return this.itemSpecificationDocumentReferenceField;
			}
			set
			{
				this.itemSpecificationDocumentReferenceField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public CountryType OriginCountry
		{
			get
			{
				return this.originCountryField;
			}
			set
			{
				this.originCountryField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("CommodityClassification", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public CommodityClassificationType[] CommodityClassification
		{
			get
			{
				return this.commodityClassificationField;
			}
			set
			{
				this.commodityClassificationField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("TransactionConditions", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public TransactionConditionsType[] TransactionConditions
		{
			get
			{
				return this.transactionConditionsField;
			}
			set
			{
				this.transactionConditionsField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("HazardousItem", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public HazardousItemType[] HazardousItem
		{
			get
			{
				return this.hazardousItemField;
			}
			set
			{
				this.hazardousItemField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("ClassifiedTaxCategory", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public TaxCategoryType[] ClassifiedTaxCategory
		{
			get
			{
				return this.classifiedTaxCategoryField;
			}
			set
			{
				this.classifiedTaxCategoryField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("AdditionalItemProperty", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public ItemPropertyType[] AdditionalItemProperty
		{
			get
			{
				return this.additionalItemPropertyField;
			}
			set
			{
				this.additionalItemPropertyField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("ManufacturerParty", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public PartyType[] ManufacturerParty
		{
			get
			{
				return this.manufacturerPartyField;
			}
			set
			{
				this.manufacturerPartyField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public PartyType InformationContentProviderParty
		{
			get
			{
				return this.informationContentProviderPartyField;
			}
			set
			{
				this.informationContentProviderPartyField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("OriginAddress", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public AddressType[] OriginAddress
		{
			get
			{
				return this.originAddressField;
			}
			set
			{
				this.originAddressField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("ItemInstance", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public ItemInstanceType[] ItemInstance
		{
			get
			{
				return this.itemInstanceField;
			}
			set
			{
				this.itemInstanceField = value;
			}
		}
	}
}