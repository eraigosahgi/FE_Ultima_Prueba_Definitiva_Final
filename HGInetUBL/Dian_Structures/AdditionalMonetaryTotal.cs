using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.dian.gov.co/contratos/facturaelectronica/v1/Structures")]
	public partial class AdditionalMonetaryTotal {
        
		private AmountType repercussionsTotalTaxAmountField;
        
		private AmountType retainTotalTaxAmountField;
        
		private AmountType invoiceTotalLocalCurrencyAmountField;
        
		/// <comentarios/>
		public AmountType RepercussionsTotalTaxAmount {
			get {
				return this.repercussionsTotalTaxAmountField;
			}
			set {
				this.repercussionsTotalTaxAmountField = value;
			}
		}
        
		/// <comentarios/>
		public AmountType RetainTotalTaxAmount {
			get {
				return this.retainTotalTaxAmountField;
			}
			set {
				this.retainTotalTaxAmountField = value;
			}
		}
        
		/// <comentarios/>
		public AmountType InvoiceTotalLocalCurrencyAmount {
			get {
				return this.invoiceTotalLocalCurrencyAmountField;
			}
			set {
				this.invoiceTotalLocalCurrencyAmountField = value;
			}
		}
	}
}