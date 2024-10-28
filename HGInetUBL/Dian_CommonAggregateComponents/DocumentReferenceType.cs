using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("AdditionalDocumentReference", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
	public partial class DocumentReferenceType {
        
		private IDType idField;
        
		private CopyIndicatorType copyIndicatorField;
        
		private UUIDType uUIDField;
        
		private IssueDateType issueDateField;
        
		private DocumentTypeCodeType documentTypeCodeField;
        
		private DocumentTypeType documentTypeField;
        
		private XPathType[] xPathField;
        
		private AttachmentType attachmentField;
        
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
		public CopyIndicatorType CopyIndicator {
			get {
				return this.copyIndicatorField;
			}
			set {
				this.copyIndicatorField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public UUIDType UUID {
			get {
				return this.uUIDField;
			}
			set {
				this.uUIDField = value;
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
		public DocumentTypeCodeType DocumentTypeCode {
			get {
				return this.documentTypeCodeField;
			}
			set {
				this.documentTypeCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public DocumentTypeType DocumentType {
			get {
				return this.documentTypeField;
			}
			set {
				this.documentTypeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("XPath", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public XPathType[] XPath {
			get {
				return this.xPathField;
			}
			set {
				this.xPathField = value;
			}
		}
        
		/// <comentarios/>
		public AttachmentType Attachment {
			get {
				return this.attachmentField;
			}
			set {
				this.attachmentField = value;
			}
		}
	}
}