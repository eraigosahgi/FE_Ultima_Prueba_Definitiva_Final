/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://docs.oasis-open.org/codelist/ns/genericode/1.0/")]
[System.Xml.Serialization.XmlRootAttribute("CodeListSet", Namespace="http://docs.oasis-open.org/codelist/ns/genericode/1.0/", IsNullable=false)]
public partial class CodeListSetDocument {
    
	private Annotation annotationField;
    
	private Identification identificationField;
    
	private CodeListRef[] codeListRefField;
    
	private CodeListSetDocument[] codeListSetField;
    
	private CodeListSetRef[] codeListSetRefField;
    
	private string baseField;
    
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
	[System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
	public Identification Identification {
		get {
			return this.identificationField;
		}
		set {
			this.identificationField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("CodeListRef", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
	public CodeListRef[] CodeListRef {
		get {
			return this.codeListRefField;
		}
		set {
			this.codeListRefField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("CodeListSet", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
	public CodeListSetDocument[] CodeListSet {
		get {
			return this.codeListSetField;
		}
		set {
			this.codeListSetField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("CodeListSetRef", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
	public CodeListSetRef[] CodeListSetRef {
		get {
			return this.codeListSetRefField;
		}
		set {
			this.codeListSetRefField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlAttributeAttribute(Form=System.Xml.Schema.XmlSchemaForm.Qualified, Namespace="http://www.w3.org/XML/1998/namespace")]
	public string @base {
		get {
			return this.baseField;
		}
		set {
			this.baseField = value;
		}
	}
}