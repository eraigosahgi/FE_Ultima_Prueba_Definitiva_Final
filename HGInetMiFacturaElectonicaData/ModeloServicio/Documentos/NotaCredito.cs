using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
    /// <summary>
    /// Documento tipo Nota Credito
    /// </summary>
    public class NotaCredito
    {
        #region Propiedades

        /// <summary>
        /// Id único de Registro del Obligado a Facturar
        /// </summary>
        public int CodigoRegistro { get; set; }

        /// <summary>
        /// Numero de Documento
        /// </summary>
        public int Documento { get; set; }

        /// <summary>
        /// Documento afectado
        /// </summary>
        public string DocumentoRef { get; set; }

        /// <summary>
        /// Cufe identificador unico de la Factura
        /// </summary>
        public string Cufe { get; set; }

        /// <summary>
        /// Fecha de documento
        /// </summary>
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Observaciones del documento
        /// </summary>
        public string Nota { get; set; }

        /// <summary>
        /// Codigo Moneda del documento segun tabla DIAN
        /// </summary>
        public string Moneda { get; set; }

        /// <summary>
        /// Datos del Obligado a facturar
        /// </summary>
        public Tercero DatosObligado { get; set; }

        /// <summary>
        /// Datos del Adquiriente de la Factura
        /// </summary>
        public Tercero DatosAdquiriente { get; set; }

        /// <summary>
        /// Valor del documento sin descuentos y sin impuestos
        /// </summary>
        public decimal Valor { get; set; }

        /// <summary>
        /// Subtotal del documento: valor del documento con descuentos y sin impuestos
        /// </summary>
        public decimal ValorSubtotal { get; set; }

        /// <summary>
        /// Descuento total del documento
        /// </summary>
        public decimal ValorDescuento { get; set; }

        /// <summary>
        /// Iva Total del documento
        /// </summary>
        public decimal ValorIva { get; set; }

        /// <summary>
        /// ReteIva total del documento
        /// </summary>
        public decimal ValorReteIva { get; set; }

        /// <summary>
        /// Impuesto al consumo total del documento
        /// </summary>
        public decimal ValorImpuestoConsumo { get; set; }

        /// <summary>
        /// ReteFuente total del documento
        /// </summary>
        public decimal ValorRetefuente { get; set; }

        /// <summary>
        /// Total del documento: Subtotal incluyendo impuestos
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// Neto del documento: Total menos Retenciones.
        /// </summary>
        public decimal Neto { get; set; }

        /// <summary>
        /// Detalle del documento
        /// </summary>
        public List<DocumentoDetalle> DocumentoDetalles { get; set; }

        #endregion
    }
}
