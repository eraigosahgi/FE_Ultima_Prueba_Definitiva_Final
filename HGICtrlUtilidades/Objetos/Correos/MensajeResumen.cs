using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	/// <summary>
	/// Resumen del envío del mensaje con seguimiento
	/// </summary>
	public class MensajeResumen
	{
		/// <summary>
		/// Fecha de recibo en MailJet
		/// ArrivedAt
		/// </summary>
		public DateTime Recibido { get; set; }

		/// <summary>
		/// Número de archivos adjuntos detectados en el mensaje.
		/// AttachmentCount
		/// </summary>
		public int Adjuntos { get; set; }

		/// <summary>
		/// Número de intentos realizados para entregar el mensaje.
		/// AttemptCount
		/// </summary>
		public int Intentos { get; set; }

		/// <summary>
		/// Referencia de ID a Campaña
		/// CampaignID
		/// </summary>
		public long IdCampana { get; set; }

		/// <summary>
		/// ID del contacto al que se envió el mensaje.
		/// ContactID
		/// </summary>
		public long IdContacto { get; set; }

		/// <summary>
		/// Retardo entre el procesamiento y la entrega del mensaje (milisegundos).
		/// Delay
		/// </summary>
		public long Restraso { get; set; }

		/// <summary>
		/// Tiempo dedicado a procesar el texto del mensaje (milisegundos).
		/// FilterTime
		/// </summary>
		public int TiempoProcesamiento { get; set; }

		/// <summary>
		/// Referencia de ID al remitente
		/// SenderID
		/// </summary>
		public long IdRemitente { get; set; }

		/// <summary>
		/// Indica si se solicitó el seguimiento de clics para este mensaje
		/// IsClickTracked
		/// </summary>
		public bool SeguimientoClic { get; set; }

		/// <summary>
		/// Indica si se solicitó el seguimiento al leer para este mensaje
		/// IsOpenTracked
		/// </summary>
		public bool SeguimientoLectura { get; set; }

		/// <summary>
		/// Indica si el mensaje contiene una parte HTML
		/// IsHtmlPartIncluded
		/// </summary>
		public bool ContenidoHtml { get; set; }

		/// <summary>
		/// Se solicitó el seguimiento de desuscripción para este mensaje
		/// IsUnsubTracked
		/// </summary>
		public bool SeguimientoDesuscripcion { get; set; }

		/// <summary>
		/// Tamaño del mensaje (en bytes).
		/// MessageSize
		/// </summary>
		public long TamanoMensaje { get; set; }

		/// <summary>
		/// Puntuación de spam para este mensaje.
		/// SpamassassinScore
		/// </summary>
		public decimal SpamPuntuacion { get; set; }

		/// <summary>
		/// Referencia de ID a MessageState Estado actual del mensaje.
		/// StateID
		/// </summary>
		public long IdEstado { get; set; }

		/// <summary>
		/// Es el estado del mensaje permanente (es decir, ya no cambiará).
		/// 
		/// </summary>
		public bool EstadoCambia { get; set; }

		/// <summary>
		/// Estado del mensaje
		/// Status
		/// </summary>
		public string Estado { get; set; }

		/// <summary>
		/// Información del seguimiento del mensaje como: entregado, lectura, clics, etc.
		/// </summary>
		public List<MensajeSeguimiento> Seguimiento { get; set; }
		/// <summary>
		/// Retorna  los siguientes id dependiendo de del resultado del porveedor de correos.
		/// * 0 No Entregado
		/// * 1 Entregado
		/// * 2 Enviado
		/// </summary>
		public int IdResultado { get; set; }
	}
}
