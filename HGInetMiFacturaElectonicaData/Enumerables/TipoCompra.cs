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
        
            [Description("Cortesía")]
            Administrador = 1,

            [Description("Compra")]
            Facturador = 2,

            [Description("Post-Pago")]
            Adquiriente = 3            
        
    }
}
