using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.Respuesta
{
	public class MensajeHistorial : ItemDatos
	{
		public string Comment { get; set; }
		public long EventAt { get; set; }
		public MensajeEstado EventType { get; set; }
		public string State { get; set; }
		public string Useragent { get; set; }
		public TimeSpan TimeInterval { get; set; }

	}
}
