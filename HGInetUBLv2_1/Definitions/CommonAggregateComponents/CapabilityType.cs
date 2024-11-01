﻿/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("Capability", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class CapabilityType {
    
	private CapabilityTypeCodeType capabilityTypeCodeField;
    
	private DescriptionType[] descriptionField;
    
	private ValueAmountType valueAmountField;
    
	private ValueQuantityType valueQuantityField;
    
	private EvidenceSuppliedType[] evidenceSuppliedField;
    
	private PeriodType validityPeriodField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public CapabilityTypeCodeType CapabilityTypeCode {
		get {
			return this.capabilityTypeCodeField;
		}
		set {
			this.capabilityTypeCodeField = value;
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
	public ValueAmountType ValueAmount {
		get {
			return this.valueAmountField;
		}
		set {
			this.valueAmountField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ValueQuantityType ValueQuantity {
		get {
			return this.valueQuantityField;
		}
		set {
			this.valueQuantityField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("EvidenceSupplied")]
	public EvidenceSuppliedType[] EvidenceSupplied {
		get {
			return this.evidenceSuppliedField;
		}
		set {
			this.evidenceSuppliedField = value;
		}
	}
    
	/// <comentarios/>
	public PeriodType ValidityPeriod {
		get {
			return this.validityPeriodField;
		}
		set {
			this.validityPeriodField = value;
		}
	}
}