using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Error;

namespace HGInetMiFacturaElectronicaWeb.wcf
{
	[ServiceContract(SessionMode = SessionMode.Allowed, Namespace = "HGInetFacturaElectronica.ServiciosWcf", Name = "ServicioResolucion")]
	public interface Iresolucion
	{
		[OperationContract(Name = "Test")]
		[WebInvoke(Method = "GET")]
		string DoWork();


		[OperationContract(Name = "Consultar")]
		[FaultContract(typeof(Error), Action = "Consultar", Name = "Error")]
		[WebInvoke(Method = "GET")]
		List<Resolucion> Consultar(string DataKey, string Identificacion);

	}
}
