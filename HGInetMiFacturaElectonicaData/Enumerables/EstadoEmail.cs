using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData
{
	public enum EstadoEmail
	{
		[Description("Envío Fallido")]
		Fallido = 0,

		[Description("Envío Exitoso")]
		Exitoso = 1,
	}
}
