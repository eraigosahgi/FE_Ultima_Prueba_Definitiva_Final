using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.Peticion
{
	/// <summary>
	/// Datos del destinatario
	/// </summary>
	public class MensajeDestinatario
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
		/// Tipo destinatario (From, To, CC, BCC)
		/// </summary>
		public TipoDestinatario Tipo { get; set; }

	}
}
