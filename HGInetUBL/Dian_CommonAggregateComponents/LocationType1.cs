using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName="LocationType", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("DeliveryLocation", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
	public partial class LocationType1 {
        
		private IDType idField;
        
		private DescriptionType descriptionField;
        
		private ConditionsType conditionsField;
        
		private CountrySubentityType countrySubentityField;
        
		private CountrySubentityCodeType countrySubentityCodeField;
        
		private PeriodType[] validityPeriodField;
        
		private AddressType addressField;
        
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
		public DescriptionType Description {
			get {
				return this.descriptionField;
			}
			set {
				this.descriptionField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public ConditionsType Conditions {
			get {
				return this.conditionsField;
			}
			set {
				this.conditionsField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public CountrySubentityType CountrySubentity {
			get {
				return this.countrySubentityField;
			}
			set {
				this.countrySubentityField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public CountrySubentityCodeType CountrySubentityCode {
			get {
				return this.countrySubentityCodeField;
			}
			set {
				this.countrySubentityCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("ValidityPeriod")]
		public PeriodType[] ValidityPeriod {
			get {
				return this.validityPeriodField;
			}
			set {
				this.validityPeriodField = value;
			}
		}
        
		/// <comentarios/>
		public AddressType Address {
			get {
				return this.addressField;
			}
			set {
				this.addressField = value;
			}
		}
	}
}