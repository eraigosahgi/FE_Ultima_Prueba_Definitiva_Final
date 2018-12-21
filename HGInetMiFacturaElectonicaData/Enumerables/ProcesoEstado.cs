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
		[Category("0")]
		Recepcion = 1,

		[Description("Validación Documento")]
		[AmbientValue("privado")]
		[Category("0")]
		Validacion = 2,

		[Description("Generación UBL")]
		[AmbientValue("privado")]
		[Category("0")]
		UBL = 3,

		[Description("Almacenamiento XML")]
		[AmbientValue("privado")]
		[Category("100")]
		AlmacenaXML = 4,

		[Description("Firma XML")]
		[AmbientValue("privado")]
		[Category("100")]
		FirmaXml = 5,

		[Description("Pendiente Envio Dian")]
		[AmbientValue("privado")]
		[Category("100")]
		CompresionXml = 6,

		[Description("Envío Dian")]
		[AmbientValue("privado")]
		[Category("200")]
		EnvioZip = 7,

		[Description("Envío E-mail Adquiriente")]
		[AmbientValue("publico")]
		[Category("300")]
		EnvioEmailAcuse = 8,

		[Description("Recepción Acuse")]
		[AmbientValue("publico")]
		[Category("300")]
		RecepcionAcuse = 9,

		[Description("Envío E-mail Acuse")]
		[AmbientValue("publico")]
		[Category("300")]
		EnvioRespuestaAcuse = 10,

		[Description("Documento Pendiente Envío Proveedor")]
		[AmbientValue("publico")]
		[Category("300")]
		PendienteEnvioProveedorDoc = 11,

		[Description("Envío Exitoso Proveedor")]
		[AmbientValue("publico")]
		[Category("300")]
		EnvioExitosoProveedor = 12,

		[Description("Acuse Pendiente Envío Proveedor")]
		[AmbientValue("publico")]
		[Category("300")]
		PendienteEnvioProveedorAcuse = 13,

		[Description("Consulta DIAN")]
		[AmbientValue("publico")]
		[Category("200")]
		ConsultaDian = 14,

		[Description("Pago Documento")]
		[AmbientValue("publico")]
		[Category("300")]
		PagoDocumento = 15,

		[Description("Error Dian, Finaliza Proceso")]
		[AmbientValue("publico")]
		[Category("400")]
		FinalizacionErrorDian = 90,

		[Description("Fin Proceso Exitoso")]
		[AmbientValue("publico")]
		[Category("300")]
		Finalizacion = 99
	}
}
