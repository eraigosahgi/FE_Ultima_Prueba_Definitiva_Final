using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.w3.org/2000/09/xmldsig#")]
	[System.Xml.Serialization.XmlRootAttribute("X509Data", Namespace="http://www.w3.org/2000/09/xmldsig#", IsNullable=false)]
	public partial class X509DataType {
        
		private object[] itemsField;
        
		private ItemsChoiceType[] itemsElementNameField;
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		[System.Xml.Serialization.XmlElementAttribute("X509CRL", typeof(byte[]), DataType="base64Binary")]
		[System.Xml.Serialization.XmlElementAttribute("X509Certificate", typeof(byte[]), DataType="base64Binary")]
		[System.Xml.Serialization.XmlElementAttribute("X509IssuerSerial", typeof(X509IssuerSerialType))]
		[System.Xml.Serialization.XmlElementAttribute("X509SKI", typeof(byte[]), DataType="base64Binary")]
		[System.Xml.Serialization.XmlElementAttribute("X509SubjectName", typeof(string))]
		[System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
		public object[] Items {
			get {
				return this.itemsField;
			}
			set {
				this.itemsField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public ItemsChoiceType[] ItemsElementName {
			get {
				return this.itemsElementNameField;
			}
			set {
				this.itemsElementNameField = value;
			}
		}
	}
}