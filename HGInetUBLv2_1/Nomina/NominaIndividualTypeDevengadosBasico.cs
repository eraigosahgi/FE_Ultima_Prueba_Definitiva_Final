/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="dian:gov:co:facturaelectronica:NominaIndividual")]
public partial class NominaIndividualTypeDevengadosBasico {
    
	private string diasTrabajadosField;
    
	private decimal sueldoTrabajadoField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(DataType="integer")]
	public string DiasTrabajados {
		get {
			return this.diasTrabajadosField;
		}
		set {
			this.diasTrabajadosField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public decimal SueldoTrabajado {
		get {
			return this.sueldoTrabajadoField;
		}
		set {
			this.sueldoTrabajadoField = value;
		}
	}
}