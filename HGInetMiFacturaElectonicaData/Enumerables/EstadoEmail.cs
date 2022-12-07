using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData
{
	public enum EstadoEmail
	{
		[Description("Envío Fallido")]
		Fallido = 0,

		[Description("Envío Exitoso")]
		Exitoso = 1,
	}

	public enum EstadoEnvio
	{
		[Description("Pendiente")]
		Pendiente = 0,

		[Description("Enviado")]
		Enviado = 1,

		[Description("Entregado")]
		Entregado = 2,

		[Description("No Entregado")]
		NoEntregado = 3,

		[Description("Leído")]
		Leido = 4,

		[Description("No Entregado")]
		Desconocido = 5,

	}
}
