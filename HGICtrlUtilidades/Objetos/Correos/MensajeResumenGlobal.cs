using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	/// <summary>
	/// Clase para consumir el servicio rest ConsultaController de mensajeria
	/// </summary>
	public class MensajeResumenGlobal
	{
		public string identificacion { get; set; }
		public string serial { get; set; }
		public long id_mensaje { get; set; }
	}
}
