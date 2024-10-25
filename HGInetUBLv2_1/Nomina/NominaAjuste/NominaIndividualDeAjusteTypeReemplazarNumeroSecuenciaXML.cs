/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="dian:gov:co:facturaelectronica:NominaIndividualDeAjuste")]
public partial class NominaIndividualDeAjusteTypeReemplazarNumeroSecuenciaXML {
    
	private string codigoTrabajadorField;
    
	private string prefijoField;
    
	private string consecutivoField;
    
	private string numeroField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public string CodigoTrabajador {
		get {
			return this.codigoTrabajadorField;
		}
		set {
			this.codigoTrabajadorField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public string Prefijo {
		get {
			return this.prefijoField;
		}
		set {
			this.prefijoField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="integer")]
	public string Consecutivo {
		get {
			return this.consecutivoField;
		}
		set {
			this.consecutivoField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public string Numero {
		get {
			return this.numeroField;
		}
		set {
			this.numeroField = value;
		}
	}
}