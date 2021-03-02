/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="dian:gov:co:facturaelectronica:NominaIndividualDeAjuste")]
public partial class NominaIndividualDeAjusteTypeReemplazarDevengadosLicenciasLicenciaMP {
    
	private System.DateTime fechaInicioField;
    
	private bool fechaInicioFieldSpecified;
    
	private System.DateTime fechaFinField;
    
	private bool fechaFinFieldSpecified;
    
	private string cantidadField;
    
	private decimal pagoField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="date")]
	public System.DateTime FechaInicio {
		get {
			return this.fechaInicioField;
		}
		set {
			this.fechaInicioField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlIgnoreAttribute()]
	public bool FechaInicioSpecified {
		get {
			return this.fechaInicioFieldSpecified;
		}
		set {
			this.fechaInicioFieldSpecified = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="date")]
	public System.DateTime FechaFin {
		get {
			return this.fechaFinField;
		}
		set {
			this.fechaFinField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlIgnoreAttribute()]
	public bool FechaFinSpecified {
		get {
			return this.fechaFinFieldSpecified;
		}
		set {
			this.fechaFinFieldSpecified = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="integer")]
	public string Cantidad {
		get {
			return this.cantidadField;
		}
		set {
			this.cantidadField = value;
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