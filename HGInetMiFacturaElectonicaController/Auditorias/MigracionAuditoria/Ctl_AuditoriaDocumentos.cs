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
	public class Ctl_AuditoriaDocumentos : MongoDBContext<TblAuditDocumentos>
	{
		#region Constructores 

		public Ctl_AuditoriaDocumentos() : base(new ModeloAutenticacion(Motores.MongoDB)) { }

		#endregion


		public List<TblAuditDocumentos> Obtener(DateTime fecha_inicio, DateTime fecha_fin)
		{
			try
			{

				fecha_fin = new DateTime(fecha_fin.Year, fecha_fin.Month, fecha_fin.Day, 23, 59, 59, 999);
				
				List<TblAuditDocumentos> registros_audit = this.GetFilter(x =>(x.DatFecha >= fecha_inicio.Date && x.DatFecha <= fecha_fin)).ToList();

				return registros_audit;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}




		/// <summary>
		/// Obtiene los registros por id de seguridad del documento e identificación del obligado
		/// </summary>
		/// <param name="id_seguridad_doc">ID de seguridad del documento</param>
		/// <param name="identificacion_obligado">Número de identificación del obligado.</param>
		/// <returns></returns>
		public List<TblAuditDocumentos> Obtener(string id_seguridad_doc, string identificacion_obligado)
		{
			try
			{
				List<TblAuditDocumentos> registros_audit = this.GetFilter(x => (x.StrIdSeguridad.Equals(id_seguridad_doc) || id_seguridad_doc.Equals("*")) && (x.StrObligado.Equals(identificacion_obligado) || identificacion_obligado.Equals("*")));

				return registros_audit;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

	}
}
	
