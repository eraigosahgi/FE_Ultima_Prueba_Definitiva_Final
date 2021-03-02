using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	public class OtroConcepto
	{

		/// <summary>
		/// Debe ir la Descripcion del Concepto.
		/// </summary>
		public string DescripcionConcepto { get; set; }

		/// <summary>
		/// Valor Pagado por conceto Salaria o No salarial.
		/// </summary>
		public NovedadSalNoSal PagoConcepto { get; set; }

	}
}
