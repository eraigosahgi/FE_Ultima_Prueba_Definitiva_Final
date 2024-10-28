/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="dian:gov:co:facturaelectronica:NominaIndividual")]
public partial class NominaIndividualTypeDeducciones {
    
	private NominaIndividualTypeDeduccionesSalud saludField;
    
	private NominaIndividualTypeDeduccionesFondoPension fondoPensionField;
    
	private NominaIndividualTypeDeduccionesFondoSP fondoSPField;
    
	private NominaIndividualTypeDeduccionesSindicato[] sindicatosField;
    
	private NominaIndividualTypeDeduccionesSancion[] sancionesField;
    
	private NominaIndividualTypeDeduccionesLibranza[] libranzasField;
    
	private decimal[] pagosTercerosField;
    
	private decimal[] anticiposField;
    
	private decimal[] otrasDeduccionesField;
    
	private decimal pensionVoluntariaField;
    
	private bool pensionVoluntariaFieldSpecified;
    
	private decimal retencionFuenteField;
    
	private bool retencionFuenteFieldSpecified;
    
	private decimal aFCField;
    
	private bool aFCFieldSpecified;
    
	private decimal cooperativaField;
    
	private bool cooperativaFieldSpecified;
    
	private decimal embargoFiscalField;
    
	private bool embargoFiscalFieldSpecified;
    
	private decimal planComplementariosField;
    
	private bool planComplementariosFieldSpecified;
    
	private decimal educacionField;
    
	private bool educacionFieldSpecified;
    
	private decimal reintegroField;
    
	private bool reintegroFieldSpecified;
    
	private decimal deudaField;
    
	private bool deudaFieldSpecified;
    
	/// <comentarios/>
	public NominaIndividualTypeDeduccionesSalud Salud {
		get {
			return this.saludField;
		}
		set {
			this.saludField = value;
		}
	}
    
	/// <comentarios/>
	public NominaIndividualTypeDeduccionesFondoPension FondoPension {
		get {
			return this.fondoPensionField;
		}
		set {
			this.fondoPensionField = value;
		}
	}
    
	/// <comentarios/>
	public NominaIndividualTypeDeduccionesFondoSP FondoSP {
		get {
			return this.fondoSPField;
		}
		set {
			this.fondoSPField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlArrayItemAttribute("Sindicato", IsNullable=false)]
	public NominaIndividualTypeDeduccionesSindicato[] Sindicatos {
		get {
			return this.sindicatosField;
		}
		set {
			this.sindicatosField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlArrayItemAttribute("Sancion", IsNullable=false)]
	public NominaIndividualTypeDeduccionesSancion[] Sanciones {
		get {
			return this.sancionesField;
		}
		set {
			this.sancionesField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlArrayItemAttribute("Libranza", IsNullable=false)]
	public NominaIndividualTypeDeduccionesLibranza[] Libranzas {
		get {
			return this.libranzasField;
		}
		set {
			this.libranzasField = value;
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
	[System.Xml.Serialization.XmlArrayItemAttribute("OtraDeduccion", IsNullable=false)]
	public decimal[] OtrasDeducciones {
		get {
			return this.otrasDeduccionesField;
		}
		set {
			this.otrasDeduccionesField = value;
		}
	}
    
	/// <comentarios/>
	public decimal PensionVoluntaria {
		get {
			return this.pensionVoluntariaField;
		}
		set {
			this.pensionVoluntariaField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlIgnoreAttribute()]
	public bool PensionVoluntariaSpecified {
		get {
			return this.pensionVoluntariaFieldSpecified;
		}
		set {
			this.pensionVoluntariaFieldSpecified = value;
		}
	}
    
	/// <comentarios/>
	public decimal RetencionFuente {
		get {
			return this.retencionFuenteField;
		}
		set {
			this.retencionFuenteField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlIgnoreAttribute()]
	public bool RetencionFuenteSpecified {
		get {
			return this.retencionFuenteFieldSpecified;
		}
		set {
			this.retencionFuenteFieldSpecified = value;
		}
	}
    
	/// <comentarios/>
	public decimal AFC {
		get {
			return this.aFCField;
		}
		set {
			this.aFCField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlIgnoreAttribute()]
	public bool AFCSpecified {
		get {
			return this.aFCFieldSpecified;
		}
		set {
			this.aFCFieldSpecified = value;
		}
	}
    
	/// <comentarios/>
	public decimal Cooperativa {
		get {
			return this.cooperativaField;
		}
		set {
			this.cooperativaField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlIgnoreAttribute()]
	public bool CooperativaSpecified {
		get {
			return this.cooperativaFieldSpecified;
		}
		set {
			this.cooperativaFieldSpecified = value;
		}
	}
    
	/// <comentarios/>
	public decimal EmbargoFiscal {
		get {
			return this.embargoFiscalField;
		}
		set {
			this.embargoFiscalField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlIgnoreAttribute()]
	public bool EmbargoFiscalSpecified {
		get {
			return this.embargoFiscalFieldSpecified;
		}
		set {
			this.embargoFiscalFieldSpecified = value;
		}
	}
    
	/// <comentarios/>
	public decimal PlanComplementarios {
		get {
			return this.planComplementariosField;
		}
		set {
			this.planComplementariosField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlIgnoreAttribute()]
	public bool PlanComplementariosSpecified {
		get {
			return this.planComplementariosFieldSpecified;
		}
		set {
			this.planComplementariosFieldSpecified = value;
		}
	}
    
	/// <comentarios/>
	public decimal Educacion {
		get {
			return this.educacionField;
		}
		set {
			this.educacionField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlIgnoreAttribute()]
	public bool EducacionSpecified {
		get {
			return this.educacionFieldSpecified;
		}
		set {
			this.educacionFieldSpecified = value;
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
    
	/// <comentarios/>
	public decimal Deuda {
		get {
			return this.deudaField;
		}
		set {
			this.deudaField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlIgnoreAttribute()]
	public bool DeudaSpecified {
		get {
			return this.deudaFieldSpecified;
		}
		set {
			this.deudaFieldSpecified = value;
		}
	}
}