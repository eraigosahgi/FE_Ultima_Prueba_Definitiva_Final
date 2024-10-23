using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace LibreriaGlobalHGInet.Enumerables
{
	public enum EstadoVerificacionEmail
	{

		[Description("Registro")]
		Registro = 0,

		[Description("Verificacion")]
		Verificacion = 1,
		[Description("Verificado")]
		Verificado = 2

	}
}
