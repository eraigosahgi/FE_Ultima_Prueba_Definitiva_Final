using HGInetMiFacturaElectonicaData.ModeloServicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HGInetUBLv2_1.DianListas;

namespace HGInetUBLv2_1
{
	public class TerceroXML
	{

		/// <summary>
		/// Llena el objeto del Obligado a facturar con los datos de la empresa
		/// </summary>
		/// <param name="empresa">Datos de la empresa</param>
		/// <returns>Objeto de tipo SupplierPartyType1</returns>
		public static SupplierPartyType ObtenerObligado(Tercero empresa, string prefijo, bool Notas = false, bool nota_adquisicion = false)
		{
			try
			{
				if (empresa == null)
					throw new Exception("Los datos de la empresa son inválidos.");

				// Datos del obligado a facturar
				SupplierPartyType AccountingSupplierParty = new SupplierPartyType();
				PartyType Party = new PartyType();

				#region Tipo de persona
				//1-Persona Juridica; 2-Persona Natural
				AdditionalAccountIDType AdditionalAccountID = new AdditionalAccountIDType();
				AdditionalAccountID.Value = empresa.TipoPersona.ToString();//Tipo de persona (LISTADO DE VALORES DEFINIDO POR LA DIAN)
				AdditionalAccountID.schemeAgencyID = "195";
				
				//**** Para Documento Equivalente(Sopote) 01 - Residente; 02 - No Residente 
				//AdditionalAccountID.schemeID = "01";

				AccountingSupplierParty.AdditionalAccountID = new AdditionalAccountIDType[1];
				AccountingSupplierParty.AdditionalAccountID[0] = AdditionalAccountID;
				#endregion

				#region Grupo con informaciones sobre el nombre comercial del emisor 
				//---Se puede tener Nombre principal, Sucursal, Razon Social
				PartyNameType[] PartyNameType = new PartyNameType[2];
				PartyNameType partyName = new PartyNameType();
				//---Razon social
				NameType1 Name_RZ = new NameType1();
				Name_RZ.Value = string.IsNullOrEmpty(empresa.NombreComercial) ? empresa.RazonSocial : empresa.NombreComercial;
				partyName.Name = Name_RZ;
				PartyNameType[0] = partyName;

				//---Nombre Comercial
				/*NameType1 Name_NC = new NameType1();
				partyName = new PartyNameType();
				Name_NC.Value = empresa.NombreComercial;
				partyName.Name = Name_NC;
				PartyNameType[1] = partyName;*/

				Party.PartyName = PartyNameType;
				#endregion


				#region Dirección---Grupo con informaciones con respeto a la localización física 
				//---Grupo con informaciónes con respeto a la localización física del emisor 
				LocationType1 PhysicalLocation = new LocationType1();
				AddressType Address = new AddressType();

				//----6.4.3. Municipios:  cbc:CityName Ver listado del DANE
				Address.ID = new IDType();
				Address.ID.Value = empresa.CodigoCiudad;
				CityNameType City = new CityNameType();
				ListaMunicipio list_municipio = new ListaMunicipio();
				if (empresa.CodigoPais.Equals("CO"))
				{
					ListaItem municipio = list_municipio.Items.Where(d => d.Codigo.Equals(empresa.CodigoCiudad)).FirstOrDefault();
					City.Value = municipio.Nombre; //empresa.Ciudad; //Ciudad (LISTADO DE VALORES DEFINIDO POR LA DIAN)
				}
				else
				{
					City.Value = empresa.Ciudad;
				}
				
				
				
				Address.CityName = City;

				//6.4.2. Departamentos (ISO 3166-2:CO):  cbc:CountrySubentity, cbc:CountrySubentityCode
				CountrySubentityType CountrySubentity = new CountrySubentityType();
				CountrySubentityCodeType CountrySubentityCode = new CountrySubentityCodeType();
				ListaDepartamentos list_depart = new ListaDepartamentos();
				if (empresa.CodigoPais.Equals("CO"))
				{
					ListaItem departamento = list_depart.Items.Where(d => d.Codigo.Equals(empresa.CodigoDepartamento)).FirstOrDefault();
					CountrySubentity.Value = departamento.Nombre;//"Antioquia";//Listado de Departamentos el Nombre
					Address.CountrySubentity = CountrySubentity;
					CountrySubentityCode.Value = empresa.CodigoDepartamento;//Listado de Departamentos el codigo
				}
				else
				{
					CountrySubentity.Value = empresa.Departamento;
					CountrySubentityCode.Value = empresa.CodigoDepartamento;

				}
				
				Address.CountrySubentityCode = CountrySubentityCode;

				//Direccion
				AddressLineType[] AddressLines = new AddressLineType[1];
				AddressLineType AddressLine = new AddressLineType();
				LineType Line = new LineType();
				Line.Value = empresa.Direccion;
				AddressLine.Line = Line;

				AddressLines[0] = AddressLine;
				Address.AddressLine = AddressLines;

				//---Zona Postal
				Address.PostalZone = new PostalZoneType();
				Address.PostalZone.Value = empresa.CodigoPostal;//Listado de Zona Postal de Colombia

				//6.4.1. Países (ISO 3166-1): cbc:IdentificationCode 
				//ISO 3166-1 alfa-2: Códigos de país de das letras. Si recomienda como el código de propósito
				//general.Estos códigos se utilizan por ejemplo en internet como dominios geográficos de nivel superior.
				CountryType Country = new CountryType();
				IdentificationCodeType IdentificationCode = new IdentificationCodeType();
				IdentificationCode.Value = empresa.CodigoPais; //Pais (LISTADO DE VALORES DEFINIDO POR LA DIAN 5.4.1)
				Country.IdentificationCode = IdentificationCode;
				Country.Name = new NameType1();
				ListaPaises list_paises = new ListaPaises();
				ListaItem pais = list_paises.Items.Where(d => d.Codigo.Equals(empresa.CodigoPais)).FirstOrDefault();
				Country.Name.Value = pais.Nombre;//"Colombia";//Pais (LISTADO DE VALORES DEFINIDO POR LA DIAN 5.4.1)
				Country.Name.languageID = "es";
				Address.Country = Country;

				PhysicalLocation.Address = Address;
				Party.PhysicalLocation = PhysicalLocation;
				#endregion

				#region Regimen --PartyTaxScheme

				PartyTaxSchemeType[] PartyTaxSchemes = new PartyTaxSchemeType[1];
				PartyTaxSchemeType PartyTaxScheme = new PartyTaxSchemeType();

				//---si es NIT debe llenarse con la Razon social y es obligatorio
				PartyTaxScheme.RegistrationName = new RegistrationNameType();
				PartyTaxScheme.RegistrationName.Value = empresa.RazonSocial;

				//Grupo de información para informar dirección fiscal - RUT
				//--Puede ser diferente la localizacion Fisica a la del RUT
				PartyTaxScheme.RegistrationAddress = new AddressType();

				//Se valida si envian una direccion fiscal diferente
				if (empresa.DireccionFiscal != null)
				{
					Address = new AddressType();

					//----6.4.3. Municipios:  cbc:CityName Ver listado del DANE
					Address.ID = new IDType();
					Address.ID.Value = empresa.DireccionFiscal.CodigoCiudad;
					City = new CityNameType();
					list_municipio = new ListaMunicipio();
					ListaItem municipio = list_municipio.Items.Where(d => d.Codigo.Equals(empresa.DireccionFiscal.CodigoCiudad)).FirstOrDefault();
					City.Value = municipio.Nombre; //empresa.Ciudad; //Ciudad (LISTADO DE VALORES DEFINIDO POR LA DIAN)
					Address.CityName = City;

					//6.4.2. Departamentos (ISO 3166-2:CO):  cbc:CountrySubentity, cbc:CountrySubentityCode
					CountrySubentity = new CountrySubentityType();
					list_depart = new ListaDepartamentos();
					ListaItem departamento = list_depart.Items.Where(d => d.Codigo.Equals(empresa.DireccionFiscal.CodigoDepartamento)).FirstOrDefault();
					CountrySubentity.Value = departamento.Nombre;//"Antioquia";//Listado de Departamentos el Nombre
					Address.CountrySubentity = CountrySubentity;
					CountrySubentityCode = new CountrySubentityCodeType();
					CountrySubentityCode.Value = empresa.DireccionFiscal.CodigoDepartamento;//Listado de Departamentos el codigo
					Address.CountrySubentityCode = CountrySubentityCode;

					//Direccion
					AddressLines = new AddressLineType[1];
					AddressLine = new AddressLineType();
					Line = new LineType();
					Line.Value = empresa.DireccionFiscal.Direccion;
					AddressLine.Line = Line;

					AddressLines[0] = AddressLine;
					Address.AddressLine = AddressLines;

					//---Zona Postal
					Address.PostalZone = new PostalZoneType();
					Address.PostalZone.Value = empresa.DireccionFiscal.CodigoPostal;//Listado de Zona Postal de Colombia

					//6.4.1. Países (ISO 3166-1): cbc:IdentificationCode 
					//ISO 3166-1 alfa-2: Códigos de país de das letras. Si recomienda como el código de propósito
					//general.Estos códigos se utilizan por ejemplo en internet como dominios geográficos de nivel superior.
					Country = new CountryType();
					IdentificationCode = new IdentificationCodeType();
					IdentificationCode.Value = empresa.DireccionFiscal.CodigoPais; //Pais (LISTADO DE VALORES DEFINIDO POR LA DIAN 5.4.1)
					Country.IdentificationCode = IdentificationCode;
					Country.Name = new NameType1();
					list_paises = new ListaPaises();
					pais = list_paises.Items.Where(d => d.Codigo.Equals(empresa.DireccionFiscal.CodigoPais)).FirstOrDefault();
					Country.Name.Value = pais.Nombre;//"Colombia";//Pais (LISTADO DE VALORES DEFINIDO POR LA DIAN 5.4.1)
					Country.Name.languageID = "es";
					Address.Country = Country;

				}
				PartyTaxScheme.RegistrationAddress = Address;

				//Identificacion
				CompanyIDType CompanyID = new CompanyIDType();

				//---Validar si es NIT
				CompanyID.Value = empresa.Identificacion.ToString();

				//---Si es Nit debe star bien calculado
				CompanyID.schemeID = empresa.IdentificacionDv.ToString();

				//----//Tipo documento (LISTADO DE VALORES DEFINIDO POR LA DIAN 6.2.1)
				CompanyID.schemeName = empresa.TipoIdentificacion.ToString();
				CompanyID.schemeAgencyID = "195";
				CompanyID.schemeAgencyName = "CO, DIAN (Dirección de Impuestos y Aduanas Nacionales)";
				PartyTaxScheme.CompanyID = CompanyID;

				//Regimen Fiscal y Responsabilidades Tributarias
				//----Validar si cambia la ocurrecia de las responsabilidades esta de 1..1 y deberia estar 1..N
				TaxLevelCodeType TaxLevelCode = new TaxLevelCodeType();
				//-----Listado 6.2.4
				//TaxLevelCode.listName = empresa.RegimenFiscal;

				//---Listado 6.2.7 Responsabilidades pero solo permite 1 actualmente
				//TaxLevelCode.Value = empresa.Regimen.ToString();
				///----Se pone responsabilidad para las pruebas
				string list_responsabilidades = LibreriaGlobalHGInet.Formato.Coleccion.ConvertListToString(empresa.Responsabilidades,";");
				TaxLevelCode.Value = list_responsabilidades;//"O-99";
				PartyTaxScheme.TaxLevelCode = TaxLevelCode;

				//Grupo de información para informar dirección fiscal - RUT
				//--Puede ser diferente la localizacion Fisica a la del RUT
				//PartyTaxScheme.RegistrationAddress = new AddressType();
				//PartyTaxScheme.RegistrationAddress = Address;

				//--- Emisor es responsable: debe existir la información correspondiente -- Ocurrencia 0..1
				//--Tabla 6.2.2
				//--Validar el llenado
				TaxSchemeType TaxScheme = new TaxSchemeType();
				TaxScheme.ID = new IDType();
				TaxScheme.ID.Value = empresa.CodigoTributo;
				ListaTipoImpuestoTercero list_tipoImp = new ListaTipoImpuestoTercero();
				ListaItem tipoimp = list_tipoImp.Items.Where(d => d.Codigo.Equals(empresa.CodigoTributo)).FirstOrDefault();
				TaxScheme.Name = new NameType1();
				TaxScheme.Name.Value = tipoimp.Nombre; //"IVA";
				PartyTaxScheme.TaxScheme = TaxScheme;
				PartyTaxSchemes[0] = PartyTaxScheme;
				Party.PartyTaxScheme = PartyTaxSchemes;
				#endregion

				#region PartyLegalEntity -- Grupo de informaciones legales del adquirente 
				//notas de adquisicion no requiere esta informacion por que el facturador del documento no esta obligado a facturar electronico
				if (nota_adquisicion == false)
				{
					PartyLegalEntityType[] PartyLegalEntitys = new PartyLegalEntityType[1];
					PartyLegalEntityType PartyLegalEntity = new PartyLegalEntityType();
					PartyLegalEntity.RegistrationName = new RegistrationNameType();
					PartyLegalEntity.RegistrationName.Value = empresa.RazonSocial;
					PartyLegalEntity.CompanyID = new CompanyIDType();
					PartyLegalEntity.CompanyID = CompanyID;

					if (Notas == false)
					{
						//Grupo de informaciones legales del emisor 
						PartyLegalEntity.CorporateRegistrationScheme = new CorporateRegistrationSchemeType();

						//Prefijo de la facturación usada para el punto de venta
						//---Validar---obligatorio para el obligado ocurrencia 1..1
						PartyLegalEntity.CorporateRegistrationScheme.ID = new IDType();
						PartyLegalEntity.CorporateRegistrationScheme.ID.Value = prefijo;

						//Número de matrícula mercantil (identificador de sucursal: punto de facturación)
						//---Validar--ocurrencia 0..1
						PartyLegalEntity.CorporateRegistrationScheme.Name = new NameType1();
						PartyLegalEntity.CorporateRegistrationScheme.Name.Value = "0";//empresa.NombreComercial; //"HGI SAS";
					}

					if (!string.IsNullOrEmpty(empresa.ActividadEconomica))
					{
						Party.IndustryClassificationCode = new IndustryClassificationCodeType();
						Party.IndustryClassificationCode.Value = empresa.ActividadEconomica;
					}

					PartyLegalEntitys[0] = PartyLegalEntity;
					Party.PartyLegalEntity = PartyLegalEntitys;
				}
				#endregion

				#region Contact
				ContactType Contact = new ContactType();
				TelephoneType Telephone = new TelephoneType();
				Telephone.Value = empresa.Telefono;
				Contact.Telephone = Telephone;
				ElectronicMailType Mail = new ElectronicMailType();
				Mail.Value = empresa.Email;
				Contact.ElectronicMail = Mail;
				Party.Contact = Contact;

				//WebsiteURIType Web = new WebsiteURIType();
				//Web.Value = empresa.PaginaWeb;
				//Party.WebsiteURI = Web;
				#endregion

				AccountingSupplierParty.Party = Party;

				return AccountingSupplierParty;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Llena el objeto del adquiriente con los datos del tercero
		/// </summary>
		/// <param name="tercero">Datos de la tercero</param>
		/// <returns>Objeto de tipo SupplierPartyType1</returns>
		public static CustomerPartyType ObtenerAquiriente(Tercero tercero)
		{
			try
			{
				if (tercero == null)
					throw new Exception("Los datos del tercero son inválidos.");

				// Datos del adquiriente de la factura
				CustomerPartyType AccountingCustomerParty = new CustomerPartyType();
				PartyType Party = new PartyType();

				

				#region Tipo de persona
				//1-Persona Juridica; 2-Persona Natural
				AdditionalAccountIDType AdditionalAccountID = new AdditionalAccountIDType();
				//if (!tercero.Identificacion.Equals("222222222222") && tercero.TipoPersona == 2)
				//{
				//	AdditionalAccountID.Value = "1"; //tercero.TipoPersona.ToString();//Tipo de persona (LISTADO DE VALORES DEFINIDO POR LA DIAN)
				//}
				//else
				//{
				//	AdditionalAccountID.Value = tercero.TipoPersona.ToString();//Tipo de persona (LISTADO DE VALORES DEFINIDO POR LA DIAN)
				//}
				AdditionalAccountID.Value = tercero.TipoPersona.ToString();//Tipo de persona (LISTADO DE VALORES DEFINIDO POR LA DIAN)
				AccountingCustomerParty.AdditionalAccountID = new AdditionalAccountIDType[1];
				AccountingCustomerParty.AdditionalAccountID[0] = AdditionalAccountID;
				#endregion

				//---Razon social 
				#region Grupo con informaciones sobre el nombre comercial del emisor 
				//---Se puede tener Nombre principal, Sucursal, Razon Social
				PartyNameType[] PartyNameType = new PartyNameType[1];
				PartyNameType partyName = new PartyNameType();
				//---Razon social
				NameType1 Name = new NameType1();
				Name.Value = tercero.RazonSocial;
				partyName.Name = Name;
				PartyNameType[0] = partyName;
				Party.PartyName = PartyNameType;
				#endregion


				#region Dirección---Grupo con informaciones con respeto a la localización física 

				LocationType1 PhysicalLocation = new LocationType1();
				AddressType Address = new AddressType();

				//----5.4.3. Municipios:  cbc:CityName Ver listado del DANE
				Address.ID = new IDType();
				Address.ID.Value = (!string.IsNullOrEmpty(tercero.CodigoCiudad) && tercero.CodigoPais.Equals("CO"))? tercero.CodigoCiudad : "";
				CityNameType City = new CityNameType();
				if (tercero.CodigoPais.Equals("CO"))
				{
					ListaMunicipio list_municipio = new ListaMunicipio();
					ListaItem municipio = list_municipio.Items.Where(d => d.Codigo.Equals(tercero.CodigoCiudad)).FirstOrDefault();
					City.Value = municipio.Nombre; //Ciudad (LISTADO DE VALORES DEFINIDO POR LA DIAN)
					tercero.Ciudad = municipio.Nombre;
				}
				else
				{
					City.Value = tercero.Ciudad;
				}
				Address.CityName = City;
				

				//5.4.2. Departamentos (ISO 3166-2:CO):  cbc:CountrySubentity, cbc:CountrySubentityCode
				CountrySubentityType CountrySubentity = new CountrySubentityType();
				if (tercero.CodigoPais.Equals("CO"))
				{
					ListaDepartamentos list_depart = new ListaDepartamentos();
					ListaItem departamento = list_depart.Items.Where(d => d.Codigo.Equals(tercero.CodigoDepartamento)).FirstOrDefault();
					CountrySubentity.Value = departamento.Nombre; //Listado de Departamentos el Nombre
					Address.CountrySubentity = CountrySubentity;
					CountrySubentityCodeType CountrySubentityCode = new CountrySubentityCodeType();
					CountrySubentityCode.Value = tercero.CodigoDepartamento; //Listado de Departamentos el codigo
					Address.CountrySubentityCode = CountrySubentityCode;
					tercero.Departamento = departamento.Nombre;
				}
				else
				{
					CountrySubentity.Value = tercero.Departamento; //Listado de Departamentos el Nombre
					Address.CountrySubentity = CountrySubentity;
					CountrySubentityCodeType CountrySubentityCode = new CountrySubentityCodeType();
					CountrySubentityCode.Value = (string.IsNullOrEmpty(tercero.CodigoDepartamento)) ? "": tercero.CodigoDepartamento; //Listado de Departamentos el codigo
					Address.CountrySubentityCode = CountrySubentityCode;
				}

				//Direccion
				//Informar la dirección, sin ciudad ni departamento.
				//Si el adquirente no es responsable de IVA entonces se puede informar solo este elemento en dirección. 
				AddressLineType[] AddressLines = new AddressLineType[1];
				AddressLineType AddressLine = new AddressLineType();
				LineType Line = new LineType();
				Line.Value = tercero.Direccion;
				AddressLine.Line = Line;

				AddressLines[0] = AddressLine;
				Address.AddressLine = AddressLines;

				//Zona Postal - Obligatorio para emisores y Adquirentes Responsables 
				Address.PostalZone = new PostalZoneType();
				Address.PostalZone.Value = tercero.CodigoPostal;//Listado de Zona Postal de Colombia

				//5.4.1. Países (ISO 3166-1): cbc:IdentificationCode 
				//ISO 3166-1 alfa-2: Códigos de país de das letras. Si recomienda como el código de propósito
				//general.Estos códigos se utilizan por ejemplo en internet como dominios geográficos de nivel superior.
				CountryType Country = new CountryType();

				IdentificationCodeType IdentificationCode = new IdentificationCodeType();
				IdentificationCode.Value = tercero.CodigoPais; //Pais (LISTADO DE VALORES DEFINIDO POR LA DIAN 5.4.1)
				Country.IdentificationCode = IdentificationCode;
				Country.Name = new NameType1();
				ListaPaises list_paises = new ListaPaises();
				ListaItem pais = list_paises.Items.Where(d => d.Codigo.Equals(tercero.CodigoPais)).FirstOrDefault();
				Country.Name.Value = pais.Nombre;//"Colombia";//Pais (LISTADO DE VALORES DEFINIDO POR LA DIAN 5.4.1)
				Country.Name.languageID = "es";
				Address.Country = Country;

				PhysicalLocation.Address = Address;
				Party.PhysicalLocation = PhysicalLocation;
				#endregion

				#region Regimen --PartyTaxScheme
				//--Rechazo:
				//--Si el grupo no es informado y si se cumple por lo menos una de las siguientes situaciones:
				//--Si el adquirente es persona jurídica:  AdditionalAccountID contiene “1”
				//--En caso de operación de exportación: Si   //cbc:InvoiceTypeCode = “02”
				//--Si el valor total de la factura es mayor de 100 UVT: si //LegalMonetaryTotal/cbc:PayableAmount es superior a este monto 

				PartyTaxSchemeType[] PartyTaxSchemes = new PartyTaxSchemeType[1];
				PartyTaxSchemeType PartyTaxScheme = new PartyTaxSchemeType();

				//---si es NIT debe llenarse con la Razon social y es obligatorio
				PartyTaxScheme.RegistrationName = new RegistrationNameType();
				PartyTaxScheme.RegistrationName.Value = tercero.RazonSocial;

				//Grupo de información para informar dirección fiscal - RUT
				//--Puede ser diferente la localizacion Fisica a la del RUT
				PartyTaxScheme.RegistrationAddress = new AddressType();

				//Se valida si envian una direccion fiscal diferente a la de entrega
				if (tercero.DireccionFiscal != null)
				{
					Address = new AddressType();

					//----5.4.3. Municipios:  cbc:CityName Ver listado del DANE
					Address.ID = new IDType();
					Address.ID.Value = (!string.IsNullOrEmpty(tercero.DireccionFiscal.CodigoCiudad) && tercero.DireccionFiscal.CodigoPais.Equals("CO")) ? tercero.DireccionFiscal.CodigoCiudad : "";
					City = new CityNameType();
					if (tercero.DireccionFiscal.CodigoPais.Equals("CO"))
					{
						ListaMunicipio list_municipio = new ListaMunicipio();
						ListaItem municipio = list_municipio.Items.Where(d => d.Codigo.Equals(tercero.DireccionFiscal.CodigoCiudad)).FirstOrDefault();
						City.Value = municipio.Nombre; //Ciudad (LISTADO DE VALORES DEFINIDO POR LA DIAN)
						tercero.DireccionFiscal.Ciudad = municipio.Nombre;
					}
					else
					{
						City.Value = tercero.DireccionFiscal.Ciudad;
					}
					Address.CityName = City;


					//5.4.2. Departamentos (ISO 3166-2:CO):  cbc:CountrySubentity, cbc:CountrySubentityCode
					CountrySubentity = new CountrySubentityType();
					if (tercero.DireccionFiscal.CodigoPais.Equals("CO"))
					{
						ListaDepartamentos list_depart = new ListaDepartamentos();
						ListaItem departamento = list_depart.Items.Where(d => d.Codigo.Equals(tercero.DireccionFiscal.CodigoDepartamento)).FirstOrDefault();
						CountrySubentity.Value = departamento.Nombre; //Listado de Departamentos el Nombre
						Address.CountrySubentity = CountrySubentity;
						CountrySubentityCodeType CountrySubentityCode = new CountrySubentityCodeType();
						CountrySubentityCode.Value = tercero.DireccionFiscal.CodigoDepartamento; //Listado de Departamentos el codigo
						Address.CountrySubentityCode = CountrySubentityCode;
						tercero.DireccionFiscal.Departamento = departamento.Nombre;
					}
					else
					{
						CountrySubentity.Value = tercero.DireccionFiscal.Departamento; //Listado de Departamentos el Nombre
						Address.CountrySubentity = CountrySubentity;
						CountrySubentityCodeType CountrySubentityCode = new CountrySubentityCodeType();
						CountrySubentityCode.Value = (string.IsNullOrEmpty(tercero.DireccionFiscal.CodigoDepartamento)) ? "" : tercero.DireccionFiscal.CodigoDepartamento; //Listado de Departamentos el codigo
						Address.CountrySubentityCode = CountrySubentityCode;
					}

					//Direccion
					//Informar la dirección, sin ciudad ni departamento.
					//Si el adquirente no es responsable de IVA entonces se puede informar solo este elemento en dirección. 
					AddressLines = new AddressLineType[1];
					AddressLine = new AddressLineType();
					Line = new LineType();
					Line.Value = tercero.DireccionFiscal.Direccion;
					AddressLine.Line = Line;

					AddressLines[0] = AddressLine;
					Address.AddressLine = AddressLines;

					//Zona Postal - Obligatorio para emisores y Adquirentes Responsables 
					Address.PostalZone = new PostalZoneType();
					Address.PostalZone.Value = tercero.DireccionFiscal.CodigoPostal;//Listado de Zona Postal de Colombia

					//5.4.1. Países (ISO 3166-1): cbc:IdentificationCode 
					//ISO 3166-1 alfa-2: Códigos de país de das letras. Si recomienda como el código de propósito
					//general.Estos códigos se utilizan por ejemplo en internet como dominios geográficos de nivel superior.
					Country = new CountryType();

					IdentificationCode = new IdentificationCodeType();
					IdentificationCode.Value = tercero.DireccionFiscal.CodigoPais; //Pais (LISTADO DE VALORES DEFINIDO POR LA DIAN 5.4.1)
					Country.IdentificationCode = IdentificationCode;
					Country.Name = new NameType1();
					list_paises = new ListaPaises();
					pais = list_paises.Items.Where(d => d.Codigo.Equals(tercero.DireccionFiscal.CodigoPais)).FirstOrDefault();
					Country.Name.Value = pais.Nombre;//"Colombia";//Pais (LISTADO DE VALORES DEFINIDO POR LA DIAN 5.4.1)
					Country.Name.languageID = "es";
					Address.Country = Country;
				}
				PartyTaxScheme.RegistrationAddress = Address;

				//Identificacion
				CompanyIDType CompanyID = new CompanyIDType();

				//---Validar si es NIT
				CompanyID.Value = tercero.Identificacion.ToString();
				//---Si es Nit debe star bien calculado
				CompanyID.schemeID = tercero.IdentificacionDv.ToString();
				//----//Tipo documento (LISTADO DE VALORES DEFINIDO POR LA DIAN 5.2.1)
				CompanyID.schemeName = tercero.TipoIdentificacion.ToString();
				CompanyID.schemeAgencyID = "195";
				CompanyID.schemeAgencyName = "CO, DIAN (Dirección de Impuestos y Aduanas Nacionales)";
				PartyTaxScheme.CompanyID = CompanyID;

				//Regimen Fiscal y Responsabilidades Tributarias
				//----validar el regimen
				TaxLevelCodeType TaxLevelCode = new TaxLevelCodeType();
				//-----Listado 6.2.4
				//TaxLevelCode.listName = tercero.RegimenFiscal;
				//---Listado 6.2.7 Responsabilidades para enviar varias se deben seperar por ;
				//TaxLevelCode.Value = tercero.Regimen.ToString();
				//----Se pone Responsabilidad para las pruebas
				string list_responsabilidades = LibreriaGlobalHGInet.Formato.Coleccion.ConvertListToString(tercero.Responsabilidades, ";");
				TaxLevelCode.Value = list_responsabilidades;
				PartyTaxScheme.TaxLevelCode = TaxLevelCode;


				//---Tributos Si el adquirente es responsable, el NIT debe estar activo en el RUT
				//--Tabla 6.2.2
				//--Validar si se pueden varias en ejemplo solo presenta 1 y si se repite todo el PartyTaxScheme para los que aplique
				TaxSchemeType TaxScheme = new TaxSchemeType();
				TaxScheme.ID = new IDType();
				TaxScheme.ID.Value = tercero.CodigoTributo;
				ListaTipoImpuestoTercero list_tipoImp = new ListaTipoImpuestoTercero();
				ListaItem tipoimp = list_tipoImp.Items.Where(d => d.Codigo.Equals(tercero.CodigoTributo)).FirstOrDefault();
				TaxScheme.Name = new NameType1();
				TaxScheme.Name.Value = tipoimp.Nombre; //"IVA";
				PartyTaxScheme.TaxScheme = TaxScheme;
				PartyTaxSchemes[0] = PartyTaxScheme;
				Party.PartyTaxScheme = PartyTaxSchemes;
				#endregion

				#region PartyLegalEntity -- Grupo de informaciones legales del adquirente 
				PartyLegalEntityType[] PartyLegalEntitys = new PartyLegalEntityType[1];
				PartyLegalEntityType PartyLegalEntity = new PartyLegalEntityType();
				PartyLegalEntity.RegistrationName = new RegistrationNameType();
				PartyLegalEntity.RegistrationName.Value = tercero.RazonSocial;
				PartyLegalEntity.CompanyID = new CompanyIDType();
				PartyLegalEntity.CompanyID = CompanyID;

				//Grupo de informaciones de registro del adquiriente 
				PartyLegalEntity.CorporateRegistrationScheme = new CorporateRegistrationSchemeType();

				//Prefijo de la facturación usada para el punto de venta
				//---Validar---
				//PartyLegalEntity.CorporateRegistrationScheme.ID = new IDType();
				//PartyLegalEntity.CorporateRegistrationScheme.ID.Value = "0123";

				//Número de matrícula mercantil (identificador de sucursal: punto de facturación)
				//---Validar
				PartyLegalEntity.CorporateRegistrationScheme.Name = new NameType1();
				PartyLegalEntity.CorporateRegistrationScheme.Name.Value = "0";//(!string.IsNullOrEmpty(tercero.NombreComercial)) ? tercero.NombreComercial : tercero.RazonSocial;

				PartyLegalEntitys[0] = PartyLegalEntity;
				Party.PartyLegalEntity = PartyLegalEntitys;
				#endregion

				#region Contact
				ContactType Contact = new ContactType();
				TelephoneType Telephone = new TelephoneType();
				Telephone.Value = tercero.Telefono;
				Contact.Telephone = Telephone;
				ElectronicMailType Mail = new ElectronicMailType();
				Mail.Value = tercero.Email;
				Contact.ElectronicMail = Mail;
				Party.Contact = Contact;

				//WebsiteURIType Web = new WebsiteURIType();
				//Web.Value = tercero.PaginaWeb;
				//Party.WebsiteURI = Web;
				#endregion

				#region Datos personales de Persona Natural Validar, no hay documentacion de esto

				if (tercero.TipoPersona.Equals(2))//Persona natural
				{
					PersonType Person = new PersonType();

					FirstNameType FirstName = new FirstNameType();
					FirstName.Value = tercero.PrimerNombre;
					Person.FirstName = FirstName;

					MiddleNameType MiddleName = new MiddleNameType();
					if (tercero.SegundoNombre != null && !tercero.SegundoNombre.Equals(string.Empty))
					{
						MiddleName.Value = tercero.SegundoNombre;
					}
					Person.MiddleName = MiddleName;

					FamilyNameType FamilyName = new FamilyNameType();
					FamilyName.Value = string.Format("{0} {1}", tercero.PrimerApellido, tercero.SegundoApellido);
					Person.FamilyName = FamilyName;

					PersonType[] persontype = new PersonType[1];
					persontype[0] = Person;
					Party.Person = persontype;

					#region Documento y tipo de documento---Ya esta en PartyTaxScheme se agrega cuando sea solo persona natural
					PartyIdentificationType[] PartyIdentifications = new PartyIdentificationType[1];
					PartyIdentificationType PartyIdentification = new PartyIdentificationType();
					IDType ID = new IDType();
					ID.schemeAgencyName = "CO, DIAN (Direccion de Impuestos y Aduanas Nacionales)";
					ID.schemeAgencyID = "195";
					ID.schemeName = tercero.TipoIdentificacion.ToString();
					ID.schemeID = tercero.IdentificacionDv.ToString(); //Tipo documento (LISTADO DE VALORES DEFINIDO POR LA DIAN)
					ID.Value = tercero.Identificacion.ToString();
					PartyIdentification.ID = ID;
					PartyIdentifications[0] = PartyIdentification;
					Party.PartyIdentification = PartyIdentifications;
					#endregion
				}
				#endregion


				AccountingCustomerParty.Party = Party;

				return AccountingCustomerParty;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

	}
}
