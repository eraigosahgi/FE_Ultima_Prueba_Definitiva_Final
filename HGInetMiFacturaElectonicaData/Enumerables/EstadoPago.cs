using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.Enumerables
{
    public enum EstadoPago
    {
        [Description("Rechazado")]
        Rechazado = 0,

        [Description("Pendiente por Iniciar")]
        Pendiente = 888,

        [Description("Pendiente por Finalizar")]
        Pendiente2 = 999,

        [Description("Aprobado")]
        Aprobado = 1,
    }
}
