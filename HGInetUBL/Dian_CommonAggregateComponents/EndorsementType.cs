using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("EmbassyEndorsement", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
	public partial class EndorsementType {
        
		private DocumentIDType documentIDField;
        
		private ApprovalStatusType approvalStatusField;
        
		private RemarksType[] remarksField;
        
		private EndorserPartyType endorserPartyField;
        
		private SignatureType[] signatureField;
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public DocumentIDType DocumentID {
			get {
				return this.documentIDField;
			}
			set {
				this.documentIDField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public ApprovalStatusType ApprovalStatus {
			get {
				return this.approvalStatusField;
			}
			set {
				this.approvalStatusField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("Remarks", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public RemarksType[] Remarks {
			get {
				return this.remarksField;
			}
			set {
				this.remarksField = value;
			}
		}
        
		/// <comentarios/>
		public EndorserPartyType EndorserParty {
			get {
				return this.endorserPartyField;
			}
			set {
				this.endorserPartyField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("Signature")]
		public SignatureType[] Signature {
			get {
				return this.signatureField;
			}
			set {
				this.signatureField = value;
			}
		}
	}
}