using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetUBL.Recursos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBL
{
    public partial class NotaDebitoXML
    {

        /// <summary>
        /// Convierte el XML-UBL en un objeto de Servicio
        /// </summary>
        /// <param name="nota_debito_ubl">Archivo XML-UBL</param>
        /// <returns>objeto tipo NotaDebito</returns>
        public static NotaDebito Convertir(DebitNoteType nota_debito_ubl)
        {

            NotaDebito nota_debito_obj = new NotaDebito();

            //Valida el prefijo de la nota credito y captura el numero del documento
            nota_debito_obj.Prefijo = string.Empty;

            if (nota_debito_ubl.CustomizationID == null)
            {
                nota_debito_obj.Documento = Convert.ToInt64(nota_debito_ubl.ID.Value);
            }
            else
            {
                if (nota_debito_ubl.CustomizationID.Value != null)
                {
                    nota_debito_obj.Prefijo = nota_debito_ubl.CustomizationID.Value;
                    string documento = nota_debito_ubl.ID.Value;
                    if (documento.Substring(0, nota_debito_obj.Prefijo.Length).Equals(nota_debito_obj.Prefijo))
                    {
                        nota_debito_obj.Documento = Convert.ToInt64(documento.Substring(nota_debito_obj.Prefijo.Length));
                    }
                }
                else
                {
                    nota_debito_obj.Documento = Convert.ToInt64(nota_debito_ubl.ID.Value);
                }
            }
            //Capturo la informacion del encabezado del documento
            nota_debito_obj.Cufe = nota_debito_ubl.UUID.Value;
            nota_debito_obj.Fecha = nota_debito_ubl.IssueDate.Value;
            nota_debito_obj.Moneda = nota_debito_ubl.DocumentCurrencyCode.Value;
            nota_debito_obj.Concepto = nota_debito_ubl.DiscrepancyResponse.FirstOrDefault().ResponseCode.Value;
            nota_debito_obj.DocumentoRef = nota_debito_ubl.BillingReference.FirstOrDefault().InvoiceDocumentReference.ID.Value;
            nota_debito_obj.CufeFactura = nota_debito_ubl.BillingReference.FirstOrDefault().InvoiceDocumentReference.UUID.Value;
            nota_debito_obj.FechaFactura = nota_debito_ubl.BillingReference.FirstOrDefault().InvoiceDocumentReference.IssueDate.Value;

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

            if ((nota_debito_ubl.Note.Count() > 1) && (nota_debito_ubl.Note[1].Value != null))
                nota_debito_obj.Nota = nota_debito_ubl.Note[1].Value;

            #region Datos del Adquiriente
            Tercero adquiriente = new Tercero();

            adquiriente.Identificacion = nota_debito_ubl.AccountingCustomerParty.Party.PartyIdentification[0].ID.Value;
            adquiriente.TipoPersona = Convert.ToInt16(nota_debito_ubl.AccountingCustomerParty.AdditionalAccountID.Value);
            adquiriente.Regimen = Convert.ToInt16(nota_debito_ubl.AccountingCustomerParty.Party.PartyTaxScheme[0].TaxLevelCode.Value);
            adquiriente.TipoIdentificacion = Convert.ToInt16(nota_debito_ubl.AccountingCustomerParty.Party.PartyIdentification[0].ID.schemeID);
            //Valida si es persona Natural 
            if (nota_debito_ubl.AccountingCustomerParty.Party.Person != null)
            {
                adquiriente.PrimerNombre = nota_debito_ubl.AccountingCustomerParty.Party.Person.FirstName.Value;
                adquiriente.SegundoNombre = nota_debito_ubl.AccountingCustomerParty.Party.Person.MiddleName.Value;
                adquiriente.PrimerApellido = nota_debito_ubl.AccountingCustomerParty.Party.Person.FamilyName.Value;
                adquiriente.RazonSocial = string.Format("{0} {1} {2}", adquiriente.PrimerApellido, adquiriente.PrimerNombre, adquiriente.SegundoNombre);
            }
            else
            {
                adquiriente.RazonSocial = nota_debito_ubl.AccountingCustomerParty.Party.PartyLegalEntity[0].RegistrationName.Value;
            }
            adquiriente.Direccion = nota_debito_ubl.AccountingCustomerParty.Party.PhysicalLocation.Address.AddressLine[0].Line.Value;
            adquiriente.Ciudad = nota_debito_ubl.AccountingCustomerParty.Party.PhysicalLocation.Address.CityName.Value;
            adquiriente.Departamento = nota_debito_ubl.AccountingCustomerParty.Party.PhysicalLocation.Address.Department.Value;
            adquiriente.CodigoPais = nota_debito_ubl.AccountingCustomerParty.Party.PhysicalLocation.Address.Country.IdentificationCode.Value;
            adquiriente.Telefono = nota_debito_ubl.AccountingCustomerParty.Party.Contact.Telephone.Value;
            adquiriente.Email = nota_debito_ubl.AccountingCustomerParty.Party.Contact.ElectronicMail.Value;
            //Valida si tiene pagina web
            if (nota_debito_ubl.AccountingCustomerParty.Party.WebsiteURI.Value != null)
                adquiriente.PaginaWeb = nota_debito_ubl.AccountingCustomerParty.Party.WebsiteURI.Value;
            nota_debito_obj.DatosAdquiriente = adquiriente;
            #endregion

            #region Datos del Obligado
            Tercero obligado = new Tercero();

            obligado.Identificacion = nota_debito_ubl.AccountingSupplierParty.Party.PartyIdentification[0].ID.Value;
            obligado.TipoPersona = Convert.ToInt16(nota_debito_ubl.AccountingSupplierParty.AdditionalAccountID.Value);
            obligado.Regimen = Convert.ToInt16(nota_debito_ubl.AccountingSupplierParty.Party.PartyTaxScheme[0].TaxLevelCode.Value);
            obligado.TipoIdentificacion = Convert.ToInt16(nota_debito_ubl.AccountingSupplierParty.Party.PartyIdentification[0].ID.schemeID);
            //Valida si es persona Natural
            if (nota_debito_ubl.AccountingSupplierParty.Party.Person != null)
            {
                obligado.PrimerNombre = nota_debito_ubl.AccountingSupplierParty.Party.Person.FirstName.Value;
                obligado.SegundoNombre = nota_debito_ubl.AccountingSupplierParty.Party.Person.MiddleName.Value;
                obligado.PrimerApellido = nota_debito_ubl.AccountingSupplierParty.Party.Person.FamilyName.Value;
                obligado.RazonSocial = string.Format("{0} {1} {2}", obligado.PrimerApellido, obligado.PrimerNombre, obligado.SegundoNombre);
            }
            else
            {
                obligado.RazonSocial = nota_debito_ubl.AccountingSupplierParty.Party.PartyLegalEntity[0].RegistrationName.Value;
            }
            obligado.Direccion = nota_debito_ubl.AccountingSupplierParty.Party.PhysicalLocation.Address.AddressLine[0].Line.Value;
            obligado.Ciudad = nota_debito_ubl.AccountingSupplierParty.Party.PhysicalLocation.Address.CityName.Value;
            obligado.Departamento = nota_debito_ubl.AccountingSupplierParty.Party.PhysicalLocation.Address.Department.Value;
            obligado.CodigoPais = nota_debito_ubl.AccountingSupplierParty.Party.PhysicalLocation.Address.Country.IdentificationCode.Value;
            obligado.Telefono = nota_debito_ubl.AccountingSupplierParty.Party.Contact.Telephone.Value;
            obligado.Email = nota_debito_ubl.AccountingSupplierParty.Party.Contact.ElectronicMail.Value;
            //Valida si tiene pagina web
            if (nota_debito_ubl.AccountingSupplierParty.Party.WebsiteURI.Value != null)
                obligado.PaginaWeb = nota_debito_ubl.AccountingSupplierParty.Party.WebsiteURI.Value;
            nota_debito_obj.DatosObligado = obligado;

            #endregion

            #region Detalle de Documento
            List<DocumentoDetalle> list_detalle = new List<DocumentoDetalle>();

            //Recorre todo el detalle del documento
            for (int i = 0; i < nota_debito_ubl.DebitNoteLine.Length; i++)
            {

                DocumentoDetalle detalle = new DocumentoDetalle();
                detalle.Codigo = Convert.ToInt16(nota_debito_ubl.DebitNoteLine[i].ID.Value);
                detalle.ProductoCodigo = nota_debito_ubl.DebitNoteLine[i].Item.CatalogueItemIdentification.ID.Value;
                detalle.ProductoNombre = nota_debito_ubl.DebitNoteLine[i].Item.Description[0].Value;
                if (nota_debito_ubl.DebitNoteLine[i].Item.AdditionalInformation != null)
                {
                    detalle.ProductoDescripcion = nota_debito_ubl.DebitNoteLine[i].Item.AdditionalInformation.Value;
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
                    detalle.UnidadCodigo = "S7";
                }
                detalle.ValorUnitario = nota_debito_ubl.DebitNoteLine[i].Price.PriceAmount.Value;
                detalle.ValorSubtotal = nota_debito_ubl.DebitNoteLine[i].LineExtensionAmount.Value;

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
            nota_debito_obj.DocumentoDetalles = list_detalle;
            #endregion

            #region Totales
            nota_debito_obj.ValorSubtotal = nota_debito_ubl.LegalMonetaryTotal.LineExtensionAmount.Value;
            nota_debito_obj.ValorDescuento = nota_debito_ubl.LegalMonetaryTotal.AllowanceTotalAmount.Value;
            nota_debito_obj.Valor = nota_debito_obj.ValorSubtotal + nota_debito_obj.ValorDescuento;
            nota_debito_obj.ValorIva = nota_debito_ubl.LegalMonetaryTotal.TaxExclusiveAmount.Value;
            nota_debito_obj.Total = nota_debito_ubl.LegalMonetaryTotal.PayableAmount.Value;
            nota_debito_obj.Neto = (nota_debito_obj.Total - (nota_debito_obj.ValorReteFuente + nota_debito_obj.ValorReteIca + nota_debito_obj.ValorReteIva));

            #endregion


            return nota_debito_obj;
        }


    }
}
