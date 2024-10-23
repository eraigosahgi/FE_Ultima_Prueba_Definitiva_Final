using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public enum InterfazDocumento
	{
		[Description("Documento")]
		Documento = 1,

		[Description("Dcto Ref")]
		DctoRef = 2,

		[Description("Ref Pago")]
		RefPago = 3,

		[Description("Dcto Banco")]
		DctoBanco = 4,

		[Description("Periodo")]
		Periodo = 5,

		[Description("Inmueble")]
		Inmueble = 6
	}
}