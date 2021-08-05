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
		[AmbientValue("25.00")]
		ExtraDiurna = 1,

		[Description("HEN")]
		[AmbientValue("75.00")]
		ExtraNocturna = 2,

		[Description("HRN")]
		[AmbientValue("35.00")]
		RecargoNocturno = 3,

		[Description("HEDDF")]
		[AmbientValue("100.00")]
		DominicalesFestivas = 4,

		[Description("HRDDF")]
		[AmbientValue("75.00")]
		RecargoDominicalesFestivas = 5,

		[Description("HENDF")]
		[AmbientValue("150.00")]
		ExtraDominicalesFestivasNocturnas = 6,

		[Description("HRNDF")]
		[AmbientValue("110.00")]
		RecargoDominicalesFestivasNocturnas = 7

	}
}
