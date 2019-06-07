/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="dian:gov:co:facturaelectronica:Structures-2-1")]
public partial class AuthrorizedInvoices {
    
	private string prefixField;
    
	private long fromField;
    
	private long toField;
    
	/// <comentarios/>
	public string Prefix {
		get {
			return this.prefixField;
		}
		set {
			this.prefixField = value;
		}
	}
    
	/// <comentarios/>
	public long From {
		get {
			return this.fromField;
		}
		set {
			this.fromField = value;
		}
	}
    
	/// <comentarios/>
	public long To {
		get {
			return this.toField;
		}
		set {
			this.toField = value;
		}
	}
}