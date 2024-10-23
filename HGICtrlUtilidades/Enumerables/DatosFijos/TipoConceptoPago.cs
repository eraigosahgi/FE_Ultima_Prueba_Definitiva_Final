using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
    public enum TipoConceptoPago
    {

        [Description("Pago")]
        Pago = 1,

        [Description("ReteIva")]
        ReteIva = 2,

        [Description("ReteFte")]
        ReteFte = 3,

        [Description("Nota")]
        Nota = 4,

        [Description("Descuento")]
        Descuento = 5,

        [Description("Otro")]
        Otro = 6,

        [Description("Deduccion")]
        Deduccion = 7,

        [Description("Anticipos")]
        Anticipos = 8,

        [Description("Retenido")]
        Retenido = 9,
    }
}
