﻿/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("AwardedTenderedProject", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class TenderedProjectType {
    
	private VariantIDType variantIDField;
    
	private FeeAmountType feeAmountField;
    
	private FeeDescriptionType[] feeDescriptionField;
    
	private TenderEnvelopeIDType tenderEnvelopeIDField;
    
	private TenderEnvelopeTypeCodeType tenderEnvelopeTypeCodeField;
    
	private ProcurementProjectLotType procurementProjectLotField;
    
	private DocumentReferenceType[] evidenceDocumentReferenceField;
    
	private TaxTotalType[] taxTotalField;
    
	private MonetaryTotalType legalMonetaryTotalField;
    
	private TenderLineType[] tenderLineField;
    
	private AwardingCriterionResponseType[] awardingCriterionResponseField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public VariantIDType VariantID {
		get {
			return this.variantIDField;
		}
		set {
			this.variantIDField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public FeeAmountType FeeAmount {
		get {
			return this.feeAmountField;
		}
		set {
			this.feeAmountField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("FeeDescription", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public FeeDescriptionType[] FeeDescription {
		get {
			return this.feeDescriptionField;
		}
		set {
			this.feeDescriptionField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public TenderEnvelopeIDType TenderEnvelopeID {
		get {
			return this.tenderEnvelopeIDField;
		}
		set {
			this.tenderEnvelopeIDField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public TenderEnvelopeTypeCodeType TenderEnvelopeTypeCode {
		get {
			return this.tenderEnvelopeTypeCodeField;
		}
		set {
			this.tenderEnvelopeTypeCodeField = value;
		}
	}
    
	/// <comentarios/>
	public ProcurementProjectLotType ProcurementProjectLot {
		get {
			return this.procurementProjectLotField;
		}
		set {
			this.procurementProjectLotField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("EvidenceDocumentReference")]
	public DocumentReferenceType[] EvidenceDocumentReference {
		get {
			return this.evidenceDocumentReferenceField;
		}
		set {
			this.evidenceDocumentReferenceField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("TaxTotal")]
	public TaxTotalType[] TaxTotal {
		get {
			return this.taxTotalField;
		}
		set {
			this.taxTotalField = value;
		}
	}
    
	/// <comentarios/>
	public MonetaryTotalType LegalMonetaryTotal {
		get {
			return this.legalMonetaryTotalField;
		}
		set {
			this.legalMonetaryTotalField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("TenderLine")]
	public TenderLineType[] TenderLine {
		get {
			return this.tenderLineField;
		}
		set {
			this.tenderLineField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("AwardingCriterionResponse")]
	public AwardingCriterionResponseType[] AwardingCriterionResponse {
		get {
			return this.awardingCriterionResponseField;
		}
		set {
			this.awardingCriterionResponseField = value;
		}
	}
}