/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="dian:gov:co:facturaelectronica:NominaIndividualDeAjuste")]
public partial class NominaIndividualDeAjusteTypeEliminar {
    
	private NominaIndividualDeAjusteTypeEliminarEliminandoPredecesor eliminandoPredecesorField;
    
	private NominaIndividualDeAjusteTypeEliminarNumeroSecuenciaXML numeroSecuenciaXMLField;
    
	private NominaIndividualDeAjusteTypeEliminarLugarGeneracionXML lugarGeneracionXMLField;
    
	private NominaIndividualDeAjusteTypeEliminarProveedorXML proveedorXMLField;
    
	private string codigoQRField;
    
	private NominaIndividualDeAjusteTypeEliminarInformacionGeneral informacionGeneralField;
    
	private string[] notasField;
    
	private NominaIndividualDeAjusteTypeEliminarEmpleador empleadorField;
    
	/// <comentarios/>
	public NominaIndividualDeAjusteTypeEliminarEliminandoPredecesor EliminandoPredecesor {
		get {
			return this.eliminandoPredecesorField;
		}
		set {
			this.eliminandoPredecesorField = value;
		}
	}
    
	/// <comentarios/>
	public NominaIndividualDeAjusteTypeEliminarNumeroSecuenciaXML NumeroSecuenciaXML {
		get {
			return this.numeroSecuenciaXMLField;
		}
		set {
			this.numeroSecuenciaXMLField = value;
		}
	}
    
	/// <comentarios/>
	public NominaIndividualDeAjusteTypeEliminarLugarGeneracionXML LugarGeneracionXML {
		get {
			return this.lugarGeneracionXMLField;
		}
		set {
			this.lugarGeneracionXMLField = value;
		}
	}
    
	/// <comentarios/>
	public NominaIndividualDeAjusteTypeEliminarProveedorXML ProveedorXML {
		get {
			return this.proveedorXMLField;
		}
		set {
			this.proveedorXMLField = value;
		}
	}
    
	/// <comentarios/>
	public string CodigoQR {
		get {
			return this.codigoQRField;
		}
		set {
			this.codigoQRField = value;
		}
	}
    
	/// <comentarios/>
	public NominaIndividualDeAjusteTypeEliminarInformacionGeneral InformacionGeneral {
		get {
			return this.informacionGeneralField;
		}
		set {
			this.informacionGeneralField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("Notas")]
	public string[] Notas {
		get {
			return this.notasField;
		}
		set {
			this.notasField = value;
		}
	}
    
	/// <comentarios/>
	public NominaIndividualDeAjusteTypeEliminarEmpleador Empleador {
		get {
			return this.empleadorField;
		}
		set {
			this.empleadorField = value;
		}
	}
}