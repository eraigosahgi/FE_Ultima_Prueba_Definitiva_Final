using HGInetMiFacturaElectronicaAudit.Modelo;
using LibreriaGlobalHGInet.RegistroLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectronicaAudit.Controladores
{
	
	public class Srv_FormatosAudit
	{
		FEHGIAuditoria db = new FEHGIAuditoria();

		/// <summary>
		/// Inserta el regeistro de auditoria en la base de datos.
		/// </summary>
		/// <param name="datos"></param>
		/// <returns></returns>
		public TblAuditFormatos Crear(TblAuditFormatos datos)
		{
			try
			{				
				db.Set<TblAuditFormatos>().Add(datos);
				db.SaveChanges();
			
				return datos;
			}
			catch (Exception excepcion)
			{
				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Auditoria, MensajeTipo.Error, MensajeAccion.creacion);
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		/// <summary>
		/// Obtiene los datos de la auditoría por código de formato y empresa
		/// </summary>
		/// <param name="codigo_formato"></param>
		/// <param name="identificacion_empresa"></param>
		/// <returns></returns>
		public List<TblAuditFormatos> Obtener(int codigo_formato, string identificacion_empresa)
		{
			try
			{
				List<TblAuditFormatos> registros_audit = db.TblAuditFormatos.Where(x => x.IntCodigoFormato == codigo_formato && x.StrEmpresa.Equals(identificacion_empresa)).ToList();

				return registros_audit.OrderByDescending(x => x.DatFechaProceso).ToList();
			}
			catch (Exception excepcion)
			{
				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Auditoria, MensajeTipo.Error, MensajeAccion.consulta);
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		/// <summary>
		/// Obtiene los datos de la auditoría por código de formato, empresa y tipo de proceso
		/// </summary>
		/// <param name="codigo_formato"></param>
		/// <param name="identificacion_empresa"></param>
		/// <param name="tipo_proceso"></param>
		/// <returns></returns>
		public List<TblAuditFormatos> Obtener(int codigo_formato, string identificacion_empresa, int tipo_proceso)
		{
			try
			{
				List<TblAuditFormatos> registros_audit = db.TblAuditFormatos.Where(x => x.IntCodigoFormato == codigo_formato && x.StrEmpresa.Equals(identificacion_empresa) && x.IntTipoProceso == tipo_proceso).ToList();

				return registros_audit.OrderByDescending(x => x.DatFechaProceso).ToList();
			}
			catch (Exception excepcion)
			{
				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Auditoria, MensajeTipo.Error, MensajeAccion.consulta);
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}



	}
}
