using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public enum TiposEntrega
	{
		[Description("Ninguno")]
		Ninguno = 0,

		[Description("Recoger en Tienda")]
		RecogerTienda = 1,

		[Description("Envío a Domicilio")]
		EnvioDomicilio = 2,
	}
}
