using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetMiFacturaElectronicaWeb.Controllers.Services;
using LibreriaGlobalHGInet.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HGInetMiFacturaElectronicaWeb.ApiServicios.Controller.Services
{
    public class EmpresasApiController : ApiController
    {
		[HttpGet]
		[Route("Api/EmpresasApi/Obtener")]
		public HttpResponseMessage Obtener(string DataKey, string Identificacion)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(DataKey))
					throw new ApplicationException("DataKey de la empresa inválido.");

				Empresa respuesta = new Empresa();

				//Válida que la key sea correcta.
				Peticion.Validar(DataKey, Identificacion);

				Ctl_Empresa ctl_empresa = new Ctl_Empresa();

				//Obtiene los datos de la empresa.
				TblEmpresas datos_tbl = ctl_empresa.Obtener(Identificacion);

				//Valida si se obtuvieron datos y convierte la tbl a Empresa.
				if (datos_tbl != null)
				{
					respuesta = Ctl_Empresa.ConvertirEmpresa(datos_tbl, true);
					DianProveedorV2 data_dian = HgiConfiguracion.GetConfiguration().DianProveedorV2;

					respuesta.PinSoftware = data_dian.Pin;

				}

				return Request.CreateResponse(HttpStatusCode.OK, respuesta);
			}
			catch (Exception exec)
			{
				Error error = new Error(CodigoError.VALIDACION, exec);
				return Request.CreateResponse(HttpStatusCode.Conflict, exec.Message);
			}
		}


		[HttpGet]
		[Route("Api/EmpresasApi/ConsultarAdquiriente")]
		public HttpResponseMessage ConsultarAdquiriente(string DataKey, string IdentificacionEmisor, string IdentificacionAdquiriente)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(DataKey))
					throw new ApplicationException("DataKey de la empresa inválido.");

				Empresa respuesta = new Empresa();

				//Válida que la key sea correcta.
				Peticion.Validar(DataKey, IdentificacionEmisor);

				Ctl_Empresa ctl_empresa = new Ctl_Empresa();

				//Obtiene los datos de la empresa.
				TblEmpresas datos_tbl = ctl_empresa.Obtener(IdentificacionAdquiriente);

				//Valida si se obtuvieron datos y convierte la tbl a Empresa.
				if (datos_tbl != null)
				{
					respuesta = Ctl_Empresa.ConvertirEmpresa(datos_tbl, false);

				}

				return Request.CreateResponse(HttpStatusCode.OK, respuesta);
			}
			catch (Exception exec)
			{
				Error error = new Error(CodigoError.VALIDACION, exec);
				return Request.CreateResponse(HttpStatusCode.Conflict, exec.Message);
			}
		}


	}
}
