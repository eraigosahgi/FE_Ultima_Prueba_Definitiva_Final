/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://docs.oasis-open.org/codelist/ns/ContextValueAssociation/1.0/")]
public partial class InstanceMetadataSets {
    
	private Annotation annotationField;
    
	private InstanceMetadataSet[] instanceMetadataSetField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
	public Annotation Annotation {
		get {
			return this.annotationField;
		}
		set {
			this.annotationField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("InstanceMetadataSet", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
	public InstanceMetadataSet[] InstanceMetadataSet {
		get {
			return this.instanceMetadataSetField;
		}
		set {
			this.instanceMetadataSetField = value;
		}
	}
}