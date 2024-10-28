using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	public class FondoSP
	{
		/// <summary>
		/// Debe corresponder a 1% según la ley.
		/// </summary>
		public decimal Porcentaje { get; set; }

		/// <summary>
		/// Valor Pagado correspondiente a Fondo de Solidaridad Pensional por parte del trabajador.
		/// </summary>
		public decimal DeduccionFSP { get; set; }

		/// <summary>
		/// Se debe colocar el Porcentaje que correspondiente al Fondo de Subsistencia correspondiente.
		/// </summary>
		public decimal PorcentajeSub { get; set; }

		/// <summary>
		/// Valor Pagado correspondiente a Fondo de Subsistencia por parte del trabajador.
		/// </summary>
		public decimal DeduccionSub { get; set; }

	}
}
