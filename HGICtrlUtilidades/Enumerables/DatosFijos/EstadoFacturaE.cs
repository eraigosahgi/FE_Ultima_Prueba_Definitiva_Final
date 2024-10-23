using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public enum EstadoFacturaE
	{
		[Description("No recibido")]
		Ninguna = 0,

		[Description("Recibido Plataforma")]
		Recibido = 100,

		[Description("Envio DIAN")]
		EnvioDian = 200,

		[Description("Validado DIAN")]
		ValidadoDian = 300,

		[Description("Fallido DIAN")]
		FallidoDian = 400,

	}

}
