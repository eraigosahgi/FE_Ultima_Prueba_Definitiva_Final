using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	class Cl_MailObjetos
	{
	}

	public class Cl_MailCliente
	{
		public string Servidor { get; set; }
		public int Puerto { get; set; }
		public string Usuario { get; set; }
		public string Clave { get; set; }
		public bool Habilitar_ssl { get; set; }
		public int TimeOut { get; set; }
	}

	public class Cl_MailAdjuntos
	{
		public int Cantidad { get; set; }

		public long TamanoTotal { get; set; }
	}
}
