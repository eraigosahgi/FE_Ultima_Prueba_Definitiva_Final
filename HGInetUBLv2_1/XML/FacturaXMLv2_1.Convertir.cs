using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetMiFacturaElectonicaData.ModeloServicio.Documentos;
using HGInetUBLv2_1.Dian;
using HGInetUBLv2_1.DianListas;
using LibreriaGlobalHGInet.Enumerables;
using LibreriaGlobalHGInet.Formato;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.RegistroLog;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HGInetUBLv2_1
{
	public partial class FacturaXMLv2_1
	{
		/// <summary>
		/// Convierte el XML-UBL 2.1 en un objeto de Servicio
		/// </summary>
		/// <param name="factura_ubl">Archivo XML-UBL</param>
		/// <param name="interopeabilidad">Indica si la conversión del archivo XML-UBL 2.1 es sólo de encabezados</param>
		/// <returns>objeto tipo Factura</returns>
		public static Factura Convertir(InvoiceType factura_ubl, TblDocumentos documento_bd, bool interopeabilidad = false)
		{
			Factura factura_obj = new Factura();

			try
			{
				
				//Captura la resolucion y el prefijo
				for (int i = 0; i < factura_ubl.UBLExtensions.Count(); i++)
				{
					if (factura_ubl.UBLExtensions[i].ExtensionContent.Name.Contains("sts:DianExtensions"))
					{
						foreach (XmlNode item in factura_ubl.UBLExtensions[i].ExtensionContent.ChildNodes)
						{
							if (item.LocalName.Equals("InvoiceControl"))
							{
								foreach (XmlNode item_child in item.ChildNodes)
								{
									if (item_child.LocalName.Equals("InvoiceAuthorization"))
									{
										factura_obj.NumeroResolucion = item_child.InnerText;
									}
									if (item_child.LocalName.Equals("AuthorizedInvoices"))
									{
										//Valida el nodo que contiene el prefijo
										foreach (XmlNode item_Authorization in item_child.ChildNodes)
										{
											if (item_Authorization.LocalName.Equals("Prefix"))
											{
												factura_obj.Prefijo = item_Authorization.InnerText;
											}
										}
										if (factura_obj.Prefijo == null)
										{
											factura_obj.Prefijo = string.Empty;
										}
									}
								}
							}
							else if (item.LocalName.Equals("SoftwareProvider"))
							{
								foreach (XmlNode item_child in item.ChildNodes)
								{
									if (item_child.LocalName.Equals("ProviderID"))
									{
										factura_obj.IdentificacionProveedor = item_child.InnerText;
									}
								}
							}
						}

						//Si ya se lleno la informacion de la resolucion 
						if (!string.IsNullOrEmpty(factura_obj.NumeroResolucion))
						{
							i = factura_ubl.UBLExtensions.Count();
						}


					}

					

				}

				

				//Si tiene prefijo lo substrae para dejar el numero del documento como entero
				if (!string.IsNullOrEmpty(factura_obj.Prefijo))
				{
					string documento = factura_ubl.ID.Value;
					if (documento.Substring(0, factura_obj.Prefijo.Count()).Equals(factura_obj.Prefijo))
					{
						factura_obj.Documento = Convert.ToInt64(documento.Substring(factura_obj.Prefijo.Count()));
					}
				}
				else
				{
					factura_obj.Documento = Convert.ToInt64(factura_ubl.ID.Value);
				}

				factura_obj.OrderReference = new ReferenciaAdicional();
				factura_obj.DocumentoRef = string.Empty;

				//Valida si tiene documento referencia de factura
				if (factura_ubl.OrderReference != null)
				{
					if (factura_ubl.OrderReference.ID != null)
					{
						factura_obj.OrderReference.Documento = factura_ubl.OrderReference.ID.Value;
						factura_obj.DocumentoRef = factura_ubl.OrderReference.ID.Value;
					}

				}

				//Valida si tiene documento referencia de pedido
				if (factura_ubl.DespatchDocumentReference != null)
				{
					factura_obj.DespatchDocument = new List<ReferenciaAdicional>();
					foreach (var item in factura_ubl.DespatchDocumentReference)
					{
						ReferenciaAdicional despacho = new ReferenciaAdicional(); 
						if (item.ID.Value != null)
						{
							despacho.Documento = item.ID.Value;
						}
						factura_obj.DespatchDocument.Add(despacho);
					}
					/*if (factura_ubl.DespatchDocumentReference.FirstOrDefault().ID.Value != null)
					{
						factura_obj.PedidoRef = factura_ubl.DespatchDocumentReference.FirstOrDefault().ID.Value;
					}*/

				}

				if (factura_ubl.AdditionalDocumentReference != null)
				{
					factura_obj.DocumentosReferencia = new List<ReferenciaAdicional>();
					foreach (var item in factura_ubl.AdditionalDocumentReference)
					{
						ReferenciaAdicional adicional = new ReferenciaAdicional();
						if (item.ID.Value != null)
						{
							adicional.Documento = item.ID.Value;

							if (item.DocumentTypeCode != null)
								adicional.CodigoReferencia = item.DocumentTypeCode.Value;
						}
						factura_obj.DocumentosReferencia.Add(adicional);
					}
				}

				factura_obj.Cufe = factura_ubl.UUID.Value;
				//string fecha_hora = string.Format("{0}{1}", factura_ubl.IssueDate.Value.ToString(Fecha.formato_fecha_hginet), factura_ubl.IssueTime.Value);
				DateTime hora = Convert.ToDateTime(factura_ubl.IssueTime.Value).AddHours(-5);
				DateTime fecha = factura_ubl.IssueDate.Value;
				DateTime fecha_hora = new DateTime(fecha.Year, fecha.Month, fecha.Day, hora.Hour, hora.Minute, hora.Second);
				factura_obj.Fecha = fecha_hora;
				if (factura_ubl.DueDate != null)
					factura_obj.FechaVence = factura_ubl.DueDate.Value;
				else if (factura_ubl.PaymentMeans.FirstOrDefault().PaymentDueDate != null)
					factura_obj.FechaVence = factura_ubl.PaymentMeans.FirstOrDefault().PaymentDueDate.Value;
				else
					factura_obj.FechaVence = fecha_hora;

				factura_obj.FormaPago = Convert.ToInt16(factura_ubl.PaymentMeans.FirstOrDefault().ID.Value);

				if (factura_ubl.PaymentMeans.FirstOrDefault().PaymentMeansCode == null)
					factura_obj.TerminoPago = 0;
				else
				{
					if (string.IsNullOrEmpty(factura_ubl.PaymentMeans.FirstOrDefault().PaymentMeansCode.Value) || factura_ubl.PaymentMeans.FirstOrDefault().PaymentMeansCode.Value.Equals("ZZZ"))
						factura_obj.TerminoPago = 0;
					else
						try
						{
							factura_obj.TerminoPago = Convert.ToInt16(factura_ubl.PaymentMeans.FirstOrDefault().PaymentMeansCode.Value);
						}
						catch (Exception)
						{
						  
						}
				}

				if (factura_ubl.PaymentTerms != null)
				{
					if (factura_ubl.PaymentTerms.FirstOrDefault().SettlementPeriod != null)
					{
						if (factura_ubl.PaymentTerms.FirstOrDefault().SettlementPeriod.DurationMeasure != null)
							factura_obj.Plazo = Convert.ToInt16(factura_ubl.PaymentTerms.FirstOrDefault().SettlementPeriod.DurationMeasure.Value);
					}
				}

				if (!interopeabilidad)
				{
					//--Falta este proceso de Validar si tiene cuotas
					#region Cuotas

					if (factura_ubl.PaymentTerms != null)
					{
						if (factura_ubl.PaymentTerms.Count() > 1)
						{
							List<Cuota> cuotas = new List<Cuota>();

							for (int i = 0; i < factura_ubl.PaymentTerms.Count(); i++)
							{
								Cuota cuota = new Cuota();
								cuota.Codigo = factura_ubl.PaymentTerms[i].ID != null ? Convert.ToInt16(factura_ubl.PaymentTerms[i].ID.Value) : i + 1;
								cuota.Valor = factura_ubl.PaymentTerms[i].Amount != null ? factura_ubl.PaymentTerms[i].Amount.Value : 0;
								cuota.FechaVence = factura_ubl.PaymentTerms[i].PaymentDueDate != null ? factura_ubl.PaymentTerms[i].PaymentDueDate.Value : factura_obj.FechaVence;
								cuotas.Add(cuota);
							}
							factura_obj.Cuotas = cuotas;
							//factura_obj.TerminoPago = 3;
						}
						else
						{
							if (factura_ubl.PaymentTerms.FirstOrDefault().SettlementPeriod != null)
							{
								if (factura_ubl.PaymentTerms.FirstOrDefault().SettlementPeriod.DurationMeasure != null)
									factura_obj.Plazo = Convert.ToInt16(factura_ubl.PaymentTerms.FirstOrDefault().SettlementPeriod.DurationMeasure.Value);
							}
							if (factura_ubl.PaymentTerms.FirstOrDefault().PaymentMeansID != null)
							{
								try
								{
									factura_obj.TerminoPago = Convert.ToInt16(factura_ubl.PaymentTerms.FirstOrDefault().PaymentMeansID.FirstOrDefault().Value);
								}
								catch (Exception)
								{

								}
							}
						}
					}
					#endregion

					factura_obj.Moneda = factura_ubl.DocumentCurrencyCode.Value;

					//Valida si trae mas notas para validar los campos
					if (factura_ubl.Note != null && factura_ubl.Note.Length > 1)
						if (factura_ubl.Note[1] != null)
							factura_obj.Nota = factura_ubl.Note[1].Value;
						else
							factura_obj.Nota = string.Empty;
					factura_obj.Notas = new List<string>();

					//---Se debe validar tema del formato ya no se guarda en el XML
					#region Guardado Formato
					/*
								if (factura_ubl.UBLExtensions.Count() > 2)
								{
									foreach (XmlNode item in factura_ubl.UBLExtensions[2].ExtensionContent.ChildNodes)
									{
										if (item.LocalName.Equals("PdfFormat"))
										{
											Formato documento_formato = new Formato();
											List<FormatoCampo> lista_campos = new List<FormatoCampo>();

											try
											{
												//Deserializa la posición 1 y las convierte en FormatoCampo
												dynamic jsonObj = JsonConvert.DeserializeObject(item.InnerText);

												if (jsonObj != null)
												{
													documento_formato.Codigo = jsonObj.Codigo;
													documento_formato.Titulo = jsonObj.Titulo;

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
											catch (Exception)
											{
												factura_obj.Notas.Add(item.InnerText);
											}

											documento_formato.CamposPredeterminados = lista_campos;
											factura_obj.DocumentoFormato = documento_formato;
										}

									}
								}*/
					#endregion
				}

				#region Datos del Adquiriente

				factura_obj.DatosAdquiriente = TerceroXMLv2_1.Obtener_adquiriente(factura_ubl.AccountingCustomerParty);

				#endregion

				#region Datos del Obligado

				factura_obj.DatosObligado = TerceroXMLv2_1.Obtener_obligado(factura_ubl.AccountingSupplierParty);

				#endregion


				if (!interopeabilidad)
				{
					#region Detalle de Documento
					List<DocumentoDetalle> list_detalle = new List<DocumentoDetalle>();

					//Recorre todo el detalle del documento 
					for (int i = 0; i < factura_ubl.InvoiceLine.Length; i++)
					{
						DocumentoDetalle detalle = new DocumentoDetalle();

						if (!string.IsNullOrWhiteSpace(factura_ubl.InvoiceLine[i].ID.Value) && Texto.ValidarExpresion(TipoExpresion.Numero, factura_ubl.InvoiceLine[i].ID.Value))
						{
							try
							{
								detalle.Codigo = Convert.ToInt16(factura_ubl.InvoiceLine[i].ID.Value);
							}
							catch (Exception)
							{
								detalle.Codigo += (i == 0) ? 1 : i;
							}
						}
						else
						{
							detalle.Codigo += (i == 0) ? 1 : i;

						}

						if (factura_ubl.InvoiceLine[i].Item.SellersItemIdentification != null)
						{
							detalle.ProductoCodigo = factura_ubl.InvoiceLine[i].Item.SellersItemIdentification.ID.Value;
						}
						else if (factura_ubl.InvoiceLine[i].Item.StandardItemIdentification != null)
						{
							detalle.ProductoCodigoEAN = factura_ubl.InvoiceLine[i].Item.StandardItemIdentification.ID.Value;
							if (string.IsNullOrEmpty(detalle.ProductoCodigo) || detalle.ProductoCodigo.Equals("0"))
								detalle.ProductoCodigo = detalle.ProductoCodigoEAN;
						}
						detalle.ProductoNombre = factura_ubl.InvoiceLine[i].Item.Description[0].Value;
						if (factura_ubl.InvoiceLine[i].Item.AdditionalInformation != null)
						{
							detalle.ProductoDescripcion = factura_ubl.InvoiceLine[i].Item.AdditionalInformation.FirstOrDefault().Value;
						}
						else
						{
							detalle.ProductoDescripcion = string.Empty;
						}
						if (factura_ubl.InvoiceLine[i].Item.OriginAddress != null)
						{
							detalle.Bodega = factura_ubl.InvoiceLine[i].Item.OriginAddress[0].ID.Value;
						}
						else
						{
							detalle.Bodega = string.Empty;
						}
						detalle.Cantidad = factura_ubl.InvoiceLine[i].InvoicedQuantity.Value;
						if (factura_ubl.InvoiceLine[i].InvoicedQuantity.unitCode != null && !string.IsNullOrEmpty(factura_ubl.InvoiceLine[i].InvoicedQuantity.unitCode.ToString()))
						{
							detalle.UnidadCodigo = factura_ubl.InvoiceLine[i].InvoicedQuantity.unitCode.ToString();
						}
						else
						{
							detalle.UnidadCodigo = "94";
						}
						detalle.ValorUnitario = factura_ubl.InvoiceLine[i].Price.PriceAmount.Value;
						detalle.ValorSubtotal = factura_ubl.InvoiceLine[i].LineExtensionAmount.Value;

						if (factura_ubl.InvoiceLine[i].Item.AdditionalItemProperty != null)
						{
							detalle.CamposAdicionales = new List<CampoValor>();
							foreach (ItemPropertyType item in factura_ubl.InvoiceLine[i].Item.AdditionalItemProperty)
							{
								if (item.Name.Value != null && item.Name.Value.Equals("Item Oculto para Impresion"))
								{
									detalle.OcultarItem = Convert.ToInt16(item.Value.Value);
								}
								else
								{
									CampoValor campo = new CampoValor();
									campo.Descripcion = item.Name.Value != null ? item.Name.Value : string.Empty;
									campo.Valor = item.Value.Value != null ? item.Value.Value : string.Empty;
									detalle.CamposAdicionales.Add(campo);
								}

							}

						}

						if (factura_ubl.InvoiceLine[i].AllowanceCharge != null)
						{

							detalle.DescuentoValor = factura_ubl.InvoiceLine[i].AllowanceCharge.FirstOrDefault().Amount.Value;
							if (factura_ubl.InvoiceLine[i].AllowanceCharge.FirstOrDefault().MultiplierFactorNumeric != null)
							{
								detalle.DescuentoPorcentaje = factura_ubl.InvoiceLine[i].AllowanceCharge.FirstOrDefault().MultiplierFactorNumeric.Value;
							}
							else
							{
								detalle.DescuentoPorcentaje = decimal.Round((detalle.DescuentoValor / (detalle.ValorUnitario * detalle.Cantidad)),6,MidpointRounding.AwayFromZero);
							}
							
						}
						else
						{
							detalle.DescuentoPorcentaje = 0.00M;
							detalle.DescuentoValor = 0.00M;
						}

						if (factura_ubl.InvoiceLine[i].FreeOfChargeIndicator != null)
						{
							if (factura_ubl.InvoiceLine[i].PricingReference != null)
							{
								if (factura_ubl.InvoiceLine[i].LineExtensionAmount != null && factura_ubl.InvoiceLine[i].LineExtensionAmount.Value == 0)
								{
									detalle.ProductoGratis = factura_ubl.InvoiceLine[i].FreeOfChargeIndicator.Value;
									if (factura_ubl.InvoiceLine[i].PricingReference.AlternativeConditionPrice.FirstOrDefault().PriceTypeCode != null)
										detalle.ProductoGratisPrecioRef = factura_ubl.InvoiceLine[i].PricingReference.AlternativeConditionPrice.FirstOrDefault().PriceTypeCode.Value;
								}
							}	
						}

						// valida que el detalle contenga el tag TaxTotal
						if (factura_ubl.InvoiceLine[i].TaxTotal != null)
						{
							for (int x = 0; x < factura_ubl.InvoiceLine[i].TaxTotal.Count(); x++)
							{
								if (factura_ubl.InvoiceLine[i].TaxTotal[x].TaxSubtotal != null && factura_ubl.InvoiceLine[i].TaxTotal[x].TaxSubtotal.Count() > 0)
								{
									for (int j = 0; j < factura_ubl.InvoiceLine[i].TaxTotal[x].TaxSubtotal.Count(); j++)
									{
										string tipo_impto = factura_ubl.InvoiceLine[i].TaxTotal[x].TaxSubtotal[j].TaxCategory.TaxScheme.ID.Value;

										decimal porcentaje_impto = 0;
										if (factura_ubl.InvoiceLine[i].TaxTotal[x].TaxSubtotal[j].TaxCategory.Percent != null)
										{
											porcentaje_impto = factura_ubl.InvoiceLine[i].TaxTotal[x].TaxSubtotal[j].TaxCategory.Percent.Value;
										}

										decimal valor_impto = factura_ubl.InvoiceLine[i].TaxTotal[x].TaxSubtotal[j].TaxAmount.Value;

										if (TipoImpuestos.Iva.Equals(tipo_impto))
										{
											detalle.IvaPorcentaje = porcentaje_impto;
											detalle.IvaValor = valor_impto;
										}
										else if (TipoImpuestos.Consumo.Equals(tipo_impto))
										{
											detalle.ImpoConsumoPorcentaje = porcentaje_impto;
											detalle.ValorImpuestoConsumo = valor_impto;
											factura_obj.ValorImpuestoConsumo += detalle.ValorImpuestoConsumo;
										}
										else if (TipoImpuestos.Ica.Equals(tipo_impto))
										{
											detalle.ReteIcaPorcentaje = porcentaje_impto;
											detalle.ReteIcaValor = valor_impto;
											factura_obj.ValorReteIca += detalle.ReteIcaValor;
										}
										else if (TipoImpuestos.ReteFte.Equals(tipo_impto))
										{
											detalle.ReteFuentePorcentaje = porcentaje_impto;
											detalle.ReteFuenteValor = valor_impto;
											factura_obj.ValorReteFuente += detalle.ReteFuenteValor;
										}
										else if (tipo_impto.Equals("22"))//Impuesto a la bolsa
										{
											detalle.ValorImpuestoConsumo = valor_impto;
											factura_obj.ValorImpuestoConsumo += detalle.ValorImpuestoConsumo;
										}

									}
								}
								
							}
						}

						list_detalle.Add(detalle);

					}
					factura_obj.DocumentoDetalles = list_detalle;
					#endregion


					try
					{
						if (factura_ubl.AllowanceCharge != null && factura_ubl.AllowanceCharge.Length > 0)
						{
							List<Descuento> list_desc = new List<Descuento>();
							foreach (var item in factura_ubl.AllowanceCharge)
							{
								Descuento desc = new Descuento();
								desc.Codigo = (item.AllowanceChargeReasonCode != null) ? item.AllowanceChargeReasonCode.Value : "11";
								desc.Descripcion = (item.AllowanceChargeReason != null) ? item.AllowanceChargeReason.FirstOrDefault().Value : "";
								desc.Porcentaje = item.MultiplierFactorNumeric.Value;
								desc.Valor = item.Amount.Value;
								list_desc.Add(desc);
							}

							factura_obj.Descuentos = list_desc;
						}
					}
					catch (Exception ex)
					{
						string mensaje = string.Format("Se presento inconsistencia convirtiendo los descuentos. Detalle: {0}", ex.Message);
						RegistroLog.EscribirLog(ex, MensajeCategoria.Convertir, MensajeTipo.Error, MensajeAccion.ninguna, mensaje);
					}

					#region Totales
				
					factura_obj.ValorSubtotal = factura_ubl.LegalMonetaryTotal.LineExtensionAmount.Value;
					factura_obj.Valor = factura_obj.ValorSubtotal + factura_obj.DocumentoDetalles.Sum(b => b.DescuentoValor);
					if (factura_ubl.LegalMonetaryTotal.AllowanceTotalAmount != null)
						factura_obj.ValorDescuento = factura_ubl.LegalMonetaryTotal.AllowanceTotalAmount.Value;
					if (factura_ubl.LegalMonetaryTotal.ChargeTotalAmount != null)
						factura_obj.ValorCargo = factura_ubl.LegalMonetaryTotal.ChargeTotalAmount.Value;
					if (factura_ubl.LegalMonetaryTotal.PrepaidAmount != null)
						factura_obj.ValorAnticipo = factura_ubl.LegalMonetaryTotal.PrepaidAmount.Value;
					factura_obj.ValorIva = factura_ubl.LegalMonetaryTotal.TaxInclusiveAmount.Value - factura_ubl.LegalMonetaryTotal.LineExtensionAmount.Value;
					factura_obj.Total = factura_ubl.LegalMonetaryTotal.PayableAmount.Value;

					//Se agrega validacion si ya se habia guardado el neto en Bd para utilizarlo o si no que sea el calculado
					if (documento_bd != null)
					{
						if (documento_bd.IntValorNeto == 0)
						{
							factura_obj.Neto = (factura_obj.Total - (factura_obj.ValorReteFuente + factura_obj.ValorReteIca + factura_obj.ValorReteIva));
						}
						else
						{
							factura_obj.Neto = documento_bd.IntValorNeto;
						}
					}
					else
					{
						factura_obj.Neto = (factura_obj.Total - (factura_obj.ValorReteFuente + factura_obj.ValorReteIca + factura_obj.ValorReteIva));
					}

					#endregion

					//Direccion de entrega
					if (factura_ubl.Delivery != null && factura_ubl.Delivery.Length >= 1)
					{
						if (factura_ubl.Delivery[0].DeliveryAddress != null)
						{
							factura_obj.DatosAdquiriente.DireccionEntrega = new Direcciones();
							if (factura_ubl.Delivery[0].DeliveryAddress.CityName != null)
								factura_obj.DatosAdquiriente.DireccionEntrega.Ciudad = factura_ubl.Delivery[0].DeliveryAddress.CityName.Value;
							if (factura_ubl.Delivery[0].DeliveryAddress.ID != null)
								factura_obj.DatosAdquiriente.DireccionEntrega.CodigoCiudad = factura_ubl.Delivery[0].DeliveryAddress.ID.Value;
							if (factura_ubl.Delivery[0].DeliveryAddress.CountrySubentity != null)
								factura_obj.DatosAdquiriente.DireccionEntrega.Departamento = factura_ubl.Delivery[0].DeliveryAddress.CountrySubentity.Value;
							if (factura_ubl.Delivery[0].DeliveryAddress.CountrySubentityCode != null)
								factura_obj.DatosAdquiriente.DireccionEntrega.CodigoDepartamento = factura_ubl.Delivery[0].DeliveryAddress.CountrySubentityCode.Value;
							if (factura_ubl.Delivery[0].DeliveryAddress.Country != null)
								factura_obj.DatosAdquiriente.DireccionEntrega.CodigoPais = factura_ubl.Delivery[0].DeliveryAddress.Country.IdentificationCode.Value;
							if (factura_ubl.Delivery[0].DeliveryAddress.AddressLine != null)
								factura_obj.DatosAdquiriente.DireccionEntrega.Direccion = factura_ubl.Delivery[0].DeliveryAddress.AddressLine[0].Line.Value;
							try
							{
								ListaPaises list_paises = new ListaPaises();
								ListaItem pais = list_paises.Items.Where(d => d.Codigo.Equals(factura_obj.DatosAdquiriente.DireccionEntrega.CodigoPais)).FirstOrDefault();
								factura_obj.DatosAdquiriente.DireccionEntrega.Pais = pais.Descripcion;
							}
							catch (Exception)
							{ }
							if (factura_ubl.Delivery[0].DeliveryAddress.PostalZone != null)
								factura_obj.DatosAdquiriente.DireccionEntrega.CodigoPostal = factura_ubl.Delivery[0].DeliveryAddress.PostalZone.Value;
						}
					}

					//Condiciones de Entrega
					if (factura_ubl.DeliveryTerms != null)
					{
						factura_obj.TipoEntrega = new CondicionEntrega();
						if (factura_ubl.DeliveryTerms.LossRiskResponsibilityCode != null)
							factura_obj.TipoEntrega.CodCondicionEntrega = factura_ubl.DeliveryTerms.LossRiskResponsibilityCode.Value;

					}

					//Tasa de Cambio
					if (factura_ubl.PaymentExchangeRate != null)
					{
						if (factura_ubl.PaymentExchangeRate.SourceCurrencyCode.Value != null && !factura_ubl.PaymentExchangeRate.SourceCurrencyCode.Value.Equals(factura_ubl.PaymentExchangeRate.TargetCurrencyCode.Value))
						{
							factura_obj.Trm = new TasaCambio();
							factura_obj.Trm.Moneda = factura_ubl.PaymentExchangeRate.TargetCurrencyCode.Value;
							factura_obj.Trm.FechaTrm = factura_ubl.PaymentExchangeRate.Date.Value;
							factura_obj.Trm.Valor = factura_ubl.PaymentExchangeRate.CalculationRate != null ? factura_ubl.PaymentExchangeRate.CalculationRate.Value : 0;

						}

					}

				}

				return factura_obj;
			}
			catch (Exception ex)
			{
				string mensaje = string.Format("Se presento inconsistencia convirtiendo el xml de Factura a objeto. Detalle: {0}", ex.Message);
				RegistroLog.EscribirLog(ex, MensajeCategoria.Convertir, MensajeTipo.Error, MensajeAccion.ninguna, mensaje);
				throw new ApplicationException(mensaje, ex.InnerException);
			}
		}




	}
}
