using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.Enumerables
{
    public enum Habilitacion
    {                         
        [Description("Valida Objeto")]
        [AmbientValue("0")]
        Valida_Objeto = 0,

        [Description("Pruebas")]
        [AmbientValue("0")]
        Pruebas = 1,
        
        [Description("Producción")]
        [AmbientValue("99")]
        Produccion = 99
    }
}




