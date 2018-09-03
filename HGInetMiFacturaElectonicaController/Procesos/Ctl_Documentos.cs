using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HGInetFirmaDigital;
using HGInetMiFacturaElectonicaController.ServiciosDian;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.Properties;
using LibreriaGlobalHGInet.Formato;
using System.Text.RegularExpressions;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData;
using HGInetDIANServicios;
using HGInetDIANServicios.DianResolucion;
using HGInetMiFacturaElectonicaController.Properties;
using LibreriaGlobalHGInet.Enumerables;
using HGInetUBL;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using HGInetMiFacturaElectonicaData.Enumerables;

namespace HGInetMiFacturaElectonicaController.Procesos
{
    /// <summary>
    /// Controlador para gestionar los documentos
    /// </summary>
    public partial class Ctl_Documentos
    {
        /// <summary>
        /// Procesa una lista de documentos tipo Factura
        /// </summary>
        /// <param name="documentos">documentos tipo Factura</param>
        /// <returns></returns>
        public static List<DocumentoRespuesta> Procesar(List<Factura> documentos)
        {
            try
            {
                Ctl_Empresa Peticion = new Ctl_Empresa();

                //Válida que la key sea correcta.
                TblEmpresas facturador_electronico = Peticion.Validar(documentos.FirstOrDefault().DataKey, documentos.FirstOrDefault().DatosObligado.Identificacion);

                if (!facturador_electronico.IntObligado)
                    throw new ApplicationException(string.Format("Licencia inválida para la Identificacion {0}.", facturador_electronico.StrIdentificacion));

                // genera un id único de la plataforma
                Guid id_peticion = Guid.NewGuid();

                DateTime fecha_actual = Fecha.GetFecha();

                List<TblEmpresasResoluciones> lista_resolucion = new List<TblEmpresasResoluciones>();

                // sobre escribe los datos del facturador electrónico si se encuentra en estado de habilitación
                if (facturador_electronico.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
                {

                    Tercero DatosObligado = new Tercero()
                    {
                        Identificacion = "811021438",
                        IdentificacionDv = 4,
                        TipoIdentificacion = 31,
                        TipoPersona = 1,
                        Regimen = 2,
                        NombreComercial = "HGI",
                        Departamento = "Antioquia",
                        Ciudad = "Medellin",
                        Direccion = "Calle 48 Nro. 77C-06",
                        Telefono = "4444584",
                        Email = "info@hgi.com.co",
                        PaginaWeb = null,
                        CodigoPais = "CO",
                        RazonSocial = "HGI SAS",
                        PrimerApellido = null,
                        SegundoApellido = null,
                        PrimerNombre = null,
                        SegundoNombre = null
                    };

                    //Valida que Resolucion tomar, con Prefijo o sin Prefijo
                    string resolucion_pruebas = string.Empty;
                    string nit_resolucion = string.Empty;
                    string prefijo_prueba = string.Empty;
                    if (documentos.FirstOrDefault().Prefijo.Equals(string.Empty))
                    {
                        resolucion_pruebas = Constantes.ResolucionPruebas;
                        nit_resolucion = Constantes.NitResolucionsinPrefijo;

                    }
                    else
                    {
                        resolucion_pruebas = Constantes.ResolucionPruebas;
                        nit_resolucion = Constantes.NitResolucionconPrefijo;
                        prefijo_prueba = Constantes.PrefijoResolucionPruebas;
                    }



                    Ctl_EmpresaResolucion _resolucion = new Ctl_EmpresaResolucion();
                    lista_resolucion.Add(_resolucion.Obtener(nit_resolucion, resolucion_pruebas, prefijo_prueba));

                    foreach (var item in documentos)
                    {
                        item.NumeroResolucion = resolucion_pruebas;
                        item.DatosObligado = DatosObligado;
                        item.Prefijo = prefijo_prueba;

                    }
                }
                else
                {
                    // actualiza las resoluciones de los servicios web de la DIAN en la base de datos
                    lista_resolucion = Ctl_Resoluciones.Actualizar(id_peticion, documentos.FirstOrDefault().DatosObligado.Identificacion);
                }


                if (lista_resolucion == null)
                    throw new ApplicationException(string.Format("No se encontraron las resoluciones para el Facturador Electrónico '{0}'", facturador_electronico.StrIdentificacion));
                else if (!lista_resolucion.Any())
                    throw new ApplicationException(string.Format("No se encontraron las resoluciones para el Facturador Electrónico '{0}'", facturador_electronico.StrIdentificacion));


                List<DocumentoRespuesta> respuesta = new List<DocumentoRespuesta>();

                Parallel.ForEach<Factura>(documentos, item =>
                {
                    DocumentoRespuesta item_respuesta = Procesar(item, facturador_electronico, id_peticion, fecha_actual, lista_resolucion);
                    respuesta.Add(item_respuesta);
                });

                PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

                if (plataforma_datos.EnvioSms)
                {
                    int docs_proc = respuesta.Where(_x => _x.IdProceso > ProcesoEstado.Validacion.GetHashCode()).Count();

                    if (docs_proc > 0)
                    {

                        string hora = Fecha.GetFecha().ToString(Fecha.formato_hora);

                        string ambiente = Enumeracion.GetEnumObjectByValue<Habilitacion>(Convert.ToInt32(facturador_electronico.IntHabilitacion)).ToString();

                        int docs_ok = respuesta.Where(_x => _x.IdProceso == ProcesoEstado.EnvioEmailAcuse.GetHashCode()).Count();

                        int docs_pd = documentos.Count - docs_ok;

                        string mensaje_sms = hora + " " + "HGInetMiFacturaE " + facturador_electronico.StrIdentificacion + " " + facturador_electronico.StrRazonSocial
                            + " " + ambiente + " env= " + documentos.Count + " proc= " + docs_proc + " ok= " + docs_ok + " pd= " + docs_pd;

                        if (facturador_electronico.IntHabilitacion == Habilitacion.Produccion.GetHashCode())
                        {

                            List<string> celulares = Constantes.SmsCelulares.Split(',').ToList();

                            Ctl_Sms.Enviar(mensaje_sms, id_peticion.ToString(), celulares);
                        }
                        else
                        {
                            if (docs_pd > 0)
                            {
                                List<string> celulares = Constantes.SmsCelulares.Split(',').ToList();

                                Ctl_Sms.Enviar(mensaje_sms, id_peticion.ToString(), celulares);
                            }
                        }
                    }
                }

                return respuesta;
            }
            catch (Exception ex)
            {
                LogExcepcion.Guardar(ex);
                throw new ApplicationException(ex.Message);
            }
        }


        /// <summary>
        /// Procesa un documento por paralelismo
        /// </summary>
        /// <param name="item">objeto de datos factura</param>
        /// <param name="facturador_electronico">facturador electrónico del documento</param>
        /// <param name="id_peticion">identificador de petición</param>
        /// <param name="fecha_actual">fecha actual de recepción del documento</param>
        /// <param name="lista_resolucion">resoluciones habilitadas para el facturador electrónico</param>
        /// <returns>resultado del proceso</returns>
        private static DocumentoRespuesta Procesar(Factura item, TblEmpresas facturador_electronico, Guid id_peticion, DateTime fecha_actual, List<TblEmpresasResoluciones> lista_resolucion)
        {
            DocumentoRespuesta item_respuesta = new DocumentoRespuesta();
            try
            {
                if (string.IsNullOrEmpty(item.NumeroResolucion))
                    throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "NumeroResolucion", "string"));


                Ctl_Documento num_doc = new Ctl_Documento();

                //valida si el Documento ya existe en Base de Datos
                TblDocumentos numero_documento = num_doc.Obtener(item.NumeroResolucion, item.Documento, TipoDocumento.Factura.GetHashCode(), item.Prefijo);

                if (numero_documento != null && item.Prefijo == numero_documento.StrPrefijo)
                    throw new ApplicationException(string.Format("El documento {0} ya existe para el Facturador Electrónico {1}", item.Documento, facturador_electronico.StrIdentificacion));

                TblEmpresasResoluciones resolucion = null;

                try
                {
                    ApplicationException exTMP = new ApplicationException(string.Format("DataRes: {0}", lista_resolucion.FirstOrDefault().StrIdSeguridad));

                    LogExcepcion.Guardar(exTMP);

                    // filtra la resolución del documento
                    resolucion = lista_resolucion.Where(_resolucion_doc => _resolucion_doc.StrEmpresa.Equals(item.DatosObligado.Identificacion)
                                                                            && _resolucion_doc.StrPrefijo.Equals(item.Prefijo)
                                                                            && _resolucion_doc.StrNumResolucion.Equals(item.NumeroResolucion)).FirstOrDefault();

                }
                catch (Exception excepcion)
                {
                    throw new ApplicationException(string.Format("No se encontró la resolución {0} para el Facturador Electrónico {1}", item.NumeroResolucion, facturador_electronico.StrIdentificacion));
                }

                if (resolucion == null)
                    throw new ApplicationException(string.Format("No se encontró la resolución {0} para el Facturador Electrónico {1} con prefijo '{2}'", item.NumeroResolucion, facturador_electronico.StrIdentificacion, item.Prefijo));



                // realiza el proceso de envío a la DIAN del documento
                item_respuesta = Procesar(id_peticion, item, TipoDocumento.Factura, resolucion, facturador_electronico);
            }
            catch (Exception excepcion)
            {

                ProcesoEstado proceso_actual = ProcesoEstado.Recepcion;
                LogExcepcion.Guardar(excepcion);
                item_respuesta = new DocumentoRespuesta()
                {
                    Aceptacion = 0,
                    CodigoRegistro = item.CodigoRegistro,
                    Cufe = "",
                    DescripcionProceso = Enumeracion.GetDescription(proceso_actual),
                    DocumentoTipo = TipoDocumento.Factura.GetHashCode(),
                    Documento = item.Documento,
                    Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error al procesar el documento. Detalle: {0} ", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.ERROR_NO_CONTROLADO, excepcion.InnerException),
                    EstadoDian = null,
                    FechaRecepcion = fecha_actual,
                    FechaUltimoProceso = fecha_actual,
                    IdDocumento = "",
                    Identificacion = "",
                    IdProceso = proceso_actual.GetHashCode(),
                    MotivoRechazo = "",
                    NumeroResolucion = item.NumeroResolucion,
                    Prefijo = item.Prefijo,
                    ProcesoFinalizado = (proceso_actual == ProcesoEstado.Finalizacion || proceso_actual == ProcesoEstado.FinalizacionErrorDian) ? (1) : 0,
                    UrlPdf = "",
                    UrlXmlUbl = ""
                };

            }
            if (item_respuesta.Error == null)
                item_respuesta.Error = new LibreriaGlobalHGInet.Error.Error();

            return item_respuesta;

        }

        /// <summary>
        /// Realiza el proceso en la plataforma para un documento
        /// 1. Generar UBL - 2. Firmar - 3. Almacenar XML - 4. Comprimir - 5. Enviar DIAN - 6. Envío Adquiriente
        /// </summary>
        /// <param name="id_peticion">id único de identificación de la plataforma</param>
        /// <param name="documento">datos del documento</param>
        /// <param name="tipo_doc">tipo de documento a procesar</param>
        /// <param name="resolucion">resolución del documento</param>
        /// <param name="empresa">facturador electrónico del documento</param>
        /// <returns>resultado del proceso</returns>
        public static DocumentoRespuesta Procesar(Guid id_peticion, object documento, TipoDocumento tipo_doc, TblEmpresasResoluciones resolucion, TblEmpresas empresa)
        {
            string numero_resolucion = string.Empty;
            string prefijo = string.Empty;

            var documento_obj = (dynamic)null;
            documento_obj = documento;

            if (documento_obj != null)
            {

                DateTime fecha_actual = Fecha.GetFecha();

                FacturaE_Documento documento_result = new FacturaE_Documento();

                DocumentoRespuesta respuesta = new DocumentoRespuesta()
                {
                    Aceptacion = 0,
                    CodigoRegistro = documento_obj.CodigoRegistro,
                    Cufe = "",
                    DescripcionProceso = "Recepción - Información del documento.",
                    DocumentoTipo = tipo_doc.GetHashCode(),
                    Documento = documento_obj.Documento,
                    Error = null,
                    FechaRecepcion = fecha_actual,
                    FechaUltimoProceso = fecha_actual,
                    IdDocumento = Guid.NewGuid().ToString(),
                    Identificacion = documento_obj.DatosAdquiriente.Identificacion,
                    IdProceso = ProcesoEstado.Recepcion.GetHashCode(),
                    MotivoRechazo = "",
                    NumeroResolucion = documento_obj.NumeroResolucion,
                    Prefijo = documento_obj.Prefijo,
                    ProcesoFinalizado = 0,
                    UrlPdf = "",
                    UrlXmlUbl = ""
                };

                try
                {

                    // valida la información del documento
                    respuesta = Validar(documento_obj, tipo_doc, resolucion, ref respuesta);
                    ValidarRespuesta(respuesta);


                    if (empresa.IntHabilitacion > Habilitacion.Valida_Objeto.GetHashCode())
                    {

                        //Guarda la id de la Peticion con la que se esta haciendo el proceso
                        documento_result.IdSeguridadPeticion = id_peticion;

                        //Guarda el Id del documento generado por la plataforma
                        documento_result.IdSeguridadDocumento = Guid.Parse(respuesta.IdDocumento);

                        Ctl_Documento documento_tmp = new Ctl_Documento();

                        //guarda documento en BD
                        TblDocumentos documentoBd = Ctl_Documento.Convertir(respuesta, documento_obj, resolucion, empresa, tipo_doc);

                        // genera el xml en ubl
                        respuesta = UblGenerar(documento_obj, tipo_doc, resolucion, documentoBd, empresa, ref respuesta, ref documento_result);
                        ValidarRespuesta(respuesta);

                        // almacena el xml en ubl
                        respuesta = UblGuardar(documentoBd, ref respuesta, ref documento_result);
                        ValidarRespuesta(respuesta);


                        Ctl_Empresa empresa_config = new Ctl_Empresa();

                        TblEmpresas adquirienteBd = null;

                        //Validacion de Adquiriente
                        try
                        {

                            //Obtiene la informacion del Adquiriente que se tiene en BD
                            adquirienteBd = empresa_config.Obtener(documento_obj.DatosAdquiriente.Identificacion);
                            try
                            {

                                //Si no existe Adquiriente se crea en BD y se crea Usuario
                                if (adquirienteBd == null)
                                {
                                    empresa_config = new Ctl_Empresa();
                                    //Creacion del Adquiriente
                                    adquirienteBd = empresa_config.Crear(documento_obj.DatosAdquiriente);

                                }
                            }
                            catch (Exception excepcion)
                            {
                                string msg_excepcion = Excepcion.Mensaje(excepcion);

                                if (!msg_excepcion.ToLowerInvariant().Contains("insert duplicate key"))
                                    throw excepcion;
                                else
                                    adquirienteBd = empresa_config.Obtener(documento_obj.DatosAdquiriente.Identificacion);
                            }
                        }
                        catch (Exception excepcion)
                        {
                            respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error al obtener el Adquiriente Detalle. Detalle: ", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.ERROR_LICENCIA, excepcion.InnerException);
                            LogExcepcion.Guardar(excepcion);
                            throw excepcion;
                        }

                        //Crea el documento en BD
                        try
                        {

                            documentoBd = documento_tmp.Crear(documentoBd);

                            documentoBd.TblEmpresasResoluciones = resolucion;


                        }
                        catch (Exception excepcion)
                        {
                            respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error al guardar el documento. Detalle: {0} ", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.ERROR_LICENCIA, excepcion.InnerException);
                            LogExcepcion.Guardar(excepcion);
                            throw excepcion;
                        }

                        //Asignación de Cufe a documento_obj 
                        documento_obj.Cufe = documento_result.CUFE;

                        // almacena Formato
                        respuesta = GuardarFormato(documento_obj, documentoBd, ref respuesta, ref documento_result);
                        ValidarRespuesta(respuesta);


                        // firma el xml
                        respuesta = UblFirmar(documentoBd, ref respuesta, ref documento_result);
                        ValidarRespuesta(respuesta);


                        // comprime el archivo xml firmado                        
                        respuesta = UblComprimir(documentoBd, ref respuesta, ref documento_result);
                        ValidarRespuesta(respuesta);


                        // envía el archivo zip con el xml firmado a la DIAN
                        HGInetDIANServicios.DianFactura.AcuseRecibo acuse = EnviarDian(documentoBd, empresa, ref respuesta, ref documento_result);
                        ValidarRespuesta(respuesta);


                        //Valida estado del documento en la Plataforma de la DIAN
                        respuesta = Consultar(documentoBd, empresa, ref respuesta);


                        // envía el mail de documentos al adquiriente
                        if (respuesta.EstadoDian.EstadoDocumento == EstadoDocumentoDian.Aceptado.GetHashCode())
                        {
                            respuesta = Envio(documento_obj, documentoBd, empresa, ref respuesta, ref documento_result);
                            ValidarRespuesta(respuesta);
                        }

                    }


                }
                catch (Exception excepcion)
                {
                    LogExcepcion.Guardar(excepcion);
                    // no se controla excepción
                }

                return respuesta;
            }
            throw new ArgumentException("No se recibieron datos para realizar el proceso");
        }

        /// <summary>
        /// Procesa una lista de documentos tipo NotaCredito
        /// </summary>
        /// <param name="documentos">documentos tipo NotaCredito</param>
        /// <returns></returns>
        public static List<DocumentoRespuesta> Procesar(List<NotaCredito> documentos)
        {

            Ctl_Empresa Peticion = new Ctl_Empresa();

            //Válida que la key sea correcta.
            TblEmpresas facturador_electronico = Peticion.Validar(documentos.FirstOrDefault().DataKey, documentos.FirstOrDefault().DatosObligado.Identificacion);

            if (!facturador_electronico.IntObligado)
                throw new ApplicationException(string.Format("Licencia inválida para la Identificacion {0}.", facturador_electronico.StrIdentificacion));

            // genera un id único de la plataforma
            Guid id_peticion = Guid.NewGuid();

            DateTime fecha_actual = Fecha.GetFecha();

            Ctl_EmpresaResolucion _resolucion = new Ctl_EmpresaResolucion();

            List<TblEmpresasResoluciones> lista_resolucion = new List<TblEmpresasResoluciones>();

            string resolucion_pruebas = Constantes.ResolucionPruebas;
            string nit_resolucion = Constantes.NitResolucionsinPrefijo;
            string prefijo_pruebas = string.Empty;

            // sobre escribe los datos del facturador electrónico si se encuentra en estado de habilitación
            if (facturador_electronico.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
            {

                Tercero DatosObligado = new Tercero()
                {
                    Identificacion = "811021438",
                    IdentificacionDv = 4,
                    TipoIdentificacion = 31,
                    TipoPersona = 1,
                    Regimen = 2,
                    NombreComercial = "HGI",
                    Departamento = "Antioquia",
                    Ciudad = "Medellin",
                    Direccion = "Calle 48 Nro. 77C-06",
                    Telefono = "4444584",
                    Email = "info@hgi.com.co",
                    PaginaWeb = null,
                    CodigoPais = "CO",
                    RazonSocial = "HGI SAS",
                    PrimerApellido = null,
                    SegundoApellido = null,
                    PrimerNombre = null,
                    SegundoNombre = null
                };

                //obtiene la resolucion de factura de pruebas
                lista_resolucion.Add(_resolucion.Obtener(nit_resolucion, resolucion_pruebas, prefijo_pruebas));

                foreach (var item in documentos)
                {
                    item.NumeroResolucion = resolucion_pruebas;
                    item.DatosObligado = DatosObligado;

                }
            }
            else
            {
                // Obtiene las resoluciones de la base de datos
                lista_resolucion = _resolucion.ObtenerResoluciones(documentos.FirstOrDefault().DatosObligado.Identificacion, "*");

            }

            List<DocumentoRespuesta> respuesta = new List<DocumentoRespuesta>();

            TblEmpresasResoluciones resolucion = new TblEmpresasResoluciones();

            foreach (NotaCredito item in documentos)
            {

                DocumentoRespuesta item_respuesta = new DocumentoRespuesta();

                try
                {

                    Ctl_Documento num_doc = new Ctl_Documento();

                    //valida si el Documento ya existe en Base de Datos
                    TblDocumentos numero_documento = num_doc.Obtener(item.DatosObligado.Identificacion, item.Documento, TipoDocumento.NotaCredito.GetHashCode(), item.Prefijo);

                    if (numero_documento != null)
                        throw new ApplicationException(string.Format("El documento '{0}' con prefijo '{1}' ya xiste para el Facturador Electrónico {2}", item.Documento, item.Prefijo, facturador_electronico.StrIdentificacion));

                    // filtra la resolución del documento con las condiciones de nit, prefijo y tipo de documento
                    TblEmpresasResoluciones resolucion_doc = lista_resolucion.Where(_resolucion_doc => _resolucion_doc.StrEmpresa.Equals(item.DatosObligado.Identificacion) &&
                                                                                    _resolucion_doc.StrPrefijo.Equals(item.Prefijo)
                                                                                    && _resolucion_doc.IntTipoDoc == TipoDocumento.NotaCredito.GetHashCode()).FirstOrDefault();
                    //si no existe la resolucion la crea
                    if (resolucion_doc == null)
                    {
                        //Se crea Resolucion
                        TblEmpresasResoluciones tbl_resolucion = new TblEmpresasResoluciones();
                        if (facturador_electronico.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
                        {
                            tbl_resolucion = Ctl_EmpresaResolucion.Convertir(facturador_electronico.StrIdentificacion, documentos.FirstOrDefault().Prefijo, TipoDocumento.NotaCredito.GetHashCode());
                        }
                        else
                        {
                            tbl_resolucion = Ctl_EmpresaResolucion.Convertir(documentos.FirstOrDefault().DatosObligado.Identificacion, documentos.FirstOrDefault().Prefijo, TipoDocumento.NotaCredito.GetHashCode());
                        }
                        // crea el registro en base de datos
                        tbl_resolucion = _resolucion.Crear(tbl_resolucion);
                        lista_resolucion.Add(tbl_resolucion);
                        resolucion = tbl_resolucion;
                        item.NumeroResolucion = resolucion.StrNumResolucion;
                    }
                    else
                    {
                        resolucion = resolucion_doc;
                        item.NumeroResolucion = resolucion.StrNumResolucion;
                    }

                    // realiza el proceso de envío a la DIAN del documento
                    item_respuesta = Procesar(id_peticion, item, TipoDocumento.NotaCredito, resolucion, facturador_electronico);

                }
                catch (Exception excepcion)
                {

                    ProcesoEstado proceso_actual = ProcesoEstado.Recepcion;
                    LogExcepcion.Guardar(excepcion);
                    item_respuesta = new DocumentoRespuesta()
                    {
                        Aceptacion = 0,
                        CodigoRegistro = item.CodigoRegistro,
                        Cufe = "",
                        DescripcionProceso = Enumeracion.GetDescription(proceso_actual),
                        DocumentoTipo = TipoDocumento.NotaCredito.GetHashCode(),
                        Documento = item.Documento,
                        Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error al procesar el documento. Detalle: {0} ", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.ERROR_NO_CONTROLADO, excepcion.InnerException),
                        EstadoDian = null,
                        FechaRecepcion = fecha_actual,
                        FechaUltimoProceso = fecha_actual,
                        IdDocumento = "",
                        Identificacion = "",
                        IdProceso = proceso_actual.GetHashCode(),
                        MotivoRechazo = "",
                        NumeroResolucion = item.NumeroResolucion,
                        Prefijo = "",
                        ProcesoFinalizado = (proceso_actual == ProcesoEstado.Finalizacion || proceso_actual == ProcesoEstado.FinalizacionErrorDian) ? (1) : 0,
                        UrlPdf = "",
                        UrlXmlUbl = ""
                    };
                }
                if (item_respuesta.Error == null)
                    item_respuesta.Error = new LibreriaGlobalHGInet.Error.Error();

                respuesta.Add(item_respuesta);
            }

            return respuesta;

        }

        /// <summary>
        /// Procesa una lista de documentos tipo NotaDebito
        /// </summary>
        /// <param name="documentos">documentos tipo NotaDebito</param>
        /// <returns></returns>
        public static List<DocumentoRespuesta> Procesar(List<NotaDebito> documentos)
        {
            Ctl_Empresa Peticion = new Ctl_Empresa();

            //Válida que la key sea correcta.
            TblEmpresas facturador_electronico = Peticion.Validar(documentos.FirstOrDefault().DataKey, documentos.FirstOrDefault().DatosObligado.Identificacion);

            if (!facturador_electronico.IntObligado)
                throw new ApplicationException(string.Format("Licencia inválida para la Identificacion {0}.", facturador_electronico.StrIdentificacion));

            // genera un id único de la plataforma
            Guid id_peticion = Guid.NewGuid();

            DateTime fecha_actual = Fecha.GetFecha();

            Ctl_EmpresaResolucion _resolucion = new Ctl_EmpresaResolucion();

            List<TblEmpresasResoluciones> lista_resolucion = new List<TblEmpresasResoluciones>();

            string resolucion_pruebas = Constantes.ResolucionPruebas;
            string nit_resolucion = Constantes.NitResolucionsinPrefijo;
            string prefijo_pruebas = string.Empty;

            // sobre escribe los datos del facturador electrónico si se encuentra en estado de habilitación
            if (facturador_electronico.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
            {

                Tercero DatosObligado = new Tercero()
                {
                    Identificacion = "811021438",
                    IdentificacionDv = 4,
                    TipoIdentificacion = 31,
                    TipoPersona = 1,
                    Regimen = 2,
                    NombreComercial = "HGI",
                    Departamento = "Antioquia",
                    Ciudad = "Medellin",
                    Direccion = "Calle 48 Nro. 77C-06",
                    Telefono = "4444584",
                    Email = "info@hgi.com.co",
                    PaginaWeb = null,
                    CodigoPais = "CO",
                    RazonSocial = "HGI SAS",
                    PrimerApellido = null,
                    SegundoApellido = null,
                    PrimerNombre = null,
                    SegundoNombre = null
                };

                //obtiene la resolucion de factura de pruebas
                lista_resolucion.Add(_resolucion.Obtener(nit_resolucion, resolucion_pruebas, prefijo_pruebas));

                foreach (var item in documentos)
                {
                    item.NumeroResolucion = resolucion_pruebas;
                    item.DatosObligado = DatosObligado;

                }
            }
            else
            {
                // Obtiene las resoluciones de la base de datos
                lista_resolucion = _resolucion.ObtenerResoluciones(documentos.FirstOrDefault().DatosObligado.Identificacion, "*");

            }

            List<DocumentoRespuesta> respuesta = new List<DocumentoRespuesta>();

            TblEmpresasResoluciones resolucion = new TblEmpresasResoluciones();

            foreach (NotaDebito item in documentos)
            {

                DocumentoRespuesta item_respuesta = new DocumentoRespuesta();

                try
                {


                    Ctl_Documento num_doc = new Ctl_Documento();

                    //valida si el Documento ya existe en Base de Datos
                     TblDocumentos numero_documento = num_doc.Obtener(item.DatosObligado.Identificacion, item.Documento, TipoDocumento.NotaDebito.GetHashCode(), item.Prefijo);

                    if (numero_documento != null)
                        throw new ApplicationException(string.Format("El documento '{0}' con prefijo '{1}' ya xiste para el Facturador Electrónico {2}", item.Documento, item.Prefijo, facturador_electronico.StrIdentificacion));

                    // filtra la resolución del documento con las condiciones de nit, prefijo y tipo de documento
                    TblEmpresasResoluciones resolucion_doc = lista_resolucion.Where(_resolucion_doc => _resolucion_doc.StrEmpresa.Equals(item.DatosObligado.Identificacion) &&
                                                                                    _resolucion_doc.StrPrefijo.Equals(item.Prefijo)
                                                                                    && _resolucion_doc.IntTipoDoc == TipoDocumento.NotaDebito.GetHashCode()).FirstOrDefault();
                    //si no existe la resolucion la crea
                    if (resolucion_doc == null)
                    {
                        //Se crea Resolucion
                        TblEmpresasResoluciones tbl_resolucion = new TblEmpresasResoluciones();
                        tbl_resolucion = Ctl_EmpresaResolucion.Convertir(documentos.FirstOrDefault().DatosObligado.Identificacion, documentos.FirstOrDefault().Prefijo, TipoDocumento.NotaDebito.GetHashCode());

                        // crea el registro en base de datos
                        tbl_resolucion = _resolucion.Crear(tbl_resolucion);
                        lista_resolucion.Add(tbl_resolucion);
                        resolucion = tbl_resolucion;
                        item.NumeroResolucion = resolucion.StrNumResolucion;
                    }
                    else
                    {
                        resolucion = resolucion_doc;
                        item.NumeroResolucion = resolucion.StrNumResolucion;
                    }

                    // realiza el proceso de envío a la DIAN del documento
                    item_respuesta = Procesar(id_peticion, item, TipoDocumento.NotaDebito, resolucion, facturador_electronico);

                }
                catch (Exception excepcion)
                {

                    ProcesoEstado proceso_actual = ProcesoEstado.Recepcion;
                    LogExcepcion.Guardar(excepcion);
                    item_respuesta = new DocumentoRespuesta()
                    {
                        Aceptacion = 0,
                        CodigoRegistro = item.CodigoRegistro,
                        Cufe = "",
                        DescripcionProceso = Enumeracion.GetDescription(proceso_actual),
                        DocumentoTipo = TipoDocumento.NotaDebito.GetHashCode(),
                        Documento = item.Documento,
                        Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error al procesar el documento. Detalle: {0}", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.ERROR_NO_CONTROLADO, excepcion.InnerException),
                        EstadoDian = null,
                        FechaRecepcion = fecha_actual,
                        FechaUltimoProceso = fecha_actual,
                        IdDocumento = "",
                        Identificacion = "",
                        IdProceso = proceso_actual.GetHashCode(),
                        MotivoRechazo = "",
                        NumeroResolucion = item.NumeroResolucion,
                        Prefijo = "",
                        ProcesoFinalizado = (proceso_actual == ProcesoEstado.Finalizacion || proceso_actual == ProcesoEstado.FinalizacionErrorDian) ? (1) : 0,
                        UrlPdf = "",
                        UrlXmlUbl = ""
                    };
                }
                if (item_respuesta.Error == null)
                    item_respuesta.Error = new LibreriaGlobalHGInet.Error.Error();

                respuesta.Add(item_respuesta);
            }

            return respuesta;

        }


        /// <summary>
        /// Validación del Objeto Factura
        /// </summary>
        /// <param name="documento">Objeto factura</param>
        /// <returns></returns>
        public static Factura Validar(Factura documento, TblEmpresasResoluciones resolucion)
        //public static Factura Validar(Factura documento)
        {
            // valida objeto recibido
            if (documento == null)
                throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "documento", "Factura"));

            //valida que no este vacio y existencia
            if (string.IsNullOrEmpty(documento.CodigoRegistro))
                throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "CodigoRegistro", "string"));

            //valida que no este vacio y existencia
            if (string.IsNullOrEmpty(documento.DataKey))
                throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "DataKey", "string"));

            // valida el número del documento no sea valor negativo
            if (documento.Documento < 0)
                throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "Documento", "int").Replace("no puede ser nulo", "no puede ser menor a 0"));

            //Inicializa la propiedad, no es un campo requerido
            if (string.IsNullOrEmpty(documento.DocumentoRef))
                documento.DocumentoRef = string.Empty;

            //setea el campo y lo deja en blanco
            documento.Cufe = string.Empty;

            //Validar que no este vacio y este vigente en los terminos.
            if (string.IsNullOrEmpty(documento.NumeroResolucion))
                throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "NumeroResolucion", "string"));

            //valida resolucion
            if (!resolucion.StrNumResolucion.Equals(documento.NumeroResolucion))
                throw new ApplicationException(string.Format("El número de resolución '{0}' no es válido", documento.NumeroResolucion));

            //valida número de la Factura este entre los rangos
            if (documento.Documento < resolucion.IntRangoInicial || documento.Documento > resolucion.IntRangoFinal)
                throw new ApplicationException(string.Format("El número del documento '{0}' no es válido según la resolución", documento.Documento));

            if (!resolucion.StrPrefijo.Equals(documento.Prefijo))
                throw new ApplicationException(string.Format("El prefijo '{0}' no es válido según Resolución", documento.Prefijo));

            //Valida que la fecha este en los terminos
            if (documento.Fecha.Date < Fecha.GetFecha().AddDays(-2).Date || documento.Fecha.Date > Fecha.GetFecha().Date)
                throw new ApplicationException(string.Format("La fecha de elaboración '{0}' no está dentro los términos.", documento.Fecha));

            if (documento.FechaVence.Date < documento.Fecha.Date)
                throw new ApplicationException(string.Format("La fecha de vencimiento '{0}' debe ser mayor o igual a la fecha de elaboración del documento '{1}'", documento.FechaVence, documento.Fecha));

            //Valida que no este vacio y Formato
            if (string.IsNullOrEmpty(documento.Moneda))
                throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "Moneda", "string"));

            if (!ConfiguracionRegional.ValidarCodigoMoneda(documento.Moneda))
                throw new ArgumentException(string.Format("No se encuentra registrado {0} con valor '{1}' según ISO 4217", "Moneda", documento.Moneda));

            //Valida que el codigo del formato que envia este disponible.
            if (string.IsNullOrEmpty(documento.DocumentoFormato.ArchivoPdf))
            {
                if (documento.DocumentoFormato.Codigo < 1 || documento.DocumentoFormato.Codigo > 4)
                    throw new ApplicationException(string.Format("El Formato '{0}' no se encuentra disponible en la plataforma.", documento.DocumentoFormato.Codigo));
            }

            //Valida que no este vacio y este bien formado 
            ValidarTercero(documento.DatosObligado, "Obligado");

            //Valida que no este vacio y este bien formado 
            ValidarTercero(documento.DatosAdquiriente, "Adquiriente");

            //Valida totales del objeto
            ValidarTotales(documento, null, null, TipoDocumento.Factura);

            return documento;
        }


        /// <summary>
        /// Validación del Objeto Nota Credito
        /// </summary>
        /// <param name="documento">Objeto NotaCredito</param>
        /// <returns></returns>
        public static NotaCredito ValidarNotaCredito(NotaCredito documento, TblEmpresasResoluciones resolucion)
        {
            // valida objeto recibido
            if (documento == null)
                throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "documento", "Nota Crédito"));

            //valida que no este vacio y existencia
            if (string.IsNullOrEmpty(documento.CodigoRegistro))
                throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "CodigoRegistro", "string"));

            //valida que no este vacio y existencia
            if (string.IsNullOrEmpty(documento.DataKey))
                throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "DataKey", "string"));

            // validar el número del documento y validez con resolucion
            if (documento.Documento < 0)
                throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "Documento", "int").Replace("no puede ser nulo", "no puede ser menor a 0"));

            //Validar que no este vacio
            if (string.IsNullOrEmpty(documento.DocumentoRef))
                throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "DocumentoRef", "string"));

            //Validar que no este vacia la fecha del documento de referencia
            if (documento.FechaFactura == null)
                throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "FechaFactura", "DateTime"));

            //valida resolucion
            if (!resolucion.StrNumResolucion.Equals(documento.NumeroResolucion))
                throw new ApplicationException(string.Format("El número de resolución '{0}' no es válido", documento.NumeroResolucion));

            //valida el prefijo si es null lo llena vacio
            /*if (string.IsNullOrEmpty(documento.Prefijo))
				documento.Prefijo = string.Empty;*/

            //Validar que no este vacia la fecha
            if (documento.Fecha == null)
                throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "Fecha", "DateTime"));

            //Valida que no este vacio el concepto
            if (documento.Concepto == null)
                throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "Concepto", "string"));

            //Valida que la fecha este en los terminos
            if (documento.Fecha.Date < Fecha.GetFecha().AddDays(-2).Date || documento.Fecha.Date > Fecha.GetFecha().Date)
                throw new ApplicationException(string.Format("La fecha de elaboración '{0}' no está dentro los términos.", documento.Fecha));

            //Valida que no este vacio y Formato
            if (string.IsNullOrEmpty(documento.Moneda))
                throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "Moneda", "string"));

            if (!ConfiguracionRegional.ValidarCodigoMoneda(documento.Moneda))
                throw new ArgumentException(string.Format("No se encuentra registrado {0} con valor '{1}' según ISO 4217", "Moneda", documento.Moneda));

            //Valida que el codigo del formato que envia este disponible.
            if (string.IsNullOrEmpty(documento.DocumentoFormato.ArchivoPdf))
            {
                if (documento.DocumentoFormato.Codigo < 1 || documento.DocumentoFormato.Codigo > 4)
                    throw new ApplicationException(string.Format("El formato '{0}' no se encuentra disponible en la plataforma.", documento.DocumentoFormato.Codigo));
            }

            //Valida que no este vacio y este bien formado 
            ValidarTercero(documento.DatosObligado, "Obligado");

            //Valida que no este vacio y este bien formado 
            ValidarTercero(documento.DatosAdquiriente, "Adquiriente");

            //Valida totales del objeto
            ValidarTotales(null, documento, null, TipoDocumento.NotaCredito);

            return documento;
        }

        /// <summary>
        /// Validación del Objeto Nota Debito
        /// </summary>
        /// <param name="documento">Objeto NotaDebito</param>
        /// <returns></returns>
        public static NotaDebito ValidarNotaDebito(NotaDebito documento, TblEmpresasResoluciones resolucion)
        {
            // valida objeto recibido
            if (documento == null)
                throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "documento", "Nota Débito"));

            //valida que no este vacio y existencia
            if (string.IsNullOrEmpty(documento.CodigoRegistro))
                throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "CodigoRegistro", "string"));

            //valida que no este vacio y existencia
            if (string.IsNullOrEmpty(documento.DataKey))
                throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "DataKey", "string"));

            // validar el número del documento y validez con resolucion
            if (documento.Documento < 0)
                throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "Documento", "int").Replace("no puede ser nulo", "no puede ser menor a 0"));

            //Validar que no este vacio
            if (string.IsNullOrEmpty(documento.DocumentoRef))
                throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "DocumentoRef", "string"));

            //Validar que no este vacia la fecha del documento de referencia
            if (documento.FechaFactura == null)
                throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "FechaFactura", "DateTime"));

            //valida resolucion
            if (!resolucion.StrNumResolucion.Equals(documento.NumeroResolucion))
                throw new ApplicationException(string.Format("La Resolución {0} no es valida", documento.NumeroResolucion));

            //Validar que no este vacia la fecha
            if (documento.Fecha == null)
                throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "Fecha", "DateTime"));

            //Valida que no este vacio el concepto
            if (documento.Concepto == null)
                throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "Concepto", "string"));

            //Valida que la fecha este en los terminos
            if (documento.Fecha.Date < Fecha.GetFecha().AddDays(-2).Date || documento.Fecha.Date > Fecha.GetFecha().Date)
                throw new ApplicationException(string.Format("La fecha {0} no esta dentro los terminos.", documento.Fecha));

            //Valida que no este vacio y Formato
            if (string.IsNullOrEmpty(documento.Moneda))
                throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "Moneda", "string"));

            if (!ConfiguracionRegional.ValidarCodigoMoneda(documento.Moneda))
                throw new ArgumentException(string.Format("No se encuentra registrado {0} con valor {1} según ISO 4217", "Moneda", documento.Moneda));

            //Valida que el codigo del formato que envia este disponible.
            if (string.IsNullOrEmpty(documento.DocumentoFormato.ArchivoPdf))
            {
                if (documento.DocumentoFormato.Codigo < 1 || documento.DocumentoFormato.Codigo > 4)
                    throw new ApplicationException(string.Format("El Formato {0} no se encuentra disponible en la plataforma.", documento.DocumentoFormato.Codigo));
            }

            //Valida que no este vacio y este bien formado 
            ValidarTercero(documento.DatosObligado, "Obligado");

            //Valida que no este vacio y este bien formado 
            ValidarTercero(documento.DatosAdquiriente, "Adquiriente");

            //Valida totales del objeto
            ValidarTotales(null, null, documento, TipoDocumento.NotaDebito);

            return documento;
        }

        /// <summary>
        /// Validacion del objeto tercero
        /// </summary>
        /// <param name="tercero">Objeto</param>
        /// <param name="tipo">Tipo de Tercero: Adquiriente - Obligado</param>
        public static void ValidarTercero(Tercero tercero, string tipo)
        {

            if (tercero == null)
                throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "Tercero", tipo).Replace("de tipo", "del"));

            //valida que la identificacion no contenga caracteres especiales
            //Regex isnumber = new Regex("[^0-9]");
            if (!string.IsNullOrEmpty(tercero.Identificacion))
            {
                if (!Texto.ValidarExpresion(TipoExpresion.Numero, tercero.Identificacion) && !Texto.ValidarExpresion(TipoExpresion.Alfanumerico, tercero.Identificacion))
                    throw new ArgumentException(string.Format("El parámetro {0} del {1} no puede contener caracteres especiales", "Identificacion", tipo));
            }
            else
                throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "Identificacion", tipo).Replace("de tipo", "del"));

            if ((tercero.IdentificacionDv < 0) || (tercero.IdentificacionDv > 9))
                throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "IdentificacionDv", tipo).Replace("de tipo", "del"));

            if (string.IsNullOrEmpty(tercero.Ciudad))
                throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "Ciudad", tipo).Replace("de tipo", "del"));

            if (string.IsNullOrEmpty(tercero.Departamento))
                throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "Departamento", tipo).Replace("de tipo", "del"));

            if (string.IsNullOrEmpty(tercero.Direccion))
                throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "Direccion", tipo).Replace("de tipo", "del"));

            if (string.IsNullOrEmpty(tercero.Telefono))
                throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "Telefono", tipo).Replace("de tipo", "del"));

            //Regex ismail = new Regex("\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*");
            if (!Texto.ValidarExpresion(TipoExpresion.Email, tercero.Email))
                throw new ArgumentException(string.Format("El parámetro {0} del {1} no esta bien formado", "Email", tipo));

            //Regex isweb = new Regex("([\\w-]+\\.)+(/[\\w- ./?%&=]*)?");
            if (tercero.PaginaWeb == null)
            {
                tercero.PaginaWeb = string.Empty;
            }
            else if (!Texto.ValidarExpresion(TipoExpresion.PaginaWeb, tercero.PaginaWeb))
                tercero.PaginaWeb = string.Empty;

            if (string.IsNullOrEmpty(tercero.CodigoPais))
                throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "CodigoPais", tipo).Replace("de tipo", "del"));

            if (!ConfiguracionRegional.ValidarCodigoPais(tercero.CodigoPais))
                throw new ArgumentException(string.Format("No se encuentra registrado {0} con valor '{1}' según ISO 3166-1 alfa-2 en el {2} ", "CodigoPais", tercero.CodigoPais, tipo));

            if (string.IsNullOrEmpty(tercero.RazonSocial))
                throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "RazonSocial", tipo).Replace("de tipo", "del"));

            if ((tercero.TipoPersona < 1) || (tercero.TipoPersona > 2))
                throw new ArgumentException(string.Format("El parámetro {0} con valor '{1}' del {2} no esta bien formado", "TipoPersona", tercero.TipoPersona, tipo));

            if (tercero.TipoPersona == 2)
            {
                if (string.IsNullOrEmpty(tercero.PrimerApellido))
                    throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "PrimerApellido", tipo).Replace("de tipo", "del"));

                if (string.IsNullOrEmpty(tercero.PrimerNombre))
                    throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "PrimerNombre", tipo).Replace("de tipo", "del"));
            }

        }

        /// <summary>
        /// Valida los totales del objeto
        /// </summary>
        /// <param name="documento_fac">Documento Factura</param>
        /// <param name="documento_nc">Documento Nota Credito</param>
        /// <param name="documento_nd">Documento Nota Debito</param>
        /// <param name="tipo_doc">Tipo de Documento enviado</param>
        public static void ValidarTotales(Factura documento_fac, NotaCredito documento_nc, NotaDebito documento_nd, TipoDocumento tipo_doc)
        {

            var documento = (dynamic)null;

            if (tipo_doc == TipoDocumento.Factura)
                documento = documento_fac;
            else if (tipo_doc == TipoDocumento.NotaCredito)
                documento = documento_nc;
            else if (tipo_doc == TipoDocumento.NotaDebito)
                documento = documento_nd;

            if (documento != null)
            {

                ValidarDetalleDocumento(documento.DocumentoDetalles);

                //Regex isnumber = new Regex(@"^(0|([1-9][0-9]*))(\.\d\d$)$");

                //Valida el Iva 
                if (documento.ValorIva == 0)
                {
                    documento.ValorIva = Convert.ToDecimal(0.00M);
                }
                if (!Texto.ValidarExpresion(TipoExpresion.Decimal, Convert.ToString(documento.ValorIva).Replace(",", ".")))
                    //if (!isnumber.IsMatch(Convert.ToString(documento.ValorIva).Replace(",", ".")))
                    throw new ApplicationException(string.Format("El campo '{0}' con valor '{1}' del encabezado no está bien formado", "Iva", documento.ValorIva));

                //Valida el Descuento 
                if (documento.ValorDescuento == 0)
                {
                    documento.ValorDescuento = Convert.ToDecimal(0.00M);
                }

                if (!Texto.ValidarExpresion(TipoExpresion.Decimal, Convert.ToString(documento.ValorDescuento).Replace(",", ".")))
                    throw new ApplicationException(string.Format("El campo '{0}' con valor '{1}' del encabezado no está bien formado", "Descuento", documento.ValorDescuento));

                //Valida el Subtotal 
                if (documento.ValorSubtotal == 0)
                {
                    documento.ValorSubtotal = Convert.ToDecimal(0.00M);
                }
                if (!Texto.ValidarExpresion(TipoExpresion.Decimal, Convert.ToString(documento.ValorSubtotal).Replace(",", ".")))
                    throw new ApplicationException(string.Format("El campo '{0}' con valor '{1}' del encabezado no está bien formado", "Subtotal", documento.ValorSubtotal));

                //Valida el Impuesto al consumo 
                if (documento.ValorImpuestoConsumo == 0)
                {
                    documento.ValorImpuestoConsumo = Convert.ToDecimal(0.00M);
                }
                if (!Texto.ValidarExpresion(TipoExpresion.Decimal, Convert.ToString(documento.ValorImpuestoConsumo).Replace(",", ".")))
                    throw new ApplicationException(string.Format("El campo '{0}' con valor '{1}' del encabezado no está bien formado", "Impuesto al Consumo", documento.ValorImpuestoConsumo));

                //Valida la Retencion en la fuente
                if (documento.ValorReteFuente == 0)
                {
                    documento.ValorReteFuente = Convert.ToDecimal(0.00M);
                }
                if (!Texto.ValidarExpresion(TipoExpresion.Decimal, Convert.ToString(documento.ValorReteFuente).Replace(",", ".")))
                    throw new ApplicationException(string.Format("El campo '{0}' con valor '{1}' del encabezado no está bien formado", "ReteFuente", documento.ValorReteFuente));

                //Valida el ReteIca 
                if (documento.ValorReteIca == 0)
                {
                    documento.ValorReteIca = Convert.ToDecimal(0.00M);
                }
                if (!Texto.ValidarExpresion(TipoExpresion.Decimal, Convert.ToString(documento.ValorReteIca).Replace(",", ".")))
                    throw new ApplicationException(string.Format("El campo '{0}' con valor '{1}' del encabezado no está bien formado", "ReteIca", documento.ValorReteIca));

                //Calculo del total con los campos enviados en el objeto
                if (documento.Total == 0)
                {
                    documento.Total = Convert.ToDecimal(0.00M);
                }
                if (!Texto.ValidarExpresion(TipoExpresion.Decimal, Convert.ToString(documento.Total).Replace(",", ".")))
                    throw new ApplicationException(string.Format("El campo '{0}' con valor '{1}' del encabezado no está bien formado", "Total", documento.Total));

                //Validacion del Neto calculado con el que es enviado en el documento
                if (documento.Neto == 0)
                {
                    documento.Neto = Convert.ToDecimal(0.00M);
                }
                if (!Texto.ValidarExpresion(TipoExpresion.Decimal, Convert.ToString(documento.Neto).Replace(",", ".")))
                    throw new ApplicationException(string.Format("El campo '{0}' con valor '{1}' del encabezado no está bien formado", "Valor Neto", documento.Neto));

                //Validacion del ReteIva calculado con el que es enviado en el documento
                if (documento.ValorReteIva == 0)
                {
                    documento.ValorReteIva = Convert.ToDecimal(0.00M);
                }

                if (!Texto.ValidarExpresion(TipoExpresion.Decimal, Convert.ToString(documento.ValorReteIva).Replace(",", ".")))
                    throw new ApplicationException(string.Format("El campo '{0}' con valor '{1}' del encabezado no está bien formado", "ReteIva", documento.ValorReteIva));

            }
        }

        /// <summary>
        /// Valida los totales enviados en el detalle
        /// </summary>
        /// <param name="documentoDetalle"></param>
        /// <returns></returns>
        public static DocumentoDetalle ValidarDetalleDocumento(List<DocumentoDetalle> documentoDetalle)
        {

            DocumentoDetalle retorno = new DocumentoDetalle();

            decimal Iva_total = 0;
            decimal Desc_total = 0;
            decimal Subtotal = 0;
            decimal Impcon = 0;
            decimal RetIca = 0;
            decimal ReteFte = 0;

            if (documentoDetalle == null || !documentoDetalle.Any())
                throw new Exception("El detalle del documento es inválido.");

            foreach (DocumentoDetalle Docdet in documentoDetalle)
            {

                //Validacion del valor unitario
                //Regex isnumber = new Regex(@"^(0|([1-9][0-9]*))(\.\d\d$)$");


                if (string.IsNullOrEmpty(Docdet.Bodega))
                    Docdet.Bodega = string.Empty;

                if (Docdet.ValorUnitario == 0)
                {
                    Docdet.ValorUnitario = 0.00M;
                }
                if (!Texto.ValidarExpresion(TipoExpresion.Decimal, Convert.ToString(Docdet.ValorUnitario).Replace(",", ".")))
                    throw new ApplicationException(string.Format("El campo '{0}' con valor '{1}' del encabezado no está bien formado", "Valor Unitario", Docdet.ValorUnitario));

                if (Docdet.DescuentoPorcentaje < 0 || Docdet.DescuentoPorcentaje > 100)
                    throw new ApplicationException(string.Format("El campo '{0}' con valor '{1}' del encabezado no está bien formado", "Porcentaje Descuento", Docdet.DescuentoPorcentaje));

                if (Docdet.DescuentoValor == 0)
                {
                    Docdet.DescuentoValor = Convert.ToDecimal(0.00M);
                }

                //Validacion del valor IVA
                if (Docdet.IvaValor == 0)
                {
                    Docdet.IvaValor = Convert.ToDecimal(0.00M);
                }
                if (!Texto.ValidarExpresion(TipoExpresion.Decimal, Convert.ToString(Docdet.IvaValor).Replace(",", ".")))
                    throw new ApplicationException(string.Format("El campo '{0}' con valor '{1}' del encabezado no está bien formado", "Iva", Docdet.IvaValor));

                //Validacion del Valor Subtotal
                if (Docdet.ValorSubtotal == 0)
                {
                    Docdet.ValorSubtotal = Convert.ToDecimal(0.00M);
                }

                if (!Texto.ValidarExpresion(TipoExpresion.Decimal, Convert.ToString(Docdet.ValorSubtotal).Replace(",", ".")))
                    throw new ApplicationException(string.Format("El campo '{0}' con valor '{1}' del encabezado no está bien formado", "Subtotal", Docdet.ValorSubtotal));

                //Validacion del Valor del Impuesto al Consumo
                if (Docdet.ValorImpuestoConsumo == 0)
                {
                    Docdet.ImpoConsumoPorcentaje = 0.00M;
                    Docdet.ValorImpuestoConsumo = 0.00M;
                }
                if (!Texto.ValidarExpresion(TipoExpresion.Decimal, Convert.ToString(Docdet.ValorImpuestoConsumo).Replace(",", ".")))
                    throw new ApplicationException(string.Format("El campo '{0}' con valor '{1}' del encabezado no está bien formado", "Impuesto al Consumo", Docdet.ValorImpuestoConsumo));


                //Validacion del Valor del ReteICA
                if (Docdet.ReteIcaValor == 0)
                {
                    Docdet.ReteIcaValor = 0.00M;
                }
                if (!Texto.ValidarExpresion(TipoExpresion.Decimal, Convert.ToString(Docdet.ReteIcaValor).Replace(",", ".")))
                    throw new ApplicationException(string.Format("El campo '{0}' con valor '{1}' del encabezado no está bien formado", "ReteIca", Docdet.ReteIcaValor));

                //Validacion del Valor del ReteFte
                if (Docdet.ReteFuenteValor == 0)
                {
                    Docdet.ReteFuenteValor = 0.00M;
                }
                if (!Texto.ValidarExpresion(TipoExpresion.Decimal, Convert.ToString(Docdet.ReteFuenteValor).Replace(",", ".")))
                    throw new ApplicationException(string.Format("El campo '{0}' con valor '{1}' del encabezado no está bien formado", "ReteFuente", Docdet.ReteFuenteValor));

                Iva_total += Docdet.IvaValor;
                Desc_total += Docdet.DescuentoValor;
                Subtotal += Docdet.ValorSubtotal;
                Impcon += Docdet.ValorImpuestoConsumo;
                RetIca += Docdet.ReteIcaValor;
                ReteFte += Docdet.ReteFuenteValor;

            }

            retorno.IvaValor = Iva_total;
            retorno.DescuentoValor = Desc_total;
            retorno.ValorSubtotal = Subtotal;
            retorno.ValorImpuestoConsumo = Impcon;
            retorno.ReteIcaValor = RetIca;
            retorno.ReteFuenteValor = ReteFte;
            return retorno;
        }

        /// <summary>
        /// Procesa una lista de documentos tipo Documento Archivo
        /// </summary>
        /// <param name="documentos">documentos Documento Archivo</param>
        /// <returns>objeto tipo Documento Respuesta</returns>
        public static List<DocumentoRespuesta> Procesar(List<DocumentoArchivo> documentos)
        {

            if (documentos == null)
                throw new ApplicationException("No se encontraron datos");
            if (documentos.FirstOrDefault() == null)
                throw new ApplicationException("No se encontraron datos en el primer registro");

            Ctl_Empresa Peticion = new Ctl_Empresa();

            //Válida que la key sea correcta.
            TblEmpresas facturador_electronico = Peticion.Validar(documentos.FirstOrDefault().DataKey, documentos.FirstOrDefault().Identificacion);

            if (!facturador_electronico.IntObligado)
                throw new ApplicationException(string.Format("Licencia inválida para la identificación '{0}'.", facturador_electronico.StrIdentificacion));

            // genera un id único de la plataforma
            Guid id_peticion = Guid.NewGuid();

            DateTime fecha_actual = Fecha.GetFecha();
            List<TblEmpresasResoluciones> lista_resolucion = new List<TblEmpresasResoluciones>();

            // sobre escribe los datos del facturador electrónico si se encuentra en estado de habilitación
            if (facturador_electronico.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
            {

                //Valida que Resolucion tomar, con Prefijo o sin Prefijo
                string resolucion_pruebas = string.Empty;
                string nit_resolucion = string.Empty;
                string prefijo_pruebas = string.Empty;
                if (documentos.FirstOrDefault().Prefijo.Equals(string.Empty))
                {
                    resolucion_pruebas = Constantes.ResolucionPruebas;
                    nit_resolucion = Constantes.NitResolucionsinPrefijo;

                }
                else
                {
                    resolucion_pruebas = Constantes.ResolucionPruebas;
                    nit_resolucion = Constantes.NitResolucionconPrefijo;
                    prefijo_pruebas = Constantes.PrefijoResolucionPruebas;
                }



                Ctl_EmpresaResolucion _resolucion = new Ctl_EmpresaResolucion();
                lista_resolucion.Add(_resolucion.Obtener(nit_resolucion, resolucion_pruebas, prefijo_pruebas));

            }
            else
            {
                // actualiza las resoluciones de los servicios web de la DIAN en la base de datos
                lista_resolucion = Ctl_Resoluciones.Actualizar(id_peticion, documentos.FirstOrDefault().Identificacion);
            }


            if (lista_resolucion == null)
                throw new ApplicationException(string.Format("No se encontraron resoluciones para el Facturador Electrónico '{0}'", facturador_electronico.StrIdentificacion));
            else if (!lista_resolucion.Any())
                throw new ApplicationException(string.Format("No se encontraron resoluciones para el Facturador Electrónico '{0}'", facturador_electronico.StrIdentificacion));


            List<DocumentoRespuesta> respuesta = new List<DocumentoRespuesta>();

            foreach (DocumentoArchivo objeto in documentos)
            {


                DocumentoRespuesta item_respuesta = new DocumentoRespuesta();

                try
                {

                    if (string.IsNullOrEmpty(objeto.NumeroResolucion))
                        throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "NumeroResolucion", "string"));


                    Ctl_Documento num_doc = new Ctl_Documento();

                    //valida si el Documento ya existe en Base de Datos
                    TblDocumentos numero_documento = num_doc.Obtener(objeto.NumeroResolucion, objeto.Documento, objeto.TipoDocumento, objeto.Prefijo);

                    if (numero_documento != null)
                        throw new ApplicationException(string.Format("El documento número '{0}' con prefijo '{1}' ya xiste para el Facturador Electrónico {2}", objeto.Documento, objeto.Prefijo, facturador_electronico.StrIdentificacion));

                    TblEmpresasResoluciones resolucion = null;

                    try
                    {
                        ApplicationException exTMP = new ApplicationException(string.Format("DataRes: {0}", lista_resolucion.FirstOrDefault().StrIdSeguridad));

                        LogExcepcion.Guardar(exTMP);

                        // filtra la resolución del documento
                        resolucion = lista_resolucion.Where(_resolucion => _resolucion.StrNumResolucion.Equals(objeto.NumeroResolucion)).FirstOrDefault();
                    }
                    catch (Exception excepcion)
                    {
                        throw new ApplicationException(string.Format("No se encontró la resolución {0} para el Facturador Electrónico {1}", objeto.NumeroResolucion, facturador_electronico.StrIdentificacion));
                    }


                    // procesa el documento
                    item_respuesta = Procesar(id_peticion, objeto, facturador_electronico, resolucion);
                }
                catch (Exception excepcion)
                {

                    ProcesoEstado proceso_actual = ProcesoEstado.Recepcion;
                    LogExcepcion.Guardar(excepcion);
                    item_respuesta = new DocumentoRespuesta()
                    {
                        Aceptacion = 0,
                        CodigoRegistro = objeto.CodigoRegistro,
                        Cufe = "",
                        DescripcionProceso = Enumeracion.GetDescription(proceso_actual),
                        DocumentoTipo = objeto.TipoDocumento,
                        Documento = objeto.Documento,
                        Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error al procesar el documento. Detalle: {0}", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.ERROR_NO_CONTROLADO, excepcion.InnerException),
                        EstadoDian = null,
                        FechaRecepcion = fecha_actual,
                        FechaUltimoProceso = fecha_actual,
                        IdDocumento = "",
                        Identificacion = "",
                        IdProceso = proceso_actual.GetHashCode(),
                        MotivoRechazo = "",
                        NumeroResolucion = objeto.NumeroResolucion,
                        Prefijo = objeto.Prefijo,
                        ProcesoFinalizado = (proceso_actual == ProcesoEstado.Finalizacion || proceso_actual == ProcesoEstado.FinalizacionErrorDian) ? (1) : 0,
                        UrlPdf = "",
                        UrlXmlUbl = ""
                    };

                }
                respuesta.Add(item_respuesta);
            }

            return respuesta;
        }

    }
}




