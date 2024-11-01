﻿/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("ConsumptionLine", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class ConsumptionLineType {
    
	private IDType idField;
    
	private ParentDocumentLineReferenceIDType parentDocumentLineReferenceIDField;
    
	private InvoicedQuantityType invoicedQuantityField;
    
	private LineExtensionAmountType lineExtensionAmountField;
    
	private PeriodType periodField;
    
	private DeliveryType[] deliveryField;
    
	private AllowanceChargeType[] allowanceChargeField;
    
	private TaxTotalType[] taxTotalField;
    
	private UtilityItemType utilityItemField;
    
	private PriceType priceField;
    
	private UnstructuredPriceType unstructuredPriceField;
    
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
	public ParentDocumentLineReferenceIDType ParentDocumentLineReferenceID {
		get {
			return this.parentDocumentLineReferenceIDField;
		}
		set {
			this.parentDocumentLineReferenceIDField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public InvoicedQuantityType InvoicedQuantity {
		get {
			return this.invoicedQuantityField;
		}
		set {
			this.invoicedQuantityField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public LineExtensionAmountType LineExtensionAmount {
		get {
			return this.lineExtensionAmountField;
		}
		set {
			this.lineExtensionAmountField = value;
		}
	}
    
	/// <comentarios/>
	public PeriodType Period {
		get {
			return this.periodField;
		}
		set {
			this.periodField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("Delivery")]
	public DeliveryType[] Delivery {
		get {
			return this.deliveryField;
		}
		set {
			this.deliveryField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("AllowanceCharge")]
	public AllowanceChargeType[] AllowanceCharge {
		get {
			return this.allowanceChargeField;
		}
		set {
			this.allowanceChargeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("TaxTotal")]
	public TaxTotalType[] TaxTotal {
		get {
			return this.taxTotalField;
		}
		set {
			this.taxTotalField = value;
		}
	}
    
	/// <comentarios/>
	public UtilityItemType UtilityItem {
		get {
			return this.utilityItemField;
		}
		set {
			this.utilityItemField = value;
		}
	}
    
	/// <comentarios/>
	public PriceType Price {
		get {
			return this.priceField;
		}
		set {
			this.priceField = value;
		}
	}
    
	/// <comentarios/>
	public UnstructuredPriceType UnstructuredPrice {
		get {
			return this.unstructuredPriceField;
		}
		set {
			this.unstructuredPriceField = value;
		}
	}
}