﻿/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("AnticipatedMonetaryTotal", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class MonetaryTotalType {
    
	private LineExtensionAmountType lineExtensionAmountField;
    
	private TaxExclusiveAmountType taxExclusiveAmountField;
    
	private TaxInclusiveAmountType taxInclusiveAmountField;
    
	private AllowanceTotalAmountType allowanceTotalAmountField;
    
	private ChargeTotalAmountType chargeTotalAmountField;
    
	private PrepaidAmountType prepaidAmountField;
    
	private PayableRoundingAmountType payableRoundingAmountField;
    
	private PayableAmountType payableAmountField;
    
	private PayableAlternativeAmountType payableAlternativeAmountField;
    
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
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public TaxExclusiveAmountType TaxExclusiveAmount {
		get {
			return this.taxExclusiveAmountField;
		}
		set {
			this.taxExclusiveAmountField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public TaxInclusiveAmountType TaxInclusiveAmount {
		get {
			return this.taxInclusiveAmountField;
		}
		set {
			this.taxInclusiveAmountField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public AllowanceTotalAmountType AllowanceTotalAmount {
		get {
			return this.allowanceTotalAmountField;
		}
		set {
			this.allowanceTotalAmountField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ChargeTotalAmountType ChargeTotalAmount {
		get {
			return this.chargeTotalAmountField;
		}
		set {
			this.chargeTotalAmountField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public PrepaidAmountType PrepaidAmount {
		get {
			return this.prepaidAmountField;
		}
		set {
			this.prepaidAmountField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public PayableRoundingAmountType PayableRoundingAmount {
		get {
			return this.payableRoundingAmountField;
		}
		set {
			this.payableRoundingAmountField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public PayableAmountType PayableAmount {
		get {
			return this.payableAmountField;
		}
		set {
			this.payableAmountField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public PayableAlternativeAmountType PayableAlternativeAmount {
		get {
			return this.payableAlternativeAmountField;
		}
		set {
			this.payableAlternativeAmountField = value;
		}
	}
}