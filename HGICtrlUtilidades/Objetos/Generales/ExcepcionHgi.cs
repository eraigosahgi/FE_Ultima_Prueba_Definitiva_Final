using HGICtrlUtilidades.Funciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public class ExcepcionHgi : Exception
	{
		public DateTime Fecha { get; set; }
		public NotificacionCodigo Codigo { get; set; }
		public string MensajeAdicional { get; set; }
		public string MensajeResultado { get; set; }
		public bool Throw { get; set; }
		public Exception Excepcion { get; set; }

		public ExcepcionHgi()
		{
		}

		public ExcepcionHgi(string message) : base(message)
		{
		}

		public ExcepcionHgi(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected ExcepcionHgi(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		public ExcepcionHgi(Exception excepcion, NotificacionCodigo tipo_notificacion, string mensaje_adicional = "")
		{
			this.Fecha = Cl_Fecha.GetFecha();
			this.Codigo = tipo_notificacion;
			this.Excepcion = excepcion;
			this.MensajeAdicional = mensaje_adicional;
			this.Validar();
		}

		/// <summary>
		/// Valida la respuesta de la excepción
		/// </summary>
		private void Validar()
		{
			this.MensajeResultado = string.Empty;
			this.Throw = false;

			if (this.Excepcion != null)
			{
				// obtiene todos los mensajes de la excepción y las excepciones anidadas
				string mensaje = ObtenerMensajes(this.Excepcion);

				// validaciones a nivel de texto en los mensajes de la excepción
				if (!string.IsNullOrEmpty(mensaje))
				{
					// validación de clave primaria en base de datos
					//if (mensaje.Contains("Violation of PRIMARY KEY constraint"))
					//{	this.MensajeResultado = "Registro ya existe";
					//	this.Throw = true;
					//}
					////"statement conflicted with the FOREIGN KEY constraint"
					//if (mensaje.Contains("statement conflicted with the FOREIGN KEY constraint"))
					//{
					//	this.MensajeResultado = "Registro no existe en el maestro ";
					//	this.Throw = true;
					//}
					
					
					//// validación de clave primaria en base de datos
					//else if (mensaje.Contains("DELETE statement conflicted with the REFERENCE constraint"))
					//{
					//	this.MensajeResultado = "No se puede eliminar el registro debido que se encuentra en uso";
					//	this.Throw = true;
					//}
					//else
					//{
						this.MensajeResultado = mensaje;
						this.Throw = true;
					//}

				}

				// si recibe un mensaje adicional
				if(!string.IsNullOrEmpty(this.MensajeAdicional))
					this.MensajeAdicional = string.Format(" Detalle: {0}", this.MensajeAdicional);

				this.MensajeResultado = string.Format("{0}.{1}", this.MensajeResultado, this.MensajeAdicional);

				// indica si la excepción es lanzada nuevamente o se controla a nivel de mensaje
				if (this.Throw)
					throw this;
			}

		}

		/// <summary>
		/// Obtiene todos los mensajes de la excepción y las excepciones anidadas
		/// </summary>
		/// <param name="exception">datos</param>
		/// <returns>mensajes</returns>
		public static string ObtenerMensajes(Exception exception)
		{
			if (exception == null)
				return string.Empty;

			var messages = exception.FromHierarchy(ex => ex.InnerException)
				.Select(ex => ex.Message);
			return String.Join(Environment.NewLine, messages);
		}
		
	}
}
