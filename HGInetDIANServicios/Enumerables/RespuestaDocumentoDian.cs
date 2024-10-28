using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetDIANServicios
{
	public enum RespuestaDocumentoDian
	{

		[Description("NO HAY RESPUESTA DE LA DIAN")]
		Norespuesta = 7200000,

		[Description("RECIBIDA")]
		Recibida = 7200001,

		[Description("EXITOSA")]
		Exitosa = 7200002,

		[Description("EN PROCESO DE VALIDACIÓN")]
		Proceso = 7200003,

		[Description("FALLIDA (Documento no cumple 1 o más validaciones de DIAN)")]
		Fallida = 7200004,

		[Description(" ERROR (El xml no es válido) ")]
		Error = 7200005,

	}
}
