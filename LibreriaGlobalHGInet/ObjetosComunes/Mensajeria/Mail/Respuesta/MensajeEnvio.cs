using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.Respuesta
{

	public class MensajeEnvio
	{
		/// <summary>
		/// Información del envío de correos
		/// </summary>
		public List<MensajeEnvioItem> Data { get; set; }

		/// <summary>
		/// Cantidad total de correos enviados
		/// </summary>
		public int Count { get; set; }

		/// <summary>
		/// Cantidad total de correos para enviar
		/// </summary>
		public int Total { get; set; }

		/// <summary>
		/// Error del proceso
		/// </summary>
		public RespuestaError Error { get; set; }
	}

	/// <summary>
	/// Item de respuesta
	/// </summary>
	public class MensajeEnvioItem
	{
		/// <summary>
		/// Email del destinatario
		/// </summary>
		public string Email { get; set; }
        /// <summary>
        /// Id de respuesta del proveedor de la plataforma de envío
        /// </summary>
		public string MessageID { get; set; }
		/// <summary>
		/// Nuevo atributo indicado por el equipo de mailjet para mayor escalabilidad
		/// </summary>
		public Guid MessageUUID { get; set; }
		//public string MessageIDSendGrid { get; set; }
	}
}
