/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="dian:gov:co:facturaelectronica:NominaIndividualDeAjuste")]
public partial class NominaIndividualDeAjusteTypeReemplazarDevengadosBonificacion {
    
	private decimal bonificacionSField;
    
	private bool bonificacionSFieldSpecified;
    
	private decimal bonificacionNSField;
    
	private bool bonificacionNSFieldSpecified;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public decimal BonificacionS {
		get {
			return this.bonificacionSField;
		}
		set {
			this.bonificacionSField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlIgnoreAttribute()]
	public bool BonificacionSSpecified {
		get {
			return this.bonificacionSFieldSpecified;
		}
		set {
			this.bonificacionSFieldSpecified = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public decimal BonificacionNS {
		get {
			return this.bonificacionNSField;
		}
		set {
			this.bonificacionNSField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlIgnoreAttribute()]
	public bool BonificacionNSSpecified {
		get {
			return this.bonificacionNSFieldSpecified;
		}
		set {
			this.bonificacionNSFieldSpecified = value;
		}
	}
}