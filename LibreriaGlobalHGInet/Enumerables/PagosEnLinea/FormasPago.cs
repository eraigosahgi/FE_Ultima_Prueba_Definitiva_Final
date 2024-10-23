using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace LibreriaGlobalHGInet.Enumerables.PagosEnLinea
{
	public enum FormasPago
	{
		[Description("No definida.")]
		NoDefinida = 0,

		[Description("Pago PSE, débito desde su cuenta corriente o de ahorro.")]
		PSE = 29,

		[Description("Pago realizado por tarjeta de crédito en línea.")]
		Tarjetacredito1 = 31,

		[Description("Pago realizado por tarjeta de crédito en línea.")]
		Tarjetacredito2 = 32,

	}
}
