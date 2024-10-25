/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="dian:gov:co:facturaelectronica:Structures-2-1")]
public partial class AdditionalMonetaryTotal {
    
	private AmountType1 repercussionsTotalTaxAmountField;
    
	private AmountType1 retainTotalTaxAmountField;
    
	private AmountType1 invoiceTotalLocalCurrencyAmountField;
    
	/// <comentarios/>
	public AmountType1 RepercussionsTotalTaxAmount {
		get {
			return this.repercussionsTotalTaxAmountField;
		}
		set {
			this.repercussionsTotalTaxAmountField = value;
		}
	}
    
	/// <comentarios/>
	public AmountType1 RetainTotalTaxAmount {
		get {
			return this.retainTotalTaxAmountField;
		}
		set {
			this.retainTotalTaxAmountField = value;
		}
	}
    
	/// <comentarios/>
	public AmountType1 InvoiceTotalLocalCurrencyAmount {
		get {
			return this.invoiceTotalLocalCurrencyAmountField;
		}
		set {
			this.invoiceTotalLocalCurrencyAmountField = value;
		}
	}
}