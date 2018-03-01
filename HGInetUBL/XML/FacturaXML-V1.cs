using HGInetUBL.XML;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace HGInetUBL
{
    public class FacturaXMLv1
    {
        string documento_obligado = string.Empty;

        /// <summary>
        /// Llena los objetos con la informacion que corresponde a una factura
        /// </summary>
        public void Factura()
        {
            try
            {
                DateTime fecha = DateTime.Now;
                InvoiceType factura = new InvoiceType();
                XmlSerializerNamespaces namespaces_xml = NamespacesXML.Obtener();

                #region factura.UBLVersionID //Versión de los esquemas UBL
                factura.UBLVersionID = new UBLVersionIDType()
                {
                    Value = Recursos.VersionesDIAN.UBLVersionID
                };
                #endregion

                #region factura.ProfileID //Versión del documento DIAN_UBL.xsd publicado por la DIAN
                factura.ProfileID = new ProfileIDType()
                {
                    Value = Recursos.VersionesDIAN.ProfileID
                };
                #endregion


                #region factura.ID //Número de documento: Número de factura o factura cambiaria.
                factura.ID = new IDType()
                {
                    Value = "990000000"
                };
                #endregion

                #region factura.IssueDate //Fecha de la factura
                factura.IssueDate = new IssueDateType()
                {
                    Value = Convert.ToDateTime(fecha.ToString(Fecha.formato_fecha_hginet))
                };
                #endregion

                #region factura.IssueTime //Hora de la factura
                factura.IssueTime = new IssueTimeType()
                {
                    Value = fecha.ToString(Fecha.formato_hora_completa)
                    // Value = fecha
                };
                #endregion

                #region factura.InvoiceTypeCode //Tipo de Documento (factura)
                //Indicar si es una factura de venta o una factura cambiaria de compraventa
                factura.InvoiceTypeCode = new InvoiceTypeCodeType()
                {
                    Value = "1",
                    listAgencyName = "CO, DIAN (Direccion de Impuestos y Aduanas Nacionales)",
                    listAgencyID = "195",
                    listSchemeURI = "http://www.dian.gov.co/contratos/facturaelectronica/v1/InvoiceType"
                };
                #endregion

                #region Note
                factura.Note = new NoteType[]
				{
					new NoteType(){
						Value="Resolución Dian Nro 9000000033394696 del 2017-02-07 del 990000000 al 995000000"
					}
				};
                #endregion

                #region factura.DocumentCurrencyCode //Divisa de la Factura
                //Divisa consolidada aplicable a toda la factura. Moneda con la que se presenta el documento
                factura.DocumentCurrencyCode = new DocumentCurrencyCodeType()
                {
                    Value = "COP"
                };
                #endregion

                #region factura.AccountingSupplierParty //Información del obligado a facturar
                factura.AccountingSupplierParty = new SupplierPartyType1()//Obligado a facturar
                {
                    //Tipo de persona (LISTADO DE VALORES DEFINIDO POR LA DIAN)
                    AdditionalAccountID = new AdditionalAccountIDType()
                    {
                        Value = "1" //Persona jurídica
                        //Value = "2"  //Persona natural 
                    },

                    Party = new PartyType1()
                    {
                        //NIT del obligado a facturar
                        PartyIdentification = new PartyIdentificationType[]
						{
							new PartyIdentificationType()
							{
								ID = new IDType()
								{								
									schemeAgencyName = "CO, DIAN (Direccion de Impuestos y Aduanas Nacionales)",
									schemeAgencyID="195",
									schemeID = "31", //Tipo de documento NIT (LISTADO DE VALORES DEFINIDO POR LA DIAN)								 
									Value =  "811021438"
								}
							}
						},

                        PartyName = new PartyNameType[]
						{
							new PartyNameType()
							{
								Name = new NameType1()
								{
									Value = "Herramientas de gestión informática"
								}							
							}
						},

                        //Dirección
                        PhysicalLocation = new LocationType2()
                        {

                            Address = new AddressType1()
                            {
                                Department = new DepartmentType()
                                {
                                    Value = "Antioquia"
                                },

                                CitySubdivisionName = new CitySubdivisionNameType()
                                {
                                    Value = ""
                                },

                                //Ciudad
                                CityName = new CityNameType()
                                {
                                    Value = "Medellín"
                                },

                                AddressLine = new AddressLineType[] 
								{ 
									new AddressLineType()
									{
										Line = new LineType()
										{
											Value="Calle 48 Nro 77C-06"
										}
									}
								},

                                Country = new CountryType()
                                {
                                    IdentificationCode = new IdentificationCodeType()
                                    {
                                        Value = "CO"
                                    }
                                }
                            }
                        },

                        //Tipo de régimen (LISTADO DE VALORES DEFINIDO POR LA DIAN)
                        PartyTaxScheme = new PartyTaxSchemeType1[1]
						{
							new PartyTaxSchemeType1()
							{
								TaxLevelCode = new TaxLevelCodeType()
								{									
									Value = "2" //Común 
									//Value = "0"//Simplificado
								},

								/*No usado por la DIAN, las partes pueden definir
								un significado o simplemente poner un elemento
								vacío ya que es obligatorio en UBL*/
								TaxScheme = new TaxSchemeType()
								{
									
								}
							}
						},

                        //Razón Social 
                        /*
                            Razón Social: Obligatorio en caso de
                            ser una persona jurídica. Razón social de la
                            empresa						 
                        */
                        PartyLegalEntity = new PartyLegalEntityType1[]
						{
							new PartyLegalEntityType1()
							{
								RegistrationName = new RegistrationNameType()
								{
									Value="HGI SAS"										
								}								
							}
						}
                    }
                };

                #endregion

                #region factura.AccountingCustomerParty //Información del Adquiriente
                /* Persona natural o jurídica que adquiere bienes y/o servicios y debe exigir factura
				51 o documento equivalente y, que tratándose de la factura electrónica, 
			    la recibe, rechaza, cuando
				52 sea del caso, y conserva para su posterior exhibición*/

                factura.AccountingCustomerParty = new CustomerPartyType1()
                {
                    //TIPO DE PERSONA (LISTADO DE VALORES DEFINIDO POR LA DIAN)
                    AdditionalAccountID = new AdditionalAccountIDType()//cuenta adicional
                    {
                        //Value = "1" //Persona jurídica
                        Value = "2"  //Persona natural 
                    },

                    Party = new PartyType1()
                    {
                        //"Número del Documento de Identidad del Adquiriente
                        PartyIdentification = new PartyIdentificationType[]
						{							 
							new PartyIdentificationType()
							{
								ID = new IDType()
								{									
									schemeAgencyName = "CO, DIAN (Direccion de Impuestos y Aduanas Nacionales)",
									schemeAgencyID="195",	
									schemeID = "13", //Tipo de documento (LISTADO DE VALORES DEFINIDO POR LA DIAN)								 
									Value =  "1037356221"
								},  
								
							}
						},

                        //Dirección
                        PhysicalLocation = new LocationType2()
                        {
                            Address = new AddressType1()
                            {
                                Department = new DepartmentType()
                                {
                                    Value = "Antioquia"
                                },

                                CitySubdivisionName = new CitySubdivisionNameType()
                                {
                                    Value = ""
                                },

                                //	//ciudad
                                CityName = new CityNameType()
                                {
                                    Value = "Medellín"
                                },

                                AddressLine = new AddressLineType[] 
								{ 
									new AddressLineType()
									{
										Line = new LineType()
										{
											Value="Calle 99 80-58"
										}
									}
								},

                                Country = new CountryType()
                                {
                                    IdentificationCode = new IdentificationCodeType()
                                    {
                                        Value = "CO"
                                    }
                                }
                            }
                        },

                        //Régimen (LISTADO DE VALORES DEFINIDO POR LA DIAN)
                        PartyTaxScheme = new PartyTaxSchemeType1[1]
						{
							new PartyTaxSchemeType1()
							{
								TaxLevelCode = new TaxLevelCodeType()
								{
									//Value = "2" //Común 
									Value = "0"//Simplificado
								}, 

								/*No usado por la DIAN, las partes pueden definir
								un significado o simplemente poner un elemento
								vacío ya que es obligatorio en UBL*/
								TaxScheme = new TaxSchemeType()
								{
									
								}
							}
						},


                        //Razón Social 
                        /*
                            Razón Social: Obligatorio en caso de
                            ser una persona jurídica. Razón social de la
                            empresa						 
                        */
                        //PartyLegalEntity = new PartyLegalEntityType1[]
                        //{
                        //	new PartyLegalEntityType1(){
                        //		RegistrationName = new RegistrationNameType()
                        //		{
                        //			Value="Daniela Monsalve"							
                        //		}
                        //	}
                        //}

                        //Si es persona natural se debe ingresar el nombre de la persona
                        Person = new PersonType1()
                        {
                            FirstName = new FirstNameType()//Primer nombre
                            {
                                Value = "Daniela"
                            },

                            MiddleName = new MiddleNameType()//Segundo nombre
                            {
                                Value = ""
                            },

                            FamilyName = new FamilyNameType()//Apellido
                            {
                                Value = "Monsalve"
                            }

                        },
                    }
                };
                #endregion

                #region	factura.TaxTotal //Impuesto y Impuesto Retenido
                /*Impuesto Retenido: Elemento raíz compuesto utilizado para informar de un impuesto	retenido. 
				 *Impuesto: Elemento raíz compuesto utilizado para informar de un impuesto.
				 */
                factura.TaxTotal = new TaxTotalType1[]
				{
					#region Impuesto 01 IVA
					new TaxTotalType1()
					{	
						/*Importe Impuesto: Importe del impuesto
						retenido*/
						TaxAmount = new TaxAmountType()
						{
							currencyID = CurrencyCodeContentType.COP,
							Value = 3800.00M 
						},

						/*Indica que el elemento es un Impuesto retenido
						(7.1.1) y no un impuesto (8.1.1)*/
						TaxEvidenceIndicator =  new TaxEvidenceIndicatorType()
						{
							Value = false
						},						

						TaxSubtotal = new TaxSubtotalType1[]
						{
							new TaxSubtotalType1()						 
							{ 
								/*Base Imponible: Base
								Imponible sobre la que se calcula la retención
								de impuesto*/
								//Base Imponible = Importe bruto + cargos - descuentos
								TaxableAmount = new TaxableAmountType()
								{
									currencyID=CurrencyCodeContentType.COP,								 
									Value = 20000.00M
								},

								/*Importe Impuesto (detalle):
								Importe del impuesto retenido*/
								TaxAmount = new TaxAmountType()
								{
									currencyID = CurrencyCodeContentType.COP,
									Value = 3800.00M
								},

								/*Porcentaje: Porcentaje a aplicar*/
								Percent =  new PercentType(){
									Value = 19
								},

								/*Tipo: Tipo o clase impuesto. Concepto
									fiscal por el que se tributa. Debería si un
									campo que referencia a una lista de códigos. En
									la lista deberían aparecer los impuestos
									estatales o nacionales*/
								TaxCategory = new TaxCategoryType()
								{
									TaxScheme = new TaxSchemeType()
									{
										ID =  new IDType()
										{
											Value= "01"
										},

										Name = new NameType1()
										{
											Value="IVA"
										}										
									}
								}
							 },							 
						 },
					},

						 new TaxTotalType1()
						{	
							/*Importe Impuesto: Importe del impuesto
							retenido*/
						TaxAmount = new TaxAmountType()
						{
							currencyID = CurrencyCodeContentType.COP,
							Value = 0.00M
						}, 

						/*Indica que el elemento es un Impuesto retenido
						(7.1.1) y no un impuesto (8.1.1)*/
						TaxEvidenceIndicator =  new TaxEvidenceIndicatorType()
						{
							Value = false
						},

						
						TaxSubtotal = new TaxSubtotalType1[]
						{

						 new TaxSubtotalType1()						 
							{ 
								/*Base Imponible: Base
								Imponible sobre la que se calcula la retención
								de impuesto*/
								//Base Imponible = Importe bruto + cargos - descuentos
								TaxableAmount = new TaxableAmountType()
								{
									currencyID=CurrencyCodeContentType.COP,								 
									Value = 20000.00M
								},

								/*Importe Impuesto (detalle):
								Importe del impuesto retenido*/
								TaxAmount = new TaxAmountType()
								{
									currencyID = CurrencyCodeContentType.COP,
									Value = 0.00M
								},

								/*Porcentaje: Porcentaje a aplicar*/
								Percent =  new PercentType(){
									Value = 0.00M
								},

								/*Tipo: Tipo o clase impuesto. Concepto
									fiscal por el que se tributa. Debería si un
									campo que referencia a una lista de códigos. En
									la lista deberían aparecer los impuestos
									estatales o nacionales*/
								TaxCategory = new TaxCategoryType()
								{
									TaxScheme = new TaxSchemeType()
									{
										ID =  new IDType()
										{
											Value= "02"
										},

										Name = new NameType1()
										{
											Value="VALOR TOTAL DE IMPUESTO AL CONSUMO"											
										}										
									}
								}
							 },
						},
						},
						
						 new TaxTotalType1()
						{
							/*Importe Impuesto: Importe del impuesto
						retenido*/
						TaxAmount = new TaxAmountType()
						{
							currencyID = CurrencyCodeContentType.COP,
							Value = 0.00M
						},
						/*Indica que el elemento es un Impuesto retenido
						(7.1.1) y no un impuesto (8.1.1)*/
						TaxEvidenceIndicator =  new TaxEvidenceIndicatorType()
						{
							Value = false
						},

						
						TaxSubtotal = new TaxSubtotalType1[]
						{

						 new TaxSubtotalType1()						 
							{ 
								/*Base Imponible: Base
								Imponible sobre la que se calcula la retención
								de impuesto*/
								//Base Imponible = Importe bruto + cargos - descuentos
								TaxableAmount = new TaxableAmountType()
								{
									currencyID=CurrencyCodeContentType.COP,								 
									Value = 20000.00M 
								},

								/*Importe Impuesto (detalle):
								Importe del impuesto retenido*/
								TaxAmount = new TaxAmountType()
								{
									currencyID = CurrencyCodeContentType.COP,
									Value = 0.00M
								},

								/*Porcentaje: Porcentaje a aplicar*/
								Percent =  new PercentType(){
									Value = 0.00M
								},

								/*Tipo: Tipo o clase impuesto. Concepto
									fiscal por el que se tributa. Debería si un
									campo que referencia a una lista de códigos. En
									la lista deberían aparecer los impuestos
									estatales o nacionales*/
								TaxCategory = new TaxCategoryType()
								{
									TaxScheme = new TaxSchemeType()
									{
										ID =  new IDType()
										{
											Value= "03"
										},

										Name = new NameType1()
										{
											Value="VALOR TOTAL DE ICA"											
										}										
									}
								}
							 },
						}

							 
					}
					
					#endregion
				};

                #endregion

                #region factura.LegalMonetaryTotal //Datos Importes Totales
                /*Agrupación de campos
				relativos a los importes totales aplicables a la
				factura. Estos importes son calculados teniendo
				en cuenta las líneas de factura y elementos a
				nivel de factura, como descuentos, cargos,
				impuestos, etc*/
                factura.LegalMonetaryTotal = new MonetaryTotalType1()
                {
                    /*Total Importe bruto antes de impuestos:
                        Total importe bruto, suma de los importes brutos
                        de las líneas de la factura.*/
                    LineExtensionAmount = new LineExtensionAmountType()
                    {
                        currencyID = CurrencyCodeContentType.COP,
                        Value = 20000.00M
                    },

                    /*Total Base Imponible (Importe
                        Bruto+Cargos-Descuentos): Base imponible para el
                        cálculo de los impuestos.*/
                    TaxExclusiveAmount = new TaxExclusiveAmountType()
                    {
                        currencyID = CurrencyCodeContentType.COP,
                        Value = 20000.00M
                    },

                    /*Total de Factura: Total importe bruto +
                        Total Impuestos-Total Impuesto Retenidos*/
                    PayableAmount = new PayableAmountType()
                    {
                        currencyID = CurrencyCodeContentType.COP,
                        Value = 23800.00M
                    },

                };
                #endregion

                #region factura.InvoiceLine  //Línea de Factura
                //Elemento que agrupa todos los campos de una línea de factura
                //Detalle del documento
                factura.InvoiceLine = new InvoiceLineType1[]
				{
					new InvoiceLineType1(){

						ID = new IDType()
						{
							Value = "1"//Consecutivo de la linea
						},

						InvoicedQuantity = new InvoicedQuantityType()
						{
							Value = 1 //Cantidad del producto
						},

						/*Costo Total: Coste Total. Resultado:
						Unidad de Medida x Precio Unidad.*/
						LineExtensionAmount = new LineExtensionAmountType()
						{
							currencyID =  CurrencyCodeContentType.COP,
							Value = 20000.00M
						},

						Item =  new ItemType1() 
						{
							//nombre del producto
							Description = new DescriptionType[]
							{
								new DescriptionType {
									Value = "AA cambio kumis vaso * 160"
								}
							},

							//Codigo del producto
							CatalogueItemIdentification= new ItemIdentificationType()
							{
								ID =  new IDType()
								{
									Value = "10617"
								}
							}


						}, 

						Price = new PriceType1()
						{
							/*Precio Unitario: Precio unitario de
							la unidad de bien o servicio servido/prestado,
							en la moneda indicada en la Cabecera de la
							Factura. Siempre sin Impuestos*/
							PriceAmount = new PriceAmountType()
							{
								currencyID = CurrencyCodeContentType.COP,//Tipo de moneda								
								Value = 20000.00M
							}
						}
					}

				};
                #endregion

                #region factura.UUID //CUFE:Codigo Unico de Facturacion Electronica.
                /*  CUFE: Obligatorio si es factura nacional.
					Codigo Unico de Facturacion Electronica. Campo que verifica la integridad de la información
					recibida, es un campo generado por el sistema Numeración de facturación, está pendiente
					definir su contenido. Esta Encriptado. La factura electrónica tiene una firma electrónica
					basada en firma digital según la política de  firma propuesta. La integridad de la factura ya
					viene asegurada por la firma digital, no es	necesaria ninguna medida más para asegurar que
					ningún campo ha sido alterado*/
                factura.UUID = new UUIDType()
                {
                    schemeName = "CUFE",
                    schemeAgencyName = "CO, DIAN (Direccion de Impuestos y Aduanas Nacionales)",
                    schemeAgencyID = "195",
                    Value = CrearCodigoCUFE(factura)
                };
                #endregion

                #region factura.UBLExtensions

                XmlDocument doc = new XmlDocument();
                doc.LoadXml("<firma></firma>");

                factura.UBLExtensions = new UBLExtensionType[]
				{		
					new UBLExtensionType()
					{
						//ExtensionContent =  ExtensionDian.Obtener()						
					},	
			
					new UBLExtensionType()
					{			
						ExtensionContent = doc.DocumentElement						
					}
				};
                #endregion

                #region CrearArchivoXML
                string ruta = CrearArchivo(factura, factura.ID.Value, documento_obligado, namespaces_xml);

                #endregion
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        /// <summary>
        /// Crea el codigo CUFE
        /// </summary>
        /// <param name="factura">Objeto de tipo InvoiceType</param>
        public string CrearCodigoCUFE(InvoiceType factura)
        {
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
            //	DateTime fecha = factura.IssueDate.Value;

            DateTime fecha = factura.IssueDate.Value;
            DateTime fecha_hora = Convert.ToDateTime(factura.IssueTime.Value);
            TimeSpan hora = new TimeSpan(fecha_hora.Hour, fecha_hora.Minute, fecha_hora.Second);
            fecha = fecha.Date + hora;



            string NumFac = factura.ID.Value;
            string FecFac = fecha.ToString(Fecha.formato_fecha_java);
            decimal ValFac = factura.LegalMonetaryTotal.LineExtensionAmount.Value;

            //Impuesto 1
            string CodImp1 = "01";
            decimal ValImp1 = 0;

            //Impuesto 2
            string CodImp2 = "02";
            decimal ValImp2 = 0;

            //Impuesto 3
            string CodImp3 = "03";
            decimal ValImp3 = 0;

            for (int i = 0; i < factura.TaxTotal.Count(); i++)
            {
                for (int j = 0; j < factura.TaxTotal[i].TaxSubtotal.Count(); j++)
                {
                    codigo_impuesto = factura.TaxTotal[i].TaxSubtotal[j].TaxCategory.TaxScheme.ID.Value;

                    if (codigo_impuesto.Equals("01"))
                    {

                        ValImp1 = factura.TaxTotal[i].TaxSubtotal[j].TaxAmount.Value;
                    }
                    else if (codigo_impuesto.Equals("02"))
                    {
                        ValImp2 = factura.TaxTotal[i].TaxSubtotal[j].TaxAmount.Value;

                    }
                    else if (codigo_impuesto.Equals("03"))
                    {
                        ValImp3 = factura.TaxTotal[i].TaxSubtotal[j].TaxAmount.Value;
                    }
                }
            }

            decimal ValImp = factura.LegalMonetaryTotal.PayableAmount.Value;

            string NitOFE = string.Empty;

            for (int contador_obligado_facturar = 0; contador_obligado_facturar < factura.AccountingSupplierParty.Party.PartyIdentification.Count(); contador_obligado_facturar++)
            {
                NitOFE = factura.AccountingSupplierParty.Party.PartyIdentification[contador_obligado_facturar].ID.Value;
                documento_obligado = NitOFE;
            }

            string TipAdq = string.Empty;
            string NumAdq = string.Empty;

            for (int contador_adquiriente = 0; contador_adquiriente < factura.AccountingCustomerParty.Party.PartyTaxScheme.Count(); contador_adquiriente++)
            {
                TipAdq = factura.AccountingCustomerParty.Party.PartyIdentification[contador_adquiriente].ID.schemeID;
                NumAdq = factura.AccountingCustomerParty.Party.PartyIdentification[contador_adquiriente].ID.Value;
            }

            string ClTec = "dd85db55545bd6566f36b0fd3be9fd8555c36e";

            string cufe = NumFac
                + FecFac
                + ValFac.ToString().Replace(",", ".")
                + CodImp1
                + ValImp1.ToString().Replace(",", ".")
                + CodImp2
                + ValImp2.ToString().Replace(",", ".")
                + CodImp3
                + ValImp3.ToString().Replace(",", ".")
                + ValImp.ToString().Replace(",", ".")
                + NitOFE
                + TipAdq
                + NumAdq
                + ClTec
            ;

            string cufe_encriptado = Encriptar.Encriptar_SHA1(cufe);
            return cufe_encriptado;
            #endregion
        }

        /// <summary>
        /// Crea el archivo XML
        /// </summary>
        /// <param name="factura">Objeto de tipo InvoiceType</param>
        /// <param name="codigo_factura">Consecutivo de la factura</param>
        /// <param name="identificacion_obligado">Identificación del obligado a facturar</param>
        /// <param name="namespaces_xml">Namespaces indicados por la DIAN</param>
        public string CrearArchivo(InvoiceType factura, string consecutivo_factura, string identificacion_obligado, XmlSerializerNamespaces namespaces_xml)
        {
            try
            {
                string nombre_archivo_xml = NombramientoArchivo.ObtenerXml(consecutivo_factura, identificacion_obligado, TipoDocumento.Factura);
                string nombre_archivo_zip = NombramientoArchivo.ObtenerZip(consecutivo_factura, identificacion_obligado, TipoDocumento.Factura);
                string ruta = @"C:\Users\dmonsalve\Downloads\xml\";
                string ruta_xml = ruta + nombre_archivo_xml;
                Xml.GuardarObjeto(factura, ruta, nombre_archivo_xml, namespaces_xml);
                //	Zip.Comprimir(ruta, nombre_archivo_xml);
                //	Archivo.ModificarNombre(ruta,( nombre_archivo_xml + ".zip"), nombre_archivo_zip);

                //      FirmaXML.Firmar(ruta_xml);

                //string nombre_certificado = "HERRAMIENTAS DE GESTION INFORMATICA S.A.S";
                ////string serial = "009060F261CE5CF8";

                //X509Certificate2 certificado = Certificados.Obtener(nombre_certificado, string.Empty);

                //Firma.FirmarXml(certificado, ruta_xml);

                var indiceNodo = 1;
                string rr = "~/Default.aspx?nit=" + indiceNodo + "&empresa=" + indiceNodo + "&compania=" + indiceNodo;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.PreserveWhitespace = true;
                xmlDoc.Load(ruta_xml);

                var nodoExtension = xmlDoc.GetElementsByTagName("ExtensionContent", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2")
               .Item(indiceNodo);
                if (nodoExtension == null)
                    throw new InvalidOperationException("No se pudo encontrar el nodo ExtensionContent en el XML");
                nodoExtension.RemoveAll();

                xmlDoc.Save(ruta_xml);
                return ruta_xml;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

    }
}
