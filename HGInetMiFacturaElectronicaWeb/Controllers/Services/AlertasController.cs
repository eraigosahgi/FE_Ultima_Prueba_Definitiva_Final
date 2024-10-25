using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectronicaWeb.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{
	public class AlertasController : ApiController
	{

		/// <summary>
		/// Notifica al Facturador o personal Hgi, según sea el caso en el que el facturador tenga un % de saldo especifico según configuración o cuando no tiene saldo disponible
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Route("Api/ConsultaNotificacionAlertas")]
		public IHttpActionResult ConsultaNotificacionAlertas()
		{			
			try
			{
				Sesion.ValidarSesion();

				Ctl_Alertas controlador = new Ctl_Alertas();				
				var datos = controlador.ObtenerNotificaciones();				
				return Ok(datos);

			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Retorna la lista de alertas de la tabla TblAlertas
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Route("Api/ConsultaListaAlertas")]
		public IHttpActionResult ConsultaListaAlertas()
		{
			try
			{
				Sesion.ValidarSesion();

				Ctl_Alertas controlador = new Ctl_Alertas();
				var datos = controlador.ObtenerListaAlertas();
				return Ok(datos);

			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		

		[HttpGet]
		[Route("Api/ReprocesarAlerta")]
		public IHttpActionResult ReprocesarAlerta(string Facturador,int idAlerta)
		{
			
			try
			{
				Sesion.ValidarSesion();

				Ctl_Alertas controlador = new Ctl_Alertas();

				TblAlertas alerta = controlador.ObtenerAlerta(idAlerta);

				switch (alerta.IntTipo)
				{
					case 1://Porcentaje
						controlador.alertaPorcentajePlan(Facturador);
						break;
					case 2://Sin Saldo
						controlador.alertaSinSaldo(Facturador);
						break;
					case 3://Vencimiento
						controlador.alertaPlanporVencer(Facturador);
						break;
					default://Sin Saldo
					
						break;
				}
						
				return Ok();
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		[HttpPost]
		[Route("Api/GuardarAlerta")]
		public IHttpActionResult GuardarAlerta(TblAlertas alerta)
		{
			try
			{
				Sesion.ValidarSesion();

				Ctl_Alertas controlador = new Ctl_Alertas();

				TblAlertas datos = new TblAlertas();
				if (alerta.IntIdAlerta == 0)
				{
					datos =controlador.Insertar(alerta);
				}else {
					datos =controlador.Actualizar(alerta);
				}

				return Ok(datos);

			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}




	}
}