using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.w3.org/2000/09/xmldsig#")]
	[System.Xml.Serialization.XmlRootAttribute("KeyValue", Namespace="http://www.w3.org/2000/09/xmldsig#", IsNullable=false)]
	public partial class KeyValueType {
        
		private object itemField;
        
		private string[] textField;
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		[System.Xml.Serialization.XmlElementAttribute("DSAKeyValue", typeof(DSAKeyValueType))]
		[System.Xml.Serialization.XmlElementAttribute("RSAKeyValue", typeof(RSAKeyValueType))]
		public object Item {
			get {
				return this.itemField;
			}
			set {
				this.itemField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlTextAttribute()]
		public string[] Text {
			get {
				return this.textField;
			}
			set {
				this.textField = value;
			}
		}
	}
}