using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public enum AplicativosPlataforma
	{
		[Description("General")]
		General = 0,

		[Description("Factura Electronica")]
		FE = 1,

		[Description("Smart")]
		Smart = 2,

		[Description("Tienda Virtual")]
		TiendaVirtual = 3,

		[Description("Móvil")]
		Movil = 4,

		[Description("Happgi")]
		Happgi = 5,
	}
}
