using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades.Enumerables.DatosFijos
{
	
	public enum CapturaPorcentajeDescuento
	{
		[Description("No")]
		No = 0,

		[Description("Si")]
		Si = 1,

		[Description("Modifica")]
		Modifica = 2,
	}
}
