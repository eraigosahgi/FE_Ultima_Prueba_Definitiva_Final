using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibreriaGlobalHGInet.ObjetosComunes.Mensajeria
{
    public class MensajeContenidoSms
    {
        /// <summary>
        /// Código de identificación del destinatario para el mensaje
        /// </summary>
		public string Id { get; set; }

        /// <summary>
        /// Texto del mensaje
        /// </summary>
		public string Contenido { get; set; }

        /// <summary>
        /// Celulares de envío indicando el código del país
        /// 57: Colombia
        /// </summary>
		public List<string> NumerosCelulares { get; set; }

    }


    public class MensajeContenidoGlobalSms
    {
        public string identificacion { get; set; }
        public string serial { get; set; }
        public List<MensajeContenidoSms> MensajeContenidoSms { get; set; }
		public string Aplicacion { get; set; }

	}
}