using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	/// <summary>
	/// Documento Tipo Attached(Contenedor)
	/// </summary>
	public class Attached
	{
		/// <summary>
		///Consecutivo del attached. 
		/// </summary>
		public string IdAttached { get; set; }

		/// <summary>
		/// Fecha completa del Attached
		/// </summary>
		public DateTime Fecha { get; set; }

		/// <summary>
		/// Identificacion del facturador del documento
		/// </summary>
		public string IdentificacionFacturador { get; set; }

		/// <summary>
		/// Identificacion del facturador del documento
		/// </summary>
		public string Identificacionadquiriente { get; set; }

		/// <summary>
		/// Número del Documento electronico que se incluye en Attached
		/// </summary>
		public long Documento { get; set; }

		/// <summary>
		/// Prefijo del Documento
		/// </summary>
		public string Prefijo { get; set; }

		/// <summary>
		/// Documento Electronico en XML sin la Expresion CDATA
		/// </summary>
		public string DocumentoElectronico { get; set; }

		/// <summary>
		/// ApplicationResponse de la DIAN en XML sin la Expresion CDATA
		/// </summary>
		public string RespuestaDianXml { get; set; }

		///// <summary>
		///// Datos del Obligado a facturar
		///// </summary>
		//public Tercero DatosObligado { get; set; }

		///// <summary>
		///// Datos del Adquiriente de la Factura
		///// </summary>
		//public Tercero DatosAdquiriente { get; set; }

		/// <summary>
		/// Código de respuesta de acuerdo a los valores definidos en Guia de Interoperabilidad en la ​Tabla No. 5.2.1: Códigos de respuesta - Capítulo No. 5.2. 
		/// </summary>
		public string CufeDocumentoElectronico { get; set; }

		/// <summary>
		/// Descripción que complementa o justifica el código de respuesta reportado
		/// </summary>
		public string MvoRespuesta { get; set; }
	}
}
