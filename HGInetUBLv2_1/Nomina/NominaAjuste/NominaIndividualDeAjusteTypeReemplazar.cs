/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="dian:gov:co:facturaelectronica:NominaIndividualDeAjuste")]
public partial class NominaIndividualDeAjusteTypeReemplazar {
    
	private NominaIndividualDeAjusteTypeReemplazarReemplazandoPredecesor reemplazandoPredecesorField;
    
	private NominaIndividualDeAjusteTypeReemplazarPeriodo periodoField;
    
	private NominaIndividualDeAjusteTypeReemplazarNumeroSecuenciaXML numeroSecuenciaXMLField;
    
	private NominaIndividualDeAjusteTypeReemplazarLugarGeneracionXML lugarGeneracionXMLField;
    
	private NominaIndividualDeAjusteTypeReemplazarProveedorXML proveedorXMLField;
    
	private string codigoQRField;
    
	private NominaIndividualDeAjusteTypeReemplazarInformacionGeneral informacionGeneralField;
    
	private string[] notasField;
    
	private NominaIndividualDeAjusteTypeReemplazarEmpleador empleadorField;
    
	private NominaIndividualDeAjusteTypeReemplazarTrabajador trabajadorField;
    
	private NominaIndividualDeAjusteTypeReemplazarPago pagoField;
    
	private System.DateTime[] fechasPagosField;
    
	private NominaIndividualDeAjusteTypeReemplazarDevengados devengadosField;
    
	private NominaIndividualDeAjusteTypeReemplazarDeducciones deduccionesField;
    
	private decimal redondeoField;
    
	private bool redondeoFieldSpecified;
    
	private decimal devengadosTotalField;
    
	private decimal deduccionesTotalField;
    
	private decimal comprobanteTotalField;
    
	/// <comentarios/>
	public NominaIndividualDeAjusteTypeReemplazarReemplazandoPredecesor ReemplazandoPredecesor {
		get {
			return this.reemplazandoPredecesorField;
		}
		set {
			this.reemplazandoPredecesorField = value;
		}
	}
    
	/// <comentarios/>
	public NominaIndividualDeAjusteTypeReemplazarPeriodo Periodo {
		get {
			return this.periodoField;
		}
		set {
			this.periodoField = value;
		}
	}
    
	/// <comentarios/>
	public NominaIndividualDeAjusteTypeReemplazarNumeroSecuenciaXML NumeroSecuenciaXML {
		get {
			return this.numeroSecuenciaXMLField;
		}
		set {
			this.numeroSecuenciaXMLField = value;
		}
	}
    
	/// <comentarios/>
	public NominaIndividualDeAjusteTypeReemplazarLugarGeneracionXML LugarGeneracionXML {
		get {
			return this.lugarGeneracionXMLField;
		}
		set {
			this.lugarGeneracionXMLField = value;
		}
	}
    
	/// <comentarios/>
	public NominaIndividualDeAjusteTypeReemplazarProveedorXML ProveedorXML {
		get {
			return this.proveedorXMLField;
		}
		set {
			this.proveedorXMLField = value;
		}
	}
    
	/// <comentarios/>
	public string CodigoQR {
		get {
			return this.codigoQRField;
		}
		set {
			this.codigoQRField = value;
		}
	}
    
	/// <comentarios/>
	public NominaIndividualDeAjusteTypeReemplazarInformacionGeneral InformacionGeneral {
		get {
			return this.informacionGeneralField;
		}
		set {
			this.informacionGeneralField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("Notas")]
	public string[] Notas {
		get {
			return this.notasField;
		}
		set {
			this.notasField = value;
		}
	}
    
	/// <comentarios/>
	public NominaIndividualDeAjusteTypeReemplazarEmpleador Empleador {
		get {
			return this.empleadorField;
		}
		set {
			this.empleadorField = value;
		}
	}
    
	/// <comentarios/>
	public NominaIndividualDeAjusteTypeReemplazarTrabajador Trabajador {
		get {
			return this.trabajadorField;
		}
		set {
			this.trabajadorField = value;
		}
	}
    
	/// <comentarios/>
	public NominaIndividualDeAjusteTypeReemplazarPago Pago {
		get {
			return this.pagoField;
		}
		set {
			this.pagoField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlArrayItemAttribute("FechaPago", DataType="date", IsNullable=false)]
	public System.DateTime[] FechasPagos {
		get {
			return this.fechasPagosField;
		}
		set {
			this.fechasPagosField = value;
		}
	}
    
	/// <comentarios/>
	public NominaIndividualDeAjusteTypeReemplazarDevengados Devengados {
		get {
			return this.devengadosField;
		}
		set {
			this.devengadosField = value;
		}
	}
    
	/// <comentarios/>
	public NominaIndividualDeAjusteTypeReemplazarDeducciones Deducciones {
		get {
			return this.deduccionesField;
		}
		set {
			this.deduccionesField = value;
		}
	}
    
	/// <comentarios/>
	public decimal Redondeo {
		get {
			return this.redondeoField;
		}
		set {
			this.redondeoField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlIgnoreAttribute()]
	public bool RedondeoSpecified {
		get {
			return this.redondeoFieldSpecified;
		}
		set {
			this.redondeoFieldSpecified = value;
		}
	}
    
	/// <comentarios/>
	public decimal DevengadosTotal {
		get {
			return this.devengadosTotalField;
		}
		set {
			this.devengadosTotalField = value;
		}
	}
    
	/// <comentarios/>
	public decimal DeduccionesTotal {
		get {
			return this.deduccionesTotalField;
		}
		set {
			this.deduccionesTotalField = value;
		}
	}
    
	/// <comentarios/>
	public decimal ComprobanteTotal {
		get {
			return this.comprobanteTotalField;
		}
		set {
			this.comprobanteTotalField = value;
		}
	}
}