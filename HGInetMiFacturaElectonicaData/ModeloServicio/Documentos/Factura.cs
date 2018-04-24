using HGInetMiFacturaElectonicaData.ModeloServicio.General;
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
		/// Código de seguridad (autenticación)
		/// </summary>
		[Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
		public string DataKey { get; set; }

		/// <summary>
		/// Identificador del Documento asigando por el Facturador Electrónico
		/// </summary>
		[Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
        public string CodigoRegistro { get; set; }

        /// <summary>
        /// Número del Documento
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
        public int Documento { get; set; }

        /// <summary>
        /// Número de Resolución del Documento asignado por la DIAN
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
        [RegularExpression("^\\d+$", ErrorMessage = "El {0} debe contener sólo números.")]
        public string NumeroResolucion { get; set; }

		/// <summary>
		/// Prefijo del Documento según la Resolución asignada por la DIAN
		/// </summary>
		public string Prefijo { get; set; }
		
        /// <summary>
        /// Fecha del Documento
        /// </summary>
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Fecha de Vencimiento del Documento
        /// </summary>
        public DateTime FechaVence { get; set; }

        /// <summary>
        /// Observaciones del documento
        /// </summary>
        public string Nota { get; set; }

		/// <summary>
		/// Código de la moneda según tabla ISO 4217 (ej: COP = Pesos Colombianos).
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
        /// Valor de descuento total del documento
        /// </summary>
        [Range(typeof(decimal), "0", "9999999999.99", ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
        public decimal ValorDescuento { get; set; }

        /// <summary>
        /// Valor total de IVA del documento
        /// </summary>
        [Range(typeof(decimal), "0", "9999999999.99", ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
        public decimal ValorIva { get; set; }

		/// <summary>
		/// Valor total de Retención de IVA del documento
		/// </summary>
		[Range(typeof(decimal), "0", "9999999999.99", ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
        public decimal ValorReteIva { get; set; }

		/// <summary>
		/// Valor total de Impuesto al consumo del documento
		/// </summary>
		[Range(typeof(decimal), "0", "9999999999.99", ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
        public decimal ValorImpuestoConsumo { get; set; }

		/// <summary>
		/// Valor total de Retención en la Fuente del documento
		/// </summary>
		[Range(typeof(decimal), "0", "9999999999.99", ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
        public decimal ValorReteFuente { get; set; }

		/// <summary>
		/// Valor total de Retención de ICA del documento.
		/// </summary>
		[Range(typeof(decimal), "0", "9999999999.99", ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
        public decimal ValorReteIca { get; set; }

        /// <summary>
        /// Valor total del documento: Subtotal incluyendo descuentos e impuestos agregados
        /// </summary>
        [Range(typeof(decimal), "0", "9999999999.99", ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
        public decimal Total { get; set; }

        /// <summary>
        /// Valor neto del documento aplicando impuestos de retención
        /// </summary>
        [Range(typeof(decimal), "0", "9999999999.99", ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
        public decimal Neto { get; set; }

        /// <summary>
        /// Detalle del documento
        /// </summary>
        public List<DocumentoDetalle> DocumentoDetalles { get; set; }

		/// <summary>
		/// Datos del formato del documento
		/// </summary>
		public Formato DocumentoFormato { get; set; }

		#endregion

	}
}