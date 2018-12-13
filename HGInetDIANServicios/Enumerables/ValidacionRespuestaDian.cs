using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetDIANServicios
{
	public enum ValidacionRespuestaDian
	{

		[Description("Pendiente")]
		Pendiente = 1,

		[Description("Recibido")]
		Recibido = 2,

		[Description("No Recibido")]
		NoRecibido = 3

	}
}
