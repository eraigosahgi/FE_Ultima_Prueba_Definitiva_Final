using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("PricingReference", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
	public partial class PricingReferenceType {
        
		private ItemLocationQuantityType originalItemLocationQuantityField;
        
		private PriceType[] alternativeConditionPriceField;
        
		/// <comentarios/>
		public ItemLocationQuantityType OriginalItemLocationQuantity {
			get {
				return this.originalItemLocationQuantityField;
			}
			set {
				this.originalItemLocationQuantityField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("AlternativeConditionPrice")]
		public PriceType[] AlternativeConditionPrice {
			get {
				return this.alternativeConditionPriceField;
			}
			set {
				this.alternativeConditionPriceField = value;
			}
		}
	}
}