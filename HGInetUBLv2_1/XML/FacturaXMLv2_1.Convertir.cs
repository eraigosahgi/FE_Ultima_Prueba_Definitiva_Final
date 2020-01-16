using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetUBLv2_1.Dian;
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
				foreach (XmlNode item in factura_ubl.UBLExtensions[0].ExtensionContent.ChildNodes)
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
					if (factura_ubl.OrderReference.ID.Value != null)
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
				factura_obj.FechaVence = factura_ubl.DueDate.Value;
				factura_obj.FormaPago = Convert.ToInt16(factura_ubl.PaymentMeans.FirstOrDefault().ID.Value);
				if (factura_ubl.PaymentMeans.FirstOrDefault().PaymentMeansCode.Value.Equals("ZZZ"))
					factura_obj.TerminoPago = 0;
				else
					factura_obj.TerminoPago = Convert.ToInt16(factura_ubl.PaymentMeans.FirstOrDefault().PaymentMeansCode.Value);

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
								cuota.Codigo = Convert.ToInt16(factura_ubl.PaymentTerms[i].ID.Value);
								cuota.Valor = factura_ubl.PaymentTerms[i].Amount.Value;
								cuota.FechaVence = factura_ubl.PaymentTerms[i].PaymentDueDate.Value;
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
								factura_obj.TerminoPago = Convert.ToInt16(factura_ubl.PaymentTerms.FirstOrDefault().PaymentMeansID.FirstOrDefault().Value);
							}
						}
					}
					#endregion

					factura_obj.Moneda = factura_ubl.DocumentCurrencyCode.Value;

					//Valida si trae mas notas para validar los campos
					if (factura_ubl.Note.Length > 1)
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

				/*
				Tercero adquiriente = new Tercero();

				adquiriente.Identificacion = factura_ubl.AccountingCustomerParty.Party.PartyTaxScheme.FirstOrDefault().CompanyID.Value;
				adquiriente.IdentificacionDv = Convert.ToInt16(factura_ubl.AccountingCustomerParty.Party.PartyTaxScheme.FirstOrDefault().CompanyID.schemeID);
				adquiriente.TipoIdentificacion = Convert.ToInt16(factura_ubl.AccountingCustomerParty.Party.PartyTaxScheme.FirstOrDefault().CompanyID.schemeName);
				adquiriente.TipoPersona = Convert.ToInt16(factura_ubl.AccountingCustomerParty.AdditionalAccountID.FirstOrDefault().Value);
				adquiriente.RegimenFiscal = factura_ubl.AccountingCustomerParty.Party.PartyTaxScheme.FirstOrDefault().TaxLevelCode.listName;
				adquiriente.Responsabilidades = LibreriaGlobalHGInet.Formato.Coleccion.ConvertirLista(factura_ubl.AccountingCustomerParty.Party.PartyTaxScheme.FirstOrDefault().TaxLevelCode.Value, ';');
				adquiriente.CodigoTributo = factura_ubl.AccountingCustomerParty.Party.PartyTaxScheme.FirstOrDefault().TaxScheme.ID.Value;
				adquiriente.RazonSocial = factura_ubl.AccountingCustomerParty.Party.PartyTaxScheme.FirstOrDefault().RegistrationName.Value;
				adquiriente.NombreComercial = factura_ubl.AccountingCustomerParty.Party.PartyLegalEntity.FirstOrDefault().CorporateRegistrationScheme.Name.Value;

				//Valida si es persona Natural 
				if (factura_ubl.AccountingCustomerParty.Party.Person != null)
				{
					adquiriente.PrimerNombre = factura_ubl.AccountingCustomerParty.Party.Person.FirstOrDefault().FirstName.Value;
					if (factura_ubl.AccountingCustomerParty.Party.Person.FirstOrDefault().MiddleName != null)
						adquiriente.SegundoNombre = factura_ubl.AccountingCustomerParty.Party.Person.FirstOrDefault().MiddleName.Value;
					if (factura_ubl.AccountingCustomerParty.Party.Person.FirstOrDefault().FamilyName != null)
						adquiriente.PrimerApellido = factura_ubl.AccountingCustomerParty.Party.Person.FirstOrDefault().FamilyName.Value;
				}
				
				adquiriente.Direccion = factura_ubl.AccountingCustomerParty.Party.PhysicalLocation.Address.AddressLine[0].Line.Value;
				adquiriente.Ciudad = factura_ubl.AccountingCustomerParty.Party.PhysicalLocation.Address.CityName.Value;
				adquiriente.CodigoCiudad = factura_ubl.AccountingCustomerParty.Party.PhysicalLocation.Address.ID.Value;
				adquiriente.Departamento = factura_ubl.AccountingCustomerParty.Party.PhysicalLocation.Address.CountrySubentity.Value;
				adquiriente.CodigoDepartamento = factura_ubl.AccountingCustomerParty.Party.PhysicalLocation.Address.CountrySubentityCode.Value;
				adquiriente.CodigoPais = factura_ubl.AccountingCustomerParty.Party.PhysicalLocation.Address.Country.IdentificationCode.Value;

				//Valida si tiene Contacto
				if (factura_ubl.AccountingCustomerParty.Party.Contact != null)
				{
					adquiriente.Telefono = factura_ubl.AccountingCustomerParty.Party.Contact.Telephone.Value;
					adquiriente.Email = factura_ubl.AccountingCustomerParty.Party.Contact.ElectronicMail.Value;
				}
				//Valida si tiene pagina web
				if (factura_ubl.AccountingCustomerParty.Party.WebsiteURI != null)
					adquiriente.PaginaWeb = factura_ubl.AccountingCustomerParty.Party.WebsiteURI.Value;
				factura_obj.DatosAdquiriente = adquiriente;*/
				#endregion

				#region Datos del Obligado

				factura_obj.DatosObligado = TerceroXMLv2_1.Obtener_obligado(factura_ubl.AccountingSupplierParty);
				/*
				Tercero obligado = new Tercero();
				
				obligado.Identificacion = factura_ubl.AccountingSupplierParty.Party.PartyTaxScheme.FirstOrDefault().CompanyID.Value;
				obligado.IdentificacionDv = Convert.ToInt16(factura_ubl.AccountingSupplierParty.Party.PartyTaxScheme.FirstOrDefault().CompanyID.schemeID);
				obligado.TipoIdentificacion = Convert.ToInt16(factura_ubl.AccountingSupplierParty.Party.PartyTaxScheme.FirstOrDefault().CompanyID.schemeName);
				obligado.TipoPersona = Convert.ToInt16(factura_ubl.AccountingSupplierParty.AdditionalAccountID.FirstOrDefault().Value);
				obligado.RegimenFiscal = factura_ubl.AccountingSupplierParty.Party.PartyTaxScheme.FirstOrDefault().TaxLevelCode.listName;
				obligado.Responsabilidades = LibreriaGlobalHGInet.Formato.Coleccion.ConvertirLista(factura_ubl.AccountingSupplierParty.Party.PartyTaxScheme.FirstOrDefault().TaxLevelCode.Value, ';');
				obligado.CodigoTributo = factura_ubl.AccountingSupplierParty.Party.PartyTaxScheme.FirstOrDefault().TaxScheme.ID.Value;
				obligado.RazonSocial = factura_ubl.AccountingSupplierParty.Party.PartyTaxScheme.FirstOrDefault().RegistrationName.Value;
				obligado.NombreComercial = factura_ubl.AccountingSupplierParty.Party.PartyLegalEntity.FirstOrDefault().CorporateRegistrationScheme.Name.Value;

				//Valida si es persona Natural
				if (factura_ubl.AccountingSupplierParty.Party.Person != null)
				{
					obligado.PrimerNombre = factura_ubl.AccountingSupplierParty.Party.Person.FirstOrDefault().FirstName.Value;
					if (factura_ubl.AccountingSupplierParty.Party.Person.FirstOrDefault().MiddleName != null)
						obligado.SegundoNombre = factura_ubl.AccountingSupplierParty.Party.Person.FirstOrDefault().MiddleName.Value;
					if (factura_ubl.AccountingSupplierParty.Party.Person.FirstOrDefault().FamilyName != null)
						obligado.PrimerApellido = factura_ubl.AccountingSupplierParty.Party.Person.FirstOrDefault().FamilyName.Value;
				}


				obligado.Direccion = factura_ubl.AccountingSupplierParty.Party.PhysicalLocation.Address.AddressLine[0].Line.Value;
				obligado.Ciudad = factura_ubl.AccountingSupplierParty.Party.PhysicalLocation.Address.CityName.Value;
				obligado.CodigoCiudad = factura_ubl.AccountingSupplierParty.Party.PhysicalLocation.Address.ID.Value;
				obligado.Departamento = factura_ubl.AccountingSupplierParty.Party.PhysicalLocation.Address.CountrySubentity.Value;
				obligado.CodigoDepartamento = factura_ubl.AccountingSupplierParty.Party.PhysicalLocation.Address.CountrySubentityCode.Value;
				obligado.CodigoPais = factura_ubl.AccountingSupplierParty.Party.PhysicalLocation.Address.Country.IdentificationCode.Value;

				if (factura_ubl.AccountingSupplierParty.Party.Contact != null)
				{
					obligado.Telefono = factura_ubl.AccountingSupplierParty.Party.Contact.Telephone.Value;
					obligado.Email = factura_ubl.AccountingSupplierParty.Party.Contact.ElectronicMail.Value;
					//Valida si tiene pagina web
					if (factura_ubl.AccountingSupplierParty.Party.WebsiteURI != null)
						obligado.PaginaWeb = factura_ubl.AccountingSupplierParty.Party.WebsiteURI.Value;
				}
				factura_obj.DatosObligado = obligado;*/

				#endregion


				if (!interopeabilidad)
				{
					#region Detalle de Documento
					List<DocumentoDetalle> list_detalle = new List<DocumentoDetalle>();

					//Recorre todo el detalle del documento
					for (int i = 0; i < factura_ubl.InvoiceLine.Length; i++)
					{

						DocumentoDetalle detalle = new DocumentoDetalle();

						if (Texto.ValidarExpresion(TipoExpresion.Numero, factura_ubl.InvoiceLine[i].ID.Value))
						{
							detalle.Codigo = Convert.ToInt16(factura_ubl.InvoiceLine[i].ID.Value);
						}
						else
						{
							detalle.Codigo += i;

						}

						if (factura_ubl.InvoiceLine[i].Item.CatalogueItemIdentification != null)
						{
							detalle.ProductoCodigo = factura_ubl.InvoiceLine[i].Item.SellersItemIdentification.ID.Value;
						}
						else if (factura_ubl.InvoiceLine[i].Item.StandardItemIdentification != null)
						{
							detalle.ProductoCodigoEAN = factura_ubl.InvoiceLine[i].Item.StandardItemIdentification.ID.Value;
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
						if (!string.IsNullOrEmpty(factura_ubl.InvoiceLine[i].InvoicedQuantity.unitCode.ToString()))
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
							detalle.OcultarItem = Convert.ToInt16(factura_ubl.InvoiceLine[i].Item.AdditionalItemProperty[0].Value.Value);
						}

						if (factura_ubl.InvoiceLine[i].AllowanceCharge != null)
						{
							detalle.DescuentoPorcentaje = factura_ubl.InvoiceLine[i].AllowanceCharge.FirstOrDefault().MultiplierFactorNumeric.Value;
							detalle.DescuentoValor = factura_ubl.InvoiceLine[i].AllowanceCharge.FirstOrDefault().Amount.Value;
						}
						else
						{
							detalle.DescuentoPorcentaje = 0.00M;
							detalle.DescuentoValor = 0.00M;
						}

						if (factura_ubl.InvoiceLine[i].FreeOfChargeIndicator != null)
						{
							detalle.ProductoGratis = factura_ubl.InvoiceLine[i].FreeOfChargeIndicator.Value;
							if (detalle.ProductoGratis == true)
								detalle.ProductoGratisPrecioRef = factura_ubl.InvoiceLine[i].PricingReference.AlternativeConditionPrice.FirstOrDefault().PriceTypeCode.Value;
						}

						// valida que el detalle contenga el tag TaxTotal
						if (factura_ubl.InvoiceLine[i].TaxTotal != null)
						{
							for (int j = 0; j < factura_ubl.InvoiceLine[i].TaxTotal[0].TaxSubtotal.Count(); j++)
							{
								string tipo_impto = factura_ubl.InvoiceLine[i].TaxTotal[0].TaxSubtotal[j].TaxCategory.TaxScheme.ID.Value;
								decimal porcentaje_impto = factura_ubl.InvoiceLine[i].TaxTotal[0].TaxSubtotal[j].TaxCategory.Percent.Value;
								decimal valor_impto = factura_ubl.InvoiceLine[i].TaxTotal[0].TaxSubtotal[j].TaxAmount.Value;

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

							}
						}

						list_detalle.Add(detalle);

					}
					factura_obj.DocumentoDetalles = list_detalle;
					#endregion


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
				}

				#endregion

				return factura_obj;
			}
			catch (Exception ex)
			{
				RegistroLog.EscribirLog(ex, MensajeCategoria.Convertir, MensajeTipo.Error, MensajeAccion.ninguna);
				throw new ApplicationException(ex.Message, ex.InnerException);
			}
		}




	}
}
