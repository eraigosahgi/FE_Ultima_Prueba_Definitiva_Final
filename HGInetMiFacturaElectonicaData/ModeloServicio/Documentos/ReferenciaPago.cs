using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio.Documentos
{
	public class ReferenciaPago
	{
		/// <summary>
		/// Codigo de EAN de la empresa
		/// </summary>
		public string EAN { get; set; }

		/// <summary>
		/// Primer Codigo de Referencia
		/// </summary>
		public string Referencia { get; set; }

		/// <summary>
		/// Segundo Codigo de Referencia
		/// </summary>
		public string Referencia2 { get; set; }

		/// <summary>
		/// Fecha del Pago (yyyymmdd sin caracteres)
		/// </summary>
		public string Fecha { get; set; }

		/// <summary>
		/// Valor a Pagar
		/// </summary>
		public decimal Valor { get; set; }
	}
}
