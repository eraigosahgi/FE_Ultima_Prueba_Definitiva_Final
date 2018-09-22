using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.Enumerables
{
    /// <summary>
    /// Enumerable de Códigos de documentos de acuerdo a los valores definidos en Guia de Interoperabilidad en la ​Tabla No. 5.2.1: Códigos de respuesta - Capítulo No. 5.2.  
    /// </summary>
    public enum DocumentType
    {
        [Description("FV")]
        Factura = 1,

        [Description("ND")]
        NotaDebito = 2,

        [Description("NC")]
        NotaCredito = 3,

        [Description("FC")]
        FacturaContingencia = 4,
    }

    /// <summary>
    /// Código de respuesta de acuerdo a los valores definidos en Guia de Interoperabilidad en la ​Tabla No. 5.2.1: Códigos de respuesta - Capítulo No. 5.2. 
    /// </summary>
    public enum ResponseCode
    {
        [Description("RECEIVED")]
        Pendiente = 0,

        [Description("ACCEPTED")]
        Aprobado = 1,

        [Description("REJECTED")]
        Rechazado = 2,

        [Description("TACITLY_ACCEPTED")]
        AprobadoTacito = 3,

        [Description("PAID")]
        Pagado = 4
    }
}
