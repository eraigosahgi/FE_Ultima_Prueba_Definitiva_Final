using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("CatalogueLine", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
	public partial class CatalogueLineType {
        
		private IDType idField;
        
		private ActionCodeType actionCodeField;
        
		private LifeCycleStatusCodeType lifeCycleStatusCodeField;
        
		private ContractSubdivisionType contractSubdivisionField;
        
		private NoteType[] noteField;
        
		private OrderableIndicatorType orderableIndicatorField;
        
		private OrderableUnitType orderableUnitField;
        
		private ContentUnitQuantityType contentUnitQuantityField;
        
		private OrderQuantityIncrementNumericType orderQuantityIncrementNumericField;
        
		private MinimumOrderQuantityType minimumOrderQuantityField;
        
		private MaximumOrderQuantityType maximumOrderQuantityField;
        
		private WarrantyInformationType[] warrantyInformationField;
        
		private PackLevelCodeType packLevelCodeField;
        
		private CustomerPartyType contractorCustomerPartyField;
        
		private SupplierPartyType sellerSupplierPartyField;
        
		private PartyType warrantyPartyField;
        
		private PeriodType warrantyValidityPeriodField;
        
		private PeriodType lineValidityPeriodField;
        
		private ItemComparisonType[] itemComparisonField;
        
		private RelatedItemType[] componentRelatedItemField;
        
		private RelatedItemType[] accessoryRelatedItemField;
        
		private RelatedItemType[] requiredRelatedItemField;
        
		private RelatedItemType[] replacementRelatedItemField;
        
		private RelatedItemType[] complementaryRelatedItemField;
        
		private ItemLocationQuantityType[] requiredItemLocationQuantityField;
        
		private DocumentReferenceType[] documentReferenceField;
        
		private ItemType itemField;
        
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
		public ActionCodeType ActionCode {
			get {
				return this.actionCodeField;
			}
			set {
				this.actionCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public LifeCycleStatusCodeType LifeCycleStatusCode {
			get {
				return this.lifeCycleStatusCodeField;
			}
			set {
				this.lifeCycleStatusCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public ContractSubdivisionType ContractSubdivision {
			get {
				return this.contractSubdivisionField;
			}
			set {
				this.contractSubdivisionField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("Note", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public NoteType[] Note {
			get {
				return this.noteField;
			}
			set {
				this.noteField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public OrderableIndicatorType OrderableIndicator {
			get {
				return this.orderableIndicatorField;
			}
			set {
				this.orderableIndicatorField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public OrderableUnitType OrderableUnit {
			get {
				return this.orderableUnitField;
			}
			set {
				this.orderableUnitField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public ContentUnitQuantityType ContentUnitQuantity {
			get {
				return this.contentUnitQuantityField;
			}
			set {
				this.contentUnitQuantityField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public OrderQuantityIncrementNumericType OrderQuantityIncrementNumeric {
			get {
				return this.orderQuantityIncrementNumericField;
			}
			set {
				this.orderQuantityIncrementNumericField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public MinimumOrderQuantityType MinimumOrderQuantity {
			get {
				return this.minimumOrderQuantityField;
			}
			set {
				this.minimumOrderQuantityField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public MaximumOrderQuantityType MaximumOrderQuantity {
			get {
				return this.maximumOrderQuantityField;
			}
			set {
				this.maximumOrderQuantityField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("WarrantyInformation", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public WarrantyInformationType[] WarrantyInformation {
			get {
				return this.warrantyInformationField;
			}
			set {
				this.warrantyInformationField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public PackLevelCodeType PackLevelCode {
			get {
				return this.packLevelCodeField;
			}
			set {
				this.packLevelCodeField = value;
			}
		}
        
		/// <comentarios/>
		public CustomerPartyType ContractorCustomerParty {
			get {
				return this.contractorCustomerPartyField;
			}
			set {
				this.contractorCustomerPartyField = value;
			}
		}
        
		/// <comentarios/>
		public SupplierPartyType SellerSupplierParty {
			get {
				return this.sellerSupplierPartyField;
			}
			set {
				this.sellerSupplierPartyField = value;
			}
		}
        
		/// <comentarios/>
		public PartyType WarrantyParty {
			get {
				return this.warrantyPartyField;
			}
			set {
				this.warrantyPartyField = value;
			}
		}
        
		/// <comentarios/>
		public PeriodType WarrantyValidityPeriod {
			get {
				return this.warrantyValidityPeriodField;
			}
			set {
				this.warrantyValidityPeriodField = value;
			}
		}
        
		/// <comentarios/>
		public PeriodType LineValidityPeriod {
			get {
				return this.lineValidityPeriodField;
			}
			set {
				this.lineValidityPeriodField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("ItemComparison")]
		public ItemComparisonType[] ItemComparison {
			get {
				return this.itemComparisonField;
			}
			set {
				this.itemComparisonField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("ComponentRelatedItem")]
		public RelatedItemType[] ComponentRelatedItem {
			get {
				return this.componentRelatedItemField;
			}
			set {
				this.componentRelatedItemField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("AccessoryRelatedItem")]
		public RelatedItemType[] AccessoryRelatedItem {
			get {
				return this.accessoryRelatedItemField;
			}
			set {
				this.accessoryRelatedItemField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("RequiredRelatedItem")]
		public RelatedItemType[] RequiredRelatedItem {
			get {
				return this.requiredRelatedItemField;
			}
			set {
				this.requiredRelatedItemField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("ReplacementRelatedItem")]
		public RelatedItemType[] ReplacementRelatedItem {
			get {
				return this.replacementRelatedItemField;
			}
			set {
				this.replacementRelatedItemField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("ComplementaryRelatedItem")]
		public RelatedItemType[] ComplementaryRelatedItem {
			get {
				return this.complementaryRelatedItemField;
			}
			set {
				this.complementaryRelatedItemField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("RequiredItemLocationQuantity")]
		public ItemLocationQuantityType[] RequiredItemLocationQuantity {
			get {
				return this.requiredItemLocationQuantityField;
			}
			set {
				this.requiredItemLocationQuantityField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("DocumentReference")]
		public DocumentReferenceType[] DocumentReference {
			get {
				return this.documentReferenceField;
			}
			set {
				this.documentReferenceField = value;
			}
		}
        
		/// <comentarios/>
		public ItemType Item {
			get {
				return this.itemField;
			}
			set {
				this.itemField = value;
			}
		}
	}
}