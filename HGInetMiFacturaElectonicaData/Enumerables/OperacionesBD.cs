using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.Enumerables
{
    public enum OperacionesBD
    {
        [Description("IntConsultar")]
        IntConsultar = 1,

        [Description("IntAgregar")]
        IntAgregar = 2,

        [Description("IntEditar")]
        IntEditar = 3,

        [Description("IntEliminar")]
        IntEliminar = 4,

        [Description("IntAnular")]
        IntAnular = 5,

        [Description("IntGestion")]
        IntGestion = 6,

    }
}
