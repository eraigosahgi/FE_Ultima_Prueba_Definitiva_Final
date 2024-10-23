using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	public class Cesantias
	{
		/// <summary>
		/// Valor Pagado por Cesantías. Ocurrencia 1-1
		/// </summary>
		public decimal Pago { get; set; }

		/// <summary>
		/// Porcentaje de Interés de Cesantías. Ocurrencia 1-1
		/// </summary>
		public decimal Porcentaje { get; set; }

		/// <summary>
		/// Valor Pagado por Intereses de Cesantías. Ocurrencia 1-1
		/// </summary>
		public decimal PagoIntereses { get; set; }
	}
}
