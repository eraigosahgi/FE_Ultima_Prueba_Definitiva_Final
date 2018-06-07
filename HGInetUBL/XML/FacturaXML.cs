using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetUBL.Objetos;
using HGInetUBL.Recursos;
using HGInetUBL.XML;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Properties;
using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using LibreriaGlobalHGInet.Objetos;
using HGInetMiFacturaElectonicaData.ControllerSql;

namespace HGInetUBL
{
	public partial class FacturaXML
	{

		/// <summary>
		/// Crea el XML con la informacion de la factura en formato UBL
		/// </summary>
		/// <param name="documento">Objeto de tipo TblDocumentos que contiene la informacion de la factura</param>
		/// <param name="resolucion">Objeto que contiene los parametros de la DIAN</param>
		/// <param name="tipo">Indica el tipo de documento</param>
		/// <returns>Ruta donde se guardo el archivo XML</returns>  
		public static FacturaE_Documento CrearDocumento(Factura documento, HGInetMiFacturaElectonicaData.ModeloServicio.ExtensionDian resolucion, TipoDocumento tipo)
		{
			try
			{
				if (documento == null)
					throw new Exception("La documento es inválido.");

				//Obtiene el nombre del archivo XML
				string nombre_archivo_xml = NombramientoArchivo.ObtenerXml(documento.Documento.ToString(), documento.DatosObligado.Identificacion.ToString(), tipo);

				if (string.IsNullOrWhiteSpace(nombre_archivo_xml))
					throw new ApplicationException("El nombre del archivo es inválido.");

				InvoiceType factura = new InvoiceType();
				XmlSerializerNamespaces namespaces_xml = NamespacesXML.Obtener();

				#region factura.UBLVersionID - Versión UBL
				/*Versión de los esquemas UBL utilizados por la Dian */
				UBLVersionIDType UBLVersionID = new UBLVersionIDType();
				UBLVersionID.Value = Recursos.VersionesDIAN.UBLVersionID;
				factura.UBLVersionID = UBLVersionID;
				#endregion

				#region factura.ProfileID - Versión del documento DIAN_UBL.xsd
				/*Versión del documento DIAN_UBL.xsd publicado por la DIAN*/
				ProfileIDType ProfileID = new ProfileIDType();
				ProfileID.Value = Recursos.VersionesDIAN.ProfileID;
				factura.ProfileID = ProfileID;
				#endregion

				#region factura.ID - Número de documento

				string numero_documento = "";
				if (!string.IsNullOrEmpty(documento.Prefijo))
					numero_documento = string.Format("{0}", documento.Prefijo);

				numero_documento = string.Format("{0}{1}", numero_documento, documento.Documento.ToString());

				/*Número de documento: Número de factura o factura cambiaria.*/
				IDType ID = new IDType();
				//ID.Value = documento.IntDocumento.ToString();
				ID.Value = numero_documento;
				factura.ID = ID;
				#endregion

				#region factura.IssueDate - Fecha de la factura
				/*Fecha de emision de la factura*/
				IssueDateType IssueDate = new IssueDateType();
				IssueDate.Value = Convert.ToDateTime(documento.Fecha.ToString(Fecha.formato_fecha_hginet));
				factura.IssueDate = IssueDate;
				#endregion

				#region factura.IssueTime - Hora de la factura
				/*Hora de emision de la factura*/
				IssueTimeType IssueTime = new IssueTimeType();
				IssueTime.Value = documento.Fecha.ToString(Fecha.formato_hora_completa);
				factura.IssueTime = IssueTime;
				#endregion

				#region Cuotas Factura
				if (documento.Cuotas != null && documento.Cuotas.Any())
				{
					factura.PaymentMeans = ObtenerCuotas(documento.Cuotas.ToList());
				}
				else
				{
					#region factura forma de pago.

					PaymentMeansType[] PaymentMeans = new PaymentMeansType[1];
					PaymentMeansType PaymentMean = new PaymentMeansType();
					PaymentMeansCodeType1 MeansCode = new PaymentMeansCodeType1();
					MeansCode.Value = "24";
					PaymentMean.PaymentMeansCode = MeansCode;
					#endregion

					#region factura.DueDate - Fecha vencimiento de la factura

					PaymentDueDateType DueDate = new PaymentDueDateType();
					DueDate.Value = Convert.ToDateTime(documento.FechaVence.ToString(Fecha.formato_fecha_hginet));
					PaymentMean.PaymentDueDate = DueDate;
					PaymentMeans[0] = PaymentMean;
					factura.PaymentMeans = PaymentMeans;
					#endregion
				}
				#endregion


				#region factura.InvoiceTypeCode - Tipo de Documento
				/*Indicar si es una factura de venta o una factura cambiaria de compraventa*/
				InvoiceTypeCodeType InvoiceTypeCode = new InvoiceTypeCodeType();
				InvoiceTypeCode.Value = "1";
				factura.InvoiceTypeCode = InvoiceTypeCode;
				#endregion

				#region Note - Nota adicional (Resolución texto)
				/*Notas adicionales informativas*/
				string prefijo = string.Empty;
				if (!string.IsNullOrEmpty(documento.Prefijo))
					prefijo = string.Format("{0}-", documento.Prefijo);

				//TblTransacciones transaccion = documento.TblTransacciones;

				string dian_resolucion = string.Format("Resolución Dian Nro {0} de {1} del {2}{3} al {4}{5}", resolucion.NumResolucion, resolucion.FechaResIni.ToString(Fecha.formato_fecha_hginet), prefijo, resolucion.RangoIni, prefijo, resolucion.RangoFin);

				NoteType[] Notes = new NoteType[1];
				NoteType Note = new NoteType();
				Note.Value = dian_resolucion;
				Notes[0] = Note;
				factura.Note = Notes;

				#endregion

				#region factura.DocumentCurrencyCode - Divisa de la Factura
				/*Divisa consolidada aplicable a toda la factura. Moneda con la que se presenta el documento*/
				DocumentCurrencyCodeType DocumentCurrencyCode = new DocumentCurrencyCodeType();
				DocumentCurrencyCode.Value = documento.Moneda;
				factura.DocumentCurrencyCode = DocumentCurrencyCode;
				#endregion


				#region período al que aplica el documento
				// <cac:InvoicePeriod>
				factura.InvoicePeriod = new PeriodType()
				{
					StartDate = new StartDateType()
					{
						Value = new DateTime(documento.Fecha.Date.Year, documento.Fecha.Date.Month, 1)
					},
					EndDate = new EndDateType()
					{
						Value = new DateTime(documento.Fecha.Date.Year, documento.Fecha.Date.Month, DateTime.DaysInMonth(documento.Fecha.Date.Year, documento.Fecha.Date.Month))
					}
				};
				#endregion

				#region factura.BillingReference //Referencia Documento (orden)

				//Referencia un documento
				factura.BillingReference = new BillingReferenceType[1];

				BillingReferenceType DocReference = new BillingReferenceType();
				DocumentReferenceType DocumentReference = new DocumentReferenceType();
				DocumentReference.ID = new IDType();
				DocumentReference.ID.Value = documento.DocumentoRef.ToString();
				DocReference.InvoiceDocumentReference = DocumentReference;

				factura.BillingReference[0] = DocReference;

				#endregion

				#region factura.AccountingSupplierParty - Información del obligado a facturar
				factura.AccountingSupplierParty = ObtenerObligado(documento.DatosObligado);
				#endregion

				#region factura.AccountingCustomerParty - Información del Adquiriente
				/* Persona natural o jurídica que adquiere bienes y/o servicios y debe exigir factura 
				51 o documento equivalente y, que tratándose de la factura electrónica, 
				la recibe, rechaza, cuando
				52 sea del caso, y conserva para su posterior exhibición*/
				//factura.AccountingCustomerParty = Aquiriente(documento.DatosTercero);

				#region factura.AccountingCustomerParty //Información del Adquiriente
				/* Persona natural o jurídica que adquiere bienes y/o servicios y debe exigir factura
				51 o documento equivalente y, que tratándose de la factura electrónica, 
			    la recibe, rechaza, cuando
				52 sea del caso, y conserva para su posterior exhibición*/

				factura.AccountingCustomerParty = ObtenerAquiriente(documento.DatosAdquiriente);
				#endregion


				#endregion

				#region factura.InvoiceLine - Línea de Factura
				/*Elemento que agrupa todos los campos de una línea de factura. Detalle del documento*/
				factura.InvoiceLine = ObtenerDetalleDocumento(documento.DocumentoDetalles.ToList(), documento.Moneda);
				#endregion

				#region	factura.TaxTotal - Impuesto y Impuesto Retenido
				/*Impuesto Retenido: Elemento raíz compuesto utilizado para informar de un impuesto	retenido. 
				 Impuesto: Elemento raíz compuesto utilizado para informar de un impuesto. */
				factura.TaxTotal = ObtenerImpuestos(documento.DocumentoDetalles.ToList(), documento.Moneda);

				#endregion

				#region factura.LegalMonetaryTotal //Datos Importes Totales
				/*Agrupación de campos relativos a los importes totales aplicables a la	factura. Estos importes son calculados teniendo
				en cuenta las líneas de factura y elementos a nivel de factura, como descuentos, cargos, impuestos, etc*/
				factura.LegalMonetaryTotal = ObtenerTotales(documento);

				#endregion

				#region factura.UUID //CUFE:Codigo Unico de Facturacion Electronica.
				/*  CUFE: Obligatorio si es factura nacional.
					Codigo Unico de Facturacion Electronica. Campo que verifica la integridad de la información
					recibida, es un campo generado por el sistema Numeración de facturación, está pendiente
					deFINir su contenido. Esta Encriptado. La factura electrónica tiene una firma electrónica
					basada en firma digital según la política de  firma propuesta. La integridad de la factura ya
					viene asegurada por la firma digital, no es	necesaria ninguna medida más para asegurar que
					ningún campo ha sido alterado*/

				if (string.IsNullOrEmpty(resolucion.ClaveTecnicaDIAN))
					throw new Exception("La clave técnica en la resolución de la DIAN es inválida para el documento.");

				UUIDType UUID = new UUIDType();
				string CUFE = CalcularCUFE(factura, resolucion.ClaveTecnicaDIAN);
				UUID.Value = CUFE;
				factura.UUID = UUID;
				#endregion

				#region factura.UBLExtensions
				XmlDocument doc = new XmlDocument();
				doc.LoadXml("<firma></firma>");

				UBLExtensionType[] UBLExtensions = new UBLExtensionType[2];

				#region Extension de la Dian
				UBLExtensionType UBLExtensionDian = new UBLExtensionType();
				UBLExtensionDian.ExtensionContent = ExtensionDian.Obtener(resolucion, tipo);
				UBLExtensions[0] = UBLExtensionDian;
				#endregion

				#region Extension de la firma
				UBLExtensionType UBLExtensionFirma = new UBLExtensionType();
				UBLExtensionFirma.ExtensionContent = doc.DocumentElement;
				UBLExtensions[1] = UBLExtensionFirma;
				#endregion

				factura.UBLExtensions = UBLExtensions;
				#endregion

				// convierte los datos del objeto en texto XML 
				StringBuilder txt_xml = ConvertirXml(factura, namespaces_xml);

				FacturaE_Documento xml_sin_firma = new FacturaE_Documento();
				xml_sin_firma.Documento = documento;
				xml_sin_firma.NombreXml = nombre_archivo_xml;
				xml_sin_firma.DocumentoXml = txt_xml;
				xml_sin_firma.CUFE = CUFE;

				return xml_sin_firma;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Calcula el codigo CUFE
		/// </summary>
		/// <param name="factura">Objeto de tipo InvoiceType que contiene la informacion de la factura</param>
		/// <param name="clave_tecnica">Clave técnica de la resolución</param>
		/// <returns>Texto con la encriptación del CUFE</returns>        
		private static string CalcularCUFE(InvoiceType factura, string clave_tecnica)
		{
			try
			{
				if (factura == null)
					throw new Exception("Los datos de la factura son inválidos.");
				if (string.IsNullOrWhiteSpace(clave_tecnica))
					throw new Exception("La clave técnica es inválida.");

				#region Documentación de la creación código CUFE
				/*
				NumFac = Número de factura.
				FecFac = Fecha de factura en formato (Java) YYYYmmddHHMMss. 
				ValFac = Valor Factura sin IVA, con punto decimal, con decimales a dos (2) dígitos, sin separadores de miles, ni símbolo pesos. 
				CodImp1 = 01 
				ValImp1 = Valor impuesto 01, con punto decimal, con decimales a dos (2) dígitos, sin separadores de miles, ni símbolo pesos. 
				CodImp2 = 02 
				ValImp2 = Valor impuesto 02, con punto decimal, con decimales a dos (2) dígitos, sin separadores de miles, ni símbolo pesos. 
				CodImp3 = 03 
				ValImp3 = Valor impuesto 03, con punto decimal, con decimales a dos (2) dígitos, sin separadores de miles, ni símbolo pesos.
				ValImp = Valor IVA, con punto decimal, con decimales a dos (2) dígitos, sin separadores de miles, ni símbolo pesos.
				NitOFE = NIT del Facturador Electrónico sin puntos ni guiones, sin digito de verificación. 
				TipAdq = tipo de adquiriente, de acuerdo con la tabla Tipos de documentos de identidad del «Anexo 001 Formato estándar XML de la Factura, notas débito y notas crédito electrónicos» 
				NumAdq = Número de identificación del adquirente sin puntos ni guiones, sin digito de verificación. 
				ClTec = Clave técnica del rango de facturación.
				CUFE = SHA-1(NumFac + FecFac + ValFac + CodImp1 + ValImp1 + CodImp2 + ValImp2 + CodImp3 +
							ValImp3 + ValImp + NitOFE + TipAdq + NumAdq + ClTec) 
			
				NumFac = /fe:Invoice/cbc:ID
				FecFac = sinSimbolos(/fe:Invoice/cbc:IssueDate + /fe:Invoice/cbc:IssueTime) formato AAAAMMDDHHMMSS
				i.e. año + mes + día + hora + minutos + segundos
				ValFac = /fe:Invoice/fe:LegalMonetaryTotal/cbc:LineExtensionAmount
				CodImp1 = /fe:Invoice/fe:TaxTotal[x]/fe:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID = 01
				ValImp1 = /fe:Invoice/fe:TaxTotal[x]/fe:TaxSubtotal/cbc:TaxAmount
				CodImp2 = /fe:Invoice/fe:TaxTotal[y]/fe:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID = 02
				ValImp2 = /fe:Invoice/fe:TaxTotal[y]/fe:TaxSubtotal/cbc:TaxAmount
				CodImp3 = /fe:Invoice/fe:TaxTotal[z]/fe:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID = 03
				ValImp3 = /fe:Invoice/fe:TaxTotal[z]/fe:TaxSubtotal/cbc:TaxAmount
				ValImp = /fe:Invoice/fe:LegalMonetaryTotal/cbc:PayableAmount
				NitOFE = /fe:Invoice/fe:AccountingSupplierParty/fe:Party/cac:PartyIdentification/cbc:ID
				TipAdq = /fe:Invoice/fe:AccountingCustomerParty/fe:Party/cac:PartyIdentification/cbc:ID/@schemeID
				NumAdq = /fe:Invoice/fe:AccountingCustomerParty/fe:Party/cac:PartyIdentification/cbc:ID
				ClTec = no está en el XML 			 
				*/
				#endregion

				#region Creación Código CUFE

				string codigo_impuesto = string.Empty;
				DateTime fecha = factura.IssueDate.Value;
				DateTime fecha_hora = Convert.ToDateTime(factura.IssueTime.Value);
				TimeSpan hora = new TimeSpan(fecha_hora.Hour, fecha_hora.Minute, fecha_hora.Second);
				fecha = fecha.Date + hora;

				string NumFac = factura.ID.Value;
				string FecFac = fecha.ToString(Fecha.formato_fecha_java);
				string ValFac = factura.LegalMonetaryTotal.LineExtensionAmount.Value.ToString();

				//Impuesto 1
				string CodImp1 = "01";
				decimal ValImp1 = 0.00M;

				//Impuesto 2
				string CodImp2 = "02";
				decimal ValImp2 = 0.00M;

				//Impuesto 3
				string CodImp3 = "03";
				decimal ValImp3 = 0.00M;

				for (int i = 0; i < factura.TaxTotal.Count(); i++)
				{
					for (int j = 0; j < factura.TaxTotal[i].TaxSubtotal.Count(); j++)
					{
						codigo_impuesto = factura.TaxTotal[i].TaxSubtotal[j].TaxCategory.TaxScheme.ID.Value;

						if (codigo_impuesto.Equals("01"))
						{
							ValImp1 += factura.TaxTotal[i].TaxSubtotal[j].TaxAmount.Value;
						}
						else if (codigo_impuesto.Equals("02"))
						{
							ValImp2 += factura.TaxTotal[i].TaxSubtotal[j].TaxAmount.Value;
						}
						else if (codigo_impuesto.Equals("03"))
						{
							ValImp3 += factura.TaxTotal[i].TaxSubtotal[j].TaxAmount.Value;
						}
					}
				}

				string ValImp = factura.LegalMonetaryTotal.PayableAmount.Value.ToString();

				string NitOFE = string.Empty;

				for (int contador_obligado_facturar = 0; contador_obligado_facturar < factura.AccountingSupplierParty.Party.PartyIdentification.Count(); contador_obligado_facturar++)
				{
					NitOFE = factura.AccountingSupplierParty.Party.PartyIdentification[contador_obligado_facturar].ID.Value;
				}

				string TipAdq = string.Empty;
				string NumAdq = string.Empty;

				for (int contador_adquiriente = 0; contador_adquiriente < factura.AccountingCustomerParty.Party.PartyTaxScheme.Count(); contador_adquiriente++)
				{
					TipAdq = factura.AccountingCustomerParty.Party.PartyIdentification[contador_adquiriente].ID.schemeID;
					NumAdq = factura.AccountingCustomerParty.Party.PartyIdentification[contador_adquiriente].ID.Value;
				}

				string cufe = NumFac
					+ FecFac
					+ ValFac.Replace(",", ".")
					+ CodImp1
					+ ValImp1.ToString().Replace(",", ".")
					+ CodImp2
					+ ValImp2.ToString().Replace(",", ".")
					+ CodImp3
					+ ValImp3.ToString().Replace(",", ".")
					+ ValImp.Replace(",", ".")
					+ NitOFE
					+ TipAdq
					+ NumAdq
					+ clave_tecnica
				;

				string cufe_encriptado = Encriptar.Encriptar_SHA1(cufe);
				return cufe_encriptado;
				#endregion
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Convierte el objeto en texto XML
		/// </summary>
		/// <param name="factura">Objeto de tipo InvoiceType que contiene la informacion de la factura</param>
		/// <param name="namespaces_xml">Namespaces</param>       
		/// <returns>Ruta donde se guardo el archivo XML</returns>     
		private static StringBuilder ConvertirXml(InvoiceType factura, XmlSerializerNamespaces namespaces_xml)
		{
			try
			{
				if (factura == null)
					throw new Exception("La factura es inválida.");

				if (namespaces_xml == null)
					throw new Exception("Los Namespaces son inválidos.");

				StringBuilder texto_xml = Xml.Convertir<InvoiceType>(factura, namespaces_xml);

				return texto_xml;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Llena el objeto del Obligado a facturar con los datos de la empresa
		/// </summary>
		/// <param name="empresa">Datos de la empresa</param>
		/// <returns>Objeto de tipo SupplierPartyType1</returns>
		private static SupplierPartyType1 ObtenerObligado(Tercero empresa)
		{
			try
			{
				if (empresa == null)
					throw new Exception("Los datos de la empresa son inválidos.");

				/* Tipo de persona en el ERP 
					 1 Natural
					 2 Jurídica                      
				   para la Dian
					 1 Persona jurídica
					 2 Persona natural             

				string dian_tipoPersona = string.Empty;
				if (empresa.TipoPersona == 1)
					dian_tipoPersona = "2";  //(LISTADO DE VALORES DEFINIDO POR LA DIAN)         
				else if (empresa.TipoPersona == 2)
					dian_tipoPersona = "1";  //(LISTADO DE VALORES DEFINIDO POR LA DIAN)

                */
				/* Tipo de regimen en el ERP 
					 1 Simplificado
					 2 Comun                  
					Para la Dian
					 0 Simplificado
					 2 Común                  

				string dian_regimen = string.Empty;
				if (empresa.Regimen == 1)
					dian_regimen = "0";  //(LISTADO DE VALORES DEFINIDO POR LA DIAN)
				else if (empresa.Regimen == 2)
					dian_regimen = "2";  //(LISTADO DE VALORES DEFINIDO POR LA DIAN)

				string dian_tipoDocumento = string.Empty;
				if (empresa.TipoIdentificacion.Equals("NI"))
					dian_tipoDocumento = "31";  //(LISTADO DE VALORES DEFINIDO POR LA DIAN)
				else if (empresa.TipoIdentificacion.Equals("CC"))
					dian_tipoDocumento = "13";  //(LISTADO DE VALORES DEFINIDO POR LA DIAN)*/

				// Datos del obligado a facturar
				SupplierPartyType1 AccountingSupplierParty = new SupplierPartyType1();
				PartyType1 Party = new PartyType1();

				#region Tipo de persona

				AdditionalAccountIDType AdditionalAccountID = new AdditionalAccountIDType();
				AdditionalAccountID.Value = empresa.TipoPersona.ToString();//Tipo de persona (LISTADO DE VALORES DEFINIDO POR LA DIAN)
				AccountingSupplierParty.AdditionalAccountID = AdditionalAccountID;
				#endregion

				#region Regimen
				PartyTaxSchemeType1[] PartyTaxSchemes = new PartyTaxSchemeType1[1];
				PartyTaxSchemeType1 PartyTaxScheme = new PartyTaxSchemeType1();
				TaxLevelCodeType TaxLevelCode = new TaxLevelCodeType();
				TaxLevelCode.Value = empresa.Regimen.ToString();
				PartyTaxScheme.TaxLevelCode = TaxLevelCode;

				TaxSchemeType TaxScheme = new TaxSchemeType();
				PartyTaxScheme.TaxScheme = TaxScheme;
				PartyTaxSchemes[0] = PartyTaxScheme;
				Party.PartyTaxScheme = PartyTaxSchemes;
				#endregion

				#region Documento y tipo de documento
				PartyIdentificationType[] PartyIdentifications = new PartyIdentificationType[1];
				PartyIdentificationType PartyIdentification = new PartyIdentificationType();
				IDType ID = new IDType();
				ID.schemeAgencyName = "CO, DIAN (Direccion de Impuestos y Aduanas Nacionales)";
				ID.schemeAgencyID = "195";
				ID.schemeID = empresa.TipoIdentificacion.ToString(); //Tipo documento (LISTADO DE VALORES DEFINIDO POR LA DIAN)
				ID.Value = empresa.Identificacion.ToString();
				PartyIdentification.ID = ID;
				PartyIdentifications[0] = PartyIdentification;
				Party.PartyIdentification = PartyIdentifications;
				#endregion

				#region Datos de la empresa

				PartyLegalEntityType1[] PartyLegalEntitys = new PartyLegalEntityType1[1];
				PartyLegalEntityType1 PartyLegalEntity = new PartyLegalEntityType1();
				RegistrationNameType RegistrationName = new RegistrationNameType();
				RegistrationName.Value = empresa.RazonSocial;
				PartyLegalEntity.RegistrationName = RegistrationName;
				PartyLegalEntitys[0] = PartyLegalEntity;
				Party.PartyLegalEntity = PartyLegalEntitys;

				#endregion

				#region Dirección
				LocationType2 PhysicalLocation = new LocationType2();
				AddressType1 Address = new AddressType1();

				CityNameType City = new CityNameType();
				City.Value = empresa.Ciudad; //Ciudad (LISTADO DE VALORES DEFINIDO POR LA DIAN)
				Address.CityName = City;

				DepartmentType Department = new DepartmentType();
				Department.Value = empresa.Departamento; //Departamento (LISTADO DE VALORES DEFINIDO POR LA DIAN)
				Address.Department = Department;

				AddressLineType[] AddressLines = new AddressLineType[1];
				AddressLineType AddressLine = new AddressLineType();
				LineType Line = new LineType();
				Line.Value = empresa.Direccion;
				AddressLine.Line = Line;

				AddressLines[0] = AddressLine;
				Address.AddressLine = AddressLines;

				ContactType Contact = new ContactType();
				TelephoneType Telephone = new TelephoneType();
				Telephone.Value = empresa.Telefono;
				Contact.Telephone = Telephone;
				ElectronicMailType Mail = new ElectronicMailType();
				Mail.Value = empresa.Email;
				Contact.ElectronicMail = Mail;
				Party.Contact = Contact;

				WebsiteURIType Web = new WebsiteURIType();
				Web.Value = empresa.PaginaWeb;
				Party.WebsiteURI = Web;

				CountryType Country = new CountryType();
				IdentificationCodeType IdentificationCode = new IdentificationCodeType();
				IdentificationCode.Value = empresa.CodigoPais; //Pais (LISTADO DE VALORES DEFINIDO POR LA DIAN)
				Country.IdentificationCode = IdentificationCode;
				Address.Country = Country;

				PhysicalLocation.Address = Address;
				Party.PhysicalLocation = PhysicalLocation;
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
		private static CustomerPartyType1 ObtenerAquiriente(Tercero tercero)
		{
			try
			{
				if (tercero == null)
					throw new Exception("Los datos del tercero son inválidos.");


				/* Tipo de persona en el ERP 
					 1 Natural
					 2 Jurídica                      
				   para la Dian
					 1 Persona jurídica
					 2 Persona natural             

				string dian_tipoPersona = string.Empty;
				if (tercero.TipoPersona == 1)
					dian_tipoPersona = "2";  //(LISTADO DE VALORES DEFINIDO POR LA DIAN)         
				else if (tercero.TipoPersona == 2)
					dian_tipoPersona = "1";  //(LISTADO DE VALORES DEFINIDO POR LA DIAN)*/

				/* Tipo de regimen en el ERP 
					 1 Simplificado
					 2 Comun                  
					Para la Dian
					 0 Simplificado
					 2 Común                 

				string dian_regimen = string.Empty;
				if (tercero.Regimen == 1)
					dian_regimen = "0";  //(LISTADO DE VALORES DEFINIDO POR LA DIAN)
				else if (tercero.Regimen == 2)
					dian_regimen = "2";  //(LISTADO DE VALORES DEFINIDO POR LA DIAN)

				string dian_tipoDocumento = string.Empty;
				if (tercero.TipoIdentificacion.Equals("NI"))
					dian_tipoDocumento = "31";  //(LISTADO DE VALORES DEFINIDO POR LA DIAN)
				else if (tercero.TipoIdentificacion.Equals("CC"))
					dian_tipoDocumento = "13";  //(LISTADO DE VALORES DEFINIDO POR LA DIAN)
				else if (tercero.TipoIdentificacion.Equals("CE"))
					dian_tipoDocumento = "22";  //(LISTADO DE VALORES DEFINIDO POR LA DIAN) */

				// Datos del adquiriente de la factura
				CustomerPartyType1 AccountingCustomerParty = new CustomerPartyType1();
				PartyType1 Party = new PartyType1();

				#region Tipo de persona
				AdditionalAccountIDType AdditionalAccountID = new AdditionalAccountIDType();
				AdditionalAccountID.Value = tercero.TipoPersona.ToString();//Tipo de persona (LISTADO DE VALORES DEFINIDO POR LA DIAN)
				AccountingCustomerParty.AdditionalAccountID = AdditionalAccountID;
				#endregion

				#region Regimen
				PartyTaxSchemeType1[] PartyTaxSchemes = new PartyTaxSchemeType1[1];
				PartyTaxSchemeType1 PartyTaxScheme = new PartyTaxSchemeType1();
				TaxLevelCodeType TaxLevelCode = new TaxLevelCodeType();
				TaxLevelCode.Value = tercero.Regimen.ToString();
				PartyTaxScheme.TaxLevelCode = TaxLevelCode;

				TaxSchemeType TaxScheme = new TaxSchemeType();
				PartyTaxScheme.TaxScheme = TaxScheme;
				PartyTaxSchemes[0] = PartyTaxScheme;
				Party.PartyTaxScheme = PartyTaxSchemes;
				#endregion

				#region Documento y tipo de documento
				PartyIdentificationType[] PartyIdentifications = new PartyIdentificationType[1];
				PartyIdentificationType PartyIdentification = new PartyIdentificationType();
				IDType ID = new IDType();
				ID.schemeAgencyName = "CO, DIAN (Direccion de Impuestos y Aduanas Nacionales)";
				ID.schemeAgencyID = "195";
				ID.schemeID = tercero.TipoIdentificacion.ToString(); //Tipo documento (LISTADO DE VALORES DEFINIDO POR LA DIAN)
				ID.Value = tercero.Identificacion.ToString();
				PartyIdentification.ID = ID;
				PartyIdentifications[0] = PartyIdentification;
				Party.PartyIdentification = PartyIdentifications;
				#endregion

				#region Datos personales segun el tipo de persona
				if (tercero.TipoPersona.Equals(2))//Persona natural
				{
					PersonType1 Person = new PersonType1();

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

					Party.Person = Person;
				}
				else if (tercero.TipoPersona.Equals(1)) //Persona juridica
				{
					PartyLegalEntityType1[] PartyLegalEntitys = new PartyLegalEntityType1[1];
					PartyLegalEntityType1 PartyLegalEntity = new PartyLegalEntityType1();
					RegistrationNameType RegistrationName = new RegistrationNameType();
					RegistrationName.Value = tercero.RazonSocial;
					PartyLegalEntity.RegistrationName = RegistrationName;
					PartyLegalEntitys[0] = PartyLegalEntity;

					Party.PartyLegalEntity = PartyLegalEntitys;
				}
				#endregion

				#region Dirección
				LocationType2 PhysicalLocation = new LocationType2();
				AddressType1 Address = new AddressType1();

				AddressLineType[] AddressLines = new AddressLineType[1];
				AddressLineType AddressLine = new AddressLineType();
				LineType Line = new LineType();
				Line.Value = tercero.Direccion;
				AddressLine.Line = Line;

				CityNameType City = new CityNameType();
				City.Value = tercero.Ciudad; //Ciudad (LISTADO DE VALORES DEFINIDO POR LA DIAN)
				Address.CityName = City;

				DepartmentType Department = new DepartmentType();
				Department.Value = tercero.Departamento; //Departamento (LISTADO DE VALORES DEFINIDO POR LA DIAN)
				Address.Department = Department;


				CountryType Country = new CountryType();
				IdentificationCodeType IdentificationCode = new IdentificationCodeType();
				IdentificationCode.Value = tercero.CodigoPais; //Pais (LISTADO DE VALORES DEFINIDO POR LA DIAN)
				Country.IdentificationCode = IdentificationCode;
				Address.Country = Country;

				AddressLines[0] = AddressLine;
				Address.AddressLine = AddressLines;

				ContactType Contact = new ContactType();
				TelephoneType Telephone = new TelephoneType();
				Telephone.Value = tercero.Telefono;
				Contact.Telephone = Telephone;
				ElectronicMailType Mail = new ElectronicMailType();
				Mail.Value = tercero.Email;
				Contact.ElectronicMail = Mail;
				Party.Contact = Contact;

				WebsiteURIType Web = new WebsiteURIType();
				Web.Value = tercero.PaginaWeb;
				Party.WebsiteURI = Web;

				PhysicalLocation.Address = Address;
				Party.PhysicalLocation = PhysicalLocation;
				#endregion

				AccountingCustomerParty.Party = Party;

				return AccountingCustomerParty;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Llena el objeto del detalle de la factura con los datos documento detalle
		/// </summary>
		/// <param name="DocumentoDetalle">Datos del detalle del documento</param>
		/// <returns>Objeto de tipo InvoiceLineType1</returns>
		private static InvoiceLineType1[] ObtenerDetalleDocumento(List<DocumentoDetalle> documentoDetalle, string moneda)
		{
			try
			{
				if (documentoDetalle == null || !documentoDetalle.Any())
					throw new Exception("El detalle del documento es inválido.");

				InvoiceType factura = new InvoiceType();
				InvoiceLineType1[] InvoicesLineType1 = new InvoiceLineType1[documentoDetalle.Count()];

				int contadorPosicion = 0;
				int contadorProducto = 1;

				foreach (var DocDet in documentoDetalle)
				{
					CurrencyCodeContentType moneda_detalle = Ctl_Enumeracion.ObtenerMoneda(moneda);

					if (string.IsNullOrEmpty(DocDet.UnidadCodigo))
						DocDet.UnidadCodigo = "S7";

					// <fe:InvoiceLine>
					// http://www.datypic.com/sc/ubl21/e-cac_InvoiceLine.html
					InvoiceLineType1 InvoiceLineType1 = new InvoiceLineType1();


					#region Id producto definido por la Dian (Contador de productos iniciando desde 1)
					// <cbc:ID>
					IDType ID = new IDType();
					ID.Value = contadorProducto.ToString();
					InvoiceLineType1.ID = ID;
					#endregion


					#region Cantidad producto
					// <cbc:InvoicedQuantity>
					InvoicedQuantityType InvoicedQuantity = new InvoicedQuantityType();
					InvoicedQuantity.Value = decimal.Round(DocDet.Cantidad, 2);

					// Unidad de medida
					InvoicedQuantity.unitCode = Ctl_Enumeracion.ObtenerUnidadMedida(DocDet.UnidadCodigo);
					InvoiceLineType1.InvoicedQuantity = InvoicedQuantity;
					#endregion


					#region Valor Total
					// <cbc:LineExtensionAmount>
					LineExtensionAmountType LineExtensionAmount = new LineExtensionAmountType();
					LineExtensionAmount.currencyID = moneda_detalle;
					decimal valorTotal = DocDet.Cantidad * DocDet.ValorUnitario;
					LineExtensionAmount.Value = decimal.Round(valorTotal, 2);
					InvoiceLineType1.LineExtensionAmount = LineExtensionAmount;
					#endregion


					#region Producto gratuito 
					// indica que la línea de factura es gratuita (verdadera) o no (falsa)
					// <cbc:FreeOfChargeIndicator>
					FreeOfChargeIndicatorType FreeOfChargeIndicator = new FreeOfChargeIndicatorType();
					FreeOfChargeIndicator.Value = DocDet.ProductoGratis;
					InvoiceLineType1.FreeOfChargeIndicator = FreeOfChargeIndicator;
					#endregion


					#region Cargo adicional ¿?
					// <cac:AllowanceCharge>
					AllowanceChargeType[] AllowanceCharges = new AllowanceChargeType[1];
					AllowanceChargeType AllowanceCharge = new AllowanceChargeType();
					AllowanceCharge.ChargeIndicator = new ChargeIndicatorType();
					AllowanceCharge.ChargeIndicator.Value = false;
					AllowanceCharge.Amount = new AmountType1();
					AllowanceCharge.Amount.currencyID = moneda_detalle;
					AllowanceCharge.Amount.Value = decimal.Round(0M, 2);
					AllowanceCharges[0] = AllowanceCharge;
					InvoiceLineType1.AllowanceCharge = AllowanceCharges;
					#endregion


					#region Impuestos del producto
					// <cac:TaxTotal>
					TaxTotalType[] TaxesTotal = new TaxTotalType[1];

					TaxTotalType TaxTotal = new TaxTotalType();

					// importe total de impuestos, por ejemplo, IVA; la suma de los subtotales fiscales para cada categoría de impuestos dentro del esquema impositivo
					// <cbc:TaxAmount>
					TaxTotal.TaxAmount = new TaxAmountType()
					{
						currencyID = moneda_detalle,
						Value = decimal.Round(DocDet.IvaValor + DocDet.ValorImpuestoConsumo + DocDet.ReteFuenteValor + DocDet.ReteIcaValor, 0)
					};

					// indicador que este total se reconoce como evidencia legal a efectos impositivos (verdadero)o no(falso).
					// <cbc:TaxEvidenceIndicator>
					TaxTotal.TaxEvidenceIndicator = new TaxEvidenceIndicatorType()
					{
						Value = false
					};

					// subtotales cuya suma es igual a la cantidad total de impuestos para un régimen impositivo particular.
					// http://www.datypic.com/sc/ubl21/e-cac_TaxSubtotal.html
					// <cac:TaxSubtotal>
					TaxSubtotalType[] TaxesSubtotal = new TaxSubtotalType[4];


					#region impuesto: IVA 
					TaxSubtotalType TaxSubtotalIva = new TaxSubtotalType();

					// importe neto al que se aplica el porcentaje del impuesto (tasa) para calcular el importe del impuesto.
					// <cbc:TaxAmount>
					TaxSubtotalIva.TaxableAmount = new TaxableAmountType()
					{
						currencyID = moneda_detalle,
						Value = DocDet.ValorSubtotal
					};

					// El monto de este subtotal fiscal.
					// <cbc:TaxAmount>
					TaxSubtotalIva.TaxAmount = new TaxAmountType()
					{
						currencyID = moneda_detalle,
						Value = DocDet.IvaValor
					};

					// tasa de impuesto de la categoría de impuestos aplicada a este subtotal fiscal, expresada como un porcentaje.
					// <cbc:Percent>
					TaxSubtotalIva.Percent = new PercentType()
					{
						Value = decimal.Round((DocDet.IvaPorcentaje * 100), 2)
					};

					// categoría de impuestos aplicable a este subtotal.
					// <cac:TaxCategory>
					TaxCategoryType TaxCategoryIva = new TaxCategoryType();

					// <cac:TaxScheme>
					TaxSchemeType TaxSchemeIva = new TaxSchemeType()
					{
						ID = new IDType()
						{
							Value = TipoImpuestos.Iva
						},

						TaxTypeCode = new TaxTypeCodeType()
						{
							Value = TipoImpuestos.Iva
						}
					};

					TaxCategoryIva.TaxScheme = TaxSchemeIva;
					TaxSubtotalIva.TaxCategory = TaxCategoryIva;
					TaxesSubtotal[0] = TaxSubtotalIva;
					#endregion


					#region impuesto: Consumo 
					TaxSubtotalType TaxSubtotalConsumo = new TaxSubtotalType();

					// importe neto al que se aplica el porcentaje del impuesto (tasa) para calcular el importe del impuesto.
					// <cbc:TaxAmount>
					TaxSubtotalConsumo.TaxableAmount = new TaxableAmountType()
					{
						currencyID = moneda_detalle,
						Value = DocDet.ValorSubtotal
					};

					// El monto de este subtotal fiscal.
					// <cbc:TaxAmount>
					TaxSubtotalConsumo.TaxAmount = new TaxAmountType()
					{
						currencyID = moneda_detalle,
						Value = DocDet.ValorImpuestoConsumo
					};

					// tasa de impuesto de la categoría de impuestos aplicada a este subtotal fiscal, expresada como un porcentaje.
					// <cbc:Percent>
					TaxSubtotalConsumo.Percent = new PercentType()
					{
						Value = decimal.Round((DocDet.ImpoConsumoPorcentaje * 100), 2)
					};

					// categoría de impuestos aplicable a este subtotal.
					// <cac:TaxCategory>
					TaxCategoryType TaxCategoryConsumo = new TaxCategoryType();

					// <cac:TaxScheme>
					TaxSchemeType TaxSchemeConsumo = new TaxSchemeType()
					{
						ID = new IDType()
						{
							Value = TipoImpuestos.Consumo
						},

						TaxTypeCode = new TaxTypeCodeType()
						{
							Value = TipoImpuestos.Consumo
						}
					};

					TaxCategoryConsumo.TaxScheme = TaxSchemeConsumo;
					TaxSubtotalConsumo.TaxCategory = TaxCategoryConsumo;
					TaxesSubtotal[1] = TaxSubtotalConsumo;
					#endregion


					#region impuesto: Ica  
					TaxSubtotalType TaxSubtotalIca = new TaxSubtotalType();

					// importe neto al que se aplica el porcentaje del impuesto (tasa) para calcular el importe del impuesto.
					// <cbc:TaxAmount>
					TaxSubtotalIca.TaxableAmount = new TaxableAmountType()
					{
						currencyID = moneda_detalle,
						Value = DocDet.ValorSubtotal
					};

					// El monto de este subtotal fiscal.
					// <cbc:TaxAmount>
					TaxSubtotalIca.TaxAmount = new TaxAmountType()
					{
						currencyID = moneda_detalle,
						Value = DocDet.ReteIcaValor
					};

					// tasa de impuesto de la categoría de impuestos aplicada a este subtotal fiscal, expresada como un porcentaje.
					// <cbc:Percent>
					TaxSubtotalIca.Percent = new PercentType()
					{
						Value = decimal.Round((DocDet.ReteIcaPorcentaje * 100), 2)
					};

					// categoría de impuestos aplicable a este subtotal.
					// <cac:TaxCategory>
					TaxCategoryType TaxCategoryIca = new TaxCategoryType();

					// <cac:TaxScheme>
					TaxSchemeType TaxSchemeIca = new TaxSchemeType()
					{
						ID = new IDType()
						{
							Value = TipoImpuestos.Ica
						},

						TaxTypeCode = new TaxTypeCodeType()
						{
							Value = TipoImpuestos.Ica
						}
					};

					TaxCategoryIca.TaxScheme = TaxSchemeIca;
					TaxSubtotalIca.TaxCategory = TaxCategoryIca;
					TaxesSubtotal[2] = TaxSubtotalIca;
					#endregion


					#region impuesto: Retención en la Fuente 
					TaxSubtotalType TaxSubtotalRteFte = new TaxSubtotalType();

					// importe neto al que se aplica el porcentaje del impuesto (tasa) para calcular el importe del impuesto.
					// <cbc:TaxAmount>
					TaxSubtotalRteFte.TaxableAmount = new TaxableAmountType()
					{
						currencyID = moneda_detalle,
						Value = DocDet.ValorSubtotal
					};

					// El monto de este subtotal fiscal.
					// <cbc:TaxAmount>
					TaxSubtotalRteFte.TaxAmount = new TaxAmountType()
					{
						currencyID = moneda_detalle,
						Value = DocDet.ReteFuenteValor
					};

					// tasa de impuesto de la categoría de impuestos aplicada a este subtotal fiscal, expresada como un porcentaje.
					// <cbc:Percent>
					TaxSubtotalRteFte.Percent = new PercentType()
					{
						Value = decimal.Round((DocDet.ReteFuentePorcentaje * 100), 2)
					};

					// categoría de impuestos aplicable a este subtotal.
					// <cac:TaxCategory>
					TaxCategoryType TaxCategoryRteFte = new TaxCategoryType();

					// <cac:TaxScheme>
					TaxSchemeType TaxSchemeRteFte = new TaxSchemeType()
					{
						ID = new IDType()
						{
							Value = TipoImpuestos.ReteFte // OJO !!!!!
						},

						TaxTypeCode = new TaxTypeCodeType()
						{
							Value = TipoImpuestos.ReteFte // OJO !!!!!
						}
					};

					TaxCategoryRteFte.TaxScheme = TaxSchemeRteFte;
					TaxSubtotalRteFte.TaxCategory = TaxCategoryRteFte;
					TaxesSubtotal[3] = TaxSubtotalRteFte;
					#endregion



					TaxTotal.TaxSubtotal = TaxesSubtotal;
					TaxesTotal[0] = TaxTotal;
					InvoiceLineType1.TaxTotal = TaxesTotal;

					#endregion


					#region Datos producto
					// <fe:Item>
					ItemType1 Item = new ItemType1();

					#region Nombre producto
					// <cbc:Description>
					DescriptionType[] Descriptions = new DescriptionType[1];
					DescriptionType Description = new DescriptionType();
					Description.Value = DocDet.ProductoNombre;
					Descriptions[0] = Description;
					Item.Description = Descriptions;
					#endregion

					#region Descripcion producto
					// 
					AdditionalInformationType Additional = new AdditionalInformationType();
					Additional.Value = DocDet.ProductoDescripcion;
					Item.AdditionalInformation = Additional;
					#endregion

					#region Id producto definido por la Empresa
					// <cac:CatalogueItemIdentification>
					ItemIdentificationType CatalogueItemIdentification = new ItemIdentificationType();
					IDType IDItem = new IDType();
					IDItem.Value = DocDet.ProductoCodigo;
					CatalogueItemIdentification.ID = IDItem;
					Item.CatalogueItemIdentification = CatalogueItemIdentification;

					// <cac:StandardItemIdentification>
					ItemIdentificationType StandardItemIdentification = new ItemIdentificationType();
					IDType IDItemStandard = new IDType();
					IDItemStandard.Value = DocDet.ProductoCodigo;
					StandardItemIdentification.ID = IDItemStandard;
					Item.StandardItemIdentification = StandardItemIdentification;
					#endregion

					InvoiceLineType1.Item = Item;
					#endregion


					#region Valor Unitario producto
					// <fe:Price>
					PriceType1 Price = new PriceType1();

					// <cbc:PriceAmount>
					PriceAmountType PriceAmount = new PriceAmountType();
					PriceAmount.currencyID = moneda_detalle;
					PriceAmount.Value = decimal.Round(DocDet.ValorUnitario, 2);
					Price.PriceAmount = PriceAmount;
					InvoiceLineType1.Price = Price;
					#endregion


					InvoicesLineType1[contadorPosicion] = InvoiceLineType1;
					contadorProducto++;
					contadorPosicion++;
				}

				factura.InvoiceLine = InvoicesLineType1;
				return factura.InvoiceLine;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}

		}

		/// <summary>
		/// Llena el objeto de los impuestos de la factura con los datos documento detalle
		/// </summary>
		/// <param name="documentoDetalle">Datos del detalle del documento</param>
		/// <returns>Objeto de tipo TaxTotalType1</returns>
		private static TaxTotalType1[] ObtenerImpuestos(List<DocumentoDetalle> documentoDetalle, string moneda)
		{
			try
			{
				if (documentoDetalle == null || documentoDetalle.Count == 0)
					throw new Exception("El detalle del documento es inválido.");

				//Toma los impuestos de IVA que tiene el producto en el detalle del documento
				var impuestos_iva = documentoDetalle.Select(_impuesto => new { _impuesto.IvaPorcentaje, TipoImpuestos.Iva, _impuesto.IvaValor }).GroupBy(_impuesto => new { _impuesto.IvaPorcentaje }).Select(_impuesto => _impuesto.First());

				List<DocumentoImpuestos> doc_impuestos = new List<DocumentoImpuestos>();

				decimal BaseImponibleImpuesto = 0;


				// moneda del primer detalle
				CurrencyCodeContentType moneda_detalle = Ctl_Enumeracion.ObtenerMoneda(moneda);

				foreach (var item in impuestos_iva)
				{
					DocumentoImpuestos imp_doc = new DocumentoImpuestos();
					List<DocumentoDetalle> doc_ = documentoDetalle.Where(docDet => docDet.IvaPorcentaje == item.IvaPorcentaje).ToList();
					BaseImponibleImpuesto = decimal.Round(documentoDetalle.Where(docDet => docDet.IvaPorcentaje == item.IvaPorcentaje).Sum(docDet => docDet.ValorUnitario * docDet.Cantidad), 2);

					//imp_doc.Codigo = item.IntIva;
					//imp_doc.Nombre = item.StrDescripcion;
					imp_doc.Porcentaje = decimal.Round(item.IvaPorcentaje * 100, 2);
					imp_doc.TipoImpuesto = item.Iva;
					imp_doc.BaseImponible = BaseImponibleImpuesto;

					foreach (var docDet in doc_)
					{
						imp_doc.ValorImpuesto = decimal.Round(imp_doc.ValorImpuesto + docDet.IvaValor, 2);
					}
					doc_impuestos.Add(imp_doc);
				}

				//Toma el impuesto al consumo de los productos que esten el detalle
				var impuesto_consumo = documentoDetalle.Select(_consumo => new { _consumo.ImpoConsumoPorcentaje, TipoImpuestos.Consumo, _consumo.ValorImpuestoConsumo }).GroupBy(_consumo => new { _consumo.ImpoConsumoPorcentaje }).Select(_consumo => _consumo.First());
				decimal BaseImponibleImpConsumo = 0;



				if (impuesto_consumo.Count() > 0)
				{
					foreach (var item in impuesto_consumo)
					{
						//Valida si hay algun producto con impuesto al consumo
						if (item.ValorImpuestoConsumo != 0)
						{
							DocumentoImpuestos imp_doc = new DocumentoImpuestos();
							List<DocumentoDetalle> doc_ = documentoDetalle.Where(docDet => docDet.ValorImpuestoConsumo != 0).ToList();
							BaseImponibleImpConsumo = decimal.Round(documentoDetalle.Where(docDet => docDet.ValorImpuestoConsumo != 0).Sum(docDet => docDet.ValorUnitario * docDet.Cantidad), 2);

							//imp_doc.Codigo = item.IntImpConsumo.ToString();
							//imp_doc.Nombre = item.StrDescripcion;
							imp_doc.Porcentaje = decimal.Round(item.ValorImpuestoConsumo * 100, 2);
							imp_doc.TipoImpuesto = item.Consumo;
							imp_doc.BaseImponible = BaseImponibleImpConsumo;
							foreach (var docDet in doc_)
							{
								imp_doc.ValorImpuesto = decimal.Round(imp_doc.ValorImpuesto + docDet.ValorImpuestoConsumo, 2);
							}
							doc_impuestos.Add(imp_doc);
						}

					}
				}

				//Toma el ReteICA de los productos que esten el detalle
				var impuesto_ica = documentoDetalle.Select(_ica => new { _ica.ReteIcaPorcentaje, TipoImpuestos.Ica, _ica.ReteIcaValor }).GroupBy(_ica => new { _ica.ReteIcaPorcentaje }).Select(_ica => _ica.First());
				decimal BaseImponibleReteIca = 0;


				if (impuesto_ica.Count() > 0)
				{
					foreach (var item in impuesto_ica)
					{
						//Valida si hay algun producto con ReteICA
						if (item.ReteIcaValor != 0)
						{
							DocumentoImpuestos imp_doc = new DocumentoImpuestos();
							List<DocumentoDetalle> doc_ = documentoDetalle.Where(docDet => docDet.ReteIcaValor != 0).ToList();
							BaseImponibleReteIca = decimal.Round(documentoDetalle.Where(docDet => docDet.ReteIcaValor != 0).Sum(docDet => docDet.ValorUnitario * docDet.Cantidad), 2);

							imp_doc.Porcentaje = decimal.Round(item.ReteIcaPorcentaje * 100, 2);
							imp_doc.TipoImpuesto = item.Ica;
							imp_doc.BaseImponible = BaseImponibleReteIca;
							foreach (var docDet in doc_)
							{
								imp_doc.ValorImpuesto = decimal.Round(imp_doc.ValorImpuesto + docDet.ReteIcaValor, 2);
							}
							doc_impuestos.Add(imp_doc);
						}

					}

				}



				var retencion = documentoDetalle.Select(_retencion => new { _retencion.ReteFuentePorcentaje, TipoImpuestos.ReteFte, _retencion.ReteFuenteValor }).GroupBy(_retencion => new { _retencion.ReteFuentePorcentaje }).Select(_retencion => _retencion.First());
				decimal BaseImponibleRetencion = 0;


				if (retencion.Count() > 0)
				{
					//TblTerceros tercero = new TblTerceros();

					foreach (var item in retencion)
					{
						if (item.ReteFuenteValor != 0)
						{
							DocumentoImpuestos imp_doc = new DocumentoImpuestos();
							List<DocumentoDetalle> doc_ = documentoDetalle.Where(docDet => docDet.ReteFuenteValor != 0).ToList();
							BaseImponibleRetencion = decimal.Round(documentoDetalle.Where(docDet => docDet.ReteFuenteValor != 0).Sum(docDet => docDet.ValorUnitario * docDet.Cantidad), 2);

							imp_doc.Porcentaje = decimal.Round(item.ReteFuentePorcentaje * 100, 2);
							imp_doc.TipoImpuesto = item.ReteFte;
							imp_doc.BaseImponible = BaseImponibleRetencion;
							foreach (var docDet in doc_)
							{
								imp_doc.ValorImpuesto = decimal.Round(imp_doc.ValorImpuesto + docDet.ReteFuenteValor, 2);
							}

							doc_impuestos.Add(imp_doc);
						}
					}

				}


				TaxTotalType1[] TaxTotals = new TaxTotalType1[doc_impuestos.Count];

				int contador = 0;
				foreach (var item in doc_impuestos)
				{

					TaxTotalType1 TaxTotal = new TaxTotalType1();

					#region Importe Impuesto: Importe del impuesto retenido
					TaxAmountType TaxAmount = new TaxAmountType();
					TaxAmount.currencyID = moneda_detalle;
					TaxAmount.Value = decimal.Round(item.ValorImpuesto, 2);
					TaxTotal.TaxAmount = TaxAmount;
					#endregion

					#region Impuesto Legal ***PENDIENTE
					// Indicador de si estos totales se reconocen como evidencia legal a efectos impositivos.
					TaxEvidenceIndicatorType TaxEvidenceIndicator = new TaxEvidenceIndicatorType();
					TaxEvidenceIndicator.Value = false;
					TaxTotal.TaxEvidenceIndicator = TaxEvidenceIndicator;
					#endregion

					TaxSubtotalType1[] TaxSubtotals = new TaxSubtotalType1[1];
					TaxSubtotalType1 TaxSubtotal = new TaxSubtotalType1();

					#region Base Imponible: Base	Imponible sobre la que se calcula la retención de impuesto
					//Base Imponible = Importe bruto + cargos - descuentos
					TaxableAmountType TaxableAmount = new TaxableAmountType();
					TaxableAmount.currencyID = moneda_detalle;
					TaxableAmount.Value = decimal.Round(item.BaseImponible, 2);
					TaxSubtotal.TaxableAmount = TaxableAmount;
					#endregion

					#region Importe Impuesto (detalle): Importe del impuesto retenido
					//Valor total del impuesto retenido
					TaxAmountType TaxAmountSubtotal = new TaxAmountType();
					TaxAmountSubtotal.currencyID = moneda_detalle;
					TaxAmountSubtotal.Value = decimal.Round(item.ValorImpuesto, 2);
					TaxSubtotal.TaxAmount = TaxAmountSubtotal;
					#endregion

					#region Porcentaje: Porcentaje a aplicar
					PercentType Percent = new PercentType();
					Percent = new PercentType();
					Percent.Value = item.Porcentaje;
					TaxSubtotal.Percent = Percent;
					#endregion

					#region Tipo o clase impuesto
					/* Tipo o clase impuesto. Concepto fiscal por el que se tributa. Debería si un	campo que referencia a una lista de códigos. En
					   la lista deberían aparecer los impuestos	estatales o nacionales*/
					TaxCategoryType TaxCategory = new TaxCategoryType();
					TaxSchemeType TaxScheme = new TaxSchemeType();
					IDType IDTaxScheme = new IDType();
					IDTaxScheme.Value = TipoImpuestos.Iva;//(LISTADO DE VALORES DEFINIDO POR LA DIAN)
					TaxScheme.ID = IDTaxScheme;

					NameType1 Name = new NameType1();
					Name.Value = item.Porcentaje.ToString();

					TaxScheme.Name = Name;
					TaxCategory.TaxScheme = TaxScheme;
					TaxSubtotal.TaxCategory = TaxCategory;
					#endregion

					TaxSubtotals[0] = TaxSubtotal;
					TaxTotal.TaxSubtotal = TaxSubtotals;
					TaxTotals[contador] = TaxTotal;
					contador++;
				}

				return TaxTotals;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Llena el objeto de las cuotas de la factura con los datos
		/// </summary>
		/// <param name="cuota">Datos de las cuotas</param>
		/// <returns>Objeto tipo PaymentMeansType</returns>
		private static PaymentMeansType[] ObtenerCuotas(List<Cuota> cuota)
		{

			PaymentMeansType[] PaymentMeans = new PaymentMeansType[cuota.Count];

			int contador = 0;
			foreach (Cuota item in cuota)
			{

				PaymentMeansType PaymentMean = new PaymentMeansType();

				IDType ID = new IDType();
				ID.Value = item.Codigo.ToString();
				PaymentMean.ID = ID;

				PaymentMeansCodeType1 MeansCode = new PaymentMeansCodeType1();
				MeansCode.Value = "24";
				PaymentMean.PaymentMeansCode = MeansCode;

				InstructionNoteType[] Instructions = new InstructionNoteType[1];
				InstructionNoteType Instruction = new InstructionNoteType();
				Instruction.Value = item.Valor.ToString();
				Instructions[0] = Instruction;
				PaymentMean.InstructionNote = Instructions;

				PaymentDueDateType DueDate = new PaymentDueDateType();
				DueDate.Value = Convert.ToDateTime(item.FechaVence.ToString(Fecha.formato_fecha_hginet));
				PaymentMean.PaymentDueDate = DueDate;
				PaymentMeans[contador] = PaymentMean;
				contador++;


			}


			return PaymentMeans;
		}

		/// <summary>
		/// Obtiene los valores del encabezado del documento
		/// </summary>
		/// <param name="documento">datos del documento</param>
		/// <returns>MonetaryTotalType1</returns>
		private static MonetaryTotalType1 ObtenerTotales(Factura documento)
		{
			try
			{
				/*
					http://www.sfti.se/download/18.1498118f15dce5e8b1e9bb1e/1502968653471/20170315-PEPPOL_BIS_4A-401.pdf
					
					https://peppol.eu/downloads/post-award/
					https://github.com/OpenPEPPOL/documentation/blob/master/PostAward/InvoiceOnly4A/20170315-PEPPOL_BIS_4A-401.pdf
					
				 
					cbc:LineExtensionAmount [0..1]    The total of Line Extension Amounts net of tax and settlement discounts, but inclusive of any applicable rounding amount.
					Sum of line amounts
					∑ LineExtensionAmount (at line level)

				 
					cbc:TaxExclusiveAmount [0..1]    The total amount exclusive of taxes.
					Invoice total amount without VAT
					LineExtensionAmount – AllowanceTotalAmount + ChargeTotalAmount
				 
					cbc:TaxInclusiveAmount [0..1]    The total amount inclusive of taxes.
					Invoice total amount with VAT
					TaxExclusiveAmount + TaxTotal TaxAmount (where tax scheme = VAT) + PayableRoundingAmount

					cbc:AllowanceTotalAmount [0..1]    The total amount of all allowances.
					Allowance/discounts on document level
					∑ Allowance Amount at document level (where ChargeIndicator = ”false”)
									 
					cbc:ChargeTotalAmount [0..1]    The total amount of all charges.
					Charges on document level
					∑ ChargeAmount(whereChargeIndicator=”true”)
				 
					cbc:PrepaidAmount [0..1]    The total prepaid amount.
					The amount prepaid
					Sum of amount previously paid

					cbc:PayableRoundingAmount [0..1]    The rounding amount (positive or negative) added to the calculated Line Extension Total Amount to produce the rounded Line Extension Total Amount.
					The amount used to round 
				 
					cbc:PayableAmount [1..1]    The total amount to be paid.
					PayableAmount to an in
					TaxInclusiveAmount (from the LegalMonetaryTotal class on document level) – PrepaidAmount (from the LegalMonetaryTotal class on document level)

				 
					Amounts MUST be given to a precision of two decimals.
					Amounts at document level MUST apply to all invoices lines.
					Total payable amount in an invoice MUST NOT be negative.
					Tax inclusive amount in an invoice MUST NOT be negative.
				 
				 */


				// moneda del documento
				CurrencyCodeContentType moneda_documento = Ctl_Enumeracion.ObtenerMoneda(documento.Moneda);

				MonetaryTotalType1 LegalMonetaryTotal = new MonetaryTotalType1();

				#region Total Importe bruto antes de impuestos
				// cbc:LineExtensionAmount [0..1]    The total of Line Extension Amounts net of tax and settlement discounts, but inclusive of any applicable rounding amount.
				//	Total importe bruto, suma de los importes brutos de las líneas de la factura.
				LineExtensionAmountType LineExtensionAmount = new LineExtensionAmountType();
				LineExtensionAmount.currencyID = moneda_documento;
				LineExtensionAmount.Value = decimal.Round(documento.ValorSubtotal, 2);
				LegalMonetaryTotal.LineExtensionAmount = LineExtensionAmount;
				#endregion

				#region Valor total base imponible (generó impuestos)
				//Total Base Imponible (Importe Bruto+Cargos-Descuentos): Base imponible para el cálculo de los impuestos.
				//Subtotal de la factura.
				TaxExclusiveAmountType TaxExclusiveAmount = new TaxExclusiveAmountType();
				TaxExclusiveAmount.currencyID = moneda_documento;
				TaxExclusiveAmount.Value = decimal.Round(documento.ValorIva, 2);
				LegalMonetaryTotal.TaxExclusiveAmount = TaxExclusiveAmount;
				#endregion

				//Opcional no usado por la DIAN, las partes pueden definir un significado o simplemente omitirlo
				/*#region Valor total base no imponible (no generó impuestos)
				TaxInclusiveAmountType TaxInclusiveAmount = new TaxInclusiveAmountType();
				TaxInclusiveAmount.currencyID = moneda_documento;
				TaxInclusiveAmount.Value = decimal.Round(documento.IntSubtotal, 2);
				LegalMonetaryTotal.TaxInclusiveAmount = TaxInclusiveAmount;
				#endregion*/

				#region Descuentos
				// Total de Descuentos: Suma de todos los descuentos presentes
				AllowanceTotalAmountType AllowanceTotalAmount = new AllowanceTotalAmountType();
				AllowanceTotalAmount.currencyID = moneda_documento;
				AllowanceTotalAmount.Value = decimal.Round(documento.ValorDescuento, 2);
				LegalMonetaryTotal.AllowanceTotalAmount = AllowanceTotalAmount;

				#endregion

				#region Valor total de pago //  Total de Factura =  Valor total bases - Valor descuentos + Valor total Impuestos - Valor total impuestos retenidos
				PayableAmountType PayableAmount = new PayableAmountType();
				PayableAmount.currencyID = moneda_documento;
				PayableAmount.Value = decimal.Round(documento.Total, 2);
				LegalMonetaryTotal.PayableAmount = PayableAmount;
				#endregion

				return LegalMonetaryTotal;

			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}
	}
}

