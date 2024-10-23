using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public class Adjunto
	{
		/// <summary>
		/// Nombre del archivo con extensión
		/// </summary>
		public string Nombre { get; set; }

		/// <summary>
		/// Datos del archivo en base 64
		/// </summary>
		public string ContenidoB64 { get; set; }
	}
}
