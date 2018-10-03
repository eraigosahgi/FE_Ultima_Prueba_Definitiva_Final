using HGInetInteroperabilidad.Objetos;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
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

namespace HGInetInteroperabilidad.Procesos
{
    public class Ctl_Recepcion
    {

        /// <summary>
        /// Procesa peticion de proveedor emisor
        /// </summary>
        /// <param name="datos">Objeto de la Peticion</param>
        /// <param name="ruta_ftp">Ruta publica de los documentos</param>
        /// <param name="proveedor_emisor">Identificacion Proveedor Emisor</param>
        /// <returns></returns>
        public static RegistroListaDocRespuesta Procesar(RegistroListaDoc datos, string ruta_ftp, string proveedor_emisor)
        {

            bool error_proceso = false;

            RegistroListaDocRespuesta datos_respuesta = new RegistroListaDocRespuesta();

            if (datos.documentos == null)
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
                            item_respuesta.nombreDocumento = objeto.nombre;
                            item_respuesta.codigoError = RespuestaInterOperabilidad.ClienteNoEncontrado.GetHashCode().ToString();
                            item_respuesta.mensaje = string.Format("{0} en el ZIP {1}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.Enumerables.RespuestaInterOperabilidad>(Convert.ToInt16(item_respuesta.codigoError))), datos.nombre);
                            error_proceso = true;
                            throw new ApplicationException(string.Format("No se encontro el Facturador Receptor {0} en nuestra Plataforma ", objeto.identificacionDestinatario));
                        }

                        string nombre_archivo = Path.GetFileNameWithoutExtension(objeto.nombre);

                        string ruta_archivo_xml = string.Format(@"{0}\{1}", ruta_ftp, objeto.nombre);

                        //Se valida que el archivo xml si exista en la ruta y almacena en carpeta del Facturador emisor
                        if (!Archivo.ValidarExistencia(ruta_archivo_xml))
                        {
                            item_respuesta.nombreDocumento = objeto.nombre;
                            item_respuesta.codigoError = RespuestaInterOperabilidad.DocumentoNoEncontradoZip.GetHashCode().ToString();
                            item_respuesta.mensaje = string.Format("{0} en el ZIP {1}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.Enumerables.RespuestaInterOperabilidad>(Convert.ToInt16(item_respuesta.codigoError))), datos.nombre);
                            error_proceso = true;
                            throw new ApplicationException(string.Format("No se encontro el archivo {0} ", ruta_archivo_xml));
                        }

                        // representación de datos en objeto
                        var documento_obj = (dynamic)null;

                        DocumentType tipo_doc = new DocumentType();

                        //Convierte el UBL en objeto
                        try
                        {
                            tipo_doc = (DocumentType)Enumeracion.ParseToEnum<DocumentType>(objeto.tipo);

                            documento_obj = ObtenerDocumento(ruta_archivo_xml, tipo_doc);
                        }
                        catch (Exception excepcion)
                        {

                            LogExcepcion.Guardar(excepcion);
                            item_respuesta.nombreDocumento = objeto.nombre;
                            item_respuesta.codigoError = RespuestaInterOperabilidad.ErrorInternoReceptor.GetHashCode().ToString();
                            item_respuesta.mensaje = string.Format("{0} en el documento {1}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.Enumerables.RespuestaInterOperabilidad>(Convert.ToInt16(item_respuesta.codigoError))), objeto.nombre);
                            error_proceso = true;
                            throw new ApplicationException(string.Format("Error al convertir xml a objeto el archivo {0} Detalle: {1}", documento_obj.Documento, excepcion.Message));

                        }

                        Ctl_Documento num_doc = new Ctl_Documento();

                        TblDocumentos documento_bd = new TblDocumentos();

                        if (tipo_doc == DocumentType.AcuseDeRecibo)
                        {

                            item_respuesta = ProcesarAcuse(documento_obj, ruta_archivo_xml, nombre_archivo, facturador_receptor);
                            item_respuesta.nombreDocumento = objeto.nombre;

                            if (item_respuesta.codigoError == Enumeracion.GetDescription(RespuestaInterOperabilidad.ErrorInternoReceptor))
                            {
                                error_proceso = true;
                                throw new ApplicationException(string.Format("Error al procesar el documento {0}", objeto.nombre));
                            }

                        }
                        else
                        {
                            //Obtiene de la BD el documento enviado
                            try
                            {
                                documento_bd = num_doc.Obtener(documento_obj.DatosObligado.Identificacion, documento_obj.Documento, documento_obj.Prefijo);
                            }
                            catch (Exception excepcion)
                            {

                                LogExcepcion.Guardar(excepcion);
                                item_respuesta.nombreDocumento = objeto.nombre;
                                item_respuesta.codigoError = RespuestaInterOperabilidad.ErrorInternoReceptor.GetHashCode().ToString();
                                item_respuesta.mensaje = string.Format("{0} en el documento {1}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.Enumerables.RespuestaInterOperabilidad>(Convert.ToInt16(item_respuesta.codigoError))), objeto.nombre);
                                error_proceso = true;
                                throw new ApplicationException(string.Format("Error al obtener el documento {0} Detalle: {1}", documento_obj.Documento, excepcion.Message));

                            }

                            //valida si el Documento ya existe en Base de Datos
                            if (documento_bd != null)
                            {
                                item_respuesta.nombreDocumento = objeto.nombre;
                                item_respuesta.codigoError = RespuestaInterOperabilidad.ProcesamientoParcial.GetHashCode().ToString();
                                item_respuesta.mensaje = string.Format("{0} El documento {1} se encuentra registrado con el Tacking ID: {2}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<RespuestaInterOperabilidad>(Convert.ToInt16(item_respuesta.codigoError))), datos.nombre, documento_bd.StrIdInteroperabilidad);
                                item_respuesta.uuid = documento_bd.StrIdInteroperabilidad.ToString();
                                error_proceso = true;
                                throw new ApplicationException(string.Format("El documento {0} con prefijo {1} ya xiste para el Facturador Electrónico {2}", documento_obj.Documento, documento_obj.Prefijo, documento_obj.DatosObligado.Identificacion));
                            }

                            //Creacion Facturador Emisor del documento 
                            TblEmpresas facturador_emisor = new TblEmpresas();

                            try
                            {
                                facturador_emisor = CrearFacturadorEmisor(documento_obj, tipo_doc.GetHashCode());


                                AlmacenarArchivo(ruta_archivo_xml, string.Format("{0}.xml", nombre_archivo), facturador_emisor);
                            }
                            catch (Exception excepcion)
                            {

                                LogExcepcion.Guardar(excepcion);
                                item_respuesta.nombreDocumento = objeto.nombre;
                                item_respuesta.codigoError = RespuestaInterOperabilidad.ErrorInternoReceptor.GetHashCode().ToString();
                                item_respuesta.mensaje = string.Format("{0} en el documento {1}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.Enumerables.RespuestaInterOperabilidad>(Convert.ToInt16(item_respuesta.codigoError))), objeto.nombre);
                                error_proceso = true;
                                throw new ApplicationException(string.Format("Error al almacenar el archivo {0} Detalle: {1}", documento_obj.Documento, excepcion.Message));

                            }

                            //se valida que exista el archivo pdf y almacena en carpeta del Facturador emisor
                            if (objeto.representacionGraficas)
                            {
                                string ruta_archivo_pdf = string.Format(@"{0}\{1}.pdf", ruta_ftp, nombre_archivo);

                                if (!Archivo.ValidarExistencia(ruta_archivo_pdf))
                                {
                                    item_respuesta.nombreDocumento = string.Format("{0}.pdf", objeto.nombre);
                                    item_respuesta.codigoError = RespuestaInterOperabilidad.DocumentoNoEncontrado.GetHashCode().ToString();
                                    item_respuesta.mensaje = string.Format("{0} en el ZIP {1}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.Enumerables.RespuestaInterOperabilidad>(Convert.ToInt16(item_respuesta.codigoError))), datos.nombre);
                                    error_proceso = true;
                                    throw new ApplicationException(string.Format("No se encontro el archivo {0}", ruta_archivo_pdf));
                                }

                                try
                                {
                                    AlmacenarArchivo(ruta_archivo_pdf, string.Format("{0}.pdf", nombre_archivo), facturador_emisor);
                                }
                                catch (Exception excepcion)
                                {

                                    LogExcepcion.Guardar(excepcion);
                                    item_respuesta.nombreDocumento = objeto.nombre;
                                    item_respuesta.codigoError = RespuestaInterOperabilidad.ErrorInternoReceptor.GetHashCode().ToString();
                                    item_respuesta.mensaje = string.Format("{0} en el documento {1}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.Enumerables.RespuestaInterOperabilidad>(Convert.ToInt16(item_respuesta.codigoError))), objeto.representacionGraficas);
                                    error_proceso = true;
                                    throw new ApplicationException(string.Format("Error al almacenar el archivo {0} Detalle: {1}", documento_obj.Documento, excepcion.Message));

                                }
                            }
                            //se valida que exista adjuntos como zip y almacena en carpeta del Facturador emisor
                            if (objeto.adjuntos)
                            {
                                string ruta_archivo_zip = string.Format(@"{0}\{1}.zip", ruta_ftp, nombre_archivo);

                                if (!Archivo.ValidarExistencia(ruta_archivo_zip))
                                {
                                    item_respuesta.nombreDocumento = string.Format("{0}.zip", objeto.nombre);
                                    item_respuesta.codigoError = RespuestaInterOperabilidad.DocumentoNoEncontrado.GetHashCode().ToString();
                                    item_respuesta.mensaje = string.Format("{0} en el ZIP {1}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.Enumerables.RespuestaInterOperabilidad>(Convert.ToInt16(item_respuesta.codigoError))), datos.nombre);
                                    error_proceso = true;
                                    throw new ApplicationException(string.Format("No se encontro el archivo {0}", ruta_archivo_zip));
                                }
                                try
                                {
                                    AlmacenarArchivo(ruta_archivo_zip, string.Format("{0}.zip", nombre_archivo), facturador_emisor);
                                }
                                catch (Exception excepcion)
                                {

                                    LogExcepcion.Guardar(excepcion);
                                    item_respuesta.nombreDocumento = objeto.nombre;
                                    item_respuesta.codigoError = RespuestaInterOperabilidad.ErrorInternoReceptor.GetHashCode().ToString();
                                    item_respuesta.mensaje = string.Format("{0} en el documento {1}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.Enumerables.RespuestaInterOperabilidad>(Convert.ToInt16(item_respuesta.codigoError))), objeto.adjuntos);
                                    error_proceso = true;
                                    throw new ApplicationException(string.Format("Error al almacenar el archivo {0} Detalle: {1}", documento_obj.Documento, excepcion.Message));

                                }
                            }

                            //Convierto el Objeto a Tbl
                            try
                            {
                                documento_bd = Convertir(documento_obj, tipo_doc, facturador_emisor, nombre_archivo, proveedor_emisor);
                            }
                            catch (Exception excepcion)
                            {

                                LogExcepcion.Guardar(excepcion);
                                item_respuesta.nombreDocumento = objeto.nombre;
                                item_respuesta.codigoError = RespuestaInterOperabilidad.ErrorInternoReceptor.GetHashCode().ToString();
                                item_respuesta.mensaje = string.Format("{0} en el documento {1}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.Enumerables.RespuestaInterOperabilidad>(Convert.ToInt16(item_respuesta.codigoError))), objeto.nombre);
                                error_proceso = true;
                                throw new ApplicationException(string.Format("Error al convertir objeto {0} Detalle: {1}", documento_obj.Documento, excepcion.Message));

                            }

                            //Guardo el documento en BD
                            Ctl_Documento documento_tmp = new Ctl_Documento();

                            try
                            {
                                documento_bd = documento_tmp.Crear(documento_bd);
                                item_respuesta.uuid = documento_bd.StrIdInteroperabilidad.ToString();
                            }
                            catch (Exception excepcion)
                            {

                                LogExcepcion.Guardar(excepcion);
                                item_respuesta.nombreDocumento = objeto.nombre;
                                item_respuesta.codigoError = RespuestaInterOperabilidad.ErrorInternoReceptor.GetHashCode().ToString();
                                item_respuesta.mensaje = string.Format("{0} en el ZIP {1}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.Enumerables.RespuestaInterOperabilidad>(Convert.ToInt16(item_respuesta.codigoError))), datos.nombre);
                                error_proceso = true;
                                throw new ApplicationException(string.Format("Error al guardar el documento {0} Detalle: {1}", objeto.nombre, excepcion.Message));

                            }
                        }

                        item_respuesta.nombreDocumento = objeto.nombre;
                        item_respuesta.codigoError = RespuestaInterOperabilidad.PendienteProcesamiento.GetHashCode().ToString();
                        item_respuesta.mensaje = string.Format("{0} en el ZIP {1}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.Enumerables.RespuestaInterOperabilidad>(Convert.ToInt16(item_respuesta.codigoError))), datos.nombre);
                        error_proceso = false;


                    }
                    catch (Exception ex)
                    {
                        LogExcepcion.Guardar(ex);

                    }

                    respuesta.Add(item_respuesta);

                }
                datos_respuesta.timeStamp = Fecha.GetFecha();
                datos_respuesta.trackingIds = respuesta;
                if (!error_proceso)
                    datos_respuesta.mensajeGlobal = string.Format("Documento {0} {1}", datos.nombre, Enumeracion.GetDescription(RespuestaInterOperabilidad.ZipRadicado));
                else
                    datos_respuesta.mensajeGlobal = string.Format("Documento {0} {1}", datos.nombre, Enumeracion.GetDescription(RespuestaInterOperabilidad.ProcesamientoParcial));
            }
            return datos_respuesta;
        }


        /// <summary>
        /// Procesa el Acuse
        /// </summary>
        /// <param name="documento_obj">Objeto Acuse</param>
        /// <param name="ruta_archivo_xml">Ruta donde esta almacenado el archivo</param>
        /// <param name="nombre_archivo">Nombre del archivo del Acuse</param>
        /// <param name="facturador_receptor">Datos del Facturador Receptor</param>
        /// <returns></returns>
        public static RegistroListaDetalleDocRespuesta ProcesarAcuse(Acuse documento_obj, string ruta_archivo_xml, string nombre_archivo, TblEmpresas facturador_receptor)
        {

            RegistroListaDetalleDocRespuesta item_respuesta = new RegistroListaDetalleDocRespuesta();

            try
            {
                Ctl_Documento num_doc = new Ctl_Documento();

                TblDocumentos documento_bd = new TblDocumentos();


                //valida si el Documento ya existe en Base de Datos para actualizar la informacion
                try
                {
                    documento_bd = num_doc.DocumentoPorIdSeguridad(Guid.Parse(documento_obj.IdSeguridad));
                }
                catch (Exception excepcion)
                {

                    LogExcepcion.Guardar(excepcion);
                    item_respuesta.codigoError = RespuestaInterOperabilidad.ErrorInternoReceptor.GetHashCode().ToString();
                    item_respuesta.mensaje = string.Format("{0} en el documento {1}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.Enumerables.RespuestaInterOperabilidad>(Convert.ToInt16(item_respuesta.codigoError))), documento_obj.Documento);
                    throw new ApplicationException(string.Format("Error al obtener el documento {0} Detalle: {1}", documento_obj.Documento, excepcion.Message));


                }

                if (documento_bd == null)
                {
                    item_respuesta.codigoError = RespuestaInterOperabilidad.ProcesamientoParcial.GetHashCode().ToString();
                    item_respuesta.mensaje = string.Format("{0} El documento {1} no se encuentra registrado en nuestra plataforma", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<RespuestaInterOperabilidad>(Convert.ToInt16(item_respuesta.codigoError))), documento_obj.Documento);
                    throw new ApplicationException(string.Format("El documento {0} no existe en la platforma a nombre del Facturador Electrónico {1} con IdSeguridad {2}", documento_obj.Documento, documento_obj.DatosObligado.Identificacion, documento_obj.IdSeguridad));
                }
                else
                {
                    //Proceso para Almacenar el acuse
                    try
                    {
                        AlmacenarArchivo(ruta_archivo_xml, string.Format("{0}.xml", nombre_archivo), facturador_receptor);
                    }
                    catch (Exception excepcion)
                    {

                        LogExcepcion.Guardar(excepcion);
                        item_respuesta.codigoError = RespuestaInterOperabilidad.ErrorInternoReceptor.GetHashCode().ToString();
                        item_respuesta.mensaje = string.Format("{0} en el documento {1}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.Enumerables.RespuestaInterOperabilidad>(Convert.ToInt16(item_respuesta.codigoError))), documento_obj.Documento);
                        throw new ApplicationException(string.Format("Error al almacenar el archivo {0} Detalle: {1}", documento_obj.Documento, excepcion.Message));
                    }

                    PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

                    // url pública
                    string url_ppal = string.Format("{0}\\{1}\\{2}", plataforma_datos.RutaDmsPublica, Constantes.RutaHGInetFacturaElectronica, facturador_receptor.StrIdSeguridad);

                    // url pública del xml
                    string UrlAcuseUbl = string.Format(@"{0}{1}/{2}.xml", url_ppal, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaXmlAcuse, nombre_archivo);

                    try
                    {
                        ResponseCode cod_resp = Enumeracion.GetValueFromDescription<ResponseCode>(documento_obj.CodigoRespuesta);

                        //Actualizacion de Campos de Acuse
                        documento_bd.IntAdquirienteRecibo = (short)cod_resp.GetHashCode();
                        documento_bd.DatAdquirienteFechaRecibo = documento_obj.Fecha;
                        documento_bd.StrAdquirienteMvoRechazo = documento_obj.MvoRespuesta;
                        documento_bd.StrUrlAcuseUbl = UrlAcuseUbl;
                        documento_bd.IntIdEstado = Convert.ToInt16(ProcesoEstado.RecepcionAcuse.GetHashCode());
                        documento_bd.DatFechaActualizaEstado = Fecha.GetFecha();

                        documento_bd = num_doc.Actualizar(documento_bd);
                        item_respuesta.codigoError = RespuestaInterOperabilidad.PendienteProcesamiento.GetHashCode().ToString();
                        item_respuesta.mensaje = string.Format("{0} en el documento {1}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.Enumerables.RespuestaInterOperabilidad>(Convert.ToInt16(item_respuesta.codigoError))), documento_obj.Documento);
                        item_respuesta.uuid = documento_bd.StrIdInteroperabilidad.ToString();
                    }
                    catch (Exception excepcion)
                    {

                        LogExcepcion.Guardar(excepcion);
                        item_respuesta.codigoError = RespuestaInterOperabilidad.ErrorInternoReceptor.GetHashCode().ToString();
                        item_respuesta.mensaje = string.Format("{0} en el documento {1}", Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.Enumerables.RespuestaInterOperabilidad>(Convert.ToInt16(item_respuesta.codigoError))), documento_obj.Documento);
                        throw new ApplicationException(string.Format("Error al actualizar el documento {0} Detalle: {1}", documento_obj.Documento, excepcion.Message));
                    }

                }
            }
            catch (Exception excepcion)
            {
                LogExcepcion.Guardar(excepcion);
            }

            return item_respuesta;

        }


        /// <summary>
        /// Genera documento a partir del Ubl
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="tipo_documento"></param>
        /// <returns></returns>
        public static dynamic ObtenerDocumento(string ruta, DocumentType tipo_documento)
        {

            try
            {
                // representación de datos en objeto
                var documento_obj = (dynamic)null;

                FileStream xml_reader = new FileStream(ruta, FileMode.Open);

                XmlSerializer serializacion = null;

                if (tipo_documento == DocumentType.FacturaNacional)
                {
                    serializacion = new XmlSerializer(typeof(InvoiceType));

                    InvoiceType conversion = (InvoiceType)serializacion.Deserialize(xml_reader);

                    documento_obj = FacturaXML.Convertir(conversion, true);

                    if (string.IsNullOrEmpty(documento_obj.DatosObligado.Email))
                        documento_obj.DatosObligado.Email = string.Empty;

                }
                else if (tipo_documento == DocumentType.NotaCredito)
                {
                    serializacion = new XmlSerializer(typeof(CreditNoteType));

                    CreditNoteType conversion = (CreditNoteType)serializacion.Deserialize(xml_reader);

                    documento_obj = NotaCreditoXML.Convertir(conversion, true);
                }
                else if (tipo_documento == DocumentType.NotaDebito)
                {
                    serializacion = new XmlSerializer(typeof(DebitNoteType));

                    DebitNoteType conversion = (DebitNoteType)serializacion.Deserialize(xml_reader);

                    documento_obj = NotaDebitoXML.Convertir(conversion, true);
                }
                else if (tipo_documento == DocumentType.AcuseDeRecibo)
                {
                    serializacion = new XmlSerializer(typeof(ApplicationResponseType));

                    ApplicationResponseType conversion = (ApplicationResponseType)serializacion.Deserialize(xml_reader);

                    documento_obj = AcuseXML.Convertir(conversion);
                }

                // cerrar la lectura del archivo xml
                xml_reader.Close();

                return documento_obj;
            }
            catch (Exception ex)
            {
                LogExcepcion.Guardar(ex);
                throw new ApplicationException(ex.Message, ex.InnerException);
            }


        }

        /// <summary>
        /// Creacion del Facturador Emisor si no existe con las respectiva Resolucion
        /// </summary>
        /// <param name="emisor"></param>
        /// <returns>Un objeto BD de Empresa</returns>
        public static TblEmpresas CrearFacturadorEmisor(dynamic documento_obj, int tipo_documento)
        {


            try
            {
                TblEmpresas facturador_emisor = new TblEmpresas();

                Ctl_Empresa empresa = new Ctl_Empresa();

                facturador_emisor = empresa.Obtener(documento_obj.DatosObligado.Identificacion);

                if (facturador_emisor == null)
                    facturador_emisor = empresa.Crear(documento_obj.DatosObligado, false);

                //Se crea Resolucion
                TblEmpresasResoluciones resolucion = new TblEmpresasResoluciones();

                Ctl_EmpresaResolucion ctl_resolucion = new Ctl_EmpresaResolucion();

                resolucion = ctl_resolucion.Obtener(documento_obj.DatosObligado.Identificacion, documento_obj.NumeroResolucion, documento_obj.Prefijo);

                if (resolucion == null)
                {

                    resolucion = Ctl_EmpresaResolucion.Convertir(documento_obj.DatosObligado.Identificacion, documento_obj.Prefijo, tipo_documento, documento_obj.NumeroResolucion);

                    // crea el registro en base de datos
                    resolucion = ctl_resolucion.Crear(resolucion);
                }

                return facturador_emisor;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        /// <summary>
        /// Convierte un objeto de Servicio en Objeto de BD
        /// </summary>
        /// <param name="documento">Objeto de Servicio</param>
        /// <param name="tipo_doc">Tipo del Documento</param>
        /// <returns>Objeto de BD</returns>
        public static TblDocumentos Convertir(object documento, DocumentType tipo_doc, TblEmpresas facturador_emisor, string nombre_archivo, string proveedor_emisor)
        {
            try
            {
                PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;



                // url pública
                //string url_ppal = LibreriaGlobalHGInet.Dms.ObtenerUrlPrincipal("", facturador_emisor.StrIdSeguridad.ToString());
                string url_ppal = string.Format("{0}\\{1}\\{2}", plataforma_datos.RutaDmsPublica, Constantes.RutaHGInetFacturaElectronica, facturador_emisor.StrIdSeguridad);

                // url pública del xml
                string UrlXmlUbl = string.Format(@"{0}{1}/{2}.xml", url_ppal, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian, nombre_archivo);

                // url pública del zip
                string url_ppal_zip = string.Format(@"{0}{1}/{2}.zip", url_ppal, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian, nombre_archivo);

                // url pública del pdf
                string url_ppal_pdf = string.Format(@"{0}{1}/{2}.pdf", url_ppal, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian, nombre_archivo);

                //Generacion del tracking
                Guid tracking = Guid.NewGuid();

                //Asigna a un objeto dinamico el objeto enviado por el usuario
                var documento_obj = (dynamic)null;
                documento_obj = documento;

                TblDocumentos tbl_documento = new TblDocumentos();

                tbl_documento.DatFechaIngreso = Fecha.GetFecha();
                tbl_documento.IntDocTipo = tipo_doc.GetHashCode();
                tbl_documento.IntNumero = documento_obj.Documento;
                tbl_documento.StrPrefijo = (!string.IsNullOrEmpty(documento_obj.Prefijo)) ? documento_obj.Prefijo : "";
                if (tipo_doc == DocumentType.NotaCredito || tipo_doc == DocumentType.NotaDebito)
                {
                    tbl_documento.DatFechaVencDocumento = documento_obj.Fecha;
                }
                else
                {
                    tbl_documento.DatFechaVencDocumento = documento_obj.FechaVence;
                }
                tbl_documento.DatFechaDocumento = documento_obj.Fecha;
                tbl_documento.StrObligadoIdRegistro = tracking.ToString();
                tbl_documento.StrNumResolucion = documento_obj.NumeroResolucion;
                tbl_documento.StrEmpresaFacturador = documento_obj.DatosObligado.Identificacion;
                tbl_documento.StrEmpresaAdquiriente = documento_obj.DatosAdquiriente.Identificacion;
                tbl_documento.StrCufe = documento_obj.Cufe;
                tbl_documento.IntVlrTotal = documento_obj.Total;
                tbl_documento.StrIdSeguridad = tracking;
                tbl_documento.IntAdquirienteRecibo = 0;
                tbl_documento.IntIdEstado = Convert.ToInt16(ProcesoEstado.EnvioEmailAcuse.GetHashCode());
                tbl_documento.DatFechaActualizaEstado = Fecha.GetFecha();
                tbl_documento.StrIdInteroperabilidad = tracking;
                tbl_documento.StrUrlArchivoUbl = UrlXmlUbl;
                tbl_documento.StrUrlArchivoPdf = url_ppal_pdf;
                tbl_documento.StrUrlArchivoZip = url_ppal_zip;
                tbl_documento.StrProveedorReceptor = Constantes.NitResolucionsinPrefijo;
                tbl_documento.StrProveedorEmisor = proveedor_emisor;

                return tbl_documento;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        /// <summary>
        /// Almacena el archivo Xml físicamente
        /// </summary>
        /// <param name="documento">datos del documento</param>
        /// <returns>datos del documento</returns>
        public static void AlmacenarArchivo(string ruta_archivo, string nombre_archivo, TblEmpresas facturador_emisor, bool documento_acuse = false)
        {
            try
            {

                PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

                // carpeta del facturador emisor
                string carpeta = string.Format("{0}\\{1}\\{2}", plataforma_datos.RutaDmsFisica, Constantes.RutaHGInetFacturaElectronica, facturador_emisor.StrIdSeguridad);

                if (!documento_acuse)
                {
                    carpeta = string.Format(@"{0}\{1}", carpeta, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaXmlAcuse);
                }
                else
                {
                    carpeta = string.Format(@"{0}\{1}", carpeta, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian);
                }

                // valida la existencia de la carpeta
                carpeta = Directorio.CrearDirectorio(carpeta);

                // Nombre del Archivo
                string archivo = nombre_archivo;

                // ruta del xml
                string ruta = string.Format(@"{0}\{1}", carpeta, archivo);

                //Copia el archivo de la ruta de origen a la ruta destino
                Archivo.CopiarArchivo(ruta_archivo, ruta);


            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }


        }

    }
}
