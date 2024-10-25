/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="dian:gov:co:facturaelectronica:NominaIndividualDeAjuste")]
public partial class NominaIndividualDeAjusteTypeReemplazarDeduccionesFondoSP {
    
	private decimal porcentajeField;
    
	private bool porcentajeFieldSpecified;
    
	private decimal deduccionField;
    
	private bool deduccionFieldSpecified;
    
	private decimal porcentajeSubField;
    
	private bool porcentajeSubFieldSpecified;
    
	private decimal deduccionSubField;
    
	private bool deduccionSubFieldSpecified;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public decimal Porcentaje {
		get {
			return this.porcentajeField;
		}
		set {
			this.porcentajeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlIgnoreAttribute()]
	public bool PorcentajeSpecified {
		get {
			return this.porcentajeFieldSpecified;
		}
		set {
			this.porcentajeFieldSpecified = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public decimal DeduccionSP {
		get {
			return this.deduccionField;
		}
		set {
			this.deduccionField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlIgnoreAttribute()]
	public bool DeduccionSpecified {
		get {
			return this.deduccionFieldSpecified;
		}
		set {
			this.deduccionFieldSpecified = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public decimal PorcentajeSub {
		get {
			return this.porcentajeSubField;
		}
		set {
			this.porcentajeSubField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlIgnoreAttribute()]
	public bool PorcentajeSubSpecified {
		get {
			return this.porcentajeSubFieldSpecified;
		}
		set {
			this.porcentajeSubFieldSpecified = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public decimal DeduccionSub {
		get {
			return this.deduccionSubField;
		}
		set {
			this.deduccionSubField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlIgnoreAttribute()]
	public bool DeduccionSubSpecified {
		get {
			return this.deduccionSubFieldSpecified;
		}
		set {
			this.deduccionSubFieldSpecified = value;
		}
	}
}