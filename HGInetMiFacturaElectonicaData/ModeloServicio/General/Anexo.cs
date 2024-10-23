using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{

	/// <summary>
	/// Archivos anexos enviados con los documentos
	/// </summary>
	public class Anexo
	{

		/// <summary>
		/// Archivo adjuntos en Base64
		/// </summary>
		public string Archivo { get; set; }

		/// <summary>
		/// Observacion de los adjuntos enviados
		/// </summary>
		public string Anotacion { get; set; }

		/// <summary>
		/// Url del cliente dispuesta para los adjuntos
		/// </summary>
		public string Url { get; set; }

	}
}
