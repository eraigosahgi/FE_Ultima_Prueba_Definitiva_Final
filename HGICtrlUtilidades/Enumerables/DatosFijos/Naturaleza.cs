using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public enum Naturaleza
	{
		[Description("=")]
		Igual = 0,

		[Description("Debito")]
		Debito = 1,

		[Description("Credito")]
		Credito = 2,
	}
}