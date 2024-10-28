using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectronicaWeb.Seguridad;
using LibreriaGlobalHGInet.Funciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{
	[EnableCors(origins: "*", headers: "*", methods: "*")]
	public class RecepcionDocumentosController : ApiController
	{

		[HttpGet]
		[Route("api/ObtenerRecepcionDocumentos")]
		public IHttpActionResult ObtenerRecepcionDocumentos(string estado, DateTime fecha_inicio, DateTime fecha_fin, int Desde, int Hasta)
		{
			try
			{
				Sesion.ValidarSesion();

				Ctl_RegistroRecepcion ctl_registro = new Ctl_RegistroRecepcion();
				List<TblRegistroRecepcion> datos = ctl_registro.ObtenerPorFecha(fecha_inicio, fecha_fin, estado, Desde, Hasta);

				if (datos == null)
				{
					return NotFound();
				}

				var retorno = datos.Select(d => new
				{
					d.StrId,
					d.DatFechaRegistro,
					d.DatFechaCorreo,
					d.IntProceso,
					d.StrRemitente,
					d.StrAsunto,
					d.IntEstado,
					d.StrObservaciones,
					
				});

				return Ok(retorno);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}



		// GET api/<controller>
		public IEnumerable<string> Get()
		{
			return new string[] { "value1", "value2" };
		}

		// GET api/<controller>/5
		public string Get(int id)
		{
			return "value";
		}

		// POST api/<controller>
		public void Post([FromBody]string value)
		{
		}

		// PUT api/<controller>/5
		public void Put(int id, [FromBody]string value)
		{
		}

		// DELETE api/<controller>/5
		public void Delete(int id)
		{
		}
	}
}