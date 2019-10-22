using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	public class CampoValor
	{
		/// <summary>
		/// Descripción del campo segun estandar
		/// </summary>
		public string Descripcion { get; set; }

		/// <summary>
		/// Valor del campo para el llenado en el xml
		/// </summary>
		public string Valor { get; set; }
	}
}
