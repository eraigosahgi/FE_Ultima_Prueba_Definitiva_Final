using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HGICtrlUtilidades
{
	public class LogArchivo
	{
		[XmlArray(ElementName = "Logs")]
		[XmlArrayItem(ElementName = "Log")]
		public List<LogClase> Logs { get; set; }

		public LogArchivo()
		{
			Logs = new List<LogClase>();
		}
	}
}
