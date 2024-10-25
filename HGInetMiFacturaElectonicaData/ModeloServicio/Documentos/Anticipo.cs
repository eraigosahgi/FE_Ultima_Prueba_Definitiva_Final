using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio.Documentos
{
	/// <summary>
	/// Anticipo generado al documento
	/// </summary>
	public class Anticipo
	{
		/// <summary>
		/// Identificación del pago del anticipo 
		/// </summary>
		public string Codigo { get; set; }

		/// <summary>
		/// Valor del pago del anticipo
		/// </summary>
		public decimal Valor { get; set; }

		/// <summary>
		/// Fecha en la cual el pago fue recibido.(*) Requerido para el sector Salud
		/// </summary>
		public DateTime Fecha { get; set; }


	}
}
