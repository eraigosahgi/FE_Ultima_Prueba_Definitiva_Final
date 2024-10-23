/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="dian:gov:co:facturaelectronica:NominaIndividualDeAjuste")]
public partial class NominaIndividualDeAjusteTypeEliminarEliminandoPredecesor {
    
	private string numeroPredField;
    
	private string cUNEPredField;
    
	private System.DateTime fechaGenPredField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public string NumeroPred {
		get {
			return this.numeroPredField;
		}
		set {
			this.numeroPredField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public string CUNEPred {
		get {
			return this.cUNEPredField;
		}
		set {
			this.cUNEPredField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="date")]
	public System.DateTime FechaGenPred {
		get {
			return this.fechaGenPredField;
		}
		set {
			this.fechaGenPredField = value;
		}
	}
}