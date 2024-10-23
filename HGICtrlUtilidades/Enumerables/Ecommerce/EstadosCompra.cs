using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public enum EstadosCompra
	{
		[Description("Cancelado")]
		Cancelado = 0,

		[Description("Pedido Confirmado")]
		PedidoConfirmado = 1,

		[Description("Pago en Proceso")]
		PagoProceso = 2,

		[Description("Pago Aprobado")]
		PagoAprobado = 3,

		[Description("Pago Rechazado")]
		PagoRechazado = 4,

		[Description("Disponible Para Recoger en Tienda")]
		DisponibleParaRecoger = 5,

		[Description("Procesando Envío")]
		ProcesandoEnvio = 6,

		[Description("Envío Programado")]
		EnvioProgramado = 7,

		[Description("Pedido Entregado")]
		PedidoEntregado = 8,

	}
}
