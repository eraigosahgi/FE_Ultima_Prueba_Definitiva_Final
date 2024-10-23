using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public enum TipoConcepto
	{
		[Description("Devengado")]
		Pago = 1,

		[Description("Deducción")]
		Deduccion = 2,
	}
}
