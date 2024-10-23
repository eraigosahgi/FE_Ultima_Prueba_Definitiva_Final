using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{

	public class Transporte
	{
		/// <summary>
		/// Valor de Auxilio de Transporte que recibe el trabajador por ley, según aplique. Ocurrencia 0-1
		/// </summary>
		public decimal AuxilioTransporte { get; set; }

		/// <summary>
		/// Valor de Viáticos, Manutención y Alojamiento de carácter Salarial. Ocurrencia 0-1
		/// </summary>
		public decimal ViaticoManuAlojS { get; set; }

		/// <summary>
		/// Valor de Viáticos, Manutención y Alojamiento de carácter No Salarial. Ocurrencia 0-1
		/// </summary>
		public decimal ViaticoManuAlojNS { get; set; }

	}
}
