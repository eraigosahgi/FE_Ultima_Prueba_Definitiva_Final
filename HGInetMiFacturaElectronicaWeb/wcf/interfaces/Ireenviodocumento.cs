using HGInetMiFacturaElectonicaData.ModeloServicio;
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
	
	[ServiceContract(SessionMode = SessionMode.Allowed, Namespace = "HGInetFacturaElectronica.ServiciosWcf", Name = "ServicioReenvioDocumento")]
	public interface Ireenviodocumento
	{
		[OperationContract(Name = "Test")]
		[WebInvoke(Method = "GET")]
		string DoWork();

		[OperationContract(Name = "Recepcion")]
		[FaultContract(typeof(Error), Action = "Recepcion", Name = "Error")]
		[WebInvoke(Method = "POST")]
		List<NotificacionCorreo> Recepcion(List<EnvioDocumento> documentos);
	}
}
