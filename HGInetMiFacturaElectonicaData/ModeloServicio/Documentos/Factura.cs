using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
    /// <summary>
    /// Documento tipo Factura de Venta
    /// </summary>
    public class Factura
    {
        #region Propiedades

        /// <summary>
        /// Id único de Registro del Documento
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
        public string CodigoRegistro { get; set; }

        /// <summary>
        /// Numero de Documento
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
        public int Documento { get; set; }

		/// <summary>
		/// Número de Resolución
		/// </summary>
		[Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
        [RegularExpression("^\\d+$", ErrorMessage = "El {0} debe contener sólo números.")]
        public string NumeroResolucion { get; set; }

		/// <summary>
		/// Prefijo segun Resolución
		/// </summary>
		public string Prefijo { get; set; }

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
        [Required(ErrorMessage = "{0} es un campo obligatorio")]
        [MaxLength(3, ErrorMessage = "La {0} no puede superar los {1} caracteres")]
        public string Moneda { get; set; }

        /// <summary>
        /// Datos del Obligado a facturar
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
        public Tercero DatosObligado { get; set; }

        /// <summary>
        /// Datos del Adquiriente de la Factura
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
        public Tercero DatosAdquiriente { get; set; }

        /// <summary>
        /// Valor del documento sin descuentos y sin impuestos
        /// </summary>
        [Required(ErrorMessage = "{0} es un campo obligatorio")]
        [Range(typeof(decimal), "0", "9999999999.99", ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
        public decimal Valor { get; set; }

        /// <summary>
        /// Subtotal del documento: valor del documento con descuentos y sin impuestos
        /// </summary>
        [Required(ErrorMessage = "{0} es un campo obligatorio")]
        [Range(typeof(decimal), "0", "9999999999.99", ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
        public decimal ValorSubtotal { get; set; }

        /// <summary>
        /// Descuento total del documento
        /// </summary>
        [Range(typeof(decimal), "0", "9999999999.99", ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
        public decimal ValorDescuento { get; set; }

        /// <summary>
        /// Iva Total del documento
        /// </summary>
        [Range(typeof(decimal), "0", "9999999999.99", ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
        public decimal ValorIva { get; set; }

        /// <summary>
        /// ReteIva total del documento
        /// </summary>
        [Range(typeof(decimal), "0", "9999999999.99", ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
        public decimal ValorReteIva { get; set; }

        /// <summary>
        /// Impuesto al consumo total del documento
        /// </summary>
        [Range(typeof(decimal), "0", "9999999999.99", ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
        public decimal ValorImpuestoConsumo { get; set; }

        /// <summary>
        /// ReteFuente total del documento
        /// </summary>
        [Range(typeof(decimal), "0", "9999999999.99", ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
        public decimal ValorRetefuente { get; set; }

        /// <summary>
        /// ReteIca total del documento
        /// </summary>
        [Range(typeof(decimal), "0", "9999999999.99", ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
        public decimal ValorReteIca { get; set; }

        /// <summary>
        /// Total del documento: Subtotal incluyendo impuestos
        /// </summary>
        [Range(typeof(decimal), "0", "9999999999.99", ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
        public decimal Total { get; set; }

        /// <summary>
        /// Neto del documento: Total menos Retenciones.
        /// </summary>
        [Range(typeof(decimal), "0", "9999999999.99", ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
        public decimal Neto { get; set; }

        /// <summary>
        /// Detalle del documento
        /// </summary>
        public List<DocumentoDetalle> DocumentoDetalles { get; set; }

        #endregion

    }
}