using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
    public enum AmbientesUrl
    {
        [Description("HGInet Factura Electrónica Producción")]
        HGInetFacturaEProduccion = 1,

        [Description("HGInet Factura Electrónica Habilitación")]
        HGInetFacturaEHabilitacion = 2,

        [Description("HGInet Factura Electrónica QA")]
        HGInetFacturaEQa = 3,

        [Description("HGInet Licencia")]
        HGInetLicencia = 4,

        [Description("MiPaquete")]
        MiPaquete = 5,

        [Description("Happgi Licencia")]
        HappgiLicencia = 6,
    }
}
