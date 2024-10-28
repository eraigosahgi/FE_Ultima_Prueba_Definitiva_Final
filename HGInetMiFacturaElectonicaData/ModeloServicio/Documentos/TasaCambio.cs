using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	/// <summary>
	/// Tasa de cambio de moneda extranjera
	/// </summary>
	public class TasaCambio
	{
		/// <summary>
		/// Valor de la Tasa de Cambio
		/// </summary>
		public decimal Valor { get; set; }

		/// <summary>
		/// Código de la moneda de la Divisa según tabla ISO 4217 (ej: USD = Dólar estadounidense).
		/// </summary>
		public string Moneda { get; set; }

		/// <summary>
		/// Fecha en la que se fijó la tasa de cambio 
		/// </summary>
		public DateTime FechaTrm { get; set; }

		/// <summary>
		/// Valor bruto antes de tributos
		/// </summary>
		public decimal FctConvCop { get; set; }

		/// <summary>
		/// Sub Total
		/// </summary>
		public decimal SubTotalCop { get; set; }

		/// <summary>
		/// Campo para informar el total de los descuentos
		/// </summary>
		public decimal DescuentoDetalleCop { get; set; }

		/// <summary>
		/// Campo para informar el total de los recargos
		/// </summary>
		public decimal RecargoDetalleCop { get; set; }

		/// <summary>
		/// Campo para informar el total bruto
		/// </summary>
		public decimal TotalBrutoFacturaCop { get; set; }

		/// <summary>
		/// Campo para informar el total IVA
		/// </summary>
		public decimal TotIvaCop { get; set; }

		/// <summary>
		/// Campo para informar el total INC
		/// </summary>
		public decimal TotIncCop { get; set; }

		/// <summary>
		/// Campo para informar el total Imp Bolsa
		/// </summary>
		public decimal TotBolCop { get; set; }

		/// <summary>
		/// Campo para informar el total Otro Imp
		/// </summary>
		public decimal ImpOtroCop { get; set; }

		/// <summary>
		/// Campo para informar el total Mnt Imp
		/// </summary>
		public decimal MntImpCop { get; set; }

		/// <summary>
		/// Campo para informar el Valor total a pagar
		/// </summary>
		public decimal TotalNetoFacturaCop { get; set; }

		/// <summary>
		/// Campo para informar el Descuento en pesos
		/// </summary>
		public decimal MntDctoCop { get; set; }

		/// <summary>
		/// Campo para informar el Recargo en pesos
		/// </summary>
		public decimal MntRcgoCop { get; set; }

		/// <summary>
		/// Campo para informar el Valor a pagar en pesos
		/// </summary>
		public decimal VlrPagarCop { get; set; }

		/// <summary>
		/// Campo para informar el Valor ReteFuente en pesos
		/// </summary>
		public decimal ReteFueCop { get; set; }

		/// <summary>
		/// Campo para informar el Valor ReteIva en pesos
		/// </summary>
		public decimal ReteIvaCop { get; set; }

		/// <summary>
		/// Campo para informar el Valor ReteIca en pesos
		/// </summary>
		public decimal ReteIcaCop { get; set; }

		/// <summary>
		/// Campo para informar el Valor Total anticipos en pesos
		/// </summary>
		public decimal TotAnticiposCop { get; set; }
	}
}
