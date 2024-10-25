/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="dian:gov:co:facturaelectronica:NominaIndividualDeAjuste")]
public partial class NominaIndividualDeAjusteTypeReemplazarDevengados {
    
	private NominaIndividualDeAjusteTypeReemplazarDevengadosBasico basicoField;
    
	private NominaIndividualDeAjusteTypeReemplazarDevengadosTransporte[] transporteField;
    
	private NominaIndividualDeAjusteTypeReemplazarDevengadosHED[] hEDsField;
    
	private NominaIndividualDeAjusteTypeReemplazarDevengadosHEN[] hENsField;
    
	private NominaIndividualDeAjusteTypeReemplazarDevengadosHRN[] hRNsField;
    
	private NominaIndividualDeAjusteTypeReemplazarDevengadosHEDDF[] hEDDFsField;
    
	private NominaIndividualDeAjusteTypeReemplazarDevengadosHRDDF[] hRDDFsField;
    
	private NominaIndividualDeAjusteTypeReemplazarDevengadosHENDF[] hENDFsField;
    
	private NominaIndividualDeAjusteTypeReemplazarDevengadosHRNDF[] hRNDFsField;
    
	private NominaIndividualDeAjusteTypeReemplazarDevengadosVacaciones vacacionesField;
    
	private NominaIndividualDeAjusteTypeReemplazarDevengadosPrimas primasField;
    
	private NominaIndividualDeAjusteTypeReemplazarDevengadosCesantias cesantiasField;
    
	private NominaIndividualDeAjusteTypeReemplazarDevengadosIncapacidad[] incapacidadesField;
    
	private NominaIndividualDeAjusteTypeReemplazarDevengadosLicencias licenciasField;
    
	private NominaIndividualDeAjusteTypeReemplazarDevengadosBonificacion[] bonificacionesField;
    
	private NominaIndividualDeAjusteTypeReemplazarDevengadosAuxilio[] auxiliosField;
    
	private NominaIndividualDeAjusteTypeReemplazarDevengadosHuelgaLegal[] huelgasLegalesField;
    
	private NominaIndividualDeAjusteTypeReemplazarDevengadosOtroConcepto[] otrosConceptosField;
    
	private NominaIndividualDeAjusteTypeReemplazarDevengadosCompensacion[] compensacionesField;
    
	private NominaIndividualDeAjusteTypeReemplazarDevengadosBonoEPCTV[] bonoEPCTVsField;
    
	private decimal[] comisionesField;
    
	private decimal[] pagosTercerosField;
    
	private decimal[] anticiposField;
    
	private decimal dotacionField;
    
	private bool dotacionFieldSpecified;
    
	private decimal apoyoSostField;
    
	private bool apoyoSostFieldSpecified;
    
	private decimal teletrabajoField;
    
	private bool teletrabajoFieldSpecified;
    
	private decimal bonifRetiroField;
    
	private bool bonifRetiroFieldSpecified;
    
	private decimal indemnizacionField;
    
	private bool indemnizacionFieldSpecified;
    
	private decimal reintegroField;
    
	private bool reintegroFieldSpecified;
    
	/// <comentarios/>
	public NominaIndividualDeAjusteTypeReemplazarDevengadosBasico Basico {
		get {
			return this.basicoField;
		}
		set {
			this.basicoField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("Transporte")]
	public NominaIndividualDeAjusteTypeReemplazarDevengadosTransporte[] Transporte {
		get {
			return this.transporteField;
		}
		set {
			this.transporteField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlArrayItemAttribute("HED", IsNullable=false)]
	public NominaIndividualDeAjusteTypeReemplazarDevengadosHED[] HEDs {
		get {
			return this.hEDsField;
		}
		set {
			this.hEDsField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlArrayItemAttribute("HEN", IsNullable=false)]
	public NominaIndividualDeAjusteTypeReemplazarDevengadosHEN[] HENs {
		get {
			return this.hENsField;
		}
		set {
			this.hENsField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlArrayItemAttribute("HRN", IsNullable=false)]
	public NominaIndividualDeAjusteTypeReemplazarDevengadosHRN[] HRNs {
		get {
			return this.hRNsField;
		}
		set {
			this.hRNsField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlArrayItemAttribute("HEDDF", IsNullable=false)]
	public NominaIndividualDeAjusteTypeReemplazarDevengadosHEDDF[] HEDDFs {
		get {
			return this.hEDDFsField;
		}
		set {
			this.hEDDFsField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlArrayItemAttribute("HRDDF", IsNullable=false)]
	public NominaIndividualDeAjusteTypeReemplazarDevengadosHRDDF[] HRDDFs {
		get {
			return this.hRDDFsField;
		}
		set {
			this.hRDDFsField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlArrayItemAttribute("HENDF", IsNullable=false)]
	public NominaIndividualDeAjusteTypeReemplazarDevengadosHENDF[] HENDFs {
		get {
			return this.hENDFsField;
		}
		set {
			this.hENDFsField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlArrayItemAttribute("HRNDF", IsNullable=false)]
	public NominaIndividualDeAjusteTypeReemplazarDevengadosHRNDF[] HRNDFs {
		get {
			return this.hRNDFsField;
		}
		set {
			this.hRNDFsField = value;
		}
	}
    
	/// <comentarios/>
	public NominaIndividualDeAjusteTypeReemplazarDevengadosVacaciones Vacaciones {
		get {
			return this.vacacionesField;
		}
		set {
			this.vacacionesField = value;
		}
	}
    
	/// <comentarios/>
	public NominaIndividualDeAjusteTypeReemplazarDevengadosPrimas Primas {
		get {
			return this.primasField;
		}
		set {
			this.primasField = value;
		}
	}
    
	/// <comentarios/>
	public NominaIndividualDeAjusteTypeReemplazarDevengadosCesantias Cesantias {
		get {
			return this.cesantiasField;
		}
		set {
			this.cesantiasField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlArrayItemAttribute("Incapacidad", IsNullable=false)]
	public NominaIndividualDeAjusteTypeReemplazarDevengadosIncapacidad[] Incapacidades {
		get {
			return this.incapacidadesField;
		}
		set {
			this.incapacidadesField = value;
		}
	}
    
	/// <comentarios/>
	public NominaIndividualDeAjusteTypeReemplazarDevengadosLicencias Licencias {
		get {
			return this.licenciasField;
		}
		set {
			this.licenciasField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlArrayItemAttribute("Bonificacion", IsNullable=false)]
	public NominaIndividualDeAjusteTypeReemplazarDevengadosBonificacion[] Bonificaciones {
		get {
			return this.bonificacionesField;
		}
		set {
			this.bonificacionesField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlArrayItemAttribute("Auxilio", IsNullable=false)]
	public NominaIndividualDeAjusteTypeReemplazarDevengadosAuxilio[] Auxilios {
		get {
			return this.auxiliosField;
		}
		set {
			this.auxiliosField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlArrayItemAttribute("HuelgaLegal", IsNullable=false)]
	public NominaIndividualDeAjusteTypeReemplazarDevengadosHuelgaLegal[] HuelgasLegales {
		get {
			return this.huelgasLegalesField;
		}
		set {
			this.huelgasLegalesField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlArrayItemAttribute("OtroConcepto", IsNullable=false)]
	public NominaIndividualDeAjusteTypeReemplazarDevengadosOtroConcepto[] OtrosConceptos {
		get {
			return this.otrosConceptosField;
		}
		set {
			this.otrosConceptosField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlArrayItemAttribute("Compensacion", IsNullable=false)]
	public NominaIndividualDeAjusteTypeReemplazarDevengadosCompensacion[] Compensaciones {
		get {
			return this.compensacionesField;
		}
		set {
			this.compensacionesField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlArrayItemAttribute("BonoEPCTV", IsNullable=false)]
	public NominaIndividualDeAjusteTypeReemplazarDevengadosBonoEPCTV[] BonoEPCTVs {
		get {
			return this.bonoEPCTVsField;
		}
		set {
			this.bonoEPCTVsField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlArrayItemAttribute("Comision", IsNullable=false)]
	public decimal[] Comisiones {
		get {
			return this.comisionesField;
		}
		set {
			this.comisionesField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlArrayItemAttribute("PagoTercero", IsNullable=false)]
	public decimal[] PagosTerceros {
		get {
			return this.pagosTercerosField;
		}
		set {
			this.pagosTercerosField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlArrayItemAttribute("Anticipo", IsNullable=false)]
	public decimal[] Anticipos {
		get {
			return this.anticiposField;
		}
		set {
			this.anticiposField = value;
		}
	}
    
	/// <comentarios/>
	public decimal Dotacion {
		get {
			return this.dotacionField;
		}
		set {
			this.dotacionField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlIgnoreAttribute()]
	public bool DotacionSpecified {
		get {
			return this.dotacionFieldSpecified;
		}
		set {
			this.dotacionFieldSpecified = value;
		}
	}
    
	/// <comentarios/>
	public decimal ApoyoSost {
		get {
			return this.apoyoSostField;
		}
		set {
			this.apoyoSostField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlIgnoreAttribute()]
	public bool ApoyoSostSpecified {
		get {
			return this.apoyoSostFieldSpecified;
		}
		set {
			this.apoyoSostFieldSpecified = value;
		}
	}
    
	/// <comentarios/>
	public decimal Teletrabajo {
		get {
			return this.teletrabajoField;
		}
		set {
			this.teletrabajoField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlIgnoreAttribute()]
	public bool TeletrabajoSpecified {
		get {
			return this.teletrabajoFieldSpecified;
		}
		set {
			this.teletrabajoFieldSpecified = value;
		}
	}
    
	/// <comentarios/>
	public decimal BonifRetiro {
		get {
			return this.bonifRetiroField;
		}
		set {
			this.bonifRetiroField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlIgnoreAttribute()]
	public bool BonifRetiroSpecified {
		get {
			return this.bonifRetiroFieldSpecified;
		}
		set {
			this.bonifRetiroFieldSpecified = value;
		}
	}
    
	/// <comentarios/>
	public decimal Indemnizacion {
		get {
			return this.indemnizacionField;
		}
		set {
			this.indemnizacionField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlIgnoreAttribute()]
	public bool IndemnizacionSpecified {
		get {
			return this.indemnizacionFieldSpecified;
		}
		set {
			this.indemnizacionFieldSpecified = value;
		}
	}
    
	/// <comentarios/>
	public decimal Reintegro {
		get {
			return this.reintegroField;
		}
		set {
			this.reintegroField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlIgnoreAttribute()]
	public bool ReintegroSpecified {
		get {
			return this.reintegroFieldSpecified;
		}
		set {
			this.reintegroFieldSpecified = value;
		}
	}
}