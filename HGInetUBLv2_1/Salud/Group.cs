using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1
{

	public partial class Group
	{

		private string schemeNameField;

		private Collection[] ColletionField;

		/// <comentarios/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string schemeName
		{
			get
			{
				return this.schemeNameField;
			}
			set
			{
				this.schemeNameField = value;
			}
		}

		/// <comentarios/>
		//[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public Collection[] Collection
		{
			get
			{
				return this.ColletionField;
			}
			set
			{
				this.ColletionField = value;
			}
		}
	}
}
