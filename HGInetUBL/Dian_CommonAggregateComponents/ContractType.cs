using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("Contract", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
	public partial class ContractType {
        
		private IDType idField;
        
		private IssueDateType issueDateField;
        
		private IssueTimeType issueTimeField;
        
		private ContractTypeCodeType contractTypeCodeField;
        
		private ContractTypeType contractType1Field;
        
		private PeriodType validityPeriodField;
        
		private DocumentReferenceType[] contractDocumentReferenceField;
        
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
		public IssueDateType IssueDate {
			get {
				return this.issueDateField;
			}
			set {
				this.issueDateField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public IssueTimeType IssueTime {
			get {
				return this.issueTimeField;
			}
			set {
				this.issueTimeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public ContractTypeCodeType ContractTypeCode {
			get {
				return this.contractTypeCodeField;
			}
			set {
				this.contractTypeCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("ContractType", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public ContractTypeType ContractType1 {
			get {
				return this.contractType1Field;
			}
			set {
				this.contractType1Field = value;
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
		[System.Xml.Serialization.XmlElementAttribute("ContractDocumentReference")]
		public DocumentReferenceType[] ContractDocumentReference {
			get {
				return this.contractDocumentReferenceField;
			}
			set {
				this.contractDocumentReferenceField = value;
			}
		}
	}
}