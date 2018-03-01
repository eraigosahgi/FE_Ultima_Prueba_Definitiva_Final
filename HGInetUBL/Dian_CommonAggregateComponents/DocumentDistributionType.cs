using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("DocumentDistribution", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
	public partial class DocumentDistributionType {
        
		private PrintQualifierType printQualifierField;
        
		private MaximumCopiesNumericType maximumCopiesNumericField;
        
		private PartyType partyField;
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public PrintQualifierType PrintQualifier {
			get {
				return this.printQualifierField;
			}
			set {
				this.printQualifierField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public MaximumCopiesNumericType MaximumCopiesNumeric {
			get {
				return this.maximumCopiesNumericField;
			}
			set {
				this.maximumCopiesNumericField = value;
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
	}
}