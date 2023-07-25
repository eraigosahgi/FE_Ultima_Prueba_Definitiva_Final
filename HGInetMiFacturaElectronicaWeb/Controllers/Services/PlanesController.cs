using HGInetMiFacturaElectonicaController.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{
	[EnableCors(origins: "*", headers: "*", methods: "*")]
	public class PlanesController : ApiController
	{
		[HttpGet]
		[Route("api/ObtenerSaldoHgiDocs")]
		public IHttpActionResult ObtenerSaldoHgiDocs(string Facturador)
		{
			try
			{
				//Busco documentos disponibles
				Ctl_PlanesTransacciones CtrPlanes = new Ctl_PlanesTransacciones();
				var Planes = CtrPlanes.obtenerSaldoDisponibles(Facturador);

				//Sin saldo
				if (Planes == null)
				{
					Planes = new { Facturador = 1, Planes = 0, TProcesadas = 0, TCompra = 0, TDisponible = 0, Porcentaje = 0 };
				}

				return Ok(Planes);
			}
			catch (Exception)
			{
				return Ok();
			}
		}

		[HttpGet]
		[Route("api/ObtenerPlanesHgiDocs")]
		public IHttpActionResult ObtenerPlanesHgiDocs(string Facturador)
		{
			try
			{
				//Busco documentos disponibles
				Ctl_PlanesTransacciones CtrPlanes = new Ctl_PlanesTransacciones();
				var Planes = CtrPlanes.obtenerPlanesHgiDocs(Facturador);

				//Sin saldo
				if (Planes == null)
				{
					Planes = new { Identificacion = Facturador, FechaCompra = 0, TProcesadas = 0, TCompra = 0, TDisponible = 0, Porcentaje = 0, Valor = 0 };
				}

				return Ok(Planes);
			}
			catch (Exception)
			{
				return Ok();
			}

		}

	}
}