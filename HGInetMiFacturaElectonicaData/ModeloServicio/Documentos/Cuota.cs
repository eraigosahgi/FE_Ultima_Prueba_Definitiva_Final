using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	/// <summary>
	/// Cuotas del documento de Factura
	/// </summary>
	public class Cuota
	{
		/// <summary>
		/// Id incrementable de cada cuota
		/// </summary>
		[Required(ErrorMessage = "{0} es un campo obligatorio")]
		public int Codigo { get; set; }

		/// <summary>
		/// Valor de la Cuota
		/// </summary>
		[Range(typeof(decimal), "0", "9999999999.99", ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
		public decimal Valor { get; set; }

		/// <summary>
		/// Fecha de Vencimiento del Documento
		/// </summary>
		public DateTime FechaVence { get; set; }
	}
}
