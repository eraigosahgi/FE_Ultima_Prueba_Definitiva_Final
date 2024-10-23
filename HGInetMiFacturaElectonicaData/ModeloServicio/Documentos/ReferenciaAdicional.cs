using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	/// <summary>
	/// Permite indicar la referencia a un documento y que tipo de documento es segun listado de la DIAN
	/// </summary>
	public class ReferenciaAdicional
	{
		/// <summary>
		/// Prefijo y Número del documento referenciado
		/// </summary>
		public string Documento { get; set; }

		/// <summary>
		/// Identificador del tipo de documento de referencia segun Listado de la DIAN
		/// </summary>
		public string CodigoReferencia { get; set; }

		/// <summary>
		/// Fecha de emisión del documento referenciado
		/// </summary>
		public DateTime FechaReferencia { get; set; }
	}
}
