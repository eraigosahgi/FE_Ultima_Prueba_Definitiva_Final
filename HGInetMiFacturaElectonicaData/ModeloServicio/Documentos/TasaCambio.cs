using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	/// <summary>
	/// Tasa de cambio de moneda extranjera
	/// </summary>
	public class TasaCambio
	{
		/// <summary>
		/// Valor de la Tasa de Cambio
		/// </summary>
		public decimal Valor { get; set; }

		/// <summary>
		/// Código de la moneda de la Divisa según tabla ISO 4217 (ej: USD = Dólar estadounidense).
		/// </summary>
		public string Moneda { get; set; }

		/// <summary>
		/// Fecha en la que se fijó la tasa de cambio 
		/// </summary>
		public DateTime FechaTrm { get; set; }
	}
}
