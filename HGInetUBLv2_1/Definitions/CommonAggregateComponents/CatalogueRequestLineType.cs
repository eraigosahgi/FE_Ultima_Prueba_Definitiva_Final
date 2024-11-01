﻿/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("CatalogueRequestLine", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class CatalogueRequestLineType {
    
	private IDType idField;
    
	private ContractSubdivisionType contractSubdivisionField;
    
	private NoteType[] noteField;
    
	private PeriodType lineValidityPeriodField;
    
	private ItemLocationQuantityType[] requiredItemLocationQuantityField;
    
	private ItemType itemField;
    
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
	public ContractSubdivisionType ContractSubdivision {
		get {
			return this.contractSubdivisionField;
		}
		set {
			this.contractSubdivisionField = value;
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
	public PeriodType LineValidityPeriod {
		get {
			return this.lineValidityPeriodField;
		}
		set {
			this.lineValidityPeriodField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("RequiredItemLocationQuantity")]
	public ItemLocationQuantityType[] RequiredItemLocationQuantity {
		get {
			return this.requiredItemLocationQuantityField;
		}
		set {
			this.requiredItemLocationQuantityField = value;
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