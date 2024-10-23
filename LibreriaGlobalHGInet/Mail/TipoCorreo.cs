using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibreriaGlobalHGInet.Mail
{
	public enum TipoCorreo
	{
		Ninguno = 0,
		Creacion = 1,
		Aprobacion = 2,
		Rechazo = 3,
		Bloqueo = 4,
		Activacion = 5,
		Recuperarclave = 6,
	}
}
