using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetUBLv2_1.Dian;
using HGInetUBLv2_1.DianListas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HGInetMiFacturaElectonicaData.ControllerSql;

namespace HGInetUBLv2_1
{
	public class ImpuestosXML
	{

		/// <summary>
		/// Llena el objeto de los impuestos de la factura con los datos documento detalle
		/// </summary>
		/// <param name="documentoDetalle">Datos del detalle del documento</param>
		/// <returns>Objeto de tipo TaxTotalType1</returns>
		public static TaxTotalType[] ObtenerImpuestos(List<DocumentoDetalle> documentoDetalle, string moneda, string version_erp, int hgi)
		{
			try
			{
				if (documentoDetalle == null || documentoDetalle.Count == 0)
					throw new Exception("El detalle del documento es inválido.");

				string version_validar = "Ver. 2020.4";
				string version_validar_Rev = "Ver. 2020.4 Rev";

				if (string.IsNullOrEmpty(version_erp))
					version_erp = version_validar;

				//Toma los impuestos de IVA que tiene el producto en el detalle del documento
				var impuestos_iva = documentoDetalle.Where(d => d.CalculaIVA == 0).ToList().Select(_impuesto => new { _impuesto.IvaPorcentaje, TipoImpuestos.Iva, _impuesto.IvaValor }).GroupBy(_impuesto => new { _impuesto.IvaPorcentaje }).Select(_impuesto => _impuesto.First()).ToList();

				List<DocumentoImpuestos> doc_impuestos = new List<DocumentoImpuestos>();

				decimal BaseImponibleImpuesto = 0;

				// moneda del primer detalle
				CurrencyCodeContentType moneda_detalle = Ctl_Enumeracion.ObtenerMoneda(moneda);

				foreach (var item in impuestos_iva)
				{
					DocumentoImpuestos imp_doc = new DocumentoImpuestos();
					decimal porcentaje = item.IvaPorcentaje;

					List<DocumentoDetalle> doc_ = documentoDetalle.Where(docDet => docDet.IvaPorcentaje == item.IvaPorcentaje).ToList();
					BaseImponibleImpuesto = decimal.Round(documentoDetalle.Where(docDet => docDet.IvaPorcentaje == item.IvaPorcentaje).Sum(docDet => docDet.BaseImpuestoIva), 2, MidpointRounding.AwayFromZero);

					//imp_doc.Codigo = item.IntIva;
					//-------Hay que hacer Enumerable
					if (item.IvaPorcentaje == 0)
						porcentaje = Convert.ToDecimal(0.00M);
					ListaTarifaImpuestoIVA lista_iva = new ListaTarifaImpuestoIVA();
					ListaItem iva = lista_iva.Items.Where(d => d.Codigo.Equals(porcentaje.ToString().Replace(",", "."))).FirstOrDefault();

					imp_doc.Nombre = iva.Nombre;
					imp_doc.Porcentaje = decimal.Round(item.IvaPorcentaje, 2);
					imp_doc.TipoImpuesto = item.Iva;
					imp_doc.BaseImponible = BaseImponibleImpuesto;

					foreach (var docDet in doc_)
					{
						imp_doc.ValorImpuesto = decimal.Round(imp_doc.ValorImpuesto + docDet.IvaValor, 2, MidpointRounding.AwayFromZero);
					}

					//Validacion de muestras
					List<DocumentoDetalle> muestra = documentoDetalle.Where(docDet => docDet.IvaPorcentaje == item.IvaPorcentaje && docDet.ProductoGratis.Equals(true) && docDet.ValorImpuestoConsumo.Equals(0)).ToList();
					decimal BaseimponibleMuestra = 0;
					foreach (var DocMues in muestra)
					{
						if (DocMues.IvaPorcentaje > 0)
							BaseimponibleMuestra = decimal.Round(BaseimponibleMuestra + ((DocMues.Cantidad * DocMues.ValorUnitario) - DocMues.DescuentoValor), 2, MidpointRounding.AwayFromZero);
					}

					if (BaseimponibleMuestra > 0)
					{
						BaseImponibleImpuesto += BaseimponibleMuestra;
						imp_doc.BaseImponible += BaseimponibleMuestra;
					}

					if ((!version_erp.Equals(version_validar) && !version_erp.Contains(version_validar_Rev)) && hgi == -1)
					{
						decimal imp_cal = decimal.Round(BaseImponibleImpuesto * (imp_doc.Porcentaje / 100), 2, MidpointRounding.AwayFromZero);

						if (imp_cal != imp_doc.ValorImpuesto)
							imp_doc.ValorImpuesto = imp_cal;

					}

					doc_impuestos.Add(imp_doc);

				}

				//Toma el impuesto al consumo de los productos que esten el detalle
				var impuesto_consumo = documentoDetalle.Where(d => d.ValorImpuestoConsumo > 0 || d.Aiu == 4).ToList().Select(_consumo => new
					{ _consumo.ImpoConsumoPorcentaje, _consumo.ValorImpuestoConsumo, _consumo.Aiu }).GroupBy(_consumo => new { _consumo.ImpoConsumoPorcentaje, _consumo.Aiu }).Select(_consumo => _consumo.First()).ToList();
				decimal BaseImponibleImpConsumo = 0;
				decimal BaseImponibleBolsa = 0;



				if (impuesto_consumo.Count() > 0)
				{
					foreach (var item in impuesto_consumo)
					{
						//Valida si hay algun producto con impuesto al consumo
						if (item.ValorImpuestoConsumo != 0)
						{
							DocumentoImpuestos imp_doc = new DocumentoImpuestos();
							List<DocumentoDetalle> doc_ = documentoDetalle.Where(docDet => docDet.ValorImpuestoConsumo != 0 && item.ImpoConsumoPorcentaje == docDet.ImpoConsumoPorcentaje && item.Aiu == docDet.Aiu).ToList();
							DocumentoDetalle bolsa = doc_.Where(det => item.Aiu == 4 && det.ValorImpuestoConsumo > 0 && det.ImpoConsumoPorcentaje == item.ImpoConsumoPorcentaje && det.Aiu == 4).FirstOrDefault();

							if (bolsa == null)
							{

								BaseImponibleImpConsumo = decimal.Round(doc_.Where(docDet => docDet.ValorImpuestoConsumo != 0).Sum(docDet => docDet.ValorSubtotal), 2, MidpointRounding.AwayFromZero);

								//imp_doc.Codigo = item.IntImpConsumo.ToString();
								//imp_doc.Nombre = item.StrDescripcion;
								if (item.ImpoConsumoPorcentaje == 0 && item.ValorImpuestoConsumo > 0)
								{
									imp_doc.Nombre = "IC";
									imp_doc.TipoImpuesto = "02";
									imp_doc.Codigo = "94";
								}
								else
								{
									ListaTarifaImpuestoINC lista_inc = new ListaTarifaImpuestoINC();
									ListaItem inc = lista_inc.Items.Where(d => d.Codigo.Equals(item.ImpoConsumoPorcentaje.ToString().Replace(",", "."))).FirstOrDefault();
									imp_doc.Nombre = inc.Nombre;
									imp_doc.TipoImpuesto = "04";//item.Consumo;
								}
								
								imp_doc.Porcentaje = decimal.Round(item.ImpoConsumoPorcentaje, 2, MidpointRounding.AwayFromZero);
								
								imp_doc.BaseImponible = BaseImponibleImpConsumo;
								foreach (var docDet in doc_)
								{
									imp_doc.ValorImpuesto = decimal.Round(imp_doc.ValorImpuesto + docDet.ValorImpuestoConsumo, 2, MidpointRounding.AwayFromZero);
								}

								if (imp_doc.Porcentaje >= 0 && imp_doc.ValorImpuesto > 0)
								{
									//decimal imp_cal = decimal.Round(BaseImponibleImpConsumo * (imp_doc.Porcentaje / 100), 2, MidpointRounding.AwayFromZero);

									//if (imp_cal != imp_doc.ValorImpuesto)
									//	imp_doc.ValorImpuesto = imp_cal;

									doc_impuestos.Add(imp_doc);
								}

							}
							else
							{
								BaseImponibleBolsa = decimal.Round(item.ValorImpuestoConsumo, 2);
								imp_doc.Porcentaje = decimal.Round(0.00M);
								ListaTipoImpuesto list_impBolsa = new ListaTipoImpuesto();
								ListaItem impBolsa = list_impBolsa.Items.Where(d => d.Codigo.Equals("22")).FirstOrDefault();

								imp_doc.TipoImpuesto = impBolsa.Codigo;
								imp_doc.Nombre = impBolsa.Nombre;//bolsa.UnidadCodigo;
								imp_doc.Codigo = bolsa.UnidadCodigo;
								imp_doc.BaseImponible = decimal.Round(bolsa.Cantidad, 2);
								imp_doc.ValorImpuesto = BaseImponibleBolsa;

								if (imp_doc.ValorImpuesto > 0)
									doc_impuestos.Add(imp_doc);

							}
						}

					}
				}

				//Se debe hacer validacion es sobre el ICA y no sobre el RETEICA
				#region ICA
				//Toma el ReteICA de los productos que esten el detalle
				/*
				var impuesto_ica = documentoDetalle.Select(_ica => new { _ica.ReteIcaPorcentaje, TipoImpuestos.Ica, _ica.ReteIcaValor })
					.GroupBy(_ica => new { _ica.ReteIcaPorcentaje }).Select(_ica => _ica.First());
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

				}*/ 
				#endregion

				List<TaxTotalType> List_Taxtotal = new List<TaxTotalType>();

				List<string> list_tiposImp = doc_impuestos.Select(_x => _x.TipoImpuesto).Distinct().ToList();

				TaxTotalType[] TaxTotals = new TaxTotalType[list_tiposImp.Count];

				foreach (string item in list_tiposImp)
				{

					TaxTotalType TaxTotal = new TaxTotalType();

					#region Importe Impuesto: Importe del impuesto retenido

					TaxAmountType TaxAmount = new TaxAmountType();
					TaxAmount.currencyID = moneda_detalle.ToString();
					TaxAmount.Value = decimal.Round(doc_impuestos.Where(d => d.TipoImpuesto.Equals(item)).Sum(v => v.ValorImpuesto), 2, MidpointRounding.AwayFromZero);
					TaxTotal.TaxAmount = TaxAmount;

					#endregion

					List<TaxSubtotalType> TaxSubtotals = new List<TaxSubtotalType>();

					foreach (var item_sub in doc_impuestos.Where(d=> d.TipoImpuesto.Equals(item)))
					{
						#region Impuesto Legal ***PENDIENTE

						if (!item_sub.TipoImpuesto.Equals("22"))
						{

							// Indicador de si estos totales se reconocen como evidencia legal a efectos impositivos.
							TaxEvidenceIndicatorType TaxEvidenceIndicator = new TaxEvidenceIndicatorType();
							TaxEvidenceIndicator.Value = false;
							TaxTotal.TaxEvidenceIndicator = TaxEvidenceIndicator;

							#endregion

							TaxSubtotalType TaxSubtotal = new TaxSubtotalType();

							#region Importe Impuesto (detalle): Importe del impuesto retenido

							//Valor total del impuesto retenido
							TaxAmountType TaxAmountSubtotal = new TaxAmountType();
							TaxAmountSubtotal.currencyID = moneda_detalle.ToString();
							TaxAmountSubtotal.Value = decimal.Round(item_sub.ValorImpuesto, 2);
							TaxSubtotal.TaxAmount = TaxAmountSubtotal;

							#endregion

							#region Tipo o clase impuesto

							/* Tipo o clase impuesto. Concepto fiscal por el que se tributa. Debería si un	campo que referencia a una lista de códigos. En
							   la lista deberían aparecer los impuestos	estatales o nacionales*/
							TaxCategoryType TaxCategory = new TaxCategoryType();



							if (item_sub.Porcentaje == 0 && item_sub.ValorImpuesto > 0)
							{

								//Base Imponible = Importe bruto + cargos - descuentos
								BaseUnitMeasureType BaseUnitMeasure = new BaseUnitMeasureType();
								BaseUnitMeasure.unitCode = item_sub.Codigo; //moneda_detalle.ToString();
								BaseUnitMeasure.Value = documentoDetalle.Where(d => d.ImpoConsumoPorcentaje == 0 && d.ValorImpuestoConsumo > 0 && d.Aiu != 4).Sum(c => c.Cantidad);//1;//decimal.Round(item_sub.BaseImponible, 2);//
								TaxSubtotal.BaseUnitMeasure = BaseUnitMeasure;

								////Base Imponible = Importe bruto + cargos - descuentos
								//TaxableAmountType TaxableAmount = new TaxableAmountType();
								//TaxableAmount.currencyID = moneda_detalle.ToString();
								//TaxableAmount.Value = BaseUnitMeasure.Value;
								//TaxSubtotal.TaxableAmount = TaxableAmount;

								PerUnitAmountType Percent_unit = new PerUnitAmountType();
								Percent_unit.currencyID = moneda_detalle.ToString();
								//decimal cantidad = documentoDetalle.Where(d => d.ImpoConsumoPorcentaje == 0 && d.ValorImpuestoConsumo > 0 && d.ProductoGratis == false && d.Aiu != 4).Sum(c => c.Cantidad);
								Percent_unit.Value = decimal.Round(item_sub.ValorImpuesto / BaseUnitMeasure.Value, 2);//decimal.Round(item_sub.ValorImpuesto / cantidad, 2);// 
								Percent_unit.Value = Percent_unit.Value + 0.00M;
								TaxSubtotal.PerUnitAmount = Percent_unit;
							}
							else
							{

								#region Base Imponible: Base	Imponible sobre la que se calcula la retención de impuesto

								//Base Imponible = Importe bruto + cargos - descuentos
								TaxableAmountType TaxableAmount = new TaxableAmountType();
								TaxableAmount.currencyID = moneda_detalle.ToString();
								TaxableAmount.Value = decimal.Round(item_sub.BaseImponible, 2);
								TaxSubtotal.TaxableAmount = TaxableAmount;

								#endregion

								#region Porcentaje: Porcentaje a aplicar

								PercentType1 Percent = new PercentType1();
								Percent = new PercentType1();
								Percent.Value = item_sub.Porcentaje;
								TaxCategory.Percent = Percent;

								#endregion
							}



							TaxSchemeType TaxScheme = new TaxSchemeType();
							IDType IDTaxScheme = new IDType();
							IDTaxScheme.Value = item_sub.TipoImpuesto; //(LISTADO DE VALORES DEFINIDO POR LA DIAN)
							TaxScheme.ID = IDTaxScheme;

							NameType1 Name = new NameType1();
							Name.Value = item_sub.Nombre.ToString();

							TaxScheme.Name = Name;
							TaxCategory.TaxScheme = TaxScheme;
							TaxSubtotal.TaxCategory = TaxCategory;
						
							#endregion

							TaxSubtotals.Add(TaxSubtotal);
							
						}
						else
						{
							TaxSubtotalType TaxSubtotal = new TaxSubtotalType();

							#region Base Imponible: Base	Imponible sobre la que se calcula la retención de impuesto

							//Base Imponible = Importe bruto + cargos - descuentos
							BaseUnitMeasureType BaseUnitMeasure = new BaseUnitMeasureType();
							BaseUnitMeasure.unitCode = item_sub.Codigo; //moneda_detalle.ToString();
							BaseUnitMeasure.Value = decimal.Round(item_sub.BaseImponible, 2);//1.00M;//
							TaxSubtotal.BaseUnitMeasure = BaseUnitMeasure;

							#endregion

							//Base Imponible = Importe bruto + cargos - descuentos
							TaxableAmountType TaxableAmount = new TaxableAmountType();
							TaxableAmount.currencyID = moneda_detalle.ToString();
							TaxableAmount.Value = 0.00M;
							TaxSubtotal.TaxableAmount = TaxableAmount;

							#region Importe Impuesto (detalle): Importe del impuesto retenido

							//Valor total del impuesto retenido
							TaxAmountType TaxAmountSubtotal = new TaxAmountType();
							TaxAmountSubtotal.currencyID = moneda_detalle.ToString();
							TaxAmountSubtotal.Value = decimal.Round(item_sub.ValorImpuesto, 2);
							TaxSubtotal.TaxAmount = TaxAmountSubtotal;

							#endregion

							#region Porcentaje: Porcentaje a aplicar

							PerUnitAmountType Percent = new PerUnitAmountType();
							Percent.currencyID = moneda_detalle.ToString();
							//decimal cantidad = documentoDetalle.Where(d => d.ImpoConsumoPorcentaje == 0 && d.ValorImpuestoConsumo > 0 && d.ProductoGratis == false && d.Aiu == 4).Sum(c => c.Cantidad);
							Percent.Value = decimal.Round(item_sub.ValorImpuesto / BaseUnitMeasure.Value, 2);//decimal.Round(item_sub.ValorImpuesto / cantidad, 2);// 
							Percent.Value = Percent.Value + 0.00M;
							//Percent.Value = decimal.Round(item_sub.ValorImpuesto / item_sub.BaseImponible, 2);

							#endregion

							#region Tipo o clase impuesto

							/* Tipo o clase impuesto. Concepto fiscal por el que se tributa. Debería si un	campo que referencia a una lista de códigos. En
							   la lista deberían aparecer los impuestos	estatales o nacionales*/
							TaxCategoryType TaxCategory = new TaxCategoryType();

							TaxSchemeType TaxScheme = new TaxSchemeType();
							IDType IDTaxScheme = new IDType();
							IDTaxScheme.Value = item_sub.TipoImpuesto; //(LISTADO DE VALORES DEFINIDO POR LA DIAN)
							TaxScheme.ID = IDTaxScheme;

							NameType1 Name = new NameType1();
							Name.Value = item_sub.Nombre.ToString();

							TaxScheme.Name = Name;
							TaxCategory.TaxScheme = TaxScheme;
							TaxSubtotal.PerUnitAmount = Percent;
							TaxSubtotal.TaxCategory = TaxCategory;

							#endregion

							TaxSubtotals.Add(TaxSubtotal);
						}

						TaxTotal.TaxSubtotal = TaxSubtotals.ToArray();
					}


					List_Taxtotal.Add(TaxTotal);
				}
				TaxTotals = List_Taxtotal.ToArray();

				return TaxTotals;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

	}
}
