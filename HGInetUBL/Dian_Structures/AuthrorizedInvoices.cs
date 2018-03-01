using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.dian.gov.co/contratos/facturaelectronica/v1/Structures")]
	public partial class AuthrorizedInvoices {
        
		private TextType prefixField;
        
		private int fromField;
        
		private int toField;
        
		/// <comentarios/>
		public TextType Prefix {
			get {
				return this.prefixField;
			}
			set {
				this.prefixField = value;
			}
		}
        
		/// <comentarios/>
		public int From {
			get {
				return this.fromField;
			}
			set {
				this.fromField = value;
			}
		}
        
		/// <comentarios/>
		public int To {
			get {
				return this.toField;
			}
			set {
				this.toField = value;
			}
		}
	}
}