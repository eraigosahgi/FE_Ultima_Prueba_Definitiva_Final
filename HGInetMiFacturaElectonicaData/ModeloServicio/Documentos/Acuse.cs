using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{

    /// <summary>
    /// Documento tipo Acuse de Recibo
    /// </summary>
    public class Acuse
    {

        /// <summary>
        ///Identificador del acuse generado por el proveedor receptor del documento electrónico. 
        /// </summary>
        public string IdAcuse { get; set; }

        /// <summary>
        /// Codigo de seguridad del documento generado por la Plataforma (Tracking Id)
        /// </summary>
        public string IdSeguridad { get; set; }

        /// <summary>
        /// Número del Documento que se le dio acuse
        /// </summary>
        public long Documento { get; set; }

		/// <summary>
		/// Prefijo del Documento
		/// </summary>
		public string Prefijo { get; set; }

		/// <summary>
		/// Código de documento de acuerdo a los valores definidos en Guia de Interoperabilidad en la ​Tabla No. 5.2.1: Códigos de respuesta - Capítulo No. 5.2. 
		/// </summary>
		public string TipoDocumento { get; set; }

        /// <summary>
        /// Fecha completa del Acuse de Recibo
        /// </summary>
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Datos del Obligado a facturar
        /// </summary>
        public Tercero DatosObligado { get; set; }

        /// <summary>
        /// Datos del Adquiriente de la Factura
        /// </summary>
        public Tercero DatosAdquiriente { get; set; }

        /// <summary>
        /// Código de respuesta de acuerdo a los valores definidos en Guia de Interoperabilidad en la ​Tabla No. 5.2.1: Códigos de respuesta - Capítulo No. 5.2. 
        /// </summary>
        public string CodigoRespuesta { get; set; }

        /// <summary>
        /// Descripción que complementa o justifica el código de respuesta reportado
        /// </summary>
        public string MvoRespuesta { get; set; }


    }
}
