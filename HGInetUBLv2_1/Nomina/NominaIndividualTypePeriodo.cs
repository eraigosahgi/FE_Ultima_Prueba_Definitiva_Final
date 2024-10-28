/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="dian:gov:co:facturaelectronica:NominaIndividual")]
public partial class NominaIndividualTypePeriodo {
    
	private System.DateTime fechaIngresoField;
    
	private System.DateTime fechaRetiroField;
    
	private bool fechaRetiroFieldSpecified;
    
	private System.DateTime fechaLiquidacionInicioField;
    
	private System.DateTime fechaLiquidacionFinField;
    
	private string tiempoLaboradoField;
    
	private System.DateTime fechaGenField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="date")]
	public System.DateTime FechaIngreso {
		get {
			return this.fechaIngresoField;
		}
		set {
			this.fechaIngresoField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="date")]
	public System.DateTime FechaRetiro {
		get {
			return this.fechaRetiroField;
		}
		set {
			this.fechaRetiroField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlIgnoreAttribute()]
	public bool FechaRetiroSpecified {
		get {
			return this.fechaRetiroFieldSpecified;
		}
		set {
			this.fechaRetiroFieldSpecified = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="date")]
	public System.DateTime FechaLiquidacionInicio {
		get {
			return this.fechaLiquidacionInicioField;
		}
		set {
			this.fechaLiquidacionInicioField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="date")]
	public System.DateTime FechaLiquidacionFin {
		get {
			return this.fechaLiquidacionFinField;
		}
		set {
			this.fechaLiquidacionFinField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public string TiempoLaborado {
		get {
			return this.tiempoLaboradoField;
		}
		set {
			this.tiempoLaboradoField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="date")]
	public System.DateTime FechaGen {
		get {
			return this.fechaGenField;
		}
		set {
			this.fechaGenField = value;
		}
	}
}