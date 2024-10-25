using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	/// <summary>
	/// Datos adicionales del documento XML
	/// </summary>
	public class ExtensionDian
	{

        /// <summary>
        /// Código del País de Expedición del documento (CO)
        /// </summary>
        public string CodPaisExpedicion { get { return "CO"; } }

        /// <summary>
        /// Tipo de Documento Segun la DIAN { get; set; } 1-Factura, 2-Nota Credito, 3-Nota Debito
        /// </summary>
        public int TipoDocumento { get; set; }

        /// <summary>
        /// Clave Tecnica otorgada por la DIAN al Obligado a Facturar
        /// </summary>
        public string ClaveTecnicaDIAN { get; set; }

        /// <summary>
        /// Número de Resolución del documento Factura*
        /// </summary>
        public string NumResolucion { get; set; }

        /// <summary>
		/// Prefijo del documento Factura*
		/// </summary>
		public string Prefijo { get; set; }

        /// <summary>
        /// Fecha Inicio de Resolucion de Factura*
        /// </summary>
        public DateTime FechaResIni { get; set; }

		/// <summary>
		/// Fecha Final de Resolucion de Factura
		/// </summary>
		public DateTime FechaResFin { get; set; }

		/// <summary>
		/// Rango inicial de Resolucion de Factura
		/// </summary>
		public int RangoIni { get; set; }

		/// <summary>
		/// Rango Final de Resolucion de Factura
		/// </summary>
		public int RangoFin { get; set; }
        
        /// <summary>
		/// Número de Identificación del Proveedor Tecnológico
		/// </summary>
		public string NitProveedor { get; set; }


        /// <summary>
        /// Identificador del software proporcionado en la plataforma de la DIAN
        /// </summary>
        public string IdSoftware { get; set; }

		/// <summary>
		/// Pin del software proporcionado en la plataforma de la DIAN
		/// </summary>
		public string PinSoftware { get; set; }

	}
}
