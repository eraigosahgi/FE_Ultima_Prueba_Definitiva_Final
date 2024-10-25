/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("ServiceProviderParty", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class ServiceProviderPartyType {
    
	private IDType idField;
    
	private ServiceTypeCodeType serviceTypeCodeField;
    
	private ServiceTypeType[] serviceTypeField;
    
	private PartyType partyField;
    
	private ContactType sellerContactField;
    
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
	public ServiceTypeCodeType ServiceTypeCode {
		get {
			return this.serviceTypeCodeField;
		}
		set {
			this.serviceTypeCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("ServiceType", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public ServiceTypeType[] ServiceType {
		get {
			return this.serviceTypeField;
		}
		set {
			this.serviceTypeField = value;
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
	public ContactType SellerContact {
		get {
			return this.sellerContactField;
		}
		set {
			this.sellerContactField = value;
		}
	}
}