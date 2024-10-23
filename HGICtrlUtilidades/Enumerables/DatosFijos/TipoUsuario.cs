using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public enum TipoUsuario
	{
		[Description("Usuario Interno")]
		Usuario_Interno = 0,

		[Description("Usuario Externo")]
		Usuario_Externo = 1,
	}
}
