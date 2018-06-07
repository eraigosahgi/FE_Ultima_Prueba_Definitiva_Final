using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetUBL.Objetos;
using HGInetUBL.Recursos;
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
			foreach (XmlElement item in factura_ubl.UBLExtensions[0].ExtensionContent.ChildNodes[0])
			{
				if (item.LocalName.Equals("InvoiceAuthorization"))
				{
					factura_obj.NumeroResolucion = item.FirstChild.InnerText;
				}
				if (item.LocalName.Equals("AuthorizedInvoices"))
				{
					factura_obj.Prefijo = item.FirstChild.InnerText;
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

			factura_obj.DocumentoRef = factura_ubl.BillingReference.FirstOrDefault().InvoiceDocumentReference.ID.Value;
			factura_obj.Fecha = factura_ubl.IssueDate.Value;
			factura_obj.FechaVence = factura_ubl.PaymentMeans.FirstOrDefault().PaymentDueDate.Value;
			factura_obj.Moneda = factura_ubl.DocumentCurrencyCode.Value;
			factura_obj.Nota = factura_ubl.Note[0].Value;

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
				detalle.ProductoDescripcion = factura_ubl.InvoiceLine[i].Item.AdditionalInformation.Value;
				detalle.Cantidad = factura_ubl.InvoiceLine[i].InvoicedQuantity.Value;
				detalle.ValorUnitario = factura_ubl.InvoiceLine[i].Price.PriceAmount.Value;
				detalle.ValorSubtotal = factura_ubl.InvoiceLine[i].LineExtensionAmount.Value;
				if (TipoImpuestos.Iva.Equals(factura_ubl.TaxTotal[i].TaxSubtotal[0].TaxCategory.TaxScheme.ID.Value))
				{
					detalle.IvaPorcentaje = factura_ubl.TaxTotal[i].TaxSubtotal[0].Percent.Value;
					detalle.IvaValor = factura_ubl.TaxTotal[i].TaxSubtotal[0].TaxAmount.Value;
				}
				else if (TipoImpuestos.Consumo.Equals(factura_ubl.TaxTotal[i].TaxSubtotal[0].TaxCategory.TaxScheme.ID.Value))
				{
					detalle.ImpoConsumoPorcentaje = factura_ubl.TaxTotal[i].TaxSubtotal[0].Percent.Value;
					detalle.ValorImpuestoConsumo = factura_ubl.TaxTotal[i].TaxSubtotal[0].TaxAmount.Value;
				}
				else if (TipoImpuestos.Ica.Equals(factura_ubl.TaxTotal[i].TaxSubtotal[0].TaxCategory.TaxScheme.ID.Value))
				{
					detalle.ReteIcaPorcentaje = factura_ubl.TaxTotal[i].TaxSubtotal[0].Percent.Value;
					detalle.ReteIcaValor = factura_ubl.TaxTotal[i].TaxSubtotal[0].TaxAmount.Value;
				}
				else if (TipoImpuestos.ReteFte.Equals(factura_ubl.TaxTotal[i].TaxSubtotal[0].TaxCategory.TaxScheme.ID.Value))
				{
					detalle.ReteFuentePorcentaje = factura_ubl.TaxTotal[i].TaxSubtotal[0].Percent.Value;
					detalle.ReteFuenteValor = factura_ubl.TaxTotal[i].TaxSubtotal[0].TaxAmount.Value;
				}
				

				list_detalle.Add(detalle);

			}

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
