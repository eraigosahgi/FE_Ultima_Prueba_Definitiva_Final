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
        /// Codigo del Producto
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
        public string ProductoCodigo { get; set; }

        /// <summary>
        /// Nombre del Producto
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
        public string ProductoNombre { get; set; }

        /// <summary>
        /// Descripcion del producto
        /// </summary>
        public string ProductoDescripcion { get; set; }

        /// <summary>
        /// Cantidad de producto del detalle
        /// </summary>
        [Required(ErrorMessage = "{0} es un campo obligatorio")]
        [Range(typeof(decimal), "0", "9999999999.99", ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
        public decimal Cantidad { get; set; }

        /// <summary>
        /// Valor Unitario sin impuestos
        /// </summary>
        [Required(ErrorMessage = "{0} es un campo obligatorio")]
        [Range(typeof(decimal), "0", "9999999999.99", ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
        public decimal ValorUnitario { get; set; }

        /// <summary>
        /// Subtotal de cada detalle 
        /// </summary>
        [Required(ErrorMessage = "{0} es un campo obligatorio")]
        [Range(typeof(decimal), "0", "9999999999.99", ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
        public decimal ValorSubtotal { get; set; }

        /// <summary>
        /// Valor del Impuesto al Consumo del detalle
        /// </summary>
        public decimal ValorImpuestoConsumo { get; set; }

        /// <summary>
        /// Porcentaje del Iva aplicado del detalle
        /// </summary>
        public decimal IvaPorcentaje { get; set; }

        /// <summary>
        /// Valor del Iva del detalle
        /// </summary>
        public decimal IvaValor { get; set; }

        /// <summary>
        /// Porcentaje de descuento aplicado en cada detalle
        /// </summary>
        public decimal DescuentoPorcentaje { get; set; }

        /// <summary>
        /// Valor del Descuento del detalle
        /// </summary>
        public decimal DescuentoValor { get; set; }


        #endregion


    }
}
