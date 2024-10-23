using HGInetMiFacturaElectonicaController.Procesos;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetMiFacturaElectronicaWeb.Controllers.Services;
using LibreriaGlobalHGInet.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Web.Http;

namespace HGInetMiFacturaElectronicaWeb.ApiServicios.Controller.Services
{
    public class ResolucionController : ApiController
    {

		[HttpGet]
		[Route("Api/Resolucion/Consultar")]
		public HttpResponseMessage Consultar(string DataKey, string Identificacion)
		{
			try
			{
				List<Resolucion> respuesta = new List<Resolucion>();

				//Válida que la key sea correcta.
				Peticion.Validar(DataKey, Identificacion);
				
				//Obtiene los datos
				respuesta = Ctl_Resoluciones.Obtener(Identificacion);

				return Request.CreateResponse(HttpStatusCode.OK, respuesta);
			}
			catch (Exception exec)
			{
				Error error = new Error(CodigoError.VALIDACION, exec);
				return Request.CreateResponse(HttpStatusCode.Conflict, exec.Message);
			}
		}


		[HttpPost]
		[Route("Api/Resolucion/ConsultarResolucion")]
		public HttpResponseMessage ConsultarResolucion(Resolucion Resolucion)
		{

			try
			{
				// valida 
				Peticion.Validar(Resolucion.DataKey, Resolucion.Identificacion);

				List<Resolucion> resolucion_creada = Ctl_Resoluciones.CrearHabilitacion(Resolucion, Resolucion.Identificacion);

				// obtiene las resoluciones
				List<Resolucion> resoluciones_respuesta = Ctl_Resoluciones.Obtener(Resolucion.Identificacion);

				//Se agrega la que se creo o actualizo en el proceso.
				resoluciones_respuesta.AddRange(resolucion_creada);

				return Request.CreateResponse(HttpStatusCode.OK, resoluciones_respuesta);
			}
			catch (Exception exec)
			{
				Error error = new Error(CodigoError.VALIDACION, exec);
				throw new FaultException<Error>(error, new FaultReason(string.Format("{0}", error.Mensaje)));
			}
		}

	}
}
