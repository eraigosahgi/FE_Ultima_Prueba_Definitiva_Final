using HGInetMiFacturaElectonicaController.Indicadores;
using HGInetMiFacturaElectonicaController.Indicadores.Objetos;
using HGInetMiFacturaElectronicaWeb.Seguridad;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{
	public class IndicadoresController : ApiController
	{
		/// <summary>
		/// Obtiene el indicador de las respuestas de acuse de los documentos.
		/// </summary>
		/// <param name="identificacion_empresa">Número de identificación de la empresa para el filtro de búsqueda.</param>
		/// <param name="tipo_empresa">1: Administrador - 2: Facturador - 3: Adquiriente</param>
		/// <param name="mensual">True: Reporte por mes actual - False: Reporte Acumulado</param>
		/// <returns></returns>
		[HttpGet]
		[Route("Api/ReporteEstadosAcuse")]
		public IHttpActionResult ReporteEstadosAcuse(string identificacion_empresa, int tipo_empresa, DateTime fecha_inicio, DateTime fecha_fin)
		{
			try
			{
				Sesion.ValidarSesion();

				List<PorcentajesResumen> datos_EstadoAcuseMensual = new List<PorcentajesResumen>();
				Ctl_Indicadores clase_indicadores = new Ctl_Indicadores();

			datos_EstadoAcuseMensual = clase_indicadores.ReporteEstadosAcuse(identificacion_empresa, tipo_empresa, fecha_inicio, fecha_fin);

				if (datos_EstadoAcuseMensual == null)
				{
					return NotFound();
				}

				return Ok(datos_EstadoAcuseMensual);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Obtiene el indicador del acumulado de los documentos por tipo
		/// </summary>
		/// <param name="identificacion_empresa">Número de identificación de la empresa para el filtro de búsqueda.</param>
		/// <param name="tipo_empresa">1: Administrador - 2: Facturador - 3: Adquiriente</param>
		/// <returns></returns>
		[HttpGet]
		[Route("Api/ReporteDocumentosPorTipo")]
		public IHttpActionResult ReporteDocumentosPorTipo(string identificacion_empresa, int tipo_empresa, DateTime fecha_inicio, DateTime fecha_fin)
		{
			try
			{
				Sesion.ValidarSesion();

				List<PorcentajesResumen> datos_tipos = new List<PorcentajesResumen>();
				Ctl_Indicadores clase_indicadores = new Ctl_Indicadores();

				datos_tipos = clase_indicadores.DocumentosPorTipo(identificacion_empresa, tipo_empresa, fecha_inicio, fecha_fin);

				if (datos_tipos == null)
				{
					return NotFound();
				}

				return Ok(datos_tipos);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Obtiene los porcentajes de documentos por estado.
		/// </summary>
		/// <param name="identificacion_empresa"></param>
		/// <param name="tipo_empresa">1: Administrador - 2: Facturador - 3: Adquiriente</param>
		/// <returns></returns>
		[HttpGet]
		[Route("Api/ReporteDocumentosPorEstado")]
		public IHttpActionResult ReporteDocumentosPorEstado(string identificacion_empresa, int tipo_empresa, DateTime fecha_inicio, DateTime fecha_fin)
		{
			try
			{
				Sesion.ValidarSesion();

				List<PorcentajesResumen> datos_estados = new List<PorcentajesResumen>();
				Ctl_Indicadores clase_indicadores = new Ctl_Indicadores();

				datos_estados = clase_indicadores.DocumentosPorEstado(identificacion_empresa, tipo_empresa, fecha_inicio, fecha_fin);

				if (datos_estados == null)
				{
					return NotFound();
				}

				return Ok(datos_estados);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Obtiene los porcentajes de documentos por estado.
		/// </summary>
		/// <param name="identificacion_empresa"></param>
		/// <param name="tipo_empresa">1: Administrador - 2: Facturador - 3: Adquiriente</param>
		/// <returns></returns>
		[HttpGet]
		[Route("Api/ReporteDocumentosPorEstadoCategoria")]
		public IHttpActionResult ReporteDocumentosPorEstadoCategoria(string identificacion_empresa, int tipo_empresa, DateTime fecha_inicio, DateTime fecha_fin)
		{
			try
			{
				Sesion.ValidarSesion();

				List<PorcentajesResumen> datos_estados = new List<PorcentajesResumen>();
				Ctl_Indicadores clase_indicadores = new Ctl_Indicadores();

				datos_estados = clase_indicadores.DocumentosPorEstadoCategoria(identificacion_empresa, tipo_empresa, fecha_inicio, fecha_fin);

				if (datos_estados == null)
				{
					return NotFound();
				}

				return Ok(datos_estados);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Obtiene el indicador de documentos por tipo mensual durante los ultimos doce meses
		/// </summary>
		/// <param name="identificacion_empresa">Número de identificación de la empresa para el filtro de búsqueda.</param>
		/// <param name="tipo_empresa">1: Administrador - 2: Facturador - 3: Adquiriente</param>
		/// <returns></returns>
		[HttpGet]
		[Route("Api/ReporteDocumentosPorTipoAnual")]
		public IHttpActionResult ReporteDocumentosPorTipoAnual(string identificacion_empresa, int tipo_empresa, DateTime fecha_inicio, DateTime fecha_fin)
		{
			try
			{
				Sesion.ValidarSesion();

				ResumenMovimientos movimientos = new ResumenMovimientos();

				Ctl_Indicadores prueba_indicador = new Ctl_Indicadores();

				List<ValoresTipoDocumento> datos_tipos = new List<ValoresTipoDocumento>();
				Ctl_Indicadores clase_indicadores = new Ctl_Indicadores();

				datos_tipos = clase_indicadores.DocumentosPorTipoAnual(identificacion_empresa, tipo_empresa, fecha_inicio, fecha_fin);

				if (datos_tipos == null)
				{
					return NotFound();
				}

				return Ok(datos_tipos);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Obtiene el resumen de los planes transaccionales del facturador.
		/// </summary>
		/// <param name="identificacion_empresa"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("Api/ReporteResumenPlanesAdquiridos")]
		public IHttpActionResult ReporteResumenPlanesAdquiridos(string identificacion_empresa, DateTime fecha_inicio, DateTime fecha_fin)
		{
			try
			{
				Sesion.ValidarSesion();

				List<ResumenPlanes> planes_adquiridos = new List<ResumenPlanes>();
				Ctl_Indicadores clase_indicadores = new Ctl_Indicadores();

				planes_adquiridos = clase_indicadores.ResumenPlanesAdquiridos(identificacion_empresa, fecha_inicio, fecha_fin);

				if (planes_adquiridos == null)
				{
					return NotFound();
				}

				return Ok(planes_adquiridos);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		[HttpGet]
		[Route("Api/ReporteVentasAnuales")]
		public IHttpActionResult ReporteVentasAnuales(DateTime fecha_inicio, DateTime fecha_fin)
		{
			try
			{
				Sesion.ValidarSesion();

				List<VentasMensuales> ventas = new List<VentasMensuales>();
				Ctl_Indicadores clase_indicadores = new Ctl_Indicadores();

				ventas = clase_indicadores.Ventas(fecha_inicio, fecha_fin);

				if (ventas == null)
				{
					return NotFound();
				}

				return Ok(ventas);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		[HttpGet]
		[Route("Api/ReporteTopCompradores")]
		public IHttpActionResult ReporteTopCompradores(int cantidad_top, DateTime fecha_inicio, DateTime fecha_fin)
		{
			try
			{
				Sesion.ValidarSesion();

				Ctl_Indicadores clase_indicadores = new Ctl_Indicadores();

				ResumenCompradores datos = clase_indicadores.TopCompradores(cantidad_top, fecha_inicio, fecha_fin);

				if (datos == null)
				{
					return NotFound();
				}

				return Ok(datos);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		[HttpGet]
		[Route("Api/ReporteTopTransaccional")]
		public IHttpActionResult ReporteTopTransaccional(int cantidad_top, DateTime fecha_inicio, DateTime fecha_fin)
		{
			try
			{
				Sesion.ValidarSesion();
								
				Ctl_Indicadores clase_indicadores = new Ctl_Indicadores();

				ResumenTopTransaccional top_transaccional = clase_indicadores.TopTransaccional(cantidad_top, fecha_inicio, fecha_fin);

				if (top_transaccional == null)
				{
					return NotFound();
				}

				return Ok(top_transaccional);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

	}
}
