/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="dian:gov:co:facturaelectronica:NominaIndividual")]
public partial class NominaIndividualTypePago {
    
	private string formaField;
    
	private string metodoField;
    
	private string bancoField;
    
	private string tipoCuentaField;
    
	private string numeroCuentaField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="integer")]
	public string Forma {
		get {
			return this.formaField;
		}
		set {
			this.formaField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="integer")]
	public string Metodo {
		get {
			return this.metodoField;
		}
		set {
			this.metodoField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public string Banco {
		get {
			return this.bancoField;
		}
		set {
			this.bancoField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public string TipoCuenta {
		get {
			return this.tipoCuentaField;
		}
		set {
			this.tipoCuentaField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public string NumeroCuenta {
		get {
			return this.numeroCuentaField;
		}
		set {
			this.numeroCuentaField = value;
		}
	}
}