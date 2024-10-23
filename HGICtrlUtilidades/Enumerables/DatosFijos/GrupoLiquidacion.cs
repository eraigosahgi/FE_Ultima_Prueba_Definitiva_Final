using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public enum GrupoLiquidacion
	{
		[Description("Salarios")]
		Salarios = 1,

		[Description("Horas extras")]
		HorasExtras = 2,

		[Description("Honorarios")]
		Honorarios = 3,

		[Description("Servicios")]
		Servicios = 4,

		[Description("Comisiones")]
		Comisiones = 5,

		[Description("Prestaciones sociales")]
		PrestacionesSociales = 6,

		[Description("Viaticos")]
		Viaticos = 7,

		[Description("Cesantias")]
		Cesantias = 8,

		[Description("Otros ingresos")]
		OtrosIngresos = 9,

		[Description("Gastos de representación")]
		GastosRepresentacion = 10,

		[Description("Aportes salud")]
		AportesSalud = 11,

		[Description("Aportes Obl pension")]
		AportesOblPension = 12,

		[Description("Aportes Vol pension")]
		AportesVolPension = 13,

		[Description("ReteFuente")]
		ReteFuente = 14,

		[Description("compensaciones")]
		compensaciones = 15,

		[Description("Pensiones de jubilación")]
		PensionesJubilacion = 16,

		[Description("Otras deducciones")]
		OtrasDeducciones = 19,

		[Description("Aportes a cuentas AFC")]
		AportesCuentasAFC = 20,

		[Description("No Aplica")]
		NoAplica = 99,

	}
}