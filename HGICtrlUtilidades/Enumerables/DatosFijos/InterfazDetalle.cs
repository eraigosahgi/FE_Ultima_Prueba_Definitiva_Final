using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public enum InterfazDetalle
	{
		
		[Description("Tercero")]
		Tercero = 1,

		[Description("Concepto")]
		Concepto = 2,

		[Description("Observaciones")]
		Observaciones = 3,

		[Description("Detalle")]
		Detalle = 4,

		[Description("Nada")]
		Nada = 5
	}
}
