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
		//No Aplica
		[Description("Pendiente")]
		Pendiente = 0,

		//Processed - Queued
		[Description("Enviado")]
		Enviado = 1,

		//Sent
		[Description("Entregado")]
		Entregado = 2,

		//Diferente a estos
		[Description("No Entregado")]
		NoEntregado = 3,

		//Opened-Clicked
		[Description("Leído")]
		Leido = 4,

		//No aplica para lo nuevo
		[Description("No Entregado")]
		Desconocido = 5,

	}
}
