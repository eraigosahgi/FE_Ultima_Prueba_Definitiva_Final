﻿/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("SubscriberConsumption", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class SubscriberConsumptionType {
    
	private ConsumptionIDType consumptionIDField;
    
	private SpecificationTypeCodeType specificationTypeCodeField;
    
	private NoteType[] noteField;
    
	private TotalMeteredQuantityType totalMeteredQuantityField;
    
	private PartyType subscriberPartyField;
    
	private ConsumptionPointType utilityConsumptionPointField;
    
	private OnAccountPaymentType[] onAccountPaymentField;
    
	private ConsumptionType consumptionField;
    
	private SupplierConsumptionType[] supplierConsumptionField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ConsumptionIDType ConsumptionID {
		get {
			return this.consumptionIDField;
		}
		set {
			this.consumptionIDField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public SpecificationTypeCodeType SpecificationTypeCode {
		get {
			return this.specificationTypeCodeField;
		}
		set {
			this.specificationTypeCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("Note", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public NoteType[] Note {
		get {
			return this.noteField;
		}
		set {
			this.noteField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public TotalMeteredQuantityType TotalMeteredQuantity {
		get {
			return this.totalMeteredQuantityField;
		}
		set {
			this.totalMeteredQuantityField = value;
		}
	}
    
	/// <comentarios/>
	public PartyType SubscriberParty {
		get {
			return this.subscriberPartyField;
		}
		set {
			this.subscriberPartyField = value;
		}
	}
    
	/// <comentarios/>
	public ConsumptionPointType UtilityConsumptionPoint {
		get {
			return this.utilityConsumptionPointField;
		}
		set {
			this.utilityConsumptionPointField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("OnAccountPayment")]
	public OnAccountPaymentType[] OnAccountPayment {
		get {
			return this.onAccountPaymentField;
		}
		set {
			this.onAccountPaymentField = value;
		}
	}
    
	/// <comentarios/>
	public ConsumptionType Consumption {
		get {
			return this.consumptionField;
		}
		set {
			this.consumptionField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("SupplierConsumption")]
	public SupplierConsumptionType[] SupplierConsumption {
		get {
			return this.supplierConsumptionField;
		}
		set {
			this.supplierConsumptionField = value;
		}
	}
}