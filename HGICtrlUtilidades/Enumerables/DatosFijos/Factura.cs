using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public enum Factura
	{
		[Description("Ninguna")]
		Ninguna = 0,

		[Description("Factura")]
		Factura = 1,

		[Description("Pruebas DIAN")]
		PruebasDIAN = 2,
	}
}
