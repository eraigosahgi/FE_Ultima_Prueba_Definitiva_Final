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
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("UBLExtensions", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2", IsNullable = false)]
	public partial class UBLExtensionsType
	{

		private UBLExtensionType[] uBLExtensionField;

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("UBLExtension")]
		public UBLExtensionType[] UBLExtension
		{
			get
			{
				return this.uBLExtensionField;
			}
			set
			{
				this.uBLExtensionField = value;
			}
		}
	}
}