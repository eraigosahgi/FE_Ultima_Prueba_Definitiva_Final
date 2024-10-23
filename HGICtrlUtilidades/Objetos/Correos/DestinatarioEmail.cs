using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public class DestinatarioEmail
	{
		/// <summary>
		/// Nombre
		/// </summary>
		public string Nombre { get; set; }

		/// <summary>
		/// Email
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Tipo destinatario 
		/// 0:From, 1:To, 2:CC, 3:BCC, 4:Reply
		/// </summary>
		public TipoDestinatario Tipo { get; set; }
	}
}
