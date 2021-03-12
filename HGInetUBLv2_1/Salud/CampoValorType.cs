using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1
{
	public partial class CampoValorType
	{
		private Name nameField;
		private Value valueField;

		/// <comentarios/>
		//[System.Xml.Serialization.XmlTextAttribute()]
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
