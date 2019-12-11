using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Objetos;

namespace HGInetUBLv2_1
{
	public class TotalesXML
	{

		/// <summary>
		/// Obtiene los valores del encabezado del documento
		/// </summary>
		/// <param name="documento">datos del documento</param>
		/// <returns>MonetaryTotalType1</returns>
		public static MonetaryTotalType ObtenerTotales(object documento_obj, decimal subtotal, decimal impuesto, decimal base_impuestos)
		{
			try
			{
				/*
					http://www.sfti.se/download/18.1498118f15dce5e8b1e9bb1e/1502968653471/20170315-PEPPOL_BIS_4A-401.pdf
					
					https://peppol.eu/downloads/post-award/
					https://github.com/OpenPEPPOL/documentation/blob/master/PostAward/InvoiceOnly4A/20170315-PEPPOL_BIS_4A-401.pdf
					
				 
					cbc:LineExtensionAmount [0..1]    The total of Line Extension Amounts net of tax and settlement discounts, but inclusive of any applicable rounding amount.
					Sum of line amounts
					∑ LineExtensionAmount (at line level)

				 
					cbc:TaxExclusiveAmount [0..1]    The total amount exclusive of taxes.
					Invoice total amount without VAT
					LineExtensionAmount – AllowanceTotalAmount + ChargeTotalAmount
				 
					cbc:TaxInclusiveAmount [0..1]    The total amount inclusive of taxes.
					Invoice total amount with VAT
					TaxExclusiveAmount + TaxTotal TaxAmount (where tax scheme = VAT) + PayableRoundingAmount

					cbc:AllowanceTotalAmount [0..1]    The total amount of all allowances.
					Allowance/discounts on document level
					∑ Allowance Amount at document level (where ChargeIndicator = ”false”)
									 
					cbc:ChargeTotalAmount [0..1]    The total amount of all charges.
					Charges on document level
					∑ ChargeAmount(whereChargeIndicator=”true”)
				 
					cbc:PrepaidAmount [0..1]    The total prepaid amount.
					The amount prepaid
					Sum of amount previously paid

					cbc:PayableRoundingAmount [0..1]    The rounding amount (positive or negative) added to the calculated Line Extension Total Amount to produce the rounded Line Extension Total Amount.
					The amount used to round 
				 
					cbc:PayableAmount [1..1]    The total amount to be paid.
					PayableAmount to an in
					TaxInclusiveAmount (from the LegalMonetaryTotal class on document level) – PrepaidAmount (from the LegalMonetaryTotal class on document level)

				 
					Amounts MUST be given to a precision of two decimals.
					Amounts at document level MUST apply to all invoices lines.
					Total payable amount in an invoice MUST NOT be negative.
					Tax inclusive amount in an invoice MUST NOT be negative.
				 
				 */


				var documento = (dynamic)null;
				documento = documento_obj;

				// moneda del documento
				CurrencyCodeContentType moneda_documento = Ctl_Enumeracion.ObtenerMoneda(documento.Moneda);

				MonetaryTotalType LegalMonetaryTotal = new MonetaryTotalType();

				#region Total Importe bruto antes de impuestos

				// cbc:LineExtensionAmount [0..1]    The total of Line Extension Amounts net of tax and settlement discounts, but inclusive of any applicable rounding amount.
				//	Total importe bruto, suma de los importes brutos de las líneas de la factura - los productos que son regalos.
				LineExtensionAmountType LineExtensionAmount = new LineExtensionAmountType();
				LineExtensionAmount.currencyID = moneda_documento.ToString();
				LineExtensionAmount.Value = decimal.Round(documento.ValorSubtotal, 2);//decimal.Round(subtotal, 2);//
				LegalMonetaryTotal.LineExtensionAmount = LineExtensionAmount;

				#endregion

				#region Valor total base imponible (generó impuestos)

				//Total Base Imponible (Importe Bruto+Cargos-Descuentos): Base imponible para el cálculo de los impuestos.
				//Subtotal de la factura incluyendo los productos que son regalo.
				TaxExclusiveAmountType TaxExclusiveAmount = new TaxExclusiveAmountType();
				TaxExclusiveAmount.currencyID = moneda_documento.ToString();
				TaxExclusiveAmount.Value = decimal.Round(base_impuestos, 2);//decimal.Round(subtotal, 2);//
				LegalMonetaryTotal.TaxExclusiveAmount = TaxExclusiveAmount;

				#endregion

				//Total de Valor bruto con tributos Los tributos retenidos son retirados en el cálculo de PayableAmount 
				//Subtotal de la factura + Iva
				#region Valor total base no imponible (no generó impuestos)
				TaxInclusiveAmountType TaxInclusiveAmount = new TaxInclusiveAmountType();
				TaxInclusiveAmount.currencyID = moneda_documento.ToString();
				TaxInclusiveAmount.Value = decimal.Round(documento.ValorSubtotal, 2) + decimal.Round(documento.ValorIva, 2) + decimal.Round(documento.ValorImpuestoConsumo, 2);//decimal.Round(subtotal, 2) + decimal.Round(impuesto, 2);//
				LegalMonetaryTotal.TaxInclusiveAmount = TaxInclusiveAmount;
				#endregion

				#region Descuentos

				// Descuentos: Suma de todos los descuentos aplicados al total de la factura
				//-------Los descuentos del Detalle se registran por detalle, este se llena cuando se aplican al total de la Factura
				AllowanceTotalAmountType AllowanceTotalAmount = new AllowanceTotalAmountType();
				AllowanceTotalAmount.currencyID = moneda_documento.ToString();
				//-----Se pone 0 en descuentos para las pruebas
				//documento.ValorDescuento = decimal.Round(0.00M, 2);
				AllowanceTotalAmount.Value = decimal.Round(documento.ValorDescuento, 2);
				LegalMonetaryTotal.AllowanceTotalAmount = AllowanceTotalAmount;

				#endregion

				//Anticipo Total: Suma de todos los pagos anticipados
				PrepaidAmountType PrepaidAmount = new PrepaidAmountType();
				PrepaidAmount.currencyID = moneda_documento.ToString();
				PrepaidAmount.Value = decimal.Round(documento.ValorAnticipo, 2);
				LegalMonetaryTotal.PrepaidAmount = PrepaidAmount;

				//Cargos: Suma de todos los cargos aplicados al total de la factura
				//-----Se utiliza para sumarle al Total, ejemplo costo de financiamiento
				ChargeTotalAmountType ChargeTotalAmount = new ChargeTotalAmountType();
				ChargeTotalAmount.currencyID = moneda_documento.ToString();
				ChargeTotalAmount.Value = decimal.Round(documento.ValorCargo, 2);
				LegalMonetaryTotal.ChargeTotalAmount = ChargeTotalAmount;

				#region Valor total de pago //  Total de Factura =  Valor total bases - Valor descuentos + Valor total Impuestos - Valor total impuestos retenidos

				PayableAmountType PayableAmount = new PayableAmountType();
				PayableAmount.currencyID = moneda_documento.ToString();
				PayableAmount.Value = decimal.Round(documento.Total, 2);
				LegalMonetaryTotal.PayableAmount = PayableAmount;

				#endregion

				
				if (decimal.Round(subtotal + impuesto + documento.ValorCargo - documento.ValorDescuento - documento.ValorAnticipo, 2) != documento.Total)
				{
					decimal total_cal = decimal.Round(subtotal + impuesto + documento.ValorCargo - documento.ValorDescuento - documento.ValorAnticipo, 2);
					PayableRoundingAmountType Rounding = new PayableRoundingAmountType();
					Rounding.currencyID = moneda_documento.ToString();
					Rounding.Value = decimal.Round(documento.Total - total_cal, 2);
					LegalMonetaryTotal.PayableRoundingAmount = Rounding;
				}

				return LegalMonetaryTotal;

			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

	}
}
