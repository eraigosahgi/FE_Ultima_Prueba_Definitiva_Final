using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectronicaAudit.Controladores;
using HGInetMiFacturaElectronicaAudit.Modelo;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.RegistroLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController.Auditorias
{
	public class Ctl_FormatosAudit
	{

		/// <summary>
		/// Inserta el regeistro de auditoria en la base de datos.
		/// </summary>
		/// <param name="datos"></param>
		/// <returns></returns>
		public TblAuditFormatos Crear(TblAuditFormatos datos)
		{
			try
			{
				Srv_FormatosAudit Srv = new Srv_FormatosAudit();

				Srv.Crear(datos);

				return datos;
			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Auditoria, MensajeTipo.Error, MensajeAccion.creacion);
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
					Id = Guid.NewGuid(),
					IntCodigoFormato = codigo_formato,
					StrEmpresa = identificacion_empresa,
					StrIdSeguridad = id_seguridad_formato,
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
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Auditoria, MensajeTipo.Error, MensajeAccion.creacion);
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
				Srv_FormatosAudit Srv = new Srv_FormatosAudit();
				List<TblAuditFormatos> registros_audit = Srv.Obtener(codigo_formato, identificacion_empresa);

				return registros_audit.OrderByDescending(x => x.DatFechaProceso).ToList();
			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Auditoria, MensajeTipo.Error, MensajeAccion.consulta);
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
				Srv_FormatosAudit Srv = new Srv_FormatosAudit();
				List<TblAuditFormatos> registros_audit = Srv.Obtener(codigo_formato, identificacion_empresa, tipo_proceso);

				return registros_audit.OrderByDescending(x => x.DatFechaProceso).ToList();
			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Auditoria, MensajeTipo.Error, MensajeAccion.consulta);
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


	}
}
