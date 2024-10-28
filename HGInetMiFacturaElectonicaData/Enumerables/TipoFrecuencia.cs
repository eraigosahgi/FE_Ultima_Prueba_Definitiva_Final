using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.Enumerables
{
	public enum TipoFrecuencia
	{
		[Description("Hoy")]
		Hoy = 1,

		[Description("Fecha")]
		Fecha = 2,

		[Description("Semana")]
		Semana = 3,

		[Description("Mes")]
		Mes = 4,

		[Description("Año")]
		Anyo = 5,

		[Description("Rango")]
		Rango = 6,



	}
}
