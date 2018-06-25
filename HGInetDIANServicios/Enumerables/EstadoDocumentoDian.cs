using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetDIANServicios
{
	public enum EstadoDocumentoDian
	{


		[Description("Pendiente")]
		Pendiente = 1,

		[Description("Aceptado")]
		Aceptado = 2,

		[Description("Rechazado")]
		Rechazado = 3


	}
}
