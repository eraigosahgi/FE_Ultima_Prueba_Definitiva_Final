using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace HGInetUBL
{
    public class AcuseReciboXML
    {

        public static FacturaE_Documento CrearDocumento(Acuse documento, TblEmpresas proveedor_receptor, TblEmpresas proveedor_emisor, TipoDocumento tipo)
        {

            try
            {
                if (documento == null)
                    throw new Exception("La documento es inválido.");

                //Obtiene el nombre del archivo XML
                string nombre_archivo_xml = NombramientoArchivo.ObtenerXml(documento.Documento.ToString(), documento.DatosObligado.Identificacion.ToString(), tipo, documento.TipoDocumento);

                if (string.IsNullOrWhiteSpace(nombre_archivo_xml))
                    throw new ApplicationException("El nombre del archivo es inválido.");

                ApplicationResponseType acuse = new ApplicationResponseType();

                XmlSerializerNamespaces namespaces_xml = NamespacesXML.Obtener();

                #region acuse.UBLVersionID - Versión UBL
                /*La versión del esquema UBL utilizada por la Dian */
                UBLVersionIDType UBLVersionID = new UBLVersionIDType();
                UBLVersionID.Value = Recursos.VersionesDIAN.UBLVersionID;
                acuse.UBLVersionID = UBLVersionID;
                #endregion

                #region acuse.ProfileID - Versión del documento DIAN_UBL.xsd
                /*Especifica la versión de las personalizaciones usadas Versión del documento DIAN_UBL.xsd publicado por la DIAN*/
                ProfileIDType ProfileID = new ProfileIDType();
                ProfileID.Value = Recursos.VersionesDIAN.ProfileID;
                acuse.ProfileID = ProfileID;
                #endregion

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
                IssueTime.Value = documento.Fecha.ToString(Fecha.formato_hora_completa);
                acuse.IssueTime = IssueTime;
                #endregion

                #region acuse.Note - Tracking ID(Id seguridad del documento original)
                List<NoteType> notas = new List<NoteType>();
                NoteType nota = new NoteType();
                nota.Value = string.Format("Tracking ID {0}", documento.IdSeguridad.ToString());
                notas.Add(nota);
                acuse.Note = notas.ToArray();
                #endregion

                #region acuse.SenderParty - Información del receptor del documento electrónico
                acuse.SenderParty = ObtenerAdquiriente(documento.DatosAdquiriente, proveedor_emisor);
                #endregion

                #region acuse.ReceiverParty - Información del emisor del documento electrónico
                acuse.ReceiverParty = ObtenerObligado(documento.DatosObligado, proveedor_receptor);
                #endregion

                #region Document response -  Document reference 
                acuse.DocumentResponse = new DocumentResponseType[1];
                DocumentResponseType DocumentResponse = new DocumentResponseType();
                ResponseType response = new ResponseType();
                response.ReferenceID = new ReferenceIDType();
                response.ReferenceID.Value = documento.Documento.ToString();
                response.ResponseCode = new ResponseCodeType();
                response.ResponseCode.Value = documento.CodigoRespuesta;
                DescriptionType[] Description = new DescriptionType[1];
                DescriptionType description = new DescriptionType();
                description.Value = documento.MvoRespuesta;
                Description[0] = description;
                response.Description = Description;
                DocumentResponse.Response = response;
                acuse.DocumentResponse[0] = DocumentResponse;


                DocumentReferenceType DocumentReference = new DocumentReferenceType();
                DocumentReference.ID = new IDType();
                DocumentReference.ID.Value = documento.Documento.ToString();
                DocumentReference.UUID = new UUIDType();
                DocumentReference.UUID.Value = documento.IdSeguridad.ToString();
                DocumentReference.DocumentType = new DocumentTypeType();
                DocumentReference.DocumentType.Value = documento.TipoDocumento;
                acuse.DocumentResponse[0].DocumentReference = DocumentReference;

                #endregion


                // convierte los datos del objeto en texto XML 
                StringBuilder txt_xml = ConvertirXml(acuse, namespaces_xml);

                FacturaE_Documento xml_acuse = new FacturaE_Documento();
                xml_acuse.Documento = documento;
                xml_acuse.NombreXml = nombre_archivo_xml;
                xml_acuse.DocumentoXml = txt_xml;


                return xml_acuse;

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
                    throw new Exception("La factura es inválida.");

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
        /// Llena los Datos del receptor del documento electrónico(Adquiriente)
        /// </summary>
        /// <param name="tercero">Objeto Adquiriente</param>
        /// <param name="proveedor_receptor">Objeto del Proveedor del Adquiriente</param>
        /// <returns>Objeto ubl con la informacion del receptor del documento electrónico</returns>
        private static PartyType ObtenerAdquiriente(Tercero tercero, TblEmpresas proveedor_receptor)
        {
            try
            {
                if (tercero == null)
                    throw new Exception("Los datos del tercero son inválidos.");

                // Datos del adquiriente de la factura

                PartyType Party = new PartyType();

                #region Documento y tipo de documento 
                //Identificación (NIT) del receptor de la factura electrónica. Esta información se deberá especificar en el campo cbc:ID 
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

                #region Razón social del receptor de la factura electrónica. Esta información se deberá especificar en el campo cbc:Name 

                List<PartyNameType> PartyName = new List<PartyNameType>();
                PartyNameType Name = new PartyNameType();
                NameType1 name = new NameType1();
                name.Value = tercero.RazonSocial;
                Name.Name = name;
                PartyName.Add(Name);
                Party.PartyName = PartyName.ToArray();
                #endregion

                #region Identificador del proveedor de facturación electrónica intermediario en la recepción del documento electrónico. Para esta información se deberá especificar el NIT del proveedor tecnológico y su razón social en los campos cac:PartyIdentification y cac:PartyName respectivamente
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

                #endregion

                return Party;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        /// <summary>
        /// Llena los Datos del emisor del documento electrónico(Obligado a facturar)
        /// </summary>
        /// <param name="tercero">Objeto Adquiriente</param>
        /// <param name="proveedor_emisor">Objeto del Proveedor del Obligado a Facturar</param>
        /// <returns>Objeto ubl con la informacion del emisor del documento electrónico</returns>
        private static PartyType ObtenerObligado(Tercero tercero, TblEmpresas proveedor_emisor)
        {
            try
            {
                if (tercero == null)
                    throw new Exception("Los datos del tercero son inválidos.");

                // Datos del adquiriente de la factura

                PartyType Party = new PartyType();

                #region Documento y tipo de documento 
                //Identificación (NIT) del receptor de la factura electrónica. Esta información se deberá especificar en el campo cbc:ID 
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

                #region Razón social del receptor de la factura electrónica. Esta información se deberá especificar en el campo cbc:Name 

                List<PartyNameType> PartyName = new List<PartyNameType>();
                PartyNameType Name = new PartyNameType();
                NameType1 name = new NameType1();
                name.Value = tercero.RazonSocial;
                Name.Name = name;
                PartyName.Add(Name);
                Party.PartyName = PartyName.ToArray();
                #endregion

                #region Identificador del proveedor de facturación electrónica intermediario en la recepción del documento electrónico. Para esta información se deberá especificar el NIT del proveedor tecnológico y su razón social en los campos cac:PartyIdentification y cac:PartyName respectivamente
                Party.AgentParty = new PartyType();
                PartyIdentificationType[] IdentificationsAgent = new PartyIdentificationType[1];
                PartyIdentificationType Identification = new PartyIdentificationType();
                IDType IDAgent = new IDType();
                IDAgent.Value = proveedor_emisor.StrIdentificacion.ToString();
                Identification.ID = IDAgent;
                IdentificationsAgent[0] = Identification;
                Party.AgentParty.PartyIdentification = IdentificationsAgent;


                List<PartyNameType> NameAgent = new List<PartyNameType>();
                PartyNameType Name_agent = new PartyNameType();
                NameType1 name_agent = new NameType1();
                name_agent.Value = proveedor_emisor.StrRazonSocial;
                Name_agent.Name = name_agent;
                NameAgent.Add(Name_agent);
                Party.AgentParty.PartyName = NameAgent.ToArray();

                #endregion

                return Party;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }
    }
}
