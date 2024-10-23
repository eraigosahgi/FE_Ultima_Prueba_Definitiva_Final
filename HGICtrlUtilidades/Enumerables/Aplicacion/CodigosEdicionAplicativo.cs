using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public enum CodigosEdicionAplicativo
	{
		[Description("Edición Factura")]
		EdicionFactura = -1,

		[Description("Edición Express")]
		EdicionExpress = 0,

		[Description("Edición Básica")]
		EdicionBasica = 1,

		[Description("Edición Estándar")]
		EdicionEstandar = 2,

		[Description("Edición Avanzada")]
		EdicionAvanzada = 3,

		[Description("Edición Contador")]
		EdicionContador = 4,
	}
}
