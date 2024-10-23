using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{

    #region TipoContrato
    public enum TipoContratoEmpleado
    {
        [Description("Indefinido")]
        Indefinido = 1,

        [Description("Fijo")]
        Fijo = 2,

        [Description("Terminación de Obra")]
        Terminacion = 3,

        [Description("Convenio ejecución sindical")]
        Convenio = 4,

    }
    #endregion

}
