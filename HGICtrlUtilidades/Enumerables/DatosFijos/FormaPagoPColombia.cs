using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public enum FormaPagoPColombia
	{
		[Description("Ningúno")]
		Ninguno = 0,

		[Description("PP - Prepago - Prepayment")]
		PPPrepagoPrepayment = 1,

		[Description("IV - Factura - Invoice")]
		IVFacturaInvoice = 2,

		[Description("PD - Efectivo anticipado - Cash on delivery")]
		PDEfectivoanticipadoCashondelivery = 3,

		[Description("PC - Efectivo - Cash")]
		PCEfectivoCash = 4,

		[Description("CC - Tarjeta de credito - Credit card")]
		CCTarjetadecreditoCreditcard = 5,

		[Description("DD - Debito directo - Direct Debit")]
		DDDebitodirectoDirectDebit = 6,

		[Description("UN - No indentificado - Unknown")]
		UNNoindentificadoUnknown = 7,

		[Description("DC - Tarjeta debito - Debit card")]
		DCTarjetadebitoDebitcard = 8,

		[Description("Puntos Colombia")]
		PuntosColombia = 9,
	}
}
