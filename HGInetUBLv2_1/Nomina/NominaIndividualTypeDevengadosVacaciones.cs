/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="dian:gov:co:facturaelectronica:NominaIndividual")]
public partial class NominaIndividualTypeDevengadosVacaciones {
    
	private NominaIndividualTypeDevengadosVacacionesVacacionesComunes[] vacacionesComunesField;
    
	private NominaIndividualTypeDevengadosVacacionesVacacionesCompensadas[] vacacionesCompensadasField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("VacacionesComunes")]
	public NominaIndividualTypeDevengadosVacacionesVacacionesComunes[] VacacionesComunes {
		get {
			return this.vacacionesComunesField;
		}
		set {
			this.vacacionesComunesField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("VacacionesCompensadas")]
	public NominaIndividualTypeDevengadosVacacionesVacacionesCompensadas[] VacacionesCompensadas {
		get {
			return this.vacacionesCompensadasField;
		}
		set {
			this.vacacionesCompensadasField = value;
		}
	}
}