using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreriaGlobalHGInet.Objetos;
using LibreriaGlobalHGInet.Funciones;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using System.Xml.Serialization;
using LibreriaGlobalHGInet.General;
using System.Xml;
using System.IO;
using LibreriaGlobalHGInet.HgiNet.Controladores;
using HGInetUBLv2_1.DianListas;
using HGInetMiFacturaElectonicaData;
using HGInetUBLv2_1.XML;
using LibreriaGlobalHGInet.RegistroLog;

namespace HGInetUBLv2_1
{
	public partial class FacturaXMLv2_1
	{

		/// <summary>
		/// Crea el XML con la informacion de la factura en formato UBL
		/// </summary>
		/// <param name="id_documento">id del documento en base de datos (GUID)</param>
		/// <param name="documento">Objeto de tipo TblDocumentos que contiene la informacion de la factura</param>
		/// <param name="resolucion">Objeto que contiene los parametros de la DIAN</param>
		/// <returns>información del documento procesado</returns>
		public static FacturaE_Documento CrearDocumento(Guid id_documento, Factura documento, HGInetMiFacturaElectonicaData.ModeloServicio.ExtensionDian resolucion, string ambiente, ref string cadena_cufe)
		{
			TipoDocumento tipo = TipoDocumento.Factura;

			if (documento == null)
				throw new Exception("Documento esta vacío.");

			try
			{
				//Obtiene el nombre del archivo XML
				string nombre_archivo_xml = NombramientoArchivo.ObtenerXml(documento.Documento.ToString(), documento.DatosObligado.Identificacion.ToString(), TipoDocumento.Factura, documento.Prefijo);

				if (string.IsNullOrWhiteSpace(nombre_archivo_xml))
					throw new ApplicationException("El nombre del archivo es inválido.");

				
				InvoiceType facturaXML = new InvoiceType();
				XmlSerializerNamespaces namespaces_xml = NamespacesXML.ObtenerNamespaces(tipo);

				//------
				//-----Se debe cambiar datos para que no sean quemados
				facturaXML.UBLVersionID = new UBLVersionIDType();
				facturaXML.UBLVersionID.Value = "UBL 2.1";

				//------se debe enviar el tipo de operacion 13.1.5. Tipos de operación - 10 Estandar * - 09 AIU - 11 Mandatos - 12 Transporte - 13 Cambiario
				facturaXML.CustomizationID = new CustomizationIDType();

				facturaXML.CustomizationID.Value = "10";

				//Documento Tipo Sector Salud
				if (documento.SectorSalud != null)
				{
					//Se hace cambio para que al momento de validar el anticipo no rechace el documento la DIAN por ser del sector salud.
					facturaXML.CustomizationID.Value = "SS-CUFE";//"SS-Recaudo";
					//facturaXML.CustomizationID.schemeID = "SS-CUFE";
					if (documento.SectorSalud.TipoOperacion == 1)
					{
						facturaXML.CustomizationID.Value = "SS-Recaudo";
						facturaXML.CustomizationID.schemeID = "SS-CUFE";
					}
					if (documento.SectorSalud.TipoOperacion == 2)
					{
						facturaXML.CustomizationID.Value = "SS-SNum";
						//facturaXML.CustomizationID.schemeID = "SS-CUFE";
					}
					if (documento.SectorSalud.TipoOperacion == 3)
					{
						facturaXML.CustomizationID.Value = "SS-Reporte";
						//facturaXML.CustomizationID.schemeID = "SS-CUFE";
					}
					if (documento.SectorSalud.TipoOperacion == 4)
					{
						facturaXML.CustomizationID.Value = "SS-SinA porte";
						//facturaXML.CustomizationID.schemeID = "SS-CUFE";
					}
					if (documento.TipoOperacion == 1)//Contingencia
					{
						facturaXML.CustomizationID.Value = "SS-CUDE";//"SS-Recaudo";
						//facturaXML.CustomizationID.schemeID = "SS-CUFE";
					}
				}
				
				
				//Operacion - AIU
				try
				{
					if (documento.DocumentoDetalles.Exists(v => v.Aiu > 0) && documento.DocumentoDetalles.Exists(x => x.ProductoDescripcion.Contains("Contrato de servicios AIU por concepto de:")))
					{
						facturaXML.CustomizationID.Value = "09";
					}
				}
				catch (Exception)
				{
				}

				//Opercacion - Mandatos
				try
				{
					if (documento.DocumentoDetalles.Exists(m => m.DatosMandatario != null))
					{
						facturaXML.CustomizationID.Value = "11";
					}
				}
				catch (Exception)
				{
				}

				try
				{
					if (documento.TipoOperacion == 4)
					{
						facturaXML.CustomizationID.Value = "12";
					}
				}
				catch (Exception)
				{
				}

				#region Contingencia-Dia Sin Iva
				//Fechas Dia Sin IVA
				DateTime fecha1 = new DateTime(2022, 03, 11);
				DateTime fecha2 = new DateTime(2022, 06, 17);
				DateTime fecha3 = new DateTime(2022, 12, 02);

				bool dia_sin_iva = false;

				if (documento.Fecha == fecha1 || documento.Fecha == fecha2 || documento.Fecha == fecha3)
				{
					dia_sin_iva = true;
				}

				//Si es contingencia y es en el dia sin iva se debe poner esta informacion resolucion 000117 emitida por la DIAN 2021-10-21
				if (documento.TipoOperacion == 1 && dia_sin_iva == true)
				{
					facturaXML.CustomizationID.Value = "20-POS";

				}
				#endregion

				if (documento.TipoOperacion == 3 && documento.DatosAdquiriente.TipoIdentificacion != 13 && documento.DatosAdquiriente.TipoIdentificacion != 31)
				{
					//16.1.4.1 Procedencia de Vendedor: cbc:CustomizationID - No Residente
					facturaXML.CustomizationID.Value = "11";
				}

				facturaXML.ProfileID = new ProfileIDType();
				if (documento.TipoOperacion != 3 && documento.TipoOperacion != 5)
					facturaXML.ProfileID.Value = "DIAN 2.1: Factura Electrónica de Venta";
				else if(documento.TipoOperacion == 3)
					facturaXML.ProfileID.Value = "DIAN 2.1: documento soporte en adquisiciones efectuadas a no obligados a facturar.";
				else if (documento.TipoOperacion == 5)
					facturaXML.ProfileID.Value = "DIAN 2.1: Documento Equivalente POS";
				else if (documento.TipoOperacion == 6)
					facturaXML.ProfileID.Value = "DIAN 2.1: Documento Equivalente Tiquete de Transporte  Terrestre de Pasajeros";

				//---Ambiente al que se va enviar el documento: 1 - Produccion, 2 - Pruebas
				facturaXML.ProfileExecutionID = new ProfileExecutionIDType();
				facturaXML.ProfileExecutionID.Value = ambiente;//"2";

				#region facturaXML.ID - Número de documento

				string numero_documento = "";

				if (!string.IsNullOrEmpty(documento.Prefijo))
					numero_documento = string.Format("{0}", documento.Prefijo);

				numero_documento = string.Format("{0}{1}", numero_documento, documento.Documento.ToString());

				/*Número de documento: Número de facturaXML o facturaXML cambiaria.*/
				IDType ID = new IDType();
				//ID.Value = documento.IntDocumento.ToString();
				ID.Value = numero_documento;
				facturaXML.ID = ID;
				#endregion

				#region facturaXML.IssueDate - Fecha de la facturaXML
				/*Fecha de emision de la facturaXML*/
				IssueDateType IssueDate = new IssueDateType();
				IssueDate.Value = Convert.ToDateTime(documento.Fecha.ToString(Fecha.formato_fecha_hginet));
				facturaXML.IssueDate = IssueDate;
				#endregion

				#region facturaXML.IssueTime - Hora de la facturaXML
				/*Hora de emision de la facturaXML*/
				//----Se debe enviar la hora de emision con -5 horas
				IssueTimeType IssueTime = new IssueTimeType();
				//string hora_documento = fecha_univ.ToString("HH:mm:sszzz");
				IssueTime.Value = documento.Fecha.AddHours(5).ToString(Fecha.formato_hora_zona);//Convert.ToDateTime(hora_documento);//Convert.ToDateTime(documento.Fecha.ToString(Fecha.formato_hora_completa)).AddHours(5);//
				facturaXML.IssueTime = IssueTime;
				#endregion

				#region facturaXML.DueDate - Fecha vencimiento de la facturaXML
				DueDateType DueDate = new DueDateType();
				DueDate.Value = Convert.ToDateTime(documento.FechaVence.ToString(Fecha.formato_fecha_hginet));
				facturaXML.DueDate = DueDate;
				#endregion


				//Lineas del Detalle
				facturaXML.LineCountNumeric = new LineCountNumericType();
				facturaXML.LineCountNumeric.Value = documento.DocumentoDetalles.Count;

				#region facturaXML forma de pago.

				//Valida si este dato no llega y pone por defecto 1 - Contado
				if (documento.FormaPago == 0)
					documento.FormaPago = 1;

				//Valida que el termino de pago si esta en 0 y lo pone ZZZ - Acuerdo mutuo
				string termino_pago = string.Empty;
				if (documento.TerminoPago == 0)
					termino_pago = "ZZZ";
				else
					termino_pago = documento.TerminoPago.ToString();

				List<PaymentMeansType> PaymentMeans = new List<PaymentMeansType>();
				PaymentMeansType PaymentMean = new PaymentMeansType();
				PaymentMeansCodeType MeansCode = new PaymentMeansCodeType();
				MeansCode.Value = termino_pago;

				//MeansCode.name = Ctl_Enumeracion.ObtenerMedioPago(documento.FormaPago);
				PaymentMean.ID = new IDType();
				PaymentMean.ID.Value = documento.FormaPago.ToString();

				//---Si es pago a credito debe indicarse el identificador del pago
				PaymentMean.PaymentID = new PaymentIDType[1];
				PaymentIDType Paymentid = new PaymentIDType();
				Paymentid.Value = "12345";  /*** QUEMADO ***/
				PaymentMean.PaymentID[0] = Paymentid;

				PaymentMean.PaymentMeansCode = MeansCode;

				if (documento.FormaPago == 2) /*** QUEMADO ***/
				{
					#region facturaXML.DueDate - Fecha vencimiento de la facturaXML segun Forma de Pago
					//2 - Credito
					PaymentDueDateType PDueDate = new PaymentDueDateType();
					PDueDate.Value = Convert.ToDateTime(documento.FechaVence.ToString(Fecha.formato_fecha_hginet));
					PaymentMean.PaymentDueDate = PDueDate;

					#endregion

					//Validar Proceso de cuotas no esta definido
					#region Cuotas facturaXML

					List<PaymentTermsType> PaymentTermsTypes = new List<PaymentTermsType>();
					PaymentTermsType TermsType = new PaymentTermsType();
					//-----Validar de donde sale el 2, esta en ejemplo generico pero no en la documentacion-------
					TermsType.ReferenceEventCode = new ReferenceEventCodeType();
					TermsType.ReferenceEventCode.Value = "2"; /*** QUEMADO ***/
															  //
					TermsType.SettlementPeriod = new PeriodType();
					TermsType.SettlementPeriod.StartDate = new StartDateType();
					TermsType.SettlementPeriod.StartDate.Value = Convert.ToDateTime(documento.Fecha.ToString(Fecha.formato_fecha_hginet));
					TermsType.SettlementPeriod.EndDate = new EndDateType();
					TermsType.SettlementPeriod.EndDate.Value = Convert.ToDateTime(documento.FechaVence.ToString(Fecha.formato_fecha_hginet));
					TermsType.SettlementPeriod.DurationMeasure = new DurationMeasureType();
					TermsType.SettlementPeriod.DurationMeasure.Value = documento.Plazo;

					//----Validar si es la misma tabla para unidad de medida----
					TermsType.SettlementPeriod.DurationMeasure.unitCode = "DAY";//Ctl_Enumeracion.ObtenerUnidadMedida("DAY");
					PaymentTermsTypes.Add(TermsType);

					//-----Validar el Manejo de cuotas no hay documentacion----
					#region Cuotas

					if (documento.Cuotas != null && documento.Cuotas.Any())
					{

						PaymentTermsTypes = ObtenerCuotas(documento.Cuotas.ToList(), documento.TerminoPago, documento.Moneda).ToList();
					}
					else
					{
						//-----Validar con el ejemplo no hay documentacion-------
						//Terminos de pago de la facturaXML

						TermsType.ID = new IDType();
						TermsType.ID.Value = string.Format("{0}{1}", documento.Prefijo, documento.Documento.ToString());
						NoteType[] Note_Terms = new NoteType[1];
						NoteType Note_term = new NoteType();
						Note_term.Value = string.Format("Pago a {0} dias", documento.Plazo);
						Note_Terms[0] = Note_term;
						TermsType.Note = Note_Terms;
						//TermsType.PaymentMeansID = new PaymentMeansIDType[1];
						//TermsType.PaymentMeansID[0].Value = documento.TerminoPago.ToString();
						//TermsType.PaymentMeansID[0].schemeName = Ctl_Enumeracion.ObtenerTerminoPago(documento.TerminoPago);
						TermsType.Amount = new AmountType2();
						TermsType.Amount.Value = documento.Total;
						//CurrencyCodeContentType moneda_documento = Ctl_Enumeracion.ObtenerMoneda(documento.Moneda);
						TermsType.Amount.currencyID = documento.Moneda;


					}
					#endregion
					facturaXML.PaymentTerms = PaymentTermsTypes.ToArray();
					#endregion
				}
				PaymentMeans.Add(PaymentMean);
				facturaXML.PaymentMeans = PaymentMeans.ToArray();
				#endregion



				//Validar con tabla 5.1.1
				#region facturaXML.InvoiceTypeCode - Tipo de Documento
				/*Indicar si es una facturaXML de venta o una facturaXML cambiaria de compraventa*/
				InvoiceTypeCodeType InvoiceTypeCode = new InvoiceTypeCodeType();
				if (documento.TipoOperacion == 0)
				{
					InvoiceTypeCode.Value = "01"; /*** QUEMADO ***/
				}
				else if (documento.TipoOperacion == 1)//Contingencia
				{
					InvoiceTypeCode.Value = "03";
				}
				else if (documento.TipoOperacion == 2)//Exportacion
				{
					InvoiceTypeCode.Value = "02";
				}
				else if (documento.TipoOperacion == 3)//documento soporte en adquisiciones efectuadas a sujetos no obligados(equivalente)
				{
					InvoiceTypeCode.Value = "05";
				}
				else if (documento.TipoOperacion == 5)//documento soporte equivalente POS
				{
					InvoiceTypeCode.Value = "20";
					nombre_archivo_xml = nombre_archivo_xml.Replace("fv", "ds");
				}
				else if (documento.TipoOperacion == 6)//documento soporte equivalente POS Pasajeros
				{
					InvoiceTypeCode.Value = "35";
					nombre_archivo_xml = nombre_archivo_xml.Replace("fv", "ds");
				}
				else
				{
					InvoiceTypeCode.Value = "01";
				}

				facturaXML.InvoiceTypeCode = InvoiceTypeCode;
				#endregion

				//----solo recibe una sola nota por encabezado---
				//En el Anexo Tecnico la Ocurrencia tiene 0..N a fecha de 2019-07-26
				#region Note - Nota adicional (Resolución texto)

				string prefijo = string.Empty;
				if (!string.IsNullOrEmpty(documento.Prefijo))
					prefijo = string.Format("{0}-", documento.Prefijo);

				string dian_resolucion = string.Format(" {0} de {1} del {2}{3} al {4}{5}", resolucion.NumResolucion, resolucion.FechaResIni.ToString(Fecha.formato_fecha_hginet), prefijo, resolucion.RangoIni, prefijo, resolucion.RangoFin);

				List<string> notas_documento = new List<string>();

				// agrega la resolución en la 1ra posición
				notas_documento.Add(dian_resolucion);

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
				facturaXML.Note = Notes;


				/*
				NoteType[] Notes = new NoteType[1];
				NoteType nota = new NoteType();
				nota.Value = documento.Nota;
				Notes[0] = nota;
				facturaXML.Note = Notes;*/

				#endregion

				#region facturaXML.DocumentCurrencyCode - Divisa de la facturaXML
				/*Divisa consolidada aplicable a toda la facturaXML. Moneda con la que se presenta el documento*/
				DocumentCurrencyCodeType DocumentCurrencyCode = new DocumentCurrencyCodeType();
				DocumentCurrencyCode.Value = documento.Moneda;
				facturaXML.DocumentCurrencyCode = DocumentCurrencyCode;
				#endregion


				#region período al que aplica el documento
				// <cac:InvoicePeriod>
				if (documento.TipoOperacion != 3 && documento.TipoOperacion != 5)
				{
					PeriodType PeriodType = new PeriodType()
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
					//Documento Tipo Sector Salud
					if (documento.SectorSalud != null)
					{
						try
						{
							if (documento.SectorSalud.FechaIni.Year == 0001 && documento.SectorSalud.FechaFin.Year == 0001)
							{
								documento.SectorSalud.FechaIni = documento.Fecha;
								documento.SectorSalud.FechaFin = documento.Fecha;
							}
							PeriodType.StartDate.Value = documento.SectorSalud.FechaIni;
							PeriodType.EndDate.Value = documento.SectorSalud.FechaFin;
						}
						catch (Exception)
						{
						}
					}
					facturaXML.InvoicePeriod = new PeriodType[1];
					facturaXML.InvoicePeriod[0] = PeriodType;
				}

				#endregion

				//Validar
				#region facturaXML.OrderReference 

				//Referencia un documento de pedido
				if (!string.IsNullOrEmpty(documento.DocumentoRef) && documento.OrderReference == null)
				{
					documento.OrderReference = new ReferenciaAdicional();
					documento.OrderReference.Documento = documento.DocumentoRef;
				}

				if (documento.OrderReference != null && documento.TipoOperacion != 5)
				{
					facturaXML.OrderReference = new OrderReferenceType();

					OrderReferenceType DocOrderReference = new OrderReferenceType();
					DocOrderReference.ID = new IDType();
					DocOrderReference.ID.Value = documento.OrderReference.Documento; //(string.IsNullOrEmpty(documento.DocumentoRef)) ? string.Empty : documento.DocumentoRef.ToString();
					facturaXML.OrderReference = DocOrderReference;

				}

				#endregion

				#region facturaXML.DespatchDocumentReference 

				if (documento.DespatchDocument != null && documento.TipoOperacion != 5)
				{
					//Referencia un documento
					facturaXML.DespatchDocumentReference = new DocumentReferenceType[documento.DespatchDocument.Count];
					List<DocumentReferenceType> List_DocumentReference = new List<DocumentReferenceType>();
					foreach (var Despatch in documento.DespatchDocument)
					{
						DocumentReferenceType DocumentReference = new DocumentReferenceType();
						DocumentReference.ID = new IDType();
						DocumentReference.ID.Value = Despatch.Documento; //(string.IsNullOrEmpty(documento.PedidoRef)) ? string.Empty : documento.PedidoRef.ToString();
						List_DocumentReference.Add(DocumentReference);
					}
					facturaXML.DespatchDocumentReference = List_DocumentReference.ToArray();
				}

				#endregion

				//Referencia Adicional si se utiliza y cuando es contingencia
				#region facturaXML.AdditionalDocumentReference

				if (documento.DocumentosReferencia != null && documento.TipoOperacion != 5)
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
					facturaXML.AdditionalDocumentReference = AdditionalDocument.ToArray();
				}

				#endregion

				#region facturaXML.ReceiptDocument 

				if (documento.ReceiptDocument != null && documento.TipoOperacion != 5)
				{
					//Referencia un documento
					facturaXML.ReceiptDocumentReference = new DocumentReferenceType[documento.ReceiptDocument.Count];
					List<DocumentReferenceType> List_DocumentReference = new List<DocumentReferenceType>();
					foreach (var Receipt in documento.ReceiptDocument)
					{
						DocumentReferenceType DocumentReference = new DocumentReferenceType();
						DocumentReference.ID = new IDType();
						DocumentReference.ID.Value = Receipt.Documento; //(string.IsNullOrEmpty(documento.PedidoRef)) ? string.Empty : documento.PedidoRef.ToString();
						List_DocumentReference.Add(DocumentReference);
					}
					facturaXML.ReceiptDocumentReference = List_DocumentReference.ToArray();
				}

				#endregion

				/*** QUEMADO ***/
				//---Validar persona autorizada por el emisor a descargar el documento desde la base de datos de la DIAN
				if (documento.TipoOperacion != 3 && documento.TipoOperacion != 5)
				{
					#region facturaXML.TaxRepresentativeParty - Grupo con informaciones que definen una persona autorizada por el emisor a descargar el documento desde la base de datos de la DIAN 
					PartyType TaxRepresentativeParty = new PartyType();
					PartyIdentificationType[] PartyIdentification = new PartyIdentificationType[1];
					PartyIdentificationType PartyIdentificate = new PartyIdentificationType();
					PartyIdentificate.ID = new IDType();
					PartyIdentificate.ID.Value = "811021438";
					PartyIdentificate.ID.schemeID = "4";
					PartyIdentificate.ID.schemeName = "31";
					PartyIdentificate.ID.schemeAgencyID = "195";
					PartyIdentificate.ID.schemeAgencyName = "CO, DIAN (Dirección de Impuestos y Aduanas Nacionales)";
					PartyIdentification[0] = PartyIdentificate;
					TaxRepresentativeParty.PartyIdentification = PartyIdentification;
					facturaXML.TaxRepresentativeParty = TaxRepresentativeParty;
					#endregion
				}

				#region facturaXML.AccountingSupplierParty - Información del obligado a facturaXMLr
				if (documento.TipoOperacion != 3)
					facturaXML.AccountingSupplierParty = TerceroXML.ObtenerObligado(documento.DatosObligado, documento.Prefijo);
				else
				{
					if (documento.DatosAdquiriente.TipoIdentificacion == 13)
						documento.DatosAdquiriente.TipoIdentificacion = 31;

					facturaXML.AccountingSupplierParty = TerceroXML.ObtenerObligado(documento.DatosAdquiriente, documento.Prefijo);
				}
					
				#endregion

				#region facturaXML.AccountingCustomerParty - Información del Adquiriente
				/* Persona natural o jurídica que adquiere bienes y/o servicios y debe exigir facturaXML 
				51 o documento equivalente y, que tratándose de la facturaXML electrónica, 
				la recibe, rechaza, cuando
				52 sea del caso, y conserva para su posterior exhibición*/
				//facturaXML.AccountingCustomerParty = Aquiriente(documento.DatosTercero);

				#region facturaXML.AccountingCustomerParty //Información del Adquiriente
				/* Persona natural o jurídica que adquiere bienes y/o servicios y debe exigir facturaXML
				51 o documento equivalente y, que tratándose de la facturaXML electrónica, 
			    la recibe, rechaza, cuando
				52 sea del caso, y conserva para su posterior exhibición*/
				if (documento.TipoOperacion != 3)
				{
					if (documento.SectorSalud == null)
					{
						facturaXML.AccountingCustomerParty = TerceroXML.ObtenerAquiriente(documento.DatosAdquiriente);
					}
					else
					{
						facturaXML.AccountingCustomerParty = TerceroXML.ObtenerAquiriente(documento.DatosAdquiriente, documento.SectorSalud.DatosBeneficiario);
					}
				}					
				else
					facturaXML.AccountingCustomerParty = TerceroXML.ObtenerAquiriente(documento.DatosObligado);
				#endregion

				if ((documento.Descuentos != null && documento.Descuentos.Sum(x => (x.Valor)) > 0) || (documento.Cargos != null && documento.Cargos.Sum(x => (x.Valor)) > 0))
					facturaXML.AllowanceCharge = ValoresAdicionalesXML.ObtenerValoresAd(documento);

				if (documento.DatosAdquiriente.DireccionEntrega != null && documento.TipoOperacion != 5)
				{
					DeliveryType[] delivery = new DeliveryType[1];
					DeliveryType deliverytype = new DeliveryType();

					AddressType Address = new AddressType();

					//----5.4.3. Municipios:  cbc:CityName Ver listado del DANE
					Address.ID = new IDType();
					Address.ID.Value = (!string.IsNullOrEmpty(documento.DatosAdquiriente.DireccionEntrega.CodigoCiudad) && documento.DatosAdquiriente.DireccionEntrega.CodigoPais.Equals("CO")) ? documento.DatosAdquiriente.DireccionEntrega.CodigoCiudad : "";
					CityNameType City = new CityNameType();
					if (documento.DatosAdquiriente.DireccionEntrega.CodigoPais.Equals("CO"))
					{
						ListaMunicipio list_municipio = new ListaMunicipio();
						ListaItem municipio = list_municipio.Items.Where(d => d.Codigo.Equals(documento.DatosAdquiriente.DireccionEntrega.CodigoCiudad)).FirstOrDefault();
						City.Value = municipio.Nombre; //Ciudad (LISTADO DE VALORES DEFINIDO POR LA DIAN)
						documento.DatosAdquiriente.DireccionEntrega.Ciudad = municipio.Nombre;
					}
					else
					{
						City.Value = documento.DatosAdquiriente.DireccionEntrega.Ciudad;
					}
					Address.CityName = City;


					//5.4.2. Departamentos (ISO 3166-2:CO):  cbc:CountrySubentity, cbc:CountrySubentityCode
					CountrySubentityType CountrySubentity = new CountrySubentityType();
					if (documento.DatosAdquiriente.DireccionEntrega.CodigoPais.Equals("CO"))
					{
						ListaDepartamentos list_depart = new ListaDepartamentos();
						ListaItem departamento = list_depart.Items.Where(d => d.Codigo.Equals(documento.DatosAdquiriente.DireccionEntrega.CodigoDepartamento)).FirstOrDefault();
						CountrySubentity.Value = departamento.Nombre; //Listado de Departamentos el Nombre
						Address.CountrySubentity = CountrySubentity;
						CountrySubentityCodeType CountrySubentityCode = new CountrySubentityCodeType();
						CountrySubentityCode.Value = documento.DatosAdquiriente.DireccionEntrega.CodigoDepartamento; //Listado de Departamentos el codigo
						Address.CountrySubentityCode = CountrySubentityCode;
						documento.DatosAdquiriente.DireccionEntrega.Departamento = departamento.Nombre;
					}
					else
					{
						CountrySubentity.Value = documento.DatosAdquiriente.DireccionEntrega.Departamento; //Listado de Departamentos el Nombre
						Address.CountrySubentity = CountrySubentity;
						CountrySubentityCodeType CountrySubentityCode = new CountrySubentityCodeType();
						CountrySubentityCode.Value = (string.IsNullOrEmpty(documento.DatosAdquiriente.DireccionEntrega.CodigoDepartamento)) ? "" : documento.DatosAdquiriente.DireccionEntrega.CodigoDepartamento; //Listado de Departamentos el codigo
						Address.CountrySubentityCode = CountrySubentityCode;
					}

					//Direccion
					//Informar la dirección, sin ciudad ni departamento.
					//Si el adquirente no es responsable de IVA entonces se puede informar solo este elemento en dirección. 
					AddressLineType[] AddressLines = new AddressLineType[1];
					AddressLineType AddressLine = new AddressLineType();
					LineType Line = new LineType();
					Line.Value = documento.DatosAdquiriente.DireccionEntrega.Direccion;
					AddressLine.Line = Line;

					AddressLines[0] = AddressLine;
					Address.AddressLine = AddressLines;

					//Zona Postal - Obligatorio para emisores y Adquirentes Responsables 
					Address.PostalZone = new PostalZoneType();
					Address.PostalZone.Value = documento.DatosAdquiriente.DireccionEntrega.CodigoPostal;//Listado de Zona Postal de Colombia

					//5.4.1. Países (ISO 3166-1): cbc:IdentificationCode 
					//ISO 3166-1 alfa-2: Códigos de país de das letras. Si recomienda como el código de propósito
					//general.Estos códigos se utilizan por ejemplo en internet como dominios geográficos de nivel superior.
					CountryType Country = new CountryType();

					IdentificationCodeType IdentificationCode = new IdentificationCodeType();
					IdentificationCode.Value = documento.DatosAdquiriente.DireccionEntrega.CodigoPais; //Pais (LISTADO DE VALORES DEFINIDO POR LA DIAN 5.4.1)
					Country.IdentificationCode = IdentificationCode;
					Country.Name = new NameType1();
					ListaPaises list_paises = new ListaPaises();
					ListaItem pais = list_paises.Items.Where(d => d.Codigo.Equals(documento.DatosAdquiriente.DireccionEntrega.CodigoPais)).FirstOrDefault();
					Country.Name.Value = pais.Nombre;//"Colombia";//Pais (LISTADO DE VALORES DEFINIDO POR LA DIAN 5.4.1)
					Country.Name.languageID = "es";
					Address.Country = Country;

					deliverytype.DeliveryAddress = Address;
					

					ActualDeliveryDateType fecha_entrega = new ActualDeliveryDateType();
					fecha_entrega.Value = documento.FechaEntrega;
					deliverytype.ActualDeliveryDate = fecha_entrega;

					delivery[0] = deliverytype;
					facturaXML.Delivery = delivery;

				}


				if (documento.TipoOperacion == TipoOperacion.FacturaExportacion.GetHashCode() && documento.TipoEntrega != null && documento.TipoOperacion != 5)
				{
					ListaTipoEntrega list_tipo_entrega = new ListaTipoEntrega();
					ListaItem entrega = list_tipo_entrega.Items.Where(d => d.Codigo.Equals(documento.TipoEntrega.CodCondicionEntrega)).FirstOrDefault();
					DeliveryTermsType TerminosEntrega = new DeliveryTermsType();
					TerminosEntrega.LossRiskResponsibilityCode = new LossRiskResponsibilityCodeType();
					TerminosEntrega.LossRiskResponsibilityCode.Value = entrega.Codigo;
					TerminosEntrega.LossRisk = new LossRiskType[1];
					LossRiskType lossRisk = new LossRiskType();
					lossRisk.Value = entrega.Descripcion;
					TerminosEntrega.LossRisk[0] = lossRisk;
					TerminosEntrega.SpecialTerms = new SpecialTermsType[1];
					SpecialTermsType specialTermsType = new SpecialTermsType();
					specialTermsType.Value = documento.TipoEntrega.TerminosEntrega;
					TerminosEntrega.SpecialTerms[0] = specialTermsType;
					facturaXML.DeliveryTerms = TerminosEntrega;

				}


				#region Anticipos
				if (documento.Anticipos != null && documento.Anticipos.Count > 0)
				{
					List<PaymentType> list_anticipos = new List<PaymentType>();
					foreach (var item in documento.Anticipos)
					{
						PaymentType anticipo = new PaymentType();
						anticipo.ID = new IDType();
						anticipo.ID.Value = item.Codigo;
						anticipo.PaidAmount = new PaidAmountType();
						anticipo.PaidAmount.Value = item.Valor;
						anticipo.PaidAmount.currencyID = Ctl_Enumeracion.ObtenerMoneda(documento.Moneda).ToString();
						list_anticipos.Add(anticipo);
					}
					facturaXML.PrepaidPayment = list_anticipos.ToArray();
				}
				#endregion


				#endregion

				/*** QUEMADO ***/
				//---Validar Se llena con informacion del ejemplo Factura Genericas
				#region PaymentExchangeRate - Conversión de divisas: cac:PaymentExchangeRate 

				if (documento.TipoOperacion != 3 && documento.TipoOperacion != 5)
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
					facturaXML.PaymentExchangeRate = PaymentExchangeRate;
				}

				#endregion

				#region facturaXML.InvoiceLine - Línea de facturaXML
				/*Elemento que agrupa todos los campos de una línea de facturaXML. Detalle del documento*/
				bool autoretenedor = false;
				string resp = LibreriaGlobalHGInet.Formato.Coleccion.ConvertListToString(documento.DatosObligado.Responsabilidades, ";");
				if (resp.Contains("O-15") == true)
					autoretenedor = true;
				facturaXML.InvoiceLine = ObtenerDetalleDocumento(documento.DocumentoDetalles.ToList(), documento.Moneda, autoretenedor, documento.DatosObligado.Identificacion.ToString(), documento.TipoOperacion, facturaXML.CustomizationID.Value);
				#endregion

				#region	facturaXML.TaxTotal - Impuesto y Impuesto Retenido
				/*Impuesto Retenido: Elemento raíz compuesto utilizado para informar de un impuesto	retenido. 
				 Impuesto: Elemento raíz compuesto utilizado para informar de un impuesto. */
				facturaXML.TaxTotal = ImpuestosXML.ObtenerImpuestos(documento.DocumentoDetalles.ToList(), documento.Moneda, documento.VersionAplicativo);

				#endregion

				#region facturaXML.LegalMonetaryTotal //Datos Importes Totales
				/*Agrupación de campos relativos a los importes totales aplicables a la	facturaXML. Estos importes son calculados teniendo
                en cuenta las líneas de facturaXML y elementos a nivel de facturaXML, como descuentos, cargos, impuestos, etc*/

				decimal subtotal = 0.00M;
				subtotal = facturaXML.InvoiceLine.Sum(s => s.LineExtensionAmount.Value);

				decimal base_impuesto = 0.00M;

				//Se valida si tiene una base diferente por si el mismo item tiene dos impuestos diferentes
				if ((documento.DocumentoDetalles.Sum(b => b.BaseImpuestoIva) == documento.DocumentoDetalles.Sum(s => s.ValorSubtotal)) && (documento.DocumentoDetalles.Where(g => g.ProductoGratis == true) == null))
				{
					base_impuesto = subtotal;
				}
				else
				{
					base_impuesto = facturaXML.InvoiceLine.Sum(s => s.TaxTotal.Take(1).Sum(b => b.TaxSubtotal.Where(k => k.TaxableAmount != null).Sum(v => v.TaxableAmount.Value)));
				}

				decimal impuestos = facturaXML.TaxTotal.Sum(i => i.TaxAmount.Value);

				facturaXML.LegalMonetaryTotal = TotalesXML.ObtenerTotales(documento,subtotal,impuestos, base_impuesto);

				#endregion

				/*** QUEMADO ***/
				UUIDType UUID = new UUIDType();
				//-----Se agrega Ambiente al cual se va enviar el documento
				string CUFE = string.Empty;
				if (facturaXML.InvoiceTypeCode.Value.Equals("01") || facturaXML.InvoiceTypeCode.Value.Equals("02"))
				{
					CUFE = CalcularCUFE(facturaXML, resolucion.ClaveTecnicaDIAN, facturaXML.ProfileExecutionID.Value, ref cadena_cufe);//resolucion.ClaveTecnicaDIAN
					UUID.schemeName = "CUFE-SHA384";
				}
				else if (facturaXML.InvoiceTypeCode.Value.Equals("03") || facturaXML.InvoiceTypeCode.Value.Equals("20"))
				{
					CUFE = CalcularCUFE(facturaXML, resolucion.PinSoftware, facturaXML.ProfileExecutionID.Value, ref cadena_cufe);
					UUID.schemeName = "CUDE-SHA384";
				}
				else if (facturaXML.InvoiceTypeCode.Value.Equals("05"))
				{
					CUFE = CalcularCUFE(facturaXML, resolucion.PinSoftware, facturaXML.ProfileExecutionID.Value, ref cadena_cufe);
					UUID.schemeName = "CUDS-SHA384";
				}

				UUID.Value = CUFE;
				UUID.schemeID = facturaXML.ProfileExecutionID.Value; //"2";
				facturaXML.UUID = UUID;


				#region factura.UBLExtensions
				XmlDocument doc = new XmlDocument();
				doc.LoadXml("<firma></firma>");

				List<UBLExtensionType> UBLExtensions = new List<UBLExtensionType>();

				//Resolucion de ejemplos de la DIAN
				/*
				HGInetMiFacturaElectonicaData.ModeloServicio.ExtensionDian resolucion = new HGInetMiFacturaElectonicaData.ModeloServicio.ExtensionDian();
				resolucion.ClaveTecnicaDIAN = "fc8eac422eba16e22ffd8c6f94b3f40a6e38162c";//"wqeeqqew43324432234ewrewr";
				resolucion.FechaResIni = new DateTime(2019, 01, 19);
				resolucion.FechaResFin = new DateTime(2030, 01, 19);
				resolucion.IdSoftware = "def923e2-8326-42e2-a022-d0fa4a2f8188";//"22fa3f6f-c40d-434b-a3db-08a5d435a372";//
				resolucion.NitProveedor = "811021438";
				resolucion.PinSoftware = "05097";
				resolucion.NumResolucion = "18760000001";
				resolucion.Prefijo = "SETP";
				resolucion.RangoIni = 990000000;
				resolucion.RangoFin = 995000000;
				resolucion.TipoDocumento = 1;
				*/

				//Informacion del QR
				string ruta_qr_Dian = string.Empty;
				if (facturaXML.ProfileExecutionID.Value.Equals("2"))
				{
					ruta_qr_Dian = string.Format("{0}{1}", "https://catalogo-vpfe-hab.dian.gov.co/document/searchqr?documentkey=", facturaXML.UUID.Value);
				}
				else
				{
					ruta_qr_Dian = string.Format("{0}{1}", "https://catalogo-vpfe.dian.gov.co/document/searchqr?documentkey=", facturaXML.UUID.Value);
				}

				////Proceso para el sector Salud
				//documento.SectorSalud = new Salud();
				//documento.SectorSalud.CamposSector = new List<CampoValor>();
				//CampoValor dato1 = new CampoValor();
				//CampoValor dato2 = new CampoValor();

				//dato1.Descripcion = "1";
				//dato1.Valor = "0521";

				//dato2.Descripcion = "2";
				//dato2.Valor = "CC";

				//documento.SectorSalud.CamposSector.Add(dato1);
				//documento.SectorSalud.CamposSector.Add(dato2);

				//documento.SectorSalud.URLDescargaAdjuntos = "URL de descarga de archivos";
				//documento.SectorSalud.URLWebService = "URL del webservices";

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
				UBLExtensionDian.ExtensionContent = ExtensionDian.Obtener(resolucion, tipo, facturaXML.ID.Value, ruta_qr_Dian);
				UBLExtensions.Add(UBLExtensionDian);

				// Extension de la firma
				UBLExtensionType UBLExtensionFirma = new UBLExtensionType();
				UBLExtensionFirma.ExtensionContent = doc.DocumentElement;
				UBLExtensions.Add(UBLExtensionFirma);

				// Extension POS
				if (documento.TipoOperacion == 5 || documento.TipoOperacion == 6)
				{
					UBLExtensionType UBLExtensionSector = new UBLExtensionType();
					UBLExtensionSector.ExtensionContent = ExtensionPos.ObtenerSW(documento.DatosPos.DatosSoftware);
					UBLExtensions.Add(UBLExtensionSector);

					if (documento.TipoOperacion == 5)
					{
						UBLExtensionSector = new UBLExtensionType();
						UBLExtensionSector.ExtensionContent = ExtensionPos.ObtenerBenComprador(documento.DatosPos.DatosComprador);
						UBLExtensions.Add(UBLExtensionSector);

						UBLExtensionSector = new UBLExtensionType();
						UBLExtensionSector.ExtensionContent = ExtensionPos.ObtenerPuntoVenta(documento.DatosPos.DatosPuntoVenta);
						UBLExtensions.Add(UBLExtensionSector);
					}

					if (documento.TipoOperacion == 6)
					{
						UBLExtensionSector = new UBLExtensionType();
						UBLExtensionSector.ExtensionContent = ExtensionPos.ObtenerInfoTicket(documento.DatosPos.DatosTicketPasajero);
						UBLExtensions.Add(UBLExtensionSector);
					}

				}

				// Extension de HGI
				//UBLExtensionType UBLExtensionHgi = new UBLExtensionType();
				//UBLExtensionHgi.ExtensionContent = ExtensionHgiSas.Obtener(id_documento, documento);
				//UBLExtensions.Add(UBLExtensionHgi);


				facturaXML.UBLExtensions = UBLExtensions.ToArray();
				#endregion

				// convierte los datos del objeto en texto XML 
				//StringBuilder txt_xml = ConvertirXml(facturaXML, namespaces_xml);
				StringBuilder txt_xml = ConvertirXML.Convertir(facturaXML, namespaces_xml, TipoDocumento.Factura);

				// valida el namespace xmlns:schemaLocation y lo reemplaza para Google Chrome
				TextReader textReader = new StringReader(txt_xml.ToString());
				string texto_xml = textReader.ReadToEnd();

				if (texto_xml.Contains("xmlns:schemaLocation"))
				{
					texto_xml = texto_xml.Replace("xmlns:schemaLocation", "xsi:schemaLocation");

					if (documento.TipoOperacion == 5)
					{
						texto_xml = texto_xml.Replace("FabricanteSoftware xmlns=\"\"", "FabricanteSoftware");
						texto_xml = texto_xml.Replace("BeneficiosComprador xmlns=\"\"", "BeneficiosComprador");
						texto_xml = texto_xml.Replace("PuntoVenta xmlns=\"\"", "PuntoVenta");
					}

					if (documento.TipoOperacion == 6)
					{
						texto_xml = texto_xml.Replace("FabricanteSoftware xmlns=\"\"", "FabricanteSoftware");
						texto_xml = texto_xml.Replace("InformacionAdicional xmlns=\"\"", "InformacionAdicional");
					}

					txt_xml = new StringBuilder(texto_xml);
				}

				FacturaE_Documento xml_sin_firma = new FacturaE_Documento();
				xml_sin_firma.Documento = documento;
				xml_sin_firma.NombreXml = nombre_archivo_xml;
				xml_sin_firma.DocumentoXml = txt_xml;
				xml_sin_firma.CUFE = CUFE;

				return xml_sin_firma;

			}
			catch (Exception excepcion)
			{
				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Archivos, MensajeTipo.Error, MensajeAccion.creacion);
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}


		}


		/// <summary>
		/// Calcula el codigo CUFE
		/// </summary>
		/// <param name="factura">Objeto de tipo InvoiceType que contiene la informacion de la factura</param>
		/// <param name="clave_tecnica">Clave técnica de la resolución ó Pin del Sw cuando es contingencia</param>
		/// <param name="ambiente">Número de identificación del ambiente utilizado por el Obligado para emitir la factura Seccion 6.1.1 (1=AmbienteProduccion , 2: AmbientePruebas)</param>
		/// <returns>Texto con la encriptación del CUFE</returns>        
		public static string CalcularCUFE(InvoiceType factura, string clave_tecnica, string ambiente, ref string cadena_cufe)
		{
			try
			{
				if (factura == null)
					throw new Exception("Los datos de la factura son inválidos.");
				if (string.IsNullOrWhiteSpace(clave_tecnica))
					throw new Exception("La clave técnica es inválida.");
				if (string.IsNullOrWhiteSpace(ambiente))
					throw new Exception("El ambiente es inválido.");

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
				NumAdq = Número de identificación del adquirente sin puntos ni guiones, sin digito de verificación. 
				ClTec = Clave técnica del rango de facturación.
				TipoAmbiente = Número de identificación del ambiente utilizado por el contribuyente para emitir la factura Seccion 6.1.1 (1=AmbienteProduccion , 2: AmbientePruebas) 

				Composición del CUFE = SHA-384(NumFac + FecFac + HorFac + ValFac + CodImp1 + ValImp1 + CodImp2 + ValImp2 + CodImp3 + ValImp3 + ValTot + NitOFE +  NumAdq + ClTec + TipoAmbie)  
			
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
				TipoAmb = Invoice/cbc:ProfileExecutionID  			 
				*/
				#endregion

				#region Creación Código CUFE

				string codigo_impuesto = string.Empty;
				DateTime fecha = factura.IssueDate.Value;
				DateTime fecha_hora = Convert.ToDateTime(factura.IssueTime.Value);
				TimeSpan hora = new TimeSpan(fecha_hora.Hour, fecha_hora.Minute, fecha_hora.Second);
				//fecha = fecha.Date + hora;

				//string hora_gmt = TimeZoneInfo.ConvertTimeToUtc(fecha).ToString("yyyy-MM-ddHH:mm:sszz:ss");

				string NumFac = factura.ID.Value;
				string FecFac = string.Format("{0}{1}", factura.IssueDate.Value.ToString(Fecha.formato_fecha_hginet), factura.IssueTime.Value);//string.Format("{0}{1}",fecha.ToString(Fecha.formato_fecha_hora_completa),fecha_hora.ToString(Fecha.formato_hora_completa));//TimeZoneInfo.ConvertTimeToUtc(fecha).ToString("yyyy-MM-ddHH:mm:sszz:ss");//
				string ValFac = factura.LegalMonetaryTotal.LineExtensionAmount.Value.ToString();

				//Impuesto 1
				string CodImp1 = "01";
				decimal ValImp1 = 0.00M;

				//Impuesto 2
				string CodImp2 = "04";
				decimal ValImp2 = 0.00M;

				//Impuesto 3
				string CodImp3 = "03";
				decimal ValImp3 = 0.00M;

				if (factura.TaxTotal != null)
				{
					for (int i = 0; i < factura.TaxTotal.Count(); i++)
					{
						for (int j = 0; j < factura.TaxTotal[i].TaxSubtotal.Count(); j++)
						{
							codigo_impuesto = factura.TaxTotal[i].TaxSubtotal[j].TaxCategory.TaxScheme.ID.Value;

							if (codigo_impuesto.Equals("01"))
							{
								ValImp1 += factura.TaxTotal[i].TaxSubtotal[j].TaxAmount.Value;
							}
							else if (codigo_impuesto.Equals("04"))
							{
								ValImp2 += factura.TaxTotal[i].TaxSubtotal[j].TaxAmount.Value;
							}
							else if (codigo_impuesto.Equals("03"))
							{
								ValImp3 += factura.TaxTotal[i].TaxSubtotal[j].TaxAmount.Value;
							}
						}
					}
				}

				string ValImp = factura.LegalMonetaryTotal.PayableAmount.Value.ToString();

				string NitOFE = string.Empty;

				for (int contador_obligado_facturar = 0; contador_obligado_facturar < factura.AccountingSupplierParty.Party.PartyTaxScheme.Count(); contador_obligado_facturar++)
				{
					NitOFE = factura.AccountingSupplierParty.Party.PartyTaxScheme[contador_obligado_facturar].CompanyID.Value;
				}

				//string TipAdq = string.Empty;
				string NumAdq = string.Empty;

				for (int contador_adquiriente = 0; contador_adquiriente < factura.AccountingCustomerParty.Party.PartyTaxScheme.Count(); contador_adquiriente++)
				{
					//TipAdq = factura.AccountingCustomerParty.Party.PartyTaxScheme[contador_adquiriente].CompanyID.schemeName;
					NumAdq = factura.AccountingCustomerParty.Party.PartyTaxScheme[contador_adquiriente].CompanyID.Value;
				}

				string cufe_encriptado = string.Empty;

				if (!factura.InvoiceTypeCode.Value.Equals("05"))
				{
					cadena_cufe = "NumDoc: " + NumFac + ", "
					+ "FechaDoc: " + FecFac + ", "
					+ "SubtotalDoc:" + ValFac.Replace(",", ".") + ", "
					+ "CodImp1: " + CodImp1 + ", "
					+ "ValImp1: " + ValImp1.ToString().Replace(",", ".") + ", "
					+ "CodImp2: " + CodImp2 + ", "
					+ "ValImp2: " + ValImp2.ToString().Replace(",", ".") + ", "
					+ "CodImp3: " + CodImp3 + ", "
					+ "ValImp3: " + ValImp3.ToString().Replace(",", ".") + ", "
					+ "TotalDoc: " + ValImp.Replace(",", ".") + ", "
					+ "NitObligado: " + NitOFE + ", "
					+ "NitAdquiriente: " + NumAdq + ", "
					+ "Clave o Pin: " + clave_tecnica + ", "
					+ "Ambiente: " + ambiente + ", "
					;

					cufe_encriptado = Ctl_CalculoCufe.CufeFacturaV2(clave_tecnica, string.Empty, NumFac, FecFac, NitOFE, ambiente, NumAdq, Convert.ToDecimal(ValImp), Convert.ToDecimal(ValFac), Convert.ToDecimal(ValImp1), Convert.ToDecimal(ValImp2), Convert.ToDecimal(ValImp3), false);
				}
				else
				{
					cadena_cufe = "NumDoc: " + NumFac + ", "
				   + "FechaDoc: " + FecFac + ", "
				   + "SubtotalDoc:" + ValFac.Replace(",", ".") + ", "
				   + "CodImp1: " + CodImp1 + ", "
				   + "ValImp1: " + ValImp1.ToString().Replace(",", ".") + ", "
				   + "TotalDoc: " + ValImp.Replace(",", ".") + ", "
				   + "NitObligado: " + NitOFE + ", "
				   + "NitAdquiriente: " + NumAdq + ", "
				   + "Clave o Pin: " + clave_tecnica + ", "
				   + "Ambiente: " + ambiente + ", "
				   ;

					string cufe = NumFac
					  + FecFac
					  + ValFac.Replace(",", ".")
					  + CodImp1
					  + ValImp1.ToString().Replace(",", ".")
					  + ValImp.Replace(",", ".")
					  + NitOFE
					  + NumAdq
					  + clave_tecnica
					  + ambiente
					  ;

					cufe_encriptado = Encriptar.Encriptar_SHA384(cufe);
				}

				

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
		private static SupplierPartyType ObtenerObligado(Tercero empresa)
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
				AccountingSupplierParty.AdditionalAccountID = new AdditionalAccountIDType[1];
				AccountingSupplierParty.AdditionalAccountID[0] = AdditionalAccountID;
				#endregion

				#region Grupo con informaciones sobre el nombre comercial del emisor 
				//---Se puede tener Nombre principal, Sucursal, Razon Social
				PartyNameType[] PartyNameType = new PartyNameType[2];
				PartyNameType partyName = new PartyNameType();
				//---Razon social
				NameType1 Name_RZ = new NameType1();
				Name_RZ.Value = empresa.RazonSocial;
				partyName.Name = Name_RZ;
				PartyNameType[0] = partyName;
				//---Nombre Comercial
				NameType1 Name_NC = new NameType1();
				partyName = new PartyNameType();
				Name_NC.Value = empresa.NombreComercial;
				partyName.Name = Name_NC;
				PartyNameType[1] = partyName;

				Party.PartyName = PartyNameType;
				#endregion


				#region Dirección---Grupo con informaciones con respeto a la localización física 

				LocationType1 PhysicalLocation = new LocationType1();
				AddressType Address = new AddressType();


				//----5.4.3. Municipios:  cbc:CityName Ver listado del DANE
				Address.ID = new IDType();
				Address.ID.Value = "05001";/*** QUEMADO ***/
				CityNameType City = new CityNameType();
				City.Value = empresa.Ciudad; //Ciudad (LISTADO DE VALORES DEFINIDO POR LA DIAN)
				Address.CityName = City;

				//5.4.2. Departamentos (ISO 3166-2:CO):  cbc:CountrySubentity, cbc:CountrySubentityCode
				CountrySubentityType CountrySubentity = new CountrySubentityType();
				CountrySubentity.Value = "Antioquia";//Listado de Departamentos el Nombre /*** QUEMADO ***/
				Address.CountrySubentity = CountrySubentity;
				CountrySubentityCodeType CountrySubentityCode = new CountrySubentityCodeType();
				CountrySubentityCode.Value = "05";//Listado de Departamentos el codigo  /*** QUEMADO ***/

				//Direccion
				AddressLineType[] AddressLines = new AddressLineType[1];
				AddressLineType AddressLine = new AddressLineType();
				LineType Line = new LineType();
				Line.Value = empresa.Direccion;
				AddressLine.Line = Line;

				AddressLines[0] = AddressLine;
				Address.AddressLine = AddressLines;

				//Zona Postal
				Address.PostalZone = new PostalZoneType();
				Address.PostalZone.Value = "";//Listado de Zona Postal de Colombia 

				//5.4.1. Países (ISO 3166-1): cbc:IdentificationCode 
				//ISO 3166-1 alfa-2: Códigos de país de das letras. Si recomienda como el código de propósito
				//general.Estos códigos se utilizan por ejemplo en internet como dominios geográficos de nivel superior.
				CountryType Country = new CountryType();
				IdentificationCodeType IdentificationCode = new IdentificationCodeType();
				IdentificationCode.Value = empresa.CodigoPais; //Pais (LISTADO DE VALORES DEFINIDO POR LA DIAN 5.4.1)
				Country.IdentificationCode = IdentificationCode;
				Country.Name = new NameType1();
				Country.Name.Value = "Colombia";//Pais (LISTADO DE VALORES DEFINIDO POR LA DIAN 5.4.1) /*** QUEMADO ***/
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

				//Identificacion
				CompanyIDType CompanyID = new CompanyIDType();

				//---Validar si es NIT
				CompanyID.Value = empresa.Identificacion.ToString();
				//---Si es Nit debe star bien calculado
				CompanyID.schemeID = empresa.IdentificacionDv.ToString();
				//----//Tipo documento (LISTADO DE VALORES DEFINIDO POR LA DIAN 5.2.1)
				CompanyID.schemeName = empresa.TipoIdentificacion.ToString();
				CompanyID.schemeAgencyID = "195"; /*** QUEMADO ***/
				CompanyID.schemeAgencyName = "CO, DIAN (Direccion de Impuestos y Aduanas Nacionales)"; /*** QUEMADO ***/
				PartyTaxScheme.CompanyID = CompanyID;

				//Regimen Fiscal y Responsabilidades Tributarias
				//----Validar si cambia la ocurrecia de las responsabilidades esta de 1..1 y deberia estar 1..N
				TaxLevelCodeType TaxLevelCode = new TaxLevelCodeType();
				//-----Listado 5.2.4
				TaxLevelCode.listName = "05"; /*** QUEMADO ***/
											  //---Listado 5.2.7 Responsabilidades pero solo permite 1 actualmente
				TaxLevelCode.Value = empresa.Regimen.ToString();
				PartyTaxScheme.TaxLevelCode = TaxLevelCode;

				//Grupo de información para informar dirección fiscal - RUT
				//--Puede ser diferente la localizacion Fisica a la del RUT
				PartyTaxScheme.RegistrationAddress = new AddressType();
				PartyTaxScheme.RegistrationAddress = Address;

				//---Tributos Si el adquirente es responsable, el NIT debe estar activo en el RUT-- Ocurrencia 0..N
				//--Tabla 5.2.2
				//--Validar el llenado de las diferentes obligaciones de tributos
				TaxSchemeType TaxScheme = new TaxSchemeType();
				TaxScheme.ID = new IDType();
				TaxScheme.ID.Value = "01";/*** QUEMADO ***/
				TaxScheme.Name = new NameType1();
				TaxScheme.Name.Value = "IVA";/*** QUEMADO ***/
				PartyTaxScheme.TaxScheme = TaxScheme;
				PartyTaxSchemes[0] = PartyTaxScheme;
				Party.PartyTaxScheme = PartyTaxSchemes;
				#endregion

				#region PartyLegalEntity -- Grupo de informaciones legales del adquirente 
				PartyLegalEntityType[] PartyLegalEntitys = new PartyLegalEntityType[1];
				PartyLegalEntityType PartyLegalEntity = new PartyLegalEntityType();
				PartyLegalEntity.RegistrationName = new RegistrationNameType();
				PartyLegalEntity.RegistrationName.Value = empresa.RazonSocial;
				PartyLegalEntity.CompanyID = new CompanyIDType();
				PartyLegalEntity.CompanyID = CompanyID;

				//Grupo de informaciones legales del emisor 
				PartyLegalEntity.CorporateRegistrationScheme = new CorporateRegistrationSchemeType();

				//Prefijo de la facturación usada para el punto de venta
				//---Validar---obligatorio para el obligado ocurrencia 1..1
				PartyLegalEntity.CorporateRegistrationScheme.ID = new IDType();
				PartyLegalEntity.CorporateRegistrationScheme.ID.Value = "SETP"; /*** QUEMADO ***/

				//Número de matrícula mercantil (identificador de sucursal: punto de facturación)
				//---Validar--ocurrencia 0..1
				/*
				PartyLegalEntity.CorporateRegistrationScheme.Name = new NameType1();
				PartyLegalEntity.CorporateRegistrationScheme.Name.Value = "81248";*/

				PartyLegalEntitys[0] = PartyLegalEntity;
				Party.PartyLegalEntity = PartyLegalEntitys;
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
		private static CustomerPartyType ObtenerAquiriente(Tercero tercero)
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
				Address.ID.Value = "05001"; /*** QUEMADO ***/
				CityNameType City = new CityNameType();
				City.Value = tercero.Ciudad; //Ciudad (LISTADO DE VALORES DEFINIDO POR LA DIAN)
				Address.CityName = City;

				//5.4.2. Departamentos (ISO 3166-2:CO):  cbc:CountrySubentity, cbc:CountrySubentityCode
				CountrySubentityType CountrySubentity = new CountrySubentityType();
				CountrySubentity.Value = "Antioquia";//Listado de Departamentos el Nombre /*** QUEMADO ***/
				Address.CountrySubentity = CountrySubentity;
				CountrySubentityCodeType CountrySubentityCode = new CountrySubentityCodeType();
				CountrySubentityCode.Value = "05";//Listado de Departamentos el codigo /*** QUEMADO ***/

				//Direccion
				AddressLineType[] AddressLines = new AddressLineType[1];
				AddressLineType AddressLine = new AddressLineType();
				LineType Line = new LineType();
				Line.Value = tercero.Direccion;
				AddressLine.Line = Line;

				AddressLines[0] = AddressLine;
				Address.AddressLine = AddressLines;

				//Zona Postal - Obligatorio para emisores y Adquirentes Responsables 
				Address.PostalZone = new PostalZoneType();
				Address.PostalZone.Value = "";//Listado de Zona Postal de Colombia /*** QUEMADO ***/

				//5.4.1. Países (ISO 3166-1): cbc:IdentificationCode 
				//ISO 3166-1 alfa-2: Códigos de país de das letras. Si recomienda como el código de propósito
				//general.Estos códigos se utilizan por ejemplo en internet como dominios geográficos de nivel superior.
				CountryType Country = new CountryType();
				IdentificationCodeType IdentificationCode = new IdentificationCodeType();
				IdentificationCode.Value = tercero.CodigoPais; //Pais (LISTADO DE VALORES DEFINIDO POR LA DIAN 5.4.1)
				Country.IdentificationCode = IdentificationCode;
				Country.Name = new NameType1();
				Country.Name.Value = "Colombia";//Pais (LISTADO DE VALORES DEFINIDO POR LA DIAN 5.4.1) /*** QUEMADO ***/
				Address.Country = Country;

				PhysicalLocation.Address = Address;
				Party.PhysicalLocation = PhysicalLocation;
				#endregion

				#region Regimen --PartyTaxScheme

				PartyTaxSchemeType[] PartyTaxSchemes = new PartyTaxSchemeType[1];
				PartyTaxSchemeType PartyTaxScheme = new PartyTaxSchemeType();

				//---si es NIT debe llenarse con la Razon social y es obligatorio
				PartyTaxScheme.RegistrationName = new RegistrationNameType();
				PartyTaxScheme.RegistrationName.Value = tercero.RazonSocial;

				//Identificacion
				CompanyIDType CompanyID = new CompanyIDType();

				//---Validar si es NIT
				CompanyID.Value = tercero.Identificacion.ToString();
				//---Si es Nit debe star bien calculado
				CompanyID.schemeID = tercero.IdentificacionDv.ToString();
				//----//Tipo documento (LISTADO DE VALORES DEFINIDO POR LA DIAN 5.2.1)
				CompanyID.schemeName = tercero.TipoIdentificacion.ToString();
				CompanyID.schemeAgencyID = "195"; /*** QUEMADO ***/
				CompanyID.schemeAgencyName = "CO, DIAN (Direccion de Impuestos y Aduanas Nacionales)"; /*** QUEMADO ***/
				PartyTaxScheme.CompanyID = CompanyID;

				//Regimen Fiscal y Responsabilidades Tributarias
				//----Validar si cambia la ocurrecia de las responsabilidades esta de 1..1 y deberia estar 1..N
				TaxLevelCodeType TaxLevelCode = new TaxLevelCodeType();
				//-----Listado 5.2.4
				TaxLevelCode.listName = "05"; /*** QUEMADO ***/
											  //---Listado 5.2.7 Responsabilidades pero solo permite 1 actualmente
				TaxLevelCode.Value = tercero.Regimen.ToString();
				PartyTaxScheme.TaxLevelCode = TaxLevelCode;


				//---Tributos Si el adquirente es responsable, el NIT debe estar activo en el RUT
				//--Tabla 5.2.2
				//--Validar si se pueden varias en ejemplo solo presenta 1 y si se repite todo el PartyTaxScheme para los que aplique
				TaxSchemeType TaxScheme = new TaxSchemeType();
				TaxScheme.ID = new IDType();
				TaxScheme.ID.Value = "01"; /*** QUEMADO ***/
				TaxScheme.Name = new NameType1();
				TaxScheme.Name.Value = "IVA"; /*** QUEMADO ***/
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
				PartyLegalEntity.CorporateRegistrationScheme.ID = new IDType();
				PartyLegalEntity.CorporateRegistrationScheme.ID.Value = "";

				//Número de matrícula mercantil (identificador de sucursal: punto de facturación)
				//---Validar
				PartyLegalEntity.CorporateRegistrationScheme.Name = new NameType1();
				PartyLegalEntity.CorporateRegistrationScheme.Name.Value = "12345"; /*** QUEMADO ***/

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
				}
				#endregion

				#region Documento y tipo de documento---Ya esta en PartyTaxScheme no va
				//PartyIdentificationType[] PartyIdentifications = new PartyIdentificationType[1];
				//PartyIdentificationType PartyIdentification = new PartyIdentificationType();
				//IDType ID = new IDType();
				//ID.schemeAgencyName = "CO, DIAN (Direccion de Impuestos y Aduanas Nacionales)";
				//ID.schemeAgencyID = "195";
				//ID.schemeID = tercero.TipoIdentificacion.ToString(); //Tipo documento (LISTADO DE VALORES DEFINIDO POR LA DIAN)
				//ID.Value = tercero.Identificacion.ToString();
				//PartyIdentification.ID = ID;
				//PartyIdentifications[0] = PartyIdentification;
				//Party.PartyIdentification = PartyIdentifications;
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
		private static InvoiceLineType[] ObtenerDetalleDocumento(List<DocumentoDetalle> documentoDetalle, string moneda, bool Autoretenedor, string identificiacion_Obligado, int tipo_operacion, string tipo_factura)
		{
			try
			{
				if (documentoDetalle == null || !documentoDetalle.Any())
					throw new Exception("El detalle del documento es inválido.");

				InvoiceType factura = new InvoiceType();
				InvoiceLineType[] InvoicesLineType1 = new InvoiceLineType[documentoDetalle.Count()];

				int contadorPosicion = 0;
				int contadorProducto = 1;

				foreach (DocumentoDetalle DocDet in documentoDetalle)
				{
					//Crear Enumerable para que lea segun la moneda
					CurrencyCodeContentType moneda_detalle = Ctl_Enumeracion.ObtenerMoneda(moneda);

					// <fe:InvoiceLine>
					// http://www.datypic.com/sc/ubl21/e-cac_InvoiceLine.html
					InvoiceLineType InvoiceLineType1 = new InvoiceLineType();

						#region Id producto definido por la Dian (Contador de productos iniciando desde 1)
						// <cbc:ID>
						IDType ID = new IDType();
						ID.Value = contadorProducto.ToString();
						InvoiceLineType1.ID = ID;
						#endregion


						#region Cantidad producto
						// <cbc:InvoicedQuantity>
						InvoicedQuantityType InvoicedQuantity = new InvoicedQuantityType();

						try
						{
						InvoicedQuantity.Value = DocDet.Cantidad;

						// Unidad de medida Ver lista de valores posibles en 6.3.6(Defecto codigo - 94)
						ListaUnidadesMedida list_unidad = new ListaUnidadesMedida();
						ListaItem unidad = list_unidad.Items.Where(d => d.Codigo.Equals(DocDet.UnidadCodigo)).FirstOrDefault();

						InvoicedQuantity.unitCode = unidad.Codigo;
						InvoiceLineType1.InvoicedQuantity = InvoicedQuantity;
						#endregion


						#region Valor Total
						// <cbc:LineExtensionAmount>
						LineExtensionAmountType LineExtensionAmount = new LineExtensionAmountType();
						LineExtensionAmount.currencyID = moneda_detalle.ToString();
						LineExtensionAmount.Value = DocDet.ValorSubtotal;//decimal.Round(DocDet.ValorSubtotal, 2);
						InvoiceLineType1.LineExtensionAmount = LineExtensionAmount;
						#endregion
					}
					catch (Exception excepcion)
					{

						RegistroLog.EscribirLog(excepcion, MensajeCategoria.Convertir, MensajeTipo.Error, MensajeAccion.creacion, "Inicio");
						throw new ApplicationException(excepcion.Message, excepcion.InnerException);
					}


					#region Cargo adicional -- Hacer cambio para que llene la informacion segun los descuentos o cargos que tenga las lineas
					// <cac:AllowanceCharge>
					try
					{
						if (DocDet.DescuentoValor > 0)
						{
							AllowanceChargeType[] AllowanceCharges = new AllowanceChargeType[1];
							AllowanceChargeType AllowanceCharge = new AllowanceChargeType();
							AllowanceCharge.BaseAmount = new BaseAmountType();
							AllowanceCharge.BaseAmount.currencyID = moneda_detalle.ToString();
							if (DocDet.DescuentoPorcentaje > 0)
							{
								decimal valorTotal = DocDet.Cantidad * DocDet.ValorUnitario;
								AllowanceCharge.BaseAmount.Value = decimal.Round(valorTotal, 6);
							}
							else
							{
								AllowanceCharge.BaseAmount.Value = 0.00M;
							}

							//*******Validar esto si no se maneja decimales
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
							AllowanceCharge.MultiplierFactorNumeric.Value = decimal.Round(DocDet.DescuentoPorcentaje, 2);
							AllowanceCharge.Amount = new AmountType2();
							AllowanceCharge.Amount.currencyID = moneda_detalle.ToString();
							AllowanceCharge.Amount.Value = DocDet.DescuentoValor;//decimal.Round(DocDet.DescuentoValor, 2);
							AllowanceCharges[0] = AllowanceCharge;

							InvoiceLineType1.AllowanceCharge = AllowanceCharges;
						}
					}
					catch (Exception excepcion)
					{

						RegistroLog.EscribirLog(excepcion, MensajeCategoria.Convertir, MensajeTipo.Error, MensajeAccion.creacion, "AllowanceCharge");
						throw new ApplicationException(excepcion.Message, excepcion.InnerException);
					}

					#endregion


					#region Impuestos del producto -- Hacer cambio para que llene la informacion segun los impuestos que tenga las lineas

					// <cac:TaxTotal>
					List<TaxTotalType> TaxesTotal = new List<TaxTotalType>();
					try
					{
						
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
									Value = DocDet.IvaValor//decimal.Round(DocDet.IvaValor, 2)
								};

								// indicador que este total se reconoce como evidencia legal a efectos impositivos (verdadero)o no(falso).
								// <cbc:TaxEvidenceIndicator>
								TaxTotal.TaxEvidenceIndicator = new TaxEvidenceIndicatorType()
								{
									Value = false
								};

								RoundingAmountType Rouding = new RoundingAmountType();
								Rouding.Value = 0.00M;
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
					}
					catch (Exception excepcion)
					{

						RegistroLog.EscribirLog(excepcion, MensajeCategoria.Convertir, MensajeTipo.Error, MensajeAccion.creacion, "TaxTotal");
						throw new ApplicationException(excepcion.Message, excepcion.InnerException);
					}

					#region impuesto: Consumo

					try
					{
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
								Value = DocDet.ValorImpuestoConsumo//decimal.Round(DocDet.ValorImpuestoConsumo, 2)
							};

							// indicador que este total se reconoce como evidencia legal a efectos impositivos (verdadero)o no(falso).
							// <cbc:TaxEvidenceIndicator>
							TaxTotal.TaxEvidenceIndicator = new TaxEvidenceIndicatorType()
							{
								Value = false
							};

							RoundingAmountType Rouding = new RoundingAmountType();
							Rouding.Value = 0.00M;
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

							if (DocDet.ImpoConsumoPorcentaje == 10)
								codigo_impuesto = "35";
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
									unitCode = InvoicedQuantity.unitCode,
									Value = DocDet.Cantidad //1.00M//decimal.Round(DocDet.Cantidad, 6)//
								};

								// tasa de impuesto de la categoría de impuestos aplicada a este subtotal fiscal, expresada como un porcentaje.
								//Tarifa del tributo
								// <cbc:Percent>
								TaxSubtotalConsumo.PerUnitAmount = new PerUnitAmountType()
								{
									currencyID = moneda_detalle.ToString(),
									Value = decimal.Round((DocDet.ValorImpuestoConsumo) / DocDet.Cantidad, 6) + 0.000000M
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
								unitCode = InvoicedQuantity.unitCode,
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

					}
					catch (Exception excepcion)
					{

						RegistroLog.EscribirLog(excepcion, MensajeCategoria.Convertir, MensajeTipo.Error, MensajeAccion.creacion, "ImptoConsumo");
						throw new ApplicationException(excepcion.Message, excepcion.InnerException);
					}

					#endregion

					#region impuesto: Consumo2

					try
					{
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
								Value = DocDet.ValorImpuestoConsumo2//decimal.Round(DocDet.ValorImpuestoConsumo, 2)
							};

							// indicador que este total se reconoce como evidencia legal a efectos impositivos (verdadero)o no(falso).
							// <cbc:TaxEvidenceIndicator>
							TaxTotal.TaxEvidenceIndicator = new TaxEvidenceIndicatorType()
							{
								Value = false
							};

							RoundingAmountType Rouding = new RoundingAmountType();
							Rouding.Value = 0.00M;
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
							if (DocDet.ValorImpuestoConsumo2 > 0 && DocDet.ImpoConsumo2Porcentaje == 0 && DocDet.Aiu == 5)
							{
								/*if (DocDet.Aiu != 4)
								{
									codigo_impuesto = "02";
								}
								else
								{
									codigo_impuesto = "22";
								} */
								codigo_impuesto = "34";

								// importe neto al que se aplica el porcentaje del impuesto (tasa) para calcular el importe del impuesto.
								//Base Imponible sobre la que se calcula el valor del tributo
								//----Se debe Solicitar la base con la que calculo el impuesto
								TaxSubtotalConsumo.BaseUnitMeasure = new BaseUnitMeasureType()
								{
									unitCode = "MLT",//InvoicedQuantity.unitCode,
									Value = decimal.Round(DocDet.Cantidad * DocDet.ProductoContenido, 2) + 0.00M//DocDet.Peso + 0.00M //1.00M//
								};

								// tasa de impuesto de la categoría de impuestos aplicada a este subtotal fiscal, expresada como un porcentaje.
								//Tarifa del tributo
								// <cbc:Percent>
								TaxSubtotalConsumo.PerUnitAmount = new PerUnitAmountType()
								{
									currencyID = moneda_detalle.ToString(),
									Value = decimal.Round((DocDet.ValorImpuestoConsumo2 / DocDet.Cantidad)/(DocDet.ProductoContenido/100), 0) + 0.00M
								};

								if (TaxTotal.TaxAmount.Value != ((TaxSubtotalConsumo.PerUnitAmount.Value * TaxSubtotalConsumo.BaseUnitMeasure.Value)/100))
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
								unitCode = InvoicedQuantity.unitCode,
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
								/*
								if (DocDet.Aiu != 4)
								{
									codigo_impuesto = "02";
								}
								else
								{
									codigo_impuesto = "22";
								} */
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

					}
					catch (Exception excepcion)
					{

						RegistroLog.EscribirLog(excepcion, MensajeCategoria.Convertir, MensajeTipo.Error, MensajeAccion.creacion, "ImptoConsumo");
						throw new ApplicationException(excepcion.Message, excepcion.InnerException);
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

					try
					{
						if (DocDet.ReteFuenteValor > 0 && Autoretenedor == true)
						{

							List<TaxTotalType> TaxesTotalRete = new List<TaxTotalType>();

							//if (decimal.Round((DocDet.ValorSubtotal * (DocDet.ReteFuentePorcentaje / 100)), 0) != DocDet.ReteFuenteValor)
							//	DocDet.ReteFuenteValor = decimal.Round((DocDet.ValorSubtotal * (DocDet.ReteFuentePorcentaje / 100)), 2);

							//Grupo de campos para informaciones relacionadas con un tributo aplicable a esta línea de la factura 
							TaxTotalType TaxTotal = new TaxTotalType();

							// importe total de impuestos, por ejemplo, IVA; la suma de los subtotales fiscales para cada categoría de impuestos dentro del esquema impositivo
							// <cbc:TaxAmount>
							TaxTotal.TaxAmount = new TaxAmountType()
							{
								currencyID = moneda_detalle.ToString(),
								Value = DocDet.ReteFuenteValor//decimal.Round(DocDet.ReteFuenteValor, 2)
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

							TaxSubtotalType TaxSubtotalRete = new TaxSubtotalType();

							// importe neto al que se aplica el porcentaje del impuesto (tasa) para calcular el importe del impuesto.
							//Base Imponible sobre la que se calcula el valor del tributo
							//----Se debe Solicitar la base con la que calculo el impuesto
							// <cbc:TaxAmount>
							TaxSubtotalRete.TaxableAmount = new TaxableAmountType()
							{
								currencyID = moneda_detalle.ToString(),
								Value = DocDet.ValorSubtotal
							};

							// El monto de este subtotal fiscal.
							//Valor del tributo: producto del porcentaje aplicado sobre la base imponible
							// <cbc:TaxAmount>
							TaxSubtotalRete.TaxAmount = new TaxAmountType()
							{
								currencyID = moneda_detalle.ToString(),
								Value = DocDet.ReteFuenteValor
							};

							// categoría de impuestos aplicable a este subtotal.
							//Grupo de informaciones sobre el tributo 
							// <cac:TaxCategory>
							TaxCategoryType TaxCategoryRete = new TaxCategoryType();

							// tasa de impuesto de la categoría de impuestos aplicada a este subtotal fiscal, expresada como un porcentaje.
							//Tarifa del tributo
							// <cbc:Percent>
							TaxCategoryRete.Percent = new PercentType1()
							{
								Value = decimal.Round((DocDet.ReteFuentePorcentaje), 2)
							};

							// <cac:TaxScheme>
							//Grupo de informaciones específicas sobre el tributo
							TaxSchemeType TaxSchemeIva = new TaxSchemeType();
							//Identificador del tributo
							ListaTipoImpuesto list_tipoImp = new ListaTipoImpuesto();


							ListaItem tipoimp = list_tipoImp.Items.Where(d => d.Codigo.Equals("06")).FirstOrDefault();

							if (tipoimp != null)
							{
								TaxSchemeIva.ID = new IDType(); /*** QUEMADO ***/
								TaxSchemeIva.ID.Value = tipoimp.Codigo; //"01";//TipoImpuestos.Iva,
								//Nombre del tributo
								TaxSchemeIva.Name = new NameType1();
								TaxSchemeIva.Name.Value = tipoimp.Nombre; //"IVA";/*** QUEMADO ***/

								//Si es documento soporte el nombre cambia
								if (tipo_operacion.Equals(3))
									TaxSchemeIva.Name.Value = "ReteRenta";

								TaxCategoryRete.TaxScheme = TaxSchemeIva;
							}

							TaxSubtotalRete.TaxCategory = TaxCategoryRete;
							TaxesSubtotal[0] = TaxSubtotalRete;
							TaxTotal.TaxSubtotal = TaxesSubtotal;
							TaxesTotalRete.Add(TaxTotal);

							InvoiceLineType1.WithholdingTaxTotal = TaxesTotalRete.ToArray();
						}
					}
					catch (Exception excepcion)
					{

						RegistroLog.EscribirLog(excepcion, MensajeCategoria.Convertir, MensajeTipo.Error, MensajeAccion.creacion, "ReteFte");
						throw new ApplicationException(excepcion.Message, excepcion.InnerException);
					}

					InvoiceLineType1.TaxTotal = TaxesTotal.ToArray();


					#endregion

					#region Datos producto
					// <fe:Item>

					try
					{
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
						if (tipo_operacion.Equals(3))
						{
							SellersItemIdentification.ExtendedID = new ExtendedIDType();
							SellersItemIdentification.ExtendedID.Value = DocDet.ProductoCodigo;
						}
						Item.SellersItemIdentification = SellersItemIdentification;

						// <cac:StandardItemIdentification>
						ItemIdentificationType StandardItemIdentification = new ItemIdentificationType();
						IDType IDItemStandard = new IDType();
						//---Validar que no venag null
						//IDItemStandard.Value = (string.IsNullOrEmpty(DocDet.ProductoCodigoEAN)) ? string.Empty : DocDet.ProductoCodigoEAN;
						// -- 6.3.5. Productos: @schemeID, @schemeName, @schemeAgencyID

						//Se valida si es Exportacion y que llegue informacion en la posicion arancelaria
						if (tipo_operacion.Equals(2) && !string.IsNullOrEmpty(DocDet.ProductoCodigoPArancelaria))
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
							IDItemStandard.schemeName = "Estándar de adopción del contribuyente";
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
						IDItemOrigen.Value = (string.IsNullOrEmpty(DocDet.Bodega)) ? string.Empty : DocDet.Bodega;//DocDet.Bodega;
						Origen.ID = IDItemOrigen;
						Address[0] = Origen;
						Item.OriginAddress = Address;
						#endregion


						#region Campos Adicionales

						if (DocDet.CamposAdicionales != null && !tipo_factura.Equals("12"))
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
						else if (tipo_factura.Equals("12"))//***Informacion del Sector de transporte
						{
							List<ItemPropertyType> Property = new List<ItemPropertyType>();
							InvoiceLineType1.ID.schemeID = "0";
							foreach (CampoValor Campos in DocDet.CamposAdicionales)
							{
								ItemPropertyType campo = new ItemPropertyType();
								campo.Name = new NameType1();
								campo.Name.Value = Campos.Descripcion;
								campo.Value = new ValueType();
								if (Campos.Descripcion.Equals("03"))
								{
									campo.Value.Value = Math.Floor(Convert.ToDecimal(Campos.Valor)).ToString();//Campos.Valor.Replace(".00","");
									campo.ValueQuantity = new ValueQuantityType();
									campo.ValueQuantity.unitCode = InvoicedQuantity.unitCode;
									campo.ValueQuantity.Value = Math.Floor(InvoicedQuantity.Value);
									InvoiceLineType1.ID.schemeID = "1";
								}
								else
								{
									campo.Value.Value = Campos.Valor;
								}
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
							
							//Se valida y si no estan enviando la propiedad se asigna
							if (string.IsNullOrEmpty(DocDet.TipoIngresoMandato))
							{
								if (!identificiacion_Obligado.Equals(DocDet.DatosMandatario.Identificacion))
									InvoiceLineType1.ID.schemeID = "1";
								else
									InvoiceLineType1.ID.schemeID = "0";
							}
							else
							{
								InvoiceLineType1.ID.schemeID = DocDet.TipoIngresoMandato.ToString();
							}
							
						}

						//Se valida si es una Factura de tipo Mandato para ingresar en el detalle si es ingreso propio
						if (tipo_operacion != 3 && tipo_factura.Equals("11") && string.IsNullOrEmpty(InvoiceLineType1.ID.schemeID) && DocDet.DatosMandatario == null)
						{
							InvoiceLineType1.ID.schemeID = "0";
						}


						#region Producto gratuito 
						// indica que la línea de factura es gratuita (verdadera) o no (falsa)
						// <cbc:FreeOfChargeIndicator>
						FreeOfChargeIndicatorType FreeOfChargeIndicator = new FreeOfChargeIndicatorType();
						FreeOfChargeIndicator.Value = DocDet.ProductoGratis;
						InvoiceLineType1.FreeOfChargeIndicator = FreeOfChargeIndicator;

						if (DocDet.ProductoGratis == true)
						{
							PricingReferenceType Precio = new PricingReferenceType();
							Precio.AlternativeConditionPrice = new PriceType[1];

							// <fe:Price>
							PriceType PriceG = new PriceType();

							// <cbc:PriceAmount>
							PriceAmountType PriceAmountP = new PriceAmountType();
							PriceAmountP.currencyID = moneda_detalle.ToString();
							PriceAmountP.Value = decimal.Round((decimal.Round(DocDet.ValorUnitario, 6) > 0) ? decimal.Round(DocDet.ValorUnitario, 6) : (decimal.Round(DocDet.ValorImpuestoConsumo, 6) / decimal.Round(DocDet.Cantidad, 6)), 6);
							PriceG.PriceAmount = PriceAmountP;

							//Código del tipo de precio informado ista de valores posibles en 6.3.10 
							PriceTypeCodeType PriceTypeCode = new PriceTypeCodeType();
							PriceTypeCode.Value = DocDet.ProductoGratisPrecioRef;
							PriceG.PriceTypeCode = PriceTypeCode;

							Precio.AlternativeConditionPrice[0] = PriceG;
							InvoiceLineType1.PricingReference = Precio;

							if (InvoiceLineType1.LineExtensionAmount.Value > 0)
								InvoiceLineType1.LineExtensionAmount.Value = 0.00M;

						}

						#endregion

						if (!string.IsNullOrEmpty(DocDet.ProductoDescripcion))
						{
							NoteType nota = new NoteType();
							nota.Value = DocDet.ProductoDescripcion;
							InvoiceLineType1.Note = new NoteType[1];
							InvoiceLineType1.Note[0] = nota;
						}

						InvoiceLineType1.Item = Item;
					}
					catch (Exception excepcion)
					{

						RegistroLog.EscribirLog(excepcion, MensajeCategoria.Convertir, MensajeTipo.Error, MensajeAccion.creacion, "Item");
						throw new ApplicationException(excepcion.Message, excepcion.InnerException);
					}
					#endregion

					#region Valor Unitario producto
					// <fe:Price>

					try
					{
						PriceType Price = new PriceType();

						// <cbc:PriceAmount>

						PriceAmountType PriceAmount = new PriceAmountType();
						PriceAmount.currencyID = moneda_detalle.ToString();
						if (DocDet.ProductoGratis == false)
						{
							PriceAmount.Value = DocDet.ValorUnitario; // decimal.Round(DocDet.ValorUnitario, 2);
						}
						else
						{
							PriceAmount.Value = decimal.Round(0, 2);
						}
						Price.PriceAmount = PriceAmount;
						//---Segun la base de la unidad utilizada
						BaseQuantityType BaseQuantity = new BaseQuantityType();
						BaseQuantity.unitCode = InvoicedQuantity.unitCode;
						BaseQuantity.Value = DocDet.Cantidad; //decimal.Round(DocDet.Cantidad, 8);
						Price.BaseQuantity = BaseQuantity;


						InvoiceLineType1.Price = Price;
					}
					catch (Exception excepcion)
					{

						RegistroLog.EscribirLog(excepcion, MensajeCategoria.Convertir, MensajeTipo.Error, MensajeAccion.creacion, "Price");
						throw new ApplicationException(excepcion.Message, excepcion.InnerException);
					}
					#endregion


					//Documento Equivalente indicando si es una compra semanal o por operacion
					if (tipo_operacion == 3)
					{
						InvoiceLineType1.InvoicePeriod = new PeriodType[1];
						InvoiceLineType1.InvoicePeriod[0] = new PeriodType();
						InvoiceLineType1.InvoicePeriod[0].StartDate = new StartDateType();
						InvoiceLineType1.InvoicePeriod[0].StartDate.Value = Fecha.GetFecha();

						// 1 - Por operación, 2 - Acumulado semanal
						InvoiceLineType1.InvoicePeriod[0].DescriptionCode = new DescriptionCodeType[1];
						InvoiceLineType1.InvoicePeriod[0].DescriptionCode[0] = new DescriptionCodeType();
						InvoiceLineType1.InvoicePeriod[0].DescriptionCode[0].Value = "1";

						InvoiceLineType1.InvoicePeriod[0].Description = new DescriptionType[1];
						InvoiceLineType1.InvoicePeriod[0].Description[0] = new DescriptionType();
						InvoiceLineType1.InvoicePeriod[0].Description[0].Value = "Por operación";

					}

					InvoicesLineType1[contadorPosicion] = InvoiceLineType1;
					contadorProducto++;
					contadorPosicion++;
				}

				factura.InvoiceLine = InvoicesLineType1;
				return factura.InvoiceLine;
			}
			catch (Exception excepcion)
			{
				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Auditoria, MensajeTipo.Error, MensajeAccion.creacion);
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}

		}

		/// <summary>
		/// Llena el objeto de las cuotas de la factura con los datos
		/// </summary>
		/// <param name="cuota">Datos de las cuotas</param>
		/// <returns>Objeto tipo PaymentMeansType</returns>
		private static List<PaymentTermsType> ObtenerCuotas(List<Cuota> cuota, int medio_pago, string moneda)
		{

			List<PaymentTermsType> PaymentTerms = new List<PaymentTermsType>();

			foreach (Cuota item in cuota)
			{

				PaymentTermsType TermsType = new PaymentTermsType();

				IDType ID = new IDType();
				ID.Value = item.Codigo.ToString();
				TermsType.ID = ID;

				//PaymentMeansCodeType1 MeansCode = new PaymentMeansCodeType1();
				PaymentMeansIDType MeansCode = new PaymentMeansIDType();
				MeansCode.Value = medio_pago.ToString();
				//MeansCode.Value = "24";
				//MeansCode.schemeName = Ctl_Enumeracion.ObtenerMedioPago(medio_pago);
				TermsType.PaymentMeansID = new PaymentMeansIDType[1];
				TermsType.PaymentMeansID[0] = MeansCode;

				List<NoteType> Instructions = new List<NoteType>();
				NoteType Instruction = new NoteType();
				Instruction.Value = string.Format("Cuota {0} de {1}", item.Codigo, cuota.Count());
				//Instruction.Value = item.Valor.ToString();
				Instructions.Add(Instruction);
				TermsType.Note = Instructions.ToArray();

				//Terminos de pago
				//TermsType.SettlementPeriod = new PeriodType();
				//TermsType.SettlementPeriod.EndDate = new EndDateType();
				//TermsType.SettlementPeriod.EndDate.Value = Convert.ToDateTime(item.FechaVence.ToString(Fecha.formato_fecha_hginet));
				TermsType.Amount = new AmountType2();
				TermsType.Amount.Value = item.Valor;
				TermsType.Amount.currencyID = moneda;

				PaymentDueDateType PDueDate = new PaymentDueDateType();
				PDueDate.Value = Convert.ToDateTime(item.FechaVence.ToString(Fecha.formato_fecha_hginet));
				TermsType.PaymentDueDate = PDueDate;

				PaymentTerms.Add(TermsType);

			}


			return PaymentTerms;
		}

		/*
		/// <summary>
		/// Obtiene los valores del encabezado del documento
		/// </summary>
		/// <param name="documento">datos del documento</param>
		/// <returns>MonetaryTotalType1</returns>
		private static MonetaryTotalType ObtenerTotales(Factura documento)
		{
			try
			{

				// moneda del documento
				CurrencyCodeContentType moneda_documento =
					Ctl_Enumeracion.ObtenerMoneda(documento.Moneda);

				MonetaryTotalType LegalMonetaryTotal = new MonetaryTotalType();

				#region Total Importe bruto antes de impuestos

				// cbc:LineExtensionAmount [0..1]    The total of Line Extension Amounts net of tax and settlement discounts, but inclusive of any applicable rounding amount.
				//	Total importe bruto, suma de los importes brutos de las líneas de la factura - los productos que son regalos.
				LineExtensionAmountType LineExtensionAmount = new LineExtensionAmountType();
				LineExtensionAmount.currencyID = moneda_documento.ToString();
				LineExtensionAmount.Value = decimal.Round(documento.ValorSubtotal, 2);
				LegalMonetaryTotal.LineExtensionAmount = LineExtensionAmount;

				#endregion

				#region Valor total base imponible (generó impuestos)

				//Total Base Imponible (Importe Bruto+Cargos-Descuentos): Base imponible para el cálculo de los impuestos.
				//Subtotal de la factura incluyendo los productos que son regalo.
				TaxExclusiveAmountType TaxExclusiveAmount = new TaxExclusiveAmountType();
				TaxExclusiveAmount.currencyID = moneda_documento.ToString();
				TaxExclusiveAmount.Value = decimal.Round(documento.ValorSubtotal, 2);
				LegalMonetaryTotal.TaxExclusiveAmount = TaxExclusiveAmount;

				#endregion

				//Total de Valor bruto con tributos Los tributos retenidos son retirados en el cálculo de PayableAmount 
				//Subtotal de la factura + Iva
				#region Valor total base no imponible (no generó impuestos)
				TaxInclusiveAmountType TaxInclusiveAmount = new TaxInclusiveAmountType();
				TaxInclusiveAmount.currencyID = moneda_documento.ToString();
				TaxInclusiveAmount.Value = decimal.Round(documento.ValorSubtotal, 2) + decimal.Round(documento.ValorIva, 2);
				LegalMonetaryTotal.TaxInclusiveAmount = TaxInclusiveAmount;
				#endregion

				#region Descuentos

				// Descuentos: Suma de todos los descuentos aplicados al total de la factura
				//-------Los descuentos del Detalle se registran por detalle, este se llena cuando se aplican al total de la Factura
				AllowanceTotalAmountType AllowanceTotalAmount = new AllowanceTotalAmountType();
				AllowanceTotalAmount.currencyID = moneda_documento.ToString();
				AllowanceTotalAmount.Value = decimal.Round(documento.ValorDescuento, 2);
				LegalMonetaryTotal.AllowanceTotalAmount = AllowanceTotalAmount;

				#endregion

				//Cargos: Suma de todos los cargos aplicados al total de la factura
				//-----Se utiliza para sumarle al subtotal, ejemplo costo de financiamiento
				ChargeTotalAmountType ChargeTotalAmount = new ChargeTotalAmountType();
				ChargeTotalAmount.currencyID = moneda_documento.ToString();
				ChargeTotalAmount.Value = decimal.Round(documento.ValorDescuento, 2);
				LegalMonetaryTotal.ChargeTotalAmount = ChargeTotalAmount;

				#region Valor total de pago //  Total de Factura =  Valor total bases - Valor descuentos + Valor total Impuestos - Valor total impuestos retenidos

				PayableAmountType PayableAmount = new PayableAmountType();
				PayableAmount.currencyID = moneda_documento.ToString();
				PayableAmount.Value = decimal.Round(documento.Total, 2);
				LegalMonetaryTotal.PayableAmount = PayableAmount;

				#endregion

				return LegalMonetaryTotal;

			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}*/

		/// <summary>
		/// Obtiene el Medio de Pago del documento
		/// http://www.unece.org/trade/untdid/d16a/tred/tred4461.htm
		/// </summary>
		/// <param name="codigo"></param>
		/// <returns>Medio de pago</returns>
		public static string ObtenerMedioPago(int codigo)
		{
			try
			{
				int medio = Enumeracion.ParseToEnum<Meanscode>(codigo).GetHashCode();
				if (medio == 0)
					codigo = 24;

				string descripcion = Enumeracion.GetDescription((Meanscode)codigo);

				return descripcion;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(string.Format("Error al obtener la enumeración {0} con código {1}. Detalle: {2}", "Meanscode", codigo, excepcion.Message));
			}
		}
	}



}

