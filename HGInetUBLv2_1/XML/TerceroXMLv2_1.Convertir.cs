using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetUBLv2_1.DianListas;

namespace HGInetUBLv2_1
{
	public class TerceroXMLv2_1
	{


		public static Tercero Obtener_obligado(SupplierPartyType empresa)
		{
			Tercero obligado = new Tercero();

			obligado.Identificacion = empresa.Party.PartyTaxScheme.FirstOrDefault().CompanyID.Value;
			obligado.IdentificacionDv = Convert.ToInt16(empresa.Party.PartyTaxScheme.FirstOrDefault().CompanyID.schemeID);
			obligado.TipoIdentificacion = Convert.ToInt16(empresa.Party.PartyTaxScheme.FirstOrDefault().CompanyID.schemeName);
			obligado.TipoPersona = Convert.ToInt16(empresa.AdditionalAccountID.FirstOrDefault().Value);
			obligado.RegimenFiscal = empresa.Party.PartyTaxScheme.FirstOrDefault().TaxLevelCode.listName;
			obligado.Responsabilidades = LibreriaGlobalHGInet.Formato.Coleccion.ConvertirLista(empresa.Party.PartyTaxScheme.FirstOrDefault().TaxLevelCode.Value, ';');
			obligado.CodigoTributo = empresa.Party.PartyTaxScheme.FirstOrDefault().TaxScheme.ID.Value;
			obligado.RazonSocial = empresa.Party.PartyTaxScheme.FirstOrDefault().RegistrationName.Value;
			obligado.NombreComercial = string.IsNullOrEmpty(empresa.Party.PartyName.FirstOrDefault().Name.Value) ? obligado.RazonSocial : empresa.Party.PartyName.FirstOrDefault().Name.Value;

			//Valida si es persona Natural
			if (empresa.Party.Person != null)
			{
				obligado.PrimerNombre = empresa.Party.Person.FirstOrDefault().FirstName.Value;
				if (empresa.Party.Person.FirstOrDefault().MiddleName != null)
					obligado.SegundoNombre = empresa.Party.Person.FirstOrDefault().MiddleName.Value;
				if (empresa.Party.Person.FirstOrDefault().FamilyName != null)
					obligado.PrimerApellido = empresa.Party.Person.FirstOrDefault().FamilyName.Value;
			}


			obligado.Direccion = empresa.Party.PhysicalLocation.Address.AddressLine[0].Line.Value;
			obligado.Ciudad = empresa.Party.PhysicalLocation.Address.CityName.Value;
			obligado.CodigoCiudad = empresa.Party.PhysicalLocation.Address.ID.Value;
			obligado.Departamento = empresa.Party.PhysicalLocation.Address.CountrySubentity.Value;
			obligado.CodigoDepartamento = empresa.Party.PhysicalLocation.Address.CountrySubentityCode.Value;
			obligado.CodigoPais = empresa.Party.PhysicalLocation.Address.Country.IdentificationCode.Value;
			try
			{
				ListaPaises list_paises = new ListaPaises();
				ListaItem pais = list_paises.Items.Where(d => d.Codigo.Equals(obligado.CodigoPais)).FirstOrDefault();
				obligado.Pais = pais.Descripcion;
			}
			catch (Exception)
			{ }
			if (empresa.Party.PhysicalLocation.Address.PostalZone != null)
				obligado.CodigoPostal = empresa.Party.PhysicalLocation.Address.PostalZone.Value;

			if (empresa.Party.Contact != null)
			{
				obligado.Telefono = empresa.Party.Contact.Telephone.Value;
				obligado.Email = empresa.Party.Contact.ElectronicMail.Value;
				//Valida si tiene pagina web
				if (empresa.Party.WebsiteURI != null)
					obligado.PaginaWeb = empresa.Party.WebsiteURI.Value;
			}

			if (empresa.Party.IndustryClassificationCode != null)
			{
				obligado.ActividadEconomica = empresa.Party.IndustryClassificationCode.Value;
			}

			return obligado;
		}


		public static Tercero Obtener_adquiriente(CustomerPartyType cliente)
		{

			Tercero adquiriente = new Tercero();

			adquiriente.Identificacion = cliente.Party.PartyTaxScheme.FirstOrDefault().CompanyID.Value;
			adquiriente.IdentificacionDv = Convert.ToInt16(cliente.Party.PartyTaxScheme.FirstOrDefault().CompanyID.schemeID);
			adquiriente.TipoIdentificacion = Convert.ToInt16(cliente.Party.PartyTaxScheme.FirstOrDefault().CompanyID.schemeName);
			adquiriente.TipoPersona = Convert.ToInt16(cliente.AdditionalAccountID.FirstOrDefault().Value);
			adquiriente.RegimenFiscal = cliente.Party.PartyTaxScheme.FirstOrDefault().TaxLevelCode.listName;
			if (!string.IsNullOrEmpty(cliente.Party.PartyTaxScheme.FirstOrDefault().TaxLevelCode.Value))
				adquiriente.Responsabilidades = LibreriaGlobalHGInet.Formato.Coleccion.ConvertirLista(cliente.Party.PartyTaxScheme.FirstOrDefault().TaxLevelCode.Value, ';');
			adquiriente.CodigoTributo = cliente.Party.PartyTaxScheme.FirstOrDefault().TaxScheme.ID.Value;
			adquiriente.RazonSocial = cliente.Party.PartyTaxScheme.FirstOrDefault().RegistrationName.Value;
			adquiriente.NombreComercial = adquiriente.RazonSocial; //cliente.Party.PartyLegalEntity.FirstOrDefault().CorporateRegistrationScheme.Name.Value;

			//Valida si es persona Natural 
			if (cliente.Party.Person != null)
			{
				adquiriente.PrimerNombre = cliente.Party.Person.FirstOrDefault().FirstName.Value;
				if (cliente.Party.Person.FirstOrDefault().MiddleName != null)
					adquiriente.SegundoNombre = cliente.Party.Person.FirstOrDefault().MiddleName.Value;
				if (cliente.Party.Person.FirstOrDefault().FamilyName != null)
					adquiriente.PrimerApellido = cliente.Party.Person.FirstOrDefault().FamilyName.Value;
			}

			adquiriente.Direccion = cliente.Party.PhysicalLocation.Address.AddressLine[0].Line.Value;
			adquiriente.Ciudad = cliente.Party.PhysicalLocation.Address.CityName.Value;
			adquiriente.CodigoCiudad = cliente.Party.PhysicalLocation.Address.ID.Value;
			adquiriente.Departamento = cliente.Party.PhysicalLocation.Address.CountrySubentity.Value;
			adquiriente.CodigoDepartamento = cliente.Party.PhysicalLocation.Address.CountrySubentityCode.Value;
			adquiriente.CodigoPais = cliente.Party.PhysicalLocation.Address.Country.IdentificationCode.Value;
			try
			{
				ListaPaises list_paises = new ListaPaises();
				ListaItem pais = list_paises.Items.Where(d => d.Codigo.Equals(adquiriente.CodigoPais)).FirstOrDefault();
				adquiriente.Pais = pais.Descripcion;
			}
			catch (Exception)
			{}
			if (cliente.Party.PhysicalLocation.Address.PostalZone != null)
				adquiriente.CodigoPostal = cliente.Party.PhysicalLocation.Address.PostalZone.Value;

			//Direccion Fiscal
			adquiriente.DireccionFiscal = new Direcciones();
			adquiriente.DireccionFiscal.Ciudad = cliente.Party.PartyTaxScheme[0].RegistrationAddress.CityName.Value;
			adquiriente.DireccionFiscal.CodigoCiudad = cliente.Party.PartyTaxScheme[0].RegistrationAddress.ID.Value;
			adquiriente.DireccionFiscal.Departamento = cliente.Party.PartyTaxScheme[0].RegistrationAddress.CountrySubentity.Value;
			adquiriente.DireccionFiscal.CodigoDepartamento = cliente.Party.PartyTaxScheme[0].RegistrationAddress.CountrySubentityCode.Value;
			adquiriente.DireccionFiscal.CodigoPais = cliente.Party.PartyTaxScheme[0].RegistrationAddress.Country.IdentificationCode.Value;
			adquiriente.DireccionFiscal.Direccion = cliente.Party.PartyTaxScheme[0].RegistrationAddress.AddressLine[0].Line.Value;
			try
			{
				ListaPaises list_paises = new ListaPaises();
				ListaItem pais = list_paises.Items.Where(d => d.Codigo.Equals(adquiriente.DireccionFiscal.CodigoPais)).FirstOrDefault();
				adquiriente.DireccionFiscal.Pais = pais.Descripcion;
			}
			catch (Exception)
			{ }
			if (cliente.Party.PartyTaxScheme[0].RegistrationAddress.PostalZone != null)
				adquiriente.DireccionFiscal.CodigoPostal = cliente.Party.PartyTaxScheme[0].RegistrationAddress.PostalZone.Value;

			//Valida si tiene Contacto
			if (cliente.Party.Contact != null)
			{
				adquiriente.Telefono = cliente.Party.Contact.Telephone.Value;
				adquiriente.Email = cliente.Party.Contact.ElectronicMail.Value;
			}
			//Valida si tiene pagina web
			if (cliente.Party.WebsiteURI != null)
				adquiriente.PaginaWeb = cliente.Party.WebsiteURI.Value;

			return adquiriente;
		}

	}
}
