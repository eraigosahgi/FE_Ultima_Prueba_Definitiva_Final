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
		public static FacturaE_Documento CrearDocumento(Guid id_documento, NotaDebito documento, HGInetMiFacturaElectonicaData.ModeloServicio.ExtensionDian resolucion, TipoDocumento tipo)
		{
			try
			{
				if (documento == null)
					throw new Exception("La documento es inválido.");

				//Obtiene el nombre del archivo XML
				//string nombre_archivo_xml = NombramientoArchivo.ObtenerXml(documento.Documento.ToString(), documento.DatosObligado.Identificacion, tipo, documento.Prefijo);

				DebitNoteType nota_debito = new DebitNoteType();
				XmlSerializerNamespaces namespaces_xml = NamespacesXML.ObtenerNamespaces();

				#region nota_debito.UBLVersionID //Versión de los esquemas UBL

				nota_debito.UBLVersionID = new UBLVersionIDType()
				{
					Value = "UBL 2.1" //Recursos.VersionesDIAN.UBLVersionID
				};

				#endregion

				#region nota_debito.CustomizationID //

				nota_debito.CustomizationID = new CustomizationIDType()
				{
					Value = "05"//documento.Prefijo
				};

				#endregion

				#region nota_debito.ProfileID //Versión del documento DIAN_UBL.xsd publicado por la DIAN

				nota_debito.ProfileID = new ProfileIDType()
				{
					Value = "DIAN 2.1" //Recursos.VersionesDIAN.ProfileID
				};

				#endregion

				//---Ambiente de Pruebas
				nota_debito.ProfileExecutionID = new ProfileExecutionIDType()
				{
					Value = "2"
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
				DateTime fecha_univ = DateTime.UtcNow;
				IssueTime.Value = documento.Fecha.ToString("HH:mm:ss");//Convert.ToDateTime(documento.Fecha.ToString(Fecha.formato_hora_completa)).AddHours(5);//Convert.ToDateTime(documento.Fecha.ToString(Fecha.formato_hora_completa));
				nota_debito.IssueTime = IssueTime;

				#endregion

				#region nota_debito.UBLExtensions

				XmlDocument doc = new XmlDocument();
				doc.LoadXml("<firma></firma>");

				List<UBLExtensionType> UBLExtensions = new List<UBLExtensionType>();

				/*
				//Resolucion de ejemplos de la DIAN
				HGInetMiFacturaElectonicaData.ModeloServicio.ExtensionDian resolucion =
					new HGInetMiFacturaElectonicaData.ModeloServicio.ExtensionDian();
				resolucion.ClaveTecnicaDIAN = "No necesita";
				resolucion.FechaResIni = new DateTime(2019, 01, 19);
				resolucion.FechaResFin = new DateTime(2030, 01, 19);
				resolucion.IdSoftware = "def923e2-8326-42e2-a022-d0fa4a2f8188";//"22fa3f6f-c40d-434b-a3db-08a5d435a372";
				resolucion.NitProveedor = "811021438";
				resolucion.PinSoftware = "05097";
				resolucion.NumResolucion = "18760000001";
				resolucion.Prefijo = "ND";
				resolucion.RangoIni = 1;
				resolucion.RangoFin = 2000;
				resolucion.TipoDocumento = 2;
				*/

				// Extension de la Dian
				UBLExtensionType UBLExtensionDian = new UBLExtensionType();
				UBLExtensionDian.ExtensionContent = ExtensionDian.Obtener(resolucion, TipoDocumento.NotaDebito, nota_debito.ID.Value);
				UBLExtensions.Add(UBLExtensionDian);

				//Extension de la firma
				UBLExtensionType UBLExtensionFirma = new UBLExtensionType();
				UBLExtensionFirma.ExtensionContent = doc.DocumentElement;
				UBLExtensions.Add(UBLExtensionFirma);

				// Extension de HGI
				/*
				UBLExtensionType UBLExtensionHgi = new UBLExtensionType();
				UBLExtensionHgi.ExtensionContent = ExtensionHgiSas.Obtener(id_documento, documento);
				UBLExtensions.Add(UBLExtensionHgi);*/

				nota_debito.UBLExtensions = UBLExtensions.ToArray();

				#endregion

				#region nota_debito.Note //Información adicional

				//Texto libre, relativo al documento
				nota_debito.Note = new NoteType[1]
				{
					new NoteType()
					{
						Value = documento.Nota
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
				Reference.ReferenceID = new ReferenceIDType();
				Reference.ReferenceID.Value = documento.DocumentoRef.ToString();
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

				
				nota_debito.BillingReference = new BillingReferenceType[1];

				BillingReferenceType DocReference = new BillingReferenceType();
				DocumentReferenceType DocumentReference = new DocumentReferenceType();
				DocumentReference.ID = new IDType();
				DocumentReference.ID.Value = documento.DocumentoRef.ToString();
				DocumentReference.UUID = new UUIDType();
				DocumentReference.UUID.Value = documento.CufeFactura;
				DocumentReference.IssueDate = new IssueDateType();
				DocumentReference.IssueDate.Value = documento.FechaFactura;
				DocReference.InvoiceDocumentReference = DocumentReference;

				nota_debito.BillingReference[0] = DocReference;

				#endregion


				#region nota_debito.OrderReference //Referencia Documento (orden)

				//Referencia un documento de pedido

				OrderReferenceType DocOrderReference = new OrderReferenceType();
				DocOrderReference.ID = new IDType() { Value = documento.PedidoRef.ToString() };
				nota_debito.OrderReference = DocOrderReference;

				#endregion

				#region nota_debito.AccountingSupplierParty // Información del obligado a facturar

				nota_debito.AccountingSupplierParty = TerceroXML.ObtenerObligado(documento.DatosObligado);

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

				nota_debito.TaxTotal = ImpuestosXML.ObtenerImpuestos(documento.DocumentoDetalles.ToList(), documento.Moneda);

				#endregion

				#region nota_debito.LegalMonetaryTotal //Datos Importes Totales

				/*Agrupación de campos
				relativos a los importes totales aplicables a la
				nota_debito. Estos importes son calculados teniendo
				en cuenta las líneas de nota_debito y elementos a
				nivel de nota_debito, como descuentos, cargos,
				impuestos, etc*/
				nota_debito.RequestedMonetaryTotal = TotalesXML.ObtenerTotales(documento);
				#endregion

				#region nota_debito.CreditNoteLine  //Línea de nota_debito

				//Elemento que agrupa todos los campos de una línea de nota_debito
				nota_debito.DebitNoteLine = ObtenerDetalleDocumento(documento.DocumentoDetalles.ToList(),documento.CufeFactura, documento.Moneda);

				#endregion

				nota_debito.LineCountNumeric = new LineCountNumericType();
				nota_debito.LineCountNumeric.Value = documento.DocumentoDetalles.Count;

				//La nota credito no requiere CUFE

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

				if (string.IsNullOrEmpty(resolucion.ClaveTecnicaDIAN))
					throw new Exception("La clave técnica en la resolución de la DIAN es inválida para el documento.");

				UUIDType UUID = new UUIDType();
				//-----Se agrega Ambiente al cual se va enviar el documento
				string CUFE = CalcularCUFE(nota_debito, resolucion.PinSoftware, documento.CufeFactura,nota_debito.ProfileExecutionID.Value);
				UUID.Value = CUFE;
				UUID.schemeName = "CUDE-SHA384";
				UUID.schemeID = "2";
				nota_debito.UUID = UUID;

				#endregion

			

				// convierte los datos del objeto en texto XML 
				StringBuilder txt_xml = ConvertirXML.Convertir(nota_debito, namespaces_xml, TipoDocumento.NotaDebito);

				FacturaE_Documento xml_sin_firma = new FacturaE_Documento();
				xml_sin_firma.Documento = documento;
				xml_sin_firma.NombreXml = string.Format("{0}{1}", "PruebaND-", numero_documento); //nombre_archivo_xml;
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
		public static string CalcularCUFE(DebitNoteType nota_debito, string pin_software, string cufe_factura,string ambiente)
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

				string cufe_encriptado = Encriptar.Encriptar_SHA384(cufe);
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
		public static DebitNoteLineType[] ObtenerDetalleDocumento(List<DocumentoDetalle> documentoDetalle,
			string cufefactura, string moneda)
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

					if (string.IsNullOrEmpty(DocDet.UnidadCodigo))
						DocDet.UnidadCodigo = "EA";

					decimal valorTotal = DocDet.Cantidad * DocDet.ValorUnitario;
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
					DebitedQuantity.Value = decimal.Round(DocDet.Cantidad, 2);
					DebitNoteLine.DebitedQuantity = DebitedQuantity;

					// Unidad de medida
					DebitedQuantity.unitCode = Ctl_Enumeracion.ObtenerUnidadMedida(DocDet.UnidadCodigo).ToString();
					DebitNoteLine.DebitedQuantity = DebitedQuantity;

					#endregion

					#region Valor Total

					LineExtensionAmountType LineExtensionAmount = new LineExtensionAmountType();
					LineExtensionAmount.currencyID =
						moneda_detalle.ToString(); //(LISTADO DE VALORES DEFINIDO POR LA DIAN)
					LineExtensionAmount.Value = decimal.Round(valorTotal, 2);
					DebitNoteLine.LineExtensionAmount = LineExtensionAmount;

					#endregion

					#region Impuestos del producto

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
						TaxSchemeIva.ID.Value = "01"; //TipoImpuestos.Iva,
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
						TaxSchemeConsumo.ID.Value = "02"; //TipoImpuestos.Consumo,
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
					IDItemStandard.Value = (string.IsNullOrEmpty(DocDet.ProductoCodigoEAN))
						? string.Empty
						: DocDet.ProductoCodigoEAN;
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


					ItemPropertyType[] Property = new ItemPropertyType[1];
					ItemPropertyType Oculto = new ItemPropertyType();
					Oculto.Name = new NameType1();
					Oculto.Name.Value = "Item Oculto para Impresion";
					Oculto.Value = new ValueType();
					Oculto.Value.Value = DocDet.OcultarItem.ToString();
					Property[0] = Oculto;
					Item.AdditionalItemProperty = Property;

					DebitNoteLine.Item = Item;

					#endregion

					#region Valor Unitario producto

					PriceType Price = new PriceType();
					PriceAmountType PriceAmount = new PriceAmountType();
					PriceAmount.currencyID = moneda_detalle.ToString(); //(LISTADO DE VALORES DEFINIDO POR LA DIAN)
					PriceAmount.Value = decimal.Round(DocDet.ValorUnitario, 2);
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
