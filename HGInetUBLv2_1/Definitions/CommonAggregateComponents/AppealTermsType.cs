/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("AppealTerms", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class AppealTermsType {
    
	private DescriptionType[] descriptionField;
    
	private PeriodType presentationPeriodField;
    
	private PartyType appealInformationPartyField;
    
	private PartyType appealReceiverPartyField;
    
	private PartyType mediationPartyField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("Description", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public DescriptionType[] Description {
		get {
			return this.descriptionField;
		}
		set {
			this.descriptionField = value;
		}
	}
    
	/// <comentarios/>
	public PeriodType PresentationPeriod {
		get {
			return this.presentationPeriodField;
		}
		set {
			this.presentationPeriodField = value;
		}
	}
    
	/// <comentarios/>
	public PartyType AppealInformationParty {
		get {
			return this.appealInformationPartyField;
		}
		set {
			this.appealInformationPartyField = value;
		}
	}
    
	/// <comentarios/>
	public PartyType AppealReceiverParty {
		get {
			return this.appealReceiverPartyField;
		}
		set {
			this.appealReceiverPartyField = value;
		}
	}
    
	/// <comentarios/>
	public PartyType MediationParty {
		get {
			return this.mediationPartyField;
		}
		set {
			this.mediationPartyField = value;
		}
	}
}