using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("ItemComparison", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
	public partial class ItemComparisonType {
        
		private PriceAmountType priceAmountField;
        
		private QuantityType1 quantityField;
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public PriceAmountType PriceAmount {
			get {
				return this.priceAmountField;
			}
			set {
				this.priceAmountField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public QuantityType1 Quantity {
			get {
				return this.quantityField;
			}
			set {
				this.quantityField = value;
			}
		}
	}
}