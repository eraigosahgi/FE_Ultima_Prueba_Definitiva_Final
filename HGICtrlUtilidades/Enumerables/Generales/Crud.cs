using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public enum Crud
	{
		[Description("Consultar")]
		Consultar = 0,

		[Description("Insertar")]
		Insertar = 1,

		[Description("Actualizar")]
		Actualizar = 2,

		[Description("Eliminar")]
		Eliminar = 3,
	}
}
