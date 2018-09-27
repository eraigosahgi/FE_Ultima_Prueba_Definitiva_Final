using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData
{
    public enum ProcesoEstado
    {
        [Description("Recepción")]
        [AmbientValue("privado")]
        Recepcion = 1,

        [Description("Validación Documento")]
        [AmbientValue("privado")]
        Validacion = 2,

        [Description("Generación UBL")]
        [AmbientValue("privado")]
        UBL = 3,

        [Description("Almacenamiento XML")]
        [AmbientValue("privado")]
        AlmacenaXML = 4,

        [Description("Firma XML")]
        [AmbientValue("privado")]
        FirmaXml = 5,

        [Description("Compresión XML")]
        [AmbientValue("privado")]
        CompresionXml = 6,

        [Description("Envío Dian")]
        [AmbientValue("privado")]
        EnvioZip = 7,

        [Description("Envío E-mail Adquiriente")]
        [AmbientValue("publico")]
        EnvioEmailAcuse = 8,

        [Description("Recepción Acuse")]
        [AmbientValue("publico")]
        RecepcionAcuse = 9,

        [Description("Envío E-mail Acuse")]
        [AmbientValue("publico")]
        EnvioRespuestaAcuse = 10,

        [Description("Documento Pendiente Envío Proveedor")]
        [AmbientValue("publico")]
        PendienteEnvioProveedorDoc = 11,

        [Description("Envío Exisoto Proveedor")]
        [AmbientValue("publico")]
        EnvioExitosoProveedor = 12,

        [Description("Acuse Pendiente Envío Proveedor")]
        [AmbientValue("publico")]
        PendienteEnvioProveedorAcuse = 13,

        [Description("Error Dian, Finaliza Proceso")]
        [AmbientValue("publico")]
        FinalizacionErrorDian = 90,

        [Description("Fin Proceso Exitoso")]
        [AmbientValue("publico")]
        Finalizacion = 99
    }
}
