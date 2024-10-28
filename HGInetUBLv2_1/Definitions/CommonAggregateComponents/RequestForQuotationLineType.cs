/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("RequestForQuotationLine", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class RequestForQuotationLineType {
    
	private IDType idField;
    
	private UUIDType uUIDField;
    
	private NoteType[] noteField;
    
	private OptionalLineItemIndicatorType optionalLineItemIndicatorField;
    
	private PrivacyCodeType privacyCodeField;
    
	private SecurityClassificationCodeType securityClassificationCodeField;
    
	private DocumentReferenceType[] documentReferenceField;
    
	private LineItemType lineItemField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public IDType ID {
		get {
			return this.idField;
		}
		set {
			this.idField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public UUIDType UUID {
		get {
			return this.uUIDField;
		}
		set {
			this.uUIDField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("Note", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public NoteType[] Note {
		get {
			return this.noteField;
		}
		set {
			this.noteField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public OptionalLineItemIndicatorType OptionalLineItemIndicator {
		get {
			return this.optionalLineItemIndicatorField;
		}
		set {
			this.optionalLineItemIndicatorField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public PrivacyCodeType PrivacyCode {
		get {
			return this.privacyCodeField;
		}
		set {
			this.privacyCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public SecurityClassificationCodeType SecurityClassificationCode {
		get {
			return this.securityClassificationCodeField;
		}
		set {
			this.securityClassificationCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("DocumentReference")]
	public DocumentReferenceType[] DocumentReference {
		get {
			return this.documentReferenceField;
		}
		set {
			this.documentReferenceField = value;
		}
	}
    
	/// <comentarios/>
	public LineItemType LineItem {
		get {
			return this.lineItemField;
		}
		set {
			this.lineItemField = value;
		}
	}
}