using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public enum InterfazAdmTercero
	{
		[Description("Generico")]
		Generico = 0,

		[Description("Tercero")]
		Tercero = 1,

		[Description("TerceroDet2")]
		TerceroDet2 = 2,

		[Description("TerceroDet")]
		TerceroDet = 3,

		[Description("TerceroAux")]
		TerceroAux = 4,

		[Description("TerceroBanco")]
		TerceroBanco = 5,

	}
}