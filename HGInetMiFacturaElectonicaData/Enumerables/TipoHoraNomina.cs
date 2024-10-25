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

	public enum TipoHoraNominaPor
	{
		[Description("1")]
		ExtraDiurna = 25,

		[Description("2")]
		ExtraNocturna = 75,

		[Description("3")]
		RecargoNocturno = 35,

		[Description("4")]
		DominicalesFestivas = 100,

		[Description("5")]
		RecargoDominicalesFestivas = 75,

		[Description("6")]
		ExtraDominicalesFestivasNocturnas = 150,

		[Description("7")]
		RecargoDominicalesFestivasNocturnas = 110

	}
}
