using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.Enumerables
{
    public enum EstadoPlan
    {
        [Description("Habilitado")]        
        Habilitado = 0,

        [Description("Inhabilitado")]
        Inhabilitado = 1,

        [Description("Procesado")]
        Procesado = 2,
    }
}
