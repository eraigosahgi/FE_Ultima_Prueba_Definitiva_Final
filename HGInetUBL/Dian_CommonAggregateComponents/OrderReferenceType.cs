using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("OrderReference", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
	public partial class OrderReferenceType {
        
		private IDType idField;
        
		private SalesOrderIDType salesOrderIDField;
        
		private CopyIndicatorType copyIndicatorField;
        
		private UUIDType uUIDField;
        
		private IssueDateType issueDateField;
        
		private IssueTimeType issueTimeField;
        
		private CustomerReferenceType customerReferenceField;
        
		private DocumentReferenceType documentReferenceField;
        
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
		public SalesOrderIDType SalesOrderID {
			get {
				return this.salesOrderIDField;
			}
			set {
				this.salesOrderIDField = value;
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
		public CustomerReferenceType CustomerReference {
			get {
				return this.customerReferenceField;
			}
			set {
				this.customerReferenceField = value;
			}
		}
        
		/// <comentarios/>
		public DocumentReferenceType DocumentReference {
			get {
				return this.documentReferenceField;
			}
			set {
				this.documentReferenceField = value;
			}
		}
	}
}