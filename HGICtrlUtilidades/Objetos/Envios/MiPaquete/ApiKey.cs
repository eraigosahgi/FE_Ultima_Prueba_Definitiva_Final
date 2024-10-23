using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades.Objetos.Envios
{
	public class ApiKey
	{
		public string email { get; set; }
		public string password { get; set; }
	}

	public class ApiKeyResponse
	{
		public string APIKey { get; set; }
		public string URL { get; set; }
	}

}
