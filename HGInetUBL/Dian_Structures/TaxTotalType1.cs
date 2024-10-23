using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName="TaxTotalType", Namespace="http://www.dian.gov.co/contratos/facturaelectronica/v1")]
	[System.Xml.Serialization.XmlRootAttribute("TaxTotal", Namespace="http://www.dian.gov.co/contratos/facturaelectronica/v1", IsNullable=false)]
	public partial class TaxTotalType1 {
        
		private TaxAmountType taxAmountField;
        
		private RoundingAmountType roundingAmountField;
        
		private TaxEvidenceIndicatorType taxEvidenceIndicatorField;
        
		private TaxSubtotalType1[] taxSubtotalField;
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public TaxAmountType TaxAmount {
			get {
				return this.taxAmountField;
			}
			set {
				this.taxAmountField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public RoundingAmountType RoundingAmount {
			get {
				return this.roundingAmountField;
			}
			set {
				this.roundingAmountField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public TaxEvidenceIndicatorType TaxEvidenceIndicator {
			get {
				return this.taxEvidenceIndicatorField;
			}
			set {
				this.taxEvidenceIndicatorField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("TaxSubtotal")]
		public TaxSubtotalType1[] TaxSubtotal {
			get {
				return this.taxSubtotalField;
			}
			set {
				this.taxSubtotalField = value;
			}
		}
	}
}