using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetUBL.Objetos;
using HGInetUBL.Recursos;
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

        public static Factura Convertir(InvoiceType factura_ubl)
        {
            Factura factura_obj = new Factura();

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

            if (!string.IsNullOrEmpty(factura_obj.Prefijo))
            {
                string documento = factura_ubl.ID.Value;
                if (documento.Substring(0, 4).Equals(factura_obj.Prefijo))
                {
                    factura_obj.Documento = Convert.ToInt32(documento.Substring(4));
                }
            }
            else
            {
                factura_obj.Documento = Convert.ToInt32(factura_ubl.ID.Value);
            }

            if (factura_ubl.BillingReference != null)
            {
                factura_obj.DocumentoRef = factura_ubl.BillingReference.FirstOrDefault().InvoiceDocumentReference.ID.Value;
            }
            else
            {
                factura_obj.DocumentoRef = string.Empty;
            }
            factura_obj.Cufe = factura_ubl.UUID.Value;
            factura_obj.Fecha = factura_ubl.IssueDate.Value;
            if (factura_ubl.PaymentMeans != null)
            {
                factura_obj.FechaVence = factura_ubl.PaymentMeans.FirstOrDefault().PaymentDueDate.Value;
            }
            factura_obj.Moneda = factura_ubl.DocumentCurrencyCode.Value;
            factura_obj.Nota = factura_ubl.Note[0].Value;

            Formato documento_formato = new Formato();
            List<FormatoCampo> lista_campos = new List<FormatoCampo>();

            try
            {
                //Deserializa la posición 1 y las convierte en FormatoCampo
                dynamic jsonObj = JsonConvert.DeserializeObject(factura_ubl.Note[1].Value);

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
            catch (Exception)
            {
            }

            documento_formato.CamposPredeterminados = lista_campos;

            factura_obj.DocumentoFormato = documento_formato;

            #region Adquiriente
            Tercero adquiriente = new Tercero();

            adquiriente.Identificacion = factura_ubl.AccountingCustomerParty.Party.PartyIdentification[0].ID.Value;
            adquiriente.TipoPersona = Convert.ToInt16(factura_ubl.AccountingCustomerParty.AdditionalAccountID.Value);
            adquiriente.Regimen = Convert.ToInt16(factura_ubl.AccountingCustomerParty.Party.PartyTaxScheme[0].TaxLevelCode.Value);
            adquiriente.TipoIdentificacion = Convert.ToInt16(factura_ubl.AccountingCustomerParty.Party.PartyIdentification[0].ID.schemeID);
            if (factura_ubl.AccountingCustomerParty.Party.Person != null)
            {
                adquiriente.PrimerNombre = factura_ubl.AccountingCustomerParty.Party.Person.FirstName.Value;
                adquiriente.SegundoNombre = factura_ubl.AccountingCustomerParty.Party.Person.MiddleName.Value;
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
            adquiriente.Telefono = factura_ubl.AccountingCustomerParty.Party.Contact.Telephone.Value;
            adquiriente.Email = factura_ubl.AccountingCustomerParty.Party.Contact.ElectronicMail.Value;
            if (factura_ubl.AccountingCustomerParty.Party.WebsiteURI != null)
                adquiriente.PaginaWeb = factura_ubl.AccountingCustomerParty.Party.WebsiteURI.Value;
            factura_obj.DatosAdquiriente = adquiriente;
            #endregion

            #region Obligado
            Tercero obligado = new Tercero();

            obligado.Identificacion = factura_ubl.AccountingSupplierParty.Party.PartyIdentification[0].ID.Value;
            obligado.TipoPersona = Convert.ToInt16(factura_ubl.AccountingSupplierParty.AdditionalAccountID.Value);
            obligado.Regimen = Convert.ToInt16(factura_ubl.AccountingSupplierParty.Party.PartyTaxScheme[0].TaxLevelCode.Value);
            obligado.TipoIdentificacion = Convert.ToInt16(factura_ubl.AccountingSupplierParty.Party.PartyIdentification[0].ID.schemeID);
            if (factura_ubl.AccountingSupplierParty.Party.Person != null)
            {
                obligado.PrimerNombre = factura_ubl.AccountingSupplierParty.Party.Person.FirstName.Value;
                obligado.SegundoNombre = factura_ubl.AccountingSupplierParty.Party.Person.MiddleName.Value;
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
            obligado.Telefono = factura_ubl.AccountingSupplierParty.Party.Contact.Telephone.Value;
            obligado.Email = factura_ubl.AccountingSupplierParty.Party.Contact.ElectronicMail.Value;
            if (factura_ubl.AccountingSupplierParty.Party.WebsiteURI != null)
                obligado.PaginaWeb = factura_ubl.AccountingSupplierParty.Party.WebsiteURI.Value;
            factura_obj.DatosObligado = obligado;

            #endregion

            #region Detalle de Documento
            List<DocumentoDetalle> list_detalle = new List<DocumentoDetalle>();


            for (int i = 0; i < factura_ubl.InvoiceLine.Length; i++)
            {

                DocumentoDetalle detalle = new DocumentoDetalle();
                detalle.Codigo = Convert.ToInt16(factura_ubl.InvoiceLine[i].ID.Value);
                detalle.ProductoCodigo = factura_ubl.InvoiceLine[i].Item.CatalogueItemIdentification.ID.Value;
                detalle.ProductoNombre = factura_ubl.InvoiceLine[i].Item.Description[0].Value;
                if (factura_ubl.InvoiceLine[i].Item.AdditionalInformation != null)
                {
                    detalle.ProductoDescripcion = factura_ubl.InvoiceLine[i].Item.AdditionalInformation.Value;
                }
                else
                {
                    detalle.ProductoDescripcion = string.Empty;
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
                        }
                        else if (TipoImpuestos.Ica.Equals(tipo_impto))
                        {
                            detalle.ReteIcaPorcentaje = porcentaje_impto;
                            detalle.ReteIcaValor = valor_impto;
                        }
                        else if (TipoImpuestos.ReteFte.Equals(tipo_impto))
                        {
                            detalle.ReteFuentePorcentaje = porcentaje_impto;
                            detalle.ReteFuenteValor = valor_impto;
                        }

                    }
                }

                list_detalle.Add(detalle);

            }
            factura_obj.DocumentoDetalles = list_detalle;
            #endregion

            #region Totales
            factura_obj.ValorSubtotal = factura_ubl.LegalMonetaryTotal.LineExtensionAmount.Value;
            factura_obj.ValorDescuento = factura_ubl.LegalMonetaryTotal.AllowanceTotalAmount.Value;
            factura_obj.ValorIva = factura_ubl.LegalMonetaryTotal.TaxExclusiveAmount.Value;
            factura_obj.Total = factura_ubl.LegalMonetaryTotal.PayableAmount.Value;

            #endregion

            return factura_obj;
        }
    }
}
