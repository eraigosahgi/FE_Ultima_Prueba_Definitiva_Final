﻿/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("Declaration", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class DeclarationType {
    
	private NameType1[] nameField;
    
	private DeclarationTypeCodeType declarationTypeCodeField;
    
	private DescriptionType[] descriptionField;
    
	private EvidenceSuppliedType[] evidenceSuppliedField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("Name", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public NameType1[] Name {
		get {
			return this.nameField;
		}
		set {
			this.nameField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public DeclarationTypeCodeType DeclarationTypeCode {
		get {
			return this.declarationTypeCodeField;
		}
		set {
			this.declarationTypeCodeField = value;
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
	[System.Xml.Serialization.XmlElementAttribute("EvidenceSupplied")]
	public EvidenceSuppliedType[] EvidenceSupplied {
		get {
			return this.evidenceSuppliedField;
		}
		set {
			this.evidenceSuppliedField = value;
		}
	}
}