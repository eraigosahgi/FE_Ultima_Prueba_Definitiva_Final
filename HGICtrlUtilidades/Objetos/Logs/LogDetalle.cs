using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public class LogDetalle
	{
		public String Linea { get; set; }
		public String Archivo { get; set; }
		public String Clase { get; set; }
		public String Metodo { get; set; }
		public String Modulo { get; set; }

		public LogDetalle()
		{
			this.Linea = String.Empty;
			this.Archivo = String.Empty;
			this.Clase = String.Empty;
			this.Metodo = String.Empty;
			this.Modulo = String.Empty;
		}

		public LogDetalle(String linea, String archivo, String clase, String metodo, String modulo)
		{
			this.Linea = linea;
			this.Archivo = archivo;
			this.Clase = clase;
			this.Metodo = metodo;
			this.Modulo = modulo;
		}
	}
}
