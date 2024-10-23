using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public enum FacturacionElectronica
	{
		[Description("Ninguna")]
		[AmbientValue("publico")]
		Ninguna = 0,

		[Description("Producción")]
		[AmbientValue("publico")]
		Produccion = 1,

		[Description("Habilitación")]
		[AmbientValue("publico")]
		Habilitacion = 2,

		[Description("QA")]
		[AmbientValue("privado")]
		QA = 3,
	}
}
