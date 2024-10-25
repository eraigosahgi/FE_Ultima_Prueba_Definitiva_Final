using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
    public class RespuestaFactura
    {

        #region Propiedades

        /// <summary>
        /// Id único del documento generado por la Plataforma
        /// </summary>
        public string IdDocumento { get; set; }

        /// <summary>
        /// Fecha de recepción del documento por la Plataforma
        /// </summary>
        public DateTime FechaRecepcion { get; set; }

        /// <summary>
        /// Identificacion del Obligado a Facturar
        /// </summary>
        public string IdObligado { get; set; }

        /// <summary>
        /// Identificación Adquiriente 
        /// </summary>
        public string IdAdquiriente { get; set; }
        
        /// <summary>
        /// Número de Resolución asignado por la DIAN.
        /// </summary>
        public string NumeroResolucion { get; set; }

        /// <summary>
        /// Prefijo de la Factura
        /// </summary>
        public string Prefijo { get; set; }

        /// <summary>
        /// Número de Documento
        /// </summary>
        public int Documento { get; set; }

        /// <summary>
        /// Código identificador del documento ante la DIAN
        /// </summary>
        public string Cufe { get; set; }

        /// <summary>
        /// Codigo del estado del Documento
        /// </summary>
        public int CodigoEstado { get; set; }

        /// <summary>
        /// Descripcion del Estado del Documento
        /// </summary>
        public string DescripcionEstado { get; set; }

        /// <summary>
        /// Indica la aceptación o no del documento (0: rechazo y 1: aceptación)
        /// </summary>
        public int Aceptacion { get; set; }

        /// <summary>
        /// Observaciones del Adquiriente de acuerdo con el rechazo del documento.
        /// </summary>
        public string MotivoRechazo { get; set; }


        #endregion

    }
}
