using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetUBL.Recursos;
using LibreriaGlobalHGInet.General;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBL
{
	public partial class NotaCreditoXML
	{

		/// <summary>
		/// Convierte el XML-UBL en un objeto de Servicio
		/// </summary>
		/// <param name="nota_credito_ubl">Archivo XML-UBL</param>
		/// <param name="interopeabilidad">Indica si la conversión del archivo XML-UBL es sólo de encabezados</param>
		/// <returns>objeto tipo NotaCredito</returns>
		public static NotaCredito Convertir(CreditNoteType nota_credito_ubl, bool interopeabilidad = false)
		{

			try
			{
				NotaCredito nota_credito_obj = new NotaCredito();

				//Valida el prefijo de la nota credito y captura el numero del documento
				nota_credito_obj.Prefijo = string.Empty;

				if (nota_credito_ubl.CustomizationID == null)
				{
					nota_credito_obj.Documento = Convert.ToInt64(nota_credito_ubl.ID.Value);
				}
				else
				{
					if (nota_credito_ubl.CustomizationID.Value != null)
					{
						nota_credito_obj.Prefijo = nota_credito_ubl.CustomizationID.Value;
						string documento = nota_credito_ubl.ID.Value;
						if (documento.Substring(0, nota_credito_obj.Prefijo.Length).Equals(nota_credito_obj.Prefijo))
						{
							nota_credito_obj.Documento = Convert.ToInt64(documento.Substring(nota_credito_obj.Prefijo.Length));
						}
					}
					else
					{
						nota_credito_obj.Documento = Convert.ToInt64(nota_credito_ubl.ID.Value);
					}
				}
				//Capturo la informacion del encabezado del documento
				nota_credito_obj.Cufe = nota_credito_ubl.UUID.Value;
				nota_credito_obj.Fecha = nota_credito_ubl.IssueDate.Value;
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
					}
				}


				nota_credito_obj.PedidoRef = string.Empty;
				// valida el documento de referencia pedido
				if (nota_credito_ubl.OrderReference != null)
				{
					if (nota_credito_ubl.OrderReference.FirstOrDefault().DocumentReference != null && nota_credito_ubl.OrderReference.FirstOrDefault().DocumentReference.ID != null && nota_credito_ubl.OrderReference.FirstOrDefault().DocumentReference.ID.Value != null)
					{
						nota_credito_obj.DocumentoRef = nota_credito_ubl.OrderReference.FirstOrDefault().DocumentReference.ID.Value;
					}
				}
				
				nota_credito_obj.CufeFactura = nota_credito_ubl.BillingReference.FirstOrDefault().InvoiceDocumentReference.UUID.Value;
				nota_credito_obj.FechaFactura = nota_credito_ubl.BillingReference.FirstOrDefault().InvoiceDocumentReference.IssueDate.Value;

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
				Tercero adquiriente = new Tercero();

				adquiriente.Identificacion = nota_credito_ubl.AccountingCustomerParty.Party.PartyIdentification[0].ID.Value;
				adquiriente.TipoPersona = Convert.ToInt16(nota_credito_ubl.AccountingCustomerParty.AdditionalAccountID.Value);
				adquiriente.Regimen = Convert.ToInt16(nota_credito_ubl.AccountingCustomerParty.Party.PartyTaxScheme[0].TaxLevelCode.Value);
				adquiriente.TipoIdentificacion = Convert.ToInt16(nota_credito_ubl.AccountingCustomerParty.Party.PartyIdentification[0].ID.schemeID);
				//Valida si es persona Natural 
				if (nota_credito_ubl.AccountingCustomerParty.Party.Person != null)
				{
					adquiriente.PrimerNombre = nota_credito_ubl.AccountingCustomerParty.Party.Person.FirstName.Value;
					adquiriente.SegundoNombre = nota_credito_ubl.AccountingCustomerParty.Party.Person.MiddleName.Value;
					adquiriente.PrimerApellido = nota_credito_ubl.AccountingCustomerParty.Party.Person.FamilyName.Value;
					adquiriente.RazonSocial = string.Format("{0} {1} {2}", adquiriente.PrimerApellido, adquiriente.PrimerNombre, adquiriente.SegundoNombre);
				}
				else
				{
					adquiriente.RazonSocial = nota_credito_ubl.AccountingCustomerParty.Party.PartyLegalEntity[0].RegistrationName.Value;
				}
				adquiriente.Direccion = nota_credito_ubl.AccountingCustomerParty.Party.PhysicalLocation.Address.AddressLine[0].Line.Value;
				adquiriente.Ciudad = nota_credito_ubl.AccountingCustomerParty.Party.PhysicalLocation.Address.CityName.Value;
				adquiriente.Departamento = nota_credito_ubl.AccountingCustomerParty.Party.PhysicalLocation.Address.Department.Value;
				adquiriente.CodigoPais = nota_credito_ubl.AccountingCustomerParty.Party.PhysicalLocation.Address.Country.IdentificationCode.Value;
				adquiriente.Telefono = nota_credito_ubl.AccountingCustomerParty.Party.Contact.Telephone.Value;
				adquiriente.Email = nota_credito_ubl.AccountingCustomerParty.Party.Contact.ElectronicMail.Value;
				//Valida si tiene pagina web
				if (nota_credito_ubl.AccountingCustomerParty.Party.WebsiteURI != null)
					adquiriente.PaginaWeb = nota_credito_ubl.AccountingCustomerParty.Party.WebsiteURI.Value;
				nota_credito_obj.DatosAdquiriente = adquiriente;
				#endregion

				#region Datos del Obligado
				Tercero obligado = new Tercero();

				obligado.Identificacion = nota_credito_ubl.AccountingSupplierParty.Party.PartyIdentification[0].ID.Value;
				obligado.TipoPersona = Convert.ToInt16(nota_credito_ubl.AccountingSupplierParty.AdditionalAccountID.Value);
				obligado.Regimen = Convert.ToInt16(nota_credito_ubl.AccountingSupplierParty.Party.PartyTaxScheme[0].TaxLevelCode.Value);
				obligado.TipoIdentificacion = Convert.ToInt16(nota_credito_ubl.AccountingSupplierParty.Party.PartyIdentification[0].ID.schemeID);
				//Valida si es persona Natural
				if (nota_credito_ubl.AccountingSupplierParty.Party.Person != null)
				{
					obligado.PrimerNombre = nota_credito_ubl.AccountingSupplierParty.Party.Person.FirstName.Value;
					obligado.SegundoNombre = nota_credito_ubl.AccountingSupplierParty.Party.Person.MiddleName.Value;
					obligado.PrimerApellido = nota_credito_ubl.AccountingSupplierParty.Party.Person.FamilyName.Value;
					obligado.RazonSocial = string.Format("{0} {1} {2}", obligado.PrimerApellido, obligado.PrimerNombre, obligado.SegundoNombre);
				}
				else
				{
					obligado.RazonSocial = nota_credito_ubl.AccountingSupplierParty.Party.PartyLegalEntity[0].RegistrationName.Value;
				}
				obligado.Direccion = nota_credito_ubl.AccountingSupplierParty.Party.PhysicalLocation.Address.AddressLine[0].Line.Value;
				obligado.Ciudad = nota_credito_ubl.AccountingSupplierParty.Party.PhysicalLocation.Address.CityName.Value;
				obligado.Departamento = nota_credito_ubl.AccountingSupplierParty.Party.PhysicalLocation.Address.Department.Value;
				obligado.CodigoPais = nota_credito_ubl.AccountingSupplierParty.Party.PhysicalLocation.Address.Country.IdentificationCode.Value;
				obligado.Telefono = nota_credito_ubl.AccountingSupplierParty.Party.Contact.Telephone.Value;
				obligado.Email = nota_credito_ubl.AccountingSupplierParty.Party.Contact.ElectronicMail.Value;
				//Valida si tiene pagina web
				if (nota_credito_ubl.AccountingSupplierParty.Party.WebsiteURI != null)
					obligado.PaginaWeb = nota_credito_ubl.AccountingSupplierParty.Party.WebsiteURI.Value;
				nota_credito_obj.DatosObligado = obligado;

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
						detalle.ProductoCodigo = nota_credito_ubl.CreditNoteLine[i].Item.CatalogueItemIdentification.ID.Value;
						detalle.ProductoNombre = nota_credito_ubl.CreditNoteLine[i].Item.Description[0].Value;
						if (nota_credito_ubl.CreditNoteLine[i].Item.AdditionalInformation != null)
						{
							detalle.ProductoDescripcion = nota_credito_ubl.CreditNoteLine[i].Item.AdditionalInformation.Value;
						}
						else
						{
							detalle.ProductoDescripcion = string.Empty;
						}
						detalle.Cantidad = nota_credito_ubl.CreditNoteLine[i].CreditedQuantity.Value;
						if (!string.IsNullOrEmpty(nota_credito_ubl.CreditNoteLine[i].CreditedQuantity.unitCode.ToString()))
						{
							detalle.UnidadCodigo = nota_credito_ubl.CreditNoteLine[i].CreditedQuantity.unitCode.ToString();
						}
						else
						{
							detalle.UnidadCodigo = "S7";
						}
						detalle.ValorUnitario = nota_credito_ubl.CreditNoteLine[i].Price.PriceAmount.Value;
						detalle.ValorSubtotal = nota_credito_ubl.CreditNoteLine[i].LineExtensionAmount.Value;

						// valida que el detalle contenga el tag TaxTotal
						if (nota_credito_ubl.CreditNoteLine[i].TaxTotal != null)
						{
							for (int j = 0; j < nota_credito_ubl.CreditNoteLine[i].TaxTotal[0].TaxSubtotal.Count(); j++)
							{
								string tipo_impto = nota_credito_ubl.CreditNoteLine[i].TaxTotal[0].TaxSubtotal[j].TaxCategory.TaxScheme.ID.Value;
								decimal porcentaje_impto = nota_credito_ubl.CreditNoteLine[i].TaxTotal[0].TaxSubtotal[j].Percent.Value;
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

							}
						}

						list_detalle.Add(detalle);

					}

				}
				nota_credito_obj.DocumentoDetalles = list_detalle;
				#endregion

				#region Totales
				nota_credito_obj.ValorSubtotal = nota_credito_ubl.LegalMonetaryTotal.LineExtensionAmount.Value;
				nota_credito_obj.ValorDescuento = nota_credito_ubl.LegalMonetaryTotal.AllowanceTotalAmount.Value;
				nota_credito_obj.Valor = nota_credito_obj.ValorSubtotal + nota_credito_obj.ValorDescuento;
				nota_credito_obj.ValorIva = nota_credito_ubl.LegalMonetaryTotal.TaxExclusiveAmount.Value;
				nota_credito_obj.Total = nota_credito_ubl.LegalMonetaryTotal.PayableAmount.Value;
				nota_credito_obj.Neto = (nota_credito_obj.Total - (nota_credito_obj.ValorReteFuente + nota_credito_obj.ValorReteIca + nota_credito_obj.ValorReteIva));

				#endregion


				return nota_credito_obj;
			}
			catch (Exception ex)
			{
				LogExcepcion.Guardar(ex);
				throw new ApplicationException(ex.Message, ex.InnerException);
			}
		}
	}
}
