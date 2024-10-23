using HGInetMiFacturaElectonicaController.Auditorias;
using HGInetMiFacturaElectronicaWeb.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{
	public class LogController:ApiController
	{
		[HttpGet]
		[Route("Api/ConsultaLog")]
		public IHttpActionResult ConsultaLog(DateTime Fecha, string Categoria, string Tipo, string Accion)
		{
			try
			{
				Sesion.ValidarSesion();
				var datos = Ctl_Log.Obtener(Fecha, Categoria, Tipo, Accion);
				var resultado = datos.Select( d=>new {
					d.Id,
					d.DatFecha,
					d.IntCategoria,
					d.StrCategoria,
					d.IntTipo,
					d.StrTipo,
					d.IntAccion,
					d.StrAccion,
					d.StrArchivo,
					d.StrClase,
					d.Strerror_custom,
					d.StrExcepcion,
					d.StrLinea,
					d.StrMensaje,
					d.StrMetodo,
					d.StrModulo				
				});

				return Ok(resultado);

			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

	}
}