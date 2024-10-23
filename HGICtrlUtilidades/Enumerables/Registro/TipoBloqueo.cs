using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public enum TipoBloqueo
	{
		[Description("Ningúno")]
		Ninguno = 0,

		[Description("Licencia")]
		Licencia = 1,

		[Description("Contrato")]
		Contrato = 2,

		[Description("Estación")]
		Estacion = 3,
	}
}
