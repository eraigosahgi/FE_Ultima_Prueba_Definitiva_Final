using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.Enumerables
{
	public enum PeriodoNomina
	{
		[Description("Semanal")]
		[AmbientValue("0")]
		Semanal = 1,

		[Description("Decenal")]
		[AmbientValue("10")]
		Decenal = 2,

		[Description("Catorcenal")]
		[AmbientValue("0")]
		Catorcenal = 3,

		[Description("Quincenal")]
		[AmbientValue("15")]
		Quincenal = 4,

		[Description("Mensual")]
		[AmbientValue("30")]
		Mensual = 5,

		[Description("Otro")]
		[AmbientValue("0")]
		Otro = 6
	}
}
