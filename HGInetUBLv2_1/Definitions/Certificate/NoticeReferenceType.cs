/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://uri.etsi.org/01903/v1.3.2#")]
public partial class NoticeReferenceType {
    
	private string organizationField;
    
	private string[] noticeNumbersField;
    
	/// <comentarios/>
	public string Organization {
		get {
			return this.organizationField;
		}
		set {
			this.organizationField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlArrayItemAttribute("int", DataType="integer", IsNullable=false)]
	public string[] NoticeNumbers {
		get {
			return this.noticeNumbersField;
		}
		set {
			this.noticeNumbersField = value;
		}
	}
}