using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("CommodityClassification", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
	public partial class CommodityClassificationType {
        
		private NatureCodeType natureCodeField;
        
		private CargoTypeCodeType cargoTypeCodeField;
        
		private CommodityCodeType commodityCodeField;
        
		private ItemClassificationCodeType itemClassificationCodeField;
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public NatureCodeType NatureCode {
			get {
				return this.natureCodeField;
			}
			set {
				this.natureCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public CargoTypeCodeType CargoTypeCode {
			get {
				return this.cargoTypeCodeField;
			}
			set {
				this.cargoTypeCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public CommodityCodeType CommodityCode {
			get {
				return this.commodityCodeField;
			}
			set {
				this.commodityCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public ItemClassificationCodeType ItemClassificationCode {
			get {
				return this.itemClassificationCodeField;
			}
			set {
				this.itemClassificationCodeField = value;
			}
		}
	}
}