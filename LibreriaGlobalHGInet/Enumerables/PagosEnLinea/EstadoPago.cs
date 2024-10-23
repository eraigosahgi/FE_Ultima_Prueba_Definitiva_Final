using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace LibreriaGlobalHGInet.Enumerables.PagosEnLinea
{
	public enum EstadoPago
	{
		[Description("Pago Rechazado o Nunca Iniciado en la Entidad Financiera.")]
		Rechazado = 0,

		[Description("Pago Pendiente por Iniciar en Entidad Financiera.")]
		Pendiente = 888,

		[Description("Pago Pendiente por Finalizar en Entidad Financiera.")]
		Pendiente2 = 999,

		[Description("Pago Exitoso.")]
		Aprobado = 1,		

		[Description("No Iniciado")]
		NoIniciado = 1000,
	}

	public enum EstadoPagoACH
	{
		[Description("Aprobado")]
		Aprobado = 0,

		[Description("Rechazado")]
		Rechazado = 1,

		[Description("Pendiente")]
		Pendiente = 2,

		[Description("Fallido")]
		Fallido = 3
	}
}
