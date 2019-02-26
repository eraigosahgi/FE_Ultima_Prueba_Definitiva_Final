using HGInetEmailServicios.ServicioEnvio;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloAuditoria.Objetos;
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

	public class Ctl_AlertasHistAudit : MongoDBContext<TblSeguimientoAlertas>
	{
		#region Constructores 

		public Ctl_AlertasHistAudit() : base(new ModeloAutenticacion(Motores.MongoDB)) { }

		#endregion


		/// <summary>
		/// Crea el registro de auditoria de un documento.
		/// </summary>
		/// <param name="datos">datos del registro.</param>
		/// <returns></returns>
		private TblSeguimientoAlertas Crear(TblSeguimientoAlertas datos)
		{
			try
			{
				var data = this.Insert(datos);

				if (data.Exception != null)
					throw new ApplicationException(data.Exception.Message, data.Exception.InnerException);

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
		public TblSeguimientoAlertas Crear(int IdAlerta, Guid facturador,string StrIdentificacion,string StrResultado,int IdTipo,Guid StrIdSeguridadPlan,string StrMensaje)
		{
			try
			{
				TblSeguimientoAlertas datos = new HGInetMiFacturaElectonicaData.ModeloAuditoria.Objetos.TblSeguimientoAlertas()
				{
					IntIdAlerta = IdAlerta,
					DatFecha = Fecha.GetFecha(),
					StrIdSeguridadEmpresa = facturador.ToString(),					
					IntIdEstado = 1,
					StrIdentificacion = StrIdentificacion,
					StrMensaje= StrMensaje,
					IntIdTipo= IdTipo,
					StrIdSeguridadPlan = StrIdSeguridadPlan.ToString(),
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
		public List<TblSeguimientoAlertas> Obtener(string StrIdentificacion, int IdAlerta) {

			try
			{
				List<TblSeguimientoAlertas> AlertaHist = this.Obtener(x => (x.StrIdentificacion.Equals(StrIdentificacion)) && (x.IntIdAlerta == IdAlerta) && (x.IntIdEstado==(Int32)Notificacion.Activa.GetHashCode()));				

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
		public TblSeguimientoAlertas Obtener(string StrIdentificacion, int IdAlerta,Guid StrIdSeguridadPlan)
		{

			try
			{
				TblSeguimientoAlertas AlertaHist = this.Obtener(x => (x.StrIdentificacion.Equals(StrIdentificacion)) && (x.IntIdAlerta == IdAlerta) && (x.IntIdEstado == (Int32)Notificacion.Activa.GetHashCode()) && (x.StrIdSeguridadPlan== StrIdSeguridadPlan.ToString())).FirstOrDefault();

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
					AlertaHistorico = this.Obtener(x => (x.StrIdSeguridadEmpresa.Equals(StrIdSeguridad)) && (x.IntIdEstado == (Int32)Notificacion.Activa.GetHashCode()));
					//(x.IntTipo == (Int32)TipoAlerta.SinPlan.GetHashCode())
					 AlertaHist = AlertaHistorico.Where(x=>(x.IntIdTipo == (Int32)TipoAlerta.Porcenjate.GetHashCode() || x.IntIdTipo== (Int32)TipoAlerta.SinPlan.GetHashCode())).ToList();
					if (AlertaHist != null)
					{
						var collection = db.GetCollection<TblSeguimientoAlertas>("TblSeguimientoAlertas");

						foreach (var item in AlertaHist)
						{
							var filter = Builders<TblSeguimientoAlertas>.Filter.Eq("Id", item.Id);
							var update = Builders<TblSeguimientoAlertas>.Update.Set("IntIdEstado", (Int32)Notificacion.Inactiva.GetHashCode());

							collection.UpdateOneAsync(filter, update);
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


		/// <summary>
		/// Realiza la inserción de un registro en la base de datos.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public async Task<bool> Update()
		{

			

			return true;

		}
	}
}