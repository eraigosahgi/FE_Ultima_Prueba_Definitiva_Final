using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public enum FormaPago
	{
		[Description("Efectivo")]
		Efectivo = 1,

		[Description("Cheque")]
		Cheque = 2,

		[Description("Transferencia")]
		Transferencia = 3,

		[Description("Tarjeta")]
		Tarjeta = 4,

		[Description("Venta Crédito")]
		VentaCredito = 5,

		[Description("Bono")]
		Bono = 6,

		[Description("Vale")]
		Vale = 7,

		[Description("Otros")]
		Otros = 8,

		[Description("Consignación")]
		Consignacion = 9,
	}
}
