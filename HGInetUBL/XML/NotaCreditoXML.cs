using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetUBL.Objetos;
using HGInetUBL.Recursos;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace HGInetUBL
{
	public class NotaCreditoXML
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
		protected static ResultadoXml CrearDocumento(NotaCredito documento, Tercero empresa, string ruta_almacenamiento, bool reemplazar_archivo, TipoDocumento tipo = TipoDocumento.NotaCredito)
		{
			try
			{
				if (documento == null)
					throw new Exception("La documento es inválido.");

				if (string.IsNullOrEmpty(Directorio.CrearDirectorio(ruta_almacenamiento)))
					throw new ApplicationException("Error al obtener la ruta de almacenamiento del xml.");

				//Obtiene el nombre del archivo XML
				//string nombre_archivo_xml = NombramientoArchivo.ObtenerXml(documento.IntDocumento.ToString(), documento.StrTercero, tipo);

				string nombre_archivo_xml = RecursoDms.ArchivoDocumento.Replace("{cod_trans}", documento.IntTransaccion).Replace("{num_doc}", documento.IntDocumento.ToString());

				if (string.IsNullOrWhiteSpace(nombre_archivo_xml))
					throw new ApplicationException("El nombre del archivo es inválido.");

				// valida la existencia del archivo xml
				bool existe_archivo = Archivo.ValidarExistencia(string.Format("{0}{1}{2}", ruta_almacenamiento, nombre_archivo_xml, ".xml"));

				if (existe_archivo && reemplazar_archivo)
					Archivo.Borrar(string.Format("{0}{1}{2}", ruta_almacenamiento, nombre_archivo_xml, ".xml"));
				else if (existe_archivo && !reemplazar_archivo)
					throw new ApplicationException(string.Format("No fue permitido sobreescribir el archivo: {0}{1}{2}.", ruta_almacenamiento, nombre_archivo_xml, ".xml"));

				#region nota_credito anterior
				/*/// <summary>
				/// Llena los objetos con la informacion que corresponde a una nota credito
				/// </summary>
				public void NotaCredito()
				{
				CreditNoteType CreditNoteType = new CreditNoteType();
				UBLExtensionsType UBLExtensions = new UBLExtensionsType();
				UBLVersionIDType UBLVersionID = new UBLVersionIDType();
				ProfileIDType ProfileID = new ProfileIDType();
				IDType ID = new IDType();
				UUIDType UUID = new UUIDType();
				IssueDateType IssueDate = new IssueDateType();
				IssueTimeType IssueTime = new IssueTimeType();
				NoteType Note = new NoteType();
				DocumentCurrencyCodeType DocumentCurrencyCode = new DocumentCurrencyCodeType();
				PeriodType InvoicePeriod = new PeriodType();
				OrderReferenceType OrderReference = new OrderReferenceType();
				BillingReferenceType BillingReference = new BillingReferenceType();
				DocumentReferenceType DespatchDocumentReference = new DocumentReferenceType();
				DocumentReferenceType ReceiptDocumentReference = new DocumentReferenceType();
				DocumentReferenceType ContractDocumentReference = new DocumentReferenceType();
				DocumentReferenceType AdditionalDocumentReference = new DocumentReferenceType();
				SupplierPartyType AccountingSupplierParty = new SupplierPartyType();
				CustomerPartyType AccountingCustomerParty = new CustomerPartyType();
				PartyType PayeeParty = new PartyType();
				TaxTotalType TaxTotal = new TaxTotalType();
				MonetaryTotalType1 LegalMonetaryTotal = new MonetaryTotalType1();
				CreditNoteLineType CreditNoteLine = new CreditNoteLineType();*/
				#endregion

				CreditNoteType nota_credito = new CreditNoteType();
				XmlSerializerNamespaces namespaces_xml = NamespacesXML.Obtener();

				#region nota_credito.UBLExtensions
				//Firma con certificado digital
				//XmlElement firma = FirmaXML.Firmar();

				////extenciones
				//nota_credito.UBLExtensions = new UBLExtensionType[1]{
				//	new UBLExtensionType(){
				//		ExtensionContent =  firma
				//	}				
				//};

				XmlDocument doc = new XmlDocument();
				doc.LoadXml("<firma></firma>");

				UBLExtensionType[] UBLExtensions = new UBLExtensionType[2];

				#region Extension de la Dian
				UBLExtensionType UBLExtensionDian = new UBLExtensionType();
				UBLExtensionDian.ExtensionContent = ExtensionDian.Obtener(documento, empresa, TipoDocumento.NotaCredito);
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

				#region nota_credito.ProfileID //Versión del documento DIAN_UBL.xsd publicado por la DIAN
				nota_credito.ProfileID = new ProfileIDType()
				{
					Value = Recursos.VersionesDIAN.ProfileID
				};
				#endregion

				#region nota_credito.ID //Número de documento: Número de nota_credito o nota_credito cambiaria.
				nota_credito.ID = new IDType();
				IDType ID = new IDType();
				ID.Value = documento.IntDocumento.ToString();
				nota_credito.ID = ID;
				#endregion

				#region nota_credito.IssueDate //Fecha de la nota_credito

				IssueDateType IssueDate = new IssueDateType();
				IssueDate.Value = Convert.ToDateTime(documento.DatFecha.ToString(Fecha.formato_fecha_hginet));
				nota_credito.IssueDate = IssueDate;
				#endregion

				#region nota_credito.IssueTime //Hora de la nota_credito
				IssueTimeType IssueTime = new IssueTimeType();
				IssueTime.Value = documento.DatFecha.ToString(Fecha.formato_hora_completa);
				nota_credito.IssueTime = IssueTime;
				#endregion

				#region nota_credito.Note //Información adicional
				//Texto libre, relativo al documento
				nota_credito.Note = new NoteType[1]{
				new NoteType(){
					Value =  documento.StrObservaciones
					}
				};
				#endregion

				#region nota_credito.DocumentCurrencyCode //Divisa de la nota_credito
				//Divisa consolidada aplicable a toda la nota_credito
				DocumentCurrencyCodeType DocumentCurrencyCode = new DocumentCurrencyCodeType();
				DocumentCurrencyCode.Value = "COP"; //(LISTADO DE VALORES DEFINIDO POR LA DIAN)                
				nota_credito.DocumentCurrencyCode = DocumentCurrencyCode;
				#endregion


				//No es necesario
				#region nota_credito.InvoicePeriod //Periodo de nota_credito
				/*//Intervalo de fechas a las que referencia la nota_credito
				nota_credito.InvoicePeriod = new PeriodType()
				{
					DurationMeasure = new DurationMeasureType()
					{
						unitCode = UnitCodeContentType.DAY,
						Value = 0
					}
				};*/
				#endregion

				//PENDIENTE CODIGO CONCEPTO
				#region nota_credito.DiscrepancyResponse // Documento afectado y sustentacion de la afectacion
				/*Contiene el tipo de nota de crédito, el sustento de la emisión de la nota de crédito, así como la 
				identificación de la factura de venta electrónica afectada.*/

				//Documento afectado por la Nota credito
				ResponseType[] DiscrepancyResponse = new ResponseType[1];
				ResponseType Reference = new ResponseType();
				Reference.ReferenceID = new ReferenceIDType();
				Reference.ReferenceID.Value = documento.IntDocRef.ToString();
				Reference.ResponseCode = new ResponseCodeType();
				Reference.ResponseCode.Value = "2";
				DiscrepancyResponse[0] = Reference;

				nota_credito.DiscrepancyResponse = DiscrepancyResponse;
				#endregion


				/*#region nota_credito.OrderReference //Referencia Documento afectar (orden)
				//Referencia a un documento afectar
				nota_credito.OrderReference = new OrderReferenceType[1];
				OrderReferenceType OrderReference = new OrderReferenceType();
                OrderReference.ID = new IDType();
				OrderReference.ID.Value = documento.IntDocRef.ToString();
				nota_credito.OrderReference[0] = OrderReference;

				#endregion*/

				#region nota_credito.BillingReference //Referencia Documento (orden)

				//Referencia a un documento
				nota_credito.BillingReference = new BillingReferenceType[1];

				BillingReferenceType DocReference = new BillingReferenceType();
				DocumentReferenceType DocumentReference = new DocumentReferenceType();
				DocumentReference.ID = new IDType();
				DocumentReference.ID.Value = documento.IntDocRef.ToString();
				DocumentReference.UUID = new UUIDType();
				DocumentReference.UUID.Value = documento.StrFacturaECufe;
				DocumentReference.IssueDate = new IssueDateType();
				DocumentReference.IssueDate.Value = documento.DatFecha1.HasValue ? documento.DatFecha1.Value : DateTime.Now;
				DocReference.InvoiceDocumentReference = DocumentReference;

				nota_credito.BillingReference[0] = DocReference;

				#endregion

				//no se utiliza
				#region nota_credito.DespatchDocumentReference //Referencia Documento (despacho)
				/*//Referencia a un documento
				nota_credito.DespatchDocumentReference = new DocumentReferenceType[1]
				{		
				new DocumentReferenceType()
				};
				#endregion

				#region nota_credito.ReceiptDocumentReference //Referencia Documento (recepción)
				//Referencia a un documento
				nota_credito.ReceiptDocumentReference = new DocumentReferenceType[1]
				{//PENDIENTE	
				new DocumentReferenceType()
				};
				#endregion

				//No aplica
				#region nota_credito.ContractDocumentReference //Referencia Documento (contrato)
				//Referencia a un documento
				nota_credito.ContractDocumentReference = new DocumentReferenceType[1]
				{	
				new DocumentReferenceType()//PENDIENTE
				};*/
				#endregion

				#region nota_credito.AdditionalDocumentReference //Referencia Documento
				//nota_credito.AdditionalDocumentReference = new DocumentReferenceType[1];
				////{
				////new DocumentReferenceType()//PENDIENTE
				////};
				//DocumentReferenceType DocuReference = new DocumentReferenceType();
				//DocuReference.ID = new IDType();
				//DocuReference.ID.Value = documento.IntDocRef.ToString();

				#endregion

				#region nota_credito.AccountingSupplierParty // Información del obligado a facturar
				nota_credito.AccountingSupplierParty = ObtenerObligado(empresa);
				#endregion

				//Anterior
				#region nota_credito.AccountingSupplierParty //Información del obligado a facturar

				/*Campos segun peru*/
				/*nota_credito.AccountingSupplierParty = new SupplierPartyType1()//Obligado a facturar
				{

					//Segun PERU emisor numero de documento
					CustomerAssignedAccountID = new CustomerAssignedAccountIDType()//cuenta asignada por el cliente 
					{
						Value = "811021438"
					},

					//Segun PERU emisor tipo de documento
					AdditionalAccountID = new AdditionalAccountIDType()//cuenta adicional
					{
						Value = "11" //CODIGO DESDE ERP
					},

					Party = new PartyType1()//individuo
					{
						PartyName = new PartyNameType[1]
						{
							new PartyNameType()	//Nombre Comercial
							{
								Name= new NameType1()
								{
									Value = "HGI SAS"
								}
							}
					
						},

						PostalAddress = new AddressType()
						{
							//id ubicacion
							ID = new IDType
							{
								Value = ""
							},

							//Direccion
							StreetName = new StreetNameType()
							{
								Value = ""
							},

							CitySubdivisionName = new CitySubdivisionNameType()
							{
								Value = ""
							},

							//ciudad
							CityName = new CityNameType()
							{
								Value = "Medellin"
							},

							//departamento
							Department = new DepartmentType()
							{
								Value = "1"
							},

							//ciudad
							Country = new CountryType()
							{
								IdentificationCode = new IdentificationCodeType()
								{
									Value = "CO"
								}
							}
						}
					},

					AccountingContact = new ContactType()
					{

					},

					DespatchContact = new ContactType()// contacto despacho
					{

					}

				};*/

				#endregion

				#region nota_credito.AccountingCustomerParty //Información del Adquiriente
				/* Persona natural o jurídica que adquiere bienes y/o servicios y debe exigir factura
				51 o documento equivalente y, que tratándose de la factura electrónica, 
			    la recibe, rechaza, cuando
				52 sea del caso, y conserva para su posterior exhibición*/

				nota_credito.AccountingCustomerParty = ObtenerAquiriente(documento.TblTerceros);
				#endregion

				//anterior
				#region nota_credito.AccountingCustomerParty //Información del Adquiriente
				/* Persona natural o jurídica que adquiere bienes y/o servicios y debe exigir factura
				51 o documento equivalente y, que tratándose de la factura electrónica, 
			    la recibe, rechaza, cuando
				52 sea del caso, y conserva para su posterior exhibición*/

				/*nota_credito.AccountingCustomerParty = new CustomerPartyType1()
				{
					//numero de identificacion
					CustomerAssignedAccountID = new CustomerAssignedAccountIDType()
					{
						Value = "1037653220"
					},

					AdditionalAccountID = new AdditionalAccountIDType()
					{
						Value = "11" //CODIGO DESDE ERP
					},

					Party = new PartyType1()//individuo
					{
						PartyName = new PartyNameType[1]
					{
						new PartyNameType()	//Nombre Comercial
						{
							Name= new NameType1()
							{
								Value = "DMF"
							}
						}
					
					},

						PostalAddress = new AddressType()
						{
							//id ubicacion
							ID = new IDType
							{
								Value = ""
							},

							//Direccion
							StreetName = new StreetNameType()
							{
								Value = ""
							},

							CitySubdivisionName = new CitySubdivisionNameType()
							{
								Value = ""
							},

							//ciudad
							CityName = new CityNameType()
							{
								Value = "Medellin"
							},

							//departamento
							Department = new DepartmentType()
							{
								Value = "1"
							},

							//ciudad
							Country = new CountryType()
							{
								IdentificationCode = new IdentificationCodeType()
								{
									Value = "CO"
								}
							}
						},

						//Nombre Legal
						PartyLegalEntity = new PartyLegalEntityType1[1] 
					{ 
						 new PartyLegalEntityType1{
							RegistrationName = new RegistrationNameType()
							{
								Value="Daniela monslave florez"
							}
						 }
					}
					},

				};*/

				#endregion

				#region nota_credito.PayeeParty //Receptor del Pago
				/*Receptor del Pago: Participante, Entidad,
				Departamento, Unidad, destinatario de la
				factura. Suele coincidir con el obligado a
				facturar Ver composición en la estructura común*/

				nota_credito.PayeeParty = new PartyType()
				{

				};//PENDIENTE


				#endregion

				#region	nota_credito.TaxTotal //Impuesto y Impuesto Retenido
				/*Impuesto Retenido: Elemento raíz compuesto utilizado para informar de un impuesto	retenido. 
			 *Impuesto: Elemento raíz compuesto utilizado para informar de un impuesto.*/

				nota_credito.TaxTotal = ObtenerImpuestos(documento.DocumentoDetalle.ToList());

				//anterior
				/*nota_credito.TaxTotal = new TaxTotalType1[1]
			{
				 new TaxTotalType1()
				 {
				
					 TaxAmount = new TaxAmountType(){
						currencyID = CurrencyCodeContentType.COP,
						Value = 0
					 },

					 TaxSubtotal = new TaxSubtotalType1[1]
					 {
						 new TaxSubtotalType1()						 
						 {
							TaxAmount = new TaxAmountType(){
								currencyID = CurrencyCodeContentType.COP,
								Value = 0
							 },

							TaxCategory = new TaxCategoryType()
                            {
                                TaxScheme = new TaxSchemeType()
                                {
									ID =  new IDType()
									{
										Value= "1000"
									},
                                    Name = new NameType1()
 									{
										Value="IGV"
									},
                                   
                                    TaxTypeCode = new TaxTypeCodeType()
									{
										Value = "VAT"
									}
                                }
                            }
						 }
					 }
				 }			
			};*/

				#endregion

				#region nota_credito.LegalMonetaryTotal //Datos Importes Totales
				/*Agrupación de campos
				relativos a los importes totales aplicables a la
				nota_credito. Estos importes son calculados teniendo
				en cuenta las líneas de nota_credito y elementos a
				nivel de nota_credito, como descuentos, cargos,
				impuestos, etc*/
				nota_credito.LegalMonetaryTotal = ObtenerTotales(documento);

				/*nota_credito.LegalMonetaryTotal = new MonetaryTotalType1()
				{
					//Total de venta
					PayableAmount = new PayableAmountType()
					{
						currencyID = CurrencyCodeContentType.COP,
						Value = 0
					},

					//Descuento Global
					AllowanceTotalAmount = new AllowanceTotalAmountType()
					{
						currencyID = CurrencyCodeContentType.COP,
						Value = 0
					},
				};*/
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

				if (string.IsNullOrEmpty(documento.TblTransacciones.StrClaveTecnicaDIAN))
					throw new Exception("La clave técnica de la DIAN en la transacción es inválida.");

				UUIDType UUID = new UUIDType();
				string CUFE = CalcularCUFE(nota_credito, documento.TblTransacciones.StrClaveTecnicaDIAN, documento.StrFacturaECufe);
				UUID.Value = CUFE;
				nota_credito.UUID = UUID;
				#endregion

				#region nota_credito.CreditNoteLine  //Línea de nota_credito
				//Elemento que agrupa todos los campos de una línea de nota_credito
				nota_credito.CreditNoteLine = ObtenerDetalleDocumento(documento.DocumentoDetalle.ToList());

				/*nota_credito.CreditNoteLine = new CreditNoteLineType[1]
			{
				new CreditNoteLineType()
			};*/
				#endregion

				//#region CrearArchivoXML
				////Obtiene los Namespaces 
				//XmlSerializerNamespaces namespaces_xml = NamespacesXML.Obtener();
				//CrearArchivo(nota_credito, "811021438", namespaces_xml);
				//#endregion

				string ruta_xml = GuardarDocumento(nota_credito, ruta_almacenamiento, nombre_archivo_xml, namespaces_xml, reemplazar_archivo);

				ResultadoXml xml_sin_firma = new ResultadoXml();
				xml_sin_firma.Documento = documento;
				xml_sin_firma.RutaXml = ruta_xml;
				xml_sin_firma.CUFE = CUFE;

				return xml_sin_firma;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Crea el archivo XML
		/// </summary>
		/// <param name="nota_credito">Objeto de tipo CreditNoteType</param>
		/// <param name="identificacion">Identificación del obligado a facturar</param>
		/// <param name="namespaces_xml">Namespaces indicados por la DIAN</param>
		public void CrearArchivo(CreditNoteType nota_credito, string identificacion, XmlSerializerNamespaces namespaces_xml)
		{
			try
			{
				string nombre_archivo_xml = NombramientoArchivo.ObtenerXml(nota_credito.ID.Value, identificacion, TipoDocumento.NotaCredito);
				string nombre_archivo_zip = NombramientoArchivo.ObtenerZip(nota_credito.ID.Value, identificacion, TipoDocumento.NotaCredito);
				string ruta = @"C:\Users\dmonsalve\Downloads\xml\";

				Xml.GuardarObjeto(nota_credito, ruta, nombre_archivo_xml, namespaces_xml);
				//	Zip.Comprimir(ruta, nombre_archivo_xml);
				//	Archivo.ModificarNombre(ruta,( nombre_archivo_xml + ".zip"), nombre_archivo_zip);

			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Calcula el codigo CUFE
		/// </summary>
		/// <param name="nota_credito">Objeto de tipo InvoiceType que contiene la informacion de la factura</param>
		/// <param name="clave_tecnica">Clave técnica de la resolución</param>
		/// <returns>Texto con la encriptación del CUFE</returns>        
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
		/// Guarda el archivo XML
		/// </summary>
		/// <param name="factura">Objeto de tipo CreditNoteType que contiene la informacion de la nota credito</param>
		/// <param name="ruta_almacenamiento">Ruta donde se va a guardar el archivo</param>
		/// <param name="nombre_archivo">Nombre del archivo XML</param>
		/// <param name="namespaces_xml">Namespaces</param>       
		/// <param name="reemplazar_archivo">Indica si sobreescribe el archivo existente</param>
		/// <returns>Ruta donde se guardo el archivo XML</returns>     
		public static string GuardarDocumento(CreditNoteType nota_credito, string ruta_almacenamiento, string nombre_archivo, XmlSerializerNamespaces namespaces_xml, bool reemplazar_archivo)
		{
			try
			{
				if (nota_credito == null)
					throw new Exception("La nota credito es inválida.");

				if (namespaces_xml == null)
					throw new Exception("Los Namespaces son inválidos.");

				if (string.IsNullOrEmpty(Directorio.CrearDirectorio(ruta_almacenamiento)))
					throw new ApplicationException("Error al obtener la ruta de almacenamiento del xml.");

				if (string.IsNullOrWhiteSpace(nombre_archivo))
					throw new ApplicationException("El nombre del archivo es inválido.");

				bool existe_archivo = Archivo.ValidarExistencia(string.Format("{0}{1}{2}", ruta_almacenamiento, nombre_archivo, ".xml"));

				if (existe_archivo && reemplazar_archivo)
					Archivo.Borrar(string.Format("{0}{1}{2}", ruta_almacenamiento, nombre_archivo, ".xml"));
				else if (existe_archivo && !reemplazar_archivo)
					throw new ApplicationException(string.Format("No fue permitido sobreescribir el archivo: {0}{1}{2}.", ruta_almacenamiento, nombre_archivo, ".xml"));

				if (!nombre_archivo.EndsWith(".xml"))
					nombre_archivo = string.Format("{0}{1}", nombre_archivo, ".xml");

				string ruta_xml = Xml.GuardarObjeto(nota_credito, ruta_almacenamiento, nombre_archivo, namespaces_xml);
				return ruta_xml;
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

				/* Tipo de persona en el ERP 
					 1 Natural
					 2 Jurídica                      
				   para la Dian
					 1 Persona jurídica
					 2 Persona natural             */

				string dian_tipoPersona = string.Empty;
				if (empresa.IntTipoPersona == 1)
					dian_tipoPersona = "2";  //(LISTADO DE VALORES DEFINIDO POR LA DIAN)         
				else if (empresa.IntTipoPersona == 2)
					dian_tipoPersona = "1";  //(LISTADO DE VALORES DEFINIDO POR LA DIAN)


				/* Tipo de regimen en el ERP 
					 1 Simplificado
					 2 Comun                  
					Para la Dian
					 0 Simplificado
					 2 Común                  */

				string dian_regimen = string.Empty;
				if (empresa.IntRegimen == 1)
					dian_regimen = "0";  //(LISTADO DE VALORES DEFINIDO POR LA DIAN)
				else if (empresa.IntRegimen == 2)
					dian_regimen = "2";  //(LISTADO DE VALORES DEFINIDO POR LA DIAN)

				string dian_tipoDocumento = string.Empty;
				if (empresa.StrTipoId.Equals("NI"))
					dian_tipoDocumento = "31";  //(LISTADO DE VALORES DEFINIDO POR LA DIAN)
				else if (empresa.StrTipoId.Equals("CC"))
					dian_tipoDocumento = "13";  //(LISTADO DE VALORES DEFINIDO POR LA DIAN)

				// Datos del obligado a facturar
				SupplierPartyType1 AccountingSupplierParty = new SupplierPartyType1();
				PartyType1 Party = new PartyType1();

				#region Tipo de persona

				AdditionalAccountIDType AdditionalAccountID = new AdditionalAccountIDType();
				AdditionalAccountID.Value = dian_tipoPersona;//Tipo de persona (LISTADO DE VALORES DEFINIDO POR LA DIAN)
				AccountingSupplierParty.AdditionalAccountID = AdditionalAccountID;
				#endregion

				#region Regimen
				PartyTaxSchemeType1[] PartyTaxSchemes = new PartyTaxSchemeType1[1];
				PartyTaxSchemeType1 PartyTaxScheme = new PartyTaxSchemeType1();
				TaxLevelCodeType TaxLevelCode = new TaxLevelCodeType();
				TaxLevelCode.Value = dian_regimen;
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
				ID.schemeID = dian_tipoDocumento; //Tipo documento (LISTADO DE VALORES DEFINIDO POR LA DIAN)
				ID.Value = empresa.StrNit;
				PartyIdentification.ID = ID;
				PartyIdentifications[0] = PartyIdentification;
				Party.PartyIdentification = PartyIdentifications;
				#endregion

				#region Datos de la empresa

				PartyLegalEntityType1[] PartyLegalEntitys = new PartyLegalEntityType1[1];
				PartyLegalEntityType1 PartyLegalEntity = new PartyLegalEntityType1();
				RegistrationNameType RegistrationName = new RegistrationNameType();
				RegistrationName.Value = empresa.StrEmpresa;
				PartyLegalEntity.RegistrationName = RegistrationName;
				PartyLegalEntitys[0] = PartyLegalEntity;
				Party.PartyLegalEntity = PartyLegalEntitys;

				#endregion



				// PENDIENTE!!
				#region Dirección
				LocationType2 PhysicalLocation = new LocationType2();
				AddressType1 Address = new AddressType1();

				DepartmentType Department = new DepartmentType();
				Department.Value = "ANTIOQUIA"; //Departamento (LISTADO DE VALORES DEFINIDO POR LA DIAN)
				Address.Department = Department;

				//CiudadBD clase_ciudad = new CiudadBD();
				//clase_ciudad.Model_db = this.Model_db;

				//clase_ciudad.Obtener<TblCiudades>("StrIdCiudad", empresa.StrCiudad);

				CityNameType City = new CityNameType();
				City.Value = "MEDELLIN"; //Ciudad (LISTADO DE VALORES DEFINIDO POR LA DIAN)
				Address.CityName = City;

				AddressLineType[] AddressLines = new AddressLineType[1];
				AddressLineType AddressLine = new AddressLineType();
				LineType Line = new LineType();
				Line.Value = empresa.StrDireccion;
				AddressLine.Line = Line;

				AddressLines[0] = AddressLine;
				Address.AddressLine = AddressLines;

				ContactType Contact = new ContactType();
				TelephoneType Telephone = new TelephoneType();
				Telephone.Value = empresa.StrTelefono;
				Contact.Telephone = Telephone;
				ElectronicMailType Mail = new ElectronicMailType();
				Mail.Value = empresa.StrMail;
				Contact.ElectronicMail = Mail;
				Party.Contact = Contact;

				WebsiteURIType Web = new WebsiteURIType();
				Web.Value = empresa.StrWeb;
				Party.WebsiteURI = Web;

				CountryType Country = new CountryType();
				IdentificationCodeType IdentificationCode = new IdentificationCodeType();
				IdentificationCode.Value = "CO"; //Pais (LISTADO DE VALORES DEFINIDO POR LA DIAN)
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


				/* Tipo de persona en el ERP 
					 1 Natural
					 2 Jurídica                      
				   para la Dian
					 1 Persona jurídica
					 2 Persona natural             */

				string dian_tipoPersona = string.Empty;
				if (tercero.IntTipoPersona == 1)
					dian_tipoPersona = "2";  //(LISTADO DE VALORES DEFINIDO POR LA DIAN)         
				else if (tercero.IntTipoPersona == 2)
					dian_tipoPersona = "1";  //(LISTADO DE VALORES DEFINIDO POR LA DIAN)

				/* Tipo de regimen en el ERP 
					 1 Simplificado
					 2 Comun                  
					Para la Dian
					 0 Simplificado
					 2 Común                  */

				string dian_regimen = string.Empty;
				if (tercero.IntRegimen == 1)
					dian_regimen = "0";  //(LISTADO DE VALORES DEFINIDO POR LA DIAN)
				else if (tercero.IntRegimen == 2)
					dian_regimen = "2";  //(LISTADO DE VALORES DEFINIDO POR LA DIAN)

				string dian_tipoDocumento = string.Empty;
				if (tercero.StrTipoId.Equals("NI"))
					dian_tipoDocumento = "31";  //(LISTADO DE VALORES DEFINIDO POR LA DIAN)
				else if (tercero.StrTipoId.Equals("CC"))
					dian_tipoDocumento = "13";  //(LISTADO DE VALORES DEFINIDO POR LA DIAN)

				// Datos del adquiriente de la factura
				CustomerPartyType1 AccountingCustomerParty = new CustomerPartyType1();
				PartyType1 Party = new PartyType1();

				#region Tipo de persona
				AdditionalAccountIDType AdditionalAccountID = new AdditionalAccountIDType();
				AdditionalAccountID.Value = dian_tipoPersona;//Tipo de persona (LISTADO DE VALORES DEFINIDO POR LA DIAN)
				AccountingCustomerParty.AdditionalAccountID = AdditionalAccountID;
				#endregion

				#region Regimen
				PartyTaxSchemeType1[] PartyTaxSchemes = new PartyTaxSchemeType1[1];
				PartyTaxSchemeType1 PartyTaxScheme = new PartyTaxSchemeType1();
				TaxLevelCodeType TaxLevelCode = new TaxLevelCodeType();
				TaxLevelCode.Value = dian_regimen;
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
				ID.schemeID = dian_tipoDocumento; //Tipo documento (LISTADO DE VALORES DEFINIDO POR LA DIAN)
				ID.Value = tercero.StrIdTercero;
				PartyIdentification.ID = ID;
				PartyIdentifications[0] = PartyIdentification;
				Party.PartyIdentification = PartyIdentifications;
				#endregion

				#region Datos personales segun el tipo de persona
				if (dian_tipoPersona.Equals("2"))//Persona natural
				{
					PersonType1 Person = new PersonType1();

					FirstNameType FirstName = new FirstNameType();
					FirstName.Value = tercero.StrNombre1;
					Person.FirstName = FirstName;

					MiddleNameType MiddleName = new MiddleNameType();
					if (tercero.StrNombre2 != null && !tercero.StrNombre2.Equals(string.Empty))
					{
						MiddleName.Value = tercero.StrNombre2;
					}
					Person.MiddleName = MiddleName;

					FamilyNameType FamilyName = new FamilyNameType();
					FamilyName.Value = string.Format("{0} {1}", tercero.StrApellido1, tercero.StrApellido2);
					Person.FamilyName = FamilyName;

					Party.Person = Person;
				}
				else if (dian_tipoPersona.Equals("1")) //Persona juridica
				{
					PartyLegalEntityType1[] PartyLegalEntitys = new PartyLegalEntityType1[1];
					PartyLegalEntityType1 PartyLegalEntity = new PartyLegalEntityType1();
					RegistrationNameType RegistrationName = new RegistrationNameType();
					RegistrationName.Value = tercero.StrNombre;
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
				Line.Value = tercero.StrDireccion;
				AddressLine.Line = Line;

				AddressLines[0] = AddressLine;
				Address.AddressLine = AddressLines;

				ContactType Contact = new ContactType();
				TelephoneType Telephone = new TelephoneType();
				Telephone.Value = tercero.IntTelefono.ToString();
				Contact.Telephone = Telephone;
				ElectronicMailType Mail = new ElectronicMailType();
				Mail.Value = tercero.StrMail;
				Contact.ElectronicMail = Mail;
				Party.Contact = Contact;

				// PENDIENTE!!
				CountryType Country = new CountryType();
				IdentificationCodeType IdentificationCode = new IdentificationCodeType();
				IdentificationCode.Value = "CO"; //Pais (LISTADO DE VALORES DEFINIDO POR LA DIAN)
				Country.IdentificationCode = IdentificationCode;
				Address.Country = Country;

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
		public static TaxTotalType1[] ObtenerImpuestos(List<DocumentoDetalle> documentoDetalle)
		{
			try
			{
				if (documentoDetalle == null || documentoDetalle.Count == 0)
					throw new Exception("El detalle del documento es inválido.");

				//Toma los impuestos de IVA que tiene el producto en el detalle del documento
				var impuestos_iva = documentoDetalle.Select(_impuesto => new { _impuesto.TblProductos.IntIva, _impuesto.TblProductos.TblIvas.intValor, Recursos.TipoImpuestos.Iva, _impuesto.TblProductos.TblIvas.StrDescripcion }).Distinct();

				List<DocumentoImpuestos> doc_impuestos = new List<DocumentoImpuestos>();

				decimal BaseImponibleImpuesto = 0;

				foreach (var item in impuestos_iva)
				{
					DocumentoImpuestos imp_doc = new DocumentoImpuestos();
					List<DocumentoDetalle> doc_ = documentoDetalle.Where(docDet => docDet.TblProductos.IntIva == item.IntIva).ToList();
					BaseImponibleImpuesto = decimal.Round(documentoDetalle.Where(docDet => docDet.TblProductos.IntIva == item.IntIva).Sum(docDet => docDet.IntValorUnitario * docDet.IntCantidad), 2);

					imp_doc.Codigo = item.IntIva;
					imp_doc.Nombre = item.StrDescripcion;
					imp_doc.Porcentaje = decimal.Round(item.intValor.Value, 2);
					imp_doc.TipoImpuesto = item.Iva;
					imp_doc.BaseImponible = BaseImponibleImpuesto;
					foreach (var docDet in doc_)
					{
						imp_doc.ValorImpuesto = decimal.Round(imp_doc.ValorImpuesto + docDet.IntValorIva, 2);
					}
					doc_impuestos.Add(imp_doc);
				}

				//Toma el impuesto al consumo de los productos que esten el detalle
				var impuesto_consumo = documentoDetalle.Select(_consumo => new { _consumo.TblProductos.IntImpConsumo, _consumo.TblProductos.TblImpuesto1.intValor, Recursos.TipoImpuestos.Consumo, _consumo.TblProductos.TblImpuesto1.StrDescripcion }).Distinct();
				decimal BaseImponibleImpConsumo = 0;

				//Valida si hay algun producto con impuesto al consumo
				if (impuesto_consumo.Count() > 0)
				{
					foreach (var item in impuesto_consumo)
					{
						if (item.IntImpConsumo != 0)
						{
							DocumentoImpuestos imp_doc = new DocumentoImpuestos();
							List<DocumentoDetalle> doc_ = documentoDetalle.Where(docDet => docDet.TblProductos.IntImpuesto1 == item.Consumo).ToList();
							BaseImponibleImpConsumo = decimal.Round(documentoDetalle.Where(docDet => docDet.TblProductos.IntImpuesto1 == item.Consumo).Sum(docDet => docDet.IntValorUnitario * docDet.IntCantidad), 2);

							imp_doc.Codigo = item.IntImpConsumo.ToString();
							imp_doc.Nombre = item.StrDescripcion;
							imp_doc.Porcentaje = decimal.Round(item.intValor.Value, 2);
							imp_doc.BaseImponible = BaseImponibleImpConsumo;
							foreach (var docDet in doc_)
							{
								imp_doc.ValorImpuesto = decimal.Round(imp_doc.ValorImpuesto + docDet.IntVrImpuesto1, 2);
							}
							doc_impuestos.Add(imp_doc);
						}

					}
				}

				//var retencion = documentoDetalle.Select(_retencion => new { _retencion.TblProductos.IntRetencion, _retencion.TblProductos.TblRetencionFte.IntBase, Recursos.TipoImpuestos.Retencion, _retencion.TblProductos.TblRetencionFte.StrDescripcion, _retencion.TblProductos.TblRetencionFte.IntIdRetencion, _retencion.TblProductos.TblRetencionFte.IntPorcentaje, _retencion.TblProductos.TblRetencionFte.IntPorcentajePn }).Distinct();
				//decimal BaseImponibleRetencion = 0;


				/*if (retencion.Count() > 0)
				{
					TblTerceros tercero = new TblTerceros();

					foreach (var item in retencion)
					{
						DocumentoImpuestos imp_doc = new DocumentoImpuestos();
						List<DocumentoDetalle> doc_ = documentoDetalle.Where(docDet => docDet.TblProductos.IntRetencion == item.IntRetencion).ToList();
						BaseImponibleRetencion = decimal.Round(documentoDetalle.Where(docDet => docDet.TblProductos.IntRetencion == item.IntRetencion).Sum(docDet => docDet.IntValorUnitario * docDet.IntCantidad), 2);

						imp_doc.Codigo = item.IntIdRetencion.ToString();
						imp_doc.Nombre = item.StrDescripcion;
						
						//Porcentaje de retencion segun el tipo de Persona: 1-Natural, 2-Juridica
						if (tercero.IntTipoPersona == 1)
						{
							imp_doc.Porcentaje = decimal.Round(item.IntPorcentajePn, 2);
						}
						else
						{
							imp_doc.Porcentaje = decimal.Round(item.IntPorcentaje, 2);
						}
						imp_doc.BaseImponible = BaseImponibleRetencion;
						foreach (var docDet in doc_)
						{
							imp_doc.ValorImpuesto = decimal.Round(imp_doc.ValorImpuesto + docDet.IntReteFte,2);
						}
						doc_impuestos.Add(imp_doc);

					}


				}*/


				TaxTotalType1[] TaxTotals = new TaxTotalType1[doc_impuestos.Count];

				int contador = 0;
				foreach (var item in doc_impuestos)
				{

					TaxTotalType1 TaxTotal = new TaxTotalType1();

					#region Importe Impuesto: Importe del impuesto retenido
					TaxAmountType TaxAmount = new TaxAmountType();
					TaxAmount.currencyID = CurrencyCodeContentType.COP;
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
					TaxableAmount.currencyID = CurrencyCodeContentType.COP;
					TaxableAmount.Value = decimal.Round(item.BaseImponible, 2);
					TaxSubtotal.TaxableAmount = TaxableAmount;
					#endregion

					#region Importe Impuesto (detalle): Importe del impuesto retenido
					//Valor total del impuesto retenido
					TaxAmountType TaxAmountSubtotal = new TaxAmountType();
					TaxAmountSubtotal.currencyID = CurrencyCodeContentType.COP;
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
					Name.Value = item.Nombre;

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

				MonetaryTotalType1 LegalMonetaryTotal = new MonetaryTotalType1();

				#region Total Importe bruto antes de impuestos
				// cbc:LineExtensionAmount [0..1]    The total of Line Extension Amounts net of tax and settlement discounts, but inclusive of any applicable rounding amount.
				//	Total importe bruto, suma de los importes brutos de las líneas de la factura.
				LineExtensionAmountType LineExtensionAmount = new LineExtensionAmountType();
				LineExtensionAmount.currencyID = CurrencyCodeContentType.COP;
				LineExtensionAmount.Value = decimal.Round(documento.IntValor, 2);
				LegalMonetaryTotal.LineExtensionAmount = LineExtensionAmount;
				#endregion

				#region Valor total base imponible (generó impuestos)
				//Total Base Imponible (Importe Bruto+Cargos-Descuentos): Base imponible para el cálculo de los impuestos.
				//Subtotal de la factura.
				TaxExclusiveAmountType TaxExclusiveAmount = new TaxExclusiveAmountType();
				TaxExclusiveAmount.currencyID = CurrencyCodeContentType.COP;
				TaxExclusiveAmount.Value = decimal.Round(documento.IntIva, 2);
				LegalMonetaryTotal.TaxExclusiveAmount = TaxExclusiveAmount;
				#endregion

				//Opcional no usado por la DIAN, las partes pueden definir un significado o simplemente omitirlo
				/*#region Valor total base no imponible (no generó impuestos)
				TaxInclusiveAmountType TaxInclusiveAmount = new TaxInclusiveAmountType();
				TaxInclusiveAmount.currencyID = CurrencyCodeContentType.COP;
				TaxInclusiveAmount.Value = decimal.Round(documento.IntSubtotal, 2);
				LegalMonetaryTotal.TaxInclusiveAmount = TaxInclusiveAmount;
				#endregion*/

				#region Descuentos
				// Total de Descuentos: Suma de todos los descuentos presentes
				AllowanceTotalAmountType AllowanceTotalAmount = new AllowanceTotalAmountType();
				AllowanceTotalAmount.currencyID = CurrencyCodeContentType.COP;
				AllowanceTotalAmount.Value = decimal.Round(documento.IntDDescuento, 2);
				LegalMonetaryTotal.AllowanceTotalAmount = AllowanceTotalAmount;

				#endregion

				#region Valor total de pago //  Total de Factura =  Valor total bases - Valor descuentos + Valor total Impuestos - Valor total impuestos retenidos
				PayableAmountType PayableAmount = new PayableAmountType();
				PayableAmount.currencyID = CurrencyCodeContentType.COP;
				PayableAmount.Value = decimal.Round(documento.IntTotal, 2);
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
		public static CreditNoteLineType[] ObtenerDetalleDocumento(List<DocumentoDetalle> documentoDetalle)
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
					decimal valorTotal = DocDet.IntCantidad * DocDet.IntValorUnitario;
					CreditNoteLineType CreditNoteLine = new CreditNoteLineType();

					#region Id producto definido por la Dian (Contador de productos iniciando desde 1)
					IDType ID = new IDType();
					ID.Value = contadorProducto.ToString();
					CreditNoteLine.ID = ID;

					CreditNoteLine.UUID = new UUIDType();
					CreditNoteLine.UUID.Value = DocDet.NotaCredito.StrFacturaECufe;

					#endregion

					#region Cantidad producto
					CreditedQuantityType CreditedQuantity = new CreditedQuantityType();
					CreditedQuantity.Value = decimal.Round(DocDet.IntCantidad, 2);
					CreditNoteLine.CreditedQuantity = CreditedQuantity;


					#endregion

					#region Valor Total
					LineExtensionAmountType LineExtensionAmount = new LineExtensionAmountType();
					LineExtensionAmount.currencyID = CurrencyCodeContentType.COP; //(LISTADO DE VALORES DEFINIDO POR LA DIAN)
					LineExtensionAmount.Value = decimal.Round(valorTotal, 2);
					CreditNoteLine.LineExtensionAmount = LineExtensionAmount;
					#endregion

					#region Datos producto
					ItemType Item = new ItemType();

					#region Descripcion producto
					DescriptionType[] Descriptions = new DescriptionType[1];
					DescriptionType Description = new DescriptionType();
					Description.Value = DocDet.TblProductos.StrDescripcion;
					Descriptions[0] = Description;
					Item.Description = Descriptions;
					#endregion

					#region Id producto definido por la Empresa ***///
					ItemIdentificationType CatalogueItemIdentification = new ItemIdentificationType();
					IDType IDItem = new IDType();
					IDItem.Value = DocDet.StrProducto;
					CatalogueItemIdentification.ID = IDItem;
					Item.CatalogueItemIdentification = CatalogueItemIdentification;
					#endregion

					CreditNoteLine.Item = Item;

					#endregion

					#region Valor Unitario producto
					PriceType Price = new PriceType();
					PriceAmountType PriceAmount = new PriceAmountType();
					PriceAmount.currencyID = CurrencyCodeContentType.COP; //(LISTADO DE VALORES DEFINIDO POR LA DIAN)
					PriceAmount.Value = decimal.Round(DocDet.IntValorUnitario, 2);
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
