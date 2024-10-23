using HGInetMiFacturaElectonicaController;
using HGInetMiFacturaElectonicaData.Modelo;
using LibreriaGlobalHGInet.ObjetosComunes.Mensajeria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{
    public class CorreosController : ApiController
    {

		[HttpPost]
		[Route("Api/ActualizarCorreo")]
		public IHttpActionResult ActualizarCorreo(MensajeResumen Correo)
		{

			try
			{
				Ctl_ProcesosCorreos proceso = new Ctl_ProcesosCorreos();

				//proceso.ActualizarCorreo(Correo);
				var Tarea1 = proceso.AsyncActualizarCorreo(Correo);

				return Ok();
			}
			catch (Exception excepcion)
			{

				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

	}
}
