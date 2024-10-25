using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	public class ReferenciaDocumento
	{
		/// <summary>
		/// Prefijo del Documento de referencia que se realizo el recaudo
		/// </summary>
		public string PrefijoDocumentoRef { get; set; }

		/// <summary>
		/// Reporta el numero de documento que se realizo el recaudo
		/// </summary>
		public string DocumentoRef { get; set; }

		/// <summary>
		/// Cufe o Cude identificador unico de la documento referenciado
		/// </summary>
		public string CufeDocumentoRef { get; set; }

		/// <summary>
		/// Fecha del documento referenciado
		/// </summary>
		public DateTime FechaDocumentoRef { get; set; }

		/// <summary>
		/// Tipo Operacion del documento (6.1.3. Tipo de Documento: para documentos electronico , documento talonario y Pos "_n_"  )
		/// </summary>
		public string TipoDocumento { get; set; }
	}
}
