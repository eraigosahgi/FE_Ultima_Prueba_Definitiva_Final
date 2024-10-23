using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public class MensajeContenido
	{
		/// <summary>
		/// Código de identificación del destinatario para el mensaje
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// Asunto del mensaje
		/// </summary>
		public string Asunto { get; set; }

		/// <summary>
		/// Emails de envío
		/// </summary>
		public List<DestinatarioEmail> Emails { get; set; }

		/// <summary>
		/// Contenido en texto plano
		/// </summary>
		public string ContenidoTexto { get; set; }

		/// <summary>
		/// Contenido en Html
		/// </summary>
		public string ContenidoHtml { get; set; }

		/// <summary>
		/// Archivos adjuntos
		/// </summary>
		public List<Adjunto> Adjuntos { get; set; }

		/// <summary>
		/// Indica si envía la información a todos los destinatarios en un correo (true) o varios correos (false)
		/// </summary>
		public bool UnoaUno { get; set; }
	}

	public class MensajeContenidoGlobal
	{
		public string identificacion { get; set; }
		public string serial { get; set; }
		public List<MensajeContenido> MensajeContenido { get; set; }
		public string Aplicacion { get; set; }

	}

}
