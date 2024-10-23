using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibreriaGlobalHGInet.ObjetosComunes.Mensajeria
{

	/// <summary>
	/// Objeto de repuesta del estado del correo para la Sonda de Validar Email
	/// </summary>
	public class MensajeValidarEmail
	{

		/// <summary>
		/// Retorna  los siguientes id dependiendo de del resultado del porveedor de correos.
		/// * 0 No Entregado
		/// * 1 Entregado
		/// * 2 Enviado
		/// </summary>
		public int IdResultado { get; set; }

		/// <summary>
		/// Fecha de recibo en MailJet
		/// ArrivedAt
		/// </summary>
		public DateTime Recibido { get; set; }

		/// <summary>
		/// Estado del mensaje
		/// Status
		/// </summary>
		public string Estado { get; set; }

		/// <summary>
		/// Email al que fue Enviado
		/// Status
		/// </summary>
		public string EmailEnviado { get; set; }


	}
}
