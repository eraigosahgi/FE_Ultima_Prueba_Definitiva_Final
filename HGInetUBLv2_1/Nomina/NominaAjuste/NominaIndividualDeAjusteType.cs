/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="dian:gov:co:facturaelectronica:NominaIndividualDeAjuste")]
[System.Xml.Serialization.XmlRootAttribute("NominaIndividualDeAjuste", Namespace="dian:gov:co:facturaelectronica:NominaIndividualDeAjuste", IsNullable=false)]
public partial class NominaIndividualDeAjusteType {
    
	private UBLExtensionType[] uBLExtensionsField;
    
	private string tipoNotaField;
    
	private NominaIndividualDeAjusteTypeReemplazar reemplazarField;
    
	private NominaIndividualDeAjusteTypeEliminar eliminarField;
    
	private string schemaLocationField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlArrayAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2")]
	[System.Xml.Serialization.XmlArrayItemAttribute("UBLExtension", IsNullable=false)]
	public UBLExtensionType[] UBLExtensions {
		get {
			return this.uBLExtensionsField;
		}
		set {
			this.uBLExtensionsField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(DataType="integer")]
	public string TipoNota {
		get {
			return this.tipoNotaField;
		}
		set {
			this.tipoNotaField = value;
		}
	}
    
	/// <comentarios/>
	public NominaIndividualDeAjusteTypeReemplazar Reemplazar {
		get {
			return this.reemplazarField;
		}
		set {
			this.reemplazarField = value;
		}
	}
    
	/// <comentarios/>
	public NominaIndividualDeAjusteTypeEliminar Eliminar {
		get {
			return this.eliminarField;
		}
		set {
			this.eliminarField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public string SchemaLocation {
		get {
			return this.schemaLocationField;
		}
		set {
			this.schemaLocationField = value;
		}
	}
}