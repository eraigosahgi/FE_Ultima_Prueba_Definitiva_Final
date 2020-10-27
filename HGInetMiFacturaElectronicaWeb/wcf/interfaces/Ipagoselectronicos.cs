using HGInetMiFacturaElectonicaData.ModeloServicio.Respuestas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using LibreriaGlobalHGInet.Error;

namespace HGInetMiFacturaElectronicaWeb.wcf
{
	// NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "Ipagoselectronicos" en el código y en el archivo de configuración a la vez.
	[ServiceContract(SessionMode = SessionMode.Allowed, Namespace = "HGInetFacturaElectronica.ServiciosWcf", Name = "ServicioPagosElectronicos")]
	public interface Ipagoselectronicos
	{
		[OperationContract(Name = "Test")]
		[WebInvoke(Method = "GET")]
		string DoWork();

		[OperationContract(Name = "ConsultaPorCodigoRegistro")]
		[FaultContract(typeof(Error), Action = "ConsultaPorCodigoRegistro", Name = "Error")]
		[WebInvoke(Method = "GET")]
		List<PagoElectronicoRespuesta> ConsultaPorCodigoRegistro(string DataKey, string Identificacion, string CodigosRegistros);
	}
}
