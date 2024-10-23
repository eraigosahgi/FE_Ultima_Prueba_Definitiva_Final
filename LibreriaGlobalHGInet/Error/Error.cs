using System;
using System.Collections.Generic;
using System.Linq;

namespace LibreriaGlobalHGInet.Error
{
	/// <summary>
	/// Clase de errores HGInet
	/// </summary>
	public partial class Error
	{
		#region propiedades

		private CodigoError _codigo;
		private string _mensaje;
		private DateTime _fecha;

		/// <summary>
		/// Código del error
		/// </summary>
		public CodigoError Codigo
		{
			get { return _codigo; }
			set { _codigo = value; }
		}

		/// <summary>
		/// Mensaje del error
		/// </summary>
		public string Mensaje
		{
			get { return _mensaje; }
			set { _mensaje = value; }
		}

		/// <summary>
		/// Fecha del error
		/// </summary>
		public DateTime Fecha
		{
			get { return _fecha; }
			set { _fecha = value; }
		}

		#endregion

		/// <summary>
		/// Constructor del error
		/// </summary>
		/// <param name="_codigo_error">código del error</param>
		/// <param name="_exec">excepción</param>
		public Error(CodigoError _codigo_error = CodigoError.NINGUNO, Exception _exec = null)
		{
			this.Codigo = _codigo_error;

			if (_exec != null)
			{
				if (_exec.InnerException != null)
					this.Mensaje = string.Format("{0} ---- InnerException:{1}", _exec.Message, _exec.InnerException.Message);
				else
					this.Mensaje = _exec.Message;
			}

			this.Fecha = Funciones.Fecha.GetFecha();
		}


		/// <summary>
		/// Constructor del error
		/// </summary>
		/// <param name="_personalizado">Mensaje personalizado</param>
		/// <param name="_codigo_error">código del error</param>
		/// <param name="_exec">excepción</param>
		public Error(string _personalizado, CodigoError _codigo_error = CodigoError.NINGUNO, Exception _exec = null)
		{
			this.Codigo = _codigo_error;

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

			this.Fecha = Funciones.Fecha.GetFecha();
		}
		/// <summary>
		/// Constructor del error vacío
		/// </summary>
		public Error() { }

	}
}
