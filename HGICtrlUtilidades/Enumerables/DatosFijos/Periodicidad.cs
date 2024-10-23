using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public enum Periodicidad
	{
		[Description("Semanal")]
		Semanal = 0,

		[Description("Decadal")]
		Decadal = 1,

		[Description("Quincenal")]
		Quincenal = 2,

		[Description("Mensual")]
		Mensual = 3,

		[Description("Trimestral")]
		Trimestral = 4,

		[Description("Semestral")]
		Semestral = 5,

		[Description("Anual")]
		Anual = 6,

		[Description("Otros")]
		Otros = 7,
	}
}