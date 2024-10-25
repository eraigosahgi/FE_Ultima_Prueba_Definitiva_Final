using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	public class Libranza
	{
		/// <summary>
		/// Debe ir la Descripción de la Libranza
		/// </summary>
		public string Descripcion { get; set; }

		/// <summary>
		/// Valor Pagado correspondiente a Aportes a Entidades Financieras por parte del trabajador.
		/// </summary>
		public decimal Deduccion { get; set; }
	}
}
