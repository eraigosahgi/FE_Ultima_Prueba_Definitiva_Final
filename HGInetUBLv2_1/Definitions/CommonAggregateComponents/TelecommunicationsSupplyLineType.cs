﻿/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("TelecommunicationsSupplyLine", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class TelecommunicationsSupplyLineType {
    
	private IDType idField;
    
	private PhoneNumberType phoneNumberField;
    
	private DescriptionType[] descriptionField;
    
	private LineExtensionAmountType lineExtensionAmountField;
    
	private ExchangeRateType[] exchangeRateField;
    
	private AllowanceChargeType[] allowanceChargeField;
    
	private TaxTotalType[] taxTotalField;
    
	private TelecommunicationsServiceType[] telecommunicationsServiceField;
    
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
	public PhoneNumberType PhoneNumber {
		get {
			return this.phoneNumberField;
		}
		set {
			this.phoneNumberField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("Description", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public DescriptionType[] Description {
		get {
			return this.descriptionField;
		}
		set {
			this.descriptionField = value;
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
	[System.Xml.Serialization.XmlElementAttribute("ExchangeRate")]
	public ExchangeRateType[] ExchangeRate {
		get {
			return this.exchangeRateField;
		}
		set {
			this.exchangeRateField = value;
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
	[System.Xml.Serialization.XmlElementAttribute("TelecommunicationsService")]
	public TelecommunicationsServiceType[] TelecommunicationsService {
		get {
			return this.telecommunicationsServiceField;
		}
		set {
			this.telecommunicationsServiceField = value;
		}
	}
}