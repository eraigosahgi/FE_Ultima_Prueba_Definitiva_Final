/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="dian:gov:co:facturaelectronica:NominaIndividualDeAjuste")]
public partial class NominaIndividualDeAjusteTypeReemplazarDevengadosOtroConcepto {
    
	private string descripcionConceptoField;
    
	private decimal conceptoSField;
    
	private bool conceptoSFieldSpecified;
    
	private decimal conceptoNSField;
    
	private bool conceptoNSFieldSpecified;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public string DescripcionConcepto {
		get {
			return this.descripcionConceptoField;
		}
		set {
			this.descripcionConceptoField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public decimal ConceptoS {
		get {
			return this.conceptoSField;
		}
		set {
			this.conceptoSField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlIgnoreAttribute()]
	public bool ConceptoSSpecified {
		get {
			return this.conceptoSFieldSpecified;
		}
		set {
			this.conceptoSFieldSpecified = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public decimal ConceptoNS {
		get {
			return this.conceptoNSField;
		}
		set {
			this.conceptoNSField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlIgnoreAttribute()]
	public bool ConceptoNSSpecified {
		get {
			return this.conceptoNSFieldSpecified;
		}
		set {
			this.conceptoNSFieldSpecified = value;
		}
	}
}