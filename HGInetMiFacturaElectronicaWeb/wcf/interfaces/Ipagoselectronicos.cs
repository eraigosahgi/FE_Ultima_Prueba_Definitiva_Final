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


		[OperationContract(Name = "ConsultaPorFechaElaboracion")]
		[FaultContract(typeof(Error), Action = "ConsultaPorFechaElaboracion", Name = "Error")]
		[WebInvoke(Method = "GET")]
		List<PagoElectronicoRespuestaPorFecha> ConsultaPorFechaElaboracion(string DataKey, string Identificacion, DateTime FechaInicial, DateTime FechaFinal, int Procesados = 0);


		[OperationContract(Name = "ConsultaAgrupadosPorFechaElaboracion")]
		[FaultContract(typeof(Error), Action = "ConsultaAgrupadosPorFechaElaboracion", Name = "Error")]
		[WebInvoke(Method = "GET")]
		List<PagoElectronicoRespuestaAgrupadoPorFecha> ConsultaAgrupadosPorFechaElaboracion(string DataKey, string Identificacion, DateTime FechaInicial, DateTime FechaFinal, int Procesados = 0);

		[OperationContract(Name = "ActualizarEstadoPago")]
		[FaultContract(typeof(Error), Action = "ActualizarEstadoPago", Name = "Error")]
		[WebInvoke(Method = "GET")]
		List<PagoElectronicoRespuestaDetalle> ActualizarEstadoPago(string DataKey, string Identificacion, string CodigosRegistros);
	}
}
