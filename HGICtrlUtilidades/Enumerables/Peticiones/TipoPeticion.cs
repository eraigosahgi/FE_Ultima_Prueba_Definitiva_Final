using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public enum TipoPeticion
	{
		[Description("GET")]
		GET = 1,

		[Description("POST")]
		POST = 2,

		[Description("PUT")]
		PUT = 3,

		[Description("DELETE")]
		DELETE = 4,
	}
}
