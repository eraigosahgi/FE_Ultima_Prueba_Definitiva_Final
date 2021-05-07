using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetUBLv2_1.DianListas;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace HGInetUBLv2_1
{
	public partial class AcuseReciboXMLv2_1
	{

		public static FacturaE_Documento CrearDocumento(Acuse documento, TblEmpresas proveedor_receptor, TblEmpresas proveedor_emisor, string ambiente, string pin_sw, string cufe_docreferenciado, HGInetMiFacturaElectonicaData.ModeloServicio.ExtensionDian resolucion, TblDocumentos documento_factura, CodigoResponseV2 tipo_acuse)
        {

            try
            {
                if (documento == null)
                    throw new Exception("La documento es inválido.");

				//Obtiene el nombre del archivo XML
				string nombre_archivo_xml = NombramientoArchivo.ObtenerXml(documento.Documento.ToString(), documento.DatosObligado.Identificacion.ToString(), TipoDocumento.AcuseRecibo, documento.Prefijo);

                if (string.IsNullOrWhiteSpace(nombre_archivo_xml))
                    throw new ApplicationException("El nombre del archivo es inválido.");

                nombre_archivo_xml = string.Format("{0}-{1}", nombre_archivo_xml, documento.IdAcuse);

				//CodigoResponseV2 tipo_acuse = Enumeracion.GetValueFromAmbiente<HGInetMiFacturaElectonicaData.CodigoResponseV2>(documento.CodigoRespuesta);

				// Información del receptor del evento cuando debe ser la DIAN
				Tercero tercero_dian = new Tercero();

				//cantidad de respuesta que se va a manejar en el applicattion response(depende del tipo de Mandato para el resto es un solo documento)
				int cant_response = 1;

				ApplicationResponseType acuse = new ApplicationResponseType();

                XmlSerializerNamespaces namespaces_xml = NamespacesXML.ObtenerNamespaces(TipoDocumento.AcuseRecibo);

                #region acuse.UBLVersionID - Versión UBL
                /*La versión del esquema UBL utilizada por la Dian */
                UBLVersionIDType UBLVersionID = new UBLVersionIDType();
                UBLVersionID.Value = Recursos.VersionesDIANv2_1.UBLVersionID;
                acuse.UBLVersionID = UBLVersionID;
				#endregion

				CustomizationIDType Customization = new CustomizationIDType();
				if (tipo_acuse.GetHashCode() < CodigoResponseV2.Inscripcion.GetHashCode())
				{
					Customization.Value = "1";
				}
				else
				{
					tercero_dian.Identificacion = "800197268";
					tercero_dian.IdentificacionDv = 4;
					tercero_dian.TipoIdentificacion = 31;
					tercero_dian.RazonSocial = "Dirección de Impuestos y Aduanas Nacionales";
					tercero_dian.CodigoTributo = "01";

					switch (tipo_acuse)
					{
						case CodigoResponseV2.Inscripcion:
							Customization.Value = "361";
							break;
						case CodigoResponseV2.EndosoPp:
							Customization.Value = "372";
							break;
						case CodigoResponseV2.EndosoG:
							Customization.Value = documento.CodigoRespuesta;
							break;
						case CodigoResponseV2.CancelacionEG:
							Customization.Value = "401";
							break;
						case CodigoResponseV2.EndosoPc:
							Customization.Value = documento.CodigoRespuesta;
							break;
						case CodigoResponseV2.MandatoG:
							Customization.Value = "433";
							//****Si el atributo @schemeID del elemento CustomizationID es igual a 2 se debe informar el DocumentResponse/cac:LineResponse y embeber el documento de “Evidencia del Contrato de Mandato”
							Customization.schemeID = "1";
							cant_response = 2;
							break;
						default:
							break;
					}

				}
				
				acuse.CustomizationID = Customization;

				#region acuse.ProfileID - Versión del documento DIAN_UBL.xsd
				/*Especifica la versión de las personalizaciones usadas Versión del documento DIAN_UBL.xsd publicado por la DIAN*/
				ProfileIDType ProfileID = new ProfileIDType();
				ProfileID.Value = Recursos.VersionesDIANv2_1.ProfileIDAR;//"DIAN 2.1: ApplicationResponse de la Factura Electrónica de Venta";//
                acuse.ProfileID = ProfileID;
                #endregion

				ProfileExecutionIDType ProfileExecution = new ProfileExecutionIDType();
				ProfileExecution.Value = ambiente;
				acuse.ProfileExecutionID = ProfileExecution;

				#region acuse.ID - identificador del acuse
				/*Número de documento: Número de Acuse generado .*/
				IDType ID = new IDType();
                ID.Value = documento.IdAcuse;
                acuse.ID = ID;
                #endregion

                #region acuse.IssueDate - Fecha de la factura
                /*Fecha de emision de la factura*/
                IssueDateType IssueDate = new IssueDateType();
                IssueDate.Value = Convert.ToDateTime(documento.Fecha.ToString(Fecha.formato_fecha_hginet));
                acuse.IssueDate = IssueDate;
                #endregion

                #region acuse.IssueTime - Hora de la factura
                /*Hora de emision de la factura*/
                IssueTimeType IssueTime = new IssueTimeType();
                IssueTime.Value = documento.Fecha.AddHours(5).ToString(Fecha.formato_hora_zona);
                acuse.IssueTime = IssueTime;
                #endregion

                #region acuse.Note - Motivo de respuesta

                if (!string.IsNullOrEmpty(documento.MvoRespuesta))
                {
	                List<NoteType> notas = new List<NoteType>();
	                NoteType nota = new NoteType();
	                //nota.Value = string.Format("Tracking ID {0}", documento.IdSeguridad.ToString());
	                nota.Value = documento.MvoRespuesta;
	                notas.Add(nota);
	                acuse.Note = notas.ToArray();
                }

                #endregion

				if (tipo_acuse.Equals(CodigoResponseV2.AprobadoTacito) || tipo_acuse.GetHashCode() > CodigoResponseV2.Expresa.GetHashCode() && !tipo_acuse.Equals(CodigoResponseV2.CancelacionEG))
				{
					List<NoteType> notas = new List<NoteType>();
					NoteType nota = new NoteType();
					decimal participacion_endoso = 0;
					bool receptor = false;
					bool mandato = false;

					switch (tipo_acuse)
					{
						case CodigoResponseV2.AprobadoTacito:
							nota.Value = string.Format("Manifiesto bajo la gravedad de juramento que transcurridos 3 días hábiles siguientes a la fecha de recepción de la mercancía o del servicio en la referida factura de este evento, el adquirente {0} identificado con NIT {1} no manifestó expresamente la aceptación o rechazo de la referida factura, ni reclamó en contra de su contenido.", documento.DatosAdquiriente.RazonSocial, documento.DatosAdquiriente.Identificacion);
							break;
						case CodigoResponseV2.Inscripcion:
							nota.Value = string.Format("HGI SAS \"OBRANDO EN NOMBRE Y REPRESENTACION DE\" {0}", documento.DatosObligado.RazonSocial);
							break;
						case CodigoResponseV2.EndosoPp:
							nota.Value = string.Format("HGI SAS \"OBRANDO EN NOMBRE Y REPRESENTACION DE\" {0}", documento.DatosObligado.RazonSocial);
							notas.Add(nota);
							nota = new NoteType();
							nota.Value = "sin mi responsabilidad u otra equivalente";
							participacion_endoso = documento_factura.IntVlrTotal;
							tercero_dian = documento.DatosObligado;
							break;
						case CodigoResponseV2.EndosoG:
							nota.Value = string.Format("HGI SAS \"OBRANDO EN NOMBRE Y REPRESENTACION DE\" {0}", documento.DatosObligado.RazonSocial);
							notas.Add(nota);
							nota = new NoteType();
							nota.Value = "\"en garantía\", \"en prenda\" u otra equivalente";
							participacion_endoso = documento_factura.IntVlrTotal;
							tercero_dian = documento.DatosObligado;
							break;
						case CodigoResponseV2.EndosoPc:
							nota.Value = string.Format("HGI SAS \"OBRANDO EN NOMBRE Y REPRESENTACION DE\" {0}", documento.DatosObligado.RazonSocial);
							notas.Add(nota);
							nota = new NoteType();
							nota.Value = "\"en procuración\", \"al cobro\" u otra equivalente";
							participacion_endoso = documento_factura.IntVlrTotal;
							tercero_dian = documento.DatosObligado;
							break;
						case CodigoResponseV2.MandatoG:
							nota.Value = string.Format("HGI SAS \"OBRANDO EN NOMBRE Y REPRESENTACION DE\" {0}", documento.DatosObligado.RazonSocial);
							notas.Add(nota);
							nota = new NoteType();
							//"XXXX, identificado con la cédula de ciudadanía (o el documento de identificación que corresponda) No. XXXX, expresamente manifiesto que obro en nombre y representación de YYYY,
							//de conformidad con el contrato de mandato verbal/escrito existente entre las partes y con las facultades señaladas en el presente documento y por el tiempo consignado en este/sin limitaciones de tiempo.";
							nota.Value = string.Format("{0}, identificado con {1} No. {2}, expresamente manifiesto que obro en nombre y representación de {3}, de conformidad con el contrato de mandato verbal existente entre las partes y con las facultades señaladas en el presente documento y por el tiempo consignado en este", "Jorge Bedoya", "Cédula de Ciudadanía", "8005097", documento.DatosAdquiriente.RazonSocial);
							receptor = true;
							mandato = true;
							break;
						default:
							break;
					}
					
					notas.Add(nota);
					acuse.Note = notas.ToArray();

					// Información del emisor del evento
					acuse.SenderParty = ObtenerTercero(documento.DatosAdquiriente, proveedor_receptor, receptor, participacion_endoso, mandato);	
					acuse.ReceiverParty = ObtenerTercero(tercero_dian, proveedor_receptor, true, participacion_endoso);
				}
				else
				{
					#region acuse.SenderParty - Información del emisor del evento
					acuse.SenderParty = ObtenerTercero(documento.DatosAdquiriente, proveedor_receptor, false);
					#endregion

					#region acuse.ReceiverParty - Información del receptor del evento
					acuse.ReceiverParty = ObtenerTercero(documento.DatosObligado, proveedor_receptor, true);
					#endregion
				}

				#region Document response -  Document reference 
				acuse.DocumentResponse = new DocumentResponseType[cant_response];
                DocumentResponseType DocumentResponse = new DocumentResponseType();
                ResponseType response = new ResponseType();

				if (!tipo_acuse.Equals(CodigoResponseV2.CancelacionEG) && !tipo_acuse.Equals(CodigoResponseV2.MandatoG))
				{
					response.ReferenceID = new ReferenceIDType();
					response.ReferenceID.Value = string.Format("{0}{1}", documento.Prefijo, documento.Documento.ToString());
				}
				
                response.ResponseCode = new ResponseCodeType();
                response.ResponseCode.Value = documento.CodigoRespuesta;
				DescriptionType[] Description = new DescriptionType[1];
                DescriptionType description = new DescriptionType();
                description.Value = Enumeracion.GetDescription(Enumeracion.GetValueFromAmbiente<HGInetMiFacturaElectonicaData.CodigoResponseV2>(documento.CodigoRespuesta));

				if (tipo_acuse.Equals(CodigoResponseV2.Rechazado))
				{
					//****Son varias opciones para indicar por que hay reclamo de la factura
					response.ResponseCode.listID = "03";
					response.ResponseCode.name = "Mercancía no entregada parcialmente";
				}
				else if (tipo_acuse.Equals(CodigoResponseV2.EndosoPp) || tipo_acuse.Equals(CodigoResponseV2.EndosoG) || tipo_acuse.Equals(CodigoResponseV2.EndosoPc))
				{
					//****Son dos opciones para indicar el endos de la factura 1 - completo y 2 - en blanco 
					response.ResponseCode.listID = "1";
					//if (tipo_acuse.Equals(CodigoResponseV2.EndosoPc))
					//{
					//	response.ResponseCode.name = "ClaseEndoso";
					//}

				}
				else if (tipo_acuse.Equals(CodigoResponseV2.CancelacionEG))
				{
					//Se debe obtener el evento de endoso en garantia que se va a cancelar
					documento.Documento = 9900012398;
					documento.Prefijo = string.Empty;
					cufe_docreferenciado = "ac00bf950ff9b4cff9078fa845a6a090ec31494d754cc2ea397b531ae721cb3eafdc74220f595dbfe8909ec9c10057c1";
					documento.TipoDocumento = "96";
				}
				else if (tipo_acuse.Equals(CodigoResponseV2.MandatoG))
				{
					//****Referencia a documentos electronicos
					// 1 - Un documento electrónico	- Corresponde a la referencia de un documento electrónico tipo invoice "01" donde se referencia el numero de factural, el CUFE y el tipo de documento
					// 2 - Maximo 20 documentos electrónicos - Corresponde a la referencia de varios documento electrónico tipo invoice "01" donde se referencia el numero de factural, el CUFE y el tipo de documento
					// 3 - Todos los documentos de tipo invoice - Corresponde a la referencia de todos los documentos electrónicos tipo invoice "01" donde no se debe referencia el numero de factural, el CUFE y el tipo de documento
					response.ResponseCode.listID = "3";

					//Fecha desde cuando puede actuar el Mandatario
					response.EffectiveDate = new EffectiveDateType();
					response.EffectiveDate.Value = Fecha.GetFecha();
				}

				Description[0] = description;
                response.Description = Description;
                DocumentResponse.Response = response;
                acuse.DocumentResponse[0] = DocumentResponse;

                acuse.DocumentResponse[0].DocumentReference = new DocumentReferenceType[1];
				DocumentReferenceType DocumentReference = new DocumentReferenceType();
                DocumentReference.ID = new IDType();
                DocumentReference.ID.Value = string.Format("{0}{1}", documento.Prefijo, documento.Documento.ToString());
				DocumentReference.UUID = new UUIDType();
                DocumentReference.UUID.Value = cufe_docreferenciado;
                if (documento.TipoDocumento.Equals("91") || documento.TipoDocumento.Equals("92") || documento.TipoDocumento.Equals("96"))
                {
	                DocumentReference.UUID.schemeName = Recursos.VersionesDIANv2_1.CUDE;
				}
                else
                {
					DocumentReference.UUID.schemeName = Recursos.VersionesDIANv2_1.CUFE;
				}
				DocumentReference.DocumentTypeCode = new DocumentTypeCodeType();
                DocumentReference.DocumentTypeCode.Value = documento.TipoDocumento;

				if (tipo_acuse.GetHashCode() > CodigoResponseV2.Expresa.GetHashCode() && !tipo_acuse.Equals(CodigoResponseV2.CancelacionEG) && !tipo_acuse.Equals(CodigoResponseV2.MandatoG))
				{
					DocumentReference.ValidityPeriod = new PeriodType();
					DocumentReference.ValidityPeriod.EndDate = new EndDateType();
					DocumentReference.ValidityPeriod.EndDate.Value = documento_factura.DatFechaVencDocumento;
				}
				
				if (tipo_acuse.Equals(CodigoResponseV2.MandatoG))
				{
					//Se debe poner el documento 1 como constante por el tipo de mandato que se esta enviando
					documento.Documento = 1;
					documento.Prefijo = string.Empty;

					DocumentReference = new DocumentReferenceType();
					DocumentReference.ID = new IDType();
					DocumentReference.ID.Value = "1";

					//Grupo de información para informar el tiempo del mandato
					DocumentReference.ValidityPeriod = new PeriodType();
					//Obligatorio para los tipos de operación “431”, “433”.	No se requiere informar cuando el mandato es ilimitado 
					DocumentReference.ValidityPeriod.StartDate = new StartDateType();
					DocumentReference.ValidityPeriod.StartDate.Value = Fecha.GetFecha();
					DocumentReference.ValidityPeriod.EndDate = new EndDateType();
					DocumentReference.ValidityPeriod.EndDate.Value = Fecha.GetFecha().AddMonths(6);

					//Tiempo del mandato - Obligatorio para todos los tipos de operación
					DocumentReference.ValidityPeriod.DescriptionCode = new DescriptionCodeType[1];
					DocumentReference.ValidityPeriod.DescriptionCode[0] = new DescriptionCodeType();
					DocumentReference.ValidityPeriod.DescriptionCode[0].Value = "1";

					//La descripción puede correspoder a dos escenarios, cuando el tiempo del mandato es por un limite de tiempo o cuando este es ilimitado.
					//-Debe contener el literal “Lapso de vigencia del Mandato-Limitado” cunado el mandato sea por tiempo Limitados
					//-Debe contener el literal “Lapso de vigencia del Mandato-Ilimitado” cunado el mandato sea por tiempo Ilimitado
					DocumentReference.ValidityPeriod.Description = new DescriptionType[1];
					DocumentReference.ValidityPeriod.Description[0] = new DescriptionType();
					DocumentReference.ValidityPeriod.Description[0].Value = "Lapso de vigencia del Mandato-Limitado";

					//Grupo de informacion del Mandatario
					PowerOfAttorneyType mandante = new PowerOfAttorneyType();
					mandante.ID = new IDType();
					mandante.ID.schemeID = "4";
					mandante.ID.schemeName = "31";
					mandante.ID.Value = "811021438";

					//Debe corresponder a un valor de la columna “descripción” del numeral 13.2.8. Tipo de Mandatario
					mandante.Description = new DescriptionType[1];
					mandante.Description[0] = new DescriptionType();
					mandante.Description[0].Value = "Mandatario Proveedor Tecnológico";

					//Debe corresponde a un “código” del numeral 13.2.8. Tipo de Mandatario
					mandante.AgentParty = new PartyType();
					mandante.AgentParty.PartyIdentification = new PartyIdentificationType[1];
					mandante.AgentParty.PartyIdentification[0] = new PartyIdentificationType();
					mandante.AgentParty.PartyIdentification[0].ID = new IDType();
					mandante.AgentParty.PartyIdentification[0].ID.Value = "M-PT";

					PersonType person = new PersonType();
					person.ID = new IDType();
					person.ID.schemeID = "";
					person.ID.schemeName = "13";
					person.ID.Value = "8005097";
					person.FirstName = new FirstNameType();
					person.FirstName.Value = "Jorge";
					person.FamilyName = new FamilyNameType();
					person.FamilyName.Value = "Bedoya";
					person.JobTitle = new JobTitleType();
					person.JobTitle.Value = "Representante Legal Principal";
					//En el Anexo del Radian las siguientes propiedades las ponen como opcionales.
					person.NationalityID = new NationalityIDType();
					person.NationalityID.Value = "Colombiana";
					person.OrganizationDepartment = new OrganizationDepartmentType();
					person.OrganizationDepartment.Value = "Administrativo";

					mandante.AgentParty.Person = new PersonType[1];
					mandante.AgentParty.Person[0] = person;

					acuse.DocumentResponse[0].IssuerParty = new PartyType();
					acuse.DocumentResponse[0].IssuerParty.PowerOfAttorney = new PowerOfAttorneyType[1];
					acuse.DocumentResponse[0].IssuerParty.PowerOfAttorney[0] = mandante;

					DocumentResponseType DocumentResponse1 = new DocumentResponseType();
					response = new ResponseType();

					response.ResponseCode = new ResponseCodeType();
					response.ResponseCode.Value = "ALL17-PT";
					Description = new DescriptionType[1];
					description = new DescriptionType();
					description.Value = "Mandato por documento General";
					Description[0] = description;
					response.Description = Description;
					DocumentResponse1.Response = response;

					DocumentReferenceType DocumentReference1 = new DocumentReferenceType();
					DocumentReference1.ID = new IDType();
					DocumentReference1.ID.Value = "1";
					DocumentResponse1.DocumentReference = new DocumentReferenceType[1];
					DocumentResponse1.DocumentReference[0] = DocumentReference1;

					LineResponseType lineresponse = new LineResponseType();
					lineresponse.LineReference = new LineReferenceType();
					lineresponse.LineReference.LineID = new LineIDType();
					lineresponse.LineReference.LineID.Value = "1";

					lineresponse.Response = new ResponseType[1];
					lineresponse.Response[0] = new ResponseType();
					lineresponse.Response[0].ResponseCode = new ResponseCodeType();
					lineresponse.Response[0].ResponseCode.Value = "ACEPTA";
					lineresponse.Response[0].Description = new DescriptionType[1];
					lineresponse.Response[0].Description[0] = new DescriptionType();
					lineresponse.Response[0].Description[0].Value = "Aceptación del Mandato por parte del Mandatario";
					DocumentResponse1.LineResponse = new LineResponseType[1];
					DocumentResponse1.LineResponse[0] = lineresponse;


					acuse.DocumentResponse[1] = DocumentResponse1;

				}

				if (tipo_acuse.Equals(CodigoResponseV2.EndosoPp) || tipo_acuse.Equals(CodigoResponseV2.EndosoG) || tipo_acuse.Equals(CodigoResponseV2.EndosoPc))
				{
					ApplicationResponseType acuse_temp = new ApplicationResponseType();
					acuse_temp.ReceiverParty = ObtenerTercero(documento.DatosAdquiriente, proveedor_receptor, false);
					DocumentReference.IssuerParty = new PartyType();
					DocumentReference.IssuerParty.PartyTaxScheme = new PartyTaxSchemeType[1];
					DocumentReference.IssuerParty.PartyTaxScheme = acuse_temp.ReceiverParty.PartyTaxScheme;
				}


				acuse.DocumentResponse[0].DocumentReference[0] = DocumentReference;


				#region Persona que recibe el documento y realiza el Acuse Anexo Tecnico Ver. 1.8
				
				//Se requiere agregar al acuse de recibo informar la persona que lo hace con estas propiedades.
				if (acuse.CustomizationID.Value.Equals("1") || acuse.CustomizationID.Value.Equals("362") || acuse.CustomizationID.Value.Equals("364"))
				{
					acuse.DocumentResponse[0].IssuerParty = new PartyType();
					acuse.DocumentResponse[0].IssuerParty.Person = new PersonType[1];
					PersonType persona = new PersonType();
					persona.ID = new IDType();
					persona.ID.Value = "1040731459";
					persona.ID.schemeName = "13";
					persona.FirstName = new FirstNameType();
					persona.FirstName.Value = "Jhon Stivens";
					persona.FamilyName = new FamilyNameType();
					persona.FamilyName.Value = "Zea Velasquez";
					//--Opcional el cargo
					persona.JobTitle = new JobTitleType();
					persona.JobTitle.Value = "Auxiliar de Cartera";
					persona.OrganizationDepartment = new OrganizationDepartmentType();
					persona.OrganizationDepartment.Value = "Cartera";
					acuse.DocumentResponse[0].IssuerParty.Person[0] = persona;
				}


				#endregion

				#endregion

				UUIDType UUID = new UUIDType();
				string CUFE = string.Empty;
				if (tipo_acuse.Equals(CodigoResponseV2.AprobadoTacito) || tipo_acuse.GetHashCode() > CodigoResponseV2.Expresa.GetHashCode())
				{
					//-----Se debe agregar validacion cuando es endoso la propiedad response.ResponseCode.listID si es 2 el calculo tiene un cambio
					string code_response = string.Empty;
					if (response.ResponseCode.listID != null && response.ResponseCode.listID.Equals("2"))
					{
						code_response = response.ResponseCode.listID;
					}
					CUFE = CufeApplicationV2(pin_sw, acuse.ID.Value, string.Format("{0}{1}", documento.Fecha.ToString(Fecha.formato_fecha_hginet), documento.Fecha.AddHours(5).ToString(Fecha.formato_hora_zona)), documento.DatosObligado.Identificacion, acuse.ReceiverParty.PartyTaxScheme[0].CompanyID.Value, documento.CodigoRespuesta, string.Format("{0}{1}", documento.Prefijo, documento.Documento.ToString()), documento.TipoDocumento, code_response);
				}
				else
				{ 
					CUFE = CufeApplicationV2(pin_sw, acuse.ID.Value, string.Format("{0}{1}", documento.Fecha.ToString(Fecha.formato_fecha_hginet), documento.Fecha.AddHours(5).ToString(Fecha.formato_hora_zona)), documento.DatosAdquiriente.Identificacion, documento.DatosObligado.Identificacion, documento.CodigoRespuesta, string.Format("{0}{1}", documento.Prefijo, documento.Documento.ToString()), documento.TipoDocumento);
				}
				UUID.Value = CUFE;
				UUID.schemeName = Recursos.VersionesDIANv2_1.CUDE;
				UUID.schemeID = acuse.ProfileExecutionID.Value; //"2";
				acuse.UUID = UUID;

				XmlDocument doc = new XmlDocument();
				doc.LoadXml("<firma></firma>");

				List<UBLExtensionType> UBLExtensions = new List<UBLExtensionType>();

				//Informacion del QR
				string ruta_qr_Dian = string.Format("{0}{1}", "https://catalogo-vpfe.dian.gov.co/document/searchqr?documentkey=", cufe_docreferenciado);//ruta_qr_Dian = string.Empty;
				
				//if (acuse.ProfileExecutionID.Value.Equals("2"))
				//{
				//	ruta_qr_Dian = string.Format("{0}{1}", "https://catalogo-vpfe-hab.dian.gov.co/document/searchqr?documentkey=", acuse.UUID.Value);
				//}
				//else
				//{
				//	ruta_qr_Dian = string.Format("{0}{1}", "https://catalogo-vpfe.dian.gov.co/document/searchqr?documentkey=", cufe_docreferenciado);
				//}


				// Extension de la Dian
				UBLExtensionType UBLExtensionDian = new UBLExtensionType();
				UBLExtensionDian.ExtensionContent = ExtensionDian.Obtener(resolucion, TipoDocumento.AcuseRecibo, acuse.ID.Value, ruta_qr_Dian);
				UBLExtensions.Add(UBLExtensionDian);

				if (tipo_acuse.GetHashCode() > CodigoResponseV2.Expresa.GetHashCode() && !tipo_acuse.Equals(CodigoResponseV2.CancelacionEG) && !tipo_acuse.Equals(CodigoResponseV2.MandatoG))
				{
					UBLExtensionType UBLExtensionSector = new UBLExtensionType();
					UBLExtensionSector.ExtensionContent = ExtensionRadian.ObtenerRadianInscripcion(documento_factura.IntVlrTotal, tipo_acuse);
					UBLExtensions.Add(UBLExtensionSector);
				}

				// Extension de la firma
				UBLExtensionType UBLExtensionFirma = new UBLExtensionType();
				UBLExtensionFirma.ExtensionContent = doc.DocumentElement;
				UBLExtensions.Add(UBLExtensionFirma);

				acuse.UBLExtensions = UBLExtensions.ToArray();


				// convierte los datos del objeto en texto XML 
				//StringBuilder txt_xml = ConvertirXml(facturaXML, namespaces_xml);
				StringBuilder txt_xml = ConvertirXML.Convertir(acuse, namespaces_xml, TipoDocumento.AcuseRecibo);

				// valida el namespace xmlns:schemaLocation y lo reemplaza para Google Chrome
				TextReader textReader = new StringReader(txt_xml.ToString());
				string texto_xml = textReader.ReadToEnd();

				if (texto_xml.Contains("xmlns:schemaLocation"))
				{
					texto_xml = texto_xml.Replace("xmlns:schemaLocation", "xsi:schemaLocation");
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
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }

        }

        /// <summary>
        /// Convierte el objeto en texto XML
        /// </summary>
        /// <param name="factura">Objeto de tipo InvoiceType que contiene la informacion de la factura</param>
        /// <param name="namespaces_xml">Namespaces</param>       
        /// <returns>Ruta donde se guardo el archivo XML</returns>     
        private static StringBuilder ConvertirXml(ApplicationResponseType acuse, XmlSerializerNamespaces namespaces_xml)
        {
            try
            {
                if (acuse == null)
                    throw new Exception("El Acuse es inválido.");

                if (namespaces_xml == null)
                    throw new Exception("Los Namespaces son inválidos.");

                StringBuilder texto_xml = Xml.Convertir<ApplicationResponseType>(acuse, namespaces_xml);

                return texto_xml;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }


		/// <summary>
		/// Llena los Datos de la Persona o institución que genera el evento 
		/// </summary>
		/// <param name="tercero">Objeto Adquiriente</param>
		/// <param name="proveedor_receptor">Objeto del Proveedor del Adquiriente</param>
		/// <returns>Objeto ubl con la informacion del generador del documento electrónico</returns>
		private static PartyType ObtenerTercero(Tercero tercero, TblEmpresas proveedor_receptor, bool receptor, decimal participacion_endoso = 0, bool mandato = false)
        {
            try
            {
                if (tercero == null)
                    throw new Exception("Los datos del tercero son inválidos.");

                // Datos del adquiriente de la factura

                PartyType Party = new PartyType();

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
				if (tercero.TipoIdentificacion.Equals(31))
				{
					CompanyID.schemeVersionID = "1";
				}
				else
				{
					CompanyID.schemeVersionID = "2";
				}
				
                CompanyID.schemeAgencyID = "195";
                CompanyID.schemeAgencyName = "CO, DIAN (Dirección de Impuestos y Aduanas Nacionales)";
                PartyTaxScheme.CompanyID = CompanyID;

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

                if (receptor == true && participacion_endoso == 0)
                {
	                ContactType Contact = new ContactType();
	                TelephoneType Telephone = new TelephoneType();
	                Telephone.Value = tercero.Telefono;
	                Contact.Telephone = Telephone;
	                ElectronicMailType Mail = new ElectronicMailType();
	                Mail.Value = tercero.Email;
	                Contact.ElectronicMail = Mail;
	                Party.Contact = Contact;
				}

				//*****se pone el mismo participante que el generador del evento
				if(participacion_endoso > 0)
				{
					PartyLegalEntityType entidad = new PartyLegalEntityType();
					entidad.RegistrationName = new RegistrationNameType();
					entidad.RegistrationName.Value = PartyTaxScheme.RegistrationName.Value;
					entidad.CompanyID = new CompanyIDType();
					entidad.CompanyID = CompanyID;
					entidad.CorporateStockAmount = new CorporateStockAmountType();
					entidad.CorporateStockAmount.Value = participacion_endoso;
					entidad.CorporateStockAmount.currencyID = "COP";
					if (receptor == true)
					{
						
						entidad.CompanyLegalFormCode = new CompanyLegalFormCodeType();
						entidad.CompanyLegalFormCode.Value = "1";
					}

					Party.PartyLegalEntity = new PartyLegalEntityType[1];
					Party.PartyLegalEntity[0] = entidad;
				}

				//Si es un evento de mandato se debe enviar la informacion del presentante legal de la empresa(Adquiriente - Facturador nuestro)
				//que se esta habilitando y le esta otorgando el mandato a HGI como su proveedor Tecnologico
				if (mandato == true)
				{
					PersonType person = new PersonType();
					person.ID = new IDType();
					person.ID.schemeID = "";
					person.ID.schemeName = "13";
					person.ID.Value = "8005097";
					person.FirstName = new FirstNameType();
					person.FirstName.Value = "Jorge";
					person.FamilyName = new FamilyNameType();
					person.FamilyName.Value = "Bedoya";
					person.JobTitle = new JobTitleType();
					person.JobTitle.Value = "Representante Legal Principal";
					//En el Anexo del Radian las siguientes propiedades las ponen como opcionales.
					person.NationalityID = new NationalityIDType();
					person.NationalityID.Value = "Colombiana";
					person.OrganizationDepartment = new OrganizationDepartmentType();
					person.OrganizationDepartment.Value = "Administrativo";

					Party.Person = new PersonType[1];
					Party.Person[0] = person;

					PowerOfAttorneyType mandante = new PowerOfAttorneyType();
					mandante.ID = new IDType();
					mandante.ID.schemeID = tercero.IdentificacionDv.ToString();
					mandante.ID.schemeName = tercero.TipoIdentificacion.ToString();
					mandante.ID.Value = tercero.Identificacion;

					//Debe corresponde a un “código” del numeral 13.2.5. Tipo de Mandante y Debe corresponde a una “descripción” del numeral 13.2.5. Tipo de Mandante
					mandante.Description = new DescriptionType[1];
					DescriptionType Description = new DescriptionType();
					Description.Value = "Mandante Facturador Electrónico";
					mandante.Description[0] = Description;
					mandante.AgentParty = new PartyType();
					mandante.AgentParty.PartyIdentification = new PartyIdentificationType[1];
					mandante.AgentParty.PartyIdentification[0] = new PartyIdentificationType();
					mandante.AgentParty.PartyIdentification[0].ID = new IDType();
					mandante.AgentParty.PartyIdentification[0].ID.Value = "Mandante-FE";

					Party.PowerOfAttorney = new PowerOfAttorneyType[1];
					Party.PowerOfAttorney[0] = mandante;
				}

				//Se comenta proceso en espera de normatividad de interoperabilidad
				#region Identificador del proveedor de facturación electrónica intermediario en la recepción del documento electrónico. Para esta información se deberá especificar el NIT del proveedor tecnológico y su razón social en los campos cac:PartyIdentification y cac:PartyName respectivamente
					/*
					Party.AgentParty = new PartyType();

					PartyIdentificationType[] IdentificationsAgent = new PartyIdentificationType[1];
					PartyIdentificationType Identification = new PartyIdentificationType();
					IDType IDAgent = new IDType();
					IDAgent.Value = proveedor_receptor.StrIdentificacion.ToString();
					Identification.ID = IDAgent;
					IdentificationsAgent[0] = Identification;
					Party.AgentParty.PartyIdentification = IdentificationsAgent;


					List<PartyNameType> NameAgent = new List<PartyNameType>();
					PartyNameType Name_agent = new PartyNameType();
					NameType1 name_agent = new NameType1();
					name_agent.Value = proveedor_receptor.StrRazonSocial;
					Name_agent.Name = name_agent;
					NameAgent.Add(Name_agent);
					Party.AgentParty.PartyName = NameAgent.ToArray();
					*/
					#endregion

				return Party;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        public static string CufeApplicationV2(string pin_software, string numero_acuse,string fecha_acuse, string nit_emisor, string nit_receptor, string concepto_acuse, string doc_acusado, string tipo_doc_acusado, string code_response = "")
        {

			/*
	        Num_DE: Número del Documento Electrónico ApplicationResponse
	        Fec_Emi: Fecha de emisión del DE ApplicationResponse
		    Hor_Emi: Hora de emisión incluyendo GMT.
		    NitFE: Documento de la Persona o institución que genera el evento
		    DocAdq: Documento de la Persona que recibe este ApplicationResponse
	        ResponseCode: Código del evento registrado en este ApplicationResponse
	        ID: Prefijo y Número del documento referenciado
		    DocumentTypeCode: Identificador del tipo de documento referenciado
	        Software - PIN
			*/
			#region Creación Código CUFE

			string cufe = string.Empty;

			if (string.IsNullOrEmpty(code_response))
			{
				cufe = numero_acuse
				+ fecha_acuse
				+ nit_emisor
				+ nit_receptor
				+ concepto_acuse
				+ doc_acusado
				+ tipo_doc_acusado
				+ pin_software;
			}
			else if(code_response.Equals("2"))
			{
				cufe = numero_acuse
					+ fecha_acuse
					+ nit_receptor
					+ concepto_acuse
					+ code_response
					+ doc_acusado
					+ tipo_doc_acusado
					+ pin_software;
			}


			string cufe_encriptado = Encriptar.Encriptar_SHA384(cufe);
			return cufe_encriptado;
			#endregion
		}






	}
}
