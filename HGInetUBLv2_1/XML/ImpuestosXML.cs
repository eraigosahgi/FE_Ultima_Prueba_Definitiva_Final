using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetUBLv2_1.Dian;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1
{
	public class ImpuestosXML
	{

		/// <summary>
		/// Llena el objeto de los impuestos de la factura con los datos documento detalle
		/// </summary>
		/// <param name="documentoDetalle">Datos del detalle del documento</param>
		/// <returns>Objeto de tipo TaxTotalType1</returns>
		public static TaxTotalType[] ObtenerImpuestos(List<DocumentoDetalle> documentoDetalle, string moneda)
		{
			try
			{
				if (documentoDetalle == null || documentoDetalle.Count == 0)
					throw new Exception("El detalle del documento es inválido.");

				//Toma los impuestos de IVA que tiene el producto en el detalle del documento
				var impuestos_iva = documentoDetalle
					.Select(_impuesto => new {_impuesto.IvaPorcentaje, TipoImpuestos.Iva, _impuesto.IvaValor})
					.GroupBy(_impuesto => new {_impuesto.IvaPorcentaje}).Select(_impuesto => _impuesto.First());

				List<DocumentoImpuestos> doc_impuestos = new List<DocumentoImpuestos>();

				decimal BaseImponibleImpuesto = 0;

				// moneda del primer detalle
				CurrencyCodeContentType moneda_detalle = Ctl_Enumeracion.ObtenerMoneda(moneda);

				foreach (var item in impuestos_iva)
				{
					DocumentoImpuestos imp_doc = new DocumentoImpuestos();
					List<DocumentoDetalle> doc_ = documentoDetalle
						.Where(docDet => docDet.IvaPorcentaje == item.IvaPorcentaje).ToList();
					BaseImponibleImpuesto =
						decimal.Round(
							documentoDetalle.Where(docDet => docDet.IvaPorcentaje == item.IvaPorcentaje)
								.Sum(docDet => docDet.ValorSubtotal), 2);

					//imp_doc.Codigo = item.IntIva;
					//-------Hay que hacer Enumerable
					imp_doc.Nombre = "IVA";
					imp_doc.Porcentaje = decimal.Round(item.IvaPorcentaje, 2);
					imp_doc.TipoImpuesto = item.Iva;
					imp_doc.BaseImponible = BaseImponibleImpuesto;

					foreach (var docDet in doc_)
					{
						imp_doc.ValorImpuesto = decimal.Round(imp_doc.ValorImpuesto + docDet.IvaValor, 2);
					}

					doc_impuestos.Add(imp_doc);

				}

				//Toma el impuesto al consumo de los productos que esten el detalle
				var impuesto_consumo = documentoDetalle
					.Select(_consumo => new
						{_consumo.ImpoConsumoPorcentaje, TipoImpuestos.Consumo, _consumo.ValorImpuestoConsumo})
					.GroupBy(_consumo => new {_consumo.ImpoConsumoPorcentaje}).Select(_consumo => _consumo.First());
				decimal BaseImponibleImpConsumo = 0;



				if (impuesto_consumo.Count() > 0)
				{
					foreach (var item in impuesto_consumo)
					{
						//Valida si hay algun producto con impuesto al consumo
						if (item.ValorImpuestoConsumo != 0)
						{
							DocumentoImpuestos imp_doc = new DocumentoImpuestos();
							List<DocumentoDetalle> doc_ = documentoDetalle
								.Where(docDet => docDet.ValorImpuestoConsumo != 0).ToList();
							BaseImponibleImpConsumo =
								decimal.Round(
									documentoDetalle.Where(docDet => docDet.ValorImpuestoConsumo != 0)
										.Sum(docDet => docDet.ValorSubtotal), 2);

							//imp_doc.Codigo = item.IntImpConsumo.ToString();
							//imp_doc.Nombre = item.StrDescripcion;
							imp_doc.Porcentaje = decimal.Round(item.ValorImpuestoConsumo, 2);
							imp_doc.TipoImpuesto = item.Consumo;
							imp_doc.BaseImponible = BaseImponibleImpConsumo;
							foreach (var docDet in doc_)
							{
								imp_doc.ValorImpuesto =
									decimal.Round(imp_doc.ValorImpuesto + docDet.ValorImpuestoConsumo, 2);
							}

							if (imp_doc.Porcentaje > 0)
								doc_impuestos.Add(imp_doc);
						}

					}
				}

				//Toma el ReteICA de los productos que esten el detalle
				var impuesto_ica = documentoDetalle
					.Select(_ica => new {_ica.ReteIcaPorcentaje, TipoImpuestos.Ica, _ica.ReteIcaValor})
					.GroupBy(_ica => new {_ica.ReteIcaPorcentaje}).Select(_ica => _ica.First());
				decimal BaseImponibleReteIca = 0;


				if (impuesto_ica.Count() > 0)
				{
					foreach (var item in impuesto_ica)
					{
						//Valida si hay algun producto con ReteICA
						if (item.ReteIcaValor != 0)
						{
							DocumentoImpuestos imp_doc = new DocumentoImpuestos();
							List<DocumentoDetalle> doc_ = documentoDetalle.Where(docDet => docDet.ReteIcaValor != 0)
								.ToList();
							BaseImponibleReteIca =
								decimal.Round(
									documentoDetalle.Where(docDet => docDet.ReteIcaValor != 0)
										.Sum(docDet => docDet.ValorSubtotal), 2);

							imp_doc.Porcentaje = decimal.Round(item.ReteIcaPorcentaje, 2);
							imp_doc.TipoImpuesto = item.Ica;
							imp_doc.BaseImponible = BaseImponibleReteIca;
							foreach (var docDet in doc_)
							{
								imp_doc.ValorImpuesto = decimal.Round(imp_doc.ValorImpuesto + docDet.ReteIcaValor, 2);
							}

							if (imp_doc.Porcentaje > 0)
								doc_impuestos.Add(imp_doc);
						}

					}

				}


				TaxTotalType[] TaxTotals = new TaxTotalType[doc_impuestos.Count];

				int contador = 0;
				foreach (var item in doc_impuestos)
				{

					TaxTotalType TaxTotal = new TaxTotalType();

					#region Importe Impuesto: Importe del impuesto retenido

					TaxAmountType TaxAmount = new TaxAmountType();
					TaxAmount.currencyID = moneda_detalle.ToString();
					TaxAmount.Value = decimal.Round(item.ValorImpuesto, 2);
					TaxTotal.TaxAmount = TaxAmount;

					#endregion

					#region Impuesto Legal ***PENDIENTE

					// Indicador de si estos totales se reconocen como evidencia legal a efectos impositivos.
					TaxEvidenceIndicatorType TaxEvidenceIndicator = new TaxEvidenceIndicatorType();
					TaxEvidenceIndicator.Value = false;
					TaxTotal.TaxEvidenceIndicator = TaxEvidenceIndicator;

					#endregion

					TaxSubtotalType[] TaxSubtotals = new TaxSubtotalType[1];
					TaxSubtotalType TaxSubtotal = new TaxSubtotalType();

					#region Base Imponible: Base	Imponible sobre la que se calcula la retención de impuesto

					//Base Imponible = Importe bruto + cargos - descuentos
					TaxableAmountType TaxableAmount = new TaxableAmountType();
					TaxableAmount.currencyID = moneda_detalle.ToString();
					TaxableAmount.Value = decimal.Round(item.BaseImponible, 2);
					TaxSubtotal.TaxableAmount = TaxableAmount;

					#endregion

					#region Importe Impuesto (detalle): Importe del impuesto retenido

					//Valor total del impuesto retenido
					TaxAmountType TaxAmountSubtotal = new TaxAmountType();
					TaxAmountSubtotal.currencyID = moneda_detalle.ToString();
					TaxAmountSubtotal.Value = decimal.Round(item.ValorImpuesto, 2);
					TaxSubtotal.TaxAmount = TaxAmountSubtotal;

					#endregion




					#region Tipo o clase impuesto

					/* Tipo o clase impuesto. Concepto fiscal por el que se tributa. Debería si un	campo que referencia a una lista de códigos. En
					   la lista deberían aparecer los impuestos	estatales o nacionales*/
					TaxCategoryType TaxCategory = new TaxCategoryType();

					#region Porcentaje: Porcentaje a aplicar

					PercentType1 Percent = new PercentType1();
					Percent = new PercentType1();
					Percent.Value = item.Porcentaje;
					TaxCategory.Percent = Percent;

					#endregion

					TaxSchemeType TaxScheme = new TaxSchemeType();
					IDType IDTaxScheme = new IDType();
					IDTaxScheme.Value = TipoImpuestos.Iva; //(LISTADO DE VALORES DEFINIDO POR LA DIAN)
					TaxScheme.ID = IDTaxScheme;

					NameType1 Name = new NameType1();
					Name.Value = item.Nombre.ToString();

					TaxScheme.Name = Name;
					TaxCategory.TaxScheme = TaxScheme;
					TaxSubtotal.TaxCategory = TaxCategory;

					#endregion

					TaxSubtotals[0] = TaxSubtotal;
					TaxTotal.TaxSubtotal = TaxSubtotals;
					TaxTotals[contador] = TaxTotal;
					contador++;
				}

				return TaxTotals;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

	}
}
