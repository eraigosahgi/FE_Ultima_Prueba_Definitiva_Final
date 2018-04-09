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
	[ServiceContract(SessionMode = SessionMode.Allowed, Namespace = "HGInetFacturaElectronica.ServiciosWcf", Name = "ServicioDocumentos")]
	public interface Idocumentos
	{
		[OperationContract(Name = "Test")]
		[WebInvoke(Method = "GET")]
		string DoWork();

		
		[OperationContract(Name = "ConsultaPorNumeros")]
		[FaultContract(typeof(Error), Action = "ConsultaPorNumeros", Name = "Error")]
		[WebInvoke(Method = "GET")]
		List<DocumentoRespuesta> ConsultaPorNumeros(string DataKey, string Identificacion, int TipoDocumento, string Numeros);

		
		[OperationContract(Name = "ConsultaPorCodigoRegistro")]
		[FaultContract(typeof(Error), Action = "ConsultaPorCodigoRegistro", Name = "Error")]
		[WebInvoke(Method = "GET")]
		List<DocumentoRespuesta> ConsultaPorCodigoRegistro(string DataKey, string Identificacion, int TipoDocumento, string CodigosRegistros);
			

		[OperationContract(Name = "ConsultaPorFechaElaboracion")]
		[FaultContract(typeof(Error), Action = "ConsultaPorFechaElaboracion", Name = "Error")]
		[WebInvoke(Method = "GET")]
		List<DocumentoRespuesta> ConsultaPorFechaElaboracion(string DataKey, string Identificacion, int TipoDocumento, DateTime FechaInicial, DateTime FechaFinal);

		[OperationContract(Name = "Recepcion")]
		[FaultContract(typeof(Error), Action = "Recepcion", Name = "Error")]
		[WebInvoke(Method = "POST")]
		List<DocumentoRespuesta> Recepcion(List<DocumentoArchivo> documentos);

	}
}
