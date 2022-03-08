using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectronicaWeb.Seguridad;
using LibreriaGlobalHGInet.Funciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{
	public class EventosRadianController : ApiController
	{

		[HttpGet]
		[Route("api/ObtenerEventosRadian")]
		public IHttpActionResult ObtenerEventosRadian(Guid id_seguridad)
		{
			try
			{
				Sesion.ValidarSesion();

				Ctl_EventosRadian Ctrl_Eventos = new Ctl_EventosRadian();

				List<TblEventosRadian> eventos = Ctrl_Eventos.Obtener(id_seguridad);

				bool inscribir_documento = false;

				foreach (var item in eventos)
				{
					if(item.IntEstadoEvento==3 || item.IntEstadoEvento == 5)
					{
						inscribir_documento = true;
					}
				}

				foreach (var item in eventos)
				{
					if (item.IntEstadoEvento >=6 )
					{
						inscribir_documento = false;
					}
				}

				if (eventos == null)
				{
					return NotFound();
				}

				var retorno = eventos.Select(d => new
				{
					Inscribir_Documento = inscribir_documento,
					d.DatFechaEvento,
					d.IntEstadoEvento,
					EstadoEvento = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<CodigoResponseV2>(d.IntEstadoEvento)),
					d.IntNumeroEvento,
					d.StrUrlEvento,
				}).OrderBy(x => x.DatFechaEvento);

				return Ok(retorno);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

	}
}
