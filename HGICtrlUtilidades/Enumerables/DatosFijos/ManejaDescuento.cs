using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades.Enumerables.DatosFijos
{
	public enum ManejaDescuento
	{
		[Description("No")]
		No = 0,

		[Description("Si")]
		Si = 1,

		[Description("Solo Tercero")]
		Solo_Tercero = 2,
	}
}
