using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	/// <summary>
	/// Información de respuesta al enviar los SMS
	/// </summary>
	public class MensajeEnvioSms
	{
		/// <summary>
		/// Id del cliente en Hablame
		/// </summary>
		public string cliente { get; set; }

		/// <summary>
		/// Número de lote generado en la solicitud, unicamente se genera si se envia 2 ó mas destinatarios
		/// </summary>
		public string lote_id { get; set; }

		/// <summary>
		/// Fecha y hora en la que se recibio la solicitud de envío del SMS
		/// </summary>
		public string fecha_recepcion { get; set; }

		/// <summary>
		/// Resultado del SMS
		/// 0 => SMS exitoso
		/// 1 => SMS fallido
		/// </summary>
		public int resultado { get; set; }

		/// <summary>
		/// texto resultado del SMS
		/// </summary>
		public string resultado_t { get; set; }

		/// <summary>
		/// Número de mensajes procesados
		/// </summary>
		public string sms_procesados { get; set; }

		/// <summary>
		/// Texto de referencia del SMS
		/// </summary>
		public string referecia { get; set; }

		/// <summary>
		/// IP de solicitud de envío
		/// </summary>
		public string ip { get; set; }

		/// <summary>
		/// Listado de mensajes SMS enviados
		/// </summary>
		public List<MensajeDatos> sms_objeto { get; set; }
	}
}
