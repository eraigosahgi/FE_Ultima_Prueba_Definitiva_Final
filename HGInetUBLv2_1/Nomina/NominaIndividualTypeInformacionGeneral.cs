/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="dian:gov:co:facturaelectronica:NominaIndividual")]
public partial class NominaIndividualTypeInformacionGeneral {
    
	private string versionField;
    
	private string ambienteField;
    
	private string tipoXMLField;
    
	private string cUNEField;
    
	private string encripCUNEField;
    
	private System.DateTime fechaGenField;
    
	private System.DateTime horaGenField;
    
	private string periodoNominaField;
    
	private string tipoMonedaField;
    
	private decimal tRMField;
    
	private bool tRMFieldSpecified;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public string Version {
		get {
			return this.versionField;
		}
		set {
			this.versionField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="integer")]
	public string Ambiente {
		get {
			return this.ambienteField;
		}
		set {
			this.ambienteField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="integer")]
	public string TipoXML {
		get {
			return this.tipoXMLField;
		}
		set {
			this.tipoXMLField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public string CUNE {
		get {
			return this.cUNEField;
		}
		set {
			this.cUNEField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public string EncripCUNE {
		get {
			return this.encripCUNEField;
		}
		set {
			this.encripCUNEField = value;
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
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="time")]
	public System.DateTime HoraGen {
		get {
			return this.horaGenField;
		}
		set {
			this.horaGenField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="integer")]
	public string PeriodoNomina {
		get {
			return this.periodoNominaField;
		}
		set {
			this.periodoNominaField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public string TipoMoneda {
		get {
			return this.tipoMonedaField;
		}
		set {
			this.tipoMonedaField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public decimal TRM {
		get {
			return this.tRMField;
		}
		set {
			this.tRMField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlIgnoreAttribute()]
	public bool TRMSpecified {
		get {
			return this.tRMFieldSpecified;
		}
		set {
			this.tRMFieldSpecified = value;
		}
	}
}