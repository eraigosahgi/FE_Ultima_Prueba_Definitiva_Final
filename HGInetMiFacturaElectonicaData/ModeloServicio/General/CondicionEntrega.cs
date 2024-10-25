using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	public class CondicionEntrega
	{

		/// <summary>
		/// Método de pago de costes de transporte: Se utilizar para indicar cómo se pagan los costes del transporte
		/// (por ejemplo, Portes Debidos, Portes Pagados) Puede ser un texto libre que entiendan el comprador y vendedor o codificarlo en una lista.
		/// por ejemplo http://www.unece.org/trade/untdid/d01b/tred/tred4215.htm
		/// </summary>
		public string TerminosEntrega { get; set; }

		/// <summary>
		/// Condiciones de Entrega: Obligatorio cuando sea una factura de exportación
		/// Ver lista de valores en 6.3.6
		/// </summary>
		public string CodCondicionEntrega { get; set; }

	}
}
