using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HGICtrlUtilidades
{
	public class LogMensaje
	{
		[XmlArray(ElementName = "Detalles")]
		[XmlArrayItem(ElementName = "Detalle")]
		public List<LogDetalle> Detalle { get; set; }

		private String excepcion;

		public String Excepcion
		{
			get
			{
				return excepcion;
			}
			set { excepcion = value; }
		}

		public LogMensaje()
		{
			Detalle = new List<LogDetalle>();
			this.Excepcion = string.Empty;
		}
	}
}
