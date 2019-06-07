/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("AccountingSupplierParty", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class SupplierPartyType {
    
	private CustomerAssignedAccountIDType customerAssignedAccountIDField;
    
	private AdditionalAccountIDType[] additionalAccountIDField;
    
	private DataSendingCapabilityType dataSendingCapabilityField;
    
	private PartyType partyField;
    
	private ContactType despatchContactField;
    
	private ContactType accountingContactField;
    
	private ContactType sellerContactField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public CustomerAssignedAccountIDType CustomerAssignedAccountID {
		get {
			return this.customerAssignedAccountIDField;
		}
		set {
			this.customerAssignedAccountIDField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("AdditionalAccountID", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public AdditionalAccountIDType[] AdditionalAccountID {
		get {
			return this.additionalAccountIDField;
		}
		set {
			this.additionalAccountIDField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public DataSendingCapabilityType DataSendingCapability {
		get {
			return this.dataSendingCapabilityField;
		}
		set {
			this.dataSendingCapabilityField = value;
		}
	}
    
	/// <comentarios/>
	public PartyType Party {
		get {
			return this.partyField;
		}
		set {
			this.partyField = value;
		}
	}
    
	/// <comentarios/>
	public ContactType DespatchContact {
		get {
			return this.despatchContactField;
		}
		set {
			this.despatchContactField = value;
		}
	}
    
	/// <comentarios/>
	public ContactType AccountingContact {
		get {
			return this.accountingContactField;
		}
		set {
			this.accountingContactField = value;
		}
	}
    
	/// <comentarios/>
	public ContactType SellerContact {
		get {
			return this.sellerContactField;
		}
		set {
			this.sellerContactField = value;
		}
	}
}