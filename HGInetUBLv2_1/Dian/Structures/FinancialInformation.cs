/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="dian:gov:co:facturaelectronica:Structures-2-1")]
public partial class FinancialInformation {
    
	private PartyType assigneeField;
    
	private FinancialAccountType paymentDetailsField;
    
	private TextType1 clauseField;
    
	private DocumentReferenceType[] documentReferenceField;
    
	/// <comentarios/>
	public PartyType Assignee {
		get {
			return this.assigneeField;
		}
		set {
			this.assigneeField = value;
		}
	}
    
	/// <comentarios/>
	public FinancialAccountType PaymentDetails {
		get {
			return this.paymentDetailsField;
		}
		set {
			this.paymentDetailsField = value;
		}
	}
    
	/// <comentarios/>
	public TextType1 Clause {
		get {
			return this.clauseField;
		}
		set {
			this.clauseField = value;
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
}