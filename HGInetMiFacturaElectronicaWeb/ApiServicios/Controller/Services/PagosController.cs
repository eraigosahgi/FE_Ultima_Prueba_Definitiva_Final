using HGInetMiFacturaElectonicaController.PagosElectronicos;
using HGInetMiFacturaElectonicaData.ModeloServicio.Respuestas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HGInetMiFacturaElectronicaWeb.ApiServicios.Controller.Services
{
    public class PagosController : ApiController
    {

		[HttpGet]
		[Route("Api/PagosApi/ConsultaHisPorFechaElaboracion")]
		public IHttpActionResult ConsultaHisPorFechaElaboracion(string Identificacion, DateTime FechaInicial, DateTime FechaFinal, int Procesados = 0)
		{
			try
			{

				DateTime fecha_corte = new DateTime(2023, 12, 31, 00, 00, 00);

				if (FechaFinal >= fecha_corte)
					FechaFinal = fecha_corte;

				Ctl_PagosElectronicos controlador = new Ctl_PagosElectronicos();
				List<PagoElectronicoRespuestaPorFecha> datos = controlador.ConsultaPorFechaElaboracion(Identificacion, FechaInicial, FechaFinal, Procesados);

				return Ok(datos);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		[HttpGet]
		[Route("Api/PagosApi/ConsultaAgrupadosHisPorFechaElaboracion")]
		public IHttpActionResult ConsultaAgrupadosHisPorFechaElaboracion(string Identificacion, DateTime FechaInicial, DateTime FechaFinal, int Procesados = 0)
		{
			try
			{

				DateTime fecha_corte = new DateTime(2023, 12, 31, 00, 00, 00);

				if (FechaFinal >= fecha_corte)
					FechaFinal = fecha_corte;

				Ctl_PagosElectronicos controlador = new Ctl_PagosElectronicos();
				List<PagoElectronicoRespuestaAgrupadoPorFecha> datos = controlador.ConsultaAgrupadosPorFechaElaboracion(Identificacion, FechaInicial, FechaFinal, Procesados);

				return Ok(datos);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


	}
}
