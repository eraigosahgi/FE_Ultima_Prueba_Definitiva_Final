/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="dian:gov:co:facturaelectronica:NominaIndividual")]
public partial class NominaIndividualTypeDevengadosPrimas {
    
	private string cantidadField;
    
	private decimal pagoField;
    
	private decimal pagoNSField;
    
	private bool pagoNSFieldSpecified;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="integer")]
	public string Cantidad {
		get {
			return this.cantidadField;
		}
		set {
			this.cantidadField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public decimal Pago {
		get {
			return this.pagoField;
		}
		set {
			this.pagoField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public decimal PagoNS {
		get {
			return this.pagoNSField;
		}
		set {
			this.pagoNSField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlIgnoreAttribute()]
	public bool PagoNSSpecified {
		get {
			return this.pagoNSFieldSpecified;
		}
		set {
			this.pagoNSFieldSpecified = value;
		}
	}
}