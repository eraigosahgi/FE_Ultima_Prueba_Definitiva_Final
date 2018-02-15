using System;
using System.Collections.Generic;
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
		public int Codigo { get; set; }

		/// <summary>
		/// Codigo del Producto
		/// </summary>
		public string ProductoCodigo { get; set; }

		/// <summary>
		/// Nombre del Producto
		/// </summary>
		public string ProductoNombre { get; set; }

		/// <summary>
		/// Descripcion del producto
		/// </summary>
		public string ProductoDescripcion { get; set; }

		/// <summary>
		/// Cantidad de producto del detalle
		/// </summary>
		public decimal Cantidad { get; set; }

		/// <summary>
		/// Subtotal de cada detalle 
		/// </summary>
		public decimal ValorSubtotal { get; set; }

		/// <summary>
		/// Valor del Impuesto al Consumo del detalle
		/// </summary>
		public decimal ValorImpuestoConsumo { get; set; }

		/// <summary>
		/// Valor del Iva del detalle
		/// </summary>
		public decimal IvaValor { get; set; }

		/// <summary>
		/// Porcentaje del Iva aplicado del detalle
		/// </summary>
		public decimal IvaPorcentaje { get; set; }

		/// <summary>
		/// Valor del Descuento del detalle
		/// </summary>
		public decimal DescuentoValor { get; set; }

		/// <summary>
		/// Porcentaje de descuento aplicado en cada detalle
		/// </summary>
		public decimal DescuentoPorcentaje { get; set; }

		/// <summary>
		/// Valor Unitario sin impuestos
		/// </summary>
		public decimal ValorUnitario { get; set; }

		#endregion


	}
}
