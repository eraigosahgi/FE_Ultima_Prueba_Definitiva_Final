/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="dian:gov:co:facturaelectronica:NominaIndividualDeAjuste")]
public partial class NominaIndividualDeAjusteTypeReemplazarDevengadosTransporte {
    
	private decimal auxilioTransporteField;
    
	private bool auxilioTransporteFieldSpecified;
    
	private decimal viaticoManuAlojSField;
    
	private bool viaticoManuAlojSFieldSpecified;
    
	private decimal viaticoManuAlojNSField;
    
	private bool viaticoManuAlojNSFieldSpecified;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public decimal AuxilioTransporte {
		get {
			return this.auxilioTransporteField;
		}
		set {
			this.auxilioTransporteField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlIgnoreAttribute()]
	public bool AuxilioTransporteSpecified {
		get {
			return this.auxilioTransporteFieldSpecified;
		}
		set {
			this.auxilioTransporteFieldSpecified = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public decimal ViaticoManuAlojS {
		get {
			return this.viaticoManuAlojSField;
		}
		set {
			this.viaticoManuAlojSField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlIgnoreAttribute()]
	public bool ViaticoManuAlojSSpecified {
		get {
			return this.viaticoManuAlojSFieldSpecified;
		}
		set {
			this.viaticoManuAlojSFieldSpecified = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public decimal ViaticoManuAlojNS {
		get {
			return this.viaticoManuAlojNSField;
		}
		set {
			this.viaticoManuAlojNSField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlIgnoreAttribute()]
	public bool ViaticoManuAlojNSSpecified {
		get {
			return this.viaticoManuAlojNSFieldSpecified;
		}
		set {
			this.viaticoManuAlojNSFieldSpecified = value;
		}
	}
}