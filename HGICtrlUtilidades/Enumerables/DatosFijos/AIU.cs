using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public enum AIU
	{
		[Description("Ninguno")]
		Ninguno = 0,

		[Description("AIU")]
		AIU = 1,

		[Description("Administracion")]
		Administracion = 2,

		[Description("Imprevistos")]
		Imprevistos = 3,

		[Description("Bolsa")]
		Bolsa = 4,
	}
}