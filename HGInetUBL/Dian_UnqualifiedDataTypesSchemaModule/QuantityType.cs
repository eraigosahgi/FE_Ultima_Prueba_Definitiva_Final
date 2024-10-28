using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(UnitQuantityType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TransportHandlingUnitQuantityType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TotalTransportHandlingUnitQuantityType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TotalPackagesQuantityType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TotalPackageQuantityType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TotalGoodsItemQuantityType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ShortQuantityType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(RejectedQuantityType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ReceivedQuantityType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(QuantityType1))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PackQuantityType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PackagesQuantityType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PackageQuantityType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(OversupplyQuantityType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(OutstandingQuantityType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(OrderQuantityType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(MinimumQuantityType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(MinimumOrderQuantityType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(MinimumBackorderQuantityType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(MaximumQuantityType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(MaximumOrderQuantityType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(MaximumBackorderQuantityType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(InvoicedQuantityType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(GoodsItemQuantityType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(DeliveredQuantityType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(DebitedQuantityType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(CustomsTariffQuantityType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(CreditedQuantityType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ContentUnitQuantityType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ConsumerUnitQuantityType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(BatchQuantityType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(BaseQuantityType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(BackorderQuantityType))]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2")]
	public partial class QuantityType {
        
		private UnitCodeContentType unitCodeField;
        
		private bool unitCodeFieldSpecified;
        
		private decimal valueField;
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public UnitCodeContentType unitCode {
			get {
				return this.unitCodeField;
			}
			set {
				this.unitCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool unitCodeSpecified {
			get {
				return this.unitCodeFieldSpecified;
			}
			set {
				this.unitCodeFieldSpecified = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlTextAttribute()]
		public decimal Value {
			get {
				return this.valueField;
			}
			set {
				this.valueField = value;
			}
		}
	}
}