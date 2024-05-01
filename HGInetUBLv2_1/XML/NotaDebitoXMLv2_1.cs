using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.Objetos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using HGInetUBLv2_1.DianListas;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.HgiNet.Controladores;
using HGInetUBLv2_1.XML;

namespace HGInetUBLv2_1
{
	public class NotaDebitoXML2_1
	{

		/// <summary>
		/// Crea el XML con la informacion de la nota debito en formato UBL
		/// </summary>
		/// <param name="id_documento">Id de seguridad del documento generado por la plataforma</param>
		/// <param name="documento">Objeto de tipo NotaDebito que contiene la informacion de la nota debito</param>
		/// <param name="resolucion">Objeto de tipo Extension DIAN </param>
		/// <param name="tipo">Indica el tipo de documento</param>
		/// <returns>Ruta donde se guardo el archivo XML</returns>  
		public static FacturaE_Documento CrearDocumento(Guid id_documento, NotaDebito documento, HGInetMiFacturaElectonicaData.ModeloServicio.ExtensionDian resolucion, TipoDocumento tipo, string ambiente_dian, ref string cadena_cufe)
		{
			try
			{
				if (documento == null)
					throw new Exception("La documento es inválido.");

				//Obtiene el nombre del archivo XML
				string nombre_archivo_xml = NombramientoArchivo.ObtenerXml(documento.Documento.ToString(), documento.DatosObligado.Identificacion, tipo, documento.Prefijo);

				DebitNoteType nota_debito = new DebitNoteType();
				XmlSerializerNamespaces namespaces_xml = NamespacesXML.ObtenerNamespaces(tipo);

				#region nota_debito.UBLVersionID //Versión de los esquemas UBL

				nota_debito.UBLVersionID = new UBLVersionIDType()
				{
					Value = "UBL 2.1" //Recursos.VersionesDIAN.UBLVersionID
				};

				#endregion

				#region nota_debito.CustomizationID //

				nota_debito.CustomizationID = new CustomizationIDType();
				nota_debito.CustomizationID.Value = documento.TipoOperacion.ToString();

				#endregion

				#region nota_debito.ProfileID //Versión del documento DIAN_UBL.xsd publicado por la DIAN

				nota_debito.ProfileID = new ProfileIDType()
				{
					Value = "DIAN 2.1: Nota Débito de Factura Electrónica de Venta" //Recursos.VersionesDIAN.ProfileID
				};

				#endregion

				//---Ambiente de Pruebas
				nota_debito.ProfileExecutionID = new ProfileExecutionIDType()
				{
					Value = ambiente_dian//"2"
				};

				#region nota_debito.ID //Número de documento: Número de nota_debito o nota_debito cambiaria.

				string numero_documento = "";
				if (!string.IsNullOrEmpty(documento.Prefijo))
					numero_documento = string.Format("{0}", documento.Prefijo);

				numero_documento = string.Format("{0}{1}", numero_documento, documento.Documento.ToString());

				nota_debito.ID = new IDType();
				IDType ID = new IDType();
				ID.Value = numero_documento;
				nota_debito.ID = ID;

				#endregion

				#region nota_debito.IssueDate //Fecha de la nota_debito

				IssueDateType IssueDate = new IssueDateType();
				IssueDate.Value = Convert.ToDateTime(documento.Fecha.ToString(Fecha.formato_fecha_hginet));
				nota_debito.IssueDate = IssueDate;

				#endregion

				#region nota_debito.IssueTime //Hora de la nota_debito

				IssueTimeType IssueTime = new IssueTimeType();
				IssueTime.Value = documento.Fecha.ToString(Fecha.formato_hora_zona);//documento.Fecha.AddHours(5).ToString("HH:mm:sszzz");//Convert.ToDateTime(documento.Fecha.ToString(Fecha.formato_hora_completa)).AddHours(5);//Convert.ToDateTime(documento.Fecha.ToString(Fecha.formato_hora_completa));
				nota_debito.IssueTime = IssueTime;

				#endregion


				#region nota_debito.Note //Información adicional

				//Texto libre, relativo al documento
				//nota_debito.Note = new NoteType[1]
				//{
				//	new NoteType()
				//	{
				//		Value = documento.Nota
				//	}
				//};

				List<string> notas_documento = new List<string>();

				// agrega los campos adicionales en el XML
				//notas_documento = FormatoNotas.CamposPredeterminados(documento.DocumentoFormato);

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

				nota_debito.Note = Notes;


				#endregion

				#region nota_debito.DocumentCurrencyCode //Divisa de la nota_debito

				//Divisa consolidada aplicable a toda la nota_debito
				DocumentCurrencyCodeType DocumentCurrencyCode = new DocumentCurrencyCodeType();
				DocumentCurrencyCode.Value =
					documento.Moneda; //(LISTADO DE VALORES DEFINIDO POR LA DIAN)                
				nota_debito.DocumentCurrencyCode = DocumentCurrencyCode;

				#endregion

				//PENDIENTE CODIGO CONCEPTO

				#region nota_debito.DiscrepancyResponse // Documento afectado y sustentacion de la afectacion

				/*Contiene el tipo de nota de crédito, el sustento de la emisión de la nota de crédito, así como la 
				identificación de la factura de venta electrónica afectada.*/

				//Documento afectado por la Nota credito
				ResponseType[] DiscrepancyResponse = new ResponseType[1];
				ResponseType Reference = new ResponseType();
				if (!string.IsNullOrEmpty(documento.DocumentoRef))
				{
					Reference.ReferenceID = new ReferenceIDType();
					Reference.ReferenceID.Value = documento.DocumentoRef;
				}
				Reference.ResponseCode = new ResponseCodeType();
				Reference.ResponseCode.Value = documento.Concepto;
				DescriptionType[] DescriptionType = new DescriptionType[1];
				DescriptionType Description = new DescriptionType();
				//Obtengo la descripcion segun el codig9o del concepto
				ListaConceptoNotaDebito list_Concepto = new ListaConceptoNotaDebito();
				ListaItem desc_concepto = list_Concepto.Items.Where(d => d.Codigo.Equals(documento.Concepto))
					.FirstOrDefault();
				Description.Value = desc_concepto.Descripcion;
				DescriptionType[0] = Description;

				Reference.Description = DescriptionType;
				DiscrepancyResponse[0] = Reference;

				nota_debito.DiscrepancyResponse = DiscrepancyResponse;

				#endregion

				#region nota_debito.BillingReference //Referencia Documento (factura)

				//Referencia a un documento afectar
				/*
				DocumentReferenceType[] DocumentReferenceType = new DocumentReferenceType[1];
				DocumentReferenceType DocumentReference = new DocumentReferenceType();
				DocumentReference.ID = new IDType();
				DocumentReference.ID.Value = documento.DocumentoRef.ToString();
				DocumentReference.UUID = new UUIDType();
				DocumentReference.UUID.Value = documento.CufeFactura;
				DocumentReference.UUID.schemeName = "CUFE-SHA384";
				DocumentReference.IssueDate = new IssueDateType();
				DocumentReference.IssueDate.Value = documento.FechaFactura;
				DocumentReferenceType[0] = DocumentReference;
				nota_debito.AdditionalDocumentReference = DocumentReferenceType;*/


				if (!string.IsNullOrEmpty(documento.DocumentoRef) && !string.IsNullOrEmpty(documento.CufeFactura))
				{
					nota_debito.BillingReference = new BillingReferenceType[1];

					BillingReferenceType DocReference = new BillingReferenceType();
					DocumentReferenceType DocumentReference = new DocumentReferenceType();
					DocumentReference.ID = new IDType();
					DocumentReference.ID.Value = documento.DocumentoRef;
					DocumentReference.UUID = new UUIDType();
					DocumentReference.UUID.Value = documento.CufeFactura;
					DocumentReference.IssueDate = new IssueDateType();
					DocumentReference.IssueDate.Value = documento.FechaFactura;
					DocReference.InvoiceDocumentReference = DocumentReference;

					nota_debito.BillingReference[0] = DocReference;
				}

				#endregion


				#region nota_debito.OrderReference //Referencia Documento (orden)

				//Referencia un documento de pedido

				if (!string.IsNullOrEmpty(documento.PedidoRef) && documento.OrderReference == null)
				{
					documento.OrderReference = new ReferenciaAdicional();
					documento.OrderReference.Documento = documento.PedidoRef;
				}

				if (documento.OrderReference != null)
				{
					OrderReferenceType DocOrderReference = new OrderReferenceType();
					DocOrderReference.ID = new IDType() { Value = documento.OrderReference.Documento };  //new IDType() { Value = documento.PedidoRef.ToString() };
					nota_debito.OrderReference = DocOrderReference;
				}
				#endregion

				#region nota_debito.DespatchDocumentReference 

				if (documento.DespatchDocument != null)
				{
					//Referencia un documento
					nota_debito.DespatchDocumentReference = new DocumentReferenceType[documento.DespatchDocument.Count];
					List<DocumentReferenceType> List_DocumentReference = new List<DocumentReferenceType>();
					foreach (var Despatch in documento.DespatchDocument)
					{
						DocumentReferenceType DReference = new DocumentReferenceType();
						DReference.ID = new IDType();
						DReference.ID.Value = Despatch.Documento; //(string.IsNullOrEmpty(documento.PedidoRef)) ? string.Empty : documento.PedidoRef.ToString();
						List_DocumentReference.Add(DReference);
					}
					nota_debito.DespatchDocumentReference = List_DocumentReference.ToArray();
				}

				#endregion

				//Referencia Adicional si se utiliza y cuando es contingencia
				#region nota_debito.AdditionalDocumentReference

				if (documento.DocumentosReferencia != null)
				{
					List<DocumentReferenceType> AdditionalDocument = new List<DocumentReferenceType>();
					foreach (var item in documento.DocumentosReferencia)
					{
						DocumentReferenceType ReceiptDocument = new DocumentReferenceType();
						ReceiptDocument.ID = new IDType();
						ReceiptDocument.ID.Value = item.Documento;
						ReceiptDocument.IssueDate = new IssueDateType();
						ReceiptDocument.IssueDate.Value = item.FechaReferencia;
						ReceiptDocument.DocumentTypeCode = new DocumentTypeCodeType();
						ReceiptDocument.DocumentTypeCode.Value = item.CodigoReferencia;
						AdditionalDocument.Add(ReceiptDocument);
					}
					nota_debito.AdditionalDocumentReference = AdditionalDocument.ToArray();
				}

				#endregion

				#region nota_debito.ReceiptDocument 

				if (documento.ReceiptDocument != null)
				{
					//Referencia un documento
					nota_debito.ReceiptDocumentReference = new DocumentReferenceType[documento.ReceiptDocument.Count];
					List<DocumentReferenceType> List_DocumentReference = new List<DocumentReferenceType>();
					foreach (var Receipt in documento.ReceiptDocument)
					{
						DocumentReferenceType DReference = new DocumentReferenceType();
						DReference.ID = new IDType();
						DReference.ID.Value = Receipt.Documento; //(string.IsNullOrEmpty(documento.PedidoRef)) ? string.Empty : documento.PedidoRef.ToString();
						List_DocumentReference.Add(DReference);
					}
					nota_debito.ReceiptDocumentReference = List_DocumentReference.ToArray();
				}

				#endregion

				#region PaymentExchangeRate - Conversión de divisas: cac:PaymentExchangeRate
				if (!documento.Moneda.Equals("COP") && documento.Trm != null)
				{
					ExchangeRateType PaymentExchangeRate = new ExchangeRateType();
					//---5.3.3. Moneda (ISO 4217):
					//cbc: SourceCurrencyCode [1..1] La moneda de referencia para este tipo de cambio; La moneda a partir de la cual se realiza el cambio.
					PaymentExchangeRate.SourceCurrencyCode = new SourceCurrencyCodeType();
					PaymentExchangeRate.SourceCurrencyCode.Value = documento.Moneda;
					//---Valor debe ser 1.00 si es pesos colombianos
					//*cbc: SourceCurrencyBaseRate[0..1] En el caso de una moneda de origen con denominaciones de pequeño valor, la unidad base.
					PaymentExchangeRate.SourceCurrencyBaseRate = new SourceCurrencyBaseRateType();
					PaymentExchangeRate.SourceCurrencyBaseRate.Value = 1.00M;
					//* cbc: TargetCurrencyCode [1..1] La moneda de destino para este tipo de cambio; La moneda a la que se realiza el cambio.
					PaymentExchangeRate.TargetCurrencyCode = new TargetCurrencyCodeType();
					PaymentExchangeRate.TargetCurrencyCode.Value = (documento.Trm != null ? documento.Trm.Moneda : documento.Moneda);//documento.Moneda;
																																	 //* cbc: TargetCurrencyBaseRate [0..1] En el caso de una moneda de destino con denominaciones de valor pequeño, la base de la unidad.
					PaymentExchangeRate.TargetCurrencyBaseRate = new TargetCurrencyBaseRateType();
					PaymentExchangeRate.TargetCurrencyBaseRate.Value = 1.00M;
					//* cbc: CalculationRate [0..1] El factor aplicado a la moneda de origen para calcular la moneda de destino.
					PaymentExchangeRate.CalculationRate = new CalculationRateType();
					PaymentExchangeRate.CalculationRate.Value = (documento.Trm != null ? documento.Trm.Valor + 0.00M : 1.00M);//1.0M;
																															  //* cbc: Fecha [0..1] La fecha en que se estableció el tipo de cambio.
					PaymentExchangeRate.Date = new DateType1();
					PaymentExchangeRate.Date.Value = ((documento.Trm != null) ? Convert.ToDateTime(documento.Trm.FechaTrm.ToString(Fecha.formato_fecha_hginet)) : Convert.ToDateTime(documento.Fecha.ToString(Fecha.formato_fecha_hginet)));
					nota_debito.PaymentExchangeRate = PaymentExchangeRate;
				}
				#endregion

				#region nota_debito.AccountingSupplierParty // Información del obligado a facturar

				nota_debito.AccountingSupplierParty = TerceroXML.ObtenerObligado(documento.DatosObligado, documento.Prefijo, true);

				#endregion

				#region nota_debito.AccountingCustomerParty //Información del Adquiriente

				/* Persona natural o jurídica que adquiere bienes y/o servicios y debe exigir factura
				51 o documento equivalente y, que tratándose de la factura electrónica, 
			    la recibe, rechaza, cuando
				52 sea del caso, y conserva para su posterior exhibición*/

				nota_debito.AccountingCustomerParty = TerceroXML.ObtenerAquiriente(documento.DatosAdquiriente);

				#endregion

				#region nota_debito.PayeeParty //Receptor del Pago

				/*Receptor del Pago: Participante, Entidad,
				Departamento, Unidad, destinatario de la
				factura. Suele coincidir con el obligado a
				facturar Ver composición en la estructura común

				nota_debito.PayeeParty = new PartyType()
				{

				};*/ //PENDIENTE


				#endregion

				#region	nota_debito.TaxTotal //Impuesto y Impuesto Retenido

				/*Impuesto Retenido: Elemento raíz compuesto utilizado para informar de un impuesto	retenido. 
			    *Impuesto: Elemento raíz compuesto utilizado para informar de un impuesto.*/

				nota_debito.TaxTotal = ImpuestosXML.ObtenerImpuestos(documento.DocumentoDetalles.ToList(), documento.Moneda, documento.VersionAplicativo);

				#endregion

				#region nota_debito.CreditNoteLine  //Línea de nota_debito

				//Elemento que agrupa todos los campos de una línea de nota_debito
				nota_debito.DebitNoteLine = ObtenerDetalleDocumento(documento.DocumentoDetalles.ToList(),documento.CufeFactura, documento.Moneda, documento.DatosObligado.Identificacion);

				#endregion

				#region --- Esta seccion se debe validar, como aplica en Nota Debito forma de pago.
				List<PaymentMeansType> PaymentMeans = new List<PaymentMeansType>();
				PaymentMeansType PaymentMean = new PaymentMeansType();
				PaymentMeansCodeType MeansCode = new PaymentMeansCodeType();
				MeansCode.Value = "ZZZ";
				PaymentMean.ID = new IDType();
				PaymentMean.ID.Value = "1";
				PaymentMean.PaymentID = new PaymentIDType[1];
				PaymentIDType Paymentid = new PaymentIDType();
				Paymentid.Value = (string.IsNullOrEmpty(documento.DocumentoRef)) ? "0" : documento.DocumentoRef;
				PaymentMean.PaymentID[0] = Paymentid;

				PaymentMean.PaymentMeansCode = MeansCode;
				PaymentMeans.Add(PaymentMean);
				nota_debito.PaymentMeans = PaymentMeans.ToArray();
				#endregion

				if (documento.Descuentos != null || documento.Cargos != null)
					nota_debito.AllowanceCharge = ValoresAdicionalesXML.ObtenerValoresAd(documento);

				nota_debito.LineCountNumeric = new LineCountNumericType();
				nota_debito.LineCountNumeric.Value = documento.DocumentoDetalles.Count;

				#region nota_debito.LegalMonetaryTotal //Datos Importes Totales

				/*Agrupación de campos
				relativos a los importes totales aplicables a la
				nota_debito. Estos importes son calculados teniendo
				en cuenta las líneas de nota_debito y elementos a
				nivel de nota_debito, como descuentos, cargos,
				impuestos, etc*/

				decimal subtotal = nota_debito.DebitNoteLine.Sum(s => s.LineExtensionAmount.Value);
				decimal base_impuesto = 0.00M;
				if ((documento.DocumentoDetalles.Sum(b => b.BaseImpuestoIva) == documento.DocumentoDetalles.Sum(s => s.ValorSubtotal)) && (documento.DocumentoDetalles.Where(g => g.ProductoGratis == true) == null))
				{
					base_impuesto = subtotal;
				}
				else
				{
					base_impuesto = nota_debito.DebitNoteLine.Sum(s => s.TaxTotal.Take(1).Sum(b => b.TaxSubtotal.Where(k => k.TaxableAmount != null).Sum(v => v.TaxableAmount.Value)));
				}
				decimal impuestos = nota_debito.TaxTotal.Sum(i => i.TaxAmount.Value);

				nota_debito.RequestedMonetaryTotal = TotalesXML.ObtenerTotales(documento, subtotal, impuestos, base_impuesto);
				#endregion

				#region nota_debito.UUID //CUDE:Codigo Unico de Documento Electronico.

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
				//-----Se agrega Ambiente al cual se va enviar el documento
				string CUFE = CalcularCUFE(nota_debito, resolucion.PinSoftware, documento.CufeFactura,nota_debito.ProfileExecutionID.Value, ref cadena_cufe);
				UUID.Value = CUFE;
				UUID.schemeName = "CUDE-SHA384";
				UUID.schemeID = nota_debito.ProfileExecutionID.Value;
				nota_debito.UUID = UUID;

				#endregion

				#region nota_debito.UBLExtensions

				XmlDocument doc = new XmlDocument();
				doc.LoadXml("<firma></firma>");

				List<UBLExtensionType> UBLExtensions = new List<UBLExtensionType>();

				//Informacion del QR
				string ruta_qr_Dian = string.Empty;
				if (nota_debito.ProfileExecutionID.Value.Equals("2"))
				{
					ruta_qr_Dian = string.Format("{0}{1}", "https://catalogo-vpfe-hab.dian.gov.co/document/searchqr?documentkey=", nota_debito.UUID.Value);
				}
				else
				{
					ruta_qr_Dian = string.Format("{0}{1}", "https://catalogo-vpfe.dian.gov.co/document/searchqr?documentkey=", nota_debito.UUID.Value);
				}

				// Extension del sector Salud
				if (documento.SectorSalud != null && documento.SectorSalud.CamposSector.Count > 0)
				{
					bool orden_diferente = false;
					//Se agrega validacion si es por este cliente que indica que el orden los 21 campos de sector salud debe ir como indica el Anexo de la resolucion 506 y no como indica la Resolucion 084
					if (!string.IsNullOrEmpty(documento.VersionAplicativo) && documento.VersionAplicativo.Contains("Ver. 202"))
					{
						orden_diferente = true;
					}
					UBLExtensionType UBLExtensionSector = new UBLExtensionType();
					UBLExtensionSector.ExtensionContent = ExtensionSector.Obtener(documento.SectorSalud, tipo, orden_diferente);
					UBLExtensions.Add(UBLExtensionSector);
				}

				// Extension de la Dian
				UBLExtensionType UBLExtensionDian = new UBLExtensionType();
				UBLExtensionDian.ExtensionContent = ExtensionDian.Obtener(resolucion, TipoDocumento.NotaDebito, nota_debito.ID.Value, ruta_qr_Dian);
				UBLExtensions.Add(UBLExtensionDian);

				//Extension de la firma
				UBLExtensionType UBLExtensionFirma = new UBLExtensionType();
				UBLExtensionFirma.ExtensionContent = doc.DocumentElement;
				UBLExtensions.Add(UBLExtensionFirma);

				nota_debito.UBLExtensions = UBLExtensions.ToArray();

				#endregion

				// convierte los datos del objeto en texto XML 
				StringBuilder txt_xml = ConvertirXML.Convertir(nota_debito, namespaces_xml, TipoDocumento.NotaDebito);

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
		/// <param name="nota_debito">Objeto de tipo CreditNoteType que contiene la informacion de la Nota Crédito</param>
		/// <param name="pin_software">pin del software autorizado</param>
		/// <param name="cufe_factura">Identificador de la factura Afectada</param>
		/// <param name="ambiente">Número de identificación del ambiente utilizado por el Obligado para emitir la factura Seccion 6.1.1 (1=AmbienteProduccion , 2: AmbientePruebas)</param>
		/// <returns></returns>
		public static string CalcularCUFE(DebitNoteType nota_debito, string pin_software, string cufe_factura,string ambiente, ref string cadena_cufe)
		{
			try
			{
				if (nota_debito == null)
					throw new Exception("Los datos de la factura son inválidos.");
				if (string.IsNullOrWhiteSpace(ambiente))
					throw new Exception("El ambiente es inválido.");

				#region Documentación de la creación código CUFE

				/*
				NumCr = Número de Nota Credito.
				Feccr = Fecha de Nota Credito en formato (Java) YYYYmmddHHMMss. 
				ValCr = Valor Nota Credito sin IVA, con punto decimal, con decimales a dos (2) dígitos, sin separadores de miles, ni símbolo pesos. 
				CodImp1 = 01 
				ValImp1 = Valor impuesto 01, con punto decimal, con decimales a dos (2) dígitos, sin separadores de miles, ni símbolo pesos. 
				CodImp2 = 02 
				ValImp2 = Valor impuesto 04, con punto decimal, con decimales a dos (2) dígitos, sin separadores de miles, ni símbolo pesos. 
				CodImp3 = 03 
				ValImp3 = Valor impuesto 03, con punto decimal, con decimales a dos (2) dígitos, sin separadores de miles, ni símbolo pesos.
				ValImp = Valor IVA, con punto decimal, con decimales a dos (2) dígitos, sin separadores de miles, ni símbolo pesos.
				NitOFE = NIT del Facturador Electrónico sin puntos ni guiones, sin digito de verificación. 
				NumAdq = Número de identificación del adquirente sin puntos ni guiones, sin digito de verificación. 
				Software-PIN = Pin del software registrado en el catalogo del participante, el cual no esta expresado en el XML 
				TipoAmbiente = Número de identificación del ambiente utilizado por el contribuyente para emitir la factura Seccion 6.1.1 (1=AmbienteProduccion , 2: AmbientePruebas) 

				Composición del CUFE = SHA-384(NumCr + Feccr + ValCr + CodImp1 + ValImp1 + CodImp2 + ValImp2 + CodImp3 + ValImp3 + ValTot + NitOFE +  NumAdq + Software-PIN + TipoAmbie)  
			
			
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
				Software-PIN = No se encuentra en el XML 
				NumAdq = /fe:CreditNote /fe:AccountingCustomerParty/fe:Party/cac:PartyIdentification/cbc:ID
				TipoAmb = /CreditNote/dcc:ProfileExecutionID
				*/

				#endregion

				#region Creación Código CUFE

				string codigo_impuesto = string.Empty;
				//DateTime fecha = nota_debito.IssueDate.Value;
				//DateTime fecha_hora = Convert.ToDateTime(nota_debito.IssueTime.Value);
				//TimeSpan hora = new TimeSpan(fecha_hora.Hour, fecha_hora.Minute, fecha_hora.Second);
				//fecha = fecha.Date + hora;

				string NumCr = nota_debito.ID.Value;
				string FecCr = string.Format("{0}{1}", nota_debito.IssueDate.Value.ToString("yyyy-MM-dd"), nota_debito.IssueTime.Value);//fecha.ToString(Fecha.formato_fecha_java);
				string ValCr = nota_debito.RequestedMonetaryTotal.LineExtensionAmount.Value.ToString();

				//Impuesto 1
				string CodImp1 = "01";
				decimal ValImp1 = 0.00M;

				//Impuesto 2
				string CodImp2 = "04";
				decimal ValImp2 = 0.00M;

				//Impuesto 3
				string CodImp3 = "03";
				decimal ValImp3 = 0.00M;

				for (int i = 0; i < nota_debito.TaxTotal.Count(); i++)
				{
					for (int j = 0; j < nota_debito.TaxTotal[i].TaxSubtotal.Count(); j++)
					{
						codigo_impuesto = nota_debito.TaxTotal[i].TaxSubtotal[j].TaxCategory.TaxScheme.ID.Value;

						if (codigo_impuesto.Equals("01"))
						{
							ValImp1 += nota_debito.TaxTotal[i].TaxSubtotal[j].TaxAmount.Value;
						}
						else if (codigo_impuesto.Equals("04"))
						{
							ValImp2 += nota_debito.TaxTotal[i].TaxSubtotal[j].TaxAmount.Value;
						}
						else if (codigo_impuesto.Equals("03"))
						{
							ValImp3 += nota_debito.TaxTotal[i].TaxSubtotal[j].TaxAmount.Value;
						}
					}
				}

				string ValImp = nota_debito.RequestedMonetaryTotal.PayableAmount.Value.ToString();

				string NitOFE = string.Empty;

				for (int contador_obligado_facturar = 0; contador_obligado_facturar < nota_debito.AccountingSupplierParty.Party.PartyTaxScheme.Count(); contador_obligado_facturar++)
				{
					NitOFE = nota_debito.AccountingSupplierParty.Party.PartyTaxScheme[contador_obligado_facturar].CompanyID.Value;
				}

				//string TipAdq = string.Empty;
				string NumAdq = string.Empty;

				for (int contador_adquiriente = 0; contador_adquiriente < nota_debito.AccountingCustomerParty.Party.PartyTaxScheme.Count(); contador_adquiriente++)
				{
					//TipAdq = nota_debito.AccountingCustomerParty.Party.PartyTaxScheme[contador_adquiriente].CompanyID.schemeName;
					NumAdq = nota_debito.AccountingCustomerParty.Party.PartyTaxScheme[contador_adquiriente].CompanyID.Value;
				}

				cadena_cufe = "NumDoc: " + NumCr + ", "
				              + "FechaDoc: " + FecCr + ", "
				              + "SubtotalDoc:" + ValCr.Replace(",", ".") + ", "
				              + "CodImp1: " + CodImp1 + ", "
				              + "ValImp1: " + ValImp1.ToString().Replace(",", ".") + ", "
				              + "CodImp2: " + CodImp2 + ", "
				              + "ValImp2: " + ValImp2.ToString().Replace(",", ".") + ", "
				              + "CodImp3: " + CodImp3 + ", "
				              + "ValImp3: " + ValImp3.ToString().Replace(",", ".") + ", "
				              + "TotalDoc: " + ValImp.Replace(",", ".") + ", "
				              + "NitObligado: " + NitOFE + ", "
				              + "NitAdquiriente: " + NumAdq + ", "
				              + "PinSW " + pin_software + ", "
				              + "Ambiente: " + ambiente + ", "
					;

				string cufe_encriptado = Ctl_CalculoCufe.CufeNotaDebitoV2(pin_software, String.Empty, NumCr, FecCr, NitOFE, ambiente, NumAdq, Convert.ToDecimal(ValImp), Convert.ToDecimal(ValCr), ValImp1, ValImp2, ValImp3, false);
				return cufe_encriptado;

				#endregion
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
		public static DebitNoteLineType[] ObtenerDetalleDocumento(List<DocumentoDetalle> documentoDetalle,string cufefactura, string moneda, string identificiacion_Obligado)
		{
			try
			{
				if (documentoDetalle == null || !documentoDetalle.Any())
					throw new Exception("El detalle del documento es inválido.");

				DebitNoteType nota = new DebitNoteType();
				DebitNoteLineType[] DebitNoteLineType = new DebitNoteLineType[documentoDetalle.Count()];

				int contadorPosicion = 0;
				int contadorProducto = 1;

				foreach (var DocDet in documentoDetalle)
				{

					//Crear Enumerable para que lea segun la moneda
					CurrencyCodeContentType moneda_detalle = Ctl_Enumeracion.ObtenerMoneda(moneda);

					DebitNoteLineType DebitNoteLine = new DebitNoteLineType();

					#region Id producto definido por la Dian (Contador de productos iniciando desde 1)

					IDType ID = new IDType();
					ID.Value = contadorProducto.ToString();
					DebitNoteLine.ID = ID;

					DebitNoteLine.UUID = new UUIDType();
					DebitNoteLine.UUID.Value = cufefactura;

					#endregion

					#region Cantidad producto

					DebitedQuantityType DebitedQuantity = new DebitedQuantityType();
					DebitedQuantity.Value = DocDet.Cantidad;
					DebitNoteLine.DebitedQuantity = DebitedQuantity;

					// Unidad de medida Ver lista de valores posibles en 6.3.6(Defecto codigo - 94)
					ListaUnidadesMedida list_unidad = new ListaUnidadesMedida();
					ListaItem unidad = list_unidad.Items.Where(d => d.Codigo.Equals(DocDet.UnidadCodigo)).FirstOrDefault();

					// Unidad de medida
					DebitedQuantity.unitCode = unidad.Codigo;
					DebitNoteLine.DebitedQuantity = DebitedQuantity;

					#endregion

					#region Valor Total

					LineExtensionAmountType LineExtensionAmount = new LineExtensionAmountType();
					LineExtensionAmount.currencyID =
						moneda_detalle.ToString(); //(LISTADO DE VALORES DEFINIDO POR LA DIAN)
					LineExtensionAmount.Value = DocDet.ValorSubtotal;// decimal.Round(DocDet.ValorSubtotal, 2);
					DebitNoteLine.LineExtensionAmount = LineExtensionAmount;

					#endregion

					if (DocDet.DescuentoValor > 0)
					{
						AllowanceChargeType[] AllowanceCharges = new AllowanceChargeType[1];
						AllowanceChargeType AllowanceCharge = new AllowanceChargeType();
						AllowanceCharge.BaseAmount = new BaseAmountType();
						AllowanceCharge.BaseAmount.currencyID = moneda_detalle.ToString();
						if (DocDet.DescuentoPorcentaje > 0)
						{
							decimal valorTotal_cal = DocDet.Cantidad * DocDet.ValorUnitario;
							AllowanceCharge.BaseAmount.Value = decimal.Round(valorTotal_cal, 6);
						}
						else
						{
							AllowanceCharge.BaseAmount.Value = 0.00M;
						}

						//decimal desc_cal = decimal.Round((AllowanceCharge.BaseAmount.Value * (DocDet.DescuentoPorcentaje / 100)), 2, MidpointRounding.AwayFromZero);
						//if ((AllowanceCharge.BaseAmount.Value - desc_cal) != DocDet.ValorSubtotal)
						//	DocDet.ValorSubtotal = AllowanceCharge.BaseAmount.Value - desc_cal;

						AllowanceCharge.ChargeIndicator = new ChargeIndicatorType();
						AllowanceCharge.ChargeIndicator.Value = false;
						AllowanceCharge.AllowanceChargeReasonCode = new AllowanceChargeReasonCodeType();
						AllowanceCharge.AllowanceChargeReasonCode.Value = "11"; /*** QUEMADO ***/
						AllowanceChargeReasonType[] AllowanceChargeReasonType = new AllowanceChargeReasonType[1];
						AllowanceChargeReasonType AllowanceChargeReason = new AllowanceChargeReasonType();
						AllowanceChargeReason.Value = "Descuento comercial"; /*** QUEMADO ***/
						AllowanceChargeReasonType[0] = AllowanceChargeReason;
						AllowanceCharge.AllowanceChargeReason = AllowanceChargeReasonType;
						AllowanceCharge.MultiplierFactorNumeric = new MultiplierFactorNumericType();
						AllowanceCharge.MultiplierFactorNumeric.Value = DocDet.DescuentoPorcentaje; //decimal.Round(DocDet.DescuentoPorcentaje, 6);
						AllowanceCharge.Amount = new AmountType2();
						AllowanceCharge.Amount.currencyID = moneda_detalle.ToString();
						AllowanceCharge.Amount.Value = DocDet.DescuentoValor;//decimal.Round(DocDet.DescuentoValor, 2);
						AllowanceCharges[0] = AllowanceCharge;

						DebitNoteLine.AllowanceCharge = AllowanceCharges;
					}

					#region Impuestos del producto

					// <cac:TaxTotal>
					List<TaxTotalType> TaxesTotal = new List<TaxTotalType>();

					if (DocDet.CalculaIVA < 2)
					{

						bool llenar_iva = false;
						if (DocDet.ValorImpuestoConsumo == 0 && DocDet.ImpoConsumoPorcentaje == 0 && DocDet.IvaValor > 0)
						{
							llenar_iva = true;
						}
						else if (DocDet.ValorImpuestoConsumo == 0 && DocDet.ImpoConsumoPorcentaje == 0 && DocDet.IvaValor == 0)
						{
							llenar_iva = true;
						}

						if (DocDet.IvaValor > 0 && llenar_iva == false)
						{
							llenar_iva = true;
						}

						if (llenar_iva == true)
						{
							//Grupo de campos para informaciones relacionadas con un tributo aplicable a esta línea de la factura 
							TaxTotalType TaxTotal = new TaxTotalType();

							//if (decimal.Round((DocDet.ValorSubtotal * (DocDet.IvaPorcentaje / 100)), 2, MidpointRounding.AwayFromZero) != DocDet.IvaValor)
							//DocDet.IvaValor = decimal.Round((DocDet.ValorSubtotal * (DocDet.IvaPorcentaje / 100)), 2, MidpointRounding.AwayFromZero);

							// importe total de impuestos, por ejemplo, IVA; la suma de los subtotales fiscales para cada categoría de impuestos dentro del esquema impositivo
							// <cbc:TaxAmount>
							TaxTotal.TaxAmount = new TaxAmountType()
							{
								currencyID = moneda_detalle.ToString(),
								Value = DocDet.IvaValor// decimal.Round(DocDet.IvaValor, 2)
							};

							// indicador que este total se reconoce como evidencia legal a efectos impositivos (verdadero)o no(falso).
							// <cbc:TaxEvidenceIndicator>
							TaxTotal.TaxEvidenceIndicator = new TaxEvidenceIndicatorType()
							{
								Value = false
							};

							RoundingAmountType Rouding = new RoundingAmountType();
							Rouding.Value = 0;
							Rouding.currencyID = moneda_detalle.ToString();
							TaxTotal.RoundingAmount = Rouding;

							// Debe ser informado un grupo de estos para cada tarifa. 
							// <cac:TaxSubtotal>
							TaxSubtotalType[] TaxesSubtotal = new TaxSubtotalType[1];


							#region impuesto: IVA 

							TaxSubtotalType TaxSubtotalIva = new TaxSubtotalType();

							// importe neto al que se aplica el porcentaje del impuesto (tasa) para calcular el importe del impuesto.
							//Base Imponible sobre la que se calcula el valor del tributo
							//----Se debe Solicitar la base con la que calculo el impuesto
							// <cbc:TaxAmount>
							TaxSubtotalIva.TaxableAmount = new TaxableAmountType();
							TaxSubtotalIva.TaxableAmount.currencyID = moneda_detalle.ToString();
							if (DocDet.ProductoGratis == true && DocDet.ValorImpuestoConsumo == 0 && DocDet.IvaPorcentaje >= 0)
							{
								TaxSubtotalIva.TaxableAmount.Value = decimal.Round((DocDet.Cantidad * DocDet.ValorUnitario) - DocDet.DescuentoValor, 6, MidpointRounding.AwayFromZero);
							}
							else
							{
								if (DocDet.BaseImpuestoIva > 0)
								{
									TaxSubtotalIva.TaxableAmount.Value = DocDet.BaseImpuestoIva;
								}
								else
								{
									TaxSubtotalIva.TaxableAmount.Value = DocDet.ValorSubtotal;
								}
							}


							// El monto de este subtotal fiscal.
							//Valor del tributo: producto del porcentaje aplicado sobre la base imponible
							// <cbc:TaxAmount>
							TaxSubtotalIva.TaxAmount = new TaxAmountType()
							{
								currencyID = moneda_detalle.ToString(),
								Value = DocDet.IvaValor
							};

							// categoría de impuestos aplicable a este subtotal.
							//Grupo de informaciones sobre el tributo 
							// <cac:TaxCategory>
							TaxCategoryType TaxCategoryIva = new TaxCategoryType();

							// tasa de impuesto de la categoría de impuestos aplicada a este subtotal fiscal, expresada como un porcentaje.
							//Tarifa del tributo
							// <cbc:Percent>
							if (DocDet.CalculaIVA > 0 && DocDet.IvaPorcentaje > 0)
								DocDet.IvaPorcentaje = 0.00M;
							TaxCategoryIva.Percent = new PercentType1()
							{
								Value = decimal.Round((DocDet.IvaPorcentaje + 0.00M), 2)
							};

							// <cac:TaxScheme>
							//Grupo de informaciones específicas sobre el tributo
							TaxSchemeType TaxSchemeIva = new TaxSchemeType();
							//Identificador del tributo
							TaxSchemeIva.ID = new IDType(); /*** QUEMADO ***/
							TaxSchemeIva.ID.Value = "01";//TipoImpuestos.Iva,
														 //Nombre del tributo
							TaxSchemeIva.Name = new NameType1();
							TaxSchemeIva.Name.Value = "IVA";/*** QUEMADO ***/

							TaxCategoryIva.TaxScheme = TaxSchemeIva;
							TaxSubtotalIva.TaxCategory = TaxCategoryIva;
							TaxesSubtotal[0] = TaxSubtotalIva;
							TaxTotal.TaxSubtotal = TaxesSubtotal;
							TaxesTotal.Add(TaxTotal);
						}

						#endregion
					}

					if (DocDet.ValorImpuestoConsumo > 0 && DocDet.ProductoGratis == false)
					{

						//Grupo de campos para informaciones relacionadas con un tributo aplicable a esta línea de la factura 
						TaxTotalType TaxTotal = new TaxTotalType();

						//if (decimal.Round((DocDet.ValorSubtotal * (DocDet.ImpoConsumoPorcentaje * 100)), 2) != DocDet.ValorImpuestoConsumo)
						//	DocDet.ValorImpuestoConsumo = decimal.Round((DocDet.ValorSubtotal * (DocDet.ImpoConsumoPorcentaje * 100)), 2);

						// importe total de impuestos, por ejemplo, IVA; la suma de los subtotales fiscales para cada categoría de impuestos dentro del esquema impositivo
						// <cbc:TaxAmount>
						TaxTotal.TaxAmount = new TaxAmountType()
						{
							currencyID = moneda_detalle.ToString(),
							Value = DocDet.ValorImpuestoConsumo //decimal.Round(DocDet.ValorImpuestoConsumo, 2)
						};

						// indicador que este total se reconoce como evidencia legal a efectos impositivos (verdadero)o no(falso).
						// <cbc:TaxEvidenceIndicator>
						TaxTotal.TaxEvidenceIndicator = new TaxEvidenceIndicatorType()
						{
							Value = false
						};

						RoundingAmountType Rouding = new RoundingAmountType();
						Rouding.Value = 0;
						Rouding.currencyID = moneda_detalle.ToString();
						TaxTotal.RoundingAmount = Rouding;

						// Debe ser informado un grupo de estos para cada tarifa. 
						// <cac:TaxSubtotal>
						TaxSubtotalType[] TaxesSubtotal = new TaxSubtotalType[1];

						TaxSubtotalType TaxSubtotalConsumo = new TaxSubtotalType();

						// El monto de este subtotal fiscal.
						//Valor del tributo: producto del porcentaje aplicado sobre la base imponible
						// <cbc:TaxAmount>
						TaxSubtotalConsumo.TaxAmount = new TaxAmountType()
						{
							currencyID = moneda_detalle.ToString(),
							Value = DocDet.ValorImpuestoConsumo
						};

						// categoría de impuestos aplicable a este subtotal.
						//Grupo de informaciones sobre el tributo 
						// <cac:TaxCategory>
						TaxCategoryType TaxCategoryConsumo = new TaxCategoryType();

						string codigo_impuesto = "04";
						//
						if (DocDet.ValorImpuestoConsumo > 0 && DocDet.ImpoConsumoPorcentaje == 0)
						{
							if (DocDet.Aiu != 4)
							{
								codigo_impuesto = "02";
							}
							else
							{
								codigo_impuesto = "22";
							}

							// importe neto al que se aplica el porcentaje del impuesto (tasa) para calcular el importe del impuesto.
							//Base Imponible sobre la que se calcula el valor del tributo
							//----Se debe Solicitar la base con la que calculo el impuesto
							TaxSubtotalConsumo.BaseUnitMeasure = new BaseUnitMeasureType()
							{
								unitCode = DebitedQuantity.unitCode,
								Value = DocDet.Cantidad //1.00M//decimal.Round(DocDet.Cantidad, 6)//
							};

							// tasa de impuesto de la categoría de impuestos aplicada a este subtotal fiscal, expresada como un porcentaje.
							//Tarifa del tributo
							// <cbc:Percent>
							TaxSubtotalConsumo.PerUnitAmount = new PerUnitAmountType()
							{
								currencyID = moneda_detalle.ToString(),
								Value = decimal.Round((DocDet.ValorImpuestoConsumo) / DocDet.Cantidad, 6) + 0.00M
							};

						}
						else
						{

							// importe neto al que se aplica el porcentaje del impuesto (tasa) para calcular el importe del impuesto.
							//Base Imponible sobre la que se calcula el valor del tributo
							//----Se debe Solicitar la base con la que calculo el impuesto
							TaxSubtotalConsumo.TaxableAmount = new TaxableAmountType()
							{
								currencyID = moneda_detalle.ToString(),
								Value = DocDet.ValorSubtotal
							};

							// tasa de impuesto de la categoría de impuestos aplicada a este subtotal fiscal, expresada como un porcentaje.
							//Tarifa del tributo
							// <cbc:Percent>
							TaxCategoryConsumo.Percent = new PercentType1()
							{
								Value = decimal.Round((DocDet.ImpoConsumoPorcentaje), 2)
							};
						}

						// <cac:TaxScheme>
						//Grupo de informaciones específicas sobre el tributo
						TaxSchemeType TaxSchemeConsumo = new TaxSchemeType();
						ListaTipoImpuesto list_tipoimp = new ListaTipoImpuesto();
						ListaItem tipoimp = list_tipoimp.Items.Where(d => d.Codigo.Equals(codigo_impuesto)).FirstOrDefault();
						//Identificador del tributo
						TaxSchemeConsumo.ID = new IDType(); /*** QUEMADO ***/
						TaxSchemeConsumo.ID.Value = tipoimp.Codigo;//"04";//TipoImpuestos.Consumo,
																   //Nombre del tributo
						TaxSchemeConsumo.Name = new NameType1();
						TaxSchemeConsumo.Name.Value = tipoimp.Nombre; //"INC"; /*** QUEMADO ***/

						TaxCategoryConsumo.TaxScheme = TaxSchemeConsumo;
						TaxSubtotalConsumo.TaxCategory = TaxCategoryConsumo;
						TaxesSubtotal[0] = TaxSubtotalConsumo;
						TaxTotal.TaxSubtotal = TaxesSubtotal;
						TaxesTotal.Add(TaxTotal);
					}
					else if (DocDet.ValorImpuestoConsumo > 0 && DocDet.ProductoGratis == true)
					{
						//Grupo de campos para informaciones relacionadas con un tributo aplicable a esta línea de la factura 
						TaxTotalType TaxTotal = new TaxTotalType();

						// importe total de impuestos, por ejemplo, IVA; la suma de los subtotales fiscales para cada categoría de impuestos dentro del esquema impositivo
						// <cbc:TaxAmount>
						TaxTotal.TaxAmount = new TaxAmountType()
						{
							currencyID = moneda_detalle.ToString(),
							Value = DocDet.ValorImpuestoConsumo// decimal.Round(DocDet.ValorImpuestoConsumo, 2)
						};

						// indicador que este total se reconoce como evidencia legal a efectos impositivos (verdadero)o no(falso).
						// <cbc:TaxEvidenceIndicator>
						TaxTotal.TaxEvidenceIndicator = new TaxEvidenceIndicatorType()
						{
							Value = false
						};

						RoundingAmountType Rouding = new RoundingAmountType();
						Rouding.Value = 0;
						Rouding.currencyID = moneda_detalle.ToString();
						TaxTotal.RoundingAmount = Rouding;

						// Debe ser informado un grupo de estos para cada tarifa. 
						// <cac:TaxSubtotal>
						TaxSubtotalType[] TaxesSubtotal = new TaxSubtotalType[1];

						TaxSubtotalType TaxSubtotalConsumo = new TaxSubtotalType();

						// importe neto al que se aplica el porcentaje del impuesto (tasa) para calcular el importe del impuesto.
						//Base Imponible sobre la que se calcula el valor del tributo
						//----Se debe Solicitar la base con la que calculo el impuesto
						TaxSubtotalConsumo.BaseUnitMeasure = new BaseUnitMeasureType()
						{
							unitCode = DebitedQuantity.unitCode,
							Value = DocDet.Cantidad //1.00M//decimal.Round(DocDet.Cantidad, 6)
						};

						// El monto de este subtotal fiscal.
						//Valor del tributo: producto del porcentaje aplicado sobre la base imponible
						// <cbc:TaxAmount>
						TaxSubtotalConsumo.TaxAmount = new TaxAmountType()
						{
							currencyID = moneda_detalle.ToString(),
							Value = DocDet.ValorImpuestoConsumo
						};

						// tasa de impuesto de la categoría de impuestos aplicada a este subtotal fiscal, expresada como un porcentaje.
						//Tarifa del tributo
						// <cbc:Percent>
						TaxSubtotalConsumo.PerUnitAmount = new PerUnitAmountType()
						{
							currencyID = moneda_detalle.ToString(),
							Value = decimal.Round((DocDet.ValorImpuestoConsumo) / DocDet.Cantidad, 6) + 0.00M//decimal.Round((DocDet.ValorImpuestoConsumo) / TaxSubtotalConsumo.BaseUnitMeasure.Value, 2) + 0.00M//
						};

						// categoría de impuestos aplicable a este subtotal.
						//Grupo de informaciones sobre el tributo 
						// <cac:TaxCategory>
						TaxCategoryType TaxCategoryConsumo = new TaxCategoryType();

						string codigo_impuesto = string.Empty;
						//
						if (DocDet.ValorImpuestoConsumo > 0 && DocDet.ImpoConsumoPorcentaje == 0)
						{
							if (DocDet.Aiu != 4)
							{
								codigo_impuesto = "02";
							}
							else
							{
								codigo_impuesto = "22";
							}
						}
						else
						{
							codigo_impuesto = "04";
						}


						// <cac:TaxScheme>
						//Grupo de informaciones específicas sobre el tributo
						TaxSchemeType TaxSchemeConsumo = new TaxSchemeType();
						ListaTipoImpuesto list_tipoimp = new ListaTipoImpuesto();
						ListaItem tipoimp = list_tipoimp.Items.Where(d => d.Codigo.Equals(codigo_impuesto)).FirstOrDefault();
						//Identificador del tributo
						TaxSchemeConsumo.ID = new IDType(); /*** QUEMADO ***/
						TaxSchemeConsumo.ID.Value = tipoimp.Codigo;//"22";//TipoImpuestos.Consumo,
																   //Nombre del tributo
						TaxSchemeConsumo.Name = new NameType1();
						TaxSchemeConsumo.Name.Value = tipoimp.Nombre; //"Bolsas"; /*** QUEMADO ***/

						TaxCategoryConsumo.TaxScheme = TaxSchemeConsumo;
						TaxSubtotalConsumo.TaxCategory = TaxCategoryConsumo;
						TaxesSubtotal[0] = TaxSubtotalConsumo;
						TaxTotal.TaxSubtotal = TaxesSubtotal;
						TaxesTotal.Add(TaxTotal);

					}

					#region Impuesto2 saludable
					if (DocDet.ValorImpuestoConsumo2 > 0 && DocDet.ProductoGratis == false)
					{


						//Grupo de campos para informaciones relacionadas con un tributo aplicable a esta línea de la factura 
						TaxTotalType TaxTotal = new TaxTotalType();

						//if (decimal.Round((DocDet.ValorSubtotal * (DocDet.ImpoConsumoPorcentaje * 100)), 2) != DocDet.ValorImpuestoConsumo)
						//	DocDet.ValorImpuestoConsumo = decimal.Round((DocDet.ValorSubtotal * (DocDet.ImpoConsumoPorcentaje * 100)), 2);

						// importe total de impuestos, por ejemplo, IVA; la suma de los subtotales fiscales para cada categoría de impuestos dentro del esquema impositivo
						// <cbc:TaxAmount>
						TaxTotal.TaxAmount = new TaxAmountType()
						{
							currencyID = moneda_detalle.ToString(),
							Value = DocDet.ValorImpuestoConsumo2// decimal.Round(DocDet.ValorImpuestoConsumo, 2)
						};

						// indicador que este total se reconoce como evidencia legal a efectos impositivos (verdadero)o no(falso).
						// <cbc:TaxEvidenceIndicator>
						TaxTotal.TaxEvidenceIndicator = new TaxEvidenceIndicatorType()
						{
							Value = false
						};

						RoundingAmountType Rouding = new RoundingAmountType();
						Rouding.Value = 0;
						Rouding.currencyID = moneda_detalle.ToString();
						TaxTotal.RoundingAmount = Rouding;

						// Debe ser informado un grupo de estos para cada tarifa. 
						// <cac:TaxSubtotal>
						TaxSubtotalType[] TaxesSubtotal = new TaxSubtotalType[1];

						TaxSubtotalType TaxSubtotalConsumo = new TaxSubtotalType();

						// El monto de este subtotal fiscal.
						//Valor del tributo: producto del porcentaje aplicado sobre la base imponible
						// <cbc:TaxAmount>
						TaxSubtotalConsumo.TaxAmount = new TaxAmountType()
						{
							currencyID = moneda_detalle.ToString(),
							Value = DocDet.ValorImpuestoConsumo2
						};

						// categoría de impuestos aplicable a este subtotal.
						//Grupo de informaciones sobre el tributo 
						// <cac:TaxCategory>
						TaxCategoryType TaxCategoryConsumo = new TaxCategoryType();

						string codigo_impuesto = "04";

						if (DocDet.ImpoConsumo2Porcentaje == 10 || DocDet.ImpoConsumo2Porcentaje == 15)
							codigo_impuesto = "35";

						//
						if (DocDet.ValorImpuestoConsumo2 > 0 && DocDet.ImpoConsumo2Porcentaje == 0)
						{
							//if (DocDet.Aiu != 4)
							//{
							//	codigo_impuesto = "02";
							//}
							//else
							//{
							//	codigo_impuesto = "22";
							//}

							codigo_impuesto = "34";

							// importe neto al que se aplica el porcentaje del impuesto (tasa) para calcular el importe del impuesto.
							//Base Imponible sobre la que se calcula el valor del tributo
							//----Se debe Solicitar la base con la que calculo el impuesto
							TaxSubtotalConsumo.BaseUnitMeasure = new BaseUnitMeasureType()
							{
								unitCode = DebitedQuantity.unitCode,
								Value = DocDet.Cantidad //1.00M//decimal.Round(DocDet.Cantidad, 6)//
							};

							// tasa de impuesto de la categoría de impuestos aplicada a este subtotal fiscal, expresada como un porcentaje.
							//Tarifa del tributo
							// <cbc:Percent>
							TaxSubtotalConsumo.PerUnitAmount = new PerUnitAmountType()
							{
								currencyID = moneda_detalle.ToString(),
								Value = decimal.Round((DocDet.ValorImpuestoConsumo2) / DocDet.Cantidad, 6) + 0.00M
							};

							if (TaxTotal.TaxAmount.Value != ((TaxSubtotalConsumo.PerUnitAmount.Value * TaxSubtotalConsumo.BaseUnitMeasure.Value) / 100))
							{
								TaxTotal.TaxAmount.Value = Decimal.Round((TaxSubtotalConsumo.PerUnitAmount.Value * TaxSubtotalConsumo.BaseUnitMeasure.Value) / 100, 2) + 0.00M;
								TaxSubtotalConsumo.TaxAmount.Value = TaxTotal.TaxAmount.Value;
							}


						}
						else
						{

							// importe neto al que se aplica el porcentaje del impuesto (tasa) para calcular el importe del impuesto.
							//Base Imponible sobre la que se calcula el valor del tributo
							//----Se debe Solicitar la base con la que calculo el impuesto
							TaxSubtotalConsumo.TaxableAmount = new TaxableAmountType()
							{
								currencyID = moneda_detalle.ToString(),
								Value = DocDet.ValorSubtotal
							};

							// tasa de impuesto de la categoría de impuestos aplicada a este subtotal fiscal, expresada como un porcentaje.
							//Tarifa del tributo
							// <cbc:Percent>
							TaxCategoryConsumo.Percent = new PercentType1()
							{
								Value = decimal.Round((DocDet.ImpoConsumo2Porcentaje), 2)
							};
						}

						// <cac:TaxScheme>
						//Grupo de informaciones específicas sobre el tributo
						TaxSchemeType TaxSchemeConsumo = new TaxSchemeType();
						ListaTipoImpuesto list_tipoimp = new ListaTipoImpuesto();
						ListaItem tipoimp = list_tipoimp.Items.Where(d => d.Codigo.Equals(codigo_impuesto)).FirstOrDefault();
						//Identificador del tributo
						TaxSchemeConsumo.ID = new IDType(); /*** QUEMADO ***/
						TaxSchemeConsumo.ID.Value = tipoimp.Codigo;//"04";//TipoImpuestos.Consumo,
																   //Nombre del tributo
						TaxSchemeConsumo.Name = new NameType1();
						TaxSchemeConsumo.Name.Value = tipoimp.Nombre; //"INC"; /*** QUEMADO ***/

						TaxCategoryConsumo.TaxScheme = TaxSchemeConsumo;
						TaxSubtotalConsumo.TaxCategory = TaxCategoryConsumo;
						TaxesSubtotal[0] = TaxSubtotalConsumo;
						TaxTotal.TaxSubtotal = TaxesSubtotal;
						TaxesTotal.Add(TaxTotal);
					}
					else if (DocDet.ValorImpuestoConsumo2 > 0 && DocDet.ProductoGratis == true)
					{
						//Grupo de campos para informaciones relacionadas con un tributo aplicable a esta línea de la factura 
						TaxTotalType TaxTotal = new TaxTotalType();

						// importe total de impuestos, por ejemplo, IVA; la suma de los subtotales fiscales para cada categoría de impuestos dentro del esquema impositivo
						// <cbc:TaxAmount>
						TaxTotal.TaxAmount = new TaxAmountType()
						{
							currencyID = moneda_detalle.ToString(),
							Value = DocDet.ValorImpuestoConsumo2// decimal.Round(DocDet.ValorImpuestoConsumo, 2)
						};

						// indicador que este total se reconoce como evidencia legal a efectos impositivos (verdadero)o no(falso).
						// <cbc:TaxEvidenceIndicator>
						TaxTotal.TaxEvidenceIndicator = new TaxEvidenceIndicatorType()
						{
							Value = false
						};

						RoundingAmountType Rouding = new RoundingAmountType();
						Rouding.Value = 0;
						Rouding.currencyID = moneda_detalle.ToString();
						TaxTotal.RoundingAmount = Rouding;

						// Debe ser informado un grupo de estos para cada tarifa. 
						// <cac:TaxSubtotal>
						TaxSubtotalType[] TaxesSubtotal = new TaxSubtotalType[1];

						TaxSubtotalType TaxSubtotalConsumo = new TaxSubtotalType();

						// importe neto al que se aplica el porcentaje del impuesto (tasa) para calcular el importe del impuesto.
						//Base Imponible sobre la que se calcula el valor del tributo
						//----Se debe Solicitar la base con la que calculo el impuesto
						TaxSubtotalConsumo.BaseUnitMeasure = new BaseUnitMeasureType()
						{
							unitCode = DebitedQuantity.unitCode,
							Value = DocDet.Cantidad //1.00M//decimal.Round(DocDet.Cantidad, 6)
						};

						// El monto de este subtotal fiscal.
						//Valor del tributo: producto del porcentaje aplicado sobre la base imponible
						// <cbc:TaxAmount>
						TaxSubtotalConsumo.TaxAmount = new TaxAmountType()
						{
							currencyID = moneda_detalle.ToString(),
							Value = DocDet.ValorImpuestoConsumo2
						};

						// tasa de impuesto de la categoría de impuestos aplicada a este subtotal fiscal, expresada como un porcentaje.
						//Tarifa del tributo
						// <cbc:Percent>
						TaxSubtotalConsumo.PerUnitAmount = new PerUnitAmountType()
						{
							currencyID = moneda_detalle.ToString(),
							Value = decimal.Round((DocDet.ValorImpuestoConsumo2) / DocDet.Cantidad, 6) + 0.00M//decimal.Round((DocDet.ValorImpuestoConsumo) / TaxSubtotalConsumo.BaseUnitMeasure.Value, 2) + 0.00M//
						};

						// categoría de impuestos aplicable a este subtotal.
						//Grupo de informaciones sobre el tributo 
						// <cac:TaxCategory>
						TaxCategoryType TaxCategoryConsumo = new TaxCategoryType();
						string codigo_impuesto = string.Empty;
						//
						if (DocDet.ValorImpuestoConsumo2 > 0 && DocDet.ImpoConsumo2Porcentaje == 0)
						{
							//if (DocDet.Aiu != 4)
							//{
							//	codigo_impuesto = "02";
							//}
							//else
							//{
							//	codigo_impuesto = "22";
							//}

							codigo_impuesto = "34";

						}
						else
						{
							codigo_impuesto = "35";
						}


						// <cac:TaxScheme>
						//Grupo de informaciones específicas sobre el tributo
						TaxSchemeType TaxSchemeConsumo = new TaxSchemeType();
						ListaTipoImpuesto list_tipoimp = new ListaTipoImpuesto();
						ListaItem tipoimp = list_tipoimp.Items.Where(d => d.Codigo.Equals(codigo_impuesto)).FirstOrDefault();
						//Identificador del tributo
						TaxSchemeConsumo.ID = new IDType(); /*** QUEMADO ***/
						TaxSchemeConsumo.ID.Value = tipoimp.Codigo;//"22";//TipoImpuestos.Consumo,
																   //Nombre del tributo
						TaxSchemeConsumo.Name = new NameType1();
						TaxSchemeConsumo.Name.Value = tipoimp.Nombre; //"Bolsas"; /*** QUEMADO ***/

						TaxCategoryConsumo.TaxScheme = TaxSchemeConsumo;
						TaxSubtotalConsumo.TaxCategory = TaxCategoryConsumo;
						TaxesSubtotal[0] = TaxSubtotalConsumo;
						TaxTotal.TaxSubtotal = TaxesSubtotal;
						TaxesTotal.Add(TaxTotal);

					}
					#endregion

					#region impuesto: Ica -- Hacer cambio 

					/*
					TaxSubtotalType TaxSubtotalIca = new TaxSubtotalType();

					// importe neto al que se aplica el porcentaje del impuesto (tasa) para calcular el importe del impuesto.
					// <cbc:TaxAmount>
					TaxSubtotalIca.TaxableAmount = new TaxableAmountType()
					{
						currencyID = moneda_detalle.ToString(),
						Value = DocDet.ValorSubtotal
					};

					// El monto de este subtotal fiscal.
					// <cbc:TaxAmount>
					TaxSubtotalIca.TaxAmount = new TaxAmountType()
					{
						currencyID = moneda_detalle.ToString(),
						Value = DocDet.ReteIcaValor
					};

					// tasa de impuesto de la categoría de impuestos aplicada a este subtotal fiscal, expresada como un porcentaje.
					// <cbc:Percent>
					TaxSubtotalIca.Percent = new PercentType1()
					{
						Value = decimal.Round((DocDet.ReteIcaPorcentaje), 2)
					};

					// categoría de impuestos aplicable a este subtotal.
					// <cac:TaxCategory>
					TaxCategoryType TaxCategoryIca = new TaxCategoryType();

					// <cac:TaxScheme>
					TaxSchemeType TaxSchemeIca = new TaxSchemeType()
					{
						ID = new IDType()
						{
							Value = TipoImpuestos.Ica,
							schemeName = "VALOR TOTAL DE ICA"
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
					TaxesTotal[0] = TaxTotal;*/

					#endregion

					DebitNoteLine.TaxTotal = TaxesTotal.ToArray();

					#endregion

					#region Datos producto

					ItemType Item = new ItemType();

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
					List<AdditionalInformationType> AdditionalInformation = new List<AdditionalInformationType>();
					AdditionalInformationType Additional = new AdditionalInformationType();
					Additional.Value = DocDet.ProductoDescripcion;
					AdditionalInformation.Add(Additional);
					Item.AdditionalInformation = AdditionalInformation.ToArray();

					#endregion

					#region Id producto definido por la Empresa

					// <cac:CatalogueItemIdentification>
					ItemIdentificationType SellersItemIdentification = new ItemIdentificationType();
					IDType IDItem = new IDType();
					IDItem.Value = DocDet.ProductoCodigo;
					SellersItemIdentification.ID = IDItem;
					Item.SellersItemIdentification = SellersItemIdentification;

					// <cac:StandardItemIdentification>
					ItemIdentificationType StandardItemIdentification = new ItemIdentificationType();
					IDType IDItemStandard = new IDType();
					//---Validar que no venag null
					//IDItemStandard.Value = (string.IsNullOrEmpty(DocDet.ProductoCodigoEAN)) ? string.Empty : DocDet.ProductoCodigoEAN;
					// -- 6.3.5. Productos: @schemeID, @schemeName, @schemeAgencyID

					//Se valida si es Exportacion y que llegue informacion en la posicion arancelaria
					if (!moneda_detalle.ToString().Equals("COP") && !string.IsNullOrEmpty(DocDet.ProductoCodigoPArancelaria))
					{
						ListaTipoCodigoProducto list_TipoPro = new ListaTipoCodigoProducto();
						ListaItem TipoProd = list_TipoPro.Items.Where(d => d.Codigo.Equals("020")).FirstOrDefault();
						IDItemStandard.schemeID = TipoProd.Codigo;
						IDItemStandard.schemeName = TipoProd.Descripcion;
						IDItemStandard.schemeAgencyID = "195";
						IDItemStandard.Value = DocDet.ProductoCodigoPArancelaria;
					}
					else if (!string.IsNullOrEmpty(DocDet.ProductoCodigoEAN))
					{
						ListaTipoCodigoProducto list_TipoPro = new ListaTipoCodigoProducto();
						ListaItem TipoProd = list_TipoPro.Items.Where(d => d.Codigo.Equals("010")).FirstOrDefault();
						IDItemStandard.schemeID = TipoProd.Codigo;
						IDItemStandard.schemeName = TipoProd.Descripcion;
						IDItemStandard.schemeAgencyID = "9";
						IDItemStandard.Value = DocDet.ProductoCodigoEAN;
					}
					else
					{
						IDItemStandard.schemeID = "999";
						IDItemStandard.Value = DocDet.ProductoCodigo;
					}
					StandardItemIdentification.ID = IDItemStandard;
					Item.StandardItemIdentification = StandardItemIdentification;
					#endregion

					#region Bodega del producto

					AddressType[] Address = new AddressType[1];
					AddressType Origen = new AddressType();
					IDType IDItemOrigen = new IDType();
					//---Validar que no venag null
					IDItemOrigen.Value =
						(string.IsNullOrEmpty(DocDet.Bodega)) ? string.Empty : DocDet.Bodega; //DocDet.Bodega;
					Origen.ID = IDItemOrigen;
					Address[0] = Origen;
					Item.OriginAddress = Address;

					#endregion


					#region Campos Adicionales

					if (DocDet.CamposAdicionales != null)
					{
						CampoValor marca = DocDet.CamposAdicionales.Where(d => d.Descripcion.Equals("MARCA")).FirstOrDefault();

						if (marca != null)
						{
							BrandNameType[] BrandName = new BrandNameType[1];
							BrandNameType Brand = new BrandNameType();
							Brand.Value = marca.Valor;
							BrandName[0] = Brand;
							Item.BrandName = BrandName;
						}

						CampoValor modelo = DocDet.CamposAdicionales.Where(d => d.Descripcion.Equals("MODELO")).FirstOrDefault();

						if (modelo != null)
						{
							ModelNameType[] ModelName = new ModelNameType[1];
							ModelNameType Model = new ModelNameType();
							Model.Value = modelo.Valor;
							ModelName[0] = Model;
							Item.ModelName = ModelName;
						}

						List<ItemPropertyType> Property = new List<ItemPropertyType>();
						if (DocDet.OcultarItem == 1)
						{
							ItemPropertyType Oculto = new ItemPropertyType();
							Oculto.Name = new NameType1();
							Oculto.Name.Value = "Item Oculto para Impresion"; /*** QUEMADO ***/
							Oculto.Value = new ValueType();
							Oculto.Value.Value = DocDet.OcultarItem.ToString();
							Property.Add(Oculto);
						}

						foreach (CampoValor Campos in DocDet.CamposAdicionales)
						{
							ItemPropertyType campo = new ItemPropertyType();
							campo.Name = new NameType1();
							campo.Name.Value = Campos.Descripcion;
							campo.Value = new ValueType();
							campo.Value.Value = Campos.Valor;
							Property.Add(campo);
						}
						Item.AdditionalItemProperty = Property.ToArray();
					}

					#endregion


					if (DocDet.DatosMandatario != null)
					{

						PartyType Mandantario = new PartyType();
						Mandantario.PowerOfAttorney = new PowerOfAttorneyType[1];
						PowerOfAttorneyType power = new PowerOfAttorneyType();
						power.AgentParty = new PartyType();
						power.AgentParty.PartyIdentification = new PartyIdentificationType[1];
						power.AgentParty.PartyIdentification[0] = new PartyIdentificationType();
						power.AgentParty.PartyIdentification[0].ID = new IDType();
						power.AgentParty.PartyIdentification[0].ID.Value = DocDet.DatosMandatario.Identificacion;
						power.AgentParty.PartyIdentification[0].ID.schemeAgencyID = "195";
						power.AgentParty.PartyIdentification[0].ID.schemeID = DocDet.DatosMandatario.IdentificacionDv.ToString();
						power.AgentParty.PartyIdentification[0].ID.schemeName = DocDet.DatosMandatario.TipoIdentificacion.ToString();
						Mandantario.PowerOfAttorney[0] = power;

						Item.InformationContentProviderParty = Mandantario;
					}

					DebitNoteLine.Item = Item;

					#endregion

					#region Valor Unitario producto

					PriceType Price = new PriceType();
					PriceAmountType PriceAmount = new PriceAmountType();
					PriceAmount.currencyID = moneda_detalle.ToString(); //(LISTADO DE VALORES DEFINIDO POR LA DIAN)
					PriceAmount.Value = DocDet.ValorUnitario; //decimal.Round(DocDet.ValorUnitario, 2);
					Price.PriceAmount = PriceAmount;

					//---Segun la base de la unidad utilizada
					BaseQuantityType BaseQuantity = new BaseQuantityType();
					BaseQuantity.unitCode = DebitedQuantity.unitCode;
					BaseQuantity.Value = DocDet.Cantidad;
					Price.BaseQuantity = BaseQuantity;
					DebitNoteLine.Price = Price;

					#endregion

					DebitNoteLineType[contadorPosicion] = DebitNoteLine;
					contadorProducto++;
					contadorPosicion++;
				}

				nota.DebitNoteLine = DebitNoteLineType;
				return nota.DebitNoteLine;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}
	}
}
