/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("InterestedProcurementProjectLot", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class ProcurementProjectLotType {
    
	private IDType idField;
    
	private TenderingTermsType tenderingTermsField;
    
	private ProcurementProjectType procurementProjectField;
    
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
	public TenderingTermsType TenderingTerms {
		get {
			return this.tenderingTermsField;
		}
		set {
			this.tenderingTermsField = value;
		}
	}
    
	/// <comentarios/>
	public ProcurementProjectType ProcurementProject {
		get {
			return this.procurementProjectField;
		}
		set {
			this.procurementProjectField = value;
		}
	}
}