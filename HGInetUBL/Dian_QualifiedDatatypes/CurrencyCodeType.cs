using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TaxCurrencyCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TargetCurrencyCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(SourceCurrencyCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(RequestedInvoiceCurrencyCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PricingCurrencyCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PaymentCurrencyCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PaymentAlternativeCurrencyCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(DocumentCurrencyCodeType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(CurrencyCodeType1))]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:QualifiedDatatypes-2")]
	public partial class CurrencyCodeType : CodeType {
	}
}