using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectronicaWeb.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{
    public class IntegradoresController : ApiController
    {

		[HttpGet]
		[Route("Api/Integradores/Obtener")]
		public IHttpActionResult Obtener()
		{
			try
			{
				Sesion.ValidarSesion();

				Ctl_Integradores _integradores = new Ctl_Integradores();

				var datos = _integradores.Obtener();

				return Ok(datos);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

	}
}
