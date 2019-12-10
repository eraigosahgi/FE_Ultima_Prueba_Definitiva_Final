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

		public static FacturaE_Documento CrearDocumento(Acuse documento, TblEmpresas proveedor_receptor, TblEmpresas proveedor_emisor, string ambiente, string pin_sw, string cufe_docreferenciado)
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

                ApplicationResponseType acuse = new ApplicationResponseType();

                XmlSerializerNamespaces namespaces_xml = NamespacesXML.ObtenerNamespaces();

                #region acuse.UBLVersionID - Versión UBL
                /*La versión del esquema UBL utilizada por la Dian */
                UBLVersionIDType UBLVersionID = new UBLVersionIDType();
                UBLVersionID.Value = Recursos.VersionesDIANv2_1.UBLVersionID;
                acuse.UBLVersionID = UBLVersionID;
				#endregion

				CustomizationIDType Customization = new CustomizationIDType();
				Customization.Value = "1";
				acuse.CustomizationID = Customization;

				#region acuse.ProfileID - Versión del documento DIAN_UBL.xsd
				/*Especifica la versión de las personalizaciones usadas Versión del documento DIAN_UBL.xsd publicado por la DIAN*/
				ProfileIDType ProfileID = new ProfileIDType();
                ProfileID.Value = Recursos.VersionesDIANv2_1.ProfileID;
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

				#region acuse.SenderParty - Información del emisor del evento
				acuse.SenderParty = ObtenerTercero(documento.DatosAdquiriente, proveedor_receptor, false);
				#endregion

				#region acuse.ReceiverParty - Información del receptor del evento
				acuse.ReceiverParty = ObtenerTercero(documento.DatosObligado, proveedor_receptor, true);
				#endregion

				#region Document response -  Document reference 
				acuse.DocumentResponse = new DocumentResponseType[1];
                DocumentResponseType DocumentResponse = new DocumentResponseType();
                ResponseType response = new ResponseType();
                response.ReferenceID = new ReferenceIDType();
                response.ReferenceID.Value = string.Format("{0}{1}", documento.Prefijo,documento.Documento.ToString());
                response.ResponseCode = new ResponseCodeType();
                response.ResponseCode.Value = documento.CodigoRespuesta;
                DescriptionType[] Description = new DescriptionType[1];
                DescriptionType description = new DescriptionType();
                description.Value = Enumeracion.GetDescription(Enumeracion.GetValueFromAmbiente<HGInetMiFacturaElectonicaData.CodigoResponseV2>(documento.CodigoRespuesta));
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
                if (documento.TipoDocumento.Equals("91") || documento.TipoDocumento.Equals("92"))
                {
	                DocumentReference.UUID.schemeName = Recursos.VersionesDIANv2_1.CUDE;
				}
                else
                {
					DocumentReference.UUID.schemeName = Recursos.VersionesDIANv2_1.CUFE;
				}
				DocumentReference.DocumentType = new DocumentTypeType();
                DocumentReference.DocumentType.Value = documento.TipoDocumento;
                acuse.DocumentResponse[0].DocumentReference[0] = DocumentReference;

				#endregion

				UUIDType UUID = new UUIDType();
				//-----Se agrega Ambiente al cual se va enviar el documento
				string CUFE = CufeApplicationV2(pin_sw, acuse.ID.Value,string.Format("{0}{1}", documento.Fecha.ToString(Fecha.formato_fecha_hginet), documento.Fecha.AddHours(5).ToString(Fecha.formato_hora_zona)),documento.DatosAdquiriente.Identificacion,documento.DatosObligado.Identificacion,documento.CodigoRespuesta, string.Format("{0}{1}", documento.Prefijo, documento.Documento.ToString()),documento.TipoDocumento);
				UUID.Value = CUFE;
				UUID.schemeName = Recursos.VersionesDIANv2_1.CUDE;
				UUID.schemeID = acuse.ProfileExecutionID.Value; //"2";
				acuse.UUID = UUID;

				XmlDocument doc = new XmlDocument();
				doc.LoadXml("<firma></firma>");

				List<UBLExtensionType> UBLExtensions = new List<UBLExtensionType>();

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
		private static PartyType ObtenerTercero(Tercero tercero, TblEmpresas proveedor_receptor, bool receptor)
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
                CompanyID.schemeAgencyID = "195";
                CompanyID.schemeAgencyName = "CO, DIAN (Dirección de Impuestos y Aduanas Nacionales)";
                PartyTaxScheme.CompanyID = CompanyID;

                TaxSchemeType TaxScheme = new TaxSchemeType();
                TaxScheme.ID = new IDType();
                TaxScheme.ID.Value = tercero.CodigoTributo;
                ListaTipoImpuesto list_tipoImp = new ListaTipoImpuesto();
                ListaItem tipoimp = list_tipoImp.Items.Where(d => d.Codigo.Equals(tercero.CodigoTributo)).FirstOrDefault();
                TaxScheme.Name = new NameType1();
                TaxScheme.Name.Value = tipoimp.Nombre; //"IVA";
                PartyTaxScheme.TaxScheme = TaxScheme;
                PartyTaxSchemes[0] = PartyTaxScheme;
                Party.PartyTaxScheme = PartyTaxSchemes;

                if (receptor == true)
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

        public static string CufeApplicationV2(string pin_software, string numero_acuse,string fecha_acuse, string nit_emisor, string nit_receptor, string concepto_acuse, string doc_acusado, string tipo_doc_acusado)
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

			string cufe = numero_acuse
				+ fecha_acuse
				+ nit_emisor
				+ nit_receptor
				+ concepto_acuse
				+ doc_acusado
				+ tipo_doc_acusado
				+ pin_software
			;

			string cufe_encriptado = Encriptar.Encriptar_SHA384(cufe);
			return cufe_encriptado;
			#endregion
		}






	}
}
