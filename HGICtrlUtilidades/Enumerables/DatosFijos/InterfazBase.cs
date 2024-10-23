using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public enum InterfazBase
	{
		[Description("")]
		Vacio = 0,

		[Description("SubTotal")]
		SubTotal = 1,

		[Description("Iva")]
		Iva = 2,

		[Description("Base")]
		Base = 3,

		[Description("Fletes")]
		Fletes = 4,

		[Description("Base2")]
		Base2 = 5,

		[Description("Valor P1")]
		ValorP1 = 6
	}
}