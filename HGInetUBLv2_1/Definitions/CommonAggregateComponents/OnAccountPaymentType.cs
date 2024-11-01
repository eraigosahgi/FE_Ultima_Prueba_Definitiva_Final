﻿/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("MainOnAccountPayment", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class OnAccountPaymentType {
    
	private EstimatedConsumedQuantityType estimatedConsumedQuantityField;
    
	private NoteType[] noteField;
    
	private PaymentTermsType[] paymentTermsField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public EstimatedConsumedQuantityType EstimatedConsumedQuantity {
		get {
			return this.estimatedConsumedQuantityField;
		}
		set {
			this.estimatedConsumedQuantityField = value;
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
	[System.Xml.Serialization.XmlElementAttribute("PaymentTerms")]
	public PaymentTermsType[] PaymentTerms {
		get {
			return this.paymentTermsField;
		}
		set {
			this.paymentTermsField = value;
		}
	}
}