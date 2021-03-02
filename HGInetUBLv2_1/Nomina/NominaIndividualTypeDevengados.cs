/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="dian:gov:co:facturaelectronica:NominaIndividual")]
public partial class NominaIndividualTypeDevengados {
    
	private NominaIndividualTypeDevengadosBasico basicoField;
    
	private NominaIndividualTypeDevengadosTransporte[] transporteField;
    
	private NominaIndividualTypeDevengadosHED[] hEDsField;
    
	private NominaIndividualTypeDevengadosHEN[] hENsField;
    
	private NominaIndividualTypeDevengadosHRN[] hRNsField;
    
	private NominaIndividualTypeDevengadosHEDDF[] hEDDFsField;
    
	private NominaIndividualTypeDevengadosHRDDF[] hRDDFsField;
    
	private NominaIndividualTypeDevengadosHENDF[] hENDFsField;
    
	private NominaIndividualTypeDevengadosHRNDF[] hRNDFsField;
    
	private NominaIndividualTypeDevengadosVacaciones vacacionesField;
    
	private NominaIndividualTypeDevengadosPrimas primasField;
    
	private NominaIndividualTypeDevengadosCesantias cesantiasField;
    
	private NominaIndividualTypeDevengadosIncapacidad[] incapacidadesField;
    
	private NominaIndividualTypeDevengadosLicencias licenciasField;
    
	private NominaIndividualTypeDevengadosBonificacion[] bonificacionesField;
    
	private NominaIndividualTypeDevengadosAuxilio[] auxiliosField;
    
	private NominaIndividualTypeDevengadosHuelgaLegal[] huelgasLegalesField;
    
	private NominaIndividualTypeDevengadosOtroConcepto[] otrosConceptosField;
    
	private NominaIndividualTypeDevengadosCompensacion[] compensacionesField;
    
	private NominaIndividualTypeDevengadosBonoEPCTV[] bonoEPCTVsField;
    
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
	public NominaIndividualTypeDevengadosBasico Basico {
		get {
			return this.basicoField;
		}
		set {
			this.basicoField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("Transporte")]
	public NominaIndividualTypeDevengadosTransporte[] Transporte {
		get {
			return this.transporteField;
		}
		set {
			this.transporteField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlArrayItemAttribute("HED", IsNullable=false)]
	public NominaIndividualTypeDevengadosHED[] HEDs {
		get {
			return this.hEDsField;
		}
		set {
			this.hEDsField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlArrayItemAttribute("HEN", IsNullable=false)]
	public NominaIndividualTypeDevengadosHEN[] HENs {
		get {
			return this.hENsField;
		}
		set {
			this.hENsField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlArrayItemAttribute("HRN", IsNullable=false)]
	public NominaIndividualTypeDevengadosHRN[] HRNs {
		get {
			return this.hRNsField;
		}
		set {
			this.hRNsField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlArrayItemAttribute("HEDDF", IsNullable=false)]
	public NominaIndividualTypeDevengadosHEDDF[] HEDDFs {
		get {
			return this.hEDDFsField;
		}
		set {
			this.hEDDFsField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlArrayItemAttribute("HRDDF", IsNullable=false)]
	public NominaIndividualTypeDevengadosHRDDF[] HRDDFs {
		get {
			return this.hRDDFsField;
		}
		set {
			this.hRDDFsField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlArrayItemAttribute("HENDF", IsNullable=false)]
	public NominaIndividualTypeDevengadosHENDF[] HENDFs {
		get {
			return this.hENDFsField;
		}
		set {
			this.hENDFsField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlArrayItemAttribute("HRNDF", IsNullable=false)]
	public NominaIndividualTypeDevengadosHRNDF[] HRNDFs {
		get {
			return this.hRNDFsField;
		}
		set {
			this.hRNDFsField = value;
		}
	}
    
	/// <comentarios/>
	public NominaIndividualTypeDevengadosVacaciones Vacaciones {
		get {
			return this.vacacionesField;
		}
		set {
			this.vacacionesField = value;
		}
	}
    
	/// <comentarios/>
	public NominaIndividualTypeDevengadosPrimas Primas {
		get {
			return this.primasField;
		}
		set {
			this.primasField = value;
		}
	}
    
	/// <comentarios/>
	public NominaIndividualTypeDevengadosCesantias Cesantias {
		get {
			return this.cesantiasField;
		}
		set {
			this.cesantiasField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlArrayItemAttribute("Incapacidad", IsNullable=false)]
	public NominaIndividualTypeDevengadosIncapacidad[] Incapacidades {
		get {
			return this.incapacidadesField;
		}
		set {
			this.incapacidadesField = value;
		}
	}
    
	/// <comentarios/>
	public NominaIndividualTypeDevengadosLicencias Licencias {
		get {
			return this.licenciasField;
		}
		set {
			this.licenciasField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlArrayItemAttribute("Bonificacion", IsNullable=false)]
	public NominaIndividualTypeDevengadosBonificacion[] Bonificaciones {
		get {
			return this.bonificacionesField;
		}
		set {
			this.bonificacionesField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlArrayItemAttribute("Auxilio", IsNullable=false)]
	public NominaIndividualTypeDevengadosAuxilio[] Auxilios {
		get {
			return this.auxiliosField;
		}
		set {
			this.auxiliosField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlArrayItemAttribute("HuelgaLegal", IsNullable=false)]
	public NominaIndividualTypeDevengadosHuelgaLegal[] HuelgasLegales {
		get {
			return this.huelgasLegalesField;
		}
		set {
			this.huelgasLegalesField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlArrayItemAttribute("OtroConcepto", IsNullable=false)]
	public NominaIndividualTypeDevengadosOtroConcepto[] OtrosConceptos {
		get {
			return this.otrosConceptosField;
		}
		set {
			this.otrosConceptosField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlArrayItemAttribute("Compensacion", IsNullable=false)]
	public NominaIndividualTypeDevengadosCompensacion[] Compensaciones {
		get {
			return this.compensacionesField;
		}
		set {
			this.compensacionesField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlArrayItemAttribute("BonoEPCTV", IsNullable=false)]
	public NominaIndividualTypeDevengadosBonoEPCTV[] BonoEPCTVs {
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