using HGInetInteroperabilidad.Objetos;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetUBL;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using LibreriaGlobalHGInet.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HGInetMiFacturaElectonicaController.Procesos
{
    public partial class Ctl_Documentos
    {

        public static RegistroListaDocRespuesta Procesar(RegistroListaDoc datos, string ruta_ftp)
        {

            bool error_proceso = false;

            RegistroListaDocRespuesta datos_respuesta = new RegistroListaDocRespuesta();

            if (datos == null)
            {
                datos_respuesta.timeStamp = Fecha.GetFecha();
                datos_respuesta.trackingIds = null;
                datos_respuesta.mensajeGlobal = string.Format("Documento {0} {1}", datos.nombre, Enumeracion.GetDescription(RespuestaInterOperabilidad.Zipvacio));
                //throw new ApplicationException("No se encontraron datos");
            }
            else
            {
                List<Documentos> documentos = datos.documentos;

                if (documentos == null)
                    throw new ApplicationException("No se encontraron datos");

                if (string.IsNullOrEmpty(ruta_ftp))
                    throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "ruta_ftp", "string"));

                List<RegistroListaDetalleDocRespuesta> respuesta = new List<RegistroListaDetalleDocRespuesta>();


                foreach (Documentos objeto in documentos)
                {

                    RegistroListaDetalleDocRespuesta item_respuesta = new RegistroListaDetalleDocRespuesta();

                    try
                    {

                        // obtiene los datos del Adquiriente enviado como facturador nuestro
                        Ctl_Empresa empresa = new Ctl_Empresa();
                        TblEmpresas facturador_receptor = empresa.Obtener(objeto.identificacionDestinatario);

                        if (facturador_receptor == null)
                        {
                            item_respuesta.codigoError = RespuestaInterOperabilidad.ClienteNoEncontrado.GetHashCode().ToString();
                            item_respuesta.mensaje = string.Format("{0} en el ZIP {1}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.Enumerables.RespuestaInterOperabilidad>(Convert.ToInt16(item_respuesta.codigoError))), datos.nombre);
                            error_proceso = true;
                            throw new ApplicationException(string.Format("No se encontro el Facturador Receptor {0} en nuestra Plataforma ", objeto.identificacionDestinatario));
                        }

                        string nombre_archivo = Path.GetFileNameWithoutExtension(objeto.nombre);

                        //Se valida que el archivo xml si exista en la ruta
                        string ruta_archivo_xml = string.Format(@"{0}\{1}", ruta_ftp, objeto.nombre);

                        if (!LibreriaGlobalHGInet.General.Directorio.ValidarExistenciaArchivo(ruta_archivo_xml))
                        {
                            item_respuesta.nombreDocumento = objeto.nombre;
                            item_respuesta.codigoError = RespuestaInterOperabilidad.DocumentoNoEncontradoZip.GetHashCode().ToString();
                            item_respuesta.mensaje = string.Format("{0} en el ZIP {1}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.Enumerables.RespuestaInterOperabilidad>(Convert.ToInt16(item_respuesta.codigoError))), datos.nombre);
                            error_proceso = true;
                            throw new ApplicationException(string.Format("No se encontro el archivo {0} ", ruta_archivo_xml));
                        }

                        //se valida que exista el archivo pdf
                        if (objeto.representacionGraficas)
                        {
                            string ruta_archivo_pdf = string.Format(@"{0}\{1}.pdf", ruta_ftp, nombre_archivo);

                            if (!LibreriaGlobalHGInet.General.Directorio.ValidarExistenciaArchivo(ruta_archivo_pdf))
                            {
                                item_respuesta.nombreDocumento = string.Format("{0}.pdf", objeto.nombre);
                                item_respuesta.codigoError = RespuestaInterOperabilidad.DocumentoNoEncontrado.GetHashCode().ToString();
                                item_respuesta.mensaje = string.Format("{0} en el ZIP {1}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.Enumerables.RespuestaInterOperabilidad>(Convert.ToInt16(item_respuesta.codigoError))), datos.nombre);
                                error_proceso = true;
                                throw new ApplicationException(string.Format("No se encontro el archivo {0}", ruta_archivo_pdf));
                            }
                        }
                        //se valida que exista adjuntos como zip
                        if (objeto.adjuntos)
                        {
                            string ruta_archivo_zip = string.Format(@"{0}\{1}.zip", ruta_ftp, nombre_archivo);

                            if (!LibreriaGlobalHGInet.General.Directorio.ValidarExistenciaArchivo(ruta_archivo_zip))
                            {
                                item_respuesta.nombreDocumento = string.Format("{0}.zip", objeto.nombre);
                                item_respuesta.codigoError = RespuestaInterOperabilidad.DocumentoNoEncontrado.GetHashCode().ToString();
                                item_respuesta.mensaje = string.Format("{0} en el ZIP {1}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.Enumerables.RespuestaInterOperabilidad>(Convert.ToInt16(item_respuesta.codigoError))), datos.nombre);
                                error_proceso = true;
                                throw new ApplicationException(string.Format("No se encontro el archivo {0}", ruta_archivo_zip));
                            }
                        }
                        // representación de datos en objeto
                        var documento_obj = (dynamic)null;

                        DocumentType tipo_doc = new DocumentType();

                        tipo_doc = (DocumentType)Enumeracion.ParseToEnum<DocumentType>(objeto.tipo);

                        documento_obj = ObtenerDocumento(ruta_archivo_xml, tipo_doc);

                        //Creacion

                       
                    }
                    catch (Exception ex)
                    {
                        LogExcepcion.Guardar(ex);

                    }

                    respuesta.Add(item_respuesta);

                }
                datos_respuesta.timeStamp = Fecha.GetFecha();
                datos_respuesta.trackingIds = respuesta;
                if (error_proceso)
                    datos_respuesta.mensajeGlobal = string.Format("Documento {0} {1}", datos.nombre, Enumeracion.GetDescription(RespuestaInterOperabilidad.ZipRadicado));
                else
                    datos_respuesta.mensajeGlobal = string.Format("Documento {0} {1}", datos.nombre, Enumeracion.GetDescription(RespuestaInterOperabilidad.ProcesamientoParcial));
            }
            return datos_respuesta;
        }


        /// <summary>
        /// Genera documento a partir del Ubl
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="tipo_documento"></param>
        /// <returns></returns>
        public static dynamic ObtenerDocumento(string ruta, DocumentType tipo_documento)
        {

            // representación de datos en objeto
            var documento_obj = (dynamic)null;

            FileStream xml_reader = new FileStream(ruta, FileMode.Open);

            XmlSerializer serializacion = null;

            if (tipo_documento == DocumentType.FacturaNacional)
            {
                serializacion = new XmlSerializer(typeof(InvoiceType));

                InvoiceType conversion = (InvoiceType)serializacion.Deserialize(xml_reader);

                documento_obj = FacturaXML.Convertir(conversion);

            }
            else if (tipo_documento == DocumentType.NotaCredito)
            {
                serializacion = new XmlSerializer(typeof(CreditNoteType));

                CreditNoteType conversion = (CreditNoteType)serializacion.Deserialize(xml_reader);

                documento_obj = NotaCreditoXML.Convertir(conversion);
            }
            else if (tipo_documento == DocumentType.NotaDebito)
            {
                serializacion = new XmlSerializer(typeof(DebitNoteType));

                DebitNoteType conversion = (DebitNoteType)serializacion.Deserialize(xml_reader);

                documento_obj = NotaDebitoXML.Convertir(conversion);
            }


            return documento_obj;
        }




    }
}
