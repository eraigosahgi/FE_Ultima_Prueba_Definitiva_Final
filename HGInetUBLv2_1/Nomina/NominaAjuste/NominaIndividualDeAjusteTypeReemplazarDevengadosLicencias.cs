/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="dian:gov:co:facturaelectronica:NominaIndividualDeAjuste")]
public partial class NominaIndividualDeAjusteTypeReemplazarDevengadosLicencias {
    
	private NominaIndividualDeAjusteTypeReemplazarDevengadosLicenciasLicenciaMP[] licenciaMPField;
    
	private NominaIndividualDeAjusteTypeReemplazarDevengadosLicenciasLicenciaR[] licenciaRField;
    
	private NominaIndividualDeAjusteTypeReemplazarDevengadosLicenciasLicenciaNR[] licenciaNRField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("LicenciaMP")]
	public NominaIndividualDeAjusteTypeReemplazarDevengadosLicenciasLicenciaMP[] LicenciaMP {
		get {
			return this.licenciaMPField;
		}
		set {
			this.licenciaMPField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("LicenciaR")]
	public NominaIndividualDeAjusteTypeReemplazarDevengadosLicenciasLicenciaR[] LicenciaR {
		get {
			return this.licenciaRField;
		}
		set {
			this.licenciaRField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("LicenciaNR")]
	public NominaIndividualDeAjusteTypeReemplazarDevengadosLicenciasLicenciaNR[] LicenciaNR {
		get {
			return this.licenciaNRField;
		}
		set {
			this.licenciaNRField = value;
		}
	}
}