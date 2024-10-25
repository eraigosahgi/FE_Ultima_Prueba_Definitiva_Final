using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	public class Prima
	{
		/// <summary>
		/// Cantidad de Días a los cuales corresponde el pago de la Prima legal.
		/// </summary>
		public int Cantidad { get; set; }

		/// <summary>
		/// Valor Pagado por Prima con afectacion Salaria o No salarial.
		/// </summary>
		public NovedadSalNoSal Pago { get; set; }
	}
}
