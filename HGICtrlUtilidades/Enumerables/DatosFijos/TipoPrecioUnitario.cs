using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades.Enumerables.DatosFijos
{
    public enum TipoPrecioUnitario {

        [Description("Sin precio")]
        SinPrecio = 0,

        [Description("Precio producto sugerido")]
        PrecioProductoSugerido = 1,

        [Description("Valor kardex sugerido")]
        ValorKardexSugerido = 2,

        [Description("Precio producto fijo")]
        PrecioProductoFijo = 3,

        [Description("Valor kardex fijo")]
        ValorKardexFijo = 4,

    }
}
