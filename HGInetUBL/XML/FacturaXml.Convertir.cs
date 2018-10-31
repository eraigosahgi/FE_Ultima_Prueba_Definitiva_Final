using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetUBL.Objetos;
using HGInetUBL.Recursos;
using LibreriaGlobalHGInet.Enumerables;
using LibreriaGlobalHGInet.Formato;
using LibreriaGlobalHGInet.General;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace HGInetUBL
{
	public partial class FacturaXML
	{
		/// <summary>
		/// Convierte el XML-UBL en un objeto de Servicio
		/// </summary>
		/// <param name="factura_ubl">Archivo XML-UBL</param>
		/// <param name="interopeabilidad">Indica si la conversión del archivo XML-UBL es sólo de encabezados</param>
		/// <returns>objeto tipo Factura</returns>
		public static Factura Convertir(InvoiceType factura_ubl, bool interopeabilidad = false)
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
				factura_obj.DocumentoRef = string.Empty;


				//Valida si tiene documento referencia de factura
				if (factura_ubl.BillingReference != null)
				{
					if (factura_ubl.BillingReference.FirstOrDefault().InvoiceDocumentReference.ID.Value != null)
					{
						factura_obj.DocumentoRef = factura_ubl.BillingReference.FirstOrDefault().InvoiceDocumentReference.ID.Value;
					}

				}

				//Valida si tiene documento referencia de pedido
				if (factura_ubl.OrderReference != null)
				{
					if (factura_ubl.OrderReference.FirstOrDefault().ID.Value != null)
					{
						factura_obj.PedidoRef = factura_ubl.OrderReference.FirstOrDefault().ID.Value;
					}

				}

				factura_obj.Cufe = factura_ubl.UUID.Value;
				factura_obj.Fecha = factura_ubl.IssueDate.Value;
				//Valida la fecha de vencimiento 
				if (factura_ubl.PaymentMeans != null)
				{
					factura_obj.FechaVence = factura_ubl.PaymentMeans.FirstOrDefault().PaymentDueDate.Value;
					factura_obj.FormaPago = Convert.ToInt16(factura_ubl.PaymentMeans.FirstOrDefault().PaymentMeansCode.Value);
				}
				else
				{
					factura_obj.FechaVence = factura_ubl.IssueDate.Value;
				}

				if (!interopeabilidad)
				{
					//Valida si tiene cuotas
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
								cuota.FechaVence = factura_ubl.PaymentTerms[i].SettlementPeriod.EndDate.Value;
								cuotas.Add(cuota);
							}
							factura_obj.Cuotas = cuotas;
							factura_obj.TerminoPago = 3;
						}
						else
						{
							if (factura_ubl.PaymentTerms.FirstOrDefault().SettlementPeriod.DurationMeasure != null)
							{
								factura_obj.Plazo = Convert.ToInt16(factura_ubl.PaymentTerms.FirstOrDefault().SettlementPeriod.DurationMeasure.Value);
							}
							if (factura_ubl.PaymentTerms.FirstOrDefault().PaymentMeansID != null)
							{
								factura_obj.TerminoPago = Convert.ToInt16(factura_ubl.PaymentTerms.FirstOrDefault().PaymentMeansID.Value);
							}
						}
					}


					factura_obj.Moneda = factura_ubl.DocumentCurrencyCode.Value;
					factura_obj.Nota = factura_ubl.Note[0].Value;
					factura_obj.Notas = new List<string>();

					//Valida si trae mas notas para validar los campos

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
					}
				}
				#region Datos del Adquiriente
				Tercero adquiriente = new Tercero();

				adquiriente.Identificacion = factura_ubl.AccountingCustomerParty.Party.PartyIdentification[0].ID.Value;
				adquiriente.TipoPersona = Convert.ToInt16(factura_ubl.AccountingCustomerParty.AdditionalAccountID.Value);
				adquiriente.Regimen = Convert.ToInt16(factura_ubl.AccountingCustomerParty.Party.PartyTaxScheme[0].TaxLevelCode.Value);
				adquiriente.TipoIdentificacion = Convert.ToInt16(factura_ubl.AccountingCustomerParty.Party.PartyIdentification[0].ID.schemeID);
				//Valida si es persona Natural 
				if (factura_ubl.AccountingCustomerParty.Party.Person != null)
				{
					adquiriente.PrimerNombre = factura_ubl.AccountingCustomerParty.Party.Person.FirstName.Value;
					if (factura_ubl.AccountingCustomerParty.Party.Person.MiddleName != null)
						adquiriente.SegundoNombre = factura_ubl.AccountingCustomerParty.Party.Person.MiddleName.Value;
					if (factura_ubl.AccountingCustomerParty.Party.Person.FamilyName != null)
						adquiriente.PrimerApellido = factura_ubl.AccountingCustomerParty.Party.Person.FamilyName.Value;
					adquiriente.RazonSocial = string.Format("{0} {1} {2}", adquiriente.PrimerApellido, adquiriente.PrimerNombre, adquiriente.SegundoNombre);
				}
				else
				{
					adquiriente.RazonSocial = factura_ubl.AccountingCustomerParty.Party.PartyLegalEntity[0].RegistrationName.Value;
				}
				adquiriente.Direccion = factura_ubl.AccountingCustomerParty.Party.PhysicalLocation.Address.AddressLine[0].Line.Value;
				adquiriente.Ciudad = factura_ubl.AccountingCustomerParty.Party.PhysicalLocation.Address.CityName.Value;
				adquiriente.Departamento = factura_ubl.AccountingCustomerParty.Party.PhysicalLocation.Address.Department.Value;
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
				factura_obj.DatosAdquiriente = adquiriente;
				#endregion

				#region Datos del Obligado
				Tercero obligado = new Tercero();

				obligado.Identificacion = factura_ubl.AccountingSupplierParty.Party.PartyIdentification[0].ID.Value;
				obligado.TipoPersona = Convert.ToInt16(factura_ubl.AccountingSupplierParty.AdditionalAccountID.Value);
				obligado.Regimen = Convert.ToInt16(factura_ubl.AccountingSupplierParty.Party.PartyTaxScheme[0].TaxLevelCode.Value);
				obligado.TipoIdentificacion = Convert.ToInt16(factura_ubl.AccountingSupplierParty.Party.PartyIdentification[0].ID.schemeID);
				//Valida si es persona Natural
				if (factura_ubl.AccountingSupplierParty.Party.Person != null)
				{
					obligado.PrimerNombre = factura_ubl.AccountingSupplierParty.Party.Person.FirstName.Value;
					if (factura_ubl.AccountingSupplierParty.Party.Person.MiddleName != null)
						obligado.SegundoNombre = factura_ubl.AccountingSupplierParty.Party.Person.MiddleName.Value;
					if (factura_ubl.AccountingSupplierParty.Party.Person.FamilyName != null)
						obligado.PrimerApellido = factura_ubl.AccountingSupplierParty.Party.Person.FamilyName.Value;
					obligado.RazonSocial = string.Format("{0} {1} {2}", obligado.PrimerApellido, obligado.PrimerNombre, obligado.SegundoNombre);
				}
				else
				{
					obligado.RazonSocial = factura_ubl.AccountingSupplierParty.Party.PartyLegalEntity[0].RegistrationName.Value;
				}
				obligado.Direccion = factura_ubl.AccountingSupplierParty.Party.PhysicalLocation.Address.AddressLine[0].Line.Value;
				obligado.Ciudad = factura_ubl.AccountingSupplierParty.Party.PhysicalLocation.Address.CityName.Value;
				obligado.Departamento = factura_ubl.AccountingSupplierParty.Party.PhysicalLocation.Address.Department.Value;
				obligado.CodigoPais = factura_ubl.AccountingSupplierParty.Party.PhysicalLocation.Address.Country.IdentificationCode.Value;
				if (factura_ubl.AccountingSupplierParty.Party.Contact != null)
				{
					obligado.Telefono = factura_ubl.AccountingSupplierParty.Party.Contact.Telephone.Value;
					obligado.Email = factura_ubl.AccountingSupplierParty.Party.Contact.ElectronicMail.Value;
					//Valida si tiene pagina web
					if (factura_ubl.AccountingSupplierParty.Party.WebsiteURI != null)
						obligado.PaginaWeb = factura_ubl.AccountingSupplierParty.Party.WebsiteURI.Value;
				}
				factura_obj.DatosObligado = obligado;

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
							detalle.ProductoCodigo = factura_ubl.InvoiceLine[i].Item.CatalogueItemIdentification.ID.Value;
						}
						else if (factura_ubl.InvoiceLine[i].Item.StandardItemIdentification != null)
						{
							detalle.ProductoCodigo = factura_ubl.InvoiceLine[i].Item.StandardItemIdentification.ID.Value;
						}
						detalle.ProductoNombre = factura_ubl.InvoiceLine[i].Item.Description[0].Value;
						if (factura_ubl.InvoiceLine[i].Item.AdditionalInformation != null)
						{
							detalle.ProductoDescripcion = factura_ubl.InvoiceLine[i].Item.AdditionalInformation.Value;
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
							detalle.UnidadCodigo = "S7";
						}
						detalle.ValorUnitario = factura_ubl.InvoiceLine[i].Price.PriceAmount.Value;
						detalle.ValorSubtotal = factura_ubl.InvoiceLine[i].LineExtensionAmount.Value;

						if (factura_ubl.InvoiceLine[i].Item.AdditionalItemProperty != null)
						{
							detalle.OcultarItem = Convert.ToUInt16(factura_ubl.InvoiceLine[i].Item.AdditionalItemProperty[0].Value.Value);
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

						// valida que el detalle contenga el tag TaxTotal
						if (factura_ubl.InvoiceLine[i].TaxTotal != null)
						{
							for (int j = 0; j < factura_ubl.InvoiceLine[i].TaxTotal[0].TaxSubtotal.Count(); j++)
							{
								string tipo_impto = factura_ubl.InvoiceLine[i].TaxTotal[0].TaxSubtotal[j].TaxCategory.TaxScheme.ID.Value;
								decimal porcentaje_impto = factura_ubl.InvoiceLine[i].TaxTotal[0].TaxSubtotal[j].Percent.Value;
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
				}

				#region Totales
				factura_obj.ValorSubtotal = factura_ubl.LegalMonetaryTotal.LineExtensionAmount.Value;
				if (factura_ubl.LegalMonetaryTotal.AllowanceTotalAmount != null)
					factura_obj.ValorDescuento = factura_ubl.LegalMonetaryTotal.AllowanceTotalAmount.Value;
				factura_obj.Valor = factura_obj.ValorSubtotal + factura_obj.ValorDescuento;
				factura_obj.ValorIva = factura_ubl.LegalMonetaryTotal.TaxExclusiveAmount.Value;
				factura_obj.Total = factura_ubl.LegalMonetaryTotal.PayableAmount.Value;
				factura_obj.Neto = (factura_obj.Total - (factura_obj.ValorReteFuente + factura_obj.ValorReteIca + factura_obj.ValorReteIva));

				#endregion

				return factura_obj;
			}
			catch (Exception ex)
			{
				LogExcepcion.Guardar(ex);
				throw new ApplicationException(ex.Message, ex.InnerException);
			}
		}
	}
}
