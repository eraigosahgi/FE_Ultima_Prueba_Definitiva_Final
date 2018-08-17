using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData
{
    public enum Perfiles
    {

        [Description("Administrador")]
        Administrador = 1,

        [Description("Facturador")]
        Facturador = 2,

        [Description("Adquiriente")]
        Adquiriente = 3,

        [Description("Integrador")]
        Integrador = 4
    }
}
