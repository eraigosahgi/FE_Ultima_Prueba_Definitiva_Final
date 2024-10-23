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
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "CustomerPartyType", Namespace = "http://www.dian.gov.co/contratos/facturaelectronica/v1")]
	[System.Xml.Serialization.XmlRootAttribute("AccountingCustomerParty", Namespace = "http://www.dian.gov.co/contratos/facturaelectronica/v1", IsNullable = false)]
	public partial class CustomerPartyType1
	{

		private CustomerAssignedAccountIDType customerAssignedAccountIDField;

		private SupplierAssignedAccountIDType supplierAssignedAccountIDField;

		private AdditionalAccountIDType additionalAccountIDField;

		private PartyType1 partyField;

		private ContactType deliveryContactField;

		private ContactType accountingContactField;

		private ContactType buyerContactField;

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public CustomerAssignedAccountIDType CustomerAssignedAccountID
		{
			get
			{
				return this.customerAssignedAccountIDField;
			}
			set
			{
				this.customerAssignedAccountIDField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public SupplierAssignedAccountIDType SupplierAssignedAccountID
		{
			get
			{
				return this.supplierAssignedAccountIDField;
			}
			set
			{
				this.supplierAssignedAccountIDField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public AdditionalAccountIDType AdditionalAccountID
		{
			get
			{
				return this.additionalAccountIDField;
			}
			set
			{
				this.additionalAccountIDField = value;
			}
		}

		/// <comentarios/>
		public PartyType1 Party
		{
			get
			{
				return this.partyField;
			}
			set
			{
				this.partyField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public ContactType DeliveryContact
		{
			get
			{
				return this.deliveryContactField;
			}
			set
			{
				this.deliveryContactField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public ContactType AccountingContact
		{
			get
			{
				return this.accountingContactField;
			}
			set
			{
				this.accountingContactField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public ContactType BuyerContact
		{
			get
			{
				return this.buyerContactField;
			}
			set
			{
				this.buyerContactField = value;
			}
		}
	}
}