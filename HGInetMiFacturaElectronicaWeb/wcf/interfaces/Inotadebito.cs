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

	[ServiceContract(SessionMode = SessionMode.Allowed, Namespace = "HGInetFacturaElectronica.ServiciosWcf", Name = "ServicioNotaDebito")]
	public interface Inotadebito
	{
		[OperationContract(Name = "Test")]
		[WebInvoke(Method = "GET")]
		string DoWork();

		[OperationContract(Name = "Recepcion")]
		[FaultContract(typeof(Error), Action = "Recepcion", Name = "Error")]
		[WebInvoke(Method = "POST")]
		List<DocumentoRespuesta> Recepcion(List<NotaDebito> documentos);

		[OperationContract(Name = "ObtenerPorFechasAdquiriente")]
		[FaultContract(typeof(Error), Action = "ObtenerPorFechasAdquiriente", Name = "Error")]
		[WebInvoke(Method = "GET")]
		List<NotaDebitoConsulta> ObtenerPorFechasAdquiriente(string DataKey, string Identificacion, DateTime FechaInicio, DateTime FechaFinal);
	}
}
