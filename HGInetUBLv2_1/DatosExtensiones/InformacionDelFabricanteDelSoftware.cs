using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HGInetUBLv2_1
{
	//[System.Xml.Serialization.XmlRootAttribute("InformacionDelFabricanteDelSoftware")]
	public class InformacionDelFabricanteDelSoftware  
	{
		private Name nameField;
		private Value valueField;
		
		/// <comentarios/>
		//[System.Xml.Serialization.XmlTextAttribute()]
		//[XmlElement(ElementName = "Name")]
		public Name Name
		{
			get
			{
				return this.nameField;
			}
			set
			{
				this.nameField = value;
			}
		}

		/// <comentarios/>
		//[System.Xml.Serialization.XmlTextAttribute()]
		public Value Value
		{
			get
			{
				return this.valueField;
			}
			set
			{
				this.valueField = value;
			}
		}

	   


	}
}
