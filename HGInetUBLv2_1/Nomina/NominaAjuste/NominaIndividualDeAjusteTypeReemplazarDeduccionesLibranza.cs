/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="dian:gov:co:facturaelectronica:NominaIndividualDeAjuste")]
public partial class NominaIndividualDeAjusteTypeReemplazarDeduccionesLibranza {
    
	private string descripcionField;
    
	private decimal deduccionField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public string Descripcion {
		get {
			return this.descripcionField;
		}
		set {
			this.descripcionField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public decimal Deduccion {
		get {
			return this.deduccionField;
		}
		set {
			this.deduccionField = value;
		}
	}
}