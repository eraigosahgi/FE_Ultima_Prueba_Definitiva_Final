using HGInetMiFacturaElectonicaController.Procesos;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Web.Http;

namespace HGInetMiFacturaElectronicaWeb.ApiServicios.Controller.Services
{
    public class AjustenominaController : ApiController
    {

		[HttpPost]
		[Route("Api/Ajustenomina/Recepcion")]
		public HttpResponseMessage Recepcion(List<NominaAjuste> documentos)
		{

			try
			{
				List<DocumentoRespuesta> lista_registros = Ctl_Documentos.Procesar(documentos);

				return Request.CreateResponse(HttpStatusCode.OK, lista_registros);
			}
			catch (Exception exec)
			{
				Error error = new Error(CodigoError.VALIDACION, exec);
				throw new FaultException<Error>(error, new FaultReason(string.Format("{0}", error.Mensaje)));
			}
		}

	}
}
