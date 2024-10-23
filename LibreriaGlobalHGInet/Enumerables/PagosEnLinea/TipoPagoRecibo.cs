using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace LibreriaGlobalHGInet.Enumerables.PagosEnLinea
{
	public enum TipoPagoRecibo
	{
		[Description("ReciboCaja")]
		ReciboCaja = 0,

		[Description("PagoPuntoVenta")]
		PagoPtoVenta = 1,

	}
}
