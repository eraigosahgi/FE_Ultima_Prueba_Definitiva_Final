using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.Respuesta
{
	public class Respuesta<T> where T : ItemDatos
	{
		public List<T> Data { get; set; }
		public int Count { get; set; }
		public int Total { get; set; }

		public RespuestaError Error { get; set; }
	}
}
