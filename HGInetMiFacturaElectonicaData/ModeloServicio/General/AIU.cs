using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio.General
{
	/// <summary>
	/// Objeto para manejo de AIU
	/// </summary>
	public class AIU
	{

		/// <summary>
		/// Valor de la Administracion
		/// </summary>
		public decimal ValorAdministracion { get; set; }

		/// <summary>
		/// Valor del Imprevisto
		/// </summary>
		public decimal ValorImprevisto { get; set; }

		/// <summary>
		/// Valor de la Utilidad
		/// </summary>
		public decimal ValorUtilidad { get; set; }

		/// <summary>
		/// Valor del porcentaje que se calculo el valor de la utilidad
		/// </summary>
		public decimal ProcentajeUtilidad { get; set; }

	}
}
