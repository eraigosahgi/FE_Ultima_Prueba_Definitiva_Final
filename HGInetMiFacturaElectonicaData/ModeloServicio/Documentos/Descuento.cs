using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio.Documentos
{
	/// <summary>
	/// Objeto de Descuento otorgados en el total listado DIAN 6.3.8
	/// </summary>
	public class Descuento
	{

		/// <summary>
		/// Codigo de tipo de descuento otorgado
		/// </summary>
		public string Codigo { get; set; }

		/// <summary>
		/// Porcentaje de descuento aplicado
		/// </summary>
		public decimal Porcentaje { get; set; }

		/// <summary>
		/// Valor de descuento aplicado
		/// </summary>
		public decimal Valor { get; set; }

		/// <summary>
		/// Descripcion del descuento otorgado
		/// </summary>
		public string Descripcion { get; set; }
	}
}
