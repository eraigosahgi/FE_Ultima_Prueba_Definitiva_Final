using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	public class NotificacionCorreo
	{

		/// <summary>
		/// Id seguridad del documento registrado en la plataforma
		/// </summary>
		public string IdSeguridad { get; set; }

		/// <summary>
		/// Correo donde se va enviar el documento
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Indica el estado del Envio(1 - Exitoso, 0 - Fallido).
		/// </summary>
		public int IdEstado{ get; set; }

		/// <summary>
		/// Mensaje del estado del Envio.
		/// </summary>
		public string Mensaje { get; set; }



	}
}
