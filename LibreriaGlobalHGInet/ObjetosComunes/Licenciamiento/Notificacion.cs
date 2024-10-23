using LibreriaGlobalHGInet.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibreriaGlobalHGInet.ObjetosComunes.Licenciamiento
{
	public class Notificacion
	{
		public DateTime Fecha { get; set; }
		public NotificacionCodigo Codigo { get; set; }
		public string Mensaje { get; set; }


		/// <summary>
		/// Constructor del error
		/// </summary>
		/// <param name="codigo_error">código del error</param>
		/// <param name="_exec">excepción</param>
		public Notificacion(NotificacionCodigo codigo_error = NotificacionCodigo.NINGUNO, Exception _exec = null)
		{
			this.Codigo = codigo_error;

			if (_exec != null)
			{
				if (_exec.InnerException != null)
					this.Mensaje = string.Format("{0} ---- InnerException:{1}", _exec.Message, _exec.InnerException.Message);
				else
					this.Mensaje = _exec.Message;
			}

			this.Fecha = LibreriaGlobalHGInet.Funciones.Fecha.GetFecha();
		}


		/// <summary>
		/// Constructor del error
		/// </summary>
		/// <param name="_personalizado">Mensaje personalizado</param>
		/// <param name="_codigo_error">código del error</param>
		/// <param name="_exec">excepción</param>
		public Notificacion(string _personalizado, NotificacionCodigo codigo_error = NotificacionCodigo.NINGUNO, Exception _exec = null)
		{
			this.Codigo = codigo_error;

			if (_exec != null)
			{
				if (_exec.InnerException != null)
					this.Mensaje = string.Format("{0} - {1} ---- InnerException:{2}", _personalizado, _exec.Message, _exec.InnerException.Message);
				else
					this.Mensaje = string.Format("{0} - {1}", _personalizado, _exec.Message);
			}
			else
			{
				this.Mensaje = _personalizado;

			}

			this.Fecha = LibreriaGlobalHGInet.Funciones.Fecha.GetFecha();
		}
		/// <summary>
		/// Constructor del error vacío
		/// </summary>
		public Notificacion() { }

	}
}
