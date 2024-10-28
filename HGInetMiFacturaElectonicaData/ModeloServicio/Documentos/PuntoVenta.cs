using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	public class InfoPuntoVenta
	{
		/// <summary>
		/// Corresponde a la Placa de inventario de la Caja
		/// </summary>
		public string PlacaCaja { get; set; }

		/// <summary>
		/// Corresponde a la Ubicación de la caja ALMACEN
		/// </summary>
		public string UbicacionCaja { get; set; }

		/// <summary>
		/// Corresponde a los Nombres y	apellidos del cajero o vendedor
		/// </summary>
		public string Cajero { get; set; }

		/// <summary>
		/// Corresponse al Tipo de Caja 
		/// </summary>
		public string TipoCaja { get; set; }

		/// <summary>
		/// Corresponde al Código de la Venta 
		/// </summary>
		public string CodigoVenta { get; set; }

		/// <summary>
		/// Corresponde al valor del Subtotal de la venta
		/// </summary>
		public decimal SubTotal { get; set; }

	}
}
