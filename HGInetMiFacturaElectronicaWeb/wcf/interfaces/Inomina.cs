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
	[ServiceContract(SessionMode = SessionMode.Allowed, Namespace = "HGInetFacturaElectronica.ServiciosWcf", Name = "ServicioNomina")]
	public interface Inomina
	{
		[OperationContract(Name = "Test")]
		[WebInvoke(Method = "GET")]
		string DoWork();

		[OperationContract(Name = "Recepcion")]
		[FaultContract(typeof(Error), Action = "Recepcion", Name = "Error")]
		[WebInvoke(Method = "POST")]
		List<DocumentoRespuesta> Recepcion(List<Nomina> documentos);
	}
}
