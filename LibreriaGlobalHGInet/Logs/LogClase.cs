using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace LibreriaGlobalHGInet.RegistroLog
{
	[XmlRoot(ElementName = "Log")]
	public class LogClase
	{
		public DateTime Fecha { get; set; }
		public MensajeAccion Categoria { get; set; }
		[XmlElement(ElementName = "Mensaje")]
		public LogMensaje Mensaje { get; set; }

		public LogClase(DateTime fecha, MensajeAccion accion, LogMensaje mensaje)
		{
			Inicializar(fecha, accion, mensaje);
		}

		public LogClase()
			: this(Funciones.Fecha.GetFecha(), MensajeAccion.ninguna, new LogMensaje()) { }


		public LogClase(MensajeAccion accion, LogMensaje mensaje)
			: this(Funciones.Fecha.GetFecha(), accion, mensaje) { }

		private void Inicializar(DateTime fecha, MensajeAccion accion, LogMensaje mensaje)
		{
			Fecha = fecha;
			Categoria = accion;
			Mensaje = mensaje;
		}
	}
}
