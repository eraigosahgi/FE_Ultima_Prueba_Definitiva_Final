using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HGICtrlUtilidades
{
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
			: this(Cl_Fecha.GetFecha(), MensajeAccion.ninguna, new LogMensaje()) { }


		public LogClase(MensajeAccion accion, LogMensaje mensaje)
			: this(Cl_Fecha.GetFecha(), accion, mensaje) { }

		private void Inicializar(DateTime fecha, MensajeAccion accion, LogMensaje mensaje)
		{
			Fecha = fecha;
			Categoria = accion;
			Mensaje = mensaje;
		}
	}
}
