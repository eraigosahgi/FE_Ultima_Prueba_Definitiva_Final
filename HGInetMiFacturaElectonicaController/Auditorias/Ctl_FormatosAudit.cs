using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloAuditoria.Objetos;
using LibreriaGlobalHGInet.Funciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController.Auditorias
{
	public class Ctl_FormatosAudit : MongoDBContext<TblAuditFormatos>
	{
		#region Constructores 

		public Ctl_FormatosAudit() : base(new ModeloAutenticacion(Motores.MongoDB)) { }

		#endregion

		/// <summary>
		/// Inserta el regeistro de auditoria en la base de datos.
		/// </summary>
		/// <param name="datos"></param>
		/// <returns></returns>
		private TblAuditFormatos Crear(TblAuditFormatos datos)
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
		/// Realiza la construcción e inserción de datos.
		/// </summary>
		/// <param name="codigo_formato"></param>
		/// <param name="identificacion_empresa"></param>
		/// <param name="id_seguridad_formato"></param>
		/// <param name="tipo_proceso"></param>
		/// <param name="id_seguridad_usuario"></param>
		/// <param name="observaciones"></param>
		/// <returns></returns>
		public TblAuditFormatos Crear(int codigo_formato, string identificacion_empresa, Guid id_seguridad_formato, TiposProceso tipo_proceso, Guid id_seguridad_usuario, string observaciones)
		{
			try
			{
				//Constuye el objeto para crear.
				TblAuditFormatos datos = new TblAuditFormatos()
				{
					IntCodigoFormato = codigo_formato,
					StrEmpresa = identificacion_empresa,
					StrIdSeguridad = id_seguridad_formato.ToString(),
					IntTipoProceso = tipo_proceso.GetHashCode(),
					StrUsuarioProceso = id_seguridad_usuario.ToString(),
					DatFechaProceso = Fecha.GetFecha(),
					StrObservaciones = observaciones
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
		/// Obtiene los datos de la auditoría por código de formato y empresa
		/// </summary>
		/// <param name="codigo_formato"></param>
		/// <param name="identificacion_empresa"></param>
		/// <returns></returns>
		public List<TblAuditFormatos> Obtener(int codigo_formato, string identificacion_empresa)
		{
			try
			{
				List<TblAuditFormatos> registros_audit = this.GetFilter(x => x.IntCodigoFormato == codigo_formato && x.StrEmpresa.Equals(identificacion_empresa));

				return registros_audit.OrderByDescending(x => x.DatFechaProceso).ToList();
			}
			catch (Exception excepcion)
			{
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
				List<TblAuditFormatos> registros_audit = this.GetFilter(x => x.IntCodigoFormato == codigo_formato && x.StrEmpresa.Equals(identificacion_empresa) && x.IntTipoProceso == tipo_proceso);

				return registros_audit.OrderByDescending(x => x.DatFechaProceso).ToList();
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


	}
}
