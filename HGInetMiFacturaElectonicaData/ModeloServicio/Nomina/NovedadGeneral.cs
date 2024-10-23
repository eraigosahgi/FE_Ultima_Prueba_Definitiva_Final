using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	public class NovedadGeneral
	{
		/// <summary>
		/// Tipo de Novedad a reportar según Clasificación DIAN. Ocurrencia 1-1
		/// </summary>
		public int Tipo { get; set; }

		/// <summary>
		/// En formato YYYY-MM-DD. Ocurrencia 0-1
		/// </summary>
		public DateTime FechaInicio { get; set; }

		/// <summary>
		/// En formato YYYY-MM-DD. Ocurrencia 0-1
		/// </summary>
		public DateTime FechaFin { get; set; }

		/// <summary>
		/// Cantidad de Dias u Horas Segun novedad reportada. Ocurrencia 1-1
		/// </summary>
		public int Cantidad { get; set; }

		/// <summary>
		/// Valor Pagado por Vacaciones. Ocurrencia 1-1
		/// </summary>
		public decimal Pago { get; set; }


	}
}
