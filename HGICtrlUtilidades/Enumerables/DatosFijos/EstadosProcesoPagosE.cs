using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public enum EstadosProcesoPagosE
	{
		[Description("Todos")]
		Todos = 0,

		[Description("Procesados")]
		Procesados = 1,

		[Description("Pendientes")]
		Pendientes = 2,
	}
}
