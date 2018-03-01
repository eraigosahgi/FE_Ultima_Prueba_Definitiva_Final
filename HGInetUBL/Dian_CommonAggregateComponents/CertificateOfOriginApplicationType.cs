using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("CertificateOfOriginApplication", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
	public partial class CertificateOfOriginApplicationType {
        
		private ReferenceIDType referenceIDField;
        
		private CertificateTypeType certificateTypeField;
        
		private ApplicationStatusCodeType applicationStatusCodeField;
        
		private OriginalJobIDType originalJobIDField;
        
		private PreviousJobIDType previousJobIDField;
        
		private RemarksType remarksField;
        
		private ShipmentType shipmentField;
        
		private EndorserPartyType[] endorserPartyField;
        
		private PartyType preparationPartyField;
        
		private PartyType issuerPartyField;
        
		private CountryType issuingCountryField;
        
		private DocumentDistributionType[] documentDistributionField;
        
		private DocumentReferenceType[] supportingDocumentReferenceField;
        
		private SignatureType[] signatureField;
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public ReferenceIDType ReferenceID {
			get {
				return this.referenceIDField;
			}
			set {
				this.referenceIDField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public CertificateTypeType CertificateType {
			get {
				return this.certificateTypeField;
			}
			set {
				this.certificateTypeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public ApplicationStatusCodeType ApplicationStatusCode {
			get {
				return this.applicationStatusCodeField;
			}
			set {
				this.applicationStatusCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public OriginalJobIDType OriginalJobID {
			get {
				return this.originalJobIDField;
			}
			set {
				this.originalJobIDField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public PreviousJobIDType PreviousJobID {
			get {
				return this.previousJobIDField;
			}
			set {
				this.previousJobIDField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public RemarksType Remarks {
			get {
				return this.remarksField;
			}
			set {
				this.remarksField = value;
			}
		}
        
		/// <comentarios/>
		public ShipmentType Shipment {
			get {
				return this.shipmentField;
			}
			set {
				this.shipmentField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("EndorserParty")]
		public EndorserPartyType[] EndorserParty {
			get {
				return this.endorserPartyField;
			}
			set {
				this.endorserPartyField = value;
			}
		}
        
		/// <comentarios/>
		public PartyType PreparationParty {
			get {
				return this.preparationPartyField;
			}
			set {
				this.preparationPartyField = value;
			}
		}
        
		/// <comentarios/>
		public PartyType IssuerParty {
			get {
				return this.issuerPartyField;
			}
			set {
				this.issuerPartyField = value;
			}
		}
        
		/// <comentarios/>
		public CountryType IssuingCountry {
			get {
				return this.issuingCountryField;
			}
			set {
				this.issuingCountryField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("DocumentDistribution")]
		public DocumentDistributionType[] DocumentDistribution {
			get {
				return this.documentDistributionField;
			}
			set {
				this.documentDistributionField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("SupportingDocumentReference")]
		public DocumentReferenceType[] SupportingDocumentReference {
			get {
				return this.supportingDocumentReferenceField;
			}
			set {
				this.supportingDocumentReferenceField = value;
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