using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1
{
	[System.Xml.Serialization.XmlRootAttribute("CustomTagGeneral", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:ApplicationResponse-2", IsNullable = false)]
	public partial class CustomTagGeneral1
	{
		private InformacionNegociacionType InformacionNegociacionField;

		private ConstanciadePagosType ConstanciadePagosField;

		public InformacionNegociacionType InformacionNegociacion
		{
			get
			{
				return this.InformacionNegociacionField;
			}
			set
			{
				this.InformacionNegociacionField = value;
			}
		}

		public ConstanciadePagosType ConstanciadePagos
		{
			get
			{
				return this.ConstanciadePagosField;
			}
			set
			{
				this.ConstanciadePagosField = value;
			}
		}
	}
}
