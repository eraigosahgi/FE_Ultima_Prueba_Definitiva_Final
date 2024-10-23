using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace LibreriaGlobalHGInet.Enumerables.ServiciosWeb
{
	public enum GestionDocumento
	{
		[Description("Pendiente")]
		Pendiente = 0,

		[Description("Gestionado")]
		Gestionado = 1,

		[Description("Picking")]
		Picking = 2,

		[Description("Despachado")]
		Despachado = 3,

		[Description("Cancelado")]
		Cancelado = 4,
	}

	public enum GestionColores
	{
		[Description("#33b5e5")]
		Pendiente = 0,

		[Description("#FFFF00")]
		Gestionado = 1,

		[Description("#ff8800")]
		Picking = 2,

		[Description("#669900")]
		Despachado = 3,

		[Description("#cc0000")]
		Cancelado = 4,
	}
}
