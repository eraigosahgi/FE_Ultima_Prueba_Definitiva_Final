using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1
{
	[System.Xml.Serialization.XmlRootAttribute("Name", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2", IsNullable = false)]
	public partial class Name : TextType
	{
		//private string valueField;

		//public string Value
		//{
		//	get
		//	{
		//		return this.valueField;
		//	}
		//	set
		//	{
		//		this.valueField = value;
		//	}
		//}
	}
}
