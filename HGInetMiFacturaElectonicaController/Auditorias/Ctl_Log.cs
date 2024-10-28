using HGInetMiFacturaElectronicaAudit.Controladores;
using HGInetMiFacturaElectronicaAudit.Modelo;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.RegistroLog;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController.Auditorias
{
	public static class Ctl_Log
	{

		public static bool Guardar(Exception e, MensajeCategoria categoria_mensaje, MensajeTipo tipo_mensaje, MensajeAccion accion_mensaje, string error_custom = "")
		{

			try
			{
				LogArchivo variable = RegistroLog.ConvertirLog(e, categoria_mensaje, tipo_mensaje, accion_mensaje, error_custom);

				TblLog Log = new TblLog();

				Log.Id = Guid.NewGuid();
				Log.DatFecha = Fecha.GetFecha();
				Log.IntCategoria = categoria_mensaje.GetHashCode();
				Log.StrCategoria = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<MensajeCategoria>(categoria_mensaje.GetHashCode()));

				Log.IntTipo = tipo_mensaje.GetHashCode();
				Log.StrTipo = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<MensajeTipo>(tipo_mensaje.GetHashCode()));

				Log.IntAccion = accion_mensaje.GetHashCode();
				Log.StrAccion = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<MensajeAccion>(accion_mensaje.GetHashCode()));

				Log.StrMensaje = JsonConvert.SerializeObject(variable);
				Log.StrExcepcion = variable.Logs.FirstOrDefault().Mensaje.ToString();
				Log.IntTipo = tipo_mensaje.GetHashCode();

				Log.Strerror_custom = error_custom;

				try
				{
					Log.StrLinea = (variable.Logs.FirstOrDefault().Mensaje.Detalle.FirstOrDefault().Linea == null) ? 0 : Convert.ToInt32(variable.Logs.FirstOrDefault().Mensaje.Detalle.FirstOrDefault().Linea);
					Log.StrArchivo = (variable.Logs.FirstOrDefault().Mensaje.Detalle.FirstOrDefault().Archivo == null) ? string.Empty : variable.Logs.FirstOrDefault().Mensaje.Detalle.FirstOrDefault().Archivo;
					Log.StrClase = (variable.Logs.FirstOrDefault().Mensaje.Detalle.FirstOrDefault().Clase == null) ? string.Empty : variable.Logs.FirstOrDefault().Mensaje.Detalle.FirstOrDefault().Clase;
					Log.StrMetodo = (variable.Logs.FirstOrDefault().Mensaje.Detalle.FirstOrDefault().Metodo == null) ? string.Empty : variable.Logs.FirstOrDefault().Mensaje.Detalle.FirstOrDefault().Metodo;
					Log.StrModulo = (variable.Logs.FirstOrDefault().Mensaje.Detalle.FirstOrDefault().Modulo == null) ? string.Empty : variable.Logs.FirstOrDefault().Mensaje.Detalle.FirstOrDefault().Modulo;
				}
				catch (Exception)
				{					
				}


				Srv_Log Srv = new Srv_Log();
				Srv.Guardar(Log);

				return true;
			}
			catch (Exception excepcion)
			{
				RegistroLog.EscribirLog(excepcion, categoria_mensaje, tipo_mensaje, accion_mensaje);
				return false;
			}
		}
	}
}
