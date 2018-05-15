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
        Recepcion = 1,

        [Description("Validación Documento")]
        Validacion = 2,

        [Description("Generación UBL")]
        UBL = 3,

        [Description("Almacenamiento XML")]
        AlmacenaXML = 4,

        [Description("Firma XML")]
        FirmaXml = 5,

        [Description("Compresión XML")]
        CompresionXml = 6,

        [Description("Envío ZIP")]
        EnvioZip = 7,

        [Description("Envío E-mail Adquiriente")]
        EnvioEmailAcuse = 8,

        [Description("Recepción Acuse")]
        RecepcionAcuse = 9,

        [Description("Envío E-mail Acuse")]
        EnvioRespuestaAcuse = 10,

		[Description("Error Dian, Finaliza Proceso")]
		FinalizacionErrorDian = 90,

		[Description("Fin Proceso Exitoso")]
        Finalizacion = 99
    }
}
