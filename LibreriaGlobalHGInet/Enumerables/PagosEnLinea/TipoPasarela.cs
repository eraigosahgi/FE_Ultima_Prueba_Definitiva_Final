using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace LibreriaGlobalHGInet.Enumerables.PagosEnLinea
{
	public enum TipoPasarela
	{

		[Description("ZonaVirtual")]
		ZonaVirtual = 1,

		[Description("Epayco")]
		Epayco = 2,

		[Description("Payvalida")]
		Payvalida = 3,
		[Description("HGI Pasarela Pagos")]
		HGIPasarelaPagos = 4

	}
}
