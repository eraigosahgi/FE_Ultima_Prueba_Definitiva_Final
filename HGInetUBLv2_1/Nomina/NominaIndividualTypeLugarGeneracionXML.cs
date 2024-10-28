/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="dian:gov:co:facturaelectronica:NominaIndividual")]
public partial class NominaIndividualTypeLugarGeneracionXML {
    
	private string paisField;
    
	private string departamentoEstadoField;
    
	private string municipioCiudadField;
    
	private string idiomaField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public string Pais {
		get {
			return this.paisField;
		}
		set {
			this.paisField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="integer")]
	public string DepartamentoEstado {
		get {
			return this.departamentoEstadoField;
		}
		set {
			this.departamentoEstadoField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="integer")]
	public string MunicipioCiudad {
		get {
			return this.municipioCiudadField;
		}
		set {
			this.municipioCiudadField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public string Idioma {
		get {
			return this.idiomaField;
		}
		set {
			this.idiomaField = value;
		}
	}
}