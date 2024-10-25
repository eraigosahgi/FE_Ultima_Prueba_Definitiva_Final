using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("LineResponse", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
	public partial class LineResponseType {
        
		private LineReferenceType lineReferenceField;
        
		private ResponseType[] responseField;
        
		/// <comentarios/>
		public LineReferenceType LineReference {
			get {
				return this.lineReferenceField;
			}
			set {
				this.lineReferenceField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("Response")]
		public ResponseType[] Response {
			get {
				return this.responseField;
			}
			set {
				this.responseField = value;
			}
		}
	}
}