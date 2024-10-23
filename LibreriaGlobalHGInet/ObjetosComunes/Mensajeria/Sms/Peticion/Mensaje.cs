using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Peticion
{	
	/// <summary>
	/// Mensaje para el envío
	/// </summary>
	public class Mensaje
	{
		/// <summary>
		/// Número o números telefonicos a enviar el SMS (separados por una coma ,)
		/// </summary>
		public string numero { get; set; }

		/// <summary>
		/// Mensaje de texto a enviar
		/// </summary>
		public string sms { get; set; }

		/// <summary>
		/// DateTime(opcional) fecha de programación futura para el envio de sms en formato "Y-m-d H:i:s" (Ejemplo: 2015-12-23 14:12:59), 
		/// si lo envia este campo el SMS se envia inmediatamente
		/// </summary>
		public DateTime? fecha { get; set; }

		/// <summary>
		/// Texto (opcional) referencia o nombre de campaña
		/// </summary>
		public string referencia { get; set; }


	}
}
