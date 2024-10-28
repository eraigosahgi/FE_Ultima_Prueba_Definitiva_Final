/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="dian:gov:co:facturaelectronica:Structures-2-1")]
public partial class SoftwareProvider {
    
	private coID2Type providerIDField;
    
	private IdentifierType1 softwareIDField;
    
	/// <comentarios/>
	public coID2Type ProviderID {
		get {
			return this.providerIDField;
		}
		set {
			this.providerIDField = value;
		}
	}
    
	/// <comentarios/>
	public IdentifierType1 SoftwareID {
		get {
			return this.softwareIDField;
		}
		set {
			this.softwareIDField = value;
		}
	}
}