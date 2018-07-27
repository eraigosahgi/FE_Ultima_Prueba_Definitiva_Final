using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetUBL.Objetos;
using HGInetUBL.Recursos;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using LibreriaGlobalHGInet.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace HGInetUBL
{
	public partial class NotaCreditoXML
	{

        /// <summary>
        /// Crea el XML con la informacion de la nota credito en formato UBL
        /// </summary>
        /// <param name="documento">Objeto de tipo NotaCredito que contiene la informacion de la nota credito</param>
        /// <param name="empresa">Objeto de tipo TblEmpresas que contiene la informacion de la empresa que expide</param>
        /// <param name="ruta_almacenamiento">Ruta donde se va a guardar el archivo</param>
        /// <param name="reemplazar_archivo">Indica si sobreescribe el archivo existente</param>
        /// <param name="tipo">Indica el tipo de documento</param>
        /// <returns>Ruta donde se guardo el archivo XML</returns>  
        public static FacturaE_Documento CrearDocumento(NotaCredito documento, HGInetMiFacturaElectonicaData.ModeloServicio.ExtensionDian resolucion, TipoDocumento tipo)
		{
			try
			{
				if (documento == null)
					throw new Exception("La documento es inválido.");

				//Obtiene el nombre del archivo XML
				string nombre_archivo_xml = NombramientoArchivo.ObtenerXml(documento.Documento.ToString(), documento.DatosObligado.Identificacion, tipo);


				CreditNoteType nota_credito = new CreditNoteType();
				XmlSerializerNamespaces namespaces_xml = NamespacesXML.Obtener();

				#region nota_credito.UBLExtensions

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

				nota_credito.UBLExtensions = UBLExtensions;

				#endregion

				#region nota_credito.UBLVersionID //Versión de los esquemas UBL

				nota_credito.UBLVersionID = new UBLVersionIDType()
				{
					Value = Recursos.VersionesDIAN.UBLVersionID
				};
				#endregion

				#region nota_credito.CustomizationID //

				nota_credito.CustomizationID = new CustomizationIDType()
				{
					Value = documento.Prefijo
				};
				#endregion

				#region nota_credito.ProfileID //Versión del documento DIAN_UBL.xsd publicado por la DIAN
				nota_credito.ProfileID = new ProfileIDType()
				{
					Value = Recursos.VersionesDIAN.ProfileID
				};
				#endregion

				#region nota_credito.ID //Número de documento: Número de nota_credito o nota_credito cambiaria.
				string numero_documento = "";
				if (!string.IsNullOrEmpty(documento.Prefijo))
					numero_documento = string.Format("{0}", documento.Prefijo);

				numero_documento = string.Format("{0}{1}", numero_documento, documento.Documento.ToString());
				nota_credito.ID = new IDType();
				IDType ID = new IDType();
				ID.Value = numero_documento;
				nota_credito.ID = ID;
				#endregion

				#region nota_credito.IssueDate //Fecha de la nota_credito

				IssueDateType IssueDate = new IssueDateType();
				IssueDate.Value = Convert.ToDateTime(documento.Fecha.ToString(Fecha.formato_fecha_hginet));
				nota_credito.IssueDate = IssueDate;
				#endregion

				#region nota_credito.IssueTime //Hora de la nota_credito
				IssueTimeType IssueTime = new IssueTimeType();
				IssueTime.Value = documento.Fecha.ToString(Fecha.formato_hora_completa);
				nota_credito.IssueTime = IssueTime;
				#endregion

				#region nota_credito.Note //Información adicional
				//Texto libre, relativo al documento
				nota_credito.Note = new NoteType[1]{
				new NoteType(){
					Value =  documento.Nota
					}
				};

				List<string> notas_documento = new List<string>();

				// agrega los campos adicionales en el XML
				notas_documento = FormatoNotas.CamposPredeterminados(documento.DocumentoFormato);

				// agrega las observaciones del documento en la 3ra posición
				notas_documento.Add(documento.Nota);

				// agrega las notas adicionales del documento
				if (documento.Notas != null)
					notas_documento.AddRange(documento.Notas);


				NoteType[] Notes = new NoteType[notas_documento.Count];

				for (int i = 0; i < notas_documento.Count; i++)
				{
					NoteType Note = new NoteType();
					Note.Value = notas_documento[i];
					Notes[i] = Note;
				}
				nota_credito.Note = Notes;


				#endregion

				#region nota_credito.DocumentCurrencyCode //Divisa de la nota_credito
				//Divisa consolidada aplicable a toda la nota_credito
				DocumentCurrencyCodeType DocumentCurrencyCode = new DocumentCurrencyCodeType();
				DocumentCurrencyCode.Value = documento.Moneda; //(LISTADO DE VALORES DEFINIDO POR LA DIAN)                
				nota_credito.DocumentCurrencyCode = DocumentCurrencyCode;
				#endregion

				//PENDIENTE CODIGO CONCEPTO
				#region nota_credito.DiscrepancyResponse // Documento afectado y sustentacion de la afectacion
				/*Contiene el tipo de nota de crédito, el sustento de la emisión de la nota de crédito, así como la 
				identificación de la factura de venta electrónica afectada.*/

				//Documento afectado por la Nota credito
				ResponseType[] DiscrepancyResponse = new ResponseType[1];
				ResponseType Reference = new ResponseType();
				Reference.ReferenceID = new ReferenceIDType();
				Reference.ReferenceID.Value = documento.DocumentoRef.ToString();
				Reference.ResponseCode = new ResponseCodeType();
				Reference.ResponseCode.Value = documento.Concepto;
				DiscrepancyResponse[0] = Reference;

				nota_credito.DiscrepancyResponse = DiscrepancyResponse;
				#endregion

				#region nota_credito.BillingReference //Referencia Documento (orden)

				//Referencia a un documento afectar
				nota_credito.BillingReference = new BillingReferenceType[1];

				BillingReferenceType DocReference = new BillingReferenceType();
				DocumentReferenceType DocumentReference = new DocumentReferenceType();
				DocumentReference.ID = new IDType();
				DocumentReference.ID.Value = documento.DocumentoRef.ToString();
				DocumentReference.UUID = new UUIDType();
				DocumentReference.UUID.Value = documento.CufeFactura;
				DocumentReference.IssueDate = new IssueDateType();
				DocumentReference.IssueDate.Value = documento.FechaFactura;
				DocReference.InvoiceDocumentReference = DocumentReference;

				nota_credito.BillingReference[0] = DocReference;

                #endregion

                #region nota_credito.AccountingSupplierParty // Información del obligado a facturar
                nota_credito.AccountingSupplierParty = ObtenerObligado(documento.DatosObligado);
				#endregion

				#region nota_credito.AccountingCustomerParty //Información del Adquiriente
				/* Persona natural o jurídica que adquiere bienes y/o servicios y debe exigir factura
				51 o documento equivalente y, que tratándose de la factura electrónica, 
			    la recibe, rechaza, cuando
				52 sea del caso, y conserva para su posterior exhibición*/

				nota_credito.AccountingCustomerParty = ObtenerAquiriente(documento.DatosAdquiriente);
				#endregion

				#region nota_credito.PayeeParty //Receptor del Pago
				/*Receptor del Pago: Participante, Entidad,
				Departamento, Unidad, destinatario de la
				factura. Suele coincidir con el obligado a
				facturar Ver composición en la estructura común

				nota_credito.PayeeParty = new PartyType()
				{

				};*///PENDIENTE


				#endregion

				#region	nota_credito.TaxTotal //Impuesto y Impuesto Retenido
				/*Impuesto Retenido: Elemento raíz compuesto utilizado para informar de un impuesto	retenido. 
			    *Impuesto: Elemento raíz compuesto utilizado para informar de un impuesto.*/

				nota_credito.TaxTotal = ObtenerImpuestos(documento.DocumentoDetalles.ToList(), documento.Moneda);
				#endregion

				#region nota_credito.LegalMonetaryTotal //Datos Importes Totales
				/*Agrupación de campos
				relativos a los importes totales aplicables a la
				nota_credito. Estos importes son calculados teniendo
				en cuenta las líneas de nota_credito y elementos a
				nivel de nota_credito, como descuentos, cargos,
				impuestos, etc*/
				nota_credito.LegalMonetaryTotal = ObtenerTotales(documento);
                #endregion

                //La nota credito no requiere CUFE
                #region nota_credito.UUID //CUFE:Codigo Unico de facturacion Electronica.
                /*
				  CUFE: Obligatorio si es factura nacional.
					Codigo Unico de Facturacion Electronica. Campo
					que verifica la integridad de la información
					recibida, es un campo generado por el sistema
					Numeración de facturación, está pendiente
					definir su contenido. Esta Encriptado. La
					factura electrónica tiene una firma electrónica
					basada en firma digital según la política de
					firma propuesta. La integridad de la factura ya
					viene asegurada por la firma digital, no es
					necesaria ninguna medida más para asegurar que
					ningún campo ha sido alterado
				*/

                if (string.IsNullOrEmpty(resolucion.ClaveTecnicaDIAN))
                    throw new Exception("La clave técnica en la resolución de la DIAN es inválida para el documento.");

                UUIDType UUID = new UUIDType();
				string CUFE = CalcularCUFE(nota_credito, resolucion.ClaveTecnicaDIAN, documento.CufeFactura);
				UUID.Value = CUFE;
				nota_credito.UUID = UUID;
				#endregion

				#region nota_credito.CreditNoteLine  //Línea de nota_credito
				//Elemento que agrupa todos los campos de una línea de nota_credito
				nota_credito.CreditNoteLine = ObtenerDetalleDocumento(documento.DocumentoDetalles.ToList(),documento.CufeFactura,documento.Moneda);

                #endregion

                // convierte los datos del objeto en texto XML 
                StringBuilder txt_xml = ConvertirXml(nota_credito, namespaces_xml);

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
        /// <param name="nota_credito">Objeto de tipo CreditNoteType que contiene la informacion de la Nota Crédito</param>
        /// <param name="clave_tecnica">Clave técnica de la resolución</param>
        /// <param name="cufe_factura">Identificador de la factura Afectada</param>
        /// <returns></returns>
		public static string CalcularCUFE(CreditNoteType nota_credito, string clave_tecnica, string cufe_factura)
		{
			try
			{
				if (nota_credito == null)
					throw new Exception("Los datos de la factura son inválidos.");
				if (string.IsNullOrWhiteSpace(clave_tecnica))
					throw new Exception("La clave técnica es inválida.");

				#region Documentación de la creación código CUFE
				/*
				NumCr = Número de Nota Credito.
				Feccr = Fecha de Nota Credito en formato (Java) YYYYmmddHHMMss. 
				ValCr = Valor Nota Credito sin IVA, con punto decimal, con decimales a dos (2) dígitos, sin separadores de miles, ni símbolo pesos. 
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
			
				NumCr = /fe:CreditNote/cbc:ID
				FecCr = sinSimbolos(/fe:CreditNote/cbc:IssueDate + /fe:CreditNote/cbc:IssueTime)
						formato AAAAMMDDHHMMSS i.e. año + mes + día + hora + minutos + segundos
				ValCr = /fe:CreditNote/fe:LegalMonetaryTotal/cbc:LineExtensionAmount
				CodImp1 = /fe:CreditNote/fe:TaxTotal[X]/fe:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID = 01
				ValImp1 = /fe:CreditNote/fe:TaxTotal[X]/fe:TaxSubtotal/cbc:TaxAmount
				CodImp2 = /fe:CreditNote/fe:TaxTotal[y]/fe:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID = 02
				ValImp2 = /fe:CreditNote/fe:TaxTotal[y]/fe:TaxSubtotal/cbc:TaxAmount
				CodImp3 = /fe:CreditNote/fe:TaxTotal[z]/fe:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID = 03
				ValImp3 = /fe:CreditNote/fe:TaxTotal[z]/fe:TaxSubtotal/cbc:TaxAmount
				ValPag = /fe:CreditNote/fe:LegalMonetaryTotal/cbc:PayableAmount
				NitOFE = /fe:CreditNote/fe:AccountingSupplierParty/fe:Party/cac:PartyIdentification/cbc:ID
				TipAdq = /fe:CreditNote/fe:AccountingCustomerParty/fe:Party/cac:PartyIdentification/cbc:ID/@schemeID
				NumAdq = /fe:CreditNote /fe:AccountingCustomerParty/fe:Party/cac:PartyIdentification/cbc:ID
				ClTec = Examine la sección Cálculos para Nota Cr. 			 
				*/
				#endregion

				#region Creación Código CUFE

				string codigo_impuesto = string.Empty;
				DateTime fecha = nota_credito.IssueDate.Value;
				DateTime fecha_hora = Convert.ToDateTime(nota_credito.IssueTime.Value);
				TimeSpan hora = new TimeSpan(fecha_hora.Hour, fecha_hora.Minute, fecha_hora.Second);
				fecha = fecha.Date + hora;

				string NumCr = nota_credito.ID.Value;
				string FecCr = fecha.ToString(Fecha.formato_fecha_java);
				string ValCr = nota_credito.LegalMonetaryTotal.LineExtensionAmount.Value.ToString();

				//Impuesto 1
				string CodImp1 = "01";
				decimal ValImp1 = 0.00M;

				//Impuesto 2
				string CodImp2 = "02";
				decimal ValImp2 = 0.00M;

				//Impuesto 3
				string CodImp3 = "03";
				decimal ValImp3 = 0.00M;

				for (int i = 0; i < nota_credito.TaxTotal.Count(); i++)
				{
					for (int j = 0; j < nota_credito.TaxTotal[i].TaxSubtotal.Count(); j++)
					{
						codigo_impuesto = nota_credito.TaxTotal[i].TaxSubtotal[j].TaxCategory.TaxScheme.ID.Value;

						if (codigo_impuesto.Equals("01"))
						{
							ValImp1 += nota_credito.TaxTotal[i].TaxSubtotal[j].TaxAmount.Value;
						}
						else if (codigo_impuesto.Equals("02"))
						{
							ValImp2 += nota_credito.TaxTotal[i].TaxSubtotal[j].TaxAmount.Value;
						}
						else if (codigo_impuesto.Equals("03"))
						{
							ValImp3 += nota_credito.TaxTotal[i].TaxSubtotal[j].TaxAmount.Value;
						}
					}
				}

				string ValImp = nota_credito.LegalMonetaryTotal.PayableAmount.Value.ToString();

				string NitOFE = string.Empty;

				for (int contador_obligado_facturar = 0; contador_obligado_facturar < nota_credito.AccountingSupplierParty.Party.PartyIdentification.Count(); contador_obligado_facturar++)
				{
					NitOFE = nota_credito.AccountingSupplierParty.Party.PartyIdentification[contador_obligado_facturar].ID.Value;
				}

				string TipAdq = string.Empty;
				string NumAdq = string.Empty;

				for (int contador_adquiriente = 0; contador_adquiriente < nota_credito.AccountingCustomerParty.Party.PartyTaxScheme.Count(); contador_adquiriente++)
				{
					TipAdq = nota_credito.AccountingCustomerParty.Party.PartyIdentification[contador_adquiriente].ID.schemeID;
					NumAdq = nota_credito.AccountingCustomerParty.Party.PartyIdentification[contador_adquiriente].ID.Value;
				}

				string cufe = NumCr
					+ FecCr
					+ ValCr.Replace(",", ".")
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
					+ cufe_factura
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
        /// <param name="nota_credito">Objeto de tipo CreditNoteType que contiene la informacion de la nota_credito</param>
        /// <param name="namespaces_xml">Namespaces</param>       
        /// <returns>Ruta donde se guardo el archivo XML</returns>     
        private static StringBuilder ConvertirXml(CreditNoteType nota_credito, XmlSerializerNamespaces namespaces_xml)
        {
            try
            {
                if (nota_credito == null)
                    throw new Exception("La Nota Crédito es inválida.");

                if (namespaces_xml == null)
                    throw new Exception("Los Namespaces son inválidos.");

                StringBuilder texto_xml = Xml.Convertir<CreditNoteType>(nota_credito, namespaces_xml);

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
        public static SupplierPartyType1 ObtenerObligado(Tercero empresa)
		{
			try
			{
				if (empresa == null)
					throw new Exception("Los datos de la empresa son inválidos.");

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
		public static CustomerPartyType1 ObtenerAquiriente(Tercero tercero)
		{
			try
			{
				if (tercero == null)
					throw new Exception("Los datos del tercero son inválidos.");

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
		/// Llena el objeto de los impuestos de la nota credito con los datos documento detalle
		/// </summary>
		/// <param name="documentoDetalle">Datos del detalle del documento</param>
		/// <returns>Objeto de tipo TaxTotalType1</returns>
		public static TaxTotalType1[] ObtenerImpuestos(List<DocumentoDetalle> documentoDetalle, string moneda)
		{
			try
			{
                if (documentoDetalle == null || documentoDetalle.Count == 0)
                    throw new Exception("El detalle del documento es inválido.");

                //Toma los impuestos de IVA que tiene el producto en el detalle del documento
                var impuestos_iva = documentoDetalle.Select(_impuesto => new { _impuesto.IvaPorcentaje, TipoImpuestos.Iva, _impuesto.IvaValor }).Distinct();

                List<DocumentoImpuestos> doc_impuestos = new List<DocumentoImpuestos>();

                decimal BaseImponibleImpuesto = 0;

				// moneda del primer detalle
				CurrencyCodeContentType moneda_detalle = Ctl_Enumeracion.ObtenerMoneda(moneda);

				foreach (var item in impuestos_iva)
                {
                    DocumentoImpuestos imp_doc = new DocumentoImpuestos();
                    List<DocumentoDetalle> doc_ = documentoDetalle.Where(docDet => docDet.IvaPorcentaje == item.IvaPorcentaje).ToList();
                    BaseImponibleImpuesto = decimal.Round(documentoDetalle.Where(docDet => docDet.IvaPorcentaje == item.IvaPorcentaje).Sum(docDet => (docDet.ValorUnitario - docDet.DescuentoValor) * docDet.Cantidad), 2);

                    //imp_doc.Codigo = item.IntIva;
                    //imp_doc.Nombre = item.StrDescripcion;
                    imp_doc.Porcentaje = decimal.Round(item.IvaPorcentaje, 2);
                    imp_doc.TipoImpuesto = item.Iva;
                    imp_doc.BaseImponible = BaseImponibleImpuesto;
                    foreach (var docDet in doc_)
                    {
                        imp_doc.ValorImpuesto = decimal.Round(imp_doc.ValorImpuesto + docDet.IvaValor, 2);
                    }
                    doc_impuestos.Add(imp_doc);
                }

                //Toma el impuesto al consumo de los productos que esten el detalle
                var impuesto_consumo = documentoDetalle.Select(_consumo => new { _consumo.ValorImpuestoConsumo, TipoImpuestos.Consumo }).Distinct();
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
                            BaseImponibleImpConsumo = decimal.Round(documentoDetalle.Where(docDet => docDet.ValorImpuestoConsumo != 0).Sum(docDet => (docDet.ValorUnitario - docDet.DescuentoValor) * docDet.Cantidad), 2);

                            //imp_doc.Codigo = item.IntImpConsumo.ToString();
                            //imp_doc.Nombre = item.StrDescripcion;
                            imp_doc.Porcentaje = decimal.Round(item.ValorImpuestoConsumo, 2);
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
                var impuesto_ica = documentoDetalle.Select(_ica => new { _ica.ReteIcaPorcentaje, TipoImpuestos.Ica, _ica.ReteIcaValor }).Distinct();
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
                            BaseImponibleReteIca = decimal.Round(documentoDetalle.Where(docDet => docDet.ReteIcaValor != 0).Sum(docDet => (docDet.ValorUnitario - docDet.DescuentoValor) * docDet.Cantidad), 2);

                            imp_doc.Porcentaje = decimal.Round(item.ReteIcaPorcentaje, 2);
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
		/// Obtiene los valores del encabezado del documento
		/// </summary>
		/// <param name="documento">datos del documento</param>
		/// <returns>MonetaryTotalType1</returns>
		public static MonetaryTotalType1 ObtenerTotales(NotaCredito documento)
		{
			try
			{
				MonetaryTotalType1 LegalMonetaryTotal = new MonetaryTotalType1();

				#region Total Importe bruto antes de impuestos
				// cbc:LineExtensionAmount [0..1]    The total of Line Extension Amounts net of tax and settlement discounts, but inclusive of any applicable rounding amount.
				//	Total importe bruto, suma de los importes brutos de las líneas de la factura.
				LineExtensionAmountType LineExtensionAmount = new LineExtensionAmountType();
				LineExtensionAmount.currencyID = CurrencyCodeContentType.COP;
				LineExtensionAmount.Value = decimal.Round(documento.ValorSubtotal, 2);
				LegalMonetaryTotal.LineExtensionAmount = LineExtensionAmount;
				#endregion

				#region Valor total base imponible (generó impuestos)
				//Total Base Imponible (Importe Bruto+Cargos-Descuentos): Base imponible para el cálculo de los impuestos.
				//Subtotal de la factura.
				TaxExclusiveAmountType TaxExclusiveAmount = new TaxExclusiveAmountType();
				TaxExclusiveAmount.currencyID = CurrencyCodeContentType.COP;
				TaxExclusiveAmount.Value = decimal.Round(documento.ValorIva, 2);
				LegalMonetaryTotal.TaxExclusiveAmount = TaxExclusiveAmount;
				#endregion

				#region Descuentos
				// Total de Descuentos: Suma de todos los descuentos presentes
				AllowanceTotalAmountType AllowanceTotalAmount = new AllowanceTotalAmountType();
				AllowanceTotalAmount.currencyID = CurrencyCodeContentType.COP;
				AllowanceTotalAmount.Value = decimal.Round(documento.ValorDescuento, 2);
				LegalMonetaryTotal.AllowanceTotalAmount = AllowanceTotalAmount;

				#endregion

				#region Valor total de pago //  Total de Factura =  Valor total bases - Valor descuentos + Valor total Impuestos - Valor total impuestos retenidos
				PayableAmountType PayableAmount = new PayableAmountType();
				PayableAmount.currencyID = CurrencyCodeContentType.COP;
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


		/// <summary>
		/// Llena el objeto del detalle de la nota credito con los datos documento detalle
		/// </summary>
		/// <param name="DocumentoDetalle">Datos del detalle del documento</param>
		/// <returns>Objeto de tipo CreditNoteLineType</returns>
		public static CreditNoteLineType[] ObtenerDetalleDocumento(List<DocumentoDetalle> documentoDetalle, string cufefactura, string moneda)
		{
			try
			{
				if (documentoDetalle == null || !documentoDetalle.Any())
					throw new Exception("El detalle del documento es inválido.");

				CreditNoteType nota = new CreditNoteType();
				CreditNoteLineType[] CreditNoteLineType = new CreditNoteLineType[documentoDetalle.Count()];

				int contadorPosicion = 0;
				int contadorProducto = 1;

				foreach (var DocDet in documentoDetalle)
				{

					CurrencyCodeContentType moneda_detalle = Ctl_Enumeracion.ObtenerMoneda(moneda);

					if (string.IsNullOrEmpty(DocDet.UnidadCodigo))
						DocDet.UnidadCodigo = "S7";

					decimal valorTotal = DocDet.Cantidad * DocDet.ValorUnitario;
					CreditNoteLineType CreditNoteLine = new CreditNoteLineType();

					#region Id producto definido por la Dian (Contador de productos iniciando desde 1)
					IDType ID = new IDType();
					ID.Value = contadorProducto.ToString();
					CreditNoteLine.ID = ID;

					CreditNoteLine.UUID = new UUIDType();
					CreditNoteLine.UUID.Value = cufefactura;

					#endregion

					#region Cantidad producto
					CreditedQuantityType CreditedQuantity = new CreditedQuantityType();
					CreditedQuantity.Value = decimal.Round(DocDet.Cantidad, 2);
					CreditNoteLine.CreditedQuantity = CreditedQuantity;

					// Unidad de medida
					CreditedQuantity.unitCode = Ctl_Enumeracion.ObtenerUnidadMedida(DocDet.UnidadCodigo);
					CreditNoteLine.CreditedQuantity = CreditedQuantity;
					#endregion

					#region Valor Total
					LineExtensionAmountType LineExtensionAmount = new LineExtensionAmountType();
					LineExtensionAmount.currencyID = moneda_detalle; //(LISTADO DE VALORES DEFINIDO POR LA DIAN)
					LineExtensionAmount.Value = decimal.Round(valorTotal, 2);
					CreditNoteLine.LineExtensionAmount = LineExtensionAmount;
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

					TaxTotal.TaxSubtotal = TaxesSubtotal;
					TaxesTotal[0] = TaxTotal;
					CreditNoteLine.TaxTotal = TaxesTotal;

					#endregion

					#region Datos producto
					ItemType Item = new ItemType();

					#region Descripcion producto
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

					#region Id producto definido por la Empresa ***///
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

					CreditNoteLine.Item = Item;

					#endregion

					#region Valor Unitario producto
					PriceType Price = new PriceType();
					PriceAmountType PriceAmount = new PriceAmountType();
					PriceAmount.currencyID = moneda_detalle; //(LISTADO DE VALORES DEFINIDO POR LA DIAN)
					PriceAmount.Value = decimal.Round(DocDet.ValorUnitario, 2);
					Price.PriceAmount = PriceAmount;
					CreditNoteLine.Price = Price;
					#endregion

					CreditNoteLineType[contadorPosicion] = CreditNoteLine;
					contadorProducto++;
					contadorPosicion++;
				}

				nota.CreditNoteLine = CreditNoteLineType;
				return nota.CreditNoteLine;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}

		}


	}
}
