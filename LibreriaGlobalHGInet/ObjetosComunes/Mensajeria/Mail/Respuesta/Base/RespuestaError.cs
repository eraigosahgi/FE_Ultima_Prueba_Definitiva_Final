using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.Respuesta
{
	public class RespuestaError
	{
		public string ErrorInfo { get; set; }
		public string ErrorMessage { get; set; }
		public int StatusCode { get; set; }
	}
}
