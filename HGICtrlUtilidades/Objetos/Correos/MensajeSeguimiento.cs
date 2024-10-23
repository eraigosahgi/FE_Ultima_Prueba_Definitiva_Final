using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	/// <summary>
	/// Seguimiento del mensaje (entregado, lectura, clics)
	/// </summary>
	public class MensajeSeguimiento
	{
		/// <summary>
		/// Más detalles para el mensaje
		/// Comment
		/// </summary>
		public string Comentario { get; set; }

		/// <summary>
		/// Marca de tiempo cuando el evento fue registrado
		/// EventAt
		/// </summary>
		public TimeSpan TiempoEvento { get; set; }

		/// <summary>
		/// Tipo de evento
		/// EventType
		/// </summary>
		public string TipoEvento { get; set; }

		/// <summary>
		/// Estado del mensaje
		/// State
		/// </summary>
		public string Estado { get; set; }

		/// <summary>
		/// Useragent utilizado para activar el evento (cuando corresponda).
		/// Useragent
		/// </summary>
		public string Agente { get; set; }

		/// <summary>
		/// Fecha del evento
		/// Calculado desde el envío del email más el EventAt
		/// </summary>
		public DateTime FechaEvento { get; set; }


	}
}
