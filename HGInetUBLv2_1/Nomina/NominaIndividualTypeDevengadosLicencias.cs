/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="dian:gov:co:facturaelectronica:NominaIndividual")]
public partial class NominaIndividualTypeDevengadosLicencias {
    
	private NominaIndividualTypeDevengadosLicenciasLicenciaMP[] licenciaMPField;
    
	private NominaIndividualTypeDevengadosLicenciasLicenciaR[] licenciaRField;
    
	private NominaIndividualTypeDevengadosLicenciasLicenciaNR[] licenciaNRField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("LicenciaMP")]
	public NominaIndividualTypeDevengadosLicenciasLicenciaMP[] LicenciaMP {
		get {
			return this.licenciaMPField;
		}
		set {
			this.licenciaMPField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("LicenciaR")]
	public NominaIndividualTypeDevengadosLicenciasLicenciaR[] LicenciaR {
		get {
			return this.licenciaRField;
		}
		set {
			this.licenciaRField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("LicenciaNR")]
	public NominaIndividualTypeDevengadosLicenciasLicenciaNR[] LicenciaNR {
		get {
			return this.licenciaNRField;
		}
		set {
			this.licenciaNRField = value;
		}
	}
}