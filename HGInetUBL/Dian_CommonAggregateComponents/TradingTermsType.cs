using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("TradingTerms", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
	public partial class TradingTermsType {
        
		private InformationType[] informationField;
        
		private ReferenceType referenceField;
        
		private AddressType applicableAddressField;
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("Information", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public InformationType[] Information {
			get {
				return this.informationField;
			}
			set {
				this.informationField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public ReferenceType Reference {
			get {
				return this.referenceField;
			}
			set {
				this.referenceField = value;
			}
		}
        
		/// <comentarios/>
		public AddressType ApplicableAddress {
			get {
				return this.applicableAddressField;
			}
			set {
				this.applicableAddressField = value;
			}
		}
	}
}