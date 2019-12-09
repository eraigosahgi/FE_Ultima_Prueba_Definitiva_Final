using HGInetEmailServicios.ServicioEnvio;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectronicaAudit.Controladores;
using HGInetMiFacturaElectronicaAudit.Modelo;
using LibreriaGlobalHGInet.Funciones;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController.Auditorias
{

	public class Ctl_AlertasHistAudit
	{
		
		/// <summary>
		/// Crea el registro de auditoria de un documento.
		/// </summary>
		/// <param name="datos">datos del registro.</param>
		/// <returns></returns>
		public TblSeguimientoAlertas Crear(TblSeguimientoAlertas datos)
		{
			try
			{
				Srv_AlertasHistAudit Srv = new Srv_AlertasHistAudit();

				datos = Srv.Crear(datos);
				return datos;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="IdAlerta"></param>
		/// <param name="facturador"></param>
		/// <param name="StrIdentificacion"></param>
		/// <param name="StrObservaciones"></param>
		/// <param name="Tipo"></param>
		/// <param name="StrIdSeguridadPlan"></param>
		/// <returns></returns>
		public TblSeguimientoAlertas Crear(int IdAlerta, Guid facturador, string StrIdentificacion, string StrResultado, int IdTipo, Guid StrIdSeguridadPlan, string StrMensaje)
		{
			try
			{
				TblSeguimientoAlertas datos = new TblSeguimientoAlertas()
				{
					Id = Guid.NewGuid(),
					IntIdAlerta = IdAlerta,
					DatFecha = Fecha.GetFecha(),
					StrIdSeguridadEmpresa = facturador,
					IntIdEstado = 1,
					StrIdentificacion = StrIdentificacion,
					StrMensaje = StrMensaje,
					IntIdTipo = IdTipo,
					StrIdSeguridadPlan = StrIdSeguridadPlan,
					StrResultadoProceso = StrResultado
				};

				datos = Crear(datos);

				return datos;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Obtiene una alerta especifica el cual este activa.
		/// </summary>
		/// <param name="StrIdSeguridad">Id de seguridad del facturador</param>
		/// <param name="IdAlerta">Id de la alerta</param>
		/// <returns></returns>
		public List<TblSeguimientoAlertas> Obtener(string StrIdentificacion, int IdAlerta)
		{

			try
			{				
				Srv_AlertasHistAudit Srv = new Srv_AlertasHistAudit();
				List<TblSeguimientoAlertas> AlertaHist = Srv.Obtener(StrIdentificacion,IdAlerta,(Int32)Notificacion.Activa.GetHashCode());

				return AlertaHist;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Retorna un plan especifico el cual fue notificado 
		/// </summary>
		/// <param name="StrIdSeguridad">Id de seguridad del facturador</param>
		/// <param name="IdAlerta">Id de la alerta</param>
		/// <param name="StrIdentificacion">StrIdSeguridadPlan</param>
		/// <returns></returns>
		public TblSeguimientoAlertas Obtener(string StrIdentificacion, int IdAlerta, Guid StrIdSeguridadPlan)
		{

			try
			{				
				Srv_AlertasHistAudit Srv = new Srv_AlertasHistAudit();
				TblSeguimientoAlertas AlertaHist = Srv.Obtener(StrIdentificacion,IdAlerta,StrIdSeguridadPlan, (Int32)Notificacion.Activa.GetHashCode());

				return AlertaHist;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		/// <summary>
		/// Cuando se hace una recarga por ejemplo, se debe ejecutar este metodo ya que este lo que hace es reiniciar la alerta para que valide nuevamente luego de procesar documentos
		/// </summary>
		/// <param name="StrIdSeguridad">Id de seguridad del facturador</param>
		/// <param name="IdAlerta"></param>
		/// <returns></returns>
		public List<TblSeguimientoAlertas> ReiniciarAlertaPorcentaje(Guid StrIdSeguridad)
		{
			try
			{
				List<TblSeguimientoAlertas> AlertaHistorico = new List<TblSeguimientoAlertas>();
				List<TblSeguimientoAlertas> AlertaHist = new List<TblSeguimientoAlertas>();
				try
				{
					Srv_AlertasHistAudit Srv = new Srv_AlertasHistAudit();
					AlertaHistorico = Srv.Obtener(StrIdSeguridad,(Int32)Notificacion.Activa.GetHashCode());
					
					AlertaHist = AlertaHistorico.Where(x => (x.IntIdTipo == (Int32)TipoAlerta.Porcenjate.GetHashCode() || x.IntIdTipo == (Int32)TipoAlerta.SinPlan.GetHashCode())).ToList();
					if (AlertaHist != null)
					{

						foreach (var item in AlertaHist)
						{
							
							Srv.Actualizar(item, (Int32)Notificacion.Inactiva.GetHashCode());
						}
						
					}
				}
				catch (Exception)
				{
					return AlertaHist;
				}

				return AlertaHist;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


	
	}
}