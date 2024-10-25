/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("BudgetAccount", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class BudgetAccountType {
    
	private IDType idField;
    
	private BudgetYearNumericType budgetYearNumericField;
    
	private ClassificationSchemeType requiredClassificationSchemeField;
    
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
	public BudgetYearNumericType BudgetYearNumeric {
		get {
			return this.budgetYearNumericField;
		}
		set {
			this.budgetYearNumericField = value;
		}
	}
    
	/// <comentarios/>
	public ClassificationSchemeType RequiredClassificationScheme {
		get {
			return this.requiredClassificationSchemeField;
		}
		set {
			this.requiredClassificationSchemeField = value;
		}
	}
}