using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{

	/// <summary>
	/// Datos de respuesta de los documentos consultados a la DIAN 
	/// </summary>
	public class RespuestaDian
	{

		/// <summary>
		/// Codigo de respuesta entregado por la DIAN
 		/// </summary>
		public string CodigoRespuesta { get; set; }

		/// <summary>
		/// Descripcion de la respuesta
		/// </summary>
		public string Descripcion { get; set; }

		/// <summary>
		/// Fecha de consulta del documento a la DIAN
		/// </summary>
		public DateTime FechaConsulta { get; set; }

		/// <summary>
		/// Estado del documento Pendiente = 1, Aceptado = 2, Rechazado = 3
		/// </summary>
		public int EstadoDocumento { get; set; }

		/// <summary>
		/// Ruta http del archivo XML de la respuesta de la DIAN relacionado con el documento de consulta.
		/// </summary>
		public string UrlXmlRespuesta { get; set; }


	}
}
