using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades.Enumerables.DatosFijos
{
	public enum ControlUvt
	{
		[Description("No Aplica")]
		NoAplica = 0,

		[Description("Alerta")]
		Alerta = 1,

		[Description("Bloquea")]
		Bloquea = 2,
	}
}
