using LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibreriaGlobalHGInet.ObjetosComunes.Mensajeria
{
    /// <summary>
	/// Datos del destinatario
	/// </summary>
	public class Destinatario
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