using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public enum TipoPersona
	{
		[Description("Natural")]
		Natural = 1,

		[Description("Jurídica")]
		Juridica = 2,
	}
}
