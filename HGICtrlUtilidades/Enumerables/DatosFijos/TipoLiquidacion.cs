using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public enum TipoLiquidacion
	{
		[Description("Salario Básico")]
		[AmbientValue("publico")]
		SalarioBasico = 1,

		[Description("Valor fijo para salario mensual inferior a base")]
		[AmbientValue("privado")]
		ValorFijoParaSalarioMensualinferiorBase = 2,

		[Description("Porcentaje a los conceptos matriculados")]
		[AmbientValue("publico")]
		PorcentajeConceptosMatriculados = 3,
		
		[Description("Suma porcentaje a las horas de novedad")]
		[AmbientValue("publico")]
		SumaPorcentajeHorasNovedad = 4,
		
		[Description("Novedades fijas por empleado")]
		[AmbientValue("publico")]
		NovedadesFijasEmpleado = 5,
		
		[Description("Porcentaje a los conceptos matriculados > base")]
		[AmbientValue("privado")]
		PorcentajeConceptosMatriculadosMayorBase = 6,

		[Description("Porcentaje a las horas de novedad")]
		[AmbientValue("publico")]
		PorcentajeHorasNovedad = 7,

		[Description("Genera Valor por unidad")]
		[AmbientValue("privado")]
		GeneraValorPorUnidad = 8,

		[Description("Reajusta salario al básico")]
		[AmbientValue("privado")]
		ReajustaSalarioBasico = 9,
		
		[Description("Valor Para Salario mensual inferior a Base x Hora")]
		[AmbientValue("publico")]
		ValorParaSalarioMensualInferiorBasexHora = 10,
		
		[Description("Retención en la fuente Pr 1")]
		[AmbientValue("privado")]
		RetencionFuentePr1 = 11,

		[Description("Porcentaje al acumulado mensual")]
		[AmbientValue("privado")]
		PorcentajeAcumuladoMensual = 12,

		[Description("Porcentaje al acumulado mensual > Base")]
		[AmbientValue("privado")]
		PorcentajeAcumuladoMensualMayorBase = 13,

		[Description("Valor al acumulado mensual inferior a base")]
		[AmbientValue("privado")]
		ValorAcumuladoMensualInferiorBase = 14,

		[Description("Fondo de solidaridad pensional")]
		[AmbientValue("privado")]
		FondoSolidaridadPensional = 15,

		[Description("Porcentaje a las horas de novedad >= minimo")]
		[AmbientValue("privado")]
		PorcentajeHorasNovedadMayorIgualMinimo = 16,

		[Description("Valor para salario inferior a Base x Hora")]
		[AmbientValue("publico")]
		ValorParaSalarioInferiorBasexHora = 17,

		[Description("Retención en la fuente Pr 2")]
		[AmbientValue("privado")]
		RetencionFuentePr2 = 18,

		[Description("Promedio dominical")]
		[AmbientValue("privado")]
		PromedioDominical = 19,

		[Description("Genera Valor x Hora empleado")]
		[AmbientValue("privado")]
		GeneraValorxHoraEmpleado = 20,

		[Description("Embargos Judiciales")]
		[AmbientValue("privado")]
		EmbargosJudiciales = 21,

		[Description("Porcentaje a los conceptos matriculados Valida base")]
		[AmbientValue("privado")]
		PorcentajeConceptosMatriculadosValidaBase = 22,

		[Description("Valor fijo para salario mayor a base")]
		[AmbientValue("privado")]
		ValorFijoParaSalarioMayorBase = 23,

		[Description("Porcentaje Mensual a los Conceptos Matriculados")]
		[AmbientValue("privado")]
		PorcentajeMensualConceptosMatriculados = 24,
		
	}
}
