using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public enum EstadoPago
	{
		[Description("Pago no Iniciado.")]
		SinIniciar = -1,

		[Description("Pago Rechazado o Nunca Iniciado en la Entidad Financiera.")]
		Rechazado = 0,

		[Description("Pago Pendiente por Iniciar en Entidad Financiera.")]
		Pendiente = 888,

		[Description("Pago Pendiente por Finalizar en Entidad Financiera.")]
		Pendiente2 = 999,

		[Description("Pago Exitoso.")]
		Aprobado = 1,
	}
}
