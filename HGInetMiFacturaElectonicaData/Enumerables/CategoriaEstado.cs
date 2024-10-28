using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData
{
	public enum CategoriaEstado
	{

		[Description("No Recibido")]
		NoRecibido = 0,

		[Description("Recibido Plataforma")]
		Recibido = 100,

		[Description("Envío DIAN")]
		EnvioDian = 200,

		[Description("Validado DIAN")]
		ValidadoDian = 300,

		[Description("Fallido DIAN")]
		FallidoDian = 400,

	}
}
