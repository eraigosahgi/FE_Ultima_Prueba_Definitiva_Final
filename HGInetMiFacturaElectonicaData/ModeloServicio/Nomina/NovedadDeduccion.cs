using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	public class NovedadDeduccion
	{
		/// <summary>
		/// Se debe colocar el Porcentaje que corresponda segun la deduccion que reporta. Ocurrencia 1-1
		/// </summary>
		public decimal Porcentaje { get; set; }

		/// <summary>
		/// Valor Correspondiente a la Suma de todos los Devengos Salariales del trabajador más Pagos No Salariales que superen el 40% del Total de la Remuneración. Ocurrencia 0-1
		/// </summary>
		public decimal ValorBase { get; set; }

		/// <summary>
		/// Valor Pagado correspondiente a la Deduccion por parte del trabajador. Ocurrencia 1-1
		/// </summary>
		public decimal Deduccion { get; set; }
	}
}
