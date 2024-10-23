using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace LibreriaGlobalHGInet.Enumerables.PagosEnLinea
{
	public enum FranquiciaPago
	{
		[Description("Amex")]
		AM = 0,

		[Description("Baloto")]
		BA = 1,

		[Description("Credencial")]
		CR = 2,

		[Description("Diners Club")]
		DC = 3,

		[Description("Efecty")]
		EF = 4,

		[Description("Gana")]
		GA = 5,

		[Description("Punto Red")]
		PR = 6,

		[Description("Red Servi")]
		RS = 7,

		[Description("Mastercard")]
		MC = 8,

		[Description("PSE")]
		PSE = 9,

		[Description("SafetyPay")]
		SP = 10,

		[Description("Visa")]
		VS = 11,

	}

	public enum FranquiciaPagoPayvalida
	{
		[Description("Apostar")]
		apostar = 0,

		[Description("Baloto")]
		baloto = 1,

		[Description("Bancolombia")]
		bancolombia = 2,

		[Description("Bevalida Colombia")]
		bevalida = 3,

		[Description("Efecty")]
		efecty = 4,

		[Description("GANA")]
		gana = 5,

		[Description("Nequi")]
		nequi = 6,

		[Description("PSE")]
		PSE = 7,

		[Description("PuntoRed")]
		puntored = 8,

		[Description("RedServi")]
		redservi = 9,

		[Description("SuRed")]
		sured = 10,

		[Description("SuSuerte")]
		susuerte = 11,

		[Description("Tarteja de Crédito")]
		tc = 11,

	}
}
