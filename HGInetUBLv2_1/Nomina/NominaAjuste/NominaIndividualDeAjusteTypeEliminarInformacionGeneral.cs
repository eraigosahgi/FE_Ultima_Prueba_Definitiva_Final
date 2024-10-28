/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="dian:gov:co:facturaelectronica:NominaIndividualDeAjuste")]
public partial class NominaIndividualDeAjusteTypeEliminarInformacionGeneral {
    
	private string versionField;
    
	private string ambienteField;
    
	private string tipoXMLField;
    
	private string cUNEField;
    
	private string encripCUNEField;
    
	private System.DateTime fechaGenField;

	private string horaGenField;

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
	[System.Xml.Serialization.XmlAttributeAttribute(DataType = "string")]
	public string HoraGen
	{
		get
		{
			return this.horaGenField;
		}
		set
		{
			this.horaGenField = value;
		}
	}
}