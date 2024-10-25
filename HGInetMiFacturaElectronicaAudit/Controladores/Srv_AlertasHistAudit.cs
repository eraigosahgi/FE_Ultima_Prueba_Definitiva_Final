using HGInetMiFacturaElectronicaAudit.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectronicaAudit.Controladores
{
	public class Srv_AlertasHistAudit
	{
		FEHGIAuditoria db = new FEHGIAuditoria();
		/// <summary>
		/// Crea el registro de auditoria de un documento.
		/// </summary>
		/// <param name="datos">datos del registro.</param>
		/// <returns></returns>
		public TblSeguimientoAlertas Crear(TblSeguimientoAlertas datos)
		{
			try
			{
				db.Set<TblSeguimientoAlertas>().Add(datos);
				db.SaveChanges();

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
		public List<TblSeguimientoAlertas> Obtener(string StrIdentificacion, int IdAlerta, int Estado)
		{

			try
			{
				List<TblSeguimientoAlertas> AlertaHist = db.TblSeguimientoAlertas.Where(x => (x.StrIdentificacion.Equals(StrIdentificacion)) && (x.IntIdAlerta == IdAlerta) && (x.IntIdEstado == Estado)).ToList();

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
		public TblSeguimientoAlertas Obtener(string StrIdentificacion, int IdAlerta, Guid StrIdSeguridadPlan, int Estado)
		{

			try
			{
				TblSeguimientoAlertas AlertaHist = db.TblSeguimientoAlertas.Where(x => (x.StrIdentificacion.Equals(StrIdentificacion)) && (x.IntIdAlerta == IdAlerta) && (x.IntIdEstado == Estado) && (x.StrIdSeguridadPlan == StrIdSeguridadPlan)).FirstOrDefault();

				return AlertaHist;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		public List<TblSeguimientoAlertas> Obtener(Guid StrIdSeguridad, int Estado)
		{

			try
			{
				List<TblSeguimientoAlertas> AlertaHist = db.TblSeguimientoAlertas.Where(x => (x.StrIdSeguridadEmpresa == StrIdSeguridad) && (x.IntIdEstado == Estado)).ToList();

				return AlertaHist;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Obtiene una alerta por el Id Principal
		/// </summary>
		/// <param name="Id">Id de seguridad de la alerta</param>
		/// <returns>TblSeguimientoAlertas</returns>
		public TblSeguimientoAlertas Obtener(Guid Id)
		{

			try
			{
				TblSeguimientoAlertas AlertaHist = db.TblSeguimientoAlertas.Where(x => (x.Id==Id)).FirstOrDefault();

				return AlertaHist;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		/// <summary>
		/// Obtiene una alerta por el Id Principal
		/// </summary>
		/// <param name="Id">Id de seguridad de la alerta</param>
		/// <returns>TblSeguimientoAlertas</returns>
		public TblSeguimientoAlertas Actualizar(TblSeguimientoAlertas Datos,int Estado)
		{

			try
			{
				
				{
					db.TblSeguimientoAlertas.Attach(Datos);

					Datos.IntIdEstado=Estado;

					db.SaveChanges();

				}

				return Datos;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

	


	}
}
