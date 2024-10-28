using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("CatalogueItemSpecificationUpdateLine", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
	public partial class CatalogueItemSpecificationUpdateLineType {
        
		private IDType idField;
        
		private CustomerPartyType contractorCustomerPartyField;
        
		private SupplierPartyType sellerSupplierPartyField;
        
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