using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public enum MotorBd
	{
		[Description("Sql")]
		Sql = 0,

		[Description("MongoDB")]
		MongoDB = 1,
	}
}
