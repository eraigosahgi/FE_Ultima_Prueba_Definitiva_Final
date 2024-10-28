/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("AccountingCustomerParty", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class CustomerPartyType {
    
	private CustomerAssignedAccountIDType customerAssignedAccountIDField;
    
	private SupplierAssignedAccountIDType supplierAssignedAccountIDField;
    
	private AdditionalAccountIDType[] additionalAccountIDField;
    
	private PartyType partyField;
    
	private ContactType deliveryContactField;
    
	private ContactType accountingContactField;
    
	private ContactType buyerContactField;
    
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
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public SupplierAssignedAccountIDType SupplierAssignedAccountID {
		get {
			return this.supplierAssignedAccountIDField;
		}
		set {
			this.supplierAssignedAccountIDField = value;
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
	public PartyType Party {
		get {
			return this.partyField;
		}
		set {
			this.partyField = value;
		}
	}
    
	/// <comentarios/>
	public ContactType DeliveryContact {
		get {
			return this.deliveryContactField;
		}
		set {
			this.deliveryContactField = value;
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
	public ContactType BuyerContact {
		get {
			return this.buyerContactField;
		}
		set {
			this.buyerContactField = value;
		}
	}
}