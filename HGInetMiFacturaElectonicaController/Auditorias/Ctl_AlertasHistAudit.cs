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

	public class Ctl_AlertasHistAudit : MongoDBContext<TblHistAlertas>
	{
		#region Constructores 

		public Ctl_AlertasHistAudit() : base(new ModeloAutenticacion(Motores.MongoDB)) { }

		#endregion


		/// <summary>
		/// Crea el registro de auditoria de un documento.
		/// </summary>
		/// <param name="datos">datos del registro.</param>
		/// <returns></returns>
		private TblHistAlertas Crear(TblHistAlertas datos)
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


		public TblHistAlertas Crear(int IdAlerta, Guid facturador,string StrIdentificacion,string StrObservaciones,int Tipo,Guid StrIdSeguridadPlan)
		{
			try
			{
				TblHistAlertas datos = new HGInetMiFacturaElectonicaData.ModeloAuditoria.Objetos.TblHistAlertas()
				{
					IntIdAlerta = IdAlerta,
					DatFecha = Fecha.GetFecha(),
					StrIdSeguridadFact = facturador.ToString(),					
					IntIdEstado = 1,
					StrIdentificacion = StrIdentificacion,
					StrObservaciones= StrObservaciones,
					IntTipo=Tipo,
					StrIdSeguridadPlan = StrIdSeguridadPlan.ToString()
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
		/// Obtener el historico 
		/// </summary>
		/// <param name="StrIdSeguridad">Id de seguridad del facturador</param>
		/// <param name="IdAlerta">Id de la alerta</param>
		/// <returns></returns>
		public List<TblHistAlertas> Obtener(string StrIdentificacion, int IdAlerta) {

			try
			{
				List<TblHistAlertas> AlertaHist = this.Obtener(x => (x.StrIdentificacion.Equals(StrIdentificacion)) && (x.IntIdAlerta == IdAlerta) && (x.IntIdEstado==(Int32)Notificacion.Activa.GetHashCode()));				

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
		public TblHistAlertas Obtener(string StrIdentificacion, int IdAlerta,Guid StrIdSeguridadPlan)
		{

			try
			{
				TblHistAlertas AlertaHist = this.Obtener(x => (x.StrIdentificacion.Equals(StrIdentificacion)) && (x.IntIdAlerta == IdAlerta) && (x.IntIdEstado == (Int32)Notificacion.Activa.GetHashCode()) && (x.StrIdSeguridadPlan== StrIdSeguridadPlan.ToString())).FirstOrDefault();

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
		public List<TblHistAlertas> ReiniciarAlertaPorcentaje(Guid StrIdSeguridad)
		{
			try
			{
				List<TblHistAlertas> AlertaHistorico = new List<TblHistAlertas>();
				List<TblHistAlertas> AlertaHist = new List<TblHistAlertas>();
				try
				{
					AlertaHistorico = this.Obtener(x => (x.StrIdSeguridadFact.Equals(StrIdSeguridad)) && (x.IntIdEstado == (Int32)Notificacion.Activa.GetHashCode()));
					//(x.IntTipo == (Int32)TipoAlerta.SinPlan.GetHashCode())
					 AlertaHist = AlertaHistorico.Where(x=>(x.IntTipo == (Int32)TipoAlerta.Porcenjate.GetHashCode() || x.IntTipo== (Int32)TipoAlerta.SinPlan.GetHashCode())).ToList();
					if (AlertaHist != null)
					{
						var collection = db.GetCollection<TblHistAlertas>("TblHistAlertas");

						foreach (var item in AlertaHist)
						{
							var filter = Builders<TblHistAlertas>.Filter.Eq("Id", item.Id);
							var update = Builders<TblHistAlertas>.Update.Set("IntIdEstado", (Int32)Notificacion.Inactiva.GetHashCode());

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