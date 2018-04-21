using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	/// <summary>
	/// Datos de respuesta para la resolución
	/// </summary>
	public class Resolucion
	{
		public string NumeroResolucion { get; set; }
		public DateTime FechaResolucion { get; set; }
		public string Prefijo { get; set; }
		public int RangoInicial { get; set; }
		public int RangoFinal { get; set; }
		public DateTime FechaVigenciaInicial { get; set; }
		public DateTime FechaVigenciaFinal { get; set; }
		public string ClaveTecnica { get; set; }

	}
}
