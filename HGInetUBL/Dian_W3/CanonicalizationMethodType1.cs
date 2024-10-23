using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "CanonicalizationMethodType", Namespace = "http://www.w3.org/2000/09/xmldsig#")]
	[System.Xml.Serialization.XmlRootAttribute("CanonicalizationMethod", Namespace = "http://www.w3.org/2000/09/xmldsig#", IsNullable = false)]
	public partial class CanonicalizationMethodType1
	{

		private System.Xml.XmlNode[] anyField;

		private string algorithmField;

		/// <comentarios/>
		[System.Xml.Serialization.XmlTextAttribute()]
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlNode[] Any
		{
			get
			{
				return this.anyField;
			}
			set
			{
				this.anyField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
		public string Algorithm
		{
			get
			{
				return this.algorithmField;
			}
			set
			{
				this.algorithmField = value;
			}
		}
	}
}