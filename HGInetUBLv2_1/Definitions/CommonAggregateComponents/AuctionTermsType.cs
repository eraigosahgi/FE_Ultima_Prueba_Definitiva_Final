﻿/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("AuctionTerms", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class AuctionTermsType {
    
	private AuctionConstraintIndicatorType auctionConstraintIndicatorField;
    
	private JustificationDescriptionType[] justificationDescriptionField;
    
	private DescriptionType[] descriptionField;
    
	private ProcessDescriptionType[] processDescriptionField;
    
	private ConditionsDescriptionType[] conditionsDescriptionField;
    
	private ElectronicDeviceDescriptionType[] electronicDeviceDescriptionField;
    
	private AuctionURIType auctionURIField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public AuctionConstraintIndicatorType AuctionConstraintIndicator {
		get {
			return this.auctionConstraintIndicatorField;
		}
		set {
			this.auctionConstraintIndicatorField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("JustificationDescription", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public JustificationDescriptionType[] JustificationDescription {
		get {
			return this.justificationDescriptionField;
		}
		set {
			this.justificationDescriptionField = value;
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
	[System.Xml.Serialization.XmlElementAttribute("ProcessDescription", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ProcessDescriptionType[] ProcessDescription {
		get {
			return this.processDescriptionField;
		}
		set {
			this.processDescriptionField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("ConditionsDescription", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ConditionsDescriptionType[] ConditionsDescription {
		get {
			return this.conditionsDescriptionField;
		}
		set {
			this.conditionsDescriptionField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("ElectronicDeviceDescription", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ElectronicDeviceDescriptionType[] ElectronicDeviceDescription {
		get {
			return this.electronicDeviceDescriptionField;
		}
		set {
			this.electronicDeviceDescriptionField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public AuctionURIType AuctionURI {
		get {
			return this.auctionURIField;
		}
		set {
			this.auctionURIField = value;
		}
	}
}