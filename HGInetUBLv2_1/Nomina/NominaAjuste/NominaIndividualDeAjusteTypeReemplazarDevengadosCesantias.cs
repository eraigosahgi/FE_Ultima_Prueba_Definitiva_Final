/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="dian:gov:co:facturaelectronica:NominaIndividualDeAjuste")]
public partial class NominaIndividualDeAjusteTypeReemplazarDevengadosCesantias {
    
	private decimal pagoField;
    
	private decimal porcentajeField;
    
	private decimal pagoInteresesField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public decimal Pago {
		get {
			return this.pagoField;
		}
		set {
			this.pagoField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public decimal Porcentaje {
		get {
			return this.porcentajeField;
		}
		set {
			this.porcentajeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public decimal PagoIntereses {
		get {
			return this.pagoInteresesField;
		}
		set {
			this.pagoInteresesField = value;
		}
	}
}