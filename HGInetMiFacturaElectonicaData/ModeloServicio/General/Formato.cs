using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	/// <summary>
	/// Información adicional del documento para generar el formato
	/// </summary>
	public class Formato
	{
		/// <summary>
		/// Código del formato
		/// </summary>
		public int Codigo { get; set; }

		/// <summary>
		/// Campos adicionales predeterminados en el formato
		/// </summary>
		public List<FormatoCampo> CamposPredeterminados { get; set; }

		/// <summary>
		/// Archivo PDF 
		/// </summary>
		public string ArchivoPdf { get; set; }

		/// <summary>
		/// Título del formato (ejemplo: Factura de Venta)
		/// </summary>
		public string Titulo { get; set; }
	}
}
