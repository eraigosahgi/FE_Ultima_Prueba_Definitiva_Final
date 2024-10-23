using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades.Enumerables.DatosFijos
{
	public enum TipoTransaccion
	{
		[Description("Documentos")]
		Documentos = 1,

		[Description("Pagos")]
		Pagos = 2,

		[Description("Causaciones")]
		Causaciones = 3
	}

	public enum TipoMvto
	{
		[Description("Saldo Inicial")]
		SaldoInicial = 0,

		[Description("Entrada")]
		Entrada = 1,

		[Description("No Kardex")]
		NoKardex = 3,

		[Description("Traslado")]
		Traslado = 4,

		[Description("Salida")]
		Salida = 20,


	}




}
