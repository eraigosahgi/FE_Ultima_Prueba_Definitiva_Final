using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetUBLv2_1;
using HGInetUBLv2_1.DianListas;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using LibreriaGlobalHGInet.HgiNet.Controladores;
using HGInetUBLv2_1.XML;

namespace HGInetUBLv2_1
{
	public class NotaCreditoXMLv2_1
	{

		/// <summary>
		/// Crea el XML con la informacion de la nota credito en formato UBL
		/// </summary>
		/// <param name="id_documento">Id de seguridad del documento generado por la plataforma</param>
		/// <param name="documento">Objeto de tipo NotaCredito que contiene la informacion de la nota credito</param>
		/// <param name="resolucion">Resolución relacionada con el documento</param>
		/// <param name="empresa">Objeto de tipo TblEmpresas que contiene la informacion de la empresa que expide</param>
		/// <param name="ruta_almacenamiento">Ruta donde se va a guardar el archivo</param>
		/// <param name="reemplazar_archivo">Indica si sobreescribe el archivo existente</param>
		/// <param name="tipo">Indica el tipo de documento</param>
		/// <returns>Ruta donde se guardo el archivo XML</returns>  
		public static FacturaE_Documento CrearDocumento(Guid id_documento, NotaCredito documento, HGInetMiFacturaElectonicaData.ModeloServicio.ExtensionDian resolucion, TipoDocumento tipo, string ambiente_dian)
		{
			try
			{
				if (documento == null)
					throw new Exception("La documento es inválido.");

				//Obtiene el nombre del archivo XML
				//string nombre_archivo_xml = NombramientoArchivo.ObtenerXml(documento.Documento.ToString(), documento.DatosObligado.Identificacion, tipo, documento.Prefijo);


				CreditNoteType nota_credito = new CreditNoteType();
				XmlSerializerNamespaces namespaces_xml = NamespacesXML.ObtenerNamespaces();

				#region nota_credito.UBLVersionID //Versión de los esquemas UBL

				nota_credito.UBLVersionID = new UBLVersionIDType()
				{
					Value = "UBL 2.1"//Recursos.VersionesDIAN.UBLVersionID
				};
				#endregion

				#region nota_credito.CustomizationID //

				nota_credito.CustomizationID = new CustomizationIDType()
				{
					Value = "05"//documento.Prefijo
				};
				#endregion

				#region nota_credito.ProfileID //Versión del documento DIAN_UBL.xsd publicado por la DIAN
				nota_credito.ProfileID = new ProfileIDType()
				{
					Value = "DIAN 2.1"//Recursos.VersionesDIAN.ProfileID
				};

				//---Ambiente de Pruebas
				nota_credito.ProfileExecutionID = new ProfileExecutionIDType()
				{
					Value = ambiente_dian//"2"
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
				IssueTime.Value = documento.Fecha.AddHours(5).ToString(Fecha.formato_hora_zona);//Convert.ToDateTime(documento.Fecha.ToString(Fecha.formato_hora_completa)).AddHours(5);//Convert.ToDateTime(documento.Fecha.ToString(Fecha.formato_hora_completa));
				nota_credito.IssueTime = IssueTime;
				#endregion

				#region --- Esta seccion se debe validar, como aplica en Nota Credito forma de pago.
				/*
				//Valida si este dato no llega y pone por defecto 1 - Contado
				if (documento.FormaPago == 0)
					documento.FormaPago = 1;

				//Valida que el termino de pago no este en 0 y lo pone por defecto en 10 - Efectivo
				if (documento.TerminoPago == 0)
					documento.TerminoPago = 10;*/

				//List<PaymentMeansType> PaymentMeans = new List<PaymentMeansType>();
				//PaymentMeansType PaymentMean = new PaymentMeansType();
				//PaymentMeansCodeType MeansCode = new PaymentMeansCodeType();
				//MeansCode.Value = "10";//documento.TerminoPago.ToString();
				////MeansCode.name = Ctl_Enumeracion.ObtenerMedioPago(documento.FormaPago);
				//PaymentMean.ID = new IDType();
				//PaymentMean.ID.Value = "1"; //documento.FormaPago.ToString();
				////---Si es pago a credito debe indicarse el identificador del pago
				//PaymentMean.PaymentID = new PaymentIDType[1];
				//PaymentIDType Paymentid = new PaymentIDType();
				//Paymentid.Value = documento.DocumentoRef.ToString();
				//PaymentMean.PaymentID[0] = Paymentid;

				////PaymentMean.PaymentMeansCode = MeansCode;
				//PaymentMeans.Add(PaymentMean);
				//nota_credito.PaymentMeans = PaymentMeans.ToArray();
				#endregion

				//---Identificador del documento
				CreditNoteTypeCodeType CreditNoteTypeCode = new CreditNoteTypeCodeType();
				CreditNoteTypeCode.Value = "91";
				nota_credito.CreditNoteTypeCode = CreditNoteTypeCode;

				//Lineas del Detalle
				nota_credito.LineCountNumeric = new LineCountNumericType();
				nota_credito.LineCountNumeric.Value = documento.DocumentoDetalles.Count;

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
				DescriptionType[] DescriptionType = new DescriptionType[1];
				DescriptionType Description = new DescriptionType();
				//Obtengo la descripcion segun el codig9o del concepto
				ListaConceptoNotaCredito list_Concepto = new ListaConceptoNotaCredito();
				ListaItem desc_concepto = list_Concepto.Items.Where(d => d.Codigo.Equals(documento.Concepto)).FirstOrDefault();
				Description.Value = desc_concepto.Descripcion;
				DescriptionType[0] = Description;

				Reference.Description = DescriptionType;
				DiscrepancyResponse[0] = Reference;

				nota_credito.DiscrepancyResponse = DiscrepancyResponse;
				#endregion

				#region nota_credito.BillingReference //Referencia Documento (factura)

				//Referencia a un documento afectar
				/*
				DocumentReferenceType[] DocumentReferenceType = new DocumentReferenceType[1];
				DocumentReferenceType DocumentReference = new DocumentReferenceType();
				DocumentReference.ID = new IDType();
				DocumentReference.ID.Value = documento.DocumentoRef.ToString();
				DocumentReference.UUID = new UUIDType();
				DocumentReference.UUID.Value = documento.CufeFactura;
				DocumentReference.UUID.schemeName = "CUDE-SHA384";
				DocumentReferenceType[0] = DocumentReference;
				nota_credito.AdditionalDocumentReference = DocumentReferenceType;*/

				
				nota_credito.BillingReference = new BillingReferenceType[1];

				BillingReferenceType DocReference = new BillingReferenceType();
				DocumentReferenceType DocumentReference = new DocumentReferenceType();
				DocumentReference.ID = new IDType();
				DocumentReference.ID.Value = documento.DocumentoRef.ToString();
				DocumentReference.UUID = new UUIDType();
				DocumentReference.UUID.Value = documento.CufeFactura;
				DocumentReference.UUID.schemeName = "CUDE-SHA384";
				DocumentReference.IssueDate = new IssueDateType();
				DocumentReference.IssueDate.Value = documento.FechaFactura;
				DocReference.InvoiceDocumentReference = DocumentReference;

				nota_credito.BillingReference[0] = DocReference;

				#endregion


				#region nota_credito.OrderReference //Referencia Documento (orden)

				//Referencia un documento de pedido

				OrderReferenceType DocOrderReference = new OrderReferenceType();
				DocOrderReference.ID = new IDType() { Value = documento.PedidoRef.ToString() };
				nota_credito.OrderReference = DocOrderReference;

				#endregion

				#region nota_credito.AccountingSupplierParty // Información del obligado a facturar
				nota_credito.AccountingSupplierParty = TerceroXML.ObtenerObligado(documento.DatosObligado,documento.Prefijo);
				#endregion

				#region nota_credito.AccountingCustomerParty //Información del Adquiriente
				/* Persona natural o jurídica que adquiere bienes y/o servicios y debe exigir factura
				51 o documento equivalente y, que tratándose de la factura electrónica, 
			    la recibe, rechaza, cuando
				52 sea del caso, y conserva para su posterior exhibición*/

				nota_credito.AccountingCustomerParty = TerceroXML.ObtenerAquiriente(documento.DatosAdquiriente);
				#endregion

				if (documento.Descuentos != null || documento.Cargos != null)
					nota_credito.AllowanceCharge = ValoresAdicionalesXML.ObtenerValoresAd(documento);

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

				nota_credito.TaxTotal = ImpuestosXML.ObtenerImpuestos(documento.DocumentoDetalles.ToList(), documento.Moneda);
				#endregion

				#region nota_credito.LegalMonetaryTotal //Datos Importes Totales
				/*Agrupación de campos
				relativos a los importes totales aplicables a la
				nota_credito. Estos importes son calculados teniendo
				en cuenta las líneas de nota_credito y elementos a
				nivel de nota_credito, como descuentos, cargos,
				impuestos, etc*/
				nota_credito.LegalMonetaryTotal = TotalesXML.ObtenerTotales(documento);
				#endregion

				
				#region nota_credito.UUID //CUDE:Codigo Unico de Documento Electronica.
				/*
				  CUDE: Campo que verifica la integridad de la información
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
				string CUFE = CalcularCUFE(nota_credito, resolucion.PinSoftware, documento.CufeFactura, nota_credito.ProfileExecutionID.Value);
				UUID.Value = CUFE;
				UUID.schemeName = "CUDE-SHA384";
				UUID.schemeID = nota_credito.ProfileExecutionID.Value; //"2";
				nota_credito.UUID = UUID;
				#endregion

				#region nota_credito.UBLExtensions

				XmlDocument doc = new XmlDocument();
				doc.LoadXml("<firma></firma>");

				List<UBLExtensionType> UBLExtensions = new List<UBLExtensionType>();

				//Informacion del QR
				string ruta_qr_Dian = "https://muisca.dian.gov.co/WebFacturaelectronica/paginas/VerificarFacturaElectronicaExterno.faces?";
				string tipo_doc = nota_credito.CreditNoteTypeCode.Value;
				string num_doc = nota_credito.ID.Value;
				string nit_fac = documento.DatosObligado.Identificacion;
				string nit_adq = documento.DatosAdquiriente.Identificacion;
				string cadena_qr = string.Format("{0}TipoDocumento={1}NroDocumento={2}NITFacturador={3}NumIdentAdquiriente={3}Cufe={4}", ruta_qr_Dian, tipo_doc, num_doc, nit_fac, nit_adq, CUFE);


				// Extension de la Dian
				UBLExtensionType UBLExtensionDian = new UBLExtensionType();
				UBLExtensionDian.ExtensionContent = HGInetUBLv2_1.ExtensionDian.Obtener(resolucion, TipoDocumento.NotaCredito, nota_credito.ID.Value, cadena_qr);
				UBLExtensions.Add(UBLExtensionDian);

				// Extension de la firma
				UBLExtensionType UBLExtensionFirma = new UBLExtensionType();
				UBLExtensionFirma.ExtensionContent = doc.DocumentElement;
				UBLExtensions.Add(UBLExtensionFirma);

				nota_credito.UBLExtensions = UBLExtensions.ToArray();

				#endregion

				#region nota_credito.CreditNoteLine  //Línea de nota_credito
				//Elemento que agrupa todos los campos de una línea de nota_credito
				nota_credito.CreditNoteLine = ObtenerDetalleDocumento(documento.DocumentoDetalles.ToList(), documento.CufeFactura, documento.Moneda);

				#endregion

				// convierte los datos del objeto en texto XML 
				StringBuilder txt_xml = ConvertirXML.Convertir(nota_credito, namespaces_xml, TipoDocumento.NotaCredito);

				FacturaE_Documento xml_sin_firma = new FacturaE_Documento();
				xml_sin_firma.Documento = documento;
				xml_sin_firma.NombreXml = string.Format("{0}{1}", "PruebaNC-", numero_documento); //nombre_archivo_xml;
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
		/// <param name="ambiente">Número de identificación del ambiente utilizado por el Obligado para emitir la factura Seccion 6.1.1 (1=AmbienteProduccion , 2: AmbientePruebas)</param>
		/// <returns></returns>
		public static string CalcularCUFE(CreditNoteType nota_credito, string pin_software, string cufe_factura, string ambiente)
		{
			try
			{
				if (nota_credito == null)
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
				//DateTime fecha = nota_credito.IssueDate.Value;
				//DateTime fecha_hora = Convert.ToDateTime(nota_credito.IssueTime.Value);
				//TimeSpan hora = new TimeSpan(fecha_hora.Hour, fecha_hora.Minute, fecha_hora.Second);
				//fecha = fecha.Date + hora;

				string NumCr = nota_credito.ID.Value;
				string FecCr = string.Format("{0}{1}", nota_credito.IssueDate.Value.ToString("yyyy-MM-dd"), nota_credito.IssueTime.Value); //fecha.ToString(Fecha.formato_fecha_java);
				string ValCr = nota_credito.LegalMonetaryTotal.LineExtensionAmount.Value.ToString();

				//Impuesto 1
				string CodImp1 = "01";
				decimal ValImp1 = 0.00M;

				//Impuesto 2
				string CodImp2 = "04";
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
						else if (codigo_impuesto.Equals("04"))
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

				for (int contador_obligado_facturar = 0; contador_obligado_facturar < nota_credito.AccountingSupplierParty.Party.PartyTaxScheme.Count(); contador_obligado_facturar++)
				{
					NitOFE = nota_credito.AccountingSupplierParty.Party.PartyTaxScheme[contador_obligado_facturar].CompanyID.Value;
				}

				//string TipAdq = string.Empty;
				string NumAdq = string.Empty;

				for (int contador_adquiriente = 0; contador_adquiriente < nota_credito.AccountingCustomerParty.Party.PartyTaxScheme.Count(); contador_adquiriente++)
				{
					//TipAdq = nota_credito.AccountingCustomerParty.Party.PartyTaxScheme[contador_adquiriente].CompanyID.schemeName;
					NumAdq = nota_credito.AccountingCustomerParty.Party.PartyTaxScheme[contador_adquiriente].CompanyID.Value;
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
					+ NumAdq
					+ pin_software
					+ ambiente
				;

				//string cufe_encriptado = Encriptar.Encriptar_SHA384(cufe);
				string cufe_encriptado = Ctl_CalculoCufe.CufeNotaCreditoV2(pin_software,String.Empty, NumCr,FecCr,NitOFE,ambiente,NumAdq,Convert.ToDecimal(ValImp), Convert.ToDecimal(ValCr),ValImp1,ValImp2,ValImp3,false);
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

					//Crear Enumerable para que lea segun la moneda
					CurrencyCodeContentType moneda_detalle = Ctl_Enumeracion.ObtenerMoneda(moneda);

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

					// Unidad de medida Ver lista de valores posibles en 6.3.6(Defecto codigo - 94)
					ListaUnidadesMedida list_unidad = new ListaUnidadesMedida();
					ListaItem unidad = list_unidad.Items.Where(d => d.Codigo.Equals(DocDet.UnidadCodigo)).FirstOrDefault();

					// Unidad de medida
					CreditedQuantity.unitCode = unidad.Codigo;
					CreditNoteLine.CreditedQuantity = CreditedQuantity;
					#endregion

					#region Valor Total
					LineExtensionAmountType LineExtensionAmount = new LineExtensionAmountType();
					LineExtensionAmount.currencyID = moneda_detalle.ToString(); //(LISTADO DE VALORES DEFINIDO POR LA DIAN)
					LineExtensionAmount.Value = decimal.Round(DocDet.ValorSubtotal, 2);
					CreditNoteLine.LineExtensionAmount = LineExtensionAmount;
					#endregion

					#region Impuestos del producto -- Hacer cambio para que llene la informacion segun los impuestos que tenga las lineas

					// <cac:TaxTotal>
					List<TaxTotalType> TaxesTotal = new List<TaxTotalType>();

					if (DocDet.IvaValor > 0)
					{
						//Grupo de campos para informaciones relacionadas con un tributo aplicable a esta línea de la factura 
						TaxTotalType TaxTotal = new TaxTotalType();

						// importe total de impuestos, por ejemplo, IVA; la suma de los subtotales fiscales para cada categoría de impuestos dentro del esquema impositivo
						// <cbc:TaxAmount>
						TaxTotal.TaxAmount = new TaxAmountType()
						{
							currencyID = moneda_detalle.ToString(),
							Value = decimal.Round(DocDet.IvaValor, 2)
						};

						// indicador que este total se reconoce como evidencia legal a efectos impositivos (verdadero)o no(falso).
						// <cbc:TaxEvidenceIndicator>
						TaxTotal.TaxEvidenceIndicator = new TaxEvidenceIndicatorType()
						{
							Value = false
						};

						// Debe ser informado un grupo de estos para cada tarifa. 
						// <cac:TaxSubtotal>
						TaxSubtotalType[] TaxesSubtotal = new TaxSubtotalType[1];


						#region impuesto: IVA 

						TaxSubtotalType TaxSubtotalIva = new TaxSubtotalType();

						// importe neto al que se aplica el porcentaje del impuesto (tasa) para calcular el importe del impuesto.
						//Base Imponible sobre la que se calcula el valor del tributo
						//----Se debe Solicitar la base con la que calculo el impuesto
						// <cbc:TaxAmount>
						TaxSubtotalIva.TaxableAmount = new TaxableAmountType()
						{
							currencyID = moneda_detalle.ToString(),
							Value = DocDet.ValorSubtotal
						};

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
						TaxCategoryIva.Percent = new PercentType1()
						{
							Value = decimal.Round((DocDet.IvaPorcentaje), 2)
						};

						// <cac:TaxScheme>
						//Grupo de informaciones específicas sobre el tributo
						TaxSchemeType TaxSchemeIva = new TaxSchemeType();
						//Identificador del tributo
						TaxSchemeIva.ID = new IDType();
						TaxSchemeIva.ID.Value = "01";//TipoImpuestos.Iva,
													 //Nombre del tributo
						TaxSchemeIva.Name = new NameType1();
						TaxSchemeIva.Name.Value = "IVA";

						TaxCategoryIva.TaxScheme = TaxSchemeIva;
						TaxSubtotalIva.TaxCategory = TaxCategoryIva;
						TaxesSubtotal[0] = TaxSubtotalIva;
						TaxTotal.TaxSubtotal = TaxesSubtotal;
						TaxesTotal.Add(TaxTotal);

						#endregion
					}

					if (DocDet.ValorImpuestoConsumo > 0)
					{
						#region impuesto: Consumo

						//Grupo de campos para informaciones relacionadas con un tributo aplicable a esta línea de la factura 
						TaxTotalType TaxTotal = new TaxTotalType();

						// importe total de impuestos, por ejemplo, IVA; la suma de los subtotales fiscales para cada categoría de impuestos dentro del esquema impositivo
						// <cbc:TaxAmount>
						TaxTotal.TaxAmount = new TaxAmountType()
						{
							currencyID = moneda_detalle.ToString(),
							Value = decimal.Round(DocDet.ValorImpuestoConsumo, 2)
						};

						// indicador que este total se reconoce como evidencia legal a efectos impositivos (verdadero)o no(falso).
						// <cbc:TaxEvidenceIndicator>
						TaxTotal.TaxEvidenceIndicator = new TaxEvidenceIndicatorType()
						{
							Value = false
						};

						// Debe ser informado un grupo de estos para cada tarifa. 
						// <cac:TaxSubtotal>
						TaxSubtotalType[] TaxesSubtotal = new TaxSubtotalType[1];

						TaxSubtotalType TaxSubtotalConsumo = new TaxSubtotalType();

						// importe neto al que se aplica el porcentaje del impuesto (tasa) para calcular el importe del impuesto.
						//Base Imponible sobre la que se calcula el valor del tributo
						//----Se debe Solicitar la base con la que calculo el impuesto
						TaxSubtotalConsumo.TaxableAmount = new TaxableAmountType()
						{
							currencyID = moneda_detalle.ToString(),
							Value = DocDet.ValorSubtotal
						};

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

						// tasa de impuesto de la categoría de impuestos aplicada a este subtotal fiscal, expresada como un porcentaje.
						//Tarifa del tributo
						// <cbc:Percent>
						TaxCategoryConsumo.Percent = new PercentType1()
						{
							Value = decimal.Round((DocDet.ImpoConsumoPorcentaje), 2)
						};

						// <cac:TaxScheme>
						//Grupo de informaciones específicas sobre el tributo
						TaxSchemeType TaxSchemeConsumo = new TaxSchemeType();
						//Identificador del tributo
						TaxSchemeConsumo.ID = new IDType();
						TaxSchemeConsumo.ID.Value = "02";//TipoImpuestos.Consumo,
														 //Nombre del tributo
						TaxSchemeConsumo.Name = new NameType1();
						TaxSchemeConsumo.Name.Value = "IC";

						TaxCategoryConsumo.TaxScheme = TaxSchemeConsumo;
						TaxSubtotalConsumo.TaxCategory = TaxCategoryConsumo;
						TaxesSubtotal[0] = TaxSubtotalConsumo;
						TaxTotal.TaxSubtotal = TaxesSubtotal;
						TaxesTotal.Add(TaxTotal);

						#endregion
					}

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
					CreditNoteLine.TaxTotal = TaxesTotal.ToArray();

					#endregion

					#region Datos producto
					// <fe:Item>
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
					IDItemStandard.Value = (string.IsNullOrEmpty(DocDet.ProductoCodigoEAN)) ? string.Empty : DocDet.ProductoCodigoEAN;
					StandardItemIdentification.ID = IDItemStandard;
					Item.StandardItemIdentification = StandardItemIdentification;
					#endregion

					#region Bodega del producto
					AddressType[] Address = new AddressType[1];
					AddressType Origen = new AddressType();
					IDType IDItemOrigen = new IDType();
					//---Validar que no venag null
					IDItemOrigen.Value = (string.IsNullOrEmpty(DocDet.Bodega)) ? string.Empty : DocDet.Bodega;//DocDet.Bodega;
					Origen.ID = IDItemOrigen;
					Address[0] = Origen;
					Item.OriginAddress = Address;
					#endregion


					ItemPropertyType[] Property = new ItemPropertyType[1];
					ItemPropertyType Oculto = new ItemPropertyType();
					Oculto.Name = new NameType1();
					Oculto.Name.Value = "Item Oculto para Impresion";
					Oculto.Value = new ValueType();
					Oculto.Value.Value = DocDet.OcultarItem.ToString();
					Property[0] = Oculto;
					Item.AdditionalItemProperty = Property;


					CreditNoteLine.Item = Item;
					#endregion

					#region Valor Unitario producto
					// <fe:Price>
					PriceType Price = new PriceType();

					// <cbc:PriceAmount>
					PriceAmountType PriceAmount = new PriceAmountType();
					PriceAmount.currencyID = moneda_detalle.ToString();
					PriceAmount.Value = decimal.Round(DocDet.ValorUnitario, 2);
					Price.PriceAmount = PriceAmount;

					//---Segun la base de la unidad utilizada
					BaseQuantityType BaseQuantity = new BaseQuantityType();
					BaseQuantity.unitCode = CreditedQuantity.unitCode;
					BaseQuantity.Value = DocDet.Cantidad;
					Price.BaseQuantity = BaseQuantity;
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
