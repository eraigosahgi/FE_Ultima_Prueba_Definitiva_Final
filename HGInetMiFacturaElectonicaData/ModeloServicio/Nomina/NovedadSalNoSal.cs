using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	/// <summary>
	/// Resporta pagos salariales o no salariales
	/// </summary>
	public class NovedadSalNoSal
	{
		/// <summary>
		/// Valor Pagado Salarial o Compensaciones Ordinarias.
		/// </summary>
		public decimal Pago { get; set; }

		/// <summary>
		///Valor Pagado No Salarial o Compensaciones Extraordinarias. Ocurrencia 0-1
		/// </summary>
		public decimal PagoNS { get; set; }
	}
}
