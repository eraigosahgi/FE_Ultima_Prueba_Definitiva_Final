using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.Enumerables
{
	public enum EstadoEmpresa
	{
		[Description("ACTIVA")]
		ACTIVA = 1,

		[Description("INACTIVA")]
		INACTIVA = 2,

		[Description("EN PROCESO DE REGISTRO ")]
		REGISTRO = 3,	
	}
}
