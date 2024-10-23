using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public enum Plazo
	{
		[Description("Del Tercero")]
		DelTercero = 0,

		[Description("Del Documento")]
		DelDocumento = 1,

		[Description("Sin Plazo")]
		SinPlazo = 2,

	}
}
