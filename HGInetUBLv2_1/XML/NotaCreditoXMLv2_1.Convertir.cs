using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetUBLv2_1.Dian;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.RegistroLog;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HGInetUBLv2_1
{
	public partial class NotaCreditoXMLv2_1
	{
		/// <summary>
		/// Convierte el XML-UBL2.1 en un objeto de Servicio
		/// </summary>
		/// <param name="nota_credito_ubl">Archivo XML-UBL</param>
		/// <param name="interopeabilidad">Indica si la conversión del archivo XML-UBL es sólo de encabezados</param>
		/// <returns>objeto tipo NotaCredito</returns>
		public static NotaCredito Convertir(CreditNoteType nota_credito_ubl, TblDocumentos documento_bd, bool interopeabilidad = false)
		{

			try
			{
				NotaCredito nota_credito_obj = new NotaCredito();

				nota_credito_obj.Prefijo = documento_bd.StrPrefijo;

				//captura el numero del documento y valida proceso de interoperabilidad
				if (interopeabilidad)
				{
					Match numero_doc = Regex.Match(nota_credito_ubl.ID.Value, "\\d+");

					nota_credito_obj.Documento = Convert.ToInt64(numero_doc.Value);
				}
				else
				{
					if (string.IsNullOrEmpty(nota_credito_obj.Prefijo))
					{
						nota_credito_obj.Documento = Convert.ToInt64(nota_credito_ubl.ID.Value);
					}
					else
					{
						string documento = nota_credito_ubl.ID.Value;
						if (documento.Substring(0, nota_credito_obj.Prefijo.Length).Equals(nota_credito_obj.Prefijo))
						{
							nota_credito_obj.Documento = Convert.ToInt64(documento.Substring(nota_credito_obj.Prefijo.Length));
						}
					}
				}
				//Capturo la informacion del encabezado del documento
				if (nota_credito_ubl.UUID != null)
					nota_credito_obj.Cufe = nota_credito_ubl.UUID.Value;
				DateTime hora = Convert.ToDateTime(nota_credito_ubl.IssueTime.Value).AddHours(-5);
				DateTime fecha = nota_credito_ubl.IssueDate.Value;
				DateTime fecha_hora = new DateTime(fecha.Year, fecha.Month, fecha.Day, hora.Hour, hora.Minute, hora.Second);
				nota_credito_obj.Fecha = fecha_hora;
				nota_credito_obj.Moneda = nota_credito_ubl.DocumentCurrencyCode.Value;

				nota_credito_obj.Concepto = string.Empty;
				// valida el concepto
				if (nota_credito_ubl.DiscrepancyResponse != null)
				{
					if (nota_credito_ubl.DiscrepancyResponse.FirstOrDefault().ResponseCode != null && nota_credito_ubl.DiscrepancyResponse.FirstOrDefault().ResponseCode.Value != null)
					{
						nota_credito_obj.Concepto = nota_credito_ubl.DiscrepancyResponse.FirstOrDefault().ResponseCode.Value;
					}
				}

				nota_credito_obj.DocumentoRef = string.Empty;
				// valida el documento de referencia factura
				if (nota_credito_ubl.BillingReference != null)
				{
					if (nota_credito_ubl.BillingReference.FirstOrDefault().InvoiceDocumentReference != null && nota_credito_ubl.BillingReference.FirstOrDefault().InvoiceDocumentReference.ID != null && nota_credito_ubl.BillingReference.FirstOrDefault().InvoiceDocumentReference.ID.Value != null)
					{
						nota_credito_obj.DocumentoRef = nota_credito_ubl.BillingReference.FirstOrDefault().InvoiceDocumentReference.ID.Value;
						nota_credito_obj.CufeFactura = nota_credito_ubl.BillingReference.FirstOrDefault().InvoiceDocumentReference.UUID.Value;
						nota_credito_obj.FechaFactura = nota_credito_ubl.BillingReference.FirstOrDefault().InvoiceDocumentReference.IssueDate.Value;
					}
				}

				nota_credito_obj.PedidoRef = string.Empty;
				nota_credito_obj.OrderReference = new ReferenciaAdicional();
				// valida el documento de referencia pedido
				if (nota_credito_ubl.OrderReference != null)
				{
					if (nota_credito_ubl.OrderReference.ID != null && nota_credito_ubl.OrderReference.ID.Value != null)
					{
						nota_credito_obj.PedidoRef = nota_credito_ubl.OrderReference.ID.Value;
						nota_credito_obj.OrderReference.Documento = nota_credito_ubl.OrderReference.ID.Value;
					}
				}

				//Valida si tiene documento referencia de despacho
				if (nota_credito_ubl.DespatchDocumentReference != null)
				{
					nota_credito_obj.DespatchDocument = new List<ReferenciaAdicional>();
					foreach (var item in nota_credito_ubl.DespatchDocumentReference)
					{
						ReferenciaAdicional despacho = new ReferenciaAdicional();
						if (item.ID.Value != null)
						{
							despacho.Documento = item.ID.Value;
						}
						nota_credito_obj.DespatchDocument.Add(despacho);
					}
				}

				if (nota_credito_ubl.AdditionalDocumentReference != null)
				{
					nota_credito_obj.DocumentosReferencia = new List<ReferenciaAdicional>();
					foreach (var item in nota_credito_ubl.AdditionalDocumentReference)
					{
						ReferenciaAdicional adicional = new ReferenciaAdicional();
						if (item.ID.Value != null)
						{
							adicional.Documento = item.ID.Value;
							adicional.CodigoReferencia = item.DocumentTypeCode.Value;
						}
						nota_credito_obj.DocumentosReferencia.Add(adicional);
					}
				}

				if (!interopeabilidad)
				{
					//valida las notas para llenar los campos del pdf
					Formato documento_formato = new Formato();
					List<FormatoCampo> lista_campos = new List<FormatoCampo>();

					try
					{

						if (nota_credito_ubl.Note[0].Value != null)
						{
							//Deserializa la posición 1 y las convierte en FormatoCampo
							dynamic jsonObj = JsonConvert.DeserializeObject(nota_credito_ubl.Note[0].Value);

							if (jsonObj != null)
							{
								documento_formato.Codigo = jsonObj.Codigo;

								if (jsonObj.CamposPredeterminados != null)
								{
									foreach (var obj in jsonObj.CamposPredeterminados)
									{
										FormatoCampo campo = new FormatoCampo();
										campo.Descripcion = obj.Descripcion;
										campo.Ubicacion = obj.Ubicacion;
										campo.Valor = obj.Valor;

										lista_campos.Add(campo);
									}
								}
							}
						}
					}
					catch (Exception)
					{
						nota_credito_obj.Notas.Add(nota_credito_ubl.Note[0].Value);
					}

					documento_formato.CamposPredeterminados = lista_campos;

					nota_credito_obj.DocumentoFormato = documento_formato;
					if (nota_credito_ubl.Note.Count() > 1 && (nota_credito_ubl.Note[1].Value != null))
						nota_credito_obj.Nota = nota_credito_ubl.Note[1].Value;
				}

				#region Datos del Adquiriente

				nota_credito_obj.DatosAdquiriente = TerceroXMLv2_1.Obtener_adquiriente(nota_credito_ubl.AccountingCustomerParty);

				#endregion

				#region Datos del Obligado

				nota_credito_obj.DatosObligado = TerceroXMLv2_1.Obtener_obligado(nota_credito_ubl.AccountingSupplierParty);

				#endregion

				#region Detalle de Documento

				List<DocumentoDetalle> list_detalle = new List<DocumentoDetalle>();

				if (!interopeabilidad)
				{
					//Recorre todo el detalle del documento
					for (int i = 0; i < nota_credito_ubl.CreditNoteLine.Length; i++)
					{

						DocumentoDetalle detalle = new DocumentoDetalle();
						detalle.Codigo = Convert.ToInt16(nota_credito_ubl.CreditNoteLine[i].ID.Value);
						if (nota_credito_ubl.CreditNoteLine[i].Item.SellersItemIdentification != null)
							detalle.ProductoCodigo = nota_credito_ubl.CreditNoteLine[i].Item.SellersItemIdentification.ID.Value;
						if (nota_credito_ubl.CreditNoteLine[i].Item.StandardItemIdentification != null)
							detalle.ProductoCodigoEAN = nota_credito_ubl.CreditNoteLine[i].Item.StandardItemIdentification.ID.Value;
						detalle.ProductoNombre = nota_credito_ubl.CreditNoteLine[i].Item.Description[0].Value;
						if (nota_credito_ubl.CreditNoteLine[i].Item.AdditionalInformation != null)
						{
							detalle.ProductoDescripcion = nota_credito_ubl.CreditNoteLine[i].Item.AdditionalInformation.FirstOrDefault().Value;
						}
						else
						{
							detalle.ProductoDescripcion = string.Empty;
						}
						detalle.Cantidad = nota_credito_ubl.CreditNoteLine[i].CreditedQuantity.Value;
						if (nota_credito_ubl.CreditNoteLine[i].CreditedQuantity.unitCode != null && !string.IsNullOrEmpty(nota_credito_ubl.CreditNoteLine[i].CreditedQuantity.unitCode.ToString()))
						{
							detalle.UnidadCodigo = nota_credito_ubl.CreditNoteLine[i].CreditedQuantity.unitCode.ToString();
						}
						else
						{
							detalle.UnidadCodigo = "94";
						}
						if (nota_credito_ubl.CreditNoteLine[i].Item.OriginAddress != null)
						{
							detalle.Bodega = nota_credito_ubl.CreditNoteLine[i].Item.OriginAddress[0].ID.Value;
						}
						else
						{
							detalle.Bodega = string.Empty;
						}

						detalle.ValorUnitario = nota_credito_ubl.CreditNoteLine[i].Price.PriceAmount.Value;
						detalle.ValorSubtotal = nota_credito_ubl.CreditNoteLine[i].LineExtensionAmount.Value;

						if (nota_credito_ubl.CreditNoteLine[i].Item.AdditionalItemProperty != null)
						{
							detalle.CamposAdicionales = new List<CampoValor>();
							foreach (ItemPropertyType item in nota_credito_ubl.CreditNoteLine[i].Item.AdditionalItemProperty)
							{
								if (item.Name.Value.Equals("Item Oculto para Impresion"))
								{
									detalle.OcultarItem = Convert.ToInt16(item.Value.Value);
								}
								else
								{
									CampoValor campo = new CampoValor();
									campo.Descripcion = item.Name.Value;
									campo.Valor = item.Value.Value;
									detalle.CamposAdicionales.Add(campo);
								}

							}
						}

						if (nota_credito_ubl.CreditNoteLine[i].AllowanceCharge != null)
						{

							detalle.DescuentoValor = nota_credito_ubl.CreditNoteLine[i].AllowanceCharge.FirstOrDefault().Amount.Value;
							if (nota_credito_ubl.CreditNoteLine[i].AllowanceCharge.FirstOrDefault().MultiplierFactorNumeric != null)
							{
								detalle.DescuentoPorcentaje = nota_credito_ubl.CreditNoteLine[i].AllowanceCharge.FirstOrDefault().MultiplierFactorNumeric.Value;
							}
							else
							{
								detalle.DescuentoPorcentaje = decimal.Round((detalle.DescuentoValor / (detalle.ValorUnitario * detalle.Cantidad)), 6, MidpointRounding.AwayFromZero);
							}

						}
						else
						{
							detalle.DescuentoPorcentaje = 0.00M;
							detalle.DescuentoValor = 0.00M;
						}

						if (nota_credito_ubl.CreditNoteLine[i].FreeOfChargeIndicator != null)
						{
							detalle.ProductoGratis = nota_credito_ubl.CreditNoteLine[i].FreeOfChargeIndicator.Value;
							if (detalle.ProductoGratis == true)
								detalle.ProductoGratisPrecioRef = nota_credito_ubl.CreditNoteLine[i].PricingReference.AlternativeConditionPrice.FirstOrDefault().PriceTypeCode.Value;
						}

						// valida que el detalle contenga el tag TaxTotal
						if (nota_credito_ubl.CreditNoteLine[i].TaxTotal != null)
						{
							for (int j = 0; j < nota_credito_ubl.CreditNoteLine[i].TaxTotal[0].TaxSubtotal.Count(); j++)
							{
								string tipo_impto = nota_credito_ubl.CreditNoteLine[i].TaxTotal[0].TaxSubtotal[j].TaxCategory.TaxScheme.ID.Value;
								decimal porcentaje_impto = 0;
								if (nota_credito_ubl.CreditNoteLine[i].TaxTotal[0].TaxSubtotal[j].TaxCategory.Percent != null)
								{
									porcentaje_impto = nota_credito_ubl.CreditNoteLine[i].TaxTotal[0].TaxSubtotal[j].TaxCategory.Percent.Value;
								}
								decimal valor_impto = nota_credito_ubl.CreditNoteLine[i].TaxTotal[0].TaxSubtotal[j].TaxAmount.Value;

								if (TipoImpuestos.Iva.Equals(tipo_impto))
								{
									detalle.IvaPorcentaje = porcentaje_impto;
									detalle.IvaValor = valor_impto;
								}
								else if (TipoImpuestos.Consumo.Equals(tipo_impto))
								{
									detalle.ImpoConsumoPorcentaje = porcentaje_impto;
									detalle.ValorImpuestoConsumo = valor_impto;
									nota_credito_obj.ValorImpuestoConsumo += detalle.ValorImpuestoConsumo;
								}
								else if (TipoImpuestos.Ica.Equals(tipo_impto))
								{
									detalle.ReteIcaPorcentaje = porcentaje_impto;
									detalle.ReteIcaValor = valor_impto;
									nota_credito_obj.ValorReteIca += detalle.ReteIcaValor;
								}
								else if (TipoImpuestos.ReteFte.Equals(tipo_impto))
								{
									detalle.ReteFuentePorcentaje = porcentaje_impto;
									detalle.ReteFuenteValor = valor_impto;
									nota_credito_obj.ValorReteFuente += detalle.ReteFuenteValor;
								}
								else if (tipo_impto.Equals("22"))//Impuesto a la bolsa
								{
									detalle.ValorImpuestoConsumo = valor_impto;
									nota_credito_obj.ValorImpuestoConsumo += detalle.ValorImpuestoConsumo;
								}

							}
						}

						list_detalle.Add(detalle);

					}

				
					nota_credito_obj.DocumentoDetalles = list_detalle;
					#endregion

					//Tasa de Cambio
					if (nota_credito_ubl.PaymentExchangeRate != null)
					{
						if (!nota_credito_ubl.PaymentExchangeRate.SourceCurrencyCode.Value.Equals(nota_credito_ubl.PaymentExchangeRate.TargetCurrencyCode.Value))
						{
							nota_credito_obj.Trm = new TasaCambio();
							nota_credito_obj.Trm.Moneda = nota_credito_ubl.PaymentExchangeRate.TargetCurrencyCode.Value;
							nota_credito_obj.Trm.FechaTrm = nota_credito_ubl.PaymentExchangeRate.Date.Value;
							nota_credito_obj.Trm.Valor = nota_credito_ubl.PaymentExchangeRate.CalculationRate.Value;

						}

					}

					#region Totales
					nota_credito_obj.ValorSubtotal = nota_credito_ubl.LegalMonetaryTotal.LineExtensionAmount.Value;
					nota_credito_obj.Valor = nota_credito_obj.ValorSubtotal + nota_credito_obj.DocumentoDetalles.Sum(b => b.DescuentoValor);
					if (nota_credito_ubl.LegalMonetaryTotal.AllowanceTotalAmount != null)
						nota_credito_obj.ValorDescuento = nota_credito_ubl.LegalMonetaryTotal.AllowanceTotalAmount.Value;
					if (nota_credito_ubl.LegalMonetaryTotal.ChargeTotalAmount != null)
						nota_credito_obj.ValorCargo = nota_credito_ubl.LegalMonetaryTotal.ChargeTotalAmount.Value;
					if (nota_credito_ubl.LegalMonetaryTotal.PrepaidAmount != null)
						nota_credito_obj.ValorAnticipo = nota_credito_ubl.LegalMonetaryTotal.PrepaidAmount.Value;
					nota_credito_obj.ValorIva = nota_credito_ubl.LegalMonetaryTotal.TaxInclusiveAmount.Value - nota_credito_ubl.LegalMonetaryTotal.LineExtensionAmount.Value;
					nota_credito_obj.Total = nota_credito_ubl.LegalMonetaryTotal.PayableAmount.Value;

					//Se agrega validacion si ya se habia guardado el neto en Bd para utilizarlo o si no que sea el calculado
					if (documento_bd != null)
					{
						if (documento_bd.IntValorNeto == 0)
						{
							nota_credito_obj.Neto = (nota_credito_obj.Total - (nota_credito_obj.ValorReteFuente + nota_credito_obj.ValorReteIca + nota_credito_obj.ValorReteIva));
						}
						else
						{
							nota_credito_obj.Neto = documento_bd.IntValorNeto;
						}
					}
					else
					{
						nota_credito_obj.Neto = (nota_credito_obj.Total - (nota_credito_obj.ValorReteFuente + nota_credito_obj.ValorReteIca + nota_credito_obj.ValorReteIva));
					}
					#endregion 
				}


				return nota_credito_obj;
			}
			catch (Exception ex)
			{
				string mensaje = string.Format("Se presento inconsistencia convirtiendo el xml de Nota Credito a objeto. Detalle: {0}", ex.Message);
				RegistroLog.EscribirLog(ex, MensajeCategoria.Convertir, MensajeTipo.Error, MensajeAccion.ninguna, mensaje);
				throw new ApplicationException(mensaje, ex.InnerException);
			}
		}
	}
}
