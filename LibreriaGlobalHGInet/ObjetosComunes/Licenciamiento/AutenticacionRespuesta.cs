using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibreriaGlobalHGInet.ObjetosComunes.Licenciamiento
{
	public class AutenticacionRespuesta
	{
		public string JwtToken { get; set; }
		public string Menu { get; set; }
		public string DatosUsuario { get; set; }
		public string ListadoEmpresas { get; set; }
		public DateTime PasswordExpiration { get; set; }
		public Notificacion Notificacion { get; set; }
	}
}
