using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio.Documentos
{
	/// <summary>
	/// Cargos que suman a los totales y no afectan los Impuestos
	/// </summary>
	public class Cargo
	{

		/// <summary>
		/// Razon del cargo
		/// </summary>
		public string Descripcion { get; set; }

		/// <summary>
		/// Porcentaje del cargo aplicado
		/// </summary>
		public decimal Porcentaje { get; set; }

		/// <summary>
		/// Valor del cargo aplicado
		/// </summary>
		public decimal Valor { get; set; }

	}
}
