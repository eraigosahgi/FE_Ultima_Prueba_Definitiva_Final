using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1
{
	public partial class URLDescagaAdjuntosType
	{
		private string URLField;

		private ParametrosArgumentosType[] ParametroArgumentosField;

		/// <comentarios/>
		//[System.Xml.Serialization.XmlAttributeAttribute()]
		public string URL
		{
			get
			{
				return this.URLField;
			}
			set
			{
				this.URLField = value;
			}
		}

		public ParametrosArgumentosType[] ParametroArgumentos
		{
			get
			{
				return this.ParametroArgumentosField;
			}
			set
			{
				this.ParametroArgumentosField = value;
			}
		}
	}
}
