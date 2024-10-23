using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public enum TipoAusentismo
	{
		[Description("No Aplica")]
		NoAplica = 0,

		[Description("Enfermedad general")]
		EnfermedadGeneral = 1,

		[Description("Licencia de maternidad")]
		LicenciaMaternidad = 2,

		[Description("Accidente de trabajo")]
		AccidenteTrabajo = 3,

		[Description("Licencia no remunerada")]
		LicenciaNoRemunerada = 4,

		[Description("Ausentismo")]
		Ausentismo = 5,

	}
}