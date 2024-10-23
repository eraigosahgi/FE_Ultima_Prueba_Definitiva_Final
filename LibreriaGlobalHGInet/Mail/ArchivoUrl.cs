using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;

namespace LibreriaGlobalHGInet.Mail
{
	public class ArchivoUrl
	{
		
		public Stream archivo { get; set; }

		public ContentType contenido { get; set; }

		public string name { get; set; }

	}
}
