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
        [AmbientValue("0")]
        Recepcion = 0,

        [Description("Aprobado")]
        [AmbientValue("1")]
        Validacion = 1,

        [Description("Rechazado")]
        [AmbientValue("2")]
        UBL = 2

    }
}
