using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData
{

	public enum TipoHoraNomina
	{
		[Description("HED")]
		[AmbientValue("0.25")]
		ExtraDiurna = 1,

		[Description("HEN")]
		[AmbientValue("0.75")]
		ExtraNocturna = 2,

		[Description("HRN")]
		[AmbientValue("0.35")]
		RecargoNocturno = 3,

		[Description("HEDDF")]
		[AmbientValue("1.00")]
		DominicalesFestivas = 4,

		[Description("HRDDF")]
		[AmbientValue("0.75")]
		RecargoDominicalesFestivas = 5,

		[Description("HENDF")]
		[AmbientValue("1.50")]
		ExtraDominicalesFestivasNocturnas = 6,

		[Description("HRNDF")]
		[AmbientValue("1.10")]
		RecargoDominicalesFestivasNocturnas = 7

	}
}
