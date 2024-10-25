using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1
{
	//[System.Xml.Serialization.XmlRootAttribute("PuntoVenta", Namespace = "co:facturaelectronica:ccce:PuntoVenta", IsNullable = false)]
	public class PuntoVenta
	{
		private InformacionCajaVenta InformacionCajaVentaField;

		public InformacionCajaVenta InformacionCajaVenta
		{
			get
			{
				return this.InformacionCajaVentaField;
			}
			set
			{
				this.InformacionCajaVentaField = value;
			}
		}
	}
}
