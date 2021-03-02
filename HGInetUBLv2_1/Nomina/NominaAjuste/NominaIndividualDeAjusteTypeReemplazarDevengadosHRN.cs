/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="dian:gov:co:facturaelectronica:NominaIndividualDeAjuste")]
public partial class NominaIndividualDeAjusteTypeReemplazarDevengadosHRN {
    
	private System.DateTime horaInicioField;
    
	private bool horaInicioFieldSpecified;
    
	private System.DateTime horaFinField;
    
	private bool horaFinFieldSpecified;
    
	private decimal cantidadField;
    
	private decimal porcentajeField;
    
	private decimal pagoField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public System.DateTime HoraInicio {
		get {
			return this.horaInicioField;
		}
		set {
			this.horaInicioField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlIgnoreAttribute()]
	public bool HoraInicioSpecified {
		get {
			return this.horaInicioFieldSpecified;
		}
		set {
			this.horaInicioFieldSpecified = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public System.DateTime HoraFin {
		get {
			return this.horaFinField;
		}
		set {
			this.horaFinField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlIgnoreAttribute()]
	public bool HoraFinSpecified {
		get {
			return this.horaFinFieldSpecified;
		}
		set {
			this.horaFinFieldSpecified = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public decimal Cantidad {
		get {
			return this.cantidadField;
		}
		set {
			this.cantidadField = value;
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
	public decimal Pago {
		get {
			return this.pagoField;
		}
		set {
			this.pagoField = value;
		}
	}
}