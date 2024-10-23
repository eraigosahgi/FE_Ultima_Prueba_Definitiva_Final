using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public enum TipoIva
	{
		[Description("No Aplica")]
		NoAplica = 0,

		[Description("Detalle")]
		Detalle = 1,

		[Description("SubTotal")]
		SubTotal = 2,

		[Description("Valor 1")]
		Valor1 = 3, 
	}
}
