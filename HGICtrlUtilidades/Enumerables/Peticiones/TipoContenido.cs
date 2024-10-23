using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public enum TipoContenido
	{
		[Description("application/json")]
		Applicationjson = 1,

		[Description("text/plain")]
		Textplain = 2,

		[Description("application/xml")]
		Applicationxml = 3,

		[Description("text/html")]
		Texthtml = 4,
	}
}
