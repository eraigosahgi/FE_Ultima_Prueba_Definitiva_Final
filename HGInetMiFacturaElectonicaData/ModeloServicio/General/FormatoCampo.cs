using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	/// <summary>
	/// Campos configurables del formato
	/// </summary>
	public class FormatoCampo
	{		

		/// <summary>
		/// Descripción del campo para el formato y la nota en el xml
		/// </summary>
		public string Descripcion { get; set; }

		/// <summary>
		/// Valor del campo para el formato y la nota en el xml
		/// </summary>
		public string Valor { get; set; }

		/// <summary>
		/// Indica la ubicación del campo
		/// </summary>
		public string Ubicacion { get; set; }

	}
}
