using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail
{
	public enum MensajeIdResultado
	{
		[Description("No Entregado")]
		NoEntregado = 0,

		[Description("Entregado")]
		Entregado = 1,

		[Description("Enviado")]
		Enviado = 2
	}
}
