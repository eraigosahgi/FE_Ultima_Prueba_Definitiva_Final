using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public class ArchivoUrl
	{
		public Stream archivo { get; set; }
		public ContentType contenido { get; set; }
		public string name { get; set; }
	}
}
