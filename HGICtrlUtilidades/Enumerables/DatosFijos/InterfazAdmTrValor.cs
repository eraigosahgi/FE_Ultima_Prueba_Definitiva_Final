using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public enum InterfazAdmTrValor
	{
		[Description("Valor")]
		Valor = 1,
		[Description("Descuento")]
		Descuento = 2,
		[Description("SubTotal")]
		SubTotal = 3,
		[Description("Total")]
		Total = 4,
		[Description("Iva")]
		Iva = 5,
		[Description("Costo")]
		Costo = 6,
		[Description("Neto")]
		Neto = 7,
		[Description("ReteIva")]
		ReteIva = 8,
		[Description("ReteFte")]
		ReteFte = 9,
		[Description("ReteCree")]
		ReteCree = 0,
		[Description("ReteIca")]
		ReteIca = 10,
		[Description("Fletes")]
		Fletes = 11,
		[Description("Intereses")]
		Intereses = 12,
		[Description("Impuesto1")]
		Impuesto1 = 13,
		[Description("Costo Agr")]
		CostoAgr = 14,
		[Description("Valor P1")]
		ValorP1 = 15,
		[Description("Valor P2")]
		ValorP2 = 16,
		[Description("Valor P3")]
		ValorP3 = 17,
		[Description("Valor P4")]
		ValorP4 = 18,
		[Description("Descuento Fin")]
		DescuentoFin = 19,
		[Description("Cambio")]
		Cambio = 20,
        [Description("Impuesto2")]
        Impuesto2 = 21,
    }
}