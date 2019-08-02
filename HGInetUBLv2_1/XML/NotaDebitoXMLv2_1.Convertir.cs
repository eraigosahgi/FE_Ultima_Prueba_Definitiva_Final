using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetUBLv2_1.Dian;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HGInetUBLv2_1
{
	public partial class NotaDebitoXMLv2_1
	{

		/// <summary>
		/// Convierte el XML-UBL2.1 en un objeto de Servicio
		/// </summary>
		/// <param name="nota_debito_ubl">Archivo XML-UBL</param>
		/// <param name="interopeabilidad">Indica si la conversión del archivo XML-UBL es sólo de encabezados</param>
		/// <returns>objeto tipo NotaDebito</returns>
		public static NotaDebito Convertir(DebitNoteType nota_debito_ubl, TblDocumentos documento_bd, bool interopeabilidad = false)
		{

			try
			{
				NotaDebito nota_debito_obj = new NotaDebito();

				nota_debito_obj.Prefijo = documento_bd.StrPrefijo;

				//captura el numero del documento y valida proceso de interoperabilidad
				if (interopeabilidad)
				{
					Match numero_doc = Regex.Match(nota_debito_ubl.ID.Value, "\\d+");

					nota_debito_obj.Documento = Convert.ToInt64(numero_doc.Value);
				}
				else
				{
					if (string.IsNullOrEmpty(nota_debito_obj.Prefijo))
					{
						nota_debito_obj.Documento = Convert.ToInt64(nota_debito_ubl.ID.Value);
					}
					else
					{
						string documento = nota_debito_ubl.ID.Value;
						if (documento.Substring(0, nota_debito_obj.Prefijo.Length).Equals(nota_debito_obj.Prefijo))
						{
							nota_debito_obj.Documento = Convert.ToInt64(documento.Substring(nota_debito_obj.Prefijo.Length));
						}
					}
				}
				//Capturo la informacion del encabezado del documento
				if (nota_debito_ubl.UUID != null)
					nota_debito_obj.Cufe = nota_debito_ubl.UUID.Value;
				string fecha_hora = string.Format("{0}{1}", nota_debito_ubl.IssueDate.Value.ToString(Fecha.formato_fecha_hginet), nota_debito_ubl.IssueTime.Value);
				nota_debito_obj.Fecha = Convert.ToDateTime(fecha_hora);
				nota_debito_obj.Moneda = nota_debito_ubl.DocumentCurrencyCode.Value;

				nota_debito_obj.Concepto = string.Empty;
				// valida el concepto
				if (nota_debito_ubl.DiscrepancyResponse != null)
				{
					if (nota_debito_ubl.DiscrepancyResponse.FirstOrDefault().ResponseCode != null && nota_debito_ubl.DiscrepancyResponse.FirstOrDefault().ResponseCode.Value != null)
					{
						nota_debito_obj.Concepto = nota_debito_ubl.DiscrepancyResponse.FirstOrDefault().ResponseCode.Value;
					}
				}

				nota_debito_obj.DocumentoRef = string.Empty;
				// valida el documento de referencia factura
				if (nota_debito_ubl.BillingReference != null)
				{
					if (nota_debito_ubl.BillingReference.FirstOrDefault().InvoiceDocumentReference != null && nota_debito_ubl.BillingReference.FirstOrDefault().InvoiceDocumentReference.ID != null && nota_debito_ubl.BillingReference.FirstOrDefault().InvoiceDocumentReference.ID.Value != null)
					{
						nota_debito_obj.DocumentoRef = nota_debito_ubl.BillingReference.FirstOrDefault().InvoiceDocumentReference.ID.Value;
						nota_debito_obj.CufeFactura = nota_debito_ubl.BillingReference.FirstOrDefault().InvoiceDocumentReference.UUID.Value;
						nota_debito_obj.FechaFactura = nota_debito_ubl.BillingReference.FirstOrDefault().InvoiceDocumentReference.IssueDate.Value;
					}
				}

				nota_debito_obj.PedidoRef = string.Empty;
				// valida el documento de referencia pedido
				if (nota_debito_ubl.OrderReference != null)
				{
					if (nota_debito_ubl.OrderReference.ID != null && nota_debito_ubl.OrderReference.ID.Value != null)
					{
						nota_debito_obj.PedidoRef = nota_debito_ubl.OrderReference.ID.Value;
					}
				}


				if (!interopeabilidad)
				{
					//valida las notas para llenar los campos del pdf
					Formato documento_formato = new Formato();
					List<FormatoCampo> lista_campos = new List<FormatoCampo>();

					try
					{

						if (nota_debito_ubl.Note[0].Value != null)
						{
							//Deserializa la posición 1 y las convierte en FormatoCampo
							dynamic jsonObj = JsonConvert.DeserializeObject(nota_debito_ubl.Note[0].Value);

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
						nota_debito_obj.Notas.Add(nota_debito_ubl.Note[0].Value);
					}

					documento_formato.CamposPredeterminados = lista_campos;

					nota_debito_obj.DocumentoFormato = documento_formato;
					if (nota_debito_ubl.Note.Count() > 1 && (nota_debito_ubl.Note[1].Value != null))
						nota_debito_obj.Nota = nota_debito_ubl.Note[1].Value;
				}

				#region Datos del Adquiriente

				nota_debito_obj.DatosAdquiriente = TerceroXMLv2_1.Obtener_adquiriente(nota_debito_ubl.AccountingCustomerParty);

				#endregion

				#region Datos del Obligado

				nota_debito_obj.DatosObligado = TerceroXMLv2_1.Obtener_obligado(nota_debito_ubl.AccountingSupplierParty);

				#endregion

				#region Detalle de Documento

				List<DocumentoDetalle> list_detalle = new List<DocumentoDetalle>();

				if (!interopeabilidad)
				{
					//Recorre todo el detalle del documento
					for (int i = 0; i < nota_debito_ubl.DebitNoteLine.Length; i++)
					{

						DocumentoDetalle detalle = new DocumentoDetalle();
						detalle.Codigo = Convert.ToInt16(nota_debito_ubl.DebitNoteLine[i].ID.Value);
						if (nota_debito_ubl.DebitNoteLine[i].Item.CatalogueItemIdentification != null)
							detalle.ProductoCodigo = nota_debito_ubl.DebitNoteLine[i].Item.CatalogueItemIdentification.ID.Value;
						if (nota_debito_ubl.DebitNoteLine[i].Item.StandardItemIdentification != null)
							detalle.ProductoCodigoEAN = nota_debito_ubl.DebitNoteLine[i].Item.StandardItemIdentification.ID.Value;
						detalle.ProductoNombre = nota_debito_ubl.DebitNoteLine[i].Item.Description[0].Value;
						if (nota_debito_ubl.DebitNoteLine[i].Item.AdditionalInformation != null)
						{
							detalle.ProductoDescripcion = nota_debito_ubl.DebitNoteLine[i].Item.AdditionalInformation.FirstOrDefault().Value;
						}
						else
						{
							detalle.ProductoDescripcion = string.Empty;
						}
						detalle.Cantidad = nota_debito_ubl.DebitNoteLine[i].DebitedQuantity.Value;
						if (!string.IsNullOrEmpty(nota_debito_ubl.DebitNoteLine[i].DebitedQuantity.unitCode.ToString()))
						{
							detalle.UnidadCodigo = nota_debito_ubl.DebitNoteLine[i].DebitedQuantity.unitCode.ToString();
						}
						else
						{
							detalle.UnidadCodigo = "94";
						}
						if (nota_debito_ubl.DebitNoteLine[i].Item.OriginAddress != null)
						{
							detalle.Bodega = nota_debito_ubl.DebitNoteLine[i].Item.OriginAddress[0].ID.Value;
						}
						else
						{
							detalle.Bodega = string.Empty;
						}

						detalle.ValorUnitario = nota_debito_ubl.DebitNoteLine[i].Price.PriceAmount.Value;
						detalle.ValorSubtotal = nota_debito_ubl.DebitNoteLine[i].LineExtensionAmount.Value;

						if (nota_debito_ubl.DebitNoteLine[i].Item.AdditionalItemProperty != null)
						{
							detalle.OcultarItem = Convert.ToInt16(nota_debito_ubl.DebitNoteLine[i].Item.AdditionalItemProperty[0].Value.Value);
						}

						if (nota_debito_ubl.DebitNoteLine[i].AllowanceCharge != null)
						{
							detalle.DescuentoPorcentaje = nota_debito_ubl.DebitNoteLine[i].AllowanceCharge.FirstOrDefault().MultiplierFactorNumeric.Value;
							detalle.DescuentoValor = nota_debito_ubl.DebitNoteLine[i].AllowanceCharge.FirstOrDefault().Amount.Value;
						}
						else
						{
							detalle.DescuentoPorcentaje = 0.00M;
							detalle.DescuentoValor = 0.00M;
						}

						// valida que el detalle contenga el tag TaxTotal
						if (nota_debito_ubl.DebitNoteLine[i].TaxTotal != null)
						{
							for (int j = 0; j < nota_debito_ubl.DebitNoteLine[i].TaxTotal[0].TaxSubtotal.Count(); j++)
							{
								string tipo_impto = nota_debito_ubl.DebitNoteLine[i].TaxTotal[0].TaxSubtotal[j].TaxCategory.TaxScheme.ID.Value;
								decimal porcentaje_impto = nota_debito_ubl.DebitNoteLine[i].TaxTotal[0].TaxSubtotal[j].Percent.Value;
								decimal valor_impto = nota_debito_ubl.DebitNoteLine[i].TaxTotal[0].TaxSubtotal[j].TaxAmount.Value;

								if (TipoImpuestos.Iva.Equals(tipo_impto))
								{
									detalle.IvaPorcentaje = porcentaje_impto;
									detalle.IvaValor = valor_impto;
								}
								else if (TipoImpuestos.Consumo.Equals(tipo_impto))
								{
									detalle.ImpoConsumoPorcentaje = porcentaje_impto;
									detalle.ValorImpuestoConsumo = valor_impto;
									nota_debito_obj.ValorImpuestoConsumo += detalle.ValorImpuestoConsumo;
								}
								else if (TipoImpuestos.Ica.Equals(tipo_impto))
								{
									detalle.ReteIcaPorcentaje = porcentaje_impto;
									detalle.ReteIcaValor = valor_impto;
									nota_debito_obj.ValorReteIca += detalle.ReteIcaValor;
								}
								else if (TipoImpuestos.ReteFte.Equals(tipo_impto))
								{
									detalle.ReteFuentePorcentaje = porcentaje_impto;
									detalle.ReteFuenteValor = valor_impto;
									nota_debito_obj.ValorReteFuente += detalle.ReteFuenteValor;
								}

							}
						}

						list_detalle.Add(detalle);

					}

				}
				nota_debito_obj.DocumentoDetalles = list_detalle;
				#endregion

				#region Totales
				nota_debito_obj.ValorSubtotal = nota_debito_ubl.RequestedMonetaryTotal.LineExtensionAmount.Value;
				nota_debito_obj.Valor = nota_debito_obj.ValorSubtotal + nota_debito_obj.ValorDescuento;
				if (nota_debito_ubl.RequestedMonetaryTotal.AllowanceTotalAmount != null)
					nota_debito_obj.ValorDescuento = nota_debito_ubl.RequestedMonetaryTotal.AllowanceTotalAmount.Value;
				if (nota_debito_ubl.RequestedMonetaryTotal.ChargeTotalAmount != null)
					nota_debito_obj.ValorCargo = nota_debito_ubl.RequestedMonetaryTotal.ChargeTotalAmount.Value;
				if (nota_debito_ubl.RequestedMonetaryTotal.PrepaidAmount != null)
					nota_debito_obj.ValorAnticipo = nota_debito_ubl.RequestedMonetaryTotal.PrepaidAmount.Value;
				nota_debito_obj.ValorIva = nota_debito_ubl.RequestedMonetaryTotal.TaxExclusiveAmount.Value - nota_debito_ubl.RequestedMonetaryTotal.LineExtensionAmount.Value;
				nota_debito_obj.Total = nota_debito_ubl.RequestedMonetaryTotal.PayableAmount.Value;

				//Se agrega validacion si ya se habia guardado el neto en Bd para utilizarlo o si no que sea el calculado
				if (documento_bd != null)
				{
					if (documento_bd.IntValorNeto == 0)
					{
						nota_debito_obj.Neto = (nota_debito_obj.Total - (nota_debito_obj.ValorReteFuente + nota_debito_obj.ValorReteIca + nota_debito_obj.ValorReteIva));
					}
					else
					{
						nota_debito_obj.Neto = documento_bd.IntValorNeto;
					}
				}
				else
				{
					nota_debito_obj.Neto = (nota_debito_obj.Total - (nota_debito_obj.ValorReteFuente + nota_debito_obj.ValorReteIca + nota_debito_obj.ValorReteIva));
				}
				#endregion


				return nota_debito_obj;
			}
			catch (Exception ex)
			{
				LogExcepcion.Guardar(ex);
				throw new ApplicationException(ex.Message, ex.InnerException);
			}
		}



	}
}
