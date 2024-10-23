/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("Attachment", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class AttachmentType {
    
	private EmbeddedDocumentBinaryObjectType embeddedDocumentBinaryObjectField;
    
	private ExternalReferenceType externalReferenceField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public EmbeddedDocumentBinaryObjectType EmbeddedDocumentBinaryObject {
		get {
			return this.embeddedDocumentBinaryObjectField;
		}
		set {
			this.embeddedDocumentBinaryObjectField = value;
		}
	}
    
	/// <comentarios/>
	public ExternalReferenceType ExternalReference {
		get {
			return this.externalReferenceField;
		}
		set {
			this.externalReferenceField = value;
		}
	}
}