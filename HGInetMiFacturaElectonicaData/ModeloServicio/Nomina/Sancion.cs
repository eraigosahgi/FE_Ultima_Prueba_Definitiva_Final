using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	public class Sancion
	{
		/// <summary>
		/// Valor Pagado correspondiente a Sanción Pública por parte del trabajador.
		/// </summary>
		public decimal SancionPublic { get; set; }

		/// <summary>
		/// Valor Pagado correspondiente a Sanción Privada por parte del trabajador.
		/// </summary>
		public decimal SancionPriv { get; set; }
	}
}
