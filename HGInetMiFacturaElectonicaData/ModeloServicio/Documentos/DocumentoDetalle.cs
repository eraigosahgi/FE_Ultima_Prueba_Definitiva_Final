using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
    /// <summary>
    /// Detalles del documento (productos y/o servicios)
    /// </summary>
    public class DocumentoDetalle
    {

        #region Propiedades 

        /// <summary>
        /// Id incrementable de cada detalle del documento
        /// </summary>
        [Required(ErrorMessage = "{0} es un campo obligatorio")]
        public int Codigo { get; set; }

		/// <summary>
		/// Código del producto y/o servicio
		/// </summary>
		[Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
        public string ProductoCodigo { get; set; }

		/// <summary>
		/// Nombre del producto y/o servicio
		/// </summary>
		[Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
        public string ProductoNombre { get; set; }

		/// <summary>
		/// Descripcion del producto y/o servicio
		/// </summary>
		public string ProductoDescripcion { get; set; }

		/// <summary>
		/// Cantidad del producto y/o servicio
		/// </summary>
		[Required(ErrorMessage = "{0} es un campo obligatorio")]
        [Range(typeof(decimal), "0", "9999999999.99", ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
        public decimal Cantidad { get; set; }

		/// <summary>
		/// Valor unitario del producto y/o servicio; sin aplicar descuentos e impuestos
		/// </summary>
		[Required(ErrorMessage = "{0} es un campo obligatorio")]
        [Range(typeof(decimal), "0", "9999999999.99", ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
        public decimal ValorUnitario { get; set; }

        /// <summary>
        /// Valor subtotal del detalle 
        /// </summary>
        [Required(ErrorMessage = "{0} es un campo obligatorio")]
        [Range(typeof(decimal), "0", "9999999999.99", ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
        public decimal ValorSubtotal { get; set; }

		/// <summary>
		/// Porcentaje aplicado de Impuesto al Consumo al Subtotal
		/// </summary>
		public decimal ImpoConsumoPorcentaje { get; set; }


		/// <summary>
		/// Valor del Impuesto al Consumo del detalle
		/// </summary>
		public decimal ValorImpuestoConsumo { get; set; }

		/// <summary>
		/// Porcentaje del IVA del producto y/o servicio
		/// </summary>
		public decimal IvaPorcentaje { get; set; }

        /// <summary>
        /// Valor del IVA aplicado al detalle
        /// </summary>
        public decimal IvaValor { get; set; }

        /// <summary>
        /// Porcentaje del ReteICA aplicado al detalle
        /// </summary>
        public decimal ReteIcaPorcentaje { get; set; }

        /// <summary>
        /// Valor del ReteICA aplicado al detalle
        /// </summary>
        public decimal ReteIcaValor { get; set; }

        /// <summary>
        /// Porcentaje aplicado de Rentencion en la Fuente al Subtotal
        /// </summary>
        public decimal ReteFuentePorcentaje { get; set; }

        /// <summary>
        /// Valor de la Retencion a la Fuente
        /// </summary>
        public decimal ReteFuenteValor { get; set; }

        /// <summary>
        /// Porcentaje de descuento aplicado al detalle
        /// </summary>
        public decimal DescuentoPorcentaje { get; set; }

        /// <summary>
        /// Valor de descuento aplicado al detalle
        /// </summary>
        public decimal DescuentoValor { get; set; }
		
		/// <summary>
		/// Indica que el producto es gratuito (true) o no (false)
		/// </summary>
		public bool ProductoGratis { get; set; }

		/// <summary>
		/// Código de la unidad de medida del detalle (unidad predeterminada = S7)
		/// http://www.datypic.com/sc/ubl20/t-clm66411_UnitCodeContentType.html
		/// </summary>
		public string UnidadCodigo { get; set; }


		#endregion


	}
}
