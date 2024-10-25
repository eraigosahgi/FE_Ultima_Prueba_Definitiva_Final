/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("PaymentMandate", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class PaymentMandateType {
    
	private IDType idField;
    
	private MandateTypeCodeType mandateTypeCodeField;
    
	private MaximumPaymentInstructionsNumericType maximumPaymentInstructionsNumericField;
    
	private MaximumPaidAmountType maximumPaidAmountField;
    
	private SignatureIDType signatureIDField;
    
	private PartyType payerPartyField;
    
	private FinancialAccountType payerFinancialAccountField;
    
	private PeriodType validityPeriodField;
    
	private PeriodType paymentReversalPeriodField;
    
	private ClauseType[] clauseField;
    
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
	public MandateTypeCodeType MandateTypeCode {
		get {
			return this.mandateTypeCodeField;
		}
		set {
			this.mandateTypeCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public MaximumPaymentInstructionsNumericType MaximumPaymentInstructionsNumeric {
		get {
			return this.maximumPaymentInstructionsNumericField;
		}
		set {
			this.maximumPaymentInstructionsNumericField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public MaximumPaidAmountType MaximumPaidAmount {
		get {
			return this.maximumPaidAmountField;
		}
		set {
			this.maximumPaidAmountField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public SignatureIDType SignatureID {
		get {
			return this.signatureIDField;
		}
		set {
			this.signatureIDField = value;
		}
	}
    
	/// <comentarios/>
	public PartyType PayerParty {
		get {
			return this.payerPartyField;
		}
		set {
			this.payerPartyField = value;
		}
	}
    
	/// <comentarios/>
	public FinancialAccountType PayerFinancialAccount {
		get {
			return this.payerFinancialAccountField;
		}
		set {
			this.payerFinancialAccountField = value;
		}
	}
    
	/// <comentarios/>
	public PeriodType ValidityPeriod {
		get {
			return this.validityPeriodField;
		}
		set {
			this.validityPeriodField = value;
		}
	}
    
	/// <comentarios/>
	public PeriodType PaymentReversalPeriod {
		get {
			return this.paymentReversalPeriodField;
		}
		set {
			this.paymentReversalPeriodField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("Clause")]
	public ClauseType[] Clause {
		get {
			return this.clauseField;
		}
		set {
			this.clauseField = value;
		}
	}
}