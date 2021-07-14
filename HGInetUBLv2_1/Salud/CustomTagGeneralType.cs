using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1
{
	[System.Xml.Serialization.XmlRootAttribute("CustomTagGeneral", Namespace = "co:facturaelectronica:ccce:CustomTagGeneral", IsNullable = false)]
	public partial class CustomTagGeneral
	{
		private Name nameField;

		private Value valueField;

		private InteroperabilidadType interoperabilidadField;

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

		public InteroperabilidadType Interoperabilidad
		{
			get
			{
				return this.interoperabilidadField;
			}
			set
			{
				this.interoperabilidadField = value;
			}
		}

	}
}
