/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="dian:gov:co:facturaelectronica:NominaIndividual")]
public partial class NominaIndividualTypeDevengadosAuxilio {
    
	private decimal auxilioSField;
    
	private bool auxilioSFieldSpecified;
    
	private decimal auxilioNSField;
    
	private bool auxilioNSFieldSpecified;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public decimal AuxilioS {
		get {
			return this.auxilioSField;
		}
		set {
			this.auxilioSField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlIgnoreAttribute()]
	public bool AuxilioSSpecified {
		get {
			return this.auxilioSFieldSpecified;
		}
		set {
			this.auxilioSFieldSpecified = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public decimal AuxilioNS {
		get {
			return this.auxilioNSField;
		}
		set {
			this.auxilioNSField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlIgnoreAttribute()]
	public bool AuxilioNSSpecified {
		get {
			return this.auxilioNSFieldSpecified;
		}
		set {
			this.auxilioNSFieldSpecified = value;
		}
	}
}