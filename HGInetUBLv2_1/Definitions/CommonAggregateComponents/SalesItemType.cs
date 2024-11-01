﻿/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("SalesItem", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class SalesItemType {
    
	private QuantityType2 quantityField;
    
	private ActivityPropertyType[] activityPropertyField;
    
	private PriceType[] taxExclusivePriceField;
    
	private PriceType[] taxInclusivePriceField;
    
	private ItemType itemField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public QuantityType2 Quantity {
		get {
			return this.quantityField;
		}
		set {
			this.quantityField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("ActivityProperty")]
	public ActivityPropertyType[] ActivityProperty {
		get {
			return this.activityPropertyField;
		}
		set {
			this.activityPropertyField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("TaxExclusivePrice")]
	public PriceType[] TaxExclusivePrice {
		get {
			return this.taxExclusivePriceField;
		}
		set {
			this.taxExclusivePriceField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("TaxInclusivePrice")]
	public PriceType[] TaxInclusivePrice {
		get {
			return this.taxInclusivePriceField;
		}
		set {
			this.taxInclusivePriceField = value;
		}
	}
    
	/// <comentarios/>
	public ItemType Item {
		get {
			return this.itemField;
		}
		set {
			this.itemField = value;
		}
	}
}