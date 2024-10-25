using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData
{
    public enum TipoCompra
    {

        [Description("Recarga Interna")]
        Cortesia = 1,

        [Description("Compra")]
        Compra = 2,

        [Description("Post-Pago")]
        PostPago = 3

    }
}
