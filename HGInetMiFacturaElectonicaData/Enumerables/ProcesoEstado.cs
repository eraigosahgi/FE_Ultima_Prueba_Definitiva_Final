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

        [Description("Envío ZIP")]
        [AmbientValue("publico")]
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

		[Description("Error Dian, Finaliza Proceso")]
        [AmbientValue("publico")]
        FinalizacionErrorDian = 90,

		[Description("Fin Proceso Exitoso")]
        [AmbientValue("publico")]
        Finalizacion = 99
    }
}
