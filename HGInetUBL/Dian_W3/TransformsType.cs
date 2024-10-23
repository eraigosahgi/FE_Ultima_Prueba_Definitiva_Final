using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.w3.org/2000/09/xmldsig#")]
	[System.Xml.Serialization.XmlRootAttribute("Transforms", Namespace="http://www.w3.org/2000/09/xmldsig#", IsNullable=false)]
	public partial class TransformsType {
        
		private TransformType[] transformField;
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("Transform")]
		public TransformType[] Transform {
			get {
				return this.transformField;
			}
			set {
				this.transformField = value;
			}
		}
	}
}