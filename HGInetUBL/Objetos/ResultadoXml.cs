using HGInetMiFacturaElectonicaData.ModeloServicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBL.Objetos
{

	/// <summary>
	/// Clase con el resultado al generar el xml sin firma
	/// </summary>
	public class ResultadoXml
	{

		public Factura Documento;

		public string RutaXml;

		public string CUFE;
		
	}
}
