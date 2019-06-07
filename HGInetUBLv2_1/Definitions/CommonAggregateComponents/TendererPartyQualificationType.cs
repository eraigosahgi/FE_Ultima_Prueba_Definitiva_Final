/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("TendererPartyQualification", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class TendererPartyQualificationType {
    
	private ProcurementProjectLotType[] interestedProcurementProjectLotField;
    
	private QualifyingPartyType mainQualifyingPartyField;
    
	private QualifyingPartyType[] additionalQualifyingPartyField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("InterestedProcurementProjectLot")]
	public ProcurementProjectLotType[] InterestedProcurementProjectLot {
		get {
			return this.interestedProcurementProjectLotField;
		}
		set {
			this.interestedProcurementProjectLotField = value;
		}
	}
    
	/// <comentarios/>
	public QualifyingPartyType MainQualifyingParty {
		get {
			return this.mainQualifyingPartyField;
		}
		set {
			this.mainQualifyingPartyField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("AdditionalQualifyingParty")]
	public QualifyingPartyType[] AdditionalQualifyingParty {
		get {
			return this.additionalQualifyingPartyField;
		}
		set {
			this.additionalQualifyingPartyField = value;
		}
	}
}