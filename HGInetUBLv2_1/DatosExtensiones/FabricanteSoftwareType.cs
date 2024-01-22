using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1
{
	//[System.Xml.Serialization.XmlRootAttribute("FabricanteSoftware", Namespace = "co:facturaelectronica:ccce:FabricanteSoftware", IsNullable = false)]
	public class FabricanteSoftware
	{
		private InformacionDelFabricanteDelSoftware InformacionDelFabricanteDelSoftwareField;

		public InformacionDelFabricanteDelSoftware InformacionDelFabricanteDelSoftware
		{
			get
			{
				return this.InformacionDelFabricanteDelSoftwareField;
			}
			set
			{
				this.InformacionDelFabricanteDelSoftwareField = value;
			}
		}

	}
}
