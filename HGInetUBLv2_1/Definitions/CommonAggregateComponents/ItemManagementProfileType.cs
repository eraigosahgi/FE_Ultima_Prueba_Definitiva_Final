/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("ItemManagementProfile", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class ItemManagementProfileType {
    
	private FrozenPeriodDaysNumericType frozenPeriodDaysNumericField;
    
	private MinimumInventoryQuantityType minimumInventoryQuantityField;
    
	private MultipleOrderQuantityType multipleOrderQuantityField;
    
	private OrderIntervalDaysNumericType orderIntervalDaysNumericField;
    
	private ReplenishmentOwnerDescriptionType[] replenishmentOwnerDescriptionField;
    
	private TargetServicePercentType targetServicePercentField;
    
	private TargetInventoryQuantityType targetInventoryQuantityField;
    
	private PeriodType effectivePeriodField;
    
	private ItemType itemField;
    
	private ItemLocationQuantityType itemLocationQuantityField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public FrozenPeriodDaysNumericType FrozenPeriodDaysNumeric {
		get {
			return this.frozenPeriodDaysNumericField;
		}
		set {
			this.frozenPeriodDaysNumericField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public MinimumInventoryQuantityType MinimumInventoryQuantity {
		get {
			return this.minimumInventoryQuantityField;
		}
		set {
			this.minimumInventoryQuantityField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public MultipleOrderQuantityType MultipleOrderQuantity {
		get {
			return this.multipleOrderQuantityField;
		}
		set {
			this.multipleOrderQuantityField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public OrderIntervalDaysNumericType OrderIntervalDaysNumeric {
		get {
			return this.orderIntervalDaysNumericField;
		}
		set {
			this.orderIntervalDaysNumericField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("ReplenishmentOwnerDescription", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ReplenishmentOwnerDescriptionType[] ReplenishmentOwnerDescription {
		get {
			return this.replenishmentOwnerDescriptionField;
		}
		set {
			this.replenishmentOwnerDescriptionField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public TargetServicePercentType TargetServicePercent {
		get {
			return this.targetServicePercentField;
		}
		set {
			this.targetServicePercentField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public TargetInventoryQuantityType TargetInventoryQuantity {
		get {
			return this.targetInventoryQuantityField;
		}
		set {
			this.targetInventoryQuantityField = value;
		}
	}
    
	/// <comentarios/>
	public PeriodType EffectivePeriod {
		get {
			return this.effectivePeriodField;
		}
		set {
			this.effectivePeriodField = value;
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
    
	/// <comentarios/>
	public ItemLocationQuantityType ItemLocationQuantity {
		get {
			return this.itemLocationQuantityField;
		}
		set {
			this.itemLocationQuantityField = value;
		}
	}
}