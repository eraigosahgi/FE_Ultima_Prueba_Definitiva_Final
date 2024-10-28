using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloAuditoria.Objetos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController.Auditorias.MigracionAuditoria
{
	public class Ctl_AuditoriaNotificaciones : MongoDBContext<TblSeguimientoAlertas>
	{
		#region Constructores 

		public Ctl_AuditoriaNotificaciones() : base(new ModeloAutenticacion(Motores.MongoDB)) { }

		#endregion


		public List<TblSeguimientoAlertas> Obtener(DateTime fecha_inicio, DateTime fecha_fin)
		{
			try
			{

				fecha_fin = new DateTime(fecha_fin.Year, fecha_fin.Month, fecha_fin.Day, 23, 59, 59, 999);

				List<TblSeguimientoAlertas> registros_audit = this.GetFilter(x => (x.DatFecha >= fecha_inicio.Date && x.DatFecha <= fecha_fin)).ToList();

				return registros_audit;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}
	}
}
