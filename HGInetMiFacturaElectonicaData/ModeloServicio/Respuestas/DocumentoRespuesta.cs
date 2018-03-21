using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreriaGlobalHGInet.Error;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	/// <summary>
	/// Datos de respuesta para los documentos
	/// </summary>
	public class DocumentoRespuesta
	{
		public string IdDocumento { get; set; }
		public DateTime FechaRecepcion { get; set; }
		public string CodigoRegistro { get; set; }
		public string Identificacion { get; set; }
		public int NumeroResolucion { get; set; }
		public string Prefijo { get; set; }
		public int Documento { get; set; }
		public string Cufe { get; set; }
		public int IdProceso { get; set; }
		public string DescripcionProceso { get; set; }
		public int Aceptacion { get; set; }
		public string MotivoRechazo { get; set; }
		public string UrlXmlUbl { get; set; }
		public string UrlPdf { get; set; }
		public DateTime FechaUltimoProceso { get; set; }
		public int IdUltimoProceso { get; set; }
		public string UltimoProceso { get; set; }
		public int ProcesoFinalizado { get; set; }
		public Error Error { get; set; }


	}
}
