using HGInetMiFacturaElectonicaData.ModeloServicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HGInetUBL
{
	public partial class FacturaXML
	{

		public static Factura Convertir(InvoiceType factura_ubl)
		{
			Factura factura_obj = new Factura();
			UBLExtensionType[] UBLExtensions = new UBLExtensionType[2];

			UBLExtensions = factura_ubl.UBLExtensions;
			UBLExtensionType UBLExtensionDian = new UBLExtensionType();
			UBLExtensionDian.ExtensionContent = UBLExtensions[0].ExtensionContent;



			factura_obj.Documento = Convert.ToInt32(factura_ubl.ID.Value);
			factura_obj.Fecha = factura_ubl.IssueDate.Value;

			InvoiceType Invoice = new InvoiceType();
			factura_obj.Nota = factura_ubl.Note[0].Value;

			Tercero Adquiriente = new Tercero();


			Adquiriente.Identificacion = factura_ubl.AccountingCustomerParty.Party.PartyIdentification[0].ID.Value;
			Adquiriente.TipoPersona = Convert.ToInt16(factura_ubl.AccountingCustomerParty.AdditionalAccountID.Value);
			Adquiriente.Regimen = Convert.ToInt16(factura_ubl.AccountingCustomerParty.Party.PartyTaxScheme[0].TaxLevelCode.Value);


			return factura_obj;
		}
	}
}
