using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData
{
    public enum AdquirienteRecibo
    {

        [Description("Pendiente")]
        Recepcion = 0,

        [Description("Aprobado")]
        Validacion = 1,

        [Description("Rechazado")]
        UBL = 2

    }
}
