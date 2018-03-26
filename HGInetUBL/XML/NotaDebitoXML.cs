using LibreriaGlobalHGInet.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using LibreriaGlobalHGInet.Objetos;

namespace HGInetUBL.XML
{
	public class NotaDebitoXML
	{
		/// <summary>
		/// Llena los objetos con la informacion que corresponde a una nota debito
		/// </summary>
		public void NotaDebito()
		{
			//DebitNoteLineType DebitNoteType = new DebitNoteLineType();
			//UBLExtensionsType UBLExtensions = new UBLExtensionsType();
			//UBLVersionIDType UBLVersionID = new UBLVersionIDType();
			//ProfileIDType ProfileID = new ProfileIDType();
			//IDType ID = new IDType();
			//UUIDType UUID = new UUIDType();
			//IssueDateType IssueDate = new IssueDateType();
			//IssueTimeType IssueTime = new IssueTimeType();
			//NoteType Note = new NoteType();
			//DocumentCurrencyCodeType DocumentCurrencyCode = new DocumentCurrencyCodeType();
			//PeriodType InvoicePeriod = new PeriodType();
			//OrderReferenceType OrderReference = new OrderReferenceType();
			//BillingReferenceType BillingReference = new BillingReferenceType();
			//DocumentReferenceType DespatchDocumentReference = new DocumentReferenceType();
			//DocumentReferenceType ReceiptDocumentReference = new DocumentReferenceType();
			//DocumentReferenceType ContractDocumentReference = new DocumentReferenceType();
			//DocumentReferenceType AdditionalDocumentReference = new DocumentReferenceType();
			//SupplierPartyType AccountingSupplierParty = new SupplierPartyType();
			//CustomerPartyType AccountingCustomerParty = new CustomerPartyType();
			//PartyType PayeeParty = new PartyType();
			//PaymentType1 PrepaidPayment = new PaymentType1();
			//ExchangeRateType PaymentExchangeRate = new ExchangeRateType();
			//TaxTotalType TaxTotal = new TaxTotalType();
			//MonetaryTotalType1 LegalMonetaryTotal = new MonetaryTotalType1();
			//DebitNoteLineType DebitNoteLine = new DebitNoteLineType();

			
			DebitNoteType nota_debito = new DebitNoteType();

			#region nota_debito.UBLExtensions
			//Firma con certificado digital
		//	XmlElement firma = FirmaXML.Firmar();

			//extenciones
			nota_debito.UBLExtensions = new UBLExtensionType[1]{
				new UBLExtensionType(){
				//	ExtensionContent =  firma
				}				
			};
			#endregion

			#region nota_debito.UBLVersionID //Versión de los esquemas UBL

			nota_debito.UBLVersionID = new UBLVersionIDType()
			{
                Value = Recursos.VersionesDIAN.UBLVersionID
			};
			#endregion

			#region nota_debito.ProfileID //Versión del documento DIAN_UBL.xsd publicado por la DIAN
			nota_debito.ProfileID = new ProfileIDType()
			{
                Value = Recursos.VersionesDIAN.ProfileID
			};
			#endregion

			#region nota_debito.ID //Número de documento: Número de nota_debito o nota_debito cambiaria.
			nota_debito.ID = new IDType()
			{
				Value = "00000001"
			};
			#endregion

			#region nota_debito.UUID //CUFE:Codigo Unico de facturacion Electronica.
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
			nota_debito.UUID = new UUIDType()
			{
				Value = ""
			};
			#endregion

			#region nota_debito.IssueDate //Fecha de la nota_debito

			nota_debito.IssueDate = new IssueDateType()
			{
				Value = DateTime.Now
			};
			#endregion

			#region nota_debito.IssueTime //Hora de la nota_debito

			nota_debito.IssueTime = new IssueTimeType()
			{
			//	Value = DateTime.Now
			};
			#endregion

			#region nota_debito.Note //Información adicional
			//Texto libre, relativo al documento
			nota_debito.Note = new NoteType[1]{
				new NoteType(){
					Value = "Esto es una prueba"
				}
			};
			#endregion

			#region nota_debito.DocumentCurrencyCode //Divisa de la nota_debito
			//Divisa consolidada aplicable a toda la nota_debito
			nota_debito.DocumentCurrencyCode = new DocumentCurrencyCodeType()
			{
				Value = "1"
			};
			#endregion

			#region nota_debito.InvoicePeriod //Periodo de nota_debitoción
			//Intervalo de fechas a las que referencia la nota_debito
			nota_debito.InvoicePeriod = new PeriodType()
			{
				DurationMeasure = new DurationMeasureType()
				{
					unitCode = UnitCodeContentType.DAY,
					Value = 0
				}
			};
			#endregion

			#region nota_debito.OrderReference //Referencia Documento (orden)
			//Referencia a un documento
			nota_debito.OrderReference = new OrderReferenceType[1]
			{
				new OrderReferenceType()
			};
			#endregion

			#region nota_debito.BillingReference //Referencia Documento (orden)
			//Referencia a un documento
			nota_debito.BillingReference = new BillingReferenceType[1]
			{//PENDIENTE	
				new BillingReferenceType()
			};
			#endregion

			#region nota_debito.DespatchDocumentReference //Referencia Documento (despacho)
			//Referencia a un documento
			nota_debito.DespatchDocumentReference = new DocumentReferenceType[1]
			{		
				new DocumentReferenceType()
			};
			#endregion

			#region nota_debito.ReceiptDocumentReference //Referencia Documento (recepción)
			//Referencia a un documento
			nota_debito.ReceiptDocumentReference = new DocumentReferenceType[1]
			{//PENDIENTE	
				new DocumentReferenceType()
			};
			#endregion

			#region nota_debito.ContractDocumentReference //Referencia Documento (contrato)
			//Referencia a un documento
			nota_debito.ContractDocumentReference = new DocumentReferenceType[1]
			{	
				new DocumentReferenceType()//PENDIENTE
			};
			#endregion

			#region nota_debito.AdditionalDocumentReference //Referencia Documento
			nota_debito.AdditionalDocumentReference = new DocumentReferenceType[1]
			{
				new DocumentReferenceType()//PENDIENTE
			};
			#endregion

			#region nota_debito.AccountingSupplierParty //Información del obligado a facturar

			/*Campos segun peru*/
			nota_debito.AccountingSupplierParty = new SupplierPartyType1()//Obligado a facturar
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

			};

			#endregion

			#region nota_debito.AccountingCustomerParty //Información del Adquiriente
			/* Persona natural o jurídica que adquiere bienes y/o servicios y debe exigir factura
				51 o documento equivalente y, que tratándose de la factura electrónica, 
			    la recibe, rechaza, cuando
				52 sea del caso, y conserva para su posterior exhibición*/

			nota_debito.AccountingCustomerParty = new CustomerPartyType1()
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

			};

			#endregion

			#region nota_debito.PayeeParty //Receptor del Pago
			/*Receptor del Pago: Participante, Entidad,
				Departamento, Unidad, destinatario de la
				factura. Suele coincidir con el obligado a
				facturar Ver composición en la estructura común*/
			nota_debito.PayeeParty = new PartyType()
			{

			};//PENDIENTE


			#endregion

			#region	nota_debito.TaxTotal //Impuesto y Impuesto Retenido
			/*Impuesto Retenido: Elemento raíz compuesto utilizado para informar de un impuesto	retenido. 
			 *Impuesto: Elemento raíz compuesto utilizado para informar de un impuesto.
			 */
			nota_debito.TaxTotal = new TaxTotalType1[1]
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
			};

			#endregion

			#region nota_debito.LegalMonetaryTotal //Datos Importes Totales
			/*Agrupación de campos
				relativos a los importes totales aplicables a la
				nota_debito. Estos importes son calculados teniendo
				en cuenta las líneas de nota_debito y elementos a
				nivel de nota_debito, como descuentos, cargos,
				impuestos, etc*/
			nota_debito.LegalMonetaryTotal = new MonetaryTotalType1()
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
			};
			#endregion

			#region nota_debito.CreditNoteLine  //Línea de nota_debito
			//Elemento que agrupa todos los campos de una línea de nota_debito
			nota_debito.DebitNoteLine = new DebitNoteLineType[1]
			{
				new DebitNoteLineType()
			};
			#endregion

			#region CrearArchivoXML
			//Obtiene los Namespaces 
			XmlSerializerNamespaces namespaces_xml = NamespacesXML.Obtener();
			CrearArchivo(nota_debito, "811021438", namespaces_xml);
			#endregion
		}

		/// <summary>
		/// Crea el archivo XML
		/// </summary>
		/// <param name="nota_dedito">Objeto de tipo DebitNoteType</param>
		/// <param name="identificacion">Identificación del obligado a facturar</param>
		/// <param name="namespaces_xml">Namespaces indicados por la DIAN</param>
		public void CrearArchivo(DebitNoteType nota_dedito, string identificacion, XmlSerializerNamespaces namespaces_xml)
		{
			try
			{
				string nombre_archivo_xml = NombramientoArchivo.ObtenerXml(nota_dedito.ID.Value, identificacion, TipoDocumento.NotaDebito);
				string nombre_archivo_zip = NombramientoArchivo.ObtenerZip(nota_dedito.ID.Value, identificacion, TipoDocumento.NotaDebito);
				string ruta = @"C:\Users\dmonsalve\Downloads\xml\";

				Xml.GuardarObjeto(nota_dedito, ruta, nombre_archivo_xml, namespaces_xml);
				//	Zip.Comprimir(ruta, nombre_archivo_xml);
				//	Archivo.ModificarNombre(ruta,( nombre_archivo_xml + ".zip"), nombre_archivo_zip);

			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

	}
}
