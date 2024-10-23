using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LibreriaGlobalHGInet.RegistroLog
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
