using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public class ObjConfiguracion
	{
		public Guid IdConfiguracion { get; set; }
		public DateTime DatFecha { get; set; }
		public int IntTipo { get; set; }
		public string StrSeguridadUsuario { get; set; }
		public string StrSeguridadClave { get; set; }
		public string StrConexionUrl { get; set; }
		public string StrDescripcion { get; set; }
		public DateTime DatFechaActualizacion { get; set; }
		public short IntIdEstado { get; set; }
		public string StrApiKey { get; set; }
	}
}
