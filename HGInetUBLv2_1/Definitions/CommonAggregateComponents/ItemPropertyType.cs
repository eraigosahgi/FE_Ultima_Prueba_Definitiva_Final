﻿/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("AdditionalItemProperty", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class ItemPropertyType {
    
	private IDType idField;
    
	private NameType1 nameField;
    
	private NameCodeType nameCodeField;
    
	private TestMethodType testMethodField;
    
	private ValueType valueField;
    
	private ValueQuantityType valueQuantityField;
    
	private ValueQualifierType[] valueQualifierField;
    
	private ImportanceCodeType importanceCodeField;
    
	private ListValueType[] listValueField;
    
	private PeriodType usabilityPeriodField;
    
	private ItemPropertyGroupType[] itemPropertyGroupField;
    
	private DimensionType rangeDimensionField;
    
	private ItemPropertyRangeType itemPropertyRangeField;
    
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
	public NameType1 Name {
		get {
			return this.nameField;
		}
		set {
			this.nameField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public NameCodeType NameCode {
		get {
			return this.nameCodeField;
		}
		set {
			this.nameCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public TestMethodType TestMethod {
		get {
			return this.testMethodField;
		}
		set {
			this.testMethodField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ValueType Value {
		get {
			return this.valueField;
		}
		set {
			this.valueField = value;
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
	[System.Xml.Serialization.XmlElementAttribute("ValueQualifier", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ValueQualifierType[] ValueQualifier {
		get {
			return this.valueQualifierField;
		}
		set {
			this.valueQualifierField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ImportanceCodeType ImportanceCode {
		get {
			return this.importanceCodeField;
		}
		set {
			this.importanceCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("ListValue", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ListValueType[] ListValue {
		get {
			return this.listValueField;
		}
		set {
			this.listValueField = value;
		}
	}
    
	/// <comentarios/>
	public PeriodType UsabilityPeriod {
		get {
			return this.usabilityPeriodField;
		}
		set {
			this.usabilityPeriodField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("ItemPropertyGroup")]
	public ItemPropertyGroupType[] ItemPropertyGroup {
		get {
			return this.itemPropertyGroupField;
		}
		set {
			this.itemPropertyGroupField = value;
		}
	}
    
	/// <comentarios/>
	public DimensionType RangeDimension {
		get {
			return this.rangeDimensionField;
		}
		set {
			this.rangeDimensionField = value;
		}
	}
    
	/// <comentarios/>
	public ItemPropertyRangeType ItemPropertyRange {
		get {
			return this.itemPropertyRangeField;
		}
		set {
			this.itemPropertyRangeField = value;
		}
	}
}