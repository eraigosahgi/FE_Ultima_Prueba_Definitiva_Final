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
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "DespatchType", Namespace = "http://www.dian.gov.co/contratos/facturaelectronica/v1")]
	[System.Xml.Serialization.XmlRootAttribute("Despatch", Namespace = "http://www.dian.gov.co/contratos/facturaelectronica/v1", IsNullable = false)]
	public partial class DespatchType1
	{

		private IDType idField;

		private RequestedDespatchDateType requestedDespatchDateField;

		private RequestedDespatchTimeType requestedDespatchTimeField;

		private EstimatedDespatchDateType estimatedDespatchDateField;

		private EstimatedDespatchTimeType estimatedDespatchTimeField;

		private ActualDespatchDateType actualDespatchDateField;

		private ActualDespatchTimeType actualDespatchTimeField;

		private AddressType1 despatchAddressField;

		private PartyType1 despatchPartyField;

		private ContactType contactField;

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public IDType ID
		{
			get
			{
				return this.idField;
			}
			set
			{
				this.idField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public RequestedDespatchDateType RequestedDespatchDate
		{
			get
			{
				return this.requestedDespatchDateField;
			}
			set
			{
				this.requestedDespatchDateField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public RequestedDespatchTimeType RequestedDespatchTime
		{
			get
			{
				return this.requestedDespatchTimeField;
			}
			set
			{
				this.requestedDespatchTimeField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public EstimatedDespatchDateType EstimatedDespatchDate
		{
			get
			{
				return this.estimatedDespatchDateField;
			}
			set
			{
				this.estimatedDespatchDateField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public EstimatedDespatchTimeType EstimatedDespatchTime
		{
			get
			{
				return this.estimatedDespatchTimeField;
			}
			set
			{
				this.estimatedDespatchTimeField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public ActualDespatchDateType ActualDespatchDate
		{
			get
			{
				return this.actualDespatchDateField;
			}
			set
			{
				this.actualDespatchDateField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public ActualDespatchTimeType ActualDespatchTime
		{
			get
			{
				return this.actualDespatchTimeField;
			}
			set
			{
				this.actualDespatchTimeField = value;
			}
		}

		/// <comentarios/>
		public AddressType1 DespatchAddress
		{
			get
			{
				return this.despatchAddressField;
			}
			set
			{
				this.despatchAddressField = value;
			}
		}

		/// <comentarios/>
		public PartyType1 DespatchParty
		{
			get
			{
				return this.despatchPartyField;
			}
			set
			{
				this.despatchPartyField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public ContactType Contact
		{
			get
			{
				return this.contactField;
			}
			set
			{
				this.contactField = value;
			}
		}
	}
}