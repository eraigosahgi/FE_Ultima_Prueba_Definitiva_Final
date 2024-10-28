/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="dian:gov:co:facturaelectronica:NominaIndividual")]
public partial class NominaIndividualTypeNovedad {
    
	private string cUNENovField;

	private bool valueField;

	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public string CUNENov {
		get {
			return this.cUNENovField;
		}
		set {
			this.cUNENovField = value;
		}
	}

	/// <comentarios/>
	[System.Xml.Serialization.XmlTextAttribute()]
	public bool Value
	{
		get
		{
			return this.valueField;
		}
		set
		{
			this.valueField = value;
		}
	}
}