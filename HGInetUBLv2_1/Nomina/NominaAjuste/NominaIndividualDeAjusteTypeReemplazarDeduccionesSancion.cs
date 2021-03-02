/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="dian:gov:co:facturaelectronica:NominaIndividualDeAjuste")]
public partial class NominaIndividualDeAjusteTypeReemplazarDeduccionesSancion {
    
	private decimal sancionPublicField;
    
	private decimal sancionPrivField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public decimal SancionPublic {
		get {
			return this.sancionPublicField;
		}
		set {
			this.sancionPublicField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public decimal SancionPriv {
		get {
			return this.sancionPrivField;
		}
		set {
			this.sancionPrivField = value;
		}
	}
}