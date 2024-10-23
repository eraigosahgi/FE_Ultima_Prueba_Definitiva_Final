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
	[ServiceContract(SessionMode = SessionMode.Allowed, Namespace = "HGInetFacturaElectronica.ServiciosWcf", Name = "ServicioListaFe")]
	public interface IlistaFe
	{
		[OperationContract(Name = "Test")]
		[WebInvoke(Method = "GET")]
		string DoWork();

		[OperationContract(Name = "Obtener")]
		[FaultContract(typeof(Error), Action = "Obtener", Name = "Error")]
		[WebInvoke(Method = "GET")]
		List<ListaFE> Obtener(string DataKey, string Identificacion, string CodigoLista);
	}
}
