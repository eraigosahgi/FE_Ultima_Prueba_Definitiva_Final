/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="dian:gov:co:facturaelectronica:NominaIndividual")]
[System.Xml.Serialization.XmlRootAttribute("NominaIndividual", Namespace="dian:gov:co:facturaelectronica:NominaIndividual", IsNullable=false)]
public partial class NominaIndividualType {
    
	private UBLExtensionType[] uBLExtensionsField;
    
	private NominaIndividualTypeNovedad novedadField;
    
	private NominaIndividualTypePeriodo periodoField;
    
	private NominaIndividualTypeNumeroSecuenciaXML numeroSecuenciaXMLField;
    
	private NominaIndividualTypeLugarGeneracionXML lugarGeneracionXMLField;
    
	private NominaIndividualTypeProveedorXML proveedorXMLField;
    
	private string codigoQRField;
    
	private NominaIndividualTypeInformacionGeneral informacionGeneralField;
    
	private string[] notasField;
    
	private NominaIndividualTypeEmpleador empleadorField;
    
	private NominaIndividualTypeTrabajador trabajadorField;
    
	private NominaIndividualTypePago pagoField;
    
	private System.DateTime[] fechasPagosField;
    
	private NominaIndividualTypeDevengados devengadosField;
    
	private NominaIndividualTypeDeducciones deduccionesField;
    
	private decimal redondeoField;
    
	private bool redondeoFieldSpecified;
    
	private decimal devengadosTotalField;
    
	private decimal deduccionesTotalField;
    
	private decimal comprobanteTotalField;
    
	private string schemaLocationField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlArrayAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2")]
	[System.Xml.Serialization.XmlArrayItemAttribute("UBLExtension", IsNullable=false)]
	public UBLExtensionType[] UBLExtensions {
		get {
			return this.uBLExtensionsField;
		}
		set {
			this.uBLExtensionsField = value;
		}
	}
    
	/// <comentarios/>
	public NominaIndividualTypeNovedad Novedad {
		get {
			return this.novedadField;
		}
		set {
			this.novedadField = value;
		}
	}
    
	/// <comentarios/>
	public NominaIndividualTypePeriodo Periodo {
		get {
			return this.periodoField;
		}
		set {
			this.periodoField = value;
		}
	}
    
	/// <comentarios/>
	public NominaIndividualTypeNumeroSecuenciaXML NumeroSecuenciaXML {
		get {
			return this.numeroSecuenciaXMLField;
		}
		set {
			this.numeroSecuenciaXMLField = value;
		}
	}
    
	/// <comentarios/>
	public NominaIndividualTypeLugarGeneracionXML LugarGeneracionXML {
		get {
			return this.lugarGeneracionXMLField;
		}
		set {
			this.lugarGeneracionXMLField = value;
		}
	}
    
	/// <comentarios/>
	public NominaIndividualTypeProveedorXML ProveedorXML {
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
	public NominaIndividualTypeInformacionGeneral InformacionGeneral {
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
	public NominaIndividualTypeEmpleador Empleador {
		get {
			return this.empleadorField;
		}
		set {
			this.empleadorField = value;
		}
	}
    
	/// <comentarios/>
	public NominaIndividualTypeTrabajador Trabajador {
		get {
			return this.trabajadorField;
		}
		set {
			this.trabajadorField = value;
		}
	}
    
	/// <comentarios/>
	public NominaIndividualTypePago Pago {
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
	public NominaIndividualTypeDevengados Devengados {
		get {
			return this.devengadosField;
		}
		set {
			this.devengadosField = value;
		}
	}
    
	/// <comentarios/>
	public NominaIndividualTypeDeducciones Deducciones {
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
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public string SchemaLocation {
		get {
			return this.schemaLocationField;
		}
		set {
			this.schemaLocationField = value;
		}
	}
}