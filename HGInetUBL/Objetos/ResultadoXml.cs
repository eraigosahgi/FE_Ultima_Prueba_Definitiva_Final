using HGInetMiFacturaElectonicaData.ModeloServicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HGInetUBL.Objetos
{

	/// <summary>
	/// Clase con el resultado del xml
	/// </summary>
	public class ResultadoXml
	{

		public Factura Documento;

		public string NombreXml;

		public string CUFE;

		public StringBuilder DocumentoXml;
		
	}
}
