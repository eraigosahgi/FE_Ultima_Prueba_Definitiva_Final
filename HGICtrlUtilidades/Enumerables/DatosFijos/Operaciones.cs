using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public enum Operaciones
	{
		[Description("Suma")]
		Suma = 1,

		[Description("Resta")]
		Resta = -1,

		[Description("Ninguno")]
		Ninguno = 0,

	}
}
