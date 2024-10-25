using HGInetMiFacturaElectronicaAudit.Modelo;
using LibreriaGlobalHGInet.RegistroLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectronicaAudit.Controladores
{
	public class Srv_Log
	{
		FEHGIAuditoria db = new FEHGIAuditoria();


		public TblLog Guardar(TblLog Log)
		{
			try
			{
				//db.Set<TblLog>().Add(Log);
				//db.SaveChanges();

				return Log;
			}
			catch (Exception excepcion)
			{
				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Auditoria, MensajeTipo.Error, MensajeAccion.creacion);
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

	}
}
